import { Injectable } from '@angular/core';
import { AppSettings } from 'src/app/appsettings';

@Injectable({
  providedIn: 'root',
})
export class AzureStorageService {
  constructor() {
  }

  getVideoUrl(fileName: string): string {
    const sasToken = AppSettings.SAS_TOKEN_QUERY_FILTER;

    return `http://77.91.126.188:10000/devstoreaccount1/videostreaming/${fileName}?` + sasToken;
  }
}
