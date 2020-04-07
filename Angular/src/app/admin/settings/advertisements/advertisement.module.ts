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

import { AdvertisemntComponent } from "./advertisement.component";
import { ListComponent } from "./partials/list.component";

/* services */
import { SettingsService } from "./services/settings.service";
import { DataService } from "./services/data.service";

/* actions */
import { AdvertisementAPIActions } from "../../../reducers/settings/advertisements/actions";

import { PartialModule } from "../../../partials/shared.module";

@NgModule({
  imports: [CommonModule, PartialModule, FormsModule],
  declarations: [AdvertisemntComponent, ListComponent],
  exports: [AdvertisemntComponent],
  providers: [SettingsService, DataService, AdvertisementAPIActions]
})
export class AdverisementModule {}
