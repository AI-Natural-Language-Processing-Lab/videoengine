/* -------------------------------------------------------------------------- */
/*                          Product Name: VideoEngine                         */
/*                            Author: Mediasoftpro                            */
/*                       Email: support@mediasoftpro.com                      */
/*       License: Read license.txt located on root of your application.       */
/*                     Copyright 2007 - 2020 @Mediasoftpro                    */
/* -------------------------------------------------------------------------- */

import { Injectable } from "@angular/core";
import * as Controls from "../../../partials/components/dynamicform/model/elements";
import { FormBase } from "../../../partials/components/dynamicform/model/base";
import * as OPTIONS from "../videos.model";
import { AppConfig } from "../../../configs/app.config";
import { CoreService } from "../../../admin/core/coreService";

@Injectable()
export class FormService {
  constructor(public config: AppConfig, private coreService: CoreService) {}

  getControls(entity: OPTIONS.VideoEntity, auth: any) {
    const controls: FormBase<any>[] = [];

    controls.push(
      new Controls.Textbox({
        key: "tags",
        label: "Tags",
        value: entity.tags,
        required: false,
        order: 2,
        maxLength: 1000,
        helpblock: `Enter one or more tags separated by comma`
      })
    );

    controls.push(
      new Controls.MultiDropdown({
        key: "categories",
        label: "Select Categories",
        value: this.coreService.prepareSelectedItems(entity.category_list),
        multiselectOptions: this.coreService.getMultiCategorySettings(),
        required: true,
        helpblock: `Select one or more categories to associate blog post`,
        order: 3
      })
    );
    controls.push(
      new Controls.Uploader({
        key: "files",
        label: "",
        value: entity.files,
        required: false,
        helpblock: "",
        uploadoptions: {
          // photouploader: true, // target photo uploader (to format uploaded photos in proper layout)
          handlerpath: this.config.getConfig("host") + "api/videos/upload",
          pickfilecaption: "Select MP4 Videos",
          uploadfilecaption: "Start Uploading",
          colcss: "col-md-4",
          pickbuttoncss: "btn btn-danger ",
          maxfilesize: "1000mb",
          unique_names: true,
          chunksize: "8mb",
          headers: {},
          extensiontitle: "MP4 Video Files",
          extensions: "mp4",
          filepath: "",
          username: auth.User.id, // 'administrator',
          removehandler: "",
          maxallowedfiles: 5,
          showFileName: false, // show filename with media file
          showoriginalSize: false, // show media in original size
          value: entity.files
        },
        order: 0
      })
    );

    return controls.sort((a, b) => a.order - b.order);
  }

