import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './components/auth/login/login.component';
import { loggedInGuard } from './guards/logged-in.guard';
import { SignupComponent } from './components/auth/signup/signup.component';
import { ConfirmEmailComponent } from './components/auth/confirm-email/confirm-email.component';
import { AvailableRoomsListComponent } from './components/rooms/available-rooms-list/available-rooms-list.component';
import { MyRoomsListComponent } from './components/rooms/my-rooms-list/my-rooms-list.component';
import { CreateRoomComponent } from './components/rooms/create-room/create-room.component';
import { EnterRoomConfirmationCodeComponent } from './components/rooms/enter-room-confirmation-code/enter-room-confirmation-code.component';
import { notLoggedInGuard } from './guards/not-logged-in.guard';
import { RoomComponent } from './components/rooms/room/room.component';
import { AddParticipantComponent } from './components/rooms/add-participant/add-participant.component';

const routes: Routes = [
  // { path: '', redirectTo: '/', pathMatch: 'full' },
  { path: '', redirectTo: '/rooms/available', pathMatch: 'full' },
  { path: 'login', component: LoginComponent, canActivate: [notLoggedInGuard] },
  {
    path: 'signup',
    component: SignupComponent,
    canActivate: [notLoggedInGuard],
  },
  {
    path: 'confirm',
    component: ConfirmEmailComponent,
    canActivate: [notLoggedInGuard],
  },
  {
    path: 'rooms/available',
    component: AvailableRoomsListComponent,
    canActivate: [loggedInGuard],
  },
  {
    path: 'rooms/my',
    component: MyRoomsListComponent,
    canActivate: [loggedInGuard],
  },
  {
    path: 'rooms/create',
    component: CreateRoomComponent,
    canActivate: [loggedInGuard],
  },
  {
    path: 'rooms/code/:id',
    component: EnterRoomConfirmationCodeComponent,
    canActivate: [loggedInGuard],
  },
  {
    path: 'rooms/:id',
    component: RoomComponent,
    canActivate: [loggedInGuard],
  },
  {
    path: 'rooms/add-participant/:id',
    component: AddParticipantComponent,
    canActivate: [loggedInGuard],
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
