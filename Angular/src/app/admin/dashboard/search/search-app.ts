/* -------------------------------------------------------------------------- */
/*                          Product Name: VideoEngine                         */
/*                            Author: Mediasoftpro                            */
/*                       Email: support@mediasoftpro.com                      */
/*       License: Read license.txt located on root of your application.       */
/*                     Copyright 2007 - 2020 @Mediasoftpro                    */
/* -------------------------------------------------------------------------- */

import { Component } from "@angular/core";

// services
import { AppConfig } from "../../../configs/app.config";

@Component({
  selector: "app-search",
  templateUrl: "./search-app.html"
})

export class SearchComponent {
  constructor(
    public config: AppConfig
  ) {}

}
