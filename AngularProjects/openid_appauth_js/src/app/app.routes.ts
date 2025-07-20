// src/app/app.routes.ts
import { Routes } from '@angular/router';
import {AuthGuard} from './auth.guard'

export const routes: Routes = [
    // lazy loading all components.
    { path: 'login', loadComponent: ()=> import('./login/login.component').then(c => c.LoginComponent)},
    { path: 'callback', loadComponent: ()=> import('./callback/callback.component').then(c => c.CallbackComponent), canActivate: [AuthGuard]},
    // { path: 'callback', redirectTo: 'callback'}
];


