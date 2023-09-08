import { Router } from '@angular/router';
import { Component, OnInit } from '@angular/core';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-signup',
  templateUrl: './signup.component.html',
  styleUrls: ['./signup.component.css'],
})
export class SignupComponent implements OnInit {
  form: any = {
    name: null,
    surname: null,
    username: null,
    email: null,
    password: null,
  };
  errorData: any = {
    name: null,
    surname: null,
    username: null,
    email: null,
    password: null,
  };
  isSuccessful = false;
  isSignUpFailed = false;
  errorMessage = '';

  constructor(private authService: AuthService, private router: Router) {}

  ngOnInit(): void {}

  onSubmit(): void {
    this.clearErrorDescription();
    
    this.authService
      .register(
        this.form.name,
        this.form.surname,
        this.form.username,
        this.form.email,
        this.form.password
      )
      .subscribe({
        next: (data) => {
          this.isSuccessful = true;
          this.isSignUpFailed = false;
          this.router.navigate(['/confirm']);
        },
        error: (err) => {
          if (err.status >= 400 && err.status <= 500) {
            if (err.error.errors) {
              this.prepareErrorDescription(err.error.errors);
            } else {
              alert(err.error.title);
            }
          }

          this.isSignUpFailed = true;
        },
      });
  }

  clearErrorDescription(): void {
    for (let key in this.errorData) {
      this.errorData[key] = '';
    }
  }

  prepareErrorDescription(errors: any): void {
    for (let key in this.errorData) {
      var errorsKey = key[0].toUpperCase() + key.slice(1);
      this.errorData[key] = errors[errorsKey];
    }
  }
}
