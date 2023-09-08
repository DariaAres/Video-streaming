import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { FullRoom } from 'src/app/models/full-room';
import { RoomsService } from 'src/app/services/rooms.service';

@Component({
  selector: 'app-my-rooms-list',
  templateUrl: './my-rooms-list.component.html',
  styleUrls: ['./my-rooms-list.component.css'],
})
export class MyRoomsListComponent implements OnInit {
  rooms: FullRoom[] = [];

  constructor(private service: RoomsService, private router: Router) {}

  ngOnInit(): void {
    this.getData();
  }

  addParticipant(room: FullRoom): void {
    this.router.navigate([`/rooms/add-participant/${room.id}`]);
  }

  open(room: FullRoom): void {
    this.router.navigate([`/rooms/${room.id}`]);
  }

  delete(room: FullRoom): void {
    this.service.delete(room.id).subscribe((_) => this.getData());
  }

  getData(): void {
    this.service.getMy().subscribe((r) => (this.rooms = r));
  }
}
