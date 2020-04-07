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

import { SetupComponent } from "./index.component";

import { PartialModule } from "../../partials/shared.module";
import { SharedSetupModule } from "../../shared/setup/shared.module";
/* 
// injectors and services
import { SettingsService } from "../../admin/settings/configurations/services/settings.service";
import { DataService } from "../../admin/settings/configurations/services/data.service";
import { FormService } from "../../admin/settings/configurations/services/form.service";

// shared services
import { CoreService } from "../../admin/core/coreService";
import { CoreAPIActions } from "../../reducers/core/actions";

// reducer actions
import { ConfigurationsAPIActions } from "../../reducers/settings/configurations/actions";
*/
const routes: Routes = [
  {
    path: "",
    data: {
      title: "Setup Application",
      urls: [{ title: "Setup Application" }]
    },
    component: SetupComponent
  }
];

@NgModule({
  imports: [
    FormsModule,
    CommonModule,
    PartialModule,
    RouterModule.forChild(routes),
    SharedSetupModule
  ],
  declarations: [SetupComponent],
  /*providers: [
    SettingsService,
    DataService,
    FormService,
    CoreService,
    CoreAPIActions,
    ConfigurationsAPIActions
  ]*/
})
export class SetupModule {}
