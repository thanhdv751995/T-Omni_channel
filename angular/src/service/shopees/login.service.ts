import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ShareServiceService } from '../shares-service/share-service.service';

@Injectable({
  providedIn: 'root'
})
export class LoginService {

  constructor(private shareService: ShareServiceService) { }
  public getUrlShopee(): Observable<any> {
    const url = `${this.shareService.REST_API_SERVER}/api/app/shopee/authorize-url`;
    return this.shareService.returnHttpClientGet(url)
  }
  public getAccessTokenShopee( shop_id : string,code :string): Observable<any> {
    const url = `${this.shareService.REST_API_SERVER}/api/app/shopee/access-token?shop_id=${shop_id}&code=${code}`;
    return this.shareService.postHttpClient(url,null)
  }
}
