import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CallbackUrlComponent } from './callback-url/callback-url.component';
import { LoginComponent } from './login/login.component';
import { ShopeeCallbackUrlComponent } from './shopees/shopee-callback-url/shopee-callback-url.component';
import { ShopeeLoginComponent } from './shopees/shopee-login/shopee-login.component';

const routes: Routes = [
  { path: '', component: LoginComponent},
  { path: 'callbackURL', component : CallbackUrlComponent },
  { path: 'shopee', component : ShopeeLoginComponent },
   { path: 'callback-url', component : ShopeeCallbackUrlComponent },
  { path: 'tiktok-connection/callback', component : CallbackUrlComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
