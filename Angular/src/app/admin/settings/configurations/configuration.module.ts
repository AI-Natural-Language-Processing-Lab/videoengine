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

import { ConfigurationComponent } from "./configuration.component";

/* services */
import { SettingsService } from "./services/settings.service";
import { DataService } from "./services/data.service";
import { FormService } from "./services/form.service";
/* actions */
import { ConfigurationsAPIActions } from "../../../reducers/settings/configurations/actions";

import { PartialModule } from "../../../partials/shared.module";

@NgModule({
  imports: [CommonModule, PartialModule, FormsModule],
  declarations: [ConfigurationComponent],
  exports: [ConfigurationComponent],
  providers: [
    SettingsService,
    DataService,
    FormService,
    ConfigurationsAPIActions
  ]
})
export class ConfigurationsModule {}
