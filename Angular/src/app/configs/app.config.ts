/* -------------------------------------------------------------------------- */
/*                          Product Name: VideoEngine                         */
/*                            Author: Mediasoftpro                            */
/*                       Email: support@mediasoftpro.com                      */
/*       License: Read license.txt located on root of your application.       */
/*                     Copyright 2007 - 2020 @Mediasoftpro                    */
/* -------------------------------------------------------------------------- */

import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";

@Injectable()
export class AppConfig {
  private config: any = {
    host: ""
  };

  // global js values passed from external application
  private global_vars: any = {
    title: "",
    userid: "",
    //username: "",
    culture: "",
    message: "",
    // videouploadoption: 0,
    url: "",
    apptype: "",
    setuptype: "",
    searchparams: {},
    img_directory: ""
  };

  private env: any = null;

  constructor(private http: HttpClient) {
    if (window["AppTitle"] !== undefined) {
       this.global_vars.title = window["AppTitle"];
    }
    if (window["Ang_UID"] !== undefined) {
       this.global_vars.userid = window["Ang_UID"];
    }
    /*if (window["Ang_UserName"] !== undefined) {
       this.global_vars.username = window["Ang_UserName"];
    }*/
    if (window["Ang_AppMessage"] !== undefined) {
       this.global_vars.message = window["Ang_AppMessage"];
    }
    if (window["Ang_Culture"] !== undefined) {
       this.global_vars.culture = window["Ang_Culture"];
    }
    /*if (window["Ang_VideoUploaderOption"] !== undefined) {
       this.global_vars.videouploadoption = window["Ang_VideoUploaderOption"];
    }*/
    if (window["AppType"] !== undefined) {
       this.global_vars.apptype = window["AppType"];
    }
    if (window["setupType"] !== undefined) {
       this.global_vars.setuptype = window["setupType"];
    }
    
    if (window["SearchParams"] !== undefined) {
       this.global_vars.searchparams = window["SearchParams"];
    }

    if (window["ImageDirectory"] !== undefined) {
      this.global_vars.img_directory = window["ImageDirectory"];
    }
    
    if (window["URL"] !== undefined) {
        this.global_vars.url = window["URL"];
        this.config.host = window["URL"];
        if (!this.config.host.endsWith("/")) {
          this.config.host = this.config.host + '/';
        }
    }
  }

  /**
   * Use to get the data found in the second file (config file)
   */
  public getConfig(key: any) {
    return this.config[key];
  }

  /**
   * Use to get the data found in the first file (env file)
   */
  public getEnv(key: any) {
    return this.env[key];
  }

  /**
   * Use to get the value of global variable
   */
  public getGlobalVar(key: any) {
    return this.global_vars[key];
  }
}
