import { ModuleWithProviders } from '@angular/core';
import { RouterModule } from '@angular/router';

import { RootComponent } from './root/root.component';
import { HomeComponent } from './home/home.component';
import { RoomComponent } from './room/room.component';

import { AuthGuard } from '../auth.guard';

export const routing: ModuleWithProviders = RouterModule.forChild([
  {
    path: 'chat',
    component: RootComponent, canActivate: [AuthGuard],

    children: [
      { path: '', component: HomeComponent },
      { path: 'home', component: HomeComponent },
      { path: 'room', component: RoomComponent }
    ]
  }
]);
