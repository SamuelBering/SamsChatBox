import { ModuleWithProviders } from '@angular/core';
import { RouterModule } from '@angular/router';
import { RootComponent } from './root/root.component';
import { HomeComponent } from './home/home.component';
import { AuthGuard } from '../auth.guard';
import { ErrorComponent } from './error/error.component';

export const routing: ModuleWithProviders = RouterModule.forChild([
  {
    path: 'guide',
    component: RootComponent, canActivate: [AuthGuard],

    children: [
      { path: '', component: HomeComponent },
      { path: 'home', component: HomeComponent },
      { path: 'error/:errorKey', component: ErrorComponent }
    ]
  }
]);
