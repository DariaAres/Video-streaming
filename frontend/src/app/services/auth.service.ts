import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { User } from '../models/user';
import { AppSettings } from '../appsettings';

const AUTH_API = `${AppSettings.API_ENDPOINT}/Auth/`;

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private httpOptions = {
    headers: new HttpHeaders(),
  };

  constructor(private http: HttpClient) {
    this.httpOptions.headers.set('Content-Type', 'application/json');
    this.httpOptions.headers.set('Access-Control-Allow-Origin', '*');
  }

  login(username: string, password: string): Observable<User> {
    return this.http.post<User>(
      AUTH_API + 'signIn',
      {
        username,
        password,
      },
      this.httpOptions
    );
  }

  register(
    name: string,
    surname: string,
    username: string,
    email: string,
    password: string
  ): Observable<any> {
    return this.http.post(
      AUTH_API + 'signUp',
      {
        name,
        surname,
        username,
        email,
        password,
      },
      this.httpOptions
    );
  }

  confirm(email: string, code: number): Observable<any> {
    return this.http.post(
      AUTH_API + 'confirm',
      {
        email,
        code,
      },
      this.httpOptions
    );
  }
}
