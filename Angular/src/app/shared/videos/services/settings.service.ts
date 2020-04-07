/* -------------------------------------------------------------------------- */
/*                          Product Name: VideoEngine                         */
/*                            Author: Mediasoftpro                            */
/*                       Email: support@mediasoftpro.com                      */
/*       License: Read license.txt located on root of your application.       */
/*                     Copyright 2007 - 2020 @Mediasoftpro                    */
/* -------------------------------------------------------------------------- */

import { Injectable } from "@angular/core";
import * as OPTIONS from "../videos.model";
import { iStyleConfig } from "../../../admin/core/core.model";
import { AppConfig } from "../../../configs/app.config";
import { CoreService } from "../../../admin/core/coreService";
import { ButtonCSS, ICONCSS, ThemeCSS } from "../../../configs/themeSettings";
import { ContentTypes } from "../../../configs/settings";

@Injectable()
export class SettingsService {
  // configurations
  private apiOptions: OPTIONS.IAPIOptions;
  private Config: iStyleConfig;

  constructor(private coreService: CoreService, public config: AppConfig) {
    const APIURL = config.getConfig("host");
    this.apiOptions = {
      load: APIURL + "api/videos/load",
      load_reports: APIURL + "api/videos/load_reports",
      getinfo: APIURL + "api/videos/getinfo",
      getinfo_acc: APIURL + "api/videos/getinfo_acc",
      action: APIURL + "api/videos/action",
      youtube: APIURL + "api/videos/get_yt_categories",
      fetch_youtube: APIURL + "api/videos/fetch_youtube",
      remove_audio: APIURL + "api/videos/remove_audio",
      proc: APIURL + "api/videos/proc_new",
      update_video_info: APIURL + "api/videos/update_video_info",
      editvideo: APIURL + "api/videos/editvideos",
      deletevideo: APIURL + "api/videos/deletevideo",
      encodevideo: APIURL + "api/ffmpeg/encode",
      load_categories: APIURL + "api/categories/load_nm",
      proc_ffmpeg_videos: APIURL + "api/videos/ffmpeg_proc",
      aws_proc: APIURL + "api/videos/aws_proc",
      movie_proc: APIURL + "api/videos/movie_proc",
      embed_proc: APIURL + "api/videos/embed_proc",
      yt_proc: APIURL + "api/videos/yt_proc",
      direct_proc: APIURL + "api/videos/direct_proc",
      update_video_thumb: APIURL + "api/videos/update_thumbnail",
      authorize_author: APIURL + "api/videos/authorize_author"
    };
  }