  editVideoControls(entity: OPTIONS.VideoEntity, auth: any, isadmin: boolean) {
    const controls: FormBase<any>[] = [];
    controls.push(
      new Controls.Textbox({
        key: "title",
        label: "Title",
        value: entity.title,
        required: true,
        order: 0,
        maxLength: 150,
        helpblock: `Enter Title`
      })
    );

    controls.push(
      new Controls.TinyMyceEditor({
        key: "description",
        label: "Description",
        value: entity.description,
        tinymiceOptions: this.coreService.prepareInitEditorSettings(),
        required: true,
        order: 1
      })
    );

    controls.push(
      new Controls.MultiDropdown({
        key: "categories",
        label: "Select Categories",
        value: this.coreService.prepareSelectedItems(entity.category_list),
        multiselectOptions: this.coreService.getMultiCategorySettings(),
        required: true,
        helpblock: `Select one or more categories to associate blog post`,
        order: 3
      })
    );

    controls.push(
      new Controls.Textbox({
        key: "tags",
        label: "Tags",
        value: entity.tags,
        required: true,
        order: 2,
        maxLength: 1000,
        helpblock: `Enter one or more tags separated by comma`
      })
    );

    controls.push(
      new Controls.RadioButtonList({
        key: "iscomments",
        label: "Comments",
        value: entity.iscomments.toString(),
        required: false,
        order: 3,
        options: [
          {
            key: '1',
            value: "Yes"
          },
          {
            key: '0',
            value: "No"
          }
        ]
      })
    );

    controls.push(
      new Controls.RadioButtonList({
        key: "isratings",
        label: "Ratings",
        value: entity.isratings.toString(),
        required: false,
        order: 4,
        options: [
          {
            key: '1',
            value: "Yes"
          },
          {
            key: '0',
            value: "No"
          }
        ]
      })
    );

    controls.push(
      new Controls.RadioButtonList({
        key: "isprivate",
        label: "Privacy",
        value: entity.isprivate.toString(),
        required: false,
        order: 5,
        options: [
          {
            key: "0",
            value: "Public"
          },
          {
            key: "1",
            value: "Private"
          },
          {
            key: "2",
            value: "Unlisted"
          }
        ]
      })
    );

    if (isadmin) {
      controls.push(
        new Controls.RadioButtonList({
          key: "isenabled",
          label: "Status",
          value: entity.isenabled.toString(),
          required: false,
          order: 6,
          options: [
            {
              key: '1',
              value: "Enabled"
            },
            {
              key: '0',
              value: "Disabled"
            }
          ]
        })
      );

      controls.push(
        new Controls.RadioButtonList({
          key: "isapproved",
          label: "Approved",
          value: entity.isapproved.toString(),
          required: false,
          order: 7,
          options: [
            {
              key: '1',
              value: "Yes"
            },
            {
              key: '0',
              value: "No"
            }
          ]
        })
      );

      controls.push(
        new Controls.RadioButtonList({
          key: "isfeatured",
          label: "Featured",
          value: entity.isfeatured.toString(),
          required: false,
          order: 7,
          options: [
            {
              key: '1',
              value: "Featured"
            },
            {
              key: '2',
              value: "Premium"
            },
            {
              key: '0',
              value: "Normal"
            }
          ]
        })
      );

      controls.push(
        new Controls.RadioButtonList({
          key: "isadult",
          label: "Adult Status",
          value: entity.isadult.toString(),
          required: false,
          order: 7,
          options: [
            {
              key: '1',
              value: "Yes"
            },
            {
              key: '0',
              value: "No"
            }
          ]
        })
      );

      controls.push(
        new Controls.Textbox({
          key: "liked",
          label: "Liked",
          value: entity.liked.toString(),
          required: false,
          order: 8,
          maxLength: 150,
          helpblock: `Update liked information`
        })
      );

      controls.push(
        new Controls.Textbox({
          key: "disliked",
          label: "Disliked",
          value: entity.disliked.toString(),
          required: false,
          order: 9,
          maxLength: 150,
          helpblock: `Update disliked information`
        })
      );

      controls.push(
        new Controls.Textbox({
          key: "views",
          label: "Views",
          value: entity.views.toString(),
          required: false,
          order: 11,
          maxLength: 150,
          helpblock: `Update views information`
        })
      );

      controls.push(
        new Controls.Textbox({
          key: "favorites",
          label: "Favorites",
          value: entity.favorites.toString(),
          required: false,
          order: 11,
          maxLength: 150,
          helpblock: `Update favorites information`
        })
      );
    }

    return controls.sort((a, b) => a.order - b.order);
  }

  getCoverControls(entity: OPTIONS.VideoThumbnailEntity, auth: any) {
    const controls: FormBase<any>[] = [];

    controls.push(
      new Controls.Uploader({
        key: "video_thumbs",
        label: "",
        value: entity.video_thumbs,
        required: false,
        helpblock: "",
        uploadoptions: {
          // photouploader: true, // target photo uploader (to format uploaded photos in proper layout)
          handlerpath:
            this.config.getConfig("host") + "api/videos/thumbuploads",
          pickfilecaption: "Select Thumbnails",
          uploadfilecaption: "Start Uploading",
          colcss: "col-md-4",
          pickbuttoncss: "btn btn-danger ",
          maxfilesize: "8mb",
          chunksize: "8mb",
          unique_names: true,
          headers: {},
          extensiontitle: "Images Files",
          extensions: "jpg,jpeg,png",
          filepath: "",
          username: auth.User.id, // 'administrator',
          removehandler: "",
          maxallowedfiles: 1000,
          showFileName: false, // show filename with media file
          showoriginalSize: false, // show media in original size
          value: entity.video_thumbs
        },
        order: 4
      })
    );

    return controls.sort((a, b) => a.order - b.order);
  }

