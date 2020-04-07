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

import { MailTemplatesComponent } from "./mailtemplates.component";
import { ListComponent } from "./partials/list.component";

/* services */
import { SettingsService } from "./services/settings.service";
import { DataService } from "./services/data.service";

/* actions */
import { MailTemplatesAPIActions } from "../../../reducers/settings/mailtemplates/actions";

import { PartialModule } from "../../../partials/shared.module";

@NgModule({
  imports: [CommonModule, PartialModule, FormsModule, RouterModule],
  declarations: [MailTemplatesComponent, ListComponent],
  exports: [MailTemplatesComponent],
  providers: [SettingsService, DataService, MailTemplatesAPIActions]
})
export class MailTemplatesModule {}
