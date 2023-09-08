import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { FullRoom } from 'src/app/models/full-room';
import { AzureStorageService } from 'src/app/services/azure-storage.service';
import { RoomsService } from 'src/app/services/rooms.service';

@Component({
  selector: 'app-room',
  templateUrl: './room.component.html',
  styleUrls: ['./room.component.css'],
})
export class RoomComponent implements OnInit {
  roomId: number = -1;
  room: FullRoom;
  videoUrl: string;

  constructor(
    private route: ActivatedRoute,
    private roomService: RoomsService,
    private blobService: AzureStorageService
  ) {}

  ngOnInit(): void {
    this.roomId = Number(this.route.snapshot.paramMap.get('id'));
    this.roomService.getRoomInfo(this.roomId).subscribe((r) => {
      console.log(r);
      this.room = r;
      this.videoUrl = this.blobService.getVideoUrl(this.room.fileName);
    });
  }
}