  init_admin_search_options(settings: any) {
    return {
      showpanel: true, // show, hide whole panel
      showSearchPanel: true,
      showAdvanceSearchLink: true,
      term: "",
      topselectioncheck: true,
      navList: [],
      filters: [
        {
          id: 1,
          title: "Featured",
          value: 1,
          default_value: 3,
          selected: false,
          attr: "isfeatured"
        },
        {
          id: 3,
          title: "Adult",
          value: 1,
          default_value: 2,
          selected: false,
          attr: "isadult"
        }
      ],
      dropdownFilters: [
        {
          id: 1,
          value: 0,
          group: "datefilter",
          caption: "Added",
          attr: "datefilter",
          options: [
            {
              id: 1,
              title: "Today",
              value: 1
            },
            {
              id: 2,
              title: "This Week",
              value: 2
            },
            {
              id: 3,
              title: "This Month",
              value: 3
            },
            {
              id: 4,
              title: "All Time",
              value: 0
            }
          ]
        },
        {
          id: 2,
          value: "video.created_at desc",
          group: "order",
          caption: "Order",
          attr: "order",
          options: [
            {
              id: 1,
              title: "Uploaded Date",
              value: "video.created_at desc"
            },
            {
              id: 2,
              title: "Title ASC",
              value: "video.title asc"
            },
            {
              id: 4,
              title: "Title DESC",
              value: "video.title desc"
            },
            {
              id: 5,
              title: "Most Viewed",
              value: "video.views desc"
            },
            {
              id: 7,
              title: "Most Rated",
              value: "video.liked desc"
            },
            {
              id: 6,
              title: "Approved",
              value: "isapproved desc"
            },
            {
              id: 7,
              title: "Unapproved",
              value: "isapproved asc"
            },
            {
              id: 7,
              title: "Enabled",
              value: "isenabled desc"
            },
            {
              id: 7,
              title: "Disabled",
              value: "isenabled asc"
            }
          ]
        }
      ],
      checkFilters: [
        /*{
          id: 4,
          value: 0,
          group: "type",
          caption: "Media Type",
          attr: "type",
          options: [
            {
              id: 4,
              title: "Videos",
              value: 0
            },
            {
              id: 5,
              title: "Audio",
              value: 1
            }
          ]
        },*/
        {
          id: 1,
          value: 2,
          group: "isenabled",
          caption: "Enable Status",
          attr: "isenabled",
          options: [
            {
              id: 4,
              title: "Active",
              value: 1
            },
            {
              id: 5,
              title: "Inactive",
              value: 0
            },
            {
              id: 6,
              title: "Any",
              value: 2
            }
          ]
        },
        {
          id: 1,
          value: 2,
          group: "isapproved",
          caption: "Approval Status",
          attr: "isapproved",
          options: [
            {
              id: 4,
              title: "Approved",
              value: 1
            },
            {
              id: 5,
              title: "Under Review",
              value: 0
            },
            {
              id: 6,
              title: "Any",
              value: 2
            }
          ]
        },
        {
          id: 1,
          value: 3,
          group: "privacy",
          caption: "Privacy",
          attr: "isprivate",
          options: [
            {
              id: 1,
              title: "Private",
              value: 1
            },
            {
              id: 2,
              title: "Public",
              value: 0
            },
            {
              id: 3,
              title: "Unlisted",
              value: 2
            },
            {
              id: 3,
              title: "Any",
              value: 3
            }
          ]
        }
      ],
      categories: [],
      multiselectOptions: this.coreService.getMultiCategorySettings(),
      selectedcategory: "",
      singleaction: false,
      actions: []
    };
  }

  init_account_search_options(settings: any) {
    return {
      showpanel: true, // show, hide whole panel
      showSearchPanel: true,
      term: "",
      topselectioncheck: true,
      singleaction: false,
      actions: [],
      filters: [],
      checkFilters: [],
      categories: [],
      dropdownFilters: [
      {
          id: 1,
          value: 0,
          group: "datefilter",
          caption: "Added",
          attr: "datefilter",
          options: [
            {
              id: 1,
              title: "Today",
              value: 1
            },
            {
              id: 2,
              title: "This Week",
              value: 2
            },
            {
              id: 3,
              title: "This Month",
              value: 3
            },
            {
              id: 4,
              title: "This Year",
              value: 4
            },
            {
              id: 5,
              title: "All Time",
              value: 0
            }
          ]
        },
        {
          id: 2,
          value: "video.created_at desc",
          group: "order",
          caption: "Order",
          attr: "order",
          options: [
            {
              id: 1,
              title: "Uploaded Date",
              value: "video.created_at desc"
            },
            {
              id: 2,
              title: "Title ASC",
              value: "video.title asc"
            },
            {
              id: 4,
              title: "Title DESC",
              value: "video.title desc"
            },
            {
              id: 5,
              title: "Most Viewed",
              value: "video.views desc"
            },
            {
              id: 7,
              title: "Top Rated",
              value: "video.liked desc"
            }
          ]
        }
      ]
    };
  }

