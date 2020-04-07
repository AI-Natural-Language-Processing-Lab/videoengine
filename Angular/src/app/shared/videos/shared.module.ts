/* -------------------------------------------------------------------------- */
/*                          Product Name: VideoEngine                         */
/*                            Author: Mediasoftpro                            */
/*                       Email: support@mediasoftpro.com                      */
/*       License: Read license.txt located on root of your application.       */
/*                     Copyright 2007 - 2020 @Mediasoftpro                    */
/* -------------------------------------------------------------------------- */

import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { RouterModule } from "@angular/router";

// shared modules
import { PartialModule } from "../../partials/shared.module";

// components
import { ListComponent } from "./components/partials/list.component";
import { MainVideosComponent } from "./components/main/main.components";
import { VideoProcComponent } from "./components/process/process.component";
import { UpdateVideoThumbComponent } from "./components/updatethumbnail/updatethumbnail.component";
import { AWSComponent } from "./components/uploaders/aws/process.component";
import { DirectUploaderComponent } from "./components/uploaders/direct/process.component";
import { FFMPEGComponent } from "./components/uploaders/ffmpeg/process.component";
import { GeneralVideoUploaderComponent } from "./components/uploaders/general/general.component";
import { YoutubeComponent } from "./components/uploaders/youtube/process.component";
import { UploadMovieComponent } from "./components/uploaders/movie/process.component";
import { VideoUploaderComponent } from "./components/uploaders/direct/partials/videouploader.component";
import { SMVideoListComponent } from './components/partials/smlist.component';
import { ViewComponent } from "./components/partials/modal.component";
// services
import { SettingsService } from "./services/settings.service";
import { DataService } from "./services/data.service";
import { FormService } from "./services/form.service";

/* actions */
import { VideoAPIActions } from "../../reducers/videos/actions";


@NgModule({
  imports: [
    CommonModule,
    PartialModule,
    RouterModule,
    FormsModule,
    ReactiveFormsModule
  ],
  declarations: [
    ListComponent,
    MainVideosComponent,
    VideoProcComponent,
    UpdateVideoThumbComponent,
    AWSComponent,
    DirectUploaderComponent,
    FFMPEGComponent,
    GeneralVideoUploaderComponent,
    YoutubeComponent,
    VideoUploaderComponent,
    SMVideoListComponent,
    UploadMovieComponent,
    ViewComponent
  ],
  exports: [
    ListComponent,
    MainVideosComponent,
    VideoProcComponent,
    UpdateVideoThumbComponent,
    AWSComponent,
    DirectUploaderComponent,
    FFMPEGComponent,
    GeneralVideoUploaderComponent,
    YoutubeComponent,
    VideoUploaderComponent,
    UploadMovieComponent,
    SMVideoListComponent,
    ViewComponent
  ],
  entryComponents: [ViewComponent],
  providers: [SettingsService, DataService, FormService, VideoAPIActions]
})
export class SharedVideoModule {}
