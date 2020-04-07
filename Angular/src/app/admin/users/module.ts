/* -------------------------------------------------------------------------- */
/*                          Product Name: VideoEngine                         */
/*                            Author: Mediasoftpro                            */
/*                       Email: support@mediasoftpro.com                      */
/*       License: Read license.txt located on root of your application.       */
/*                     Copyright 2007 - 2020 @Mediasoftpro                    */
/* -------------------------------------------------------------------------- */

import { NgModule } from "@angular/core";
import { NgbModule } from "@ng-bootstrap/ng-bootstrap";
import { CommonModule } from "@angular/common";
import { FormsModule } from "@angular/forms";
import { Routes, RouterModule } from "@angular/router";

// components
import { UsersComponent } from "./index";
import { UserProfileComponent } from "./profile/process.component";
import { UserReportsComponent } from "./reports/reports.components";
import { UserProfileSettingsComponent } from "./settings/settings.component";
import { UserProfileAttributesComponent } from "./attributes/attributes.component";

import { ViewComponent } from "./partials/modal";

/* modules */
import { UserProfileModule } from "./profile/process.module";
import { UserProfileSettingsModule } from "./settings/settings.module";
import { UserProfileAttributesModule } from "./attributes/attributes.module";
import { UserReportModule } from "./reports/reports.module";

// shared modules
import { PartialModule } from "../../partials/shared.module";
import { SharedUsersModule } from "./shared.module";

const routes: Routes = [
  {
    path: "",
    data: {
      title: "Member Management",
      urls: [
        { title: "Dashboard", url: "/" },
        { title: "Users", url: "/users" },
        { title: "Management" }
      ]
    },
    component: UsersComponent
  },
  {
    path: "profile/:id",
    data: {
      title: "User Profile",
      urls: [
        { title: "Dashboard", url: "/" },
        { title: "Users", url: "/users" },
        { title: "User profile" }
      ]
    },
    component: UserProfileComponent
  },
  {
    path: "reports",
    data: {
      title: "Report Overview",
      urls: [
        { title: "Dashboard", url: "/" },
        { title: "Users", url: "/users" },
        { title: "Report Overview" }
      ]
    },
    component: UserReportsComponent
  },
  {
    path: "attributes/:id",
    data: {
      title: "Manage User Profile Attributes",
      urls: [
        { title: "Dashboard", url: "/" },
        { title: "Users", url: "/users" },
        { title: "Settings", url: "/users/settings" },
        { title: "Manage User Profile Attributes" }
      ]
    },
    component: UserProfileAttributesComponent
  },
  {
    path: "settings",
    data: {
      title: "Manage User Profile Settings",
      urls: [
        { title: "Dashboard", url: "/" },
        { title: "Users", url: "/users" },
        { title: "Settings" }
      ]
    },
    component: UserProfileSettingsComponent
  }
];

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    PartialModule,
    UserProfileModule,
    UserProfileSettingsModule,
    UserProfileAttributesModule,
    UserReportModule,
    SharedUsersModule,
    NgbModule,
    RouterModule.forChild(routes)
  ],
  declarations: [UsersComponent, ViewComponent],
  entryComponents: [ViewComponent],
  exports: [UsersComponent]
})
export class UserModule {}
