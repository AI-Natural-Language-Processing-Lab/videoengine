/* -------------------------------------------------------------------------- */
/*                          Product Name: VideoEngine                         */
/*                            Author: Mediasoftpro                            */
/*                       Email: support@mediasoftpro.com                      */
/*       License: Read license.txt located on root of your application.       */
/*                     Copyright 2007 - 2020 @Mediasoftpro                    */
/* -------------------------------------------------------------------------- */

import {
  Component,
  Input,
  Output,
  OnInit,
  OnChanges,
  EventEmitter,
  ChangeDetectionStrategy
} from "@angular/core";
import { FormGroup } from "@angular/forms";

import { FormBase } from "./model/base";
import { ControlService } from "./services/control.service";
import { ThemeCSS } from "../../../configs/themeSettings";
@Component({
  selector: "dynamic-modal-form",
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: "./dynamic-modal-form.html",
  providers: [ControlService]
})
export class DynamicModalFormComponent implements OnInit, OnChanges {
  @Input() controls: FormBase<any>[] = [];
  @Input() showCancel = true;
  @Input() showModal = true;
  @Input() submitText = "Submit";
  @Input() cancelText = "Cancel";
  @Input() skipBtnText = "";
  @Input() submitCss = "";
  @Output() OnClose = new EventEmitter<any>();
  @Output() OnSkip = new EventEmitter<any>();
  @Output() OnSubmit = new EventEmitter<any>();
  @Output() OnDropdownSelection = new EventEmitter<any>();
  @Output() FileRemoved = new EventEmitter<any>();
  @Input() showLoader = false;

  tempSubmitText = "";

  form: FormGroup;
  payLoad = "";
  isSubmit = false;
  submitBtnCss = ThemeCSS.SUBMIT_BUTTON_CSS;
  disableSubmit = false;

  constructor(private qcs: ControlService) {}

  ngOnInit() {
    if (this.submitCss !== "") {
      this.submitBtnCss = this.submitCss;
    }
    this.generateForm();
  }

  ngOnChanges() {
    this.generateForm();
  }

  generateForm() {
    if (this.controls.length > 0) {
      this.form = this.qcs.toFormGroup(this.controls);
    }
  }

  OnDropdownSelectionChange(payload: any) {
    this.OnDropdownSelection.emit(payload);
  }

  OnFileRemoved(payload: any) {
    this.FileRemoved.emit(payload);
  }

  uploadCompleted(files: any) {
    this.submitText = this.tempSubmitText;
    this.disableSubmit = false;
  }

  uploadProgress(isenabled: any) {
    this.disableSubmit = true;
    this.tempSubmitText = this.submitText;
    this.submitText = "Uploading...";
  }
  onSubmit() {
    this.isSubmit = true;
    if (this.form.valid) {
      this.payLoad = this.form.value;
      this.OnSubmit.emit(this.payLoad);
    }
  }

  close() {
    this.OnClose.emit(true);
  }

  skip() {
    // remove validation
    this.isSubmit = false;
    for (const control of this.controls) {
      console.log("cc");
      console.log(control.key);
      this.form.get(control.key).clearValidators();
      this.form.get(control.key).updateValueAndValidity();
    }
    this.OnSkip.emit(true);
  }
}
