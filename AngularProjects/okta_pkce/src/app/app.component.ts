import { Component, Inject, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { OktaAuthStateService, OKTA_AUTH } from '@okta/okta-angular';
import { AuthState, OktaAuth } from '@okta/okta-auth-js';
import { filter, map, Observable } from 'rxjs';



@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})

export class AppComponent implements OnInit {

  public isAuthenticated$!: Observable<boolean>;

  constructor(private _router: Router, private _oktaStateService: OktaAuthStateService, @Inject(OKTA_AUTH) private _oktaAuth: OktaAuth) { }

  public ngOnInit(): void {
    this.isAuthenticated$ = this._oktaStateService.authState$.pipe(
      filter((s: AuthState) => !!s),
      map((s: AuthState) => s.isAuthenticated ?? false)
    );
  }

  public async signIn() : Promise<void> {
    await this._oktaAuth.signInWithRedirect({
      // https://openid.net/specs/openid-connect-core-1_0.html#NonceNotes
      // for demo purpose we will set a fixed nonce, otherwise a randomid will be generated.
      nonce: "this_is_a_test_nonce_999999" 
    });
  }

  public async signOut(): Promise<void> {
    await this._oktaAuth.signOut();
  }
}
