import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AvailableRoom } from 'src/app/models/available-room';
import { RoomsService } from 'src/app/services/rooms.service';

@Component({
  selector: 'app-enter-room-confirmation-code',
  templateUrl: './enter-room-confirmation-code.component.html',
  styleUrls: ['./enter-room-confirmation-code.component.css'],
})
export class EnterRoomConfirmationCodeComponent implements OnInit {
  form: any = {
    code: '',
  };

  errorData: any = {
    code: '',
  };

  roomId: number = -1;
  room: AvailableRoom | null = null;

  constructor(
    private roomsService: RoomsService,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.roomId = Number(this.route.snapshot.paramMap.get('id'));
    this.roomsService
      .getAvailableRoomInfo(this.roomId)
      .subscribe((r) => (this.room = r));
  }

  onSubmit(): void {
    this.clearErrorDescription();

    if (this.form.code.length === 0) {
      alert('Введите код');
      return;
    }

    this.roomsService.checkRoomCode(this.roomId, this.form.code).subscribe({
      next: (data) => {
        this.router.navigate([`/rooms/${this.roomId}`]);
      },
      error: (err) => {
        alert(err.error.title);
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
