/* -------------------------------------------------------------------------- */
/*                          Product Name: VideoEngine                         */
/*                            Author: Mediasoftpro                            */
/*                       Email: support@mediasoftpro.com                      */
/*       License: Read license.txt located on root of your application.       */
/*                     Copyright 2007 - 2020 @Mediasoftpro                    */
/* -------------------------------------------------------------------------- */

import { NgModule } from "@angular/core";
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { CommonModule } from "@angular/common";
import { FormsModule } from "@angular/forms";
import { Routes, RouterModule } from "@angular/router";

import { VideosComponent } from "./videos.component";

// Other pages
import { FavoritedVideosComponent } from "./favorites/favorites.component";
import { LikedVideosComponent } from "./liked/liked.component";
import { PlaylistVideosComponent } from "./playlists/playlist.component";

// video uploaders
import { DirectVideoUploaderComponent } from "../../account/videos/uploaders/direct/process.component";
import { DirectVideoUploadModule } from "../../account/videos/uploaders/direct/process.module";

import { EmbedYoutubeUploaderComponent } from "../../account/videos/uploaders/youtube/process.component";
import { YoutubeVideoModule } from "../../account/videos/uploaders/youtube/process.module";

import { FFMPEGUPloaderComponent } from "../../account/videos/uploaders/ffmpeg/process.component";
import { FFMPEGVideoModule } from "../../account/videos/uploaders/ffmpeg/process.module";

import { AWSUPloaderComponent } from "../../account/videos/uploaders/aws/process.component";
import { AWSVideoModule } from "../../account/videos/uploaders/aws/process.module";

import { MovieUPloaderComponent } from "../../account/videos/uploaders/movie/process.component";
import { MovieVideoModule } from "../../account/videos/uploaders/movie/process.module";

import { EmbedVideoComponent } from "./uploaders/embed/process.component";
import { EmbedVideoModule } from "./uploaders/embed/process.module";

import { GeneralUploaderComponent } from "../../account/videos/uploaders/general/general.component";
import { GeneralVideoModule  } from "../../account/videos/uploaders/general/general.module";

// edit video information
import { VideoProcessModule } from "./process/process.module";
import { VideoProcessComponent } from "./process/process.component";
// update video thumbnail
import { UpdateVideoThumbnailModule } from "./updatethumbnail/process.module";
import { UpdateVideoThumbnailComponent } from "./updatethumbnail/process.component";

// shared modules
import { PartialModule } from "../../partials/shared.module";
import { SharedVideoModule } from "../../shared/videos/shared.module";

import { NavigationMenuIndex } from "../../configs/settings";


