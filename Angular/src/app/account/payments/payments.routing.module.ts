/* -------------------------------------------------------------------------- */
/*                          Product Name: VideoEngine                         */
/*                            Author: Mediasoftpro                            */
/*                       Email: support@mediasoftpro.com                      */
/*       License: Read license.txt located on root of your application.       */
/*                     Copyright 2007 - 2020 @Mediasoftpro                    */
/* -------------------------------------------------------------------------- */

import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";

import { PaymentsComponent } from "./payments.components";
import { MyPurchasesComponent } from "./mypurchases/mypurchases.component";
import { HistoryComponent } from "./history/history.component";
import { PackagesComponent } from "./packages/packages.component";

import { NavigationMenuIndex } from "../../configs/settings";

const settingRoutes: Routes = [
  {
    path: "",
    component: PaymentsComponent,
    data: {
      topmenuIndex: NavigationMenuIndex.TOPMENU_SETTINGS_INDEX,
      leftmenuIndex: NavigationMenuIndex.SETTINGS_MYPURCHASE_INDEX,
      title: "My Account",
      urls: [
        { title: "My Account", url: "/" },
        { title: "Payments", url: "/payments" },
        { title: "My Purchases" }
      ]
    },
    children: [
      { path: "", component: MyPurchasesComponent },
      {
        path: "history",
        children: [{ path: "", component: HistoryComponent }],
        data: {
          topmenuIndex: NavigationMenuIndex.TOPMENU_SETTINGS_INDEX,
          leftmenuIndex: NavigationMenuIndex.SETTINGS_MYPURCHASE_INDEX,
          title: "My Account",
          urls: [
            { title: "My Account", url: "/" },
            { title: "Payments", url: "/payments" },
            { title: "History" }
          ]
        }
      },
      {
        path: "packages",
        children: [{ path: "", component: PackagesComponent }],
        data: {
          topmenuIndex: NavigationMenuIndex.TOPMENU_SETTINGS_INDEX,
          leftmenuIndex: NavigationMenuIndex.SETTINGS_MYPURCHASE_INDEX,
          title: "My Account",
          urls: [
            { title: "My Account", url: "/" },
            { title: "Payments", url: "/payments" },
            { title: "Packages" }
          ]
        }
      }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(settingRoutes)],
  exports: [RouterModule]
})
export class PaymentRoutingModule {}
