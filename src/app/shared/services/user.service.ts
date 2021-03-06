import { Injectable } from '@angular/core';
import { Http, Response, Headers, RequestOptions } from '@angular/http';

import { UserRegistration } from '../models/user.registration.interface';
import { ConfigService } from '../utils/config.service';

import { BaseService } from './base.service';

import { Observable } from 'rxjs/Rx';
import { BehaviorSubject } from 'rxjs/Rx';

// import * as _ from 'lodash';
// Add the RxJS Observable operators we need in this app.
import '../../rxjs-operators';

@Injectable()

export class UserService extends BaseService {

  baseUrl: string = '';

  // Observable navItem source
  private _authNavStatusSource = new BehaviorSubject<boolean>(false);
  apiAccessNavStatusSource = new BehaviorSubject<boolean>(false);

  // Observable navItem stream
  authNavStatus$ = this._authNavStatusSource.asObservable();

  private loggedIn = false;

  private apiAccess = false;

  constructor(private http: Http, private configService: ConfigService) {
    super();
    this.loggedIn = !!localStorage.getItem('auth_token');
    // ?? not sure if this the best way to broadcast the status but seems to resolve issue on page refresh where auth status is lost in
    // header component resulting in authed user nav links disappearing despite the fact user is still logged in
    this._authNavStatusSource.next(this.loggedIn);

    this.apiAccess = (localStorage.getItem('api_access') === 'true');

    this.apiAccessNavStatusSource.next(this.apiAccess);

    this.baseUrl = configService.getApiURI();
  }

  register(email: string, password: string, firstName: string, lastName: string, location: string): Observable<UserRegistration> {
    let body = JSON.stringify({ email, password, firstName, lastName, location });
    let headers = new Headers({ 'Content-Type': 'application/json' });
    let options = new RequestOptions({ headers: headers });

    return this.http.post(this.baseUrl + '/accounts', body, options)
      .map(res => true)
      .catch(this.handleError);
  }

  login(userName, password) {
    let headers = new Headers();
    headers.append('Content-Type', 'application/json');

    return this.http
      .post(
      this.baseUrl + '/auth/login',
      JSON.stringify({ userName, password }), { headers }
      )
      .map(res => res.json())
      .map(res => {
        this.apiAccess = res.hasApiAccess;
        this.apiAccessNavStatusSource.next(this.apiAccess);
        localStorage.setItem('auth_token', res.auth_token);
        localStorage.setItem('api_access', this.apiAccess.toString());
        localStorage.setItem('userName', userName);
        this.loggedIn = true;
        this._authNavStatusSource.next(true);
        return true;
      })
      .catch(this.handleError);
  }

  logout() {
    localStorage.removeItem('auth_token');
    localStorage.removeItem('api_access');
    localStorage.removeItem('userName');
    this.loggedIn = false;
    this.apiAccess = false;
    this._authNavStatusSource.next(false);
    this.apiAccessNavStatusSource.next(false);
  }

  isLoggedIn() {
    return this.loggedIn;
  }

  hasApiAccess() {
    return this.apiAccess;
  }

}
