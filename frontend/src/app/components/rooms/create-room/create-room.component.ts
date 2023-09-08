import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { FileData } from 'src/app/models/file-data';
import { FileDataServiceService } from 'src/app/services/file-data-service.service';
import { RoomsService } from 'src/app/services/rooms.service';

@Component({
  selector: 'app-create-room',
  templateUrl: './create-room.component.html',
  styleUrls: ['./create-room.component.css'],
})
export class CreateRoomComponent implements OnInit {
  form: any = {
    title: '',
    fileId: '',
    // participantEmails: [],
  };
  errorData: any = {
    title: '',
    fileId: '',
    // participantEmails: '',
  };

  files: FileData[];
  filesSearch: string = '';

  constructor(
    private roomsService: RoomsService,
    private fileService: FileDataServiceService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.getData();
  }

  onSubmit(): void {
    this.clearErrorDescription();

    let { title, fileId } = this.form;

    this.roomsService.create(title, fileId).subscribe({
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

  getData(): void {
    this.fileService
      .search(this.filesSearch)
      .subscribe((f) => (this.files = f));
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
