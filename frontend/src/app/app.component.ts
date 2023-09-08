import { Component, OnChanges, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { TokenStorageService } from './services/token-storage.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent implements OnInit {
  title = 'Rovie';
  username?: string;

  constructor(
    private tokenStorageService: TokenStorageService,
    public router: Router
  ) {}

  ngOnInit(): void {
    const user = this.tokenStorageService.getUser();

    if (user) {
      // this.roles = user.roles;
      // this.showAdminBoard = this.roles.includes('ROLE_ADMIN');

      this.username = user.username;
    } else {
      let splitted = window.location.toString().split('/');
      let page = splitted[splitted.length - 1];

      if (page !== 'signup' && page !== 'login' && page !== 'confirm') {
        this.router.navigate(['/login']);
      }
    }
  }

  isAdmin(): boolean {
    let user = this.tokenStorageService.getUser();
    if (!user) {
      return false;
    }

    return user?.username === 'admin';
  }

  logout(): void {
    this.tokenStorageService.signOut();
    window.location.reload();
  }
}
