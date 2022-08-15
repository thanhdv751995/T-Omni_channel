import { Injectable } from '@angular/core';
import { ShareServiceService } from '../shares-service/share-service.service';
import * as signalR from '@microsoft/signalr';

@Injectable({
  providedIn: 'root',
})
export class SignalRService {
  public connection;
  public connectionID;
  constructor(private shareService: ShareServiceService) {}

  SetConnection() {
    this.connection = new signalR.HubConnectionBuilder()
      .withUrl(this.shareService.REST_API_SERVER + '/notify')
      .build();
    this.StartConnection();
  }
  StartConnection() {
    this.connection
      .start()
      .then(() => this.GetConnectionID())
      .catch((err) => {
        console.log('err here', err);
      });
  }
  GetConnectionID() {
    this.connection.on('GetIdConnection', (Id) => {
      this.connectionID = Id;
      // this.shareService.setConnectID(({localStorage.:Id}));
    });
  }
}
