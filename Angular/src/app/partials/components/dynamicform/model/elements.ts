/* -------------------------------------------------------------------------- */
/*                          Product Name: VideoEngine                         */
/*                            Author: Mediasoftpro                            */
/*                       Email: support@mediasoftpro.com                      */
/*       License: Read license.txt located on root of your application.       */
/*                     Copyright 2007 - 2020 @Mediasoftpro                    */
/* -------------------------------------------------------------------------- */

import { FormBase } from "./base";

export class Textbox extends FormBase<string> {
  controlType = "textbox";
  type: string;

  constructor(options: {} = {}) {
    super(options);
    this.type = options["type"] || "";
  }
}

export class TextArea extends FormBase<string> {
  controlType = "textarea";
  type: string;

  constructor(options: {} = {}) {
    super(options);
    this.type = options["type"] || "";
  }
}

export class TinyMyceEditor extends FormBase<string> {
  controlType = "tinymce";
  type: string;

  constructor(options: {} = {}) {
    super(options);
    this.type = options["type"] || "";
  }
}

export class Dropdown extends FormBase<string> {
  controlType = "dropdown";
  options: { key: string; value: string }[] = [];

  constructor(options: {} = {}) {
    super(options);
    this.options = options["options"] || [];
  }
}

export class MultiDropdown extends FormBase<string> {
  controlType = "multidropdown";
  options: { key: string; value: string }[] = [];

  constructor(options: {} = {}) {
    super(options);
    this.options = options["options"] || [];
  }
}

export class Select extends FormBase<string> {
  controlType = "select";
  options: { key: string; value: string }[] = [];

  constructor(options: {} = {}) {
    super(options);
    this.options = options["options"] || [];
  }
}

export class RadioButtonList extends FormBase<string> {
  controlType = "radiolist";
  // radiolist: RadioButton[];
  options: { key: string; value: string }[] = [];
  constructor(options: {} = {}) {
    super(options);
    // this.radiolist = options['radiolist'] || [];
    this.options = options["options"] || [];
  }
}

export class RadioButton extends FormBase<boolean> {
  controlType = "radio";
  checked: boolean;

  constructor(options: {} = {}) {
    super(options);
    this.checked = options["checked"] || false;
  }
}

export class CheckBox extends FormBase<boolean> {
  controlType = "check";
  checked: boolean;

  constructor(options: {} = {}) {
    super(options);
    this.checked = options["checked"] || false;
  }
}

export class CheckBoxList extends FormBase<string> {
  controlType = "checklist";
  checklist: CheckBox[];

  constructor(options: {} = {}) {
    super(options);
    this.checklist = options["checklist"] || [];
  }
}

export class Uploader extends FormBase<string> {
  controlType = "uploader";
  type: string;

  constructor(options: {} = {}) {
    super(options);
    this.type = options["type"] || "";
  }
}

export class ImageCropper extends FormBase<string> {
  controlType = "cropper";
  type: string;

  constructor(options: {} = {}) {
    super(options);
    this.type = options["type"] || "";
  }
}

export class AutoComplete extends FormBase<string> {
  controlType = "autocomplete";
  type: string;

  constructor(options: {} = {}) {
    super(options);
    this.type = options["type"] || "";
  }
}

export class MultiTextOptions extends FormBase<string> {
  controlType = "multitextoptions";
  type: string;

  constructor(options: {} = {}) {
    super(options);
    this.type = options["type"] || "";
  }
}

export class SectionHeader extends FormBase<string> {
  controlType = "section";

  constructor(options: {} = {}) {
    super(options);
  }
}

export class DropdownList extends FormBase<string> {
  controlType = "dropdownlist";
  checklist: Dropdown[];

  constructor(options: {} = {}) {
    super(options);
    this.checklist = options["checklist"] || [];
  }
}

export class TextBoxList extends FormBase<string> {
  controlType = "textboxlist";
  checklist: Dropdown[];

  constructor(options: {} = {}) {
    super(options);
    this.checklist = options["checklist"] || [];
  }
}