  getNavActions(isadmin: boolean, settings: any) {
    const actions: any = [];
    let css = "btn m-b-5 btn-block btn-primary";
    if (!isadmin) { 
       css = "btn btn-primary";
    } 

    actions.push({
      id: actions.length + 1,
      title: "Uploads",
      tooltip: "Upload Videos",
      css: css,
      event: "upload"
    });

    if (isadmin) {
      actions.push({
        id: 100,
        title: "Abuse Reports",
        tooltip: "Load Reported Records",
        css: "btn btn-block m-b-5 btn-info",
        event: "abuse"
      });
      
      actions.push({
        id: 101,
        title: "Reports",
        tooltip: "Load Reports",
        css: "btn btn-block m-b-5 btn-info",
        event: "reports"
      });
    }
    
    /*if (settings.general !== undefined) {
      if (isadmin) {
        if (settings.aws.enable) {
          actions.push({
            id: actions.length + 1,
            title: "Upload [AWS]",
            tooltip: "Upload videos using AWS Uploader",
            css: "btn m-b-5 btn-block btn-primary",
            event: "aws"
          });
        }
        if (settings.direct.enable) {
          actions.push({
            id: actions.length + 1,
            title: "Upload [Direct]",
            tooltip: "Upload videos using Direct Uploader",
            css: "btn m-b-5 btn-block btn-primary",
            event: "add"
          });
        }
        if (settings.youtube.enable) {
          actions.push({
            id: 3,
            title: "Upload [Youtube]",
            tooltip: "Upload videos using Youtube Uploader",
            css: "btn m-b-5 btn-block btn-primary",
            event: "youtube"
          });
        }
        if (settings.ffmpeg.enable) {
          actions.push({
            id: 3,
            title: "Upload [FFMPEG]",
            tooltip: "Upload videos using Open Source FFMPEG",
            css: "btn m-b-5 btn-block btn-primary",
            event: "ffmpeg"
          });
        }
        return actions;
      } else {
        if (settings.aws.enable) {
          actions.push({
            id: actions.length + 1,
            title: "Upload",
            tooltip: "Upload Videos",
            css: "btn m-b-5 btn-block btn-primary",
            event: "aws"
          });
        } else if (settings.direct.enable) {
          actions.push({
            id: actions.length + 1,
            title: "Upload",
            tooltip: "Upload Videos",
            css: "btn m-b-5 btn-primary m-l-5",
            event: "add"
          });
        } else if (settings.ffmpeg.enable) {
          actions.push({
            id: 3,
            title: "Upload",
            tooltip: "Upload Videos",
            css: "btn m-b-5 btn-primary m-l-5",
            event: "ffmpeg"
          });
        }
        if (settings.youtube.enable) {
          actions.push({
            id: 3,
            title: "Youtube Upload",
            tooltip: "Upload Youtube Videos",
            css: "btn btn-primary m-l-5",
            event: "youtube"
          });
        }

        return actions;
      }
    }*/

    return actions;
  }

