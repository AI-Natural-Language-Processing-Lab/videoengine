
/* -------------------------------------------------------------------------- */
/*                          Product Name: VideoEngine                         */
/*                            Author: Mediasoftpro                            */
/*                       Email: support@mediasoftpro.com                      */
/*       License: Read license.txt located on root of your application.       */
/*                     Copyright 2007 - 2020 @Mediasoftpro                    */
/* -------------------------------------------------------------------------- */

import { Injectable } from "@angular/core";

/* core service responsible for handling user roles & permissions in whole website resources */
@Injectable()
export class PermissionService {
  // access id for main objects (access ids can be generated at time of creating objects via settings -> roles -> permission objects)
  ADMIN_FULLACCESS_ACCESSID = "1521140258949";
  ADMIN_READONLY_ACCESSID = "1521143471417";

  // Grand resource access will match
  // i: currently logged in user have admin full access
  // ii: if not logged in user have Admin readonly access
  // iii: if not logged in user have specific resource full access
  // iv: if not logged in user have specific resource readonly access

  GrandResourceAccess(
    isPublic: boolean,
    FullAcessID: string,
    ReadOnlyAccessID: string,
    Role: any
  ) {
    if (isPublic) {
      return true;
    }
    if (Role.length === 0) {
      return false;
    }
    let grantaccess = false;
    for (const item of Role) {
      if (item === this.ADMIN_FULLACCESS_ACCESSID) {
        grantaccess = true;
      } else if (item === this.ADMIN_READONLY_ACCESSID) {
        grantaccess = true; // access granted
      } else if (item === FullAcessID) {
        grantaccess = true; // access granted
      } else if (item === ReadOnlyAccessID) {
        grantaccess = true; // access granted
      }
    }
    return grantaccess;
  }

  // GrantResourceAction will check
  // whether user have sufficient right to execute action.
  // for executing action, user should have at least resrouce full access
  GrandResourceAction(FullAccessID, Role: any) {
    if (Role.length === 0) {
      return false;
    }
    let grantaccess = false;
    for (const item of Role) {
      if (item === this.ADMIN_FULLACCESS_ACCESSID) {
        grantaccess = true;
      } else if (item === FullAccessID) {
        grantaccess = true; // access granted
      }
    }
    return grantaccess;
  }
}
