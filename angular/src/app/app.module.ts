import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HttpClientModule } from '@angular/common/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { registerLocaleData } from '@angular/common';
import localeVi from '@angular/common/locales/vi'; 
import { CallbackUrlComponent } from './callback-url/callback-url.component';
import { LoginComponent } from './login/login.component';
import { TDSButtonMenuModule } from 'tds-ui/button-menu';
import { TDSButtonModule } from 'tds-ui/button';
import { TDS_I18N, vi_VN } from 'tds-ui/i18n';
import { TDSTableModule } from 'tds-ui/table';
import { TDSModalModule } from 'tds-ui/modal';
import { FormatDescription } from './Pipe/FormatDescription';
import { TDSFlexModule } from 'tds-ui/flex';
import { TDSTagModule } from 'tds-ui/tag';
import { TDSMessageModule } from 'tds-ui/message';
import { ShopeeLoginComponent } from './shopees/shopee-login/shopee-login.component';
import { ShopeeCallbackUrlComponent } from './shopees/shopee-callback-url/shopee-callback-url.component';
// Thiết lập tiếng Việt
registerLocaleData(localeVi); 
@NgModule({
  declarations: [
    FormatDescription,
    AppComponent,
    CallbackUrlComponent,
    LoginComponent,
    ShopeeLoginComponent,
    ShopeeCallbackUrlComponent
  ],
  imports: [
    TDSMessageModule ,
    TDSTagModule ,
    TDSFlexModule,
    TDSModalModule,
    TDSTableModule,
    TDSButtonModule,
    TDSButtonMenuModule,
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    BrowserAnimationsModule
  ],
  providers: [{ provide: TDS_I18N, useValue: vi_VN }],
  bootstrap: [AppComponent]
})
export class AppModule { }
