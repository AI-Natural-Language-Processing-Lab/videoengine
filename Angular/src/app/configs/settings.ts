/* -------------------------------------------------------------------------- */
/*                          Product Name: VideoEngine                         */
/*                            Author: Mediasoftpro                            */
/*                       Email: support@mediasoftpro.com                      */
/*       License: Read license.txt located on root of your application.       */
/*                     Copyright 2007 - 2020 @Mediasoftpro                    */
/* -------------------------------------------------------------------------- */

export class NormalRegex {
  static readonly USERNAME_REGEX = "^[a-z0-9_-]{5,15}$";
  static readonly PASSWORD_REGEX =
    "^(?=.*[0-9])(?=.*[!@#$%^&*])[a-zA-Z0-9!@#$%^&*]{6,16}$";
  static readonly WEBSITE_REGEX =
    "https?://(www.)?[-a-zA-Z0-9@:%._+~#=]{2,256}.[a-z]{2,6}\b([-a-zA-Z0-9@:%_+.~#?&//=]*)";
}

export class AppNavigation {
  
  // myaccount sub menus
  static readonly MYACCOUNT_SETTINGS = [
    { title: "Overview", value: "/", index: 0 },
    { title: "Profile Setup", value: "/profile-setup", index: 1 },
    { title: "Email Options", value: "/email-options", index: 2 },
    { title: "My Purchases", value: "/payments", index: 3 },
    { title: "Manage Account", value: "/manage-account", index: 4 }
  ];

  // videos sub menus
  static readonly MYACCOUNT_VIDEOS = [
    { title: "My Videos", value: "/my-videos", index: 0 },
    // { title: "Playlists", value: "/my-videos/playlists" },
    { title: "Favorited", value: "/my-videos/favorites", index: 1 },
    { title: "Liked", value: "/my-videos/liked", index: 2 }
  ];

}

export class NavigationMenuIndex {
  // Index to highlight and load appropriate sub menus for different contents
  // top menu index
  static readonly TOPMENU_SETTINGS_INDEX = 0;
  static readonly TOPMENU_VIDEOS_INDEX = 1;
  

  // settings sub menu indexes
  static readonly SETTINGS_OVERVIEW_INDEX = 0;
  static readonly SETTINGS_PROFILE_SETUP_INDEX = 1;
  static readonly SETTINGS_EMAIL_OPTIONS_INDEX = 2;
  static readonly SETTINGS_MYPURCHASE_INDEX = 3;
  static readonly SETTINGS_MANAGE_ACCOUNT_INDEX = 4;

  // videos sub menu indexes
  static readonly VIDEOS_MY_INDEX = 0;
  static readonly VIDEOS_FAVORITES_INDEX = 1;
  static readonly VIDEOS_LIKED_INDEX = 2;
  static readonly VIDEOS_PLAYLIST_INDEX = 3;
 
}

export class ContentTypes {


  static readonly MEDIA_TYPES = [
    { title: "Video", value: 0 },
    //{ title: "Audio", value: 1 }
  ];

  static readonly ROLE_TYPES = [
    {
      title: "Roles",
      value: "1",
      add_title: "Add Role",
      add_tooltip: "Add new role"
    },
    {
      title: "Role Objects",
      value: "2",
      add_title: "Add Role Object",
      add_tooltip: "Add new role"
    }
  ];

  static readonly USER_TYPES = [
    { title: "User", value: "Member" },
    { title: "Administrator", value: "Admin" },
    { title: "Certified User", value: "Manager" }
  ];
}
