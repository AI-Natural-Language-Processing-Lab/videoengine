/* -------------------------------------------------------------------------- */
/*                          Product Name: VideoEngine                         */
/*                            Author: Mediasoftpro                            */
/*                       Email: support@mediasoftpro.com                      */
/*       License: Read license.txt located on root of your application.       */
/*                     Copyright 2007 - 2020 @Mediasoftpro                    */
/* -------------------------------------------------------------------------- */

import { Injectable } from "@angular/core";
import {
  CanActivate,
  ActivatedRouteSnapshot,
  RouterStateSnapshot
} from "@angular/router";
import { Observable } from "rxjs/Observable";
import { Router } from "@angular/router";
import { select } from "@angular-redux/store";

@Injectable()
export class JMediaLazyGuard implements CanActivate {
  @select(["users", "auth"])
  readonly auth$: Observable<any>;

  isAuth: any;

  constructor(private router: Router) {
    this.auth$.subscribe(auth => {
      this.isAuth = auth;
    });
  }

  canActivate(
    next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): Observable<boolean> | Promise<boolean> | boolean {
    console.log("reached");
    console.log(this.isAuth);
    if (this.isAuth.isAuthenticated) {
      return true;
    } else {
      this.router.navigate([""]);
      return false;
    }
  }
}