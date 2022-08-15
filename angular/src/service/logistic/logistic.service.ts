import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ShareServiceService } from '../shares-service/share-service.service';

@Injectable({
  providedIn: 'root'
})
export class LogisticService {

  constructor(private shareService: ShareServiceService) 
  { }
  public getListWarehouseByChannelToken(channel_token : string): Observable<any> {
    const url = `${this.shareService.REST_API_SERVER}/api/app/logistic/ware-house-by-channel-token?channel_token=${channel_token}`;
    return this.shareService.returnHttpClientGet(url)
  }
}
