import { Component, OnDestroy, OnInit } from '@angular/core';
import { OktaAuthStateService } from '@okta/okta-angular';
import { filter, map, mergeMap, Observable, Subscription } from 'rxjs';
import { AuthState } from '@okta/okta-auth-js'
import { TokenService } from '../token.service';


@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})


export class LoginComponent implements OnInit, OnDestroy {

  // public idtoken$!: Observable<any>;
  // public claims$!: Observable<any>;
  public tokensubscription!: Subscription;
  public idtoken: any;
  public idtokenclaims: any;
  public accesstoken: any;
  public accesstokenclaims: any;

  constructor(private _oktaAuthStateService: OktaAuthStateService, public tokenservice: TokenService) { }

  public ngOnInit(): void {
    console.log("first")

    this.tokensubscription = this._oktaAuthStateService.authState$.pipe(
      filter((authState: AuthState) => !!authState && !!authState.isAuthenticated),
      map((authState: AuthState)=>{
        this.accesstoken = authState.accessToken;
        this.tokenservice.addAccessToken(authState.accessToken!.accessToken)
        this.tokenservice.addAccessTokenClaims(authState.accessToken!.claims)
        this.accesstokenclaims = authState.accessToken!.claims
        return authState;
      }),
      map((authState: AuthState) => authState.idToken),
      map((idtoken) =>{
        this.tokenservice.addIdToken(idtoken!.idToken);
        this.idtoken = idtoken
        console.log(idtoken!.idToken)    
        return idtoken;
      })
    ).subscribe(idtoken =>{
      this.idtokenclaims = idtoken!.claims;
      this.tokenservice.addIdTokenClaims(idtoken!.claims)
    });

    // this.claims$ = this._oktaAuthStateService.authState$.pipe(
    //   filter((authState: AuthState) => !!authState && !!authState.isAuthenticated),
    //   map((authState: AuthState) => authState.idToken),
    //   map((idtoken) =>{
    //     //this.tokenservice.addToken(idtoken!.claims);
    //     console.log(idtoken!.claims)    
    //     this.claims = idtoken!.claims
    //     return idtoken?.claims;
    //   })
    // );    
  }

  public ngOnDestroy(): void {
      this.tokensubscription.unsubscribe();
  }

  public isString(item: any): boolean 
   { return typeof item === 'string'; }

}