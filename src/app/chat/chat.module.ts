import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RootComponent } from './root/root.component';
import { HomeComponent } from './home/home.component';
import { routing } from './chat.routing';
import { FormsModule } from '@angular/forms';


@NgModule({
  imports: [
    CommonModule,
    routing,
    FormsModule
  ],
  declarations: [RootComponent, HomeComponent]
})
export class ChatModule { }
