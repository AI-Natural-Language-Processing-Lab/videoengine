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

import { DictionaryComponent } from "./dictionary.component";
import { ListComponent } from "./partials/list.component";
import { ViewComponent } from "./partials/modal.component";

/* services */
import { SettingsService } from "./services/settings.service";
import { DataService } from "./services/data.service";

/* actions */
import { DictionaryAPIActions } from "../../../reducers/settings/dictionary/actions";

import { PartialModule } from "../../../partials/shared.module";

@NgModule({
  imports: [CommonModule, PartialModule, FormsModule],
  declarations: [DictionaryComponent, ListComponent, ViewComponent],
  entryComponents: [ViewComponent],
  exports: [DictionaryComponent],
  providers: [SettingsService, DataService, DictionaryAPIActions]
})
export class DictionaryModule {}