  getVideoEditControls(entity: OPTIONS.VideoEntity, isadmin: boolean) {
    const controls: FormBase<any>[] = [];

    controls.push(
      new Controls.Textbox({
        key: "title",
        label: "Title",
        value: entity.title,
        required: true,
        order: 0,
        maxLength: 300,
        placeholder: "Enter title"
        // helpblock: `Enter post title`
      })
    );

    controls.push(
      new Controls.TextArea({
        key: "description",
        label: "Description",
        value: entity.description,
        required: false,
        helpblock: `Enter description`,
        order: 1
      })
    );

    controls.push(
      new Controls.Textbox({
        key: "tags",
        label: "Tags",
        value: entity.tags,
        required: false,
        order: 2,
        maxLength: 1000,
        helpblock: `Enter one or more tags separated by comma`
      })
    );

    controls.push(
      new Controls.Textbox({
        key: "views",
        label: "Views",
        value: entity.views.toString(),
        required: false,
        order: 3,
        maxLength: 8,
        colsize: "col-md-6"
      })
    );

    controls.push(
      new Controls.Textbox({
        key: "liked",
        label: "Likes",
        value: entity.liked.toString(),
        required: false,
        order: 4,
        maxLength: 8,
        colsize: "col-md-6"
      })
    );

    controls.push(
      new Controls.MultiDropdown({
        key: "categories",
        label: "Select Categories",
        value: this.coreService.prepareSelectedItems(entity.category_list),
        multiselectOptions: this.coreService.getMultiCategorySettings(),
        required: true,
        helpblock: `Select one or more categories to associate blog post`,
        order: 3
      })
    );

    if (isadmin) {
      controls.push(
        new Controls.Textbox({
          key: "actors",
          label: "Actors",
          value: entity.actors,
          required: false,
          order: 6,
          maxLength: 1000,
          helpblock: `Enter one or more tags separated by comma`
        })
      );

      controls.push(
        new Controls.Textbox({
          key: "actresses",
          label: "Actresses",
          value: entity.actresses,
          required: false,
          order: 7,
          maxLength: 1000,
          helpblock: `Enter one or more tags separated by comma`
        })
      );
    }

    controls.push(
      new Controls.RadioButtonList({
        key: "iscomments",
        label: "Comments",
        value: entity.iscomments,
        required: false,
        order: 3,
        options: [
          {
            key: 1,
            value: "Yes"
          },
          {
            key: 0,
            value: "No"
          }
        ]
      })
    );

    controls.push(
      new Controls.RadioButtonList({
        key: "isratings",
        label: "Ratings",
        value: entity.isratings,
        required: false,
        order: 4,
        options: [
          {
            key: 1,
            value: "Yes"
          },
          {
            key: 0,
            value: "No"
          }
        ]
      })
    );

    controls.push(
      new Controls.RadioButtonList({
        key: "isprivate",
        label: "Privacy",
        value: entity.isprivate.toString(),
        required: false,
        order: 5,
        options: [
          {
            key: "0",
            value: "Public"
          },
          {
            key: "1",
            value: "Private"
          },
          {
            key: "2",
            value: "Unlisted"
          }
        ]
      })
    );

    return controls.sort((a, b) => a.order - b.order);
  }

  uploaderOptionControls(entity: OPTIONS.VideoEntity) {
    const controls: FormBase<any>[] = [];

    controls.push(
      new Controls.Textbox({
        key: "tags",
        label: "Tags",
        value: entity.tags,
        required: false,
        order: 2,
        maxLength: 1000,
        helpblock: `Enter one or more tags separated by comma`
      })
    );

    controls.push(
      new Controls.MultiDropdown({
        key: "categories",
        label: "Select Categories",
        value: this.coreService.prepareSelectedItems(entity.category_list),
        multiselectOptions: this.coreService.getMultiCategorySettings(),
        required: true,
        helpblock: `Select one or more categories to associate blog post`,
        order: 3
      })
    );

    return controls.sort((a, b) => a.order - b.order);
  }

