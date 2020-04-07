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

import { UserProfileComponent } from "./process.component";
import { UserRoleComponent } from "./partials/role.component";
/* services */
import { SettingsService } from "../services/settings.service";
import { DataService } from "../services/data.service";
import { FormService } from "../services/form.service";
/* actions */
import { UserAPIActions } from "../../../reducers/users/actions";

/* cropper */
//import { BannerUploaderComponent } from "../../../shared/cropie/uploader";

/* shared modules */
import { PartialModule } from "../../../partials/shared.module";

@NgModule({
  imports: [CommonModule, PartialModule, FormsModule],
  declarations: [
    UserProfileComponent,
    UserRoleComponent,
    //BannerUploaderComponent
  ],
  exports: [UserProfileComponent],
  providers: [SettingsService, DataService, FormService, UserAPIActions]
})
export class UserProfileModule {}
