import { Injectable } from '@angular/core';
import { AppSettings } from '../appsettings';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AvailableUser } from '../models/available-user';

@Injectable({
  providedIn: 'root',
})
export class UsersService {
  controllerUrl = `${AppSettings.API_ENDPOINT}/users`;

  constructor(private httpClient: HttpClient) {}

  public getAvailable(roomId: number): Observable<AvailableUser[]> {
    return this.httpClient.get<AvailableUser[]>(
      `${this.controllerUrl}/exceptParticipants/${roomId}`
    );
  }
}
