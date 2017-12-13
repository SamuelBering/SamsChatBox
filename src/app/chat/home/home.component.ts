// import { Component, OnInit, OnDestroy } from '@angular/core';
import { Component, OnInit } from '@angular/core';

// import { ChatService } from '../services/chat.service';
import { Room } from '../models/room.interface';
// import { Subscription } from 'rxjs/Subscription';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
// export class HomeComponent implements OnInit, OnDestroy {
export class HomeComponent implements OnInit {


  // connectionStatusSubscription: Subscription;

  // constructor(private chatService: ChatService) { }
  constructor() { }

  ngOnInit() {
    // this.connectionStatusSubscription = this.chatService.connectionStatusSource
    //   .subscribe(statusIsConnected => {
    //     if (statusIsConnected) {
    //       let room: Room = { id: 0, title: '(No room)' };
    //       this.chatService.enterRoom(room);
    //     }
    //   });
  }

  // ngOnDestroy() {
  //   prevent memory leak when component is destroyed
  //   this.connectionStatusSubscription.unsubscribe();
  // }


}
