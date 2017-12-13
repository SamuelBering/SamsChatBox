import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { HubConnection, HttpConnection, TransportType } from '@aspnet/signalr-client';
import { ChatService } from '../services/chat.service';
import { Room } from '../models/room.interface';
import { Subscription } from 'rxjs/Subscription';

@Component({
  selector: 'app-room',
  templateUrl: './room.component.html',
  styleUrls: ['./room.component.scss']
})
export class RoomComponent implements OnInit, OnDestroy {

  private _hubConnection: HubConnection;
  public async: any;
  message = '';
  messages: string[] = [];
  users: string[] = [];
  connectionStatusSubscription: Subscription;
  queryParamsSubscription: Subscription;

  currentRoom: Room;


  constructor(private chatService: ChatService, private route: ActivatedRoute) { }

  public sendMessage(): void {
    this.chatService.sendMessage(this.message, this.currentRoom);
  }

  ngOnInit() {
    this.queryParamsSubscription = this.route.queryParams.subscribe(params => {

      this.currentRoom = { id: params['roomId'], title: params['roomTitle'] };
      this.connectionStatusSubscription = this.chatService.connectionStatusSource
        .subscribe(statusIsConnected => {
          if (statusIsConnected) {
            if (this.currentRoom !== undefined)
              this.chatService.enterRoom(this.currentRoom);
          }
        });

    });

    this._hubConnection = this.chatService.HubConnection;
    this._hubConnection.on('send', (message: string, sender: string) => {
      const received: string = `${sender}: ${message}`;
      this.messages.push(received);
    });

    this._hubConnection.on('updateUsers', (data: any) => {
      this.users = data;
    });



  }

  ngOnDestroy() {
    // prevent memory leak when component is destroyed
    this.connectionStatusSubscription.unsubscribe();
  }

}
