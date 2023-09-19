import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './home/login.component';
import { OktaCallbackComponent } from '@okta/okta-angular';
import { SignoutpageComponent } from './signoutpage/signoutpage.component';
import { ValidatetokenComponent } from './validatetoken/validatetoken.component';


const routes: Routes = [
 {path: '', component: LoginComponent},
 { path: 'login/callback', component: OktaCallbackComponent },
 { path: 'signout', component: SignoutpageComponent},
 { path: 'validate', component:ValidatetokenComponent}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
