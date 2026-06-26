import { Routes } from '@angular/router';
import { authGuard } from './guards/auth.guard';
import { HomeComponent } from './components/home/home';
import { AuthorComponent } from './components/author/author';
import { LoginComponent } from './components/login/login';
import { SignupComponent } from './components/signup/signup';
import { DashboardComponent } from './components/dashboard/dashboard';
import { Tickets } from './components/tickets/tickets';
import { TicketDetailComponent } from './components/ticket-detail/ticket-detail';
import { ProfileComponent } from './components/profile/profile';

export const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'author', component: AuthorComponent },
  { path: 'login', component: LoginComponent },
  { path: 'signup', component: SignupComponent },
  { path: 'dashboard', component: DashboardComponent, canActivate: [authGuard] },
  { path: 'tickets', component: Tickets, canActivate: [authGuard] },
  { path: 'tickets/:id', component: TicketDetailComponent, canActivate: [authGuard] },
  { path: 'profile', component: ProfileComponent, canActivate: [authGuard] }
];