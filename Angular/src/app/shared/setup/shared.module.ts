/* -------------------------------------------------------------------------- */
/*                          Product Name: VideoEngine                         */
/*                            Author: Mediasoftpro                            */
/*                       Email: support@mediasoftpro.com                      */
/*       License: Read license.txt located on root of your application.       */
/*                     Copyright 2007 - 2020 @Mediasoftpro                    */
/* -------------------------------------------------------------------------- */

import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { RouterModule } from "@angular/router";

// shared modules
import { PartialModule } from "../../partials/shared.module";

// components
import { MainSetupComponent } from "./components/setup.component";

// injectors and services
import { SettingsService } from "../../admin/settings/configurations/services/settings.service";
import { DataService } from "../../admin/settings/configurations/services/data.service";
import { FormService } from "../../admin/settings/configurations/services/form.service";

// shared services
import { CoreService } from "../../admin/core/coreService";
import { CoreAPIActions } from "../../reducers/core/actions";

// reducer actions
import { ConfigurationsAPIActions } from "../../reducers/settings/configurations/actions";


@NgModule({
  imports: [
    CommonModule,
    PartialModule,
    RouterModule,
    FormsModule,
    ReactiveFormsModule
  ],
  declarations: [
    MainSetupComponent
  ],
  exports: [
    MainSetupComponent
  ],
  providers: [SettingsService, DataService, FormService, ConfigurationsAPIActions, CoreService, CoreAPIActions]
})
export class SharedSetupModule {}
