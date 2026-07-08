# ---- Build faza ----
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Prvo samo .csproj fajlovi radi keširanja restore-a
COPY Supportly.sln ./
COPY Application/Application.csproj Application/
COPY Domain/Domain.csproj Domain/
COPY Implementation/Implementation.csproj Implementation/
COPY Supportly.DataAccess/Supportly.DataAccess.csproj Supportly.DataAccess/
COPY Supportly.API/Supportly.API.csproj Supportly.API/
COPY Tests/Tests.csproj Tests/

RUN dotnet restore Supportly.API/Supportly.API.csproj

# Zatim ostatak koda i publish
COPY . .
RUN dotnet publish Supportly.API/Supportly.API.csproj -c Release -o /app/publish

# ---- Runtime faza ----
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish ./

# Folder za upload attachment-a (mount-uje se volume)
RUN mkdir -p /app/wwwroot/Uploads

ENV ASPNETCORE_URLS=http://+:5064
EXPOSE 5064
ENTRYPOINT ["dotnet", "Supportly.API.dll"]
