/* -------------------------------------------------------------------------- */
/*                          Product Name: VideoEngine                         */
/*                            Author: Mediasoftpro                            */
/*                       Email: support@mediasoftpro.com                      */
/*       License: Read license.txt located on root of your application.       */
/*                     Copyright 2007 - 2020 @Mediasoftpro                    */
/* -------------------------------------------------------------------------- */

import { Component, OnInit } from "@angular/core";
import { ActivatedRoute, Router } from "@angular/router";

@Component({
  templateUrl: "./attributes.html"
})
export class UserProfileAttributesComponent implements OnInit {

  Attr_Type = 3; // 0: Ads, 1: Agency / Company, 2: Artists, 3: User Profile
  RecordID = 0;

  constructor(
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit() {
    // fetch param from url
    this.route.params.subscribe(params => {
      this.RecordID = parseInt(params["id"], 10);
      if (isNaN(this.RecordID)) {
        this.RecordID = 0;
      }
      
      if (this.RecordID === 0) {
         this.router.navigate(["/users/settings"]);
      }
    });
  }
}
