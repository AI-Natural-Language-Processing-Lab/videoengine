/* -------------------------------------------------------------------------- */
/*                          Product Name: VideoEngine                         */
/*                            Author: Mediasoftpro                            */
/*                       Email: support@mediasoftpro.com                      */
/*       License: Read license.txt located on root of your application.       */
/*                     Copyright 2007 - 2020 @Mediasoftpro                    */
/* -------------------------------------------------------------------------- */

import { Injectable } from "@angular/core";
import * as Controls from "../../../partials/components/dynamicform/model/elements";
import { FormBase } from "../../../partials/components/dynamicform/model/base";
import { AppConfig } from "../../../configs/app.config";
import { CoreService } from "../../../admin/core/coreService";

@Injectable()
export class FormService {

  constructor(public config: AppConfig, private coreService: CoreService) {}

  getTemplateControls(entity: any) {
    const controls: FormBase<any>[] = [];

    controls.push(
      new Controls.Textbox({
        key: "title",
        label: "Title",
        value: entity.title,
        required: true,
        order: 0,
        maxLength: 300
      })
    );

    return controls.sort((a, b) => a.order - b.order);
  }

  getTemplateSectionControls(entity: any) {
    const controls: FormBase<any>[] = [];

    controls.push(
      new Controls.Textbox({
        key: "title",
        label: "Title",
        value: entity.title,
        required: true,
        order: 0,
        maxLength: 300
      })
    );

    controls.push(
      new Controls.Textbox({
        key: "priority",
        label: "Priority",
        value: entity.priority.toString(),
        required: true,
        pattern:  "[0-9]+",
        helpblock: `Arrange sections based on priority in list`,
        order: 1
      })
    );

    controls.push(
      new Controls.Dropdown({
        key: "showsection",
        label: "Display Attributes",
        required: true,
        value: entity.showsection.toString(),
        options: [
          {
            key: 0,
            value: "Without Section"
          },
          {
            key: 1,
            value: "With Section  [Both Controls + Display]"
          },
          {
            key: 2,
            value: "With Section  [Only Controls]"
          }
        ],
        order: 2,
        helpblock: `Toggle on | off displaying section information on top at form / public display areas`,
      })
    );

    return controls.sort((a, b) => a.order - b.order);
  }

  getAttributeControls(entity: any) {
    const controls: FormBase<any>[] = [];

    controls.push(
      new Controls.Textbox({
        key: "title",
        label: "Title",
        value: entity.title,
        required: true,
        order: 0,
        maxLength: 100
        // helpblock: `Enter post title`
      })
    );

    controls.push(
      new Controls.Textbox({
        key: "value",
        label: "Placeholder",
        value: entity.value,
        required: false,
        order: 1,
        maxLength: 100,
        helpblock: `Instruction info for display if control is text box or text area. e.g default size 24 cm`
      })
    );

    controls.push(
      new Controls.Textbox({
        key: "priority",
        label: "Priority",
        value: entity.priority.toString(),
        required: true,
        helpblock: `Arrange attributes based on priority in list or order by in forms`,
        order: 2
      })
    );

    controls.push(
      new Controls.Dropdown({
        key: "element_type",
        label: "Element Type",
        required: true,
        value: entity.element_type.toString(),
        options: [
          {
            key: 0,
            value: "Text Box"
          },
          {
            key: 1,
            value: "Text Area"
          },
          {
            key: 5,
            value: "Rich Text Area"
          },
          {
            key: 2,
            value: "Dropdown"
          },
          {
            key: 3,
            value: "Check Box"
          },
          {
            key: 4,
            value: "Radio Button"
          }
        ],
        order: 3
      })
    );

    var nameArr = entity.options.split(",");
    let options = [];
    for (let item of nameArr) {
      options.push({ id: options.length + 1, value: item });
    }
    controls.push(
      new Controls.MultiTextOptions({
        key: "options",
        label: "Options",
        value: options,
        required: false,
        helpblock: `Add two or more options if element type if dropdown / checkboxes or radio buttons`,
        order: 4
      })
    );

    controls.push(
      new Controls.Dropdown({
        key: "isrequired",
        label: "Required",
        required: true,
        value: entity.isrequired.toString(),
        options: [
          {
            key: 0,
            value: "No"
          },
          {
            key: 1,
            value: "Yes"
          }
        ],
        order: 5
      })
    );

    controls.push(
      new Controls.Dropdown({
        key: "variable_type",
        label: "Variable Type",
        required: true,
        value: entity.variable_type.toString(),
        options: [
          {
            key: 0,
            value: "String"
          },
          {
            key: 1,
            value: "Number"
          },
          {
            key: 2,
            value: "Year"
          }
        ],
        order: 6
      })
    );

    controls.push(
      new Controls.Textbox({
        key: "icon",
        label: "Icon",
        value: entity.icon,
        required: false,
        order: 7,
        maxLength: 150,
        helpblock: `icon class of path to associate icon with this attribute`
      })
    );

    controls.push(
      new Controls.Textbox({
        key: "helpblock",
        label: "Help Instruction",
        value: entity.helpblock,
        required: false,
        order: 8,
        maxLength: 150,
        helpblock: `optional instruction to be displayed below element`
      })
    );

    controls.push(
      new Controls.Textbox({
        key: "min",
        label: "Minimum Characters",
        value: entity.min.toString(),
        required: false,
        pattern:  "[0-9]+",
        helpblock: `Minimum no of characters (validation purpose), 0: unlimited`,
        order: 9
      })
    );

    controls.push(
      new Controls.Textbox({
        key: "max",
        label: "Maximum Characters",
        value: entity.max.toString(),
        required: false,
        pattern:  "[0-9]+",
        helpblock: `Maximum no of characters (validation purpose), 0: unlimited`,
        order: 10
      })
    );

    controls.push(
      new Controls.Textbox({
        key: "postfix",
        label: "Postfix",
        value: entity.postfix,
        required: false,
        helpblock: `Add some characters after actual value e.g km -> 100km`,
        order: 11
      })
    );

    controls.push(
      new Controls.Textbox({
        key: "prefix",
        label: "Prefix",
        value: entity.prefix,
        required: false,
        helpblock: `Add some characters before actual value e.g Mr -> Mr Shane`,
        order: 12
      })
    );

    controls.push(
      new Controls.Textbox({
        key: "tooltip",
        label: "Tooltip",
        value: entity.tooltip,
        required: false,
        helpblock: `Add optional tooltip message when mouse over on display time.`,
        order: 13
      })
    );

    controls.push(
      new Controls.Textbox({
        key: "url",
        label: "Url",
        value: entity.url,
        required: false,
        helpblock: `Add url if you want to link the display with internal or external link`,
        order: 14
      })
    );

    return controls.sort((a, b) => a.order - b.order);
  }
}
