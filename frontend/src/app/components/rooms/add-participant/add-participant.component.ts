import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AvailableUser } from 'src/app/models/available-user';
import { RoomsService } from 'src/app/services/rooms.service';
import { UsersService } from 'src/app/services/users.service';

@Component({
  selector: 'app-add-participant',
  templateUrl: './add-participant.component.html',
  styleUrls: ['./add-participant.component.css'],
})
export class AddParticipantComponent implements OnInit {
  form: any = {
    participantUserName: '',
  };
  errorData: any = {
    participantUserName: '',
  };

  roomId: number;
  users: AvailableUser[];

  constructor(
    private route: ActivatedRoute,
    private userService: UsersService,
    private roomService: RoomsService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.roomId = Number(this.route.snapshot.paramMap.get('id'));
    this.userService
      .getAvailable(this.roomId)
      .subscribe((u) => (this.users = u));
  }

  onSubmit(): void {
    this.clearErrorDescription();

    this.roomService
      .addParticipant(this.roomId, this.form.participantUserName)
      .subscribe({
        next: (_) => {
          this.router.navigate([`/rooms/my`]);
        },
        error: (err) => {
          if (err.status >= 400 && err.status <= 500) {
            if (err.error.errors) {
              this.prepareErrorDescription(err.error.errors);
            } else {
              alert(err.error.title);
            }
          }
        },
      });
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
