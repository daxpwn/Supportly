# CLAUDE.md

Smernice za rad u ovom repozitorijumu. Komunikacija sa korisnikom je na srpskom.

## Šta je ovo

**Supportly** — helpdesk aplikacija u izradi (ASP.NET Core Web API, .NET 10).

> **Status:** pozorišni/repertoar domen iz originalnog `ASPLAB2` template-a je **uklonjen**.
> Osnova: Clean Architecture + CQRS-lite use case obrazac + auth (registracija, JWT + refresh token).
> Helpdesk **entiteti** (Ticket, User, Role, Department, Priority, Status, Category, Tag, ...) su
> dodati u `Domain/` (code-first), registrovani kao DbSet-ovi u `LabDbContext`, i imaju pune EF Fluent
> konfiguracije (`Configurations/*Configuration.cs`) verne originalnom SQL-u (tipovi, indeksi, FK
> pravila, check constraint, seed šifarnika). `User` je evoluiran na helpdesk šemu (login preko
> **Email**-a, `PasswordHash`, `RoleId`), auth kod usklađen. `Initial` migracija primenjena na bazu.
> **Napomena:** trigger `trg_Tickets_UpdatedAt` i view `vOpenTickets` iz SQL-a NISU u modelu —
> dodati ih raw SQL-om (`migrationBuilder.Sql(...)`) ako se žele u bazi.
>
> Projekti i namespace-ovi su preimenovani iz `ASPLAB2.*` u `Supportly.*` (`Supportly.API`,
> `Supportly.DataAccess`, solution `Supportly.sln`), baza je `Supportly` (LocalDB). Iz template-a je
> ostalo još samo ime klase `LabDbContext` (može se preimenovati kasnije).

Nije git repozitorijum (još).

## Build / Run / Test

```bash
dotnet build Supportly.sln                 # build celog solution-a
dotnet run --project Supportly.API             # pokretanje API-ja (http://localhost:5064, Development)
dotnet test                                   # pokretanje testova (Tests projekat, xUnit)
dotnet ef migrations add <Naziv> --project Supportly.DataAccess --startup-project Supportly.API
dotnet ef database update --project Supportly.DataAccess --startup-project Supportly.API
```

- Baza: **SQL Server (LocalDB)** — `Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Supportly`, integrated security. Connection string u `Supportly.API/appsettings.{Development,Test,Production}.json` i u default konstruktoru `LabDbContext` (koji koriste testovi/EF design-time).
- Migracije: `Initial` (ceo model — auth + helpdesk) + `RoleUseCases` (dozvole po roli umesto po korisniku). Za promene modela: `migrations add <Naziv>` pa `database update`.
- EF alat: `dotnet-ef` global tool (instaliran). `Microsoft.EntityFrameworkCore.Design` referenciran u API projektu zbog tooling-a.
- Testovi gađaju **realnu lokalnu bazu** (`new LabDbContext()` bez DI), nisu izolovani in-memory.

## Arhitektura — slojevi (Clean / Onion)

Zavisnosti idu ka unutra: `API → Implementation → Application → Domain`. `DataAccess` zavisi od `Domain`.

| Projekat | Uloga |
|---|---|
| **Domain** | POCO entiteti, EF lazy-loading preko `virtual` navigacija. Auth: `User`, `RoleUseCase`, `AuthToken`. Helpdesk: `Role`, `Department`, `Priority`, `Status`, `Category`, `Ticket`, `TicketComment`, `Attachment`, `Tag`, `TicketTag`, `TicketHistory`, `CannedResponse`. Ključevi prate SQL tipove (`byte`=TINYINT, `short`=SMALLINT, `int`=INT/`BaseEntity`, `long`=BIGINT). |
| **Application** | Apstrakcije: `ICommand<TRequest>`, `IQuery<TParam,TResponse>`, `IUseCase`, `IApplicationUser`, DTO-ovi (`RegisterUserDTO`, `PagedResponse<T>`), interfejsi use case-ova (`IRegisterUserCommand`). Bez implementacije. |
| **Implementation** | EF implementacije use case-ova (`Ef*Command`/`Ef*Query`, nasleđuju `EfUseCase`), FluentValidation validatori, `UseCaseHandler`, `UnauthorizedUser`. |
| **Supportly.DataAccess** | `LabDbContext`, EF `IEntityTypeConfiguration` po entitetu (`Configurations/`: `UserConfiguration`, `AuthTokenConfiguration`), migracije. |
| **Supportly.API** | Kontroleri (`AuthController`, `RegisterController`), DI setup (`ApiExtensions.SetupApplication`), JWT, middleware, `AppSettings`. |
| **Tests** | xUnit + FluentAssertions. Trenutno samo `JwtTests` (auth/refresh flow). |

