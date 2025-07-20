import { Component, inject, OnInit } from '@angular/core';
import { AuthorizationService } from '../authorization.service';
import { DataService } from '../data.service';
import { jwtDecode, JwtPayload } from 'jwt-decode'
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { NgFor, NgIf } from '@angular/common';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-callback',
  imports: [MatTableModule, NgFor, NgIf],
  templateUrl: './callback.component.html',
  styleUrl: './callback.component.scss',
  standalone: true
})
export class CallbackComponent implements OnInit {
  private authorizationService = inject(AuthorizationService);
  private dataService = inject(DataService);
  public code: any;
  public tokenResponse!: any;
  public accessTokenClaims!: any;
  // public dataSource: MatTableDataSource<any[]> = new MatTableDataSource<any[]>([]);
  // public claimsDataSource: MatTableDataSource<any[]> = new MatTableDataSource<any[]>([]);
  public dataSourceToken: MatTableDataSource<any> = new MatTableDataSource<any>();
  public dataSourceAccessTokenClaims: MatTableDataSource<any> = new MatTableDataSource<any>();
  public dataSourceIdTokenClaims: MatTableDataSource<any> = new MatTableDataSource<any>();
  public tokenData: any[] = [];
  // public unverifiedAccessTokenClaims: any[]= [];
  public displayedColumns = ['key', 'value'];
  public accessTokenClaimsAvailable:boolean = false;
  public idTokenClaimsAvailable:boolean = false;
  public tokenAvailable:boolean = false;
  public isAuthenticated:boolean = false;
  constructor(private activatedRoute: ActivatedRoute) { }

  ngOnInit(): void {

    this.dataService.getTokenDataListener().subscribe((data:any)=>{
      this.dataSourceToken.data = data;
      this.tokenAvailable = true;
    });

    this.dataService.getAccessTokenClaimsDataListener().subscribe((data:any)=>{
      this.dataSourceAccessTokenClaims = data;
      this.accessTokenClaimsAvailable = true;
    });

    this.dataService.getIdTokenClaimsDataListener().subscribe((data:any)=>{
      this.dataSourceIdTokenClaims = data;
      this.idTokenClaimsAvailable = true;
    });  
     
    this.activatedRoute.queryParamMap.subscribe((values)=>{
      if (values.has('code')){
     // this.code = new URL(window.location.href).searchParams.get('code')

      // subscribe and make a tokenrequest for exchange of authorization code with jwt
      this.authorizationService.getTokenResponseListener().subscribe((response) => {
        //set the signal that the user is now authenticated.
        this.authorizationService.authenticated.set(true);
        this.tokenResponse = response.toJson();
        // const data: any[] = [];
        for (let item in this.tokenResponse) {
          this.tokenData.push({ key: item, value: this.tokenResponse[item] })
        }
        this.dataService.tokenData.next(this.tokenData);
        // this.dataSource.data = this.tokenData;
        //  console.log(this.dataSource);
        let accessTokenClaims:any = jwtDecode(response.accessToken)
        let unverifiedAccessTokenClaims: any[] = []
        for(let key in accessTokenClaims){
          let value:any = accessTokenClaims[key];
          if (Array.isArray(value)){
            value = JSON.stringify(value);
          }
          unverifiedAccessTokenClaims.push({key: key, value: value});
        };
        this.dataService.accessTokenClaimsData.next(unverifiedAccessTokenClaims)

        if(response.idToken){
          let unverifiedIdTokenClaims: any[] = []
          let idTokenClaims:any = jwtDecode(response.idToken)

          for( let key in idTokenClaims){
            let value:any = idTokenClaims[key];
            if ( Array.isArray(value)){
              value = JSON.stringify(value);
            }
            unverifiedIdTokenClaims.push({key: key, value: value});
          }
          this.dataService.idTokenClaimsData.next(unverifiedIdTokenClaims)
        }


        // this.claimsDataSource.data = this.unverifiedTokenClaims;
        // this.claimsavailable = true;
        // console.log(unverifiedTokenClaims);
        
      });
      console.log("sending token request");
      this.authorizationService.makeTokenRequest();
      }
    });

  }

}

