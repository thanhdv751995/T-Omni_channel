import { Component, OnInit } from '@angular/core';
import { LoginService } from 'src/service/shopees/login.service';

@Component({
  selector: 'app-shopee-login',
  templateUrl: './shopee-login.component.html',
  styleUrls: ['./shopee-login.component.scss']
})
export class ShopeeLoginComponent implements OnInit {

  url;
  constructor(private loginService: LoginService) { }

  ngOnInit(): void {
    this.getURl();
  }
  myFunction(){
    window.open(this.url,
      '_blank',
    'toolbar=yes,scrollbars=yes,resizable=yes,left=-3000,width=900,height=900');
  }
  getURl(){
    this.loginService.getUrlShopee().subscribe(data=> {
      console.log(data.authorizeUrl);
      this.url = data.authorizeUrl;
    })
  }
}
