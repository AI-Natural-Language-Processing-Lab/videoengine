/* -------------------------------------------------------------------------- */
/*                          Product Name: VideoEngine                         */
/*                            Author: Mediasoftpro                            */
/*                       Email: support@mediasoftpro.com                      */
/*       License: Read license.txt located on root of your application.       */
/*                     Copyright 2007 - 2020 @Mediasoftpro                    */
/* -------------------------------------------------------------------------- */

import { NgModule } from "@angular/core";
import { NgbModule } from "@ng-bootstrap/ng-bootstrap";
import { CommonModule } from "@angular/common";
import { FormsModule } from "@angular/forms";
import { Routes, RouterModule } from "@angular/router";

// admin specific component
import { VideosComponent } from "./videos.component";
import { VideoProfileModule } from "./profile/process.module";
import { VideoProfileComponent } from "./profile/process.component";

// uploaders
import { DirectVideoUploaderComponent } from "./uploaders/direct/process.component";
import { DirectVideoUploadModule } from "./uploaders/direct/process.module";

import { EmbedYoutubeUploaderComponent } from "./uploaders/youtube/process.component";
import { YoutubeVideoModule } from "./uploaders/youtube/process.module";

import { FFMPEGUPloaderComponent } from "./uploaders/ffmpeg/process.component";
import { FFMPEGVideoModule } from "./uploaders/ffmpeg/process.module";

import { MovieUPloaderComponent } from "./uploaders/movie/process.component";
import { MovieVideoModule } from "./uploaders/movie/process.module";

import { EmbedVideoComponent } from "./uploaders/embed/process.component";
import { EmbedVideoModule } from "./uploaders/embed/process.module";

import { AWSUPloaderComponent } from "./uploaders/aws/process.component";

import { GeneralUploaderComponent } from "../../admin/videos/uploaders/general/general.component";
import { GeneralVideoModule  } from "../../admin/videos/uploaders/general/general.module";

import { AWSVideoModule } from "./uploaders/aws/process.module";

// edit video information
import { VideoProcessModule } from "./process/process.module";
import { VideoProcessComponent } from "./process/process.component";
// update video thumbnail
import { UpdateVideoThumbnailModule } from "./updatethumbnail/process.module";
import { UpdateVideoThumbnailComponent } from "./updatethumbnail/process.component";

// edit video information
import { VideoReportModule } from "./reports/reports.module";
import { VideoReportsComponent } from "./reports/reports.components";

// shared modules
import { PartialModule } from "../../partials/shared.module";
import { SharedVideoModule } from "../../shared/videos/shared.module";

const routes: Routes = [
  {
    path: "",
    data: {
      title: "Videos Management",
      urls: [
        { title: "Dashboard", url: "/" },
        { title: "Videos", url: "/videos" },
        { title: "Management" }
      ]
    },
    component: VideosComponent
  },
  {
    path: "tag/:tagname",
    data: {
      title: "Videos Management",
      urls: [
        { title: "Dashboard", url: "/" },
        { title: "Videos", url: "/videos" },
        { title: "Management" }
      ]
    },
    component: VideosComponent
  },
  {
    path: "category/:catname",
    data: {
      title: "Videos Management",
      urls: [
        { title: "Dashboard", url: "/" },
        { title: "Videos", url: "/videos" },
        { title: "Management" }
      ]
    },
    component: VideosComponent
  },
  {
    path: "user/:uname",
    data: {
      title: "Videos Management",
      urls: [
        { title: "Dashboard", url: "/" },
        { title: "Videos", url: "/videos" },
        { title: "Management" }
      ]
    },
    component: VideosComponent
  },
  {
    path: "filter/:abuse",
    data: {
      title: "Videos Management (Reported Videos)",
      urls: [
        { title: "Dashboard", url: "/" },
        { title: "Videos", url: "/videos" },
        { title: "Reported Videos" }
      ]
    },
    component: VideosComponent
  },
  
  {
    path: "profile/:id",
    data: {
      title: "Video Information",
      urls: [
        { title: "Dashboard", url: "/" },
        { title: "Videos", url: "/videos" },
        { title: "Video Information" }
      ]
    },
    component: VideoProfileComponent
  },

  {
    path: "reports",
    data: {
      title: "Reports Overview",
      urls: [
        { title: "Dashboard", url: "/" },
        { title: "Videos", url: "/videos" },
        { title: "Reports Overview" }
      ]
    },
    component: VideoReportsComponent
  },

  {
    path: "process/:id",
    data: {
      title: "My Account",
      urls: [
        { title: "My Account", url: "/" },
        { title: "Videos", url: "/my-videos" },
        { title: "Edit Video Information" }
      ]
    },
    component: VideoProcessComponent
  },
  {
    path: "updatethumbnail/:id",
    data: {
      title: "My Account",
      urls: [
        { title: "My Account", url: "/" },
        { title: "Videos", url: "/my-videos" },
        { title: "Update Video Thumbnail" }
      ]
    },
    component: UpdateVideoThumbnailComponent
  },
  {
    path: "directuploader/:id",
    data: {
      title: "Direct Video Uploaders",
      urls: [
        { title: "Dashboard", url: "/" },
        { title: "Videos", url: "/videos" },
        { title: "Direct Video Uploader" }
      ]
    },
    component: DirectVideoUploaderComponent
  },
  {
    path: "uploads",
    data: {
      title: "Upload Videos",
      urls: [
        { title: "Dashboard", url: "/" },
        { title: "Videos", url: "/videos" },
        { title: "Upload Videos" }
      ]
    },
    component: GeneralUploaderComponent
  },
  {
    path: "youtube",
    data: {
      title: "Youtube Uploader",
      urls: [
        { title: "Dashboard", url: "/" },
        { title: "Videos", url: "/videos" },
        { title: "Upload Youtube Videos" }
      ]
    },
    component: EmbedYoutubeUploaderComponent
  },
  {
    path: "movie",
    data: {
      title: "Movie Uploader",
      urls: [
        { title: "Dashboard", url: "/" },
        { title: "Videos", url: "/videos" },
        { title: "Upload Movie" }
      ]
    },
    component: MovieUPloaderComponent
  },
  {
    path: "embed",
    data: {
      title: "Embed Movie / Video",
      urls: [
        { title: "Dashboard", url: "/" },
        { title: "Videos", url: "/videos" },
        { title: "Embed Movie / Video" }
      ]
    },
    component: EmbedVideoComponent
  },
  {
    path: "ffmpeg",
    data: {
      title: "FFMPEG Uploader",
      urls: [
        { title: "Dashboard", url: "/" },
        { title: "Videos", url: "/videos" },
        { title: "FFMPEG Video Uploader" }
      ]
    },
    component: FFMPEGUPloaderComponent
  },
  {
    path: "aws",
    data: {
      title: "AWS Video Uploader",
      urls: [
        { title: "Dashboard", url: "/" },
        { title: "Videos", url: "/videos" },
        { title: "AWS Upload Videos" }
      ]
    },
    component: AWSUPloaderComponent
  }
];

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    PartialModule,
    VideoProfileModule,
    DirectVideoUploadModule,
    GeneralVideoModule,
    YoutubeVideoModule,
    FFMPEGVideoModule,
    AWSVideoModule,
    NgbModule,
    SharedVideoModule,
    VideoProcessModule,
    VideoReportModule,
    EmbedVideoModule,
    MovieVideoModule,
    UpdateVideoThumbnailModule,
    RouterModule.forChild(routes)
  ],
  declarations: [
    VideosComponent
  ],
  exports: [VideosComponent]
})
export class VideosModule {}