## Ključni obrasci (CQRS-lite use case)

- **Use case = jedan Command ili Query.** Svaki ima `Name` (opis) i `Id` (stabilan string ključ, npr. `"register"`).
- **`UseCaseHandler`** (`Implementation/UseCases/UseCaseHandler.cs`) je centralna tačka izvršavanja: radi **autorizaciju** (proverava da li `IApplicationUser.AllowedUseCases` sadrži `useCase.Id`, inače baca `UnauthorizedUseCaseException`) + meri vreme. Kontroleri pozivaju `handler.ExecuteCommand(cmd, dto)` / `handler.ExecuteQuery(query, search)` (vidi `RegisterController`).
- **Identitet korisnika**: `IApplicationUser` se rešava u DI (`ApiExtensions.cs`) iz `Authorization: Bearer` headera (parsira JWT claims → `JwtUser`); ako nema validnog tokena → `UnauthorizedUser` (gost; trenutno dozvoljen samo `register`).
- **Validacija**: FluentValidation. Validatori bacaju preko `ValidateAndThrow`; `GlobalExceptionHandlingMiddleware` mapira `ValidationException → 422` sa listom grešaka.
- **EF use case bazni tip**: `EfUseCase` drži `protected LabDbContext ctx`.

## Auth

- **JWT + refresh token**, oba čuvana kao `AuthToken` redovi u bazi (`BaseTokenId == null` → JWT, inače refresh). `JwtHandler.MakeToken` pravi par i upisuje ih.
- Pri svakom requestu (`Program.cs`, `OnTokenValidated`) proverava se da `AuthToken` postoji i nije invalidiran (`InvalidatedAt`) — omogućava server-side logout/invalidaciju.
- `AuthController`: `Login`, `Logout` (invalidira JWT + refresh), `Refresh` (rotacija tokena). Lozinke: **BCrypt**.
- ⚠️ `AuthController` ima dva `[HttpPost]` bez rute (`Login` i `Refresh`) — kolizija ruta, verovatno bug; treba dodeliti eksplicitne rute.
- **API key** middleware (`ApiKeyAuthorizationMiddleware`) štiti samo endpointe označene `[ApiKeyAuthorization]` atributom; ključevi u `appsettings`. (Trenutno nijedan endpoint ne koristi atribut.)

## Cross-cutting

- **Globalni exception handling**: `GlobalExceptionHandlingMiddleware` (422 validacija, 401 neautorizovan use case, 500 + Sentry log za ostalo).
- **Logging grešaka**: `IExceptionLogger` → `SentryExceptionLogger` (Sentry DSN trenutno hardkodiran u `Program.cs` — kandidat za premeštanje u config).
- **Pipeline redosled** (`Program.cs`): GlobalException → ApiKey → Authorization → MapControllers.

## Konvencije

- Sve nove use case implementacije: interfejs u `Application/` (Commands/ ili Queries/), EF implementacija u `Implementation/UseCases/`, registracija u `ApiExtensions.SetupApplication`, izloženo kroz kontroler preko `UseCaseHandler`.
- Komentari u postojećem kodu su mešavina srpskog/engleskog — prati stil fajla koji menjaš.

## Kada se gradi helpdesk domen

Pri izgradnji helpdesk domena: novi entiteti (npr. `Ticket`, `Agent`, `Customer`, `Comment`) u `Domain/`, EF konfiguracije + migracija u `DataAccess/`, use case-ovi po CQRS obrascu gore, dozvole kroz `RoleUseCases` (po roli; šablon u `Domain/Authorization/RoleUseCaseTemplate`) → JWT `UseCase` claims → `IApplicationUser.AllowedUseCases` (npr. `create-ticket`, `assign-ticket`, `close-ticket`).
