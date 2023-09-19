import { Component, OnDestroy, OnInit } from '@angular/core';
import { Observable, Subscription, filter, map, switchMap } from 'rxjs';
import { TokenService } from '../token.service';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-validatetoken',
  templateUrl: './validatetoken.component.html',
  styleUrls: ['./validatetoken.component.scss']
})
export class ValidatetokenComponent implements OnInit, OnDestroy {
  input_idtoken: string="";
  verified_idtoken: any;
  input_accesstoken: string="";
  verified_accesstoken: any;
  nonce: string ="";
  private accesstokensubscription!: Subscription;
  private idtokensubscription!: Subscription;
  constructor(public tokenservice: TokenService, private http: HttpClient){}
  
  // public ngOnInit(): void {
  //   this.tokensubscription = this.tokenservice.getTokenSourceListener()
  //     .subscribe((token)=>{
  //       this.token = token;
  //       // console.log(this.token)
  //     })

  public ngOnInit(): void {
    this.idtokensubscription = this.tokenservice.getIdTokenSourceListener().pipe(
      map(idtoken=>{
        this.input_idtoken = idtoken;
        // console.log(this.input_idtoken);
        return idtoken;
      }),
      // we just want to transform on idtoken using another observable but return idtoken without emitting the other observable
      map(idtoken =>{   
        this.tokenservice.getIdTokenClaimSourceListener().pipe(
          map(idtokenclaims=>{
            this.nonce = idtokenclaims.nonce;
            console.log("extracting nonce from service")
            console.log(this.nonce)
          })
        )
        return idtoken
      })
     ,    
      switchMap(idtoken =>{  // we want to emit the results of another observable
        // console.log(idtoken);
        const payload =  {
          idtoken: idtoken
        }
        return this.http.post<any>('http://localhost:3000/verify/idtoken', payload)
      })
     ).subscribe(verifiedclaims =>{
        this.verified_idtoken = verifiedclaims;
        console.log(this.verified_idtoken)
     })


    this.accesstokensubscription = this.tokenservice.getAccessTokenSourceListener().pipe(
      map(accesstoken=>{
        this.input_accesstoken = accesstoken;
        // console.log(this.input_idtoken);
        return accesstoken;
      }),    
      switchMap(accesstoken =>{  // we want to emit the results of another observable
        // console.log(idtoken);
        const payload =  {
          accesstoken: accesstoken
        }
        return this.http.post<any>('http://localhost:3000/verify/accesstoken', payload)
      })
     ).subscribe(claims =>{
        this.verified_accesstoken = claims;
        console.log(this.verified_accesstoken)
     })
    // this.claims_subscription = this.tokenservice.getClaimSourceListener()
    // .subscribe((claims)=>{
    //   this.claims = claims;
    // })
  }

  // public tokenVerify(token: string){
  //   this.http.post<any>('http://localhost:3000/verify',this.token)
  //     .subscribe(results =>{
  //       // this.claims = results;
  //       this.tokenservice.addClaims(results);
  //     })

  // }

  public ngOnDestroy(): void {
    this.idtokensubscription.unsubscribe();
    this.accesstokensubscription.unsubscribe();
  }

}
