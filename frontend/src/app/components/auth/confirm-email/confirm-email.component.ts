import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/services/auth.service';
import { TokenStorageService } from 'src/app/services/token-storage.service';

@Component({
  selector: 'app-confirm-email',
  templateUrl: './confirm-email.component.html',
  styleUrls: ['./confirm-email.component.css'],
})
export class ConfirmEmailComponent implements OnInit {
  form: any = {
    email: null,
    code: null,
  };
  errorData: any = {
    email: '',
    code: '',
  };
  isLoggedIn = false;
  isConfirmFailed = false;
  errorMessage = '';
  // roles: string[] = [];

  constructor(
    private authService: AuthService,
    private tokenStorage: TokenStorageService,
    private router: Router
  ) {}

  ngOnInit(): void {
    if (this.tokenStorage.getToken()) {
      this.isLoggedIn = true;
      this.router.navigate(['/']);
      // this.roles = this.tokenStorage.getUser().roles;
    }
  }

  onSubmit(): void {
    this.clearErrorDescription();
    const { email, code } = this.form;
    this.authService.confirm(email, code).subscribe({
      next: () => {
        this.isConfirmFailed = false;
        this.isLoggedIn = true;

        this.router.navigate(['/login']);
      },
      error: (err) => {
        if (err.status >= 400 && err.status <= 500) {
          if (err.error.errors) {
            this.prepareErrorDescription(err.error.errors);
          } else {
            alert(err.error.title);
          }
        }

        // this.errorMessage = err.error.message;
        this.isConfirmFailed = true;
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
