import { Injectable, signal, computed } from '@angular/core';
import {
  AuthorizationServiceConfiguration,
  AuthorizationServiceConfigurationJson,
  AuthorizationRequest,
  AuthorizationRequestHandler,
  AuthorizationNotifier,
  RedirectRequestHandler,
  Requestor,
  log,
  DefaultCrypto,
  LocalStorageBackend,
  BaseTokenRequestHandler,
  TokenRequest,
  GRANT_TYPE_AUTHORIZATION_CODE,
  StringMap,
  TokenResponse
} from '@openid/appauth';
import { BehaviorSubject, Subject } from 'rxjs';
import { NoHashQueryStringUtils } from './utils';  // https://github.com/openid/AppAuth-JS/issues/195#issuecomment-953363001
import { environment } from '../environments/environment';


@Injectable({
  providedIn: 'root'
})


export class AuthorizationService {
  private notifier: AuthorizationNotifier;
  private authorizationHandler: AuthorizationRequestHandler;
  private authorizationcode!: string;
  private issuer: string = environment.oauthConfig.issuer;
  private scope: string = environment.oauthConfig.scopes;
  private redirect_uri = environment.oauthConfig.redirect_uri
  private client_id = environment.oauthConfig.client_id
  private client_secret = environment.oauthConfig.client_secret
  private pfidpadapterid = environment.oauthConfig.pfidpadapterid
  private configurationListener = new Subject<AuthorizationServiceConfiguration>();
  private configuration!: AuthorizationServiceConfiguration;
  private tokenResponseListener = new Subject<TokenResponse>();
  public authenticated = signal<boolean>(false);
  // private configuration: AuthorizationServiceConfiguration;

  constructor(private requestor: Requestor) {

    this.notifier = new AuthorizationNotifier();
    this.authorizationHandler = new RedirectRequestHandler(new LocalStorageBackend(), new NoHashQueryStringUtils(), window.location, new DefaultCrypto());
    this.authorizationHandler.setAuthorizationNotifier(this.notifier);

  }

  private async getConfiguration() {
    console.log("in async block");
    const response = await AuthorizationServiceConfiguration.fetchFromIssuer(this.issuer, this.requestor);
    console.log(response);
    window.localStorage.setItem('AuthServiceConfiguration', JSON.stringify(response.toJson()))
    return response;
  };

  public onSignIn() {
    let isAuthenticated = computed<boolean>(() => this.authenticated());
    if (isAuthenticated() == false) {
      this.getConfiguration().then(configuration => {
        this.configurationListener.next(configuration);
        // this configuration is lost after the redirect and service is not having any data.
        // this.configuration = configuration;  
        // console.log(this.configuration);
        const extras: StringMap = {'prompt': 'consent', 'access_type': 'offline'};
        if (environment.oauthConfig.pfidpadapterid){
          extras['pfidpadapterid'] = environment.oauthConfig.pfidpadapterid
        }

        let request = new AuthorizationRequest({
          client_id: this.client_id,
          redirect_uri: this.redirect_uri,
          scope: this.scope,
          response_type: AuthorizationRequest.RESPONSE_TYPE_CODE,
          state: undefined,
          extras: extras
        });
        this.authorizationHandler.performAuthorizationRequest(configuration, request);
        // this will redirect the page to callback path if successful and returns the code param
      })
    }


  };

  public makeTokenRequest() {
    return new Promise((resolve, reject) => {
      this.authorizationHandler.completeAuthorizationRequestIfPossible(); //important to run this step to return the notifier listeners.
      this.notifier.setAuthorizationListener((request, response, error) => {
        //  log("test", request, response,error);
        console.log('Authorization request complete ', request, response, error);
        if (response) {
          this.authorizationcode = response.code;
          const tokenHandler = new BaseTokenRequestHandler(this.requestor);
          const extras: StringMap = {};
          // extras['client_secret'] = this.client_secret;
          if (request.internal) {
            extras['code_verifier'] = request.internal['code_verifier'];
          }

          const tokenRequest = new TokenRequest({
            client_id: this.client_id,
            redirect_uri: this.redirect_uri,
            grant_type: GRANT_TYPE_AUTHORIZATION_CODE,
            code: response.code,
            extras: extras
          });
          console.dir(tokenRequest);
          // console.log(this.configuration);
          let authConfigurationjson: AuthorizationServiceConfigurationJson = JSON.parse(window.localStorage.getItem('AuthServiceConfiguration')!)
          let authConfiguration: AuthorizationServiceConfiguration = new AuthorizationServiceConfiguration(authConfigurationjson)
          // console.log(authConfiguration);
          tokenHandler.performTokenRequest(authConfiguration, tokenRequest).then((response) => {
            // console.log(response);
            this.tokenResponseListener.next(response);
          })
            .catch((error) => {
              console.log(error);
            });
        }
      });
    })


  }
  public getTokenResponseListener() {
    return this.tokenResponseListener.asObservable()
  }

}
