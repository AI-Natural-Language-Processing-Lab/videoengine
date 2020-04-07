/* -------------------------------------------------------------------------- */
/*                          Product Name: VideoEngine                         */
/*                            Author: Mediasoftpro                            */
/*                       Email: support@mediasoftpro.com                      */
/*       License: Read license.txt located on root of your application.       */
/*                     Copyright 2007 - 2020 @Mediasoftpro                    */
/* -------------------------------------------------------------------------- */

export class FormBase<T> {
  value: T;
  type?: string;
  options?: any;
  key: string;
  label: string;
  labelasheading?: boolean;
  required: boolean;
  email: boolean;
  minLength?: number;
  maxLength?: number;
  pattern?: string;
  order: number;
  controlType: string;
  checked?: boolean;
  checklist?: any;
  helpblock?: any;
  uploadoptions?: any; // file uploader options / settings
  cropperOptions?: any; // cropper (image) / settings
  multiselectOptions?: any; // multi select options / settings
  tinymiceOptions?: any; // tinymice edition options / settings
  autocompleteOptions?: any; // auto complete options / settings
  colsize?: any;
  disabled?: any;
  items?: any;
  placeholder?: string;
  isVisible?: boolean;
  css?: string;
  constructor(
    options: {
      value?: T;
      type?: string;
      options?: any;
      key?: string;
      label?: string;
      required?: boolean;
      email?: boolean;
      minLength?: number;
      maxLength?: number;
      pattern?: string;
      order?: number;
      controlType?: string;
      checked?: boolean;
      checklist?: any;
      helpblock?: any;
      uploadoptions?: any;
      cropperOptions?: any;
      multiselectOptions?: any;
      tinymiceOptions?: any;
      autocompleteOptions?: any;
      colsize?: any;
      disabled?: any;
      items?: any; // for select dropdown
      placeholder?: string;
      labelasheading?: boolean;
      isVisible?: boolean,
      css?: string
    } = {}
  ) {
    this.value = options.value;
    this.type = options.type || "";
    this.options = options.options === undefined ? [] : options.options;
    this.key = options.key || "";
    this.minLength = options.minLength;
    this.maxLength = options.maxLength;
    this.pattern = options.pattern;
    this.label = options.label || "";
    this.required = !!options.required;
    this.email = !!options.email;
    this.order = options.order === undefined ? 1 : options.order;
    this.controlType = options.controlType || "";
    this.checked = options.checked === undefined ? false : options.checked;
    this.checklist = options.checklist === undefined ? [] : options.checklist;
    this.helpblock = options.helpblock === undefined ? "" : options.helpblock;
    this.uploadoptions =
      options.uploadoptions === undefined ? "" : options.uploadoptions;
    this.cropperOptions =
      options.cropperOptions === undefined ? "" : options.cropperOptions;
    this.multiselectOptions =
      options.multiselectOptions === undefined
        ? ""
        : options.multiselectOptions;
    this.tinymiceOptions =
      options.tinymiceOptions === undefined ? "" : options.tinymiceOptions;
    this.autocompleteOptions =
      options.autocompleteOptions === undefined
        ? {}
        : options.autocompleteOptions;
    this.colsize =
      options.colsize === undefined ? "col-md-12" : options.colsize;
    this.disabled = options.disabled === undefined ? false : options.disabled;
    this.items = options.items === undefined ? [] : options.items;
    this.placeholder =
      options.placeholder === undefined ? "" : options.placeholder;
    this.labelasheading =
      options.labelasheading === undefined ? false : options.labelasheading;
      this.isVisible = options.isVisible === undefined? true: options.isVisible;
      this.css = options.css === undefined? "m-t-10": options.css;
  }
}
