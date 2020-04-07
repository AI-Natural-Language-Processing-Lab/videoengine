/* -------------------------------------------------------------------------- */
/*                          Product Name: VideoEngine                         */
/*                            Author: Mediasoftpro                            */
/*                       Email: support@mediasoftpro.com                      */
/*       License: Read license.txt located on root of your application.       */
/*                     Copyright 2007 - 2020 @Mediasoftpro                    */
/* -------------------------------------------------------------------------- */
import { Injectable } from "@angular/core";
import * as OPTIONS from "../model";
import { iUploadOptions } from "../../core/core.model";
import { AppConfig } from "../../../configs/app.config";
import { CoreService } from "../../core/coreService";
import { ButtonCSS, ICONCSS, ThemeCSS } from "../../../configs/themeSettings";

@Injectable()
export class SettingsService {
  // configurations
  private apiOptions: OPTIONS.IAPIOptions;
  private uploadOptions: iUploadOptions;
  private toolbarOptions: any;
  private searchOptions: any;

  constructor(private coreService: CoreService, public config: AppConfig) {
    const APIURL = config.getConfig("host");
    this.apiOptions = {
      load: APIURL + "api/user/load",
      load_reports: APIURL + "api/adlisting/load_reports",
      getinfo: APIURL + "api/user/getinfo", // load user info along with role
      getinfo_auth: APIURL + "api/user/getUserAuth", // load user info along with role
      action: APIURL + "api/user/action",
      proc: APIURL + "api/user/proc",
      updatethumb: APIURL + "api/user/updatethumb", // old photo update module
      updateavator: APIURL + "api/user/updateavator", // new cropper version
      cemail: APIURL + "api/user/cemail",
      chpass: APIURL + "api/user/chpass",
      ctype: APIURL + "api/user/ctype",
      userlog: APIURL + "api/user/userlog",
      authenticate: APIURL + "api/user/authenticate",
      updaterole: APIURL + "api/user/updaterole",
      archive: APIURL + "api/user/archive"
    };

    this.init_toolbar_options();
    this.init_search_options();
  }
  init_search_options() {
    this.searchOptions = this.init_search_main_options();
  }
  /* navigation for listing */
  init_search_main_options() {
    return {
      showpanel: true, // show, hide whole panel
      showSearchPanel: true,
      term: "",
      topselectioncheck: true,
      showAdvanceSearchLink: true,
      navList: [],
      filters: [],
      dropdownFilters: [
        {
          id: 1,
          value: 0,
          group: "datefilter",
          caption: "Registered",
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
          id: 1,
          value: "created_at desc",
          group: "order",
          caption: "Order By",
          attr: "order",
          options: [
            {
              id: 1,
              title: "Account Created",
              value: "created_at desc"
            },
            {
              id: 2,
              title: "Name",
              value: "firstname asc, username asc"
            },
            {
              id: 4,
              title: "Most Viewed",
              value: "views desc"
            },
            {
              id: 5,
              title: "Last Signed In",
              value: "last_login desc"
            }
          ]
        }
      ],
      checkFilters: [
        {
          id: 1,
          value: 2,
          group: "isenabled",
          caption: "User Status",
          attr: "lockoutenabled",
          options: [
            {
              id: 1,
              title: "Active",
              value: 1
            },
            {
              id: 2,
              title: "Inactive",
              value: 0
            },
            {
              id: 3,
              title: "Any",
              value: 2
            }
          ]
        },
        {
          id: 2,
          value: 2,
          group: "emailconfirmed",
          caption: "Email Confirmed",
          attr: "emailconfirmed",
          options: [
            {
              id: 1,
              title: "Active",
              value: 1
            },
            {
              id: 2,
              title: "Inactive",
              value: 0
            },
            {
              id: 3,
              title: "Any",
              value: 2
            }
          ]
        }
      ],
      categories: [],
      selectedcategory: "",
      singleaction: false,
      actions: [
        {
          id: 1,
          title: "Create Account",
          tooltip: "Create new account",
          row: 1,
          icon: "icon-file-plus",
          options: {},
          css: "btn m-b-5 btn-block btn-success",
          event: "add"
        },
        {
          id: 5,
          title: "Settings",
          tooltip: "Manage Settings",
          row: 2,
          icon: "icon-movie",
          options: {},
          css: "btn btn-block btn-warning",
          event: "settings"
        }
      ]
    };
  }
  /* navigation for profile */
  init_search_profile_options() {
    return {
      showpanel: true, // show, hide whole panel
      showSearchPanel: false,
      term: "",
      topselectioncheck: false,
      navList: this.navList(),
      filters: [],
      dropdownFilters: [],
      categories: [],
      selectedcategory: "",
      singleaction: false,
      actions: [
        {
          id: 1,
          title: "Create Account",
          tooltip: "Create new account",
          row: 1,
          icon: "icon-file-plus",
          options: {},
          css: "btn m-b-5 btn-block btn-success",
          event: "add"
        },
        {
          id: 101,
          title: "Reports",
          tooltip: "Load Reports",
          css: "btn btn-block m-b-5 btn-info",
          event: "reports"
        }
      ]
    };
  }

