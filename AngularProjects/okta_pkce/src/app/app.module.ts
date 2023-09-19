import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { LoginComponent } from './home/login.component';
import { OktaAuthModule, OKTA_CONFIG } from '@okta/okta-angular';
import { OktaAuth } from '@okta/okta-auth-js';
import { SignoutpageComponent } from './signoutpage/signoutpage.component';
import { ValidatetokenComponent } from './validatetoken/validatetoken.component';
import { HttpClientModule } from '@angular/common/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import {MatIconModule} from '@angular/material/icon';
import {MatButtonModule} from '@angular/material/button';
import {MatToolbarModule} from '@angular/material/toolbar';
import {MatTooltipModule} from '@angular/material/tooltip';

const oktaAuth = new OktaAuth({
  issuer: 'https://dev-45061733.okta.com/oauth2/aus64hhbp70XUaSIN5d7',
  clientId: '0oa64hdgz2uKuv80c5d7',
  redirectUri: window.location.origin + '/login/callback',
  postLogoutRedirectUri: window.location.origin + '/signout'
});

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    SignoutpageComponent,
    ValidatetokenComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    OktaAuthModule.forRoot({oktaAuth}),
    HttpClientModule,
    BrowserAnimationsModule,
    MatToolbarModule, 
    MatButtonModule, 
    MatIconModule,
    MatTooltipModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
