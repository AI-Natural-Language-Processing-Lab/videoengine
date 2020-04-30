/* -------------------------------------------------------------------------- */
/*                          Product Name: VideoEngine                    */
/*                            Author: Mediasoftpro                            */
/*                       Email: support@mediasoftpro.com                      */
/*       License: Read license.txt located on root of your application.       */
/*                     Copyright 2007 - 2020 @Mediasoftpro                    */
/* -------------------------------------------------------------------------- */

import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { RouterModule } from "@angular/router";
import { PartialModule } from "../../partials/shared.module";

// components
import { DynamicAttributesComponent } from "../../shared/attr/attributes/attributes.component";
import { DynamicSettingsComponent } from "../../shared/attr/settings/settings.component";
import { ViewComponent } from "../../shared/attr/partials/modal.component";

// services
import { SettingsService } from "../../shared/attr/services/settings.service";
import { FormService } from "../../shared/attr/services/form.service";
import { DataService } from "../../shared/attr/services/data.service";

@NgModule({
  imports: [
    CommonModule,
    PartialModule,
    RouterModule,
    FormsModule,
    ReactiveFormsModule
  ],
  declarations: [DynamicAttributesComponent,DynamicSettingsComponent,ViewComponent],
  entryComponents: [ViewComponent],
  exports: [DynamicAttributesComponent,DynamicSettingsComponent],
  providers: [SettingsService, FormService, DataService]
})
export class AttrModule {}
