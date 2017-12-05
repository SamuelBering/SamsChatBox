import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RootComponent } from './root/root.component';
import { HomeComponent } from './home/home.component';
import { routing } from './chat.routing';
import { FormsModule } from '@angular/forms';
import { ChatService } from './services/chat.service';
import { RoomComponent } from './room/room.component';
import { ModalModule } from 'ng2-modal';

@NgModule({
  imports: [
    CommonModule,
    routing,
    FormsModule,
    ModalModule
  ],
  declarations: [RootComponent, HomeComponent, RoomComponent],
  providers: [ChatService]
})
export class ChatModule { }
