import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AvailableRoom } from 'src/app/models/available-room';
import { RoomsService } from 'src/app/services/rooms.service';

@Component({
  selector: 'app-available-rooms-list',
  templateUrl: './available-rooms-list.component.html',
  styleUrls: ['./available-rooms-list.component.css'],
})
export class AvailableRoomsListComponent implements OnInit {
  rooms: AvailableRoom[] = [];

  constructor(private service: RoomsService, private router: Router) {}

  ngOnInit(): void {
    this.service.getAvailable().subscribe((a) => (this.rooms = a));
  }

  enterRoom(id: number): void {
    this.router.navigate([`/rooms/code/${id}`]);
  }
}
