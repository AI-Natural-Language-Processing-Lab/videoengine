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
  EventEmitter,
  OnInit,
  AfterViewInit,
  ChangeDetectorRef
} from "@angular/core";
import { FormGroup } from "@angular/forms";
import { FormBase } from "./model/base";
// cropper directives
import { NgbModal, NgbModalOptions } from "@ng-bootstrap/ng-bootstrap";
import { CropperViewComponent } from "../../../shared/cropie/modal";

import { CoreAPIActions } from "../../../reducers/core/actions";

@Component({
  selector: "df-control",
  templateUrl: "./dynamic-form-control.html",
  providers: [CoreAPIActions]
})
export class DynamicFormControlComponent implements OnInit, AfterViewInit {
  @Input() control: FormBase<any>;
  @Input() form: FormGroup;
  @Input() isSubmit = false;
  @Output() OnDropdownSelectionChange = new EventEmitter<any>();
  @Output() OnFileRemoved = new EventEmitter<any>();
  @Output() uploadStatus = new EventEmitter<any>();
  @Output() uploadCompleted = new EventEmitter<any>();
  @Output() autoTextChange = new EventEmitter<string>();

  constructor(
    private ref: ChangeDetectorRef,
    private modalService: NgbModal,
    private coreAction: CoreAPIActions
  ) {}

  showUploadBtn = true;
  cropperView = false;
  uploadedFiles: any = [];
  readCropperImage: string | ArrayBuffer;
 
  private errorMessages = {
    email: params => "Invalid email address",
    required: params => params.key + " is required",
    minlength: params =>
      "The min number of characters is " + params.requiredLength, //  (params.requiredLength - params.actualLength) + ' characters needed',
    maxlength: params =>
      "The max allowed number of characters is " +
      params.requiredLength +
      ", you typed " +
      params.actualLength,
    pattern: params => "Incorrect " + params.key,
    years: params => params.message,
    countryCity: params => params.message,
    uniqueName: params => params.message,
    telephoneNumbers: params => params.message,
    telephoneNumber: params => params.message
  };

  ngOnInit() {
    // console.log('ng on after init called');
    if (this.control.controlType === "uploader") {
      if (this.control.value.length > 0) {
        for (const file of this.control.value) {
          this.uploadedFiles.push(file);
        }
      }
    }
  }

   ngAfterViewInit() {
    /*console.log('ng after init called');
    if (this.control.controlType === 'uploader') {
      if (this.control.value.length > 0) {
        for (const file of this.control.value) {
           this.uploadedFiles.push(file);
        }
      }
    }*/
  }

  shouldShowErrors(): boolean {
    return (
      this.form.controls[this.control.key] &&
      this.form.controls[this.control.key].errors &&
      (this.form.controls[this.control.key].dirty ||
        this.form.controls[this.control.key].touched ||
        this.isSubmit)
    );
  }

  listOfErrors(): string[] {
    return Object.keys(this.form.controls[this.control.key].errors).map(field =>
      this.getMessage(
        field,
        this.control.key,
        this.control.label,
        this.form.controls[this.control.key].errors[field]
      )
    );
  }

  private getMessage(type: string, key: string, label: string, params: any) {
    if (type === "required") {
      if (label !== undefined && label !== "") {
        return label + " is required";
      } else {
        return key + " is required";
      }
    } else if (type === "pattern") {
      if (label !== "") {
        return "Incorrect " + label;
      } else {
        return "Incorrect " + key;
      }
    } else {
      return this.errorMessages[type](params);
    }
  }

  /* -------------------------------------------------------------------------- */
  /*                            auto complete events                            */
  /* -------------------------------------------------------------------------- */
  onChangeSearch(text: any) {
    this.coreAction.triggleEvent({ term: text });
  }

  // pass dropdown selection value with key reference to parent component
  selectedDropdownValue(key: any, value: any) {
    this.OnDropdownSelectionChange.emit({ key, value });
  }

  filesUploaded(files: any) {
    for (const file of files) {
      this.uploadedFiles.push(file);
    }

    this.control.value = this.uploadedFiles;
    this.uploadCompleted.emit(files);
  }

  uploadProgress(isenabled: any) {
    this.uploadStatus.emit(isenabled);
  }

  filesUploaded_v2(files: any) {
    //
  }

  removedItems(output: any) {
    this.uploadedFiles = output.files;
    this.control.value = this.uploadedFiles;
    this.OnFileRemoved.emit({
      key: this.control.key,
      file: output.removedItems
    });
  }

  choose(value, event) {
    console.log("chosen " + value);
    this.control.value = value;
  }

  changeCheckedValues(arr) {
    const options = arr.filter(opt => opt.checked).map(opt => opt.value);
    console.log(options);
  }

  // Image Cropper Functionality
  changeListener($event): void {
    this.readThis($event.target);
  }

  readThis(inputValue: any): void {
    const file: File = inputValue.files[0];
    const myReader: FileReader = new FileReader();
    const _this = this;
    myReader.onloadend = function(e) {
      _this.readCropperImage = myReader.result;
      _this.control.cropperOptions.original_picture = _this.readCropperImage;
      _this.ref.detectChanges();
      _this.modal_popup();
    };
    myReader.readAsDataURL(file);
  }

  modal_popup() {
    const _options: NgbModalOptions = {
      backdrop: false,
      size: "lg"
    };
    console.log("crop options");
    console.log(this.control.cropperOptions);
    const modalRef = this.modalService.open(CropperViewComponent, _options);
    modalRef.componentInstance.Info = {
      title: "Editor",
      data: this.control.cropperOptions,
      cropoption: this.control.cropperOptions.croptype,
      settings: this.control.cropperOptions.settings,
      scroller: true
    };
    modalRef.result.then(
      result => {
        this.control.value = result.data.image;
        this.control.cropperOptions.cropped_picture = result.data.image;
        this.ref.detectChanges();
      },
      dismissed => {
        console.log("dismissed");
      }
    );
  }

  removeCropperImage(event) {
    this.control.cropperOptions.cropped_picture = "";
    event.stopPropagation();
  }

  /* -------------------------------------------------------------------------- */
  /*                           remove uploaded images                           */
  /* -------------------------------------------------------------------------- */
  remove(obj, index, event) {
    if (index > -1) {
      this.uploadedFiles.splice(index, 1);
      this.removedItems({ files: this.uploadedFiles, removedItems: obj });
    }
    event.stopPropagation();
  }

  /* -------------------------------------------------------------------------- */
  /*                             multi text options                             */
  /* -------------------------------------------------------------------------- */
  changeOption(option: any) {
      this.control.value = option;
  }

}
