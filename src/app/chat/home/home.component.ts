import { Component, OnInit } from '@angular/core';
import { ChatService } from '../services/chat.service';
import { Room } from '../models/room.interface';
import { HubConnection } from '@aspnet/signalr-client';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {

  private _hubConnection: HubConnection;


  constructor(private chatService: ChatService) { }

  ngOnInit() {

    this._hubConnection = this.chatService.HubConnection;

    let room: Room = { id: 0, title: '(No room)' };

    this._hubConnection.start()
      .then(() => {
        console.log('Hub connection started');
        this.chatService.enterRoom(room);
      })
      .catch(err => {
        console.log('Error while establishing connection');
      });

  }


}
