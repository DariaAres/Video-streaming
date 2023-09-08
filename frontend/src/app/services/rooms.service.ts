import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { AvailableRoom } from '../models/available-room';
import { AppSettings } from '../appsettings';
import { FullRoom } from '../models/full-room';

@Injectable({
  providedIn: 'root',
})
export class RoomsService {
  controllerUrl = `${AppSettings.API_ENDPOINT}/rooms`;

  constructor(private httpClient: HttpClient) {}

  public getAvailable(): Observable<AvailableRoom[]> {
    return this.httpClient.get<AvailableRoom[]>(
      `${this.controllerUrl}/available`
    );
  }

  public getMy(): Observable<FullRoom[]> {
    return this.httpClient.get<FullRoom[]>(`${this.controllerUrl}/my`);
  }

  public getAvailableRoomInfo(roomId: number): Observable<AvailableRoom> {
    return this.httpClient.get<AvailableRoom>(
      `${this.controllerUrl}/available/${roomId}`
    );
  }

  public getRoomInfo(roomId: number): Observable<FullRoom> {
    return this.httpClient.get<FullRoom>(
      `${this.controllerUrl}/full/${roomId}`
    );
  }

  public checkRoomCode(roomId: number, code: string): Observable<any> {
    return this.httpClient.get(
      `${this.controllerUrl}/checkCode/${roomId}/${code}`
    );
  }

  public create(
    title: string,
    fileId: string
    // participantEmails: string[]
  ): Observable<any> {
    return this.httpClient.post(this.controllerUrl, {
      title,
      fileId,
      // participantEmails,
    });
  }

  public delete(roomId: number): Observable<any> {
    return this.httpClient.delete(`${this.controllerUrl}/${roomId}`);
  }

  public addParticipant(
    roomId: number,
    participantUserName: string
  ): Observable<any> {
    return this.httpClient.post(`${this.controllerUrl}/participant`, {
      roomId,
      participantUserName,
    });
  }

  public deleteParticipant(
    roomId: number,
    participantUserName: string
  ): Observable<any> {
    return this.httpClient.delete(
      `${this.controllerUrl}/participant/${roomId}/${participantUserName}`
    );
  }

  public startVideo(roomId: number): Observable<any> {
    return this.httpClient.post(
      `${this.controllerUrl}/startVideo/${roomId}`,
      {}
    );
  }

  public stopVideo(roomId: number): Observable<any> {
    return this.httpClient.post(
      `${this.controllerUrl}/stopVideo/${roomId}`,
      {}
    );
  }
}
