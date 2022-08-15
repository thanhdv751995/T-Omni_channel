import { HttpParams } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { map } from 'rxjs/operators';
import { ChannelAuthenticationService } from 'src/service/channelAuthentication/channel-authentication.service';
import { ProductService } from 'src/service/products/product.service';
import { SignalRService } from 'src/service/signalR/signal-r.service';
import { TDSMessageService } from 'tds-ui/message';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  redirectUrl = 'https://auth.tiktok-shops.com/oauth/authorize?app_key=4l89pg';
  currentRouter = '';
  user_id = '';
  shopId = ''
  listShop : any;
  productDetail : any;
  isVisible = false;
  infoShop : any;
  connectionHub: any;
  channelToken =''
  result = {
    app : '',
    clientId : '',
    e_channel : ''
  }
  constructor(private route: ActivatedRoute,
    private router: Router,
    private productService : ProductService,
    private channelAuthenticationService : ChannelAuthenticationService,
    private signalRService: SignalRService,
    private message: TDSMessageService) {
  }

  ngOnInit(): void {
    this.router.navigate(['.'], { relativeTo: this.route, queryParams: {e_channel:'TiktokShop' , app: 'tpos' , clientId:'hoa.tpos.vn' } });
    this.getListProductShop()
    this.getAuthenticationShopId()
    this.connectionHub = this.signalRService.connection;
    this.listenDisposeMessage()
  }
  async myFunction() {
    window.open(
      this.redirectUrl + `&state=${await this.generateState(64)}`,
      '_blank',
      'toolbar=yes,scrollbars=yes,resizable=yes,left=-10000,width=800,height=800'
    );
  }
  getParams() {
    return new Promise((resolve, rej) => {
      this.route.queryParams.subscribe((qp) => {
          this.result.app = qp.app;
          this.result.clientId = qp.clientId;
          this.result.e_channel = qp.e_channel;
        return this.result;
      });
      resolve(this.result)
    })
 }
  async generateState(length: any) {
    var mapping = await this.getParams();
    console.log(mapping);
    
    var result = '';
    var characters = 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789';
    var charactersLength = characters.length;
    for (var i = 0; i < length; i++) {
      result += characters.charAt(Math.floor(Math.random() * charactersLength));
    }
    if (mapping) {
      result = `${result}e_channel${mapping['e_channel']}app${mapping['app']}clientId${mapping['clientId']}`;
    }
    return result;
  }
  ///real time
  listenDisposeMessage(){
    this.connectionHub.on("ReceiveMessage",(value)=>{
      setTimeout(()=>{
        this.getListProductShop()
        this.getAuthenticationShopId()
        this.message.success(value);
      },300)
    })
  }n 
  getListProductShop() {
    if(localStorage.getItem("channel_token"))
    { 
      this.channelToken = localStorage.getItem("channel_token")
      
      this.productService.getListProductTikTokShopByShopId(this.channelToken).subscribe(res=>{       
        this.listShop = res.data   
        console.log(this.listShop);
          
      })
    }
  }
  getAuthenticationShopId()
  { 
    if(localStorage.getItem("channel_token"))
    { 
      this.channelToken = localStorage.getItem("channel_token")
      
      this.channelAuthenticationService.getChannelAuthenticationShop(this.channelToken).subscribe(res=>{
        this.infoShop = res 
      })
    }
  }
  showModal(id : string): void {
    this.productService.getListProductDetail(id , this.channelToken).subscribe(res=>{
      this.productDetail = res?.data
      this.isVisible = true;
    })
  }

  handleOk(): void {
    this.isVisible = false;
  }

  handleCancel(): void {
    this.isVisible = false;
  }
  convertTime(time : any) {
    return new Date(time * 1000);
  }
}
