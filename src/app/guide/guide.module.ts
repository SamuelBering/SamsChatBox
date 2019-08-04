import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RootComponent } from './root/root.component';
import { HomeComponent } from './home/home.component';
import { PlacesComponent } from './places/places.component';
import { PlaceService } from './services/place.service';

import { routing } from './guide.routing';
// import { FormsModule } from '@angular/forms';
// import { ModalModule } from 'ng2-modal';

@NgModule({
  imports: [
    CommonModule,
    routing,
    // FormsModule,
    // ModalModule
  ],
  declarations: [RootComponent, HomeComponent, PlacesComponent],
  providers: [PlaceService]
})
export class GuideModule { }