  navList() {
    return [
      {
        id: 0,
        title: "Home",
        clickevent: true,
        event: "home_view",
        css: "",
        url: "",
        icon: ""
      },
      {
        id: 1,
        title: "Edit Profile Information",
        clickevent: true,
        event: "edit_profile",
        css: "",
        url: "",
        icon: ""
      },
      {
        id: 2,
        title: "Change Password",
        clickevent: true,
        event: "change_password",
        css: "",
        url: "",
        icon: ""
      },
      {
        id: 3,
        title: "Change Email",
        clickevent: true,
        event: "change_email",
        css: "",
        url: "",
        icon: ""
      },
      {
        id: 7,
        title: "Change Role",
        clickevent: true,
        event: "change_usertype",
        css: "",
        url: "",
        icon: ""
      },
      {
        id: 8,
        title: "View History",
        clickevent: true,
        event: "view_history",
        css: "",
        url: "",
        icon: ""
      }
    ];
  }

  init_toolbar_options() {
    this.toolbarOptions = {
      showtoolbar: false,
      showsecondarytoolbar: true,
      showcheckAll: true,
      navbarcss: ThemeCSS.NAVBAR_CSS,
      left_options: [],
      left_caption: "Filter:",
      right_caption: "",
      right_options: [],
      actions: []
    };

    this.toolbarOptions.actions.push({
      title: "Mark As",
      showOnMainBar: false,
      ismultiple: true,
      icon: "",
      Options: [
        {
          id: "1",
          title: "Enable",
          value: false,
          actionstatus: "enable",
          attr: "lockoutenabled",
          isclick: true,
          clickevent: "m_markas",
          icon: "",
          css: ButtonCSS.SUCCESS_BUTTON,
          tooltip: "Enable selected records"
        },
        {
          id: "2",
          title: "Disable",
          value: true,
          actionstatus: "disable",
          attr: "lockoutenabled",
          isclick: true,
          clickevent: "m_markas",
          icon: "",
          css: ButtonCSS.SUCCESS_BUTTON,
          tooltip: "Disable selected records"
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
  }
  init_toolbar_user_log_options() {
    const options = {
      showtoolbar: false,
      showsecondarytoolbar: true,
      showcheckAll: true,
      navbarcss: ThemeCSS.NAVBAR_CSS,
      left_options: [],
      left_caption: "Filter:",
      right_caption: "",
      right_options: [],
      actions: []
    };
    options.actions.push({
      title: "Mark As",
      showOnMainBar: false,
      ismultiple: true,
      icon: "",
      Options: [
        {
          id: "2",
          title: "Delete All",
          value: 0,
          actionstatus: "delete",
          css: ButtonCSS.DANGER_BUTTON,
          attr: "",
          isclick: true,
          clickevent: "m_log_markas",
          icon: ICONCSS.DELETE_ICON,
          tooltip: "Clear all log"
        }
      ]
    });
    return options;
  }

  /* -------------------------------------------------------------------------- */
  /*                       Options for top search options                       */
  /* -------------------------------------------------------------------------- */
  init_top_search_options() {
    return {
      NavList: [
        {
          id: 1,
          title: "Register",
          value: 0,
          attr: "datefilter",
          options: [
            { id: 1, title: "Today", value: 1 },
            { id: 2, title: "This Week", value: 2 },
            { id: 3, title: "This Month", value: 3},
            { id: 4, title: "This Year", value: 4},
            { id: 5, title: "All Time", value: 0}
          ]
        },
        {
          id: 2,
          title: "Sort By",
          value: 'blog.created_at desc',
          attr: "order",
          options: [
            { id: 1, title: 'Register Date', value: 'created_at desc' },
            { id: 3, title: 'View Count', value: 'views desc, created_at desc' }
          ]
        }
      ]
    };
  }
  getApiOptions() {
    return this.apiOptions;
  }

  getUploadOptions() {
    return this.uploadOptions;
  }

  getToolbarOptions() {
    return this.toolbarOptions;
  }

  getUserLogToolbarOptions() {
    return this.init_toolbar_user_log_options();
  }

  getSearchOptions() {
    return this.searchOptions;
  }

  getSearchOptionsProfile() {
    return this.init_search_profile_options();
  }
 
  getTopSearchSettings() {
    return this.init_top_search_options();
  }

  getInitObject(): OPTIONS.UserEntity {
    return {
      Id: "",
      role_name: "Member",
      username: "",
      email: "",
      firstname: "",
      lastname: "",
      password: "",
      cpassword: "",
      opassword: "",
      aboutme: "",
      gender: "",
      website: "",
      isallowbirthday: 1,
      relationshipstatus: "Single",
      hometown: "",
      currentcity: "",
      zipcode: "",
      countryname: "",
      description: "",
      files: [],
      picture: ""
    };
  }
}
