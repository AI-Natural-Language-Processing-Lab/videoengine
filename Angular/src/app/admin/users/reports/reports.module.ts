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
import { RouterModule } from "@angular/router";
/* custom component */
import { UserReportsComponent } from "./reports.components";

/* services */
import { SettingsService } from "../../../admin/users/services/settings.service";
import { DataService } from "../../../admin/users/services/data.service";
import { FormService } from "../../../admin/users/services/form.service";

/* actions */
import { UserAPIActions } from "../../../reducers/users/actions";
import { PartialModule } from "../../../partials/shared.module";

import { Ng2GoogleChartsModule } from "ng2-google-charts";

@NgModule({
  imports: [
    CommonModule,
    PartialModule,
    RouterModule,
    FormsModule,
    Ng2GoogleChartsModule
  ],
  declarations: [UserReportsComponent],
  exports: [UserReportsComponent],
  providers: [SettingsService, DataService, FormService, UserAPIActions]
})
export class UserReportModule {}
