import { Injectable } from "@angular/core";
import { CanActivate, ParamMap, Params } from "@angular/router";
import { ActivatedRouteSnapshot, RouterStateSnapshot } from "@angular/router";
import { MaybeAsync } from "@angular/router";
import { GuardResult } from "@angular/router";

@Injectable({
    providedIn: 'root'
})
class UserToken {}
@Injectable({
    providedIn: 'root'
})
class UserPermissions {
  canActivate(currentUser: UserToken, routeconfig: any ): boolean {
    console.log(routeconfig)
    if (routeconfig.path == 'create'){
        console.log(true)
        return true;
    }
    // console.log(false)
    return true;
  }
}
@Injectable({
    providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  constructor(private permissions: UserPermissions, private currentUser: UserToken) {}
  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): MaybeAsync<GuardResult> {
    return this.permissions.canActivate(this.currentUser, route.routeConfig);
  }
}
