import { Injectable } from '@angular/core';
import { Http, Response, Headers, RequestOptions } from '@angular/http';
import { HubConnection, HttpConnection, TransportType } from '@aspnet/signalr-client';

import { ConfigService } from '../../shared/utils/config.service';

import { BaseService } from '../../shared/services/base.service';

import { Observable } from 'rxjs/Rx';

// import * as _ from 'lodash';
// Add the RxJS Observable operators we need in this app.
import '../../rxjs-operators';
import { Room } from '../models/room.interface';
import { BehaviorSubject } from 'rxjs/BehaviorSubject';

@Injectable()

export class ChatService extends BaseService {

    baseUrl: string = '';
    private _hubConnection: HubConnection;
    connectionStatusSource = new BehaviorSubject<boolean>(false);


    constructor(private http: Http, private configService: ConfigService) {
        super();
        this.baseUrl = configService.getApiURI();
    }

    get HubConnection(): HubConnection {
        return this._hubConnection;
    }

    GetHubConnection(): HubConnection {
        let access_token = localStorage.getItem('auth_token');
        let url = '/chat' + '?signalRTokenHeader=' + access_token;
        let httpCon = new HttpConnection(url, { transport: TransportType.WebSockets });
        let hubConnection = new HubConnection(httpCon);
        this._hubConnection = hubConnection;
        return this._hubConnection;
    }

    startHubConnection() {
        this._hubConnection.start()
            .then(() => {
                console.log('Hub connection started');
                this.connectionStatusSource.next(true);
            })
            .catch(err => {
                this.connectionStatusSource.next(false);
                console.log('Error while establishing connection');
            });
    }

    public sendMessage(message: string, room: Room): void {

        this._hubConnection.invoke('SendToRoom', message, room);
    }

    createRoom(title: string): Observable<Room> {
        let body = JSON.stringify({ title });
        let headers = new Headers({ 'Content-Type': 'application/json' });
        let options = new RequestOptions({ headers: headers });

        return this.http.post(this.baseUrl + '/chat/createroom', body, options)
            .map(res => res.json())
            .catch(this.handleError);
    }

    enterRoom(room: Room) {
        this._hubConnection.invoke('EnterRoom', room);
    }



}
