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
import * as OPTIONS from "../roles.model";


@Injectable()
export class FormService {
  getRoleControls(entity: OPTIONS.RoleEntity) {
    const controls: FormBase<any>[] = [];

    controls.push(
      new Controls.Textbox({
        key: "rolename",
        label: "Role Name",
        value: entity.rolename,
        required: true,
        order: 0,
        maxLength: 100,
        placeholder: "Enter Role Name"
      })
    );

    return controls.sort((a, b) => a.order - b.order);
  }

  getObjectControls(entity: OPTIONS.RoleObjectEntity) {
    const controls: FormBase<any>[] = [];

    controls.push(
      new Controls.Textbox({
        key: "objectname",
        label: "Object Name",
        value: entity.objectname,
        required: true,
        order: 0,
        maxLength: 100,
        placeholder: "Enter Object Name"
      })
    );

    controls.push(
      new Controls.TextArea({
        key: "description",
        label: "Description",
        value: entity.description,
        order: 1,
        maxLength: 1000,
        placeholder: "Enter Short Description (Optional)"
      })
    );

    return controls.sort((a, b) => a.order - b.order);
  }
}
