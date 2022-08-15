import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { map, switchMap } from 'rxjs/operators';
import { postChannelAuthenticationDto } from 'src/Modal/postChannelAuthenticationDto';
import { postChannelAuthenticationShopeeDto } from 'src/Modal/postChannelAuthenticationShopeeDto';
import { ChannelAuthenticationService } from 'src/service/channelAuthentication/channel-authentication.service';
import { LoginService } from 'src/service/shopees/login.service';

@Component({
  selector: 'app-shopee-callback-url',
  templateUrl: './shopee-callback-url.component.html',
  styleUrls: ['./shopee-callback-url.component.scss']
})
export class ShopeeCallbackUrlComponent implements OnInit {
  currentRouter = '';
  shop_id : '';
  code : '';
  accesstken ;
  postChannelAuthenticationDto: postChannelAuthenticationShopeeDto;
  
  constructor(private route: ActivatedRoute,
    private loginService: LoginService,
    private channelService: ChannelAuthenticationService,
    ) { }

  ngOnInit(): void {
    this.checkCurrentRoute();
  }
  checkCurrentRoute(){
    
    if (window.location.href.includes('code') && window.location.href.includes('shop_id')){
      this.route.queryParams.pipe(map((params) =>{
        this.shop_id = params.shop_id;
        this.code = params.code;
      }),switchMap(() => {
        return this.loginService.getAccessTokenShopee(this.shop_id,this.code);
      }),
      switchMap((res) => {
        console.log(res);
        const dto = {
          client_id: "thanh.tpos.vn",
          app : "tpos",
          access_token: res.access_token,
          access_token_expire_in: res.access_token_expire_in.toString(),
          refresh_token: res.refresh_token,
          refresh_token_expire_in: res.refresh_token_expire_in.toString(),
          open_id: res.open_id.toString(),
          seller_name: res.seller_name,
          shop_id: res.shop_id.toString(),
          e_Channel : res.e_channel
        };
        this.postChannelAuthenticationDto = dto;
        
        console.log(this.postChannelAuthenticationDto);
        
        return this.channelService.postChannelAuthenticationShopee(
          this.postChannelAuthenticationDto
        );
       // return res;
      })
      ).subscribe(async res =>{
        window.close();
      })
    }
  }


}
