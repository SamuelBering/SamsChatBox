import { Component, OnInit } from '@angular/core';
import { HubConnection, HttpConnection, TransportType } from '@aspnet/signalr-client';
import { ChatService } from '../services/chat.service';
import { Room } from '../models/room.interface';

@Component({
  selector: 'app-room',
  templateUrl: './room.component.html',
  styleUrls: ['./room.component.scss']
})
export class RoomComponent implements OnInit {

  private _hubConnection: HubConnection;
  public async: any;
  message = '';
  messages: string[] = [];
  users: string[] = [];

  constructor(private chatService: ChatService) { }

  public sendMessage(): void {
    this.chatService.sendMessage(this.message, 1, this._hubConnection);
  }

  ngOnInit() {
    this._hubConnection = this.chatService.HubConnection;

    this._hubConnection.on('send', (data: any) => {
      const received = `Received: ${data}`;
      this.messages.push(received);
    });

    this._hubConnection.on('updateUsers', (data: any) => {
      this.users = data;
    });
    let room: Room = { id: 1, title: 'Kitchen' };

    this.chatService.enterRoom(room);

  }

}
