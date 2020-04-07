
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
import { Routes, RouterModule } from "@angular/router";

import { NotFoundComponent } from "./notfound.component";
import { PartialModule } from "../../partials/shared.module";

const routes: Routes = [
  {
    path: "",
    data: {
      title: "Page Not Found",
      urls: [{ title: "Page Not Found" }]
    },
    component: NotFoundComponent
  }
];

@NgModule({
  imports: [
    FormsModule,
    CommonModule,
    PartialModule,
    RouterModule.forChild(routes)
  ],
  declarations: [NotFoundComponent]
})
export class NotFoundModule {}
