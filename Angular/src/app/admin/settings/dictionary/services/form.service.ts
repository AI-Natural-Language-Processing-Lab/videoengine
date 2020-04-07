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
import * as OPTIONS from "../dictionary.model";

@Injectable()
export class FormService {
  getControls(entity: OPTIONS.IDictionaryEntity) {
    const controls: FormBase<any>[] = [];

    controls.push(
      new Controls.Textbox({
        key: "value",
        label: "Value",
        value: entity.value,
        required: true,
        minLength: 5,
        maxLength: 40,
        // pattern: '^\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}$',
        order: 1
      })
    );

    controls.push(
      new Controls.Dropdown({
        key: "type",
        label: "Type",
        value: entity.type.toString(),
        required: true,
        options: [
          { key: "0", value: "Screening Worlds" },
          { key: "1", value: "Restricted Usernams" }
        ],
        order: 2
      })
    );
    return controls.sort((a, b) => a.order - b.order);
  }
}
