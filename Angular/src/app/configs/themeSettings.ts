
/* -------------------------------------------------------------------------- */
/*                          Product Name: VideoEngine                         */
/*                            Author: Mediasoftpro                            */
/*                       Email: support@mediasoftpro.com                      */
/*       License: Read license.txt located on root of your application.       */
/*                     Copyright 2007 - 2020 @Mediasoftpro                    */
/* -------------------------------------------------------------------------- */
export class ButtonCSS {
  static readonly PRIMARY_BUTTON = "btn btn-primary";
  static readonly SECONDARY_BUTTON = "btn btn-secondary";
  static readonly SUCCESS_BUTTON = "btn btn-success";
  static readonly DANGER_BUTTON = "btn btn-danger";
  static readonly WARNING_BUTTON = "btn btn-warning";
  static readonly INFO_BUTTON = "btn btn-info";
  static readonly LIGHT_BUTTON = "btn btn-light";
  static readonly DARK_BUTTON = "btn btn-dark";
}
export class ICONCSS {
  static readonly DELETE_ICON = "fa fa-trash";
  static readonly ADD_ICON = "fa fa-plus";
}

export class ThemeCSS {
  static readonly NAVBAR_CSS = "navbar navbar-expand-lg navbar-light bg-light";
  static readonly SUBMIT_BUTTON_CSS = "btn btn-primary";
}

export class AppState {
  static DEFAULT_LANG = "en";
  static readonly SUPPORTED_LANGS = ["en", "es", "fr", "de", "ar"]; // for ng2-translate
  static readonly SUPPORTED_LANGS_EXTENDED = [
    {
      icon: "flag-icon-us",
      title: "English",
      culture: "en"
    },
    {
      icon: "flag-icon-fr",
      title: "French",
      culture: "fr"
    },
    {
      icon: "flag-icon-es",
      title: "Spanish",
      culture: "es"
    },
    {
      icon: "flag-icon-de",
      title: "German",
      culture: "de"
    },
    {
      icon: "flag-icon-sa",
      title: "Arabic",
      culture: "ar"
    }
  ]; // for theme dropdown
}
