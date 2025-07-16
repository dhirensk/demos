import { Injectable } from '@angular/core';
import { BehaviorSubject, Subject } from 'rxjs';
import { MatTableDataSource } from '@angular/material/table';


@Injectable({
  providedIn: 'root'
})
export class DataService {

  public tokenData = new BehaviorSubject<any[]>([]);
  public accessTokenClaimsData = new BehaviorSubject<any[]>([]);
  public idTokenClaimsData = new BehaviorSubject<any[]>([]);

  constructor() { }

  getTokenDataListener(){
    return this.tokenData.asObservable();
  };

  getAccessTokenClaimsDataListener(){
    return this.accessTokenClaimsData.asObservable();
  }
  getIdTokenClaimsDataListener(){
    return this.idTokenClaimsData.asObservable();
  }

}
