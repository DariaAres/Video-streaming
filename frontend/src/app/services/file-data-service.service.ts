import { Injectable } from '@angular/core';
import { AppSettings } from '../appsettings';
import { FileData } from '../models/file-data';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class FileDataServiceService {
  controllerUrl = `${AppSettings.API_ENDPOINT}/files`;

  constructor(private httpClient: HttpClient) {}

  public search(name = ''): Observable<FileData[]> {
    return this.httpClient.get<FileData[]>(`${this.controllerUrl}/${name}`);
  }
}
