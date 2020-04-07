/* -------------------------------------------------------------------------- */
/*                          Product Name: VideoEngine                         */
/*                            Author: Mediasoftpro                            */
/*                       Email: support@mediasoftpro.com                      */
/*       License: Read license.txt located on root of your application.       */
/*                     Copyright 2007 - 2020 @Mediasoftpro                    */
/* -------------------------------------------------------------------------- */

import { Component, OnInit } from "@angular/core";
import { select } from "@angular-redux/store";
import { Observable } from "rxjs/Observable";
import { ActivatedRoute } from "@angular/router";
// services
import { SettingsService } from "./services/settings.service";
import { DataService } from "./services/data.service";

// shared services
import { CoreService } from "../../core/coreService";
import { CoreAPIActions } from "../../../reducers/core/actions";

// reducer actions
import { ConfigurationsAPIActions } from "../../../reducers/settings/configurations/actions";
import { FormService } from "./services/form.service";

import { PermissionService } from "../../../admin/users/services/permission.service";

@Component({
  templateUrl: "./configuration.html"
})
export class ConfigurationComponent implements OnInit {
  constructor(
    private settingService: SettingsService,
    private dataService: DataService,
    private coreService: CoreService,
    private coreActions: CoreAPIActions,
    public permission: PermissionService,
    private actions: ConfigurationsAPIActions,
    private route: ActivatedRoute,
    private formService: FormService
  ) {}

  //@select(['configurations', 'posts'])
  //readonly Data$: Observable<any>;

  @select(["configurations", "configurations"])
  readonly Configurations$: Observable<any>;

  @select(["configurations", "loading"])
  readonly loading$: Observable<boolean>;

  @select(["configurations", "isloaded"])
  readonly isloaded$: Observable<any>;

  @select(["users", "auth"])
  readonly auth$: Observable<any>;

  // permission logic
  isAccessGranted = false; // Granc access on resource that can be full access or read only access with no action rights
  isActionGranded = false; // Grand action on resources like add / edit /delete

  navTabs: any = [];
  SearchOptions: any;
  IsLoaded = false;
  Configs = [];
  Configurations: any = {};
  ConfigType = 0;
  ChildConfigType = 0;
  controls: any = [];
  showProcessing = false;
  formHeading = "";
  submitText = "Save Changes";
  primay_prop = "";
  child_prop = "";

  ngOnInit() {
    // user authentication & access right management
    // full resource access key and readonly key can be generated via roles management
    this.auth$.subscribe((auth: any) => {
      const FullAccessID = "1521396255768";
      const ReadOnlyAccessID = "1521396280060";
      if (
        this.permission.GrandResourceAccess(
          false,
          FullAccessID,
          ReadOnlyAccessID,
          auth.Role
        )
      ) {
        this.isAccessGranted = true;
        if (this.permission.GrandResourceAction(FullAccessID, auth.Role)) {
          this.isActionGranded = true;
        }
      }
    });

    this.route.params.subscribe(params => {
      this.ConfigType = Number.parseInt(params["type"], 10);
      if (isNaN(this.ConfigType)) {
        this.ConfigType = 0;
      }
    });

    this.isloaded$.subscribe((loaded: boolean) => {
      this.IsLoaded = loaded;
      if (!this.IsLoaded) {
        this.loadRecords();
      }
    });

    this.Configurations$.subscribe((settings: any) => {
      this.Configurations = settings;
      this.InitializeMainNavigation();
    });
  }

  loadRecords() {
    this.dataService.LoadRecords();
  }

  InitializeMainNavigation() {
    let NavList = [];
    let NavId = 0;
    for (const prop in this.Configurations) {
      NavList.push({
        id: NavId,
        title: prop[0].toUpperCase() + prop.slice(1),
        child_nav: []
      });
      NavId++;
    }
    this.SearchOptions = this.settingService.getSearchOptions(NavList);

    this.attachSubNavItems();
  }

  attachSubNavItems() {
    for (const nav of this.SearchOptions.navList) {
      for (const prop in this.Configurations) {
        if (prop === nav.title.toLowerCase()) {
          let NavId = 0;
          for (const inner_prop in this.Configurations[prop]) {
            nav.child_nav.push({
              id: NavId,
              title: inner_prop[0].toUpperCase() + inner_prop.slice(1),
              parent_attr: prop,
              child_attr: inner_prop
            });
            NavId++;
          }
        }
      }
    }

    this.Initialize();
  }

  Initialize() {
    if (this.SearchOptions.navList.length > 0) {
      const nav = this.SearchOptions.navList[0];
      if (nav.child_nav.length > 0) {
        this.renderForm(
          nav.child_nav[0].parent_attr,
          nav.child_nav[0].child_attr
        );
      }
    }
  }

  /*initializeTabs() {
        this.navTabs = [];
        for (const nav of this.SearchOptions.navList) {
            if (nav.id === this.ConfigType) {
                for (const prop in this.Configurations) {
                    if (prop === nav.title.toLowerCase()) {
                        let NavId = 0;
                        for (const inner_prop in this.Configurations[prop]) {
                              this.navTabs.push({
                                 id: NavId,
                                 title: inner_prop[0].toUpperCase() + inner_prop.slice(1),
                                 parent_attr: prop,
                                 child_attr: inner_prop
                             });
                             NavId++;
                        }
                    }
                }
            }
        }
        // render form with first tab item for first time
        if (this.navTabs.length > 0) {
            this.renderForm(this.navTabs[0].parent_attr, this.navTabs[0].child_attr);
        }
   }*/

  /*renderPrimaryNav(nav: any, event: any) {
        this.ConfigType = nav.id;
        this.initializeTabs();
       
        event.stopPropagation();
   } */

  renderNav(p_nav: any, nav: any, event: any) {
    this.ConfigType = p_nav.id;
    this.ChildConfigType = nav.id;
    this.renderForm(nav.parent_attr, nav.child_attr);
    event.stopPropagation();
  }

  renderForm(primary_prop: string, sub_prop: string) {
    this.formHeading =
      sub_prop[0].toUpperCase() + sub_prop.slice(1) + " Settings";
    this.primay_prop = primary_prop;
    this.child_prop = sub_prop;
    this.controls = this.formService.getControls(
      this.Configurations[primary_prop][sub_prop],
      primary_prop,
      sub_prop
    );
  }

  SubmitForm(payload) {
    if (!this.isActionGranded) {
      this.coreActions.Notify({
        title: "Permission Denied",
        text: "",
        css: "bg-danger"
      });
      return;
    }

    let settings = this.Configurations[this.primay_prop][this.child_prop];
    for (const prop in settings) {
      for (const payload_prop in payload) {
        if (payload_prop === prop) {
          settings[prop] = payload[payload_prop];
        }
      }
    }
    console.log(settings);
    this.showProcessing = true;
    this.dataService
      .UpdateConfigurations(settings, this.primay_prop, this.child_prop)
      .subscribe(
        (data: any) => {
          if (data.status === "error") {
            this.coreActions.Notify({
              title: data.message,
              text: "",
              css: "bg-success"
            });
          } else {
            this.coreActions.Notify({
              title: "Settings Updated",
              text: "",
              css: "bg-success"
            });
          }
          this.showProcessing = false;
        },
        err => {
          this.coreActions.Notify({
            title: "Error Occured",
            text: "",
            css: "bg-danger"
          });
          this.showProcessing = false;
        }
      );
  }
}
