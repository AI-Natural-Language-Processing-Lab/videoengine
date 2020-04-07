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

import { DashboardComponent } from "./dashboard.component";
import { AdminDashboardComponent } from "./partials/admin-dashboard";
import { AccountDashboardComponent } from "./partials/account-dashboard";
import { SetupDashboardComponent } from "./partials/setup-dashboard";

import { PartialModule } from "../../partials/shared.module";

// search navigation
import { SearchComponent } from './search/search-app';
import { VideoSearchComponent } from './search/videos/videos.component';
import { UsersSearchComponent } from './search/users/users.component';
import { NavigationComponent } from "./search/partials/navigation.component";

// search list modules
import { SharedVideoModule } from "../../shared/videos/shared.module";
import { SharedUsersModule } from "../../admin/users/shared.module";
import { NavigationMenuIndex } from "../../configs/settings";
const routes: Routes = [
  {
    path: '',
    data: {
      title: "Account",
      topmenuIndex: NavigationMenuIndex.TOPMENU_SETTINGS_INDEX,
      leftmenuIndex: NavigationMenuIndex.SETTINGS_OVERVIEW_INDEX,
      urls: [
        { title: "Account Overview" }
      ]
    },
    component: DashboardComponent
  }
];

@NgModule({
  imports: [
    FormsModule,
    CommonModule,
    PartialModule,
    SharedVideoModule,
    SharedUsersModule,
    RouterModule.forChild(routes)
  ],
  declarations: [
    DashboardComponent,
    AdminDashboardComponent,
    AccountDashboardComponent,
    SetupDashboardComponent,
    SearchComponent,
    NavigationComponent,
    VideoSearchComponent,
    UsersSearchComponent
  ]
})
export class DashboardModule {}
