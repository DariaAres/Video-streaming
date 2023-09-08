import { Injectable } from '@angular/core';
import { User } from '../models/user';

const TOKEN_KEY = 'auth-token';
const USER_KEY = 'auth-user';

@Injectable({
  providedIn: 'root',
})
export class TokenStorageService {
  constructor() {}

  signOut(): void {
    window.localStorage.clear();
  }

  public saveToken(token: string): void {
    window.localStorage.removeItem(TOKEN_KEY);
    window.localStorage.setItem(TOKEN_KEY, token);
  }

  public getToken(): string | null {
    return window.localStorage.getItem(TOKEN_KEY);
  }

  public saveUser(user: any): void {
    window.localStorage.removeItem(USER_KEY);
    window.localStorage.setItem(USER_KEY, JSON.stringify(user));
  }

  public getUser(): User | null {
    const user = window.localStorage.getItem(USER_KEY);

    if (user) {
      let parsed = JSON.parse(user);
      let result: User = parsed;
      result.username = parsed.userName; // sry

      return result;
    }

    return null;
  }
}
