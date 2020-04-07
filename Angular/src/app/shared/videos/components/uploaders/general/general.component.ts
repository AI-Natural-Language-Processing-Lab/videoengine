/* -------------------------------------------------------------------------- */
/*                          Product Name: VideoEngine                         */
/*                            Author: Mediasoftpro                            */
/*                       Email: support@mediasoftpro.com                      */
/*       License: Read license.txt located on root of your application.       */
/*                     Copyright 2007 - 2020 @Mediasoftpro                    */
/* -------------------------------------------------------------------------- */

import { Component, OnInit, Input } from "@angular/core";
import { Router } from "@angular/router";
import { select } from "@angular-redux/store";
import { Observable } from "rxjs/Observable";

// shared services
import { CoreService } from "../../../../../admin/core/coreService";
import { CoreAPIActions } from "../../../../../reducers/core/actions";

// reducer actions
import { fadeInAnimation } from "../../../../../animations/core";

@Component({
  templateUrl: "./general.html",
  selector: "app-general-uploader",
  animations: [fadeInAnimation]
})
export class GeneralVideoUploaderComponent implements OnInit {

  constructor(
    private coreActions: CoreAPIActions,
    private router: Router,
    private coreService: CoreService
  ) {}

  @Input() isAdmin = true;
  @Input() route_path = '/videos/';

  @select(["configuration", "configs"])
  readonly settings$: Observable<any>;

  formHeading = "Choose Upload Option";
  

  ngOnInit() {
  
  }

}
