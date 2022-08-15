import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { ShareServiceService } from '../shares-service/share-service.service';

@Injectable({
  providedIn: 'root'
})
export class ChannelAuthenticationService {
  public REST_API_SERVER = environment.apis.default.url;
  constructor(
    private shareService: ShareServiceService) { }

  postChannelAuthentication(dto :  any) {
    const url = `${this.shareService.REST_API_SERVER}/api/app/channel-authentication/channel-authentication`;
    return this.shareService.postHttpClient(url, dto);
  }
  public getChannelAuthenticationShop(channel_token : string): Observable<any> {
    const url = `${this.shareService.REST_API_SERVER}/api/app/channel-authentication/channel-authentication-by-id?channel_token=${channel_token}`;
    return this.shareService.returnHttpClientGet(url)
  }
  public updateChannelAuthentication(channel_token : string , dto : any): Observable<any> {
    const url = `${this.shareService.REST_API_SERVER}/api/app/channel-authentication?channel_token=${channel_token}`;
    return this.shareService.putHttpClient(url,dto);
  }
  postChannelAuthenticationShopee(dto :  any) {
    const url = `${this.shareService.REST_API_SERVER}/api/app/channel-authentication/channel-authentication-shopee`;
    return this.shareService.postHttpClient(url, dto);
  }
}
