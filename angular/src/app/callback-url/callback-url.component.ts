import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { filter, map, switchMap, takeUntil } from 'rxjs/operators';
import { postChannelAuthenticationDto } from 'src/Modal/postChannelAuthenticationDto';
import { AuthenServiceService } from 'src/service/authen/authen-service.service';
import { ChannelAuthenticationService } from 'src/service/channelAuthentication/channel-authentication.service';
import { LogisticService } from 'src/service/logistic/logistic.service';
import { ProductService } from 'src/service/products/product.service';
import { ShopServiceService } from 'src/service/shop/shop-service.service';
@Component({
  selector: 'app-callback-url',
  templateUrl: './callback-url.component.html',
  styleUrls: ['./callback-url.component.scss']
})
export class CallbackUrlComponent implements OnInit {
  currentRouter = '';
  client_id = '';
  app= '';
  eChannel = '';
  postChannelAuthenticationDto: postChannelAuthenticationDto;
  constructor(private route: ActivatedRoute,
              private authenServiceService: AuthenServiceService,
              private shopServiceService: ShopServiceService,
              private logisticService : LogisticService,
              private channelService: ChannelAuthenticationService,
              private productService : ProductService
              ) { }
  ngOnInit(): void {
    this.checkCurrentRoute();
  }
  checkCurrentRoute() {
    if (window.location.href.includes('code') && window.location.href.includes('state')) {
      var cnToken;
      this.route.queryParams
        .pipe(
          map((params) => {
              this.currentRouter = params.code;
              this.client_id = params.state.split('clientId')[1];            
              this.app = params.state.split('app')[1].split('clientId')[0];      
              this.eChannel = params.state.split('app')[0].split('e_channel')[1]
          }),
          switchMap(() => {
            return this.authenServiceService.getAccessToken(this.currentRouter);
          }),
          switchMap((res) => {
              const dto = {
                client_id: this.client_id,
                app : this.app,
                access_token: res.data.access_token,
                access_token_expire_in: res.data.access_token_expire_in.toString(),
                refresh_token: res.data.refresh_token,
                refresh_token_expire_in: res.data.refresh_token_expire_in,
                open_id: res.data.open_id,
                seller_name: res.data.seller_name,
                shop_id: '',
                eChannel : this.eChannel
              };
              this.postChannelAuthenticationDto = dto;
              return this.shopServiceService.getInfoShop(res.data.access_token);
          }),
          switchMap((data) => {
            this.postChannelAuthenticationDto.shop_id = data.data.shop_list[0].shop_id;
            return this.channelService.postChannelAuthentication(
              this.postChannelAuthenticationDto
            );
          }),switchMap((res)=>{
             localStorage.setItem('channel_token', res.channel_token);

             cnToken = res.channel_token;

            return this.logisticService.getListWarehouseByChannelToken(res.channel_token);
          }),
          switchMap((res)=>{
            return this.channelService.updateChannelAuthentication(cnToken, res);
          })
        )
        .subscribe(async res => {
           window.close();
        }); 
    }
  }
}
