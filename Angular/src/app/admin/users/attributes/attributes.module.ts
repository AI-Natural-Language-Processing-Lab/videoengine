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

import { UserProfileAttributesComponent } from "./attributes.component";

// shared modules
import { PartialModule } from "../../../partials/shared.module";
import { AttrModule } from "../../../shared/attr/attr.module";


@NgModule({
  imports: [CommonModule, PartialModule, FormsModule, RouterModule, AttrModule],
  declarations: [UserProfileAttributesComponent],
  exports: [UserProfileAttributesComponent],
})
export class UserProfileAttributesModule {}