const routes: Routes = [
  {
    path: "",
    data: {
      topmenuIndex: NavigationMenuIndex.TOPMENU_VIDEOS_INDEX,
      leftmenuIndex: NavigationMenuIndex.VIDEOS_MY_INDEX,
      title: "Account",
      urls: [{ title: "My Account", url: "/" }, { title: "Manage Videos" }]
    },
    component: VideosComponent
  },
  {
    path: "process/:id",
    data: {
      topmenuIndex: NavigationMenuIndex.TOPMENU_VIDEOS_INDEX,
      leftmenuIndex: NavigationMenuIndex.VIDEOS_MY_INDEX,
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
      topmenuIndex: NavigationMenuIndex.TOPMENU_VIDEOS_INDEX,
      leftmenuIndex: NavigationMenuIndex.VIDEOS_MY_INDEX,
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
      topmenuIndex: NavigationMenuIndex.TOPMENU_VIDEOS_INDEX,
      leftmenuIndex: NavigationMenuIndex.VIDEOS_MY_INDEX,
      title: "My Account",
      urls: [
        { title: "My Account", url: "/" },
        { title: "Videos", url: "/my-videos" },
        { title: "Upload Videos" }
      ]
    },
    component: DirectVideoUploaderComponent
  },
  {
    path: "youtube",
    data: {
      topmenuIndex: NavigationMenuIndex.TOPMENU_VIDEOS_INDEX,
      leftmenuIndex: NavigationMenuIndex.VIDEOS_MY_INDEX,
      title: "My Account",
      urls: [
        { title: "My Account", url: "/" },
        { title: "Videos", url: "/my-videos" },
        { title: "Upload Videos" }
      ]
    },
    component: EmbedYoutubeUploaderComponent
  },
  {
    path: "ffmpeg",
    data: {
      topmenuIndex: NavigationMenuIndex.TOPMENU_VIDEOS_INDEX,
      leftmenuIndex: NavigationMenuIndex.VIDEOS_MY_INDEX,
      title: "My Account",
      urls: [
        { title: "My Account", url: "/" },
        { title: "Videos", url: "/my-videos" },
        { title: "Upload Videos" }
      ]
    },
    component: FFMPEGUPloaderComponent
  },
  {
    path: "aws",
    data: {
      topmenuIndex: NavigationMenuIndex.TOPMENU_VIDEOS_INDEX,
      leftmenuIndex: NavigationMenuIndex.VIDEOS_MY_INDEX,
      title: "My Account",
      urls: [
        { title: "My Account", url: "/" },
        { title: "Videos", url: "/my-videos" },
        { title: "Upload Videos" }
      ]
    },
    component: AWSUPloaderComponent
  },
  {
    path: "movie",
    data: {
      topmenuIndex: NavigationMenuIndex.TOPMENU_VIDEOS_INDEX,
      leftmenuIndex: NavigationMenuIndex.VIDEOS_MY_INDEX,
      title: "My Account",
      urls: [
        { title: "My Account", url: "/" },
        { title: "Videos", url: "/my-videos" },
        { title: "Upload Movie" }
      ]
    },
    component: MovieUPloaderComponent
  },
  {
    path: "embed",
    data: {
      topmenuIndex: NavigationMenuIndex.TOPMENU_VIDEOS_INDEX,
      leftmenuIndex: NavigationMenuIndex.VIDEOS_MY_INDEX,
      title: "My Account",
      urls: [
        { title: "My Account", url: "/" },
        { title: "Videos", url: "/my-videos" },
        { title: "Embed Movie / Video" }
      ]
    },
    component: EmbedVideoComponent
  },
  {
    path: "uploads",
    data: {
      topmenuIndex: NavigationMenuIndex.TOPMENU_VIDEOS_INDEX,
      leftmenuIndex: NavigationMenuIndex.VIDEOS_MY_INDEX,
      title: "My Account",
      urls: [
        { title: "My Account", url: "/" },
        { title: "Videos", url: "/my-videos" },
        { title: "Upload Videos" }
      ]
    },
    component: GeneralUploaderComponent
  },
  {
    path: "favorites",
    data: {
      topmenuIndex: NavigationMenuIndex.TOPMENU_VIDEOS_INDEX,
      leftmenuIndex: NavigationMenuIndex.VIDEOS_FAVORITES_INDEX,
      title: "My Account",
      urls: [
        { title: "My Account", url: "/" },
        { title: "Videos", url: "/my-videos" },
        { title: "Favorited Videos" }
      ]
    },
    component: FavoritedVideosComponent
  },
  {
    path: "liked",
    data: {
      topmenuIndex: NavigationMenuIndex.TOPMENU_VIDEOS_INDEX,
      leftmenuIndex: NavigationMenuIndex.VIDEOS_LIKED_INDEX,
      title: "My Account",
      urls: [
        { title: "My Account", url: "/" },
        { title: "Videos", url: "/my-videos" },
        { title: "Liked Videos" }
      ]
    },
    component: LikedVideosComponent
  },
  {
    path: "playlists",
    data: {
      topmenuIndex: NavigationMenuIndex.TOPMENU_VIDEOS_INDEX,
      leftmenuIndex: NavigationMenuIndex.VIDEOS_PLAYLIST_INDEX,
      title: "My Account",
      urls: [
        { title: "My Account", url: "/" },
        { title: "Videos", url: "/my-videos" },
        { title: "My Playlists" }
      ]
    },
    component: PlaylistVideosComponent
  }
];

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    PartialModule,
    DirectVideoUploadModule,
    YoutubeVideoModule,
    FFMPEGVideoModule,
    AWSVideoModule,
    NgbModule,
    SharedVideoModule,
    VideoProcessModule,
    UpdateVideoThumbnailModule,
    MovieVideoModule,
    GeneralVideoModule,
    EmbedVideoModule,
    RouterModule.forChild(routes)
  ],
  declarations: [
    VideosComponent,
    LikedVideosComponent,
    PlaylistVideosComponent,
    FavoritedVideosComponent
  ],
  exports: [VideosComponent]
})
export class VideosModule {}
