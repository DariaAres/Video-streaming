import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { LoginComponent } from './components/auth/login/login.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { SignupComponent } from './components/auth/signup/signup.component';
import { ConfirmEmailComponent } from './components/auth/confirm-email/confirm-email.component';
import { AvailableRoomsListComponent } from './components/rooms/available-rooms-list/available-rooms-list.component';
import { authInterceptorProviders } from './interceptors/auth.interceptor';
import { corsInterceptorProviders } from './interceptors/cors.interceptor';
import { errorTitleInterceptorProviders } from './interceptors/error-title.interceptor';
import { MyRoomsListComponent } from './components/rooms/my-rooms-list/my-rooms-list.component';
import { CreateRoomComponent } from './components/rooms/create-room/create-room.component';
import { EnterRoomConfirmationCodeComponent } from './components/rooms/enter-room-confirmation-code/enter-room-confirmation-code.component';
import { RoomComponent } from './components/rooms/room/room.component';
import { AddParticipantComponent } from './components/rooms/add-participant/add-participant.component';

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    SignupComponent,
    ConfirmEmailComponent,
    AvailableRoomsListComponent,
    MyRoomsListComponent,
    CreateRoomComponent,
    EnterRoomConfirmationCodeComponent,
    RoomComponent,
    AddParticipantComponent,
  ],
  imports: [
    BrowserModule,
    CommonModule,
    AppRoutingModule,
    FormsModule,
    HttpClientModule,
    ReactiveFormsModule,
  ],
  providers: [
    authInterceptorProviders,
    corsInterceptorProviders,
    errorTitleInterceptorProviders,
  ],
  bootstrap: [AppComponent],
})
export class AppModule {}