  init_admin_toolbar_options() {
    let options = {
      showtoolbar: true,
      showsecondarytoolbar: true,
      showcheckAll: true,
      navbarcss: ThemeCSS.NAVBAR_CSS,
      left_options: [],
      left_caption: "",
      right_caption: "",
      right_options: [],
      actions: []
    };
    /*for (const type of ContentTypes.MEDIA_TYPES) {
      options.left_options.push({
        title: type.title,
        ismultiple: false,
        icon: "", // icon-sort-amount-desc position-left
        Options: [],
        clickevent: "f_type",
        value: type.value,
        tooltip: "Load " + type.title
      });
    }*/

    options.actions.push({
      title: "Mark As",
      showOnMainBar: false,
      ismultiple: true,
      icon: "",
      Options: [
        {
          id: "0",
          title: "Approve",
          value: 1,
          actionstatus: "approve",
          attr: "isapproved",
          isclick: true,
          clickevent: "m_markas",
          icon: "",
          css: ButtonCSS.SUCCESS_BUTTON,
          tooltip: "Approve selected records"
        },
        {
          id: "1",
          title: "Enable",
          value: 1,
          actionstatus: "enable",
          attr: "isenabled",
          isclick: true,
          clickevent: "m_markas",
          icon: "",
          css: ButtonCSS.SUCCESS_BUTTON,
          tooltip: "Enable selected records"
        },
        {
          id: "2",
          title: "Disable",
          value: 0,
          actionstatus: "disable",
          attr: "isenabled",
          isclick: true,
          clickevent: "m_markas",
          icon: "",
          css: ButtonCSS.SUCCESS_BUTTON,
          tooltip: "Disable selected records"
        },
        {
          id: "0",
          title: "Normal",
          value: 1,
          actionstatus: "featured",
          attr: "isfeatured",
          isclick: true,
          clickevent: "m_markas",
          icon: "",
          css: ButtonCSS.SUCCESS_BUTTON,
          tooltip: "Mark as featured"
        },
        {
          id: "1",
          title: "Featured",
          value: 1,
          actionstatus: "featured",
          attr: "isfeatured",
          isclick: true,
          clickevent: "m_markas",
          icon: "",
          css: ButtonCSS.SUCCESS_BUTTON,
          tooltip: "Mark as featured"
        },
        {
          id: "3",
          title: "Adult",
          value: 1,
          actionstatus: "adult",
          isclick: true,
          attr: "isadult",
          clickevent: "m_markas",
          icon: "",
          css: ButtonCSS.SUCCESS_BUTTON,
          tooltip: "Mark as adult"
        },
        {
          id: "4",
          title: "Non Adult",
          value: 0,
          actionstatus: "nonadult",
          attr: "isadult",
          isclick: true,
          clickevent: "m_markas",
          icon: "",
          css: ButtonCSS.SUCCESS_BUTTON,
          tooltip: "Mark as non adult"
        },
        {
          id: "1",
          title: "Private",
          value: 1,
          actionstatus: "private",
          attr: "privacy",
          isclick: true,
          clickevent: "m_markas",
          icon: "",
          css: ButtonCSS.SUCCESS_BUTTON,
          tooltip: "Mark as private"
        },
        {
          id: "2",
          title: "Public",
          value: 0,
          actionstatus: "public",
          attr: "privacy",
          isclick: true,
          clickevent: "m_markas",
          icon: "",
          css: ButtonCSS.SUCCESS_BUTTON,
          tooltip: "Mark as public"
        },
        {
          id: "2",
          title: "Delete",
          value: 0,
          actionstatus: "delete",
          css: ButtonCSS.DANGER_BUTTON,
          attr: "",
          isclick: true,
          clickevent: "m_markas",
          icon: ICONCSS.DELETE_ICON,
          tooltip: "Delete selected records"
        }
      ]
    });
    return options;
  }

  init_account_toolbar_options(type: number) {
    let options = {
      showtoolbar: false,
      showsecondarytoolbar: true,
      showcheckAll: true,
      navbarcss: ThemeCSS.NAVBAR_CSS,
      left_options: [],
      left_caption: "",
      right_caption: "",
      right_options: [],
      actions: []
    };

    if (type === 0) {
      // my videos options
      options.actions.push({
        title: "Mark As",
        showOnMainBar: false,
        ismultiple: true,
        icon: "",
        Options: [
          {
            id: "1",
            title: "Private",
            value: 1,
            actionstatus: "private",
            attr: "privacy",
            isclick: true,
            clickevent: "m_markas",
            icon: "",
            css: ButtonCSS.PRIMARY_BUTTON,
            tooltip: "Mark as private"
          },
          {
            id: "2",
            title: "Public",
            value: 0,
            actionstatus: "public",
            attr: "privacy",
            isclick: true,
            clickevent: "m_markas",
            icon: "",
            css: ButtonCSS.PRIMARY_BUTTON,
            tooltip: "Mark as public"
          },
          {
            id: "2",
            title: "Delete",
            value: 0,
            actionstatus: "delete",
            css: ButtonCSS.DANGER_BUTTON,
            attr: "",
            isclick: true,
            clickevent: "m_markas",
            icon: ICONCSS.DELETE_ICON,
            tooltip: "Delete selected records"
          }
        ]
      });
    } else if (type === 1) {
      // my favorite options
      options.actions.push({
        title: "Mark As",
        showOnMainBar: false,
        ismultiple: true,
        icon: "",
        Options: [
          {
            id: "2",
            title: "Delete",
            value: 0,
            actionstatus: "delete_fav",
            css: ButtonCSS.DANGER_BUTTON,
            attr: "",
            isclick: true,
            clickevent: "m_markas",
            icon: ICONCSS.DELETE_ICON,
            tooltip: "Remove selected item from favorited list"
          }
        ]
      });
    } else if (type === 2) {
      // my liked options
      options.actions.push({
        title: "Mark As",
        showOnMainBar: false,
        ismultiple: true,
        icon: "",
        Options: [
          {
            id: "2",
            title: "Delete",
            value: 0,
            actionstatus: "delete_like",
            css: ButtonCSS.DANGER_BUTTON,
            attr: "",
            isclick: true,
            clickevent: "m_markas",
            icon: ICONCSS.DELETE_ICON,
            tooltip: "Remove selected record from liked list"
          }
        ]
      });
    } else if (type === 3) {
      // my playlist options
      options.actions.push({
        title: "Mark As",
        showOnMainBar: false,
        ismultiple: true,
        icon: "",
        Options: [
          {
            id: "2",
            title: "Delete",
            value: 0,
            actionstatus: "delete_playlist",
            css: ButtonCSS.DANGER_BUTTON,
            attr: "",
            isclick: true,
            clickevent: "m_markas",
            icon: ICONCSS.DELETE_ICON,
            tooltip: "Delete selected playlsits"
          }
        ]
      });
    }
    return options;
  }

  navList() {
    return [
      {
        id: 1,
        title: "Edit",
        clickevent: true,
        event: "edit",
        css: "",
        url: "",
        icon: ""
      },
      {
        id: 2,
        title: "Delete",
        clickevent: true,
        event: "delete",
        css: "",
        url: "",
        icon: ""
      }
    ];
  }

  getApiOptions() {
    return this.apiOptions;
  }

  getConfigs() {
    return this.Config;
  }

  getToolbarOptions(type: number, isadmin: boolean) {
    if (isadmin) {
      return this.init_admin_toolbar_options();
    } else {
      return this.init_account_toolbar_options(type);
    }
  }

  getSearchOptions(isadmin: boolean, settings: any) {
    if (isadmin) {
      return this.init_admin_search_options(settings);
    } else {
      return this.init_account_search_options(settings);
    }
  }

  /* -------------------------------------------------------------------------- */
  /*                       Options for top search options                       */
  /* -------------------------------------------------------------------------- */
  getTopSearchSettings() {
    return {
      NavList: [
        {
          id: 1,
          title: "Upload Date",
          value: 0,
          attr: "datefilter",
          options: [
            { id: 1, title: "Today", value: 1 },
            { id: 2, title: "This Week", value: 2 },
            { id: 3, title: "This Month", value: 3 },
            { id: 4, title: "This Year", value: 4 },
            { id: 5, title: "All Time", value: 0 }
          ]
        },
        {
          id: 1,
          title: "Categories",
          value: 0,
          attr: "categorid",
          options: []
        },
        {
          id: 2,
          title: "Sort By",
          value: "video.created_at desc",
          attr: "order",
          options: [
            { id: 1, title: "Upload Date", value: "video.created_at desc" },
            {
              id: 2,
              title: "View Count",
              value: "video.views desc, video.created_at desc"
            },
            {
              id: 3,
              title: "Rating",
              value: "video.avg_rating desc, video.created_at desc"
            }
          ]
        }
      ]
    };
  }

  getInitObject(): OPTIONS.VideoEntity {
    return {
      id: 0,
      userid: "",
      title: "",
      description: "",
      tags: "",
      categories: "",
      category_list: [],
      files: [],
      views: 0,
      liked: 0,
      actors: "",
      actresses: ""
    };
  }
}
