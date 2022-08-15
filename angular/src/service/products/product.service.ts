import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/internal/Observable';
import { ShareServiceService } from '../shares-service/share-service.service';

@Injectable({
  providedIn: 'root'
})
export class ProductService {

  constructor(private shareService: ShareServiceService) { }

  getListProductTikTokShopByShopId(channel_token : string) {
    let dto = {
      page_number : 1,
      page_size: 20
    }
    const url = `${this.shareService.REST_API_SERVER}/api/app/product/products?channel_token=${channel_token}`;
    return this.shareService.postHttpClient(url, dto);
  }
  public getListProductDetail(product_id : string , channel_token : string): Observable<any> {
    const url = `${this.shareService.REST_API_SERVER}/api/app/product/product-detail?product_id=${product_id}&channel_token=${channel_token}`;
    return this.shareService.returnHttpClientGet(url)
  }
}
