/* -------------------------------------------------------------------------- */
/*                          Product Name: VideoEngine                         */
/*                            Author: Mediasoftpro                            */
/*                       Email: support@mediasoftpro.com                      */
/*       License: Read license.txt located on root of your application.       */
/*                     Copyright 2007 - 2020 @Mediasoftpro                    */
/* -------------------------------------------------------------------------- */
import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { FormsModule } from "@angular/forms";
import { Routes, RouterModule } from "@angular/router";

import { ManageAccountComponent } from "./manageaccount.component";
import { PartialModule } from "../../partials/shared.module";

import { NavigationMenuIndex } from "../../configs/settings";

const routes: Routes = [
  {
    path: "",
    data: {
      topmenuIndex: NavigationMenuIndex.TOPMENU_SETTINGS_INDEX,
      leftmenuIndex: NavigationMenuIndex.SETTINGS_MANAGE_ACCOUNT_INDEX,
      title: "My Account",
      urls: [{ title: "My Account", url: "/" }, { title: "Manage Account" }]
    },
    component: ManageAccountComponent
  }
];

@NgModule({
  imports: [
    FormsModule,
    CommonModule,
    PartialModule,
    RouterModule.forChild(routes)
  ],
  declarations: [ManageAccountComponent]
})
export class ManageAccountModule {}
