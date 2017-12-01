import { Component, OnInit } from '@angular/core';
import { HubConnection } from '@aspnet/signalr-client';

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

  constructor() { }

  public sendMessage(): void {
    const data = `Sent: ${this.message}`;

    this._hubConnection.invoke('Send', data);
    this.messages.push(data);
  }

  ngOnInit() {
    this._hubConnection = new HubConnection('/chat');

    this._hubConnection.on('send', (data: any) => {
      const received = `Received: ${data}`;
      this.messages.push(received);
    });

    this._hubConnection.start()
      .then(() => {
        console.log('Hub connection started');
      })
      .catch(err => {
        console.log('Error while establishing connection');
      });
  }

}
