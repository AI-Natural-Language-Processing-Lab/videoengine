
/* -------------------------------------------------------------------------- */
/*                          Product Name: VideoEngine                         */
/*                            Author: Mediasoftpro                            */
/*                       Email: support@mediasoftpro.com                      */
/*       License: Read license.txt located on root of your application.       */
/*                     Copyright 2007 - 2020 @Mediasoftpro                    */
/* -------------------------------------------------------------------------- */

import { Injectable } from "@angular/core";
import * as Controls from "../../../../partials/components/dynamicform/model/elements";
import { FormBase } from "../../../../partials/components/dynamicform/model/base";

import { IBlockEntity } from "../blockip.model";

@Injectable()
export class FormService {
  getControls(data: IBlockEntity) {
    const controls: FormBase<any>[] = [];

    controls.push(
      new Controls.Textbox({
        key: "ipaddress",
        label: "IP Address",
        value: data.ipaddress,
        required: true,
        minLength: 5,
        maxLength: 40,
        // pattern: '^\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}$',
        order: 1
      })
    );

    return controls.sort((a, b) => a.order - b.order);
  }
}
