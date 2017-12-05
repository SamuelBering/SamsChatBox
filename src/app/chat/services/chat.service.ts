import { Injectable } from '@angular/core';
import { Http, Response, Headers, RequestOptions } from '@angular/http';
import { HubConnection, HttpConnection, TransportType } from '@aspnet/signalr-client';

import { ConfigService } from '../../shared/utils/config.service';

import { BaseService } from '../../shared/services/base.service';

import { Observable } from 'rxjs/Rx';

// import * as _ from 'lodash';
// Add the RxJS Observable operators we need in this app.
import '../../rxjs-operators';

@Injectable()

export class ChatService extends BaseService {

    baseUrl: string = '';

    constructor(private http: Http, private configService: ConfigService) {
        super();
        this.baseUrl = configService.getApiURI();
    }

    GetHubConnection(roomId: number): HubConnection {
        let access_token = localStorage.getItem('auth_token');
        let url = '/chat' + '?signalRTokenHeader=' + access_token + '&roomId=' + roomId;
        let httpCon = new HttpConnection(url, { transport: TransportType.WebSockets });
        let hubConnection = new HubConnection(httpCon);
        return hubConnection;
    }

    public sendMessage(message: string, roomId: number, hubConnection: HubConnection): void {

        hubConnection.invoke('Send', message, roomId);
    }



}