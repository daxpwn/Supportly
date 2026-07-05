import { Routes } from '@angular/router';
import { authGuard } from './guards/auth.guard';
import { HomeComponent } from './components/home/home';
import { AuthorComponent } from './components/author/author';
import { LoginComponent } from './components/login/login';
import { SignupComponent } from './components/signup/signup';
import { DashboardComponent } from './components/dashboard/dashboard';
import { Tickets } from './components/tickets/tickets';
import { TicketDetailComponent } from './components/ticket-detail/ticket-detail';
import { ApplyTicketComponent } from './components/apply-ticket/apply-ticket';
import { MyTicketsComponent } from './components/my-tickets/my-tickets';
import { ProfileComponent } from './components/profile/profile';
import { UsersComponent } from './components/users/users';
import { UserEditComponent } from './components/user-edit/user-edit';
import { roleGuard } from './guards/role.guard';

export const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'author', component: AuthorComponent },
  { path: 'login', component: LoginComponent },
  { path: 'signup', component: SignupComponent },
  { path: 'dashboard', component: DashboardComponent, canActivate: [authGuard, roleGuard('admin')] },
  { path: 'my-tickets', component: MyTicketsComponent, canActivate: [authGuard, roleGuard('customer')] },
  { path: 'tickets', component: Tickets, canActivate: [authGuard, roleGuard('admin')] },
  { path: 'tickets/new', component: ApplyTicketComponent, canActivate: [authGuard, roleGuard('customer')] },
  { path: 'tickets/:id', component: TicketDetailComponent, canActivate: [authGuard, roleGuard('admin', 'customer')] },
  { path: 'profile', component: ProfileComponent, canActivate: [authGuard, roleGuard('admin')] },
  { path: 'users', component: UsersComponent, canActivate: [authGuard, roleGuard('admin')] },
  { path: 'users/:id', component: UserEditComponent, canActivate: [authGuard, roleGuard('admin')] }
];