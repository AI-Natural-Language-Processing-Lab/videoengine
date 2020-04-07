/* -------------------------------------------------------------------------- */
/*                          Product Name: VideoEngine                         */
/*                            Author: Mediasoftpro                            */
/*                       Email: support@mediasoftpro.com                      */
/*       License: Read license.txt located on root of your application.       */
/*                     Copyright 2007 - 2020 @Mediasoftpro                    */
/* -------------------------------------------------------------------------- */

import { Component } from "@angular/core";

@Component({
  templateUrl: "./playlist.html"
})
export class PlaylistVideosComponent {
  
  isAdmin = false;
  PublicView = false; 
  type = 3; // 0: My Videos, 1: Favorited Videos, 2: Liked Videos, 3: Playlists

  constructor(
  ) {}

}
