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
  EventEmitter
} from "@angular/core";
import { FormGroup } from "@angular/forms";

import { FormBase } from "../../../../../../partials/components/dynamicform/model/base";
import { ControlService } from "../../../../../../partials/components/dynamicform/services/control.service";
import { ThemeCSS } from "../../../../../../configs/themeSettings";

/* services */
import { SettingsService } from "../../../../../../shared/videos/services/settings.service";
import { DataService } from "../../../../../../shared/videos/services/data.service";

@Component({
  selector: "app-photo-uploader",
  // changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: "./videouploader.html",
  providers: [ControlService]
})
export class VideoUploaderComponent implements OnInit, OnChanges {
  @Input() controls: FormBase<any>[] = [];
  @Input() showCancel = true;
  @Input() showModal = true;
  @Input() submitText = "Submit";
  @Output() OnClose = new EventEmitter<any>();
  @Output() OnSubmit = new EventEmitter<any>();
  @Output() OnDropdownSelection = new EventEmitter<any>();
  @Output() FileRemoved = new EventEmitter<any>();

  @Input() showLoader = false;

  form: FormGroup;
  payLoad: any = null;
  isSubmit = false;
  submitBtnCss = ThemeCSS.SUBMIT_BUTTON_CSS;

  @Input() Info: any;

  constructor(
    private qcs: ControlService,
    private settingService: SettingsService,
    private dataService: DataService
  ) {}

  ngOnInit() {
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

  onSubmit() {
    this.isSubmit = true;
    console.log("submit true");
    if (this.form.valid) {
      this.payLoad = this.form.value;
      this.OnSubmit.emit(this.payLoad);
    }
  }

  close() {
    this.OnClose.emit(true);
  }
}
