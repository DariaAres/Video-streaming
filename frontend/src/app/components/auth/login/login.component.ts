import { Router } from '@angular/router';
import { Component, OnInit } from '@angular/core';
import { AuthService } from 'src/app/services/auth.service';
import { TokenStorageService } from 'src/app/services/token-storage.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
})
export class LoginComponent implements OnInit {
  form: any = {
    username: null,
    password: null,
  };
  errorData: any = {
    username: '',
    password: '',
  };
  isLoggedIn = false;
  isLoginFailed = false;
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
      this.router.navigate(['accounts']);
      // this.roles = this.tokenStorage.getUser().roles;
    }
  }

  onSubmit(): void {
    this.clearErrorDescription();
    const { username, password } = this.form;
    this.authService.login(username, password).subscribe({
      next: (data) => {
        this.tokenStorage.saveToken(data.token);
        this.tokenStorage.saveUser(data);

        this.isLoginFailed = false;
        this.isLoggedIn = true;
        this.reloadPage();
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
        this.isLoginFailed = true;
      },
    });
  }

  reloadPage(): void {
    window.location.reload();
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
