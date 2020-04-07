/* -------------------------------------------------------------------------- */
/*                          Product Name: VideoEngine                         */
/*                            Author: Mediasoftpro                            */
/*                       Email: support@mediasoftpro.com                      */
/*       License: Read license.txt located on root of your application.       */
/*                     Copyright 2007 - 2020 @Mediasoftpro                    */
/* -------------------------------------------------------------------------- */

import { NgModule } from "@angular/core";

import { Spinner1Component } from "./spinner01";
import { Spinner2Component } from "./spinner02";
import { Spinner3Component } from "./spinner03";
import { Spinner4Component } from "./spinner04";
import { Spinner5Component } from "./spinner05";
import { Spinner6Component } from "./spinner06";

@NgModule({
  imports: [],
  declarations: [
    Spinner1Component,
    Spinner2Component,
    Spinner3Component,
    Spinner4Component,
    Spinner5Component,
    Spinner6Component
  ],
  exports: [
    Spinner1Component,
    Spinner2Component,
    Spinner3Component,
    Spinner4Component,
    Spinner5Component,
    Spinner6Component
  ]
})
export class SpinnerModule {}
