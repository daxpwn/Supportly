import { Component, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { LoginService } from '../../services/login.service';

@Component({
  selector: 'app-signup',
  imports: [FormsModule, RouterLink],
  templateUrl: './signup.html',
})
export class SignupComponent {
  fullName = '';
  email = '';
  phone = '';
  password = '';
  confirmPassword = '';
  error = signal('');

  private readonly loginService = inject(LoginService);
  private readonly router = inject(Router);
  private readonly toastr = inject(ToastrService);

  onSubmit() {
    this.error.set('');

    if (
      this.fullName.trim() === '' ||
      this.email.trim() === '' ||
      this.password.trim() === '' ||
      this.confirmPassword.trim() === ''
    ) {
      this.error.set('All fields are required');
      return;
    }

    if (this.fullName.trim().length < 3) {
      this.error.set('Full name must be at least 3 characters long');
      return;
    }

    const emailPattern = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    if (!emailPattern.test(this.email.trim())) {
      this.error.set('Please enter a valid email address');
      return;
    }

    // Ista pravila kao backend validator: min 8, veliko + malo slovo, cifra, specijalni znak.
    const passwordPattern = /^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[\W_]).{8,}$/;
    if (!passwordPattern.test(this.password)) {
      this.error.set(
        'Password must be at least 8 characters and include an uppercase letter, a lowercase letter, a digit, and a special character.',
      );
      return;
    }

    if (this.password !== this.confirmPassword) {
      this.error.set('Passwords do not match');
      return;
    }

    this.loginService
      .register({
        fullName: this.fullName,
        email: this.email,
        password: this.password,
        phone: this.phone,
      })
      .subscribe({
        next: () => {
          // Backend ne vraća token — nalog je kreiran, korisnik se sad prijavljuje.
          this.toastr.success('Nalog je kreiran. Prijavite se.');
          this.router.navigate(['/login']);
        },
        error: (err) => {
          this.error.set('Registration failed. Please try again later.');
          console.error(err);
        },
      });
  }
}
