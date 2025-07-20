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
  issuer: 'https://dev-13511724.okta.com/oauth2/ausfg0h3ttKW1i4R45d7',
  clientId: '0oafg0e3qhPkjd43y5d7',
  redirectUri: window.location.origin + '/callback',
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
