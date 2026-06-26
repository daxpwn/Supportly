import { Component, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { LoginService } from '../../services/login.service';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-signup',
  imports: [FormsModule, RouterLink],
  templateUrl: './signup.html',
})
export class SignupComponent {
  email = '';
  password = '';
  confirmPassword = '';
  error = signal('');

  private readonly loginService = inject(LoginService);
  private readonly auth = inject(AuthService);
  private readonly router = inject(Router);

  onSubmit() {
    this.error.set('');

    if (
      this.email.trim() === '' ||
      this.password.trim() === '' ||
      this.confirmPassword.trim() === ''
    ) {
      this.error.set('All fields are required');
      return;
    }

    const emailPattern = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    if (!emailPattern.test(this.email.trim())) {
      this.error.set('Please enter a valid email address');
      return;
    }

    if (this.password.length < 6) {
      this.error.set('Password must be at least 6 characters long');
      return;
    }

    if (this.password !== this.confirmPassword) {
      this.error.set('Passwords do not match');
      return;
    }

    this.loginService
      .register({ email: this.email, password: this.password })
      .subscribe({
        next: (res) => {
          this.auth.setToken(res.token);
          this.router.navigate(['/dashboard']);
        },
        error: (err) => {
          this.error.set('Registration failed. Please try again later.');
          console.error(err);
        },
      });
  }
}
