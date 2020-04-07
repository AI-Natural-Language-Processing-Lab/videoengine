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
import { ProcRoleComponent } from "./process.component";

/* services */
import { SettingsService } from "../services/settings.service";
import { DataService } from "../services/data.service";
import { FormService } from "../services/form.service";
/* actions */
import { ROLEAPIActions } from "../../../../reducers/settings/roles/actions";

import { PartialModule } from "../../../../partials/shared.module";

@NgModule({
  imports: [CommonModule, PartialModule, FormsModule],
  declarations: [ProcRoleComponent],
  exports: [ProcRoleComponent],
  providers: [SettingsService, DataService, FormService, ROLEAPIActions]
})
export class ProcRoleModule {}
