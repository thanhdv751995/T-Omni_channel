import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ShareServiceService } from '../shares-service/share-service.service';


@Injectable({
  providedIn: 'root'
})
export class ShopServiceService {

  constructor(private shareService: ShareServiceService) { }

  public getInfoShop(access_token : string): Observable<any> {
    const url = `${this.shareService.REST_API_SERVER}/api/app/shop/authorized-shop?access_token=${access_token}`;
    return this.shareService.returnHttpClientGet(url)
  }
}
