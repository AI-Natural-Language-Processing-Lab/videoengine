/* -------------------------------------------------------------------------- */
/*                          Product Name: VideoEngine                         */
/*                            Author: Mediasoftpro                            */
/*                       Email: support@mediasoftpro.com                      */
/*       License: Read license.txt located on root of your application.       */
/*                     Copyright 2007 - 2020 @Mediasoftpro                    */
/* -------------------------------------------------------------------------- */

import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { PaginationComponent } from './components/pagination/pagination.component';
import { LoaderComponent } from './components/loader/loader.component';
import { AlertComponent } from './components/alert/alert.component';

import { SpinnerModule} from './components/loader/types/spinner.module';


import { NoRecordFoundComponent } from './bootstrap4/components/norecord/norecord.component';

/* services */
import { NotifyService } from './components/pnotify/pnotify.service';

/* pluloader */
import { PlUploadDirective } from './directives/uploader/plupload';
/* Core Service */
import { CoreService } from '../admin/core/coreService';
import { CoreAPIActions } from '../reducers/core/actions';
/* validators */
// import { BirthYearValidatorDirective } from './components/dynamicform/validators/birthyear';

/* bootstrap 4 components */
import { BootstrapNavigationComponent } from './bootstrap4/components/navigation/navigation.component';
import { BootstrapAlertComponent } from './bootstrap4/components/alert/alert.component';
import { BootstrapToolbarComponent } from './bootstrap4/components/toolbar/toolbar.component';

/* Dynamic Form Components */
// import { DynamicFormComponent } from './components/dynamicform/form.component';
import { DynamicFormControlComponent } from './components/dynamicform/form.control';
import { DynamicModalFormComponent } from './components/dynamicform/dynamic-modal-form';
import { FileDisplayComponent } from './components/dynamicform/components/display/display.component';
import { DisplayAudioComponent } from './components/dynamicform/components/display/audio/audio.component';
import { DisplayImageComponent } from './components/dynamicform/components/display/image/image.component';
import { DisplayVideoComponent } from './components/dynamicform/components/display/video/video.component';
import { DisplayFileComponent } from './components/dynamicform/components/display/file/file.component';

// import { AvatorComponent } from '../shared/avator/avator';

// import {SelectModule} from 'ng2-select';
import { EditorModule } from '@tinymce/tinymce-angular';

// video uploader
import { VideoUploadComponent } from '../shared/videos/components/uploaders/components/uploader';

// cropie component
import { CroppieComponent } from './directives/croppie/croppie';
import { CropperViewComponent } from "../shared/cropie/modal";

// multi select component
import { NgMultiSelectDropDownModule } from 'ng-multiselect-dropdown';

// multi select component
import {AutocompleteLibModule} from 'angular-ng-autocomplete';

// multi options
import { MultiOptionsComponent } from './components/dynamicform/components/multioptions/multiopitons.component';

/* cropper */
import { BannerUploaderComponent } from "../shared/cropie/uploader";

@NgModule({
  imports: [
    CommonModule,
    RouterModule,
    SpinnerModule,
    FormsModule,
    ReactiveFormsModule,
    EditorModule,
    NgMultiSelectDropDownModule,
    AutocompleteLibModule
  ],
  declarations: [
    BootstrapNavigationComponent,
    BootstrapAlertComponent,
    BootstrapToolbarComponent,
    PaginationComponent,
    LoaderComponent,
    AlertComponent,
    DynamicFormControlComponent,
    DynamicModalFormComponent,
    FileDisplayComponent,
    DisplayAudioComponent,
    DisplayImageComponent,
    DisplayVideoComponent,
    DisplayFileComponent,
    PlUploadDirective,
    NoRecordFoundComponent,
    VideoUploadComponent,
    //AvatorComponent,
    CroppieComponent,
    CropperViewComponent,
    MultiOptionsComponent,
    BannerUploaderComponent
  ],
  exports: [
    BootstrapNavigationComponent,
    BootstrapAlertComponent,
    BootstrapToolbarComponent,
    PaginationComponent,
    LoaderComponent,
    AlertComponent,
    DynamicFormControlComponent,
    DynamicModalFormComponent,
    FileDisplayComponent,
    DisplayAudioComponent,
    DisplayImageComponent,
    DisplayVideoComponent,
    DisplayFileComponent,
    PlUploadDirective,
    NoRecordFoundComponent,
    VideoUploadComponent,
    //AvatorComponent,
    CroppieComponent,
    CropperViewComponent,
    MultiOptionsComponent,
    BannerUploaderComponent
  ],
  entryComponents: [CropperViewComponent],
  providers: [NotifyService, CoreService, CoreAPIActions]
})
export class PartialModule {}
