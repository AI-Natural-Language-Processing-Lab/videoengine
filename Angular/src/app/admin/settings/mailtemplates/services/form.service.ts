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
import * as OPTIONS from "../mailtemplates.model";
import { CoreService } from "../../../../admin/core/coreService";

@Injectable()
export class FormService {

  constructor(private coreService: CoreService) {}

  getControls(entity: OPTIONS.MailTemplatesEntity, mailtypes: any) {
    const controls: FormBase<any>[] = [];

    let _disabled = false;
    if (entity.id > 0) {
      _disabled = true;
    }
    controls.push(
      new Controls.Textbox({
        key: "templatekey",
        label: "Template Key",
        value: entity.templatekey,
        required: true,
        minLength: 3,
        maxLength: 25,
        // pattern: '^\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}$',
        order: 1,
        //colsize: 'col-md-2',
        disabled: _disabled,
        helpblock:
          "Unique key used to fetch mail template within site events. e.g REGUSER for user regiration"
      })
    );

    controls.push(
      new Controls.TextArea({
        key: "description",
        label: "Description",
        value: entity.description,
        required: true,
        helpblock:
          "A block of help text that breaks onto a new line and may extend beyond one line.",
        // pattern: '^\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}$',
        order: 2
      })
    );

    controls.push(
      new Controls.Textbox({
        key: "subjecttags",
        label: "Subject Tags",
        value: entity.subjecttags,
        required: true,
        order: 3,
        maxLength: 100,
        helpblock:
          "Enter subject tags for informative purpose e.g [username], [email], that may be used in template subject for dynamic text processing"
      })
    );

    controls.push(
      new Controls.Textbox({
        key: "subject",
        label: "Subject",
        value: entity.subject,
        required: true,
        maxLength: 150,
        order: 4
      })
    );
    controls.push(
      new Controls.Textbox({
        key: "tags",
        label: "Content Tags",
        value: entity.tags,
        required: true,
        order: 5,
        maxLength: 100,
        helpblock:
          "Enter tags for informative purpose e.g [username], [email], that may be used in template body for dynamic text processing"
      })
    );

    controls.push(
      new Controls.TinyMyceEditor({
        key: "contents",
        label: "Contents",
        value: entity.contents,
        tinymiceOptions: this.coreService.prepareInitAdvacneEditorSettings(),
        required: true,
        order: 6
      })
    );

    const mailtemplate_types: any = [];
    for (const prop in mailtypes) {
      mailtemplate_types.push({
         key: mailtypes[prop].toString(),
         value: prop
      })
    }

    controls.push(
      new Controls.Dropdown({
        key: "type",
        label: "Type",
        value: entity.type,
        options: mailtemplate_types,
        order: 7
      })
    );

    return controls.sort((a, b) => a.order - b.order);
  }
}
