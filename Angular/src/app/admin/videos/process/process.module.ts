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

/* custom component */
import { VideoProcessComponent } from "./process.component";


// shared modules
import { PartialModule } from "../../../partials/shared.module";
import { SharedVideoModule } from "../../../shared/videos/shared.module";

@NgModule({
  imports: [
    CommonModule,
    PartialModule,
    RouterModule,
    FormsModule,
    SharedVideoModule
  ],
  declarations: [VideoProcessComponent],
  exports: [VideoProcessComponent]
})
export class VideoProcessModule {}
