import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';


@Injectable({
  providedIn: 'root'
})
export class TokenService {
  public idtoken: string = "";
  public accesstoken: string = ""
  public idtokenclaims: any;
  public accesstokenclaims: any;
  private idtokenSource = new BehaviorSubject<string>("");
  private accesstokenSource = new BehaviorSubject<string>("");
  private idtokenclaimSource = new BehaviorSubject<any>(null);
  private accesstokenclaimSource = new BehaviorSubject<any>(null);

  getIdToken(){
    return this.idtoken;
  }

  getAccessToken(){
    return this.accesstoken;
  }

  getidtokenClaims(){
    return this.idtokenclaims
  }

  getaccesstokenClaims(){
    return this.accesstokenclaims
  }  

  getAccessTokenSourceListener(){
    return this.accesstokenSource.asObservable();
  }
  getIdTokenSourceListener(){
    return this.idtokenSource.asObservable();
  }

  getIdTokenClaimSourceListener(){
    return this.idtokenclaimSource.asObservable();
  }

  getAccessTokenClaimSourceListener(){
    return this.accesstokenclaims.asObservable();
  }

  addIdToken(idtoken: string){
    this.idtokenSource.next(idtoken);
    console.log("from service:" + idtoken);
    this.idtoken = idtoken;
  }


  addAccessToken(accesstoken: string){
    this.accesstokenSource.next(accesstoken);
    console.log("from service:" + accesstoken);
    this.accesstoken = accesstoken;
  }  


  addIdTokenClaims(idtokenclaims: any){
    this.idtokenclaimSource.next(idtokenclaims);
    this.idtokenclaims = idtokenclaims;
    console.log("from service:"+ idtokenclaims.nonce);
  }

  addAccessTokenClaims(accesstokenclaims: any){
    this.accesstokenclaimSource.next(accesstokenclaims);
    this.accesstokenclaims = accesstokenclaims;
    console.log("from service:"+ accesstokenclaims.nonce);
  }


}
