import { Component, OnInit } from '@angular/core';
import { HubConnection, HttpConnection, TransportType } from '@aspnet/signalr-client';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {

  private _hubConnection: HubConnection;
  public async: any;
  message = '';
  messages: string[] = [];
  users: string[] = [];

  constructor() { }

  public sendMessage(): void {
    const data = `Sent: ${this.message}`;

    this._hubConnection.invoke('Send', data);
    this.messages.push(data);
  }

  public addUser(userName: string): void {
    const data = `${userName}`;
    this._hubConnection.invoke('AddUser', data);
  }

  ngOnInit() {
    let access_token = localStorage.getItem('auth_token');
    let url = '/chat' + '?signalRTokenHeader=' + access_token;
    let httpCon = new HttpConnection(url, { transport: TransportType.WebSockets });
    this._hubConnection = new HubConnection(httpCon);

    this._hubConnection.on('send', (data: any) => {
      const received = `Received: ${data}`;
      this.messages.push(received);
    });

    this._hubConnection.on('updateUsers', (data: any) => {
      this.users = data;
    });

    this._hubConnection.start()
      .then(() => {
        console.log('Hub connection started');
        // let userName = localStorage.getItem('userName');
        // this.addUser(userName);
      })
      .catch(err => {
        console.log('Error while establishing connection');
      });
  }

}
