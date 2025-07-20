import { Component, OnInit } from '@angular/core';
import { AuthorizationService } from '../authorization.service';
import { inject } from '@angular/core';

@Component({
  selector: 'app-login',
  imports: [],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss',
  standalone: true
})
export class LoginComponent implements OnInit {

  public authorizationservice = inject(AuthorizationService);
   constructor(){}

   ngOnInit(): void {
     
     this.authorizationservice.onSignIn();
   }

}