  uploadMovieControls(entity: any) {
    const controls: FormBase<any>[] = [];

    controls.push(
      new Controls.Textbox({
        key: "title",
        label: "Title",
        value: entity.title,
        required: true,
        order: 0,
        maxLength: 150,
        helpblock: `Enter movie or video title`
      })
    );

    controls.push(
      new Controls.TinyMyceEditor({
        key: "description",
        label: "Description",
        value: entity.description,
        tinymiceOptions: this.coreService.prepareInitAdvacneEditorSettings(),
        required: true,
        order: 1
      })
    );
    controls.push(
      new Controls.Textbox({
        key: "tags",
        label: "Tags",
        value: entity.tags,
        required: false,
        order: 2,
        maxLength: 300,
        helpblock: `Enter one or more tags separated by comma`
      })
    );

    controls.push(
      new Controls.MultiDropdown({
        key: "categories",
        label: "Select Categories",
        value: this.coreService.prepareSelectedItems(entity.category_list),
        multiselectOptions: this.coreService.getMultiCategorySettings(),
        required: true,
        helpblock: `Select one or more categories to associate blog post`,
        order: 3
      })
    );

    controls.push(
      new Controls.Textbox({
        key: "duration",
        label: "Duration",
        value: entity.duration,
        required: true,
        order: 4,
        maxLength: 30,
        helpblock: `Put duration in hh:mm:ss format`
      })
    );

    controls.push(
      new Controls.Dropdown({
        key: "movietype",
        label: "Movie Type",
        value: entity.movietype.toString(),
        options: [
          {
            key: "0",
            value: "Clip"
          },
          {
            key: "1",
            value: "Movie"
          }
        ],
        order: 5
      })
    );

    controls.push(
      new Controls.Dropdown({
        key: "isapproved",
        label: "Visible",
        value: entity.isapproved.toString(),
        options: [
          {
            key: "0",
            value: "No"
          },
          {
            key: "1",
            value: "Yes"
          }
        ],
        order: 5
      })
    );

    controls.push(
      new Controls.SectionHeader({
        key: "config_section_01",
        label: "File Paths",
        order: 6
      })
    );

    controls.push(
      new Controls.Textbox({
        key: "pub_url",
        label: "Stream Video Url",
        value: entity.pub_url,
        required: true,
        order: 7,
        maxLength: 200,
        helpblock: `Put complete url or relative url (e.g /movie/sample.mp4) to automatically adjust endpoint set in general settings`
      })
    );

    controls.push(
      new Controls.Textbox({
        key: "org_url",
        label: "Original Video / Download Url",
        value: entity.org_url,
        required: true,
        order: 8,
        maxLength: 200,
        helpblock: `Put complete url or relative url (e.g /movie/sample.mp4) to automatically adjust endpoint set in general settings`
      })
    );

    controls.push(
      new Controls.Textbox({
        key: "preview_url",
        label: "Preview Video Url",
        value: entity.preview_url,
        required: true,
        order: 8,
        maxLength: 200,
        helpblock: `Put complete url or relative url (e.g /movie/sample.mp4) to automatically adjust endpoint set in general settings`
      })
    );

    controls.push(
      new Controls.Textbox({
        key: "thumb_url",
        label: "Thumbnail Url",
        value: entity.thumb_url,
        required: true,
        order: 9,
        maxLength: 200,
        helpblock: `Put complete url or relative url (e.g /movie/sample.mp4) to automatically adjust endpoint set in general settings`
      })
    );

    controls.push(
      new Controls.Textbox({
        key: "coverurl",
        label: "Cover Url",
        value: entity.coverurl,
        required: true,
        order: 10,
        maxLength: 200,
        helpblock: `Put complete url or relative url (e.g /movie/sample.mp4) to automatically adjust endpoint set in general settings`
      })
    );

    controls.push(
      new Controls.Textbox({
        key: "price",
        label: "Price",
        value: entity.price.toString(),
        required: true,
        order: 11,
        helpblock: `e.g $9`
      })
    );
    return controls.sort((a, b) => a.order - b.order);
  }

  embedVideoControls(entity: any) {
    const controls: FormBase<any>[] = [];

    controls.push(
      new Controls.Textbox({
        key: "title",
        label: "Title",
        value: entity.title,
        required: true,
        order: 0,
        maxLength: 150,
        helpblock: `Enter movie or video title`
      })
    );

    controls.push(
      new Controls.TinyMyceEditor({
        key: "description",
        label: "Description",
        value: entity.description,
        tinymiceOptions: this.coreService.prepareInitAdvacneEditorSettings(),
        required: true,
        order: 1
      })
    );
    controls.push(
      new Controls.Textbox({
        key: "tags",
        label: "Tags",
        value: entity.tags,
        required: false,
        order: 2,
        maxLength: 300,
        helpblock: `Enter one or more tags separated by comma`
      })
    );

    controls.push(
      new Controls.MultiDropdown({
        key: "categories",
        label: "Select Categories",
        value: this.coreService.prepareSelectedItems(entity.category_list),
        multiselectOptions: this.coreService.getMultiCategorySettings(),
        required: true,
        helpblock: `Select one or more categories to associate blog post`,
        order: 3
      })
    );
    
    controls.push(
      new Controls.Dropdown({
        key: "movietype",
        label: "Movie Type",
        value: entity.movietype.toString(),
        options: [
          {
            key: "0",
            value: "Clip"
          },
          {
            key: "1",
            value: "Movie"
          }
        ],
        order: 5
      })
    );

    controls.push(
      new Controls.TextArea({
        key: "embed_script",
        label: "Embed Script",
        value: entity.embed_script,
        required: true,
        order: 7,
        maxLength: 200,
        helpblock: `Put video embed script here`
      })
    );

    controls.push(
      new Controls.Textbox({
        key: "thumb_url",
        label: "Thumbnail Url",
        value: entity.thumb_url,
        required: true,
        order: 9,
        maxLength: 200,
        helpblock: `Put complete thumbnail url `
      })
    );

    return controls.sort((a, b) => a.order - b.order);
  }
}
