import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { ShareServiceService } from '../shares-service/share-service.service';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthenServiceService {
  public REST_API_SERVER = environment.apis.default.url;
  constructor(
    private shareService: ShareServiceService) { }

  getAccessToken(auth_code :  string) : Observable<any> {
    const url = `${this.shareService.REST_API_SERVER}/api/app/authentication/access-token-with-auth-code`;
    let dto = {
      app_key : "4l89pg",
      app_secret : "2c9d1a45484d0d171a995814ee0b5b12b1d02820",
      auth_code : auth_code,
      grant_type : "authorized_code"
    }
    return this.shareService.postHttpClient(url, dto);
  }
  getAccessTokenByIdShop(idShop :  string) : Observable<any> {
    const url = `${this.shareService.REST_API_SERVER}/api/app/share/access-token-shop-id?shop_id=${idShop}`;
    return this.shareService.postHttpClient(url, null);
  }
}
