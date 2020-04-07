/* -------------------------------------------------------------------------- */
/*                          Product Name: VideoEngine                         */
/*                            Author: Mediasoftpro                            */
/*                       Email: support@mediasoftpro.com                      */
/*       License: Read license.txt located on root of your application.       */
/*                     Copyright 2007 - 2020 @Mediasoftpro                    */
/* -------------------------------------------------------------------------- */

export interface IAPIOptions {
  load: string;
  getinfo: string; // complete detail (for preview purpose)
  getinfo_acc: string; // only video info
  load_reports: string;
  action: string;
  youtube: string;
  fetch_youtube: string;
  remove_audio: string;
  proc: string;
  update_video_info: string;
  editvideo: string;
  deletevideo: string;
  encodevideo: string;
  load_categories: string;
  proc_ffmpeg_videos: string;
  aws_proc: string;
  movie_proc: string;
  embed_proc: string;
  direct_proc: string;
  yt_proc: string;
  update_video_thumb: string;
  authorize_author: string;
}

export interface VideoEntity {
  id: number;
  userid: string;
  title: string;
  description: string;
  tags: string;
  categories: any;
  category_list: any;
  files: any;
  views: number;
  liked: number;
  actors: string;
  actresses: string;
  iscomments?: number;
  isratings?: number;
  isprivate?: number;
  isenabled?: number;
  isapproved?: number;
  isadult?: number;
  isfeatured?: number;
  disliked?: number;
  favorites?: number;
}

export interface VideoThumbnailEntity {
  video_thumbs: any;
}
