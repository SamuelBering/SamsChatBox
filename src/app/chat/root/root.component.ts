import { Component, OnInit, ChangeDetectorRef } from '@angular/core';
import { NgForm } from '@angular/forms/src/directives/ng_form';
import { ChatService } from '../services/chat.service';
import { HubConnection } from '@aspnet/signalr-client';
import { Room } from '../models/room.interface';
import { retry } from 'rxjs/operator/retry';

@Component({
  selector: 'app-root',
  templateUrl: './root.component.html',
  styleUrls: ['./root.component.scss']
})
export class RootComponent implements OnInit {

  errors: string;
  isRequesting: boolean;
  submitted: boolean = false;
  private _hubConnection: HubConnection;
  private _roomsList: Room[];

  constructor(private chatService: ChatService, private changeDetectorRef: ChangeDetectorRef) {

    // updateRoomsList
  }

  ngOnInit() {
    this._hubConnection = this.chatService.GetHubConnection();

    this._hubConnection.on('updateRoomsList', (data: any) => {
      const objArray = JSON.parse(data);
      let roomslistdebug: Room[] = objArray as Array<Room>;
      this._roomsList = roomslistdebug;
      let debug: string = '';
      this.changeDetectorRef.detectChanges();
    });

  }

  onSubmitNewRoom(f: NgForm) {
    if (f.valid) {
      let roomName = f.value['roomName'];
      let id = this.createRoom(roomName);
    }
  }

  createRoom(title: string) {
    this.submitted = true;
    this.isRequesting = true;
    this.errors = '';
    this.chatService.createRoom(title)
      .finally(() => this.isRequesting = false)
      .subscribe(
      result => {
        let id = result.id;

        // if (result) {
        //   this.router.navigate(['/login'], { queryParams: { brandNew: true, email: value.email } });
        // }
      },
      errors => this.errors = errors);

  }

  // debug() {
  //   let debug = this._roomsList;
  // }

}
