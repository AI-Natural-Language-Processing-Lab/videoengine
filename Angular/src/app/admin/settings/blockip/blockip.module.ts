
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

import { BlockIPComponent } from "./blockip.component";
import { ListComponent } from "./partials/list.component";

/* services */
import { SettingsService } from "./services/settings.service";
import { DataService } from "./services/data.service";
import { ViewComponent } from "./partials/modal.component";
/* actions */
import { BlockIPAPIActions } from "../../../reducers/settings/blockip/actions";
import { PartialModule } from "../../../partials/shared.module";

@NgModule({
  imports: [CommonModule, PartialModule, FormsModule],
  declarations: [BlockIPComponent, ListComponent, ViewComponent],
  entryComponents: [ViewComponent],
  exports: [BlockIPComponent],
  providers: [SettingsService, DataService, BlockIPAPIActions]
})
export class BlockIPModule {}
