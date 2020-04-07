
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
import * as OPTIONS from "../categories.model";
import { ContentTypes } from "../../../../configs/settings";
import { AppConfig } from "../../../../configs/app.config";
import { CoreService } from "../../../../admin/core/coreService";

@Injectable()
export class FormService {
  constructor(public config: AppConfig, private coreService: CoreService) {}

  getControls(entity: OPTIONS.CategoriesEntity, settings: any, category_types: any) {
    const controls: FormBase<any>[] = [];

    controls.push(
      new Controls.Textbox({
        key: "title",
        label: "Title",
        value: entity.title,
        required: true,
        minLength: 3,
        maxLength: 150,
        // pattern: '^\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}$',
        order: 1
        // colsize: 'col-md-2',
        // disabled: _disabled,
        // helpblock: 'Unique key used to fetch mail template within site events. e.g REGUSER for user regiration'
      })
    );

    controls.push(
      new Controls.Textbox({
        key: "term",
        label: "Term",
        value: entity.term,
        minLength: 3,
        maxLength: 150,
        order: 2
      })
    );

    const categories: any = [];
    for (const prop in category_types) {
      categories.push({
         key: category_types[prop].toString(),
         value: prop
      })
    }

    controls.push(
      new Controls.Dropdown({
        key: "type",
        label: "Select Type",
        required: true,
        value: entity.type.toString(),
        options: categories,
        order: 3
      })
    );

    controls.push(
      new Controls.Dropdown({
        key: "parentid",
        label: "Select Parent",
        required: true,
        value: entity.parentid.toString(),
        options: [],
        order: 4
      })
    );

    controls.push(
      new Controls.Textbox({
        key: "priority",
        label: "Priority",
        value: entity.priority.toString(),
        minLength: 3,
        maxLength: 150,
        order: 5
      })
    );

    controls.push(
      new Controls.TinyMyceEditor({
        key: "description",
        label: "Description",
        value: entity.description,
        tinymiceOptions: this.coreService.prepareInitEditorSettings(),
        required: false,
        order: 6
      })
    );

    controls.push(
      new Controls.Dropdown({
        key: "isenabled",
        label: "Select Status",
        required: true,
        value: entity.isenabled.toString(),
        options: [
          { key: "1", value: "Public" },
          { key: "0", value: "Private" }
        ],
        order: 7
      })
    );

    controls.push(
      new Controls.Textbox({
        key: "icon",
        label: "Icon",
        value: entity.icon,
        minLength: 1,
        maxLength: 100,
        order: 8,
        helpblock: "Place font-awsome icon to display with selected category"
      })
    );

    // Image Cropper
    const cropperOptions = {
      cropped_picture: entity.picturename,
      croptype: 1, // general cropped settings
      upload_id: 'cover_upload',
      colcss: 'col-md-8',
      settings: {
        width: settings.category_thumbnail_width,
        height: settings.category_thumbnail_height
      },
      uploadbtntext: "Upload Image",
      btncss: "btn btn-success"
    };

    controls.push(
      new Controls.ImageCropper({
        key: "picturename",
        label: "",
        value: entity.picturename,
        required: false,
        cropperOptions: cropperOptions,
        helpblock:
          "Cropsize: " +
          cropperOptions.settings.width +
          "x" +
          cropperOptions.settings.height,
        order: 9
      })
    );

    return controls.sort((a, b) => a.order - b.order);
  }
}
