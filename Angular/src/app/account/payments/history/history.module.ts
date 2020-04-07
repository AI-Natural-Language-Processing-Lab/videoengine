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
import { HistoryComponent } from "./history.component";
import { PartialModule } from "../../../partials/shared.module";

import { DataService } from "../../../account/payments/history/services/data.service";
import { SettingsService } from "../../../account/payments/history/services/settings.service";
import { HistoryAPIActions } from "../../../reducers/account/history/actions";

@NgModule({
  imports: [CommonModule, PartialModule, FormsModule, RouterModule],
  declarations: [HistoryComponent],
  exports: [HistoryComponent],
  providers: [DataService, SettingsService, HistoryAPIActions]
})
export class HistoryModule {}
