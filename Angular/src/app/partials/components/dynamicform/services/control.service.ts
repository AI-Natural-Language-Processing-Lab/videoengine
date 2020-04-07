/* -------------------------------------------------------------------------- */
/*                          Product Name: VideoEngine                         */
/*                            Author: Mediasoftpro                            */
/*                       Email: support@mediasoftpro.com                      */
/*       License: Read license.txt located on root of your application.       */
/*                     Copyright 2007 - 2020 @Mediasoftpro                    */
/* -------------------------------------------------------------------------- */

import { Injectable } from "@angular/core";
import { FormControl, FormGroup, Validators } from "@angular/forms";
import { FormBase } from "../model/base";

@Injectable()
export class ControlService {
  constructor() {}

  toFormGroup(controls: FormBase<any>[]) {
    const group: any = {};

    controls.forEach(control => {
      if (control.checklist.length > 0) {
        control.checklist.forEach(option => {
          group[option.key] = option.required
            ? new FormControl(option.value || "", [
                Validators.required,
                Validators.minLength(option.minLength),
                Validators.maxLength(option.maxLength),
                Validators.pattern(option.pattern)
              ])
            : new FormControl(option.value || "");
        });
      }

      if (control.email) {
        group[control.key] = control.required
          ? new FormControl(control.value || "", [
              Validators.required,
              Validators.email,
              Validators.minLength(control.minLength),
              Validators.maxLength(control.maxLength),
              Validators.pattern(control.pattern)
            ])
          : new FormControl(control.value || "");
      } else {
        group[control.key] = control.required
          ? new FormControl(control.value || "", [
              Validators.required,
              Validators.minLength(control.minLength),
              Validators.maxLength(control.maxLength),
              Validators.pattern(control.pattern)
            ])
          : new FormControl(control.value || "");
      }
    });
    return new FormGroup(group);
  }
}
