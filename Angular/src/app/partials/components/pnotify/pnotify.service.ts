/* -------------------------------------------------------------------------- */
/*                          Product Name: VideoEngine                         */
/*                            Author: Mediasoftpro                            */
/*                       Email: support@mediasoftpro.com                      */
/*       License: Read license.txt located on root of your application.       */
/*                     Copyright 2007 - 2020 @Mediasoftpro                    */
/* -------------------------------------------------------------------------- */

import { Injectable } from "@angular/core";
import { ToastrService } from "ngx-toastr";
@Injectable()
export class NotifyService {
  constructor(private toastrService: ToastrService) {}

  render(title, text, css) {
    const params = {
      // timeOut: 3000,
      closeButton: true
    };

    if (css === "bg-success") {
      this.toastrService.success(text, title, params);
    } else if (css === "bg-error" || css === "bg-danger") {
      this.toastrService.error(text, title, params);
    } else {
      this.toastrService.info(text, title, params);
    }
  }
}
