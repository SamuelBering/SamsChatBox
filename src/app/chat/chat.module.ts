import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RootComponent } from './root/root.component';
import { HomeComponent } from './home/home.component';
import { routing } from './chat.routing';


@NgModule({
  imports: [
    CommonModule,
    routing
  ],
  declarations: [RootComponent, HomeComponent]
})
export class ChatModule { }
