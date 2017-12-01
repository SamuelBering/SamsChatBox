import { Component, OnInit, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs/Subscription';

import { UserService } from '../shared/services/user.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent implements OnInit, OnDestroy {

  status: boolean;
  subscription: Subscription;
  hasApiAccess: boolean = false;
  apiAccessStatusSubscription: Subscription;

  isIn = false;   // store state

  constructor(private userService: UserService) {
  }

  logout() {
    this.userService.logout();
    this.isIn = false;
  }

  ngOnInit() {
    this.subscription = this.userService.authNavStatus$.subscribe(status => this.status = status);
    this.apiAccessStatusSubscription = this.userService.apiAccessNavStatusSource
      .subscribe(status => {
      this.hasApiAccess = status;
      });
  }

  ngOnDestroy() {
    // prevent memory leak when component is destroyed
    this.subscription.unsubscribe();
    this.apiAccessStatusSubscription.unsubscribe();
  }

  toggleState() { // click handler
    let bool = this.isIn;
    this.isIn = bool === false ? true : false;
  }

}

