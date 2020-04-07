/* -------------------------------------------------------------------------- */
/*                          Product Name: VideoEngine                         */
/*                            Author: Mediasoftpro                            */
/*                       Email: support@mediasoftpro.com                      */
/*       License: Read license.txt located on root of your application.       */
/*                     Copyright 2007 - 2020 @Mediasoftpro                    */
/* -------------------------------------------------------------------------- */
import { Component, OnInit, Input } from "@angular/core";

import { select } from "@angular-redux/store";
import { Observable } from "rxjs/Observable";

// services
import { SettingsService } from "../../../admin/settings/configurations/services/settings.service";
import { DataService } from "../../../admin/settings/configurations/services/data.service";
import { FormService } from "../../../admin/settings/configurations/services/form.service";

// shared services
import { CoreService } from "../../../admin/core/coreService";
import { CoreAPIActions } from "../../../reducers/core/actions";

// reducer actions
import { ConfigurationsAPIActions } from "../../../reducers/settings/configurations/actions";

@Component({
  selector: "app-setup",
  templateUrl: "./setup.html"
})
export class MainSetupComponent implements OnInit {
  constructor(
    private settingService: SettingsService,
    private dataService: DataService,
    private coreService: CoreService,
    private coreActions: CoreAPIActions,
    private actions: ConfigurationsAPIActions,
    private formService: FormService
  ) {}

  @select(["configurations", "configurations"])
  readonly Configurations$: Observable<any>;

  @select(["configurations", "loading"])
  readonly loading$: Observable<boolean>;

  @select(["configurations", "isloaded"])
  readonly isloaded$: Observable<any>;

  @Input() SetupType = 0; // 0: Database Setup, 1: User Setup
  Configs = [];
  Configurations: any = {};
  formHeading = "";
  submitText = "Next";
  skipBtnText = ""; // disable skip (Skip (Setup Later))
  primay_prop = "";
  child_prop = "";
  controls: any = [];
  showProcessing = false;
  finalstep = false;
  
  primary_prop_steps: any = [
    "general",
    "general",
    "general",
    "general",
    "general",
    "general",
    "general",
    "general",
    "general",
    "general",
    "videos",
    "videos",
    "videos",
    "videos",
    "videos"
  ];
  child_prop_steps: any = [
    "dbusersetup",
    "general",
    "features",
    "authentication",
    "registration",
    "rechapcha",
    "aws",
    "social",
    "contact",
    "smtp",
    "general",
    "aws",
    "ffmpeg",
    "youtube",
    "player"
  ];
  stepIndex = 0;
  ngOnInit() {
    if (this.SetupType === 0) {
      this.submitText = "Save Changes";
    }
    this.isloaded$.subscribe((loaded: boolean) => {
      if (!loaded) {
        this.loadRecords();
      }
    });
    this.Configurations$.subscribe((settings: any) => {
      this.Configurations = settings;
      if (this.SetupType === 0) {
        this.renderForm("general", "dbsetup");
      } else {
        this.renderForm(
          this.primary_prop_steps[this.stepIndex],
          this.child_prop_steps[this.stepIndex]
        );
      }
    });
  }

  loadRecords() {
    this.dataService.LoadRecords();
  }

  renderForm(primary_prop: string, sub_prop: string) {
    let entity = {};
    this.formHeading = "";
    this.primay_prop = primary_prop;
    this.child_prop = sub_prop;

    this.formHeading = sub_prop[0].toUpperCase() + sub_prop.slice(1) + " Settings";
    if (primary_prop === 'videos') {
      this.formHeading = "[Videos] " + this.formHeading;
    }
    if (sub_prop === "dbsetup") {
      entity = {
        host: "",
        database: "",
        userid: "",
        password: ""
      };
    } else if (sub_prop === 'dbusersetup') {
      this.formHeading = "Create Admin User";
       entity = {
        username: "",
        email: "",
        firstname: "",
        lastname: "",
        password: ""
      };
    }
    else {
      entity = this.Configurations[primary_prop][sub_prop];
    }

    this.controls = this.formService.getControls(
      entity,
      primary_prop,
      sub_prop,
      true
    );

  }

  SubmitForm(payload) {
    let settings = payload;
    if (this.child_prop !== "dbsetup" && this.child_prop !== "dbusersetup") {
      this.Configurations[this.primay_prop][this.child_prop];
      for (const prop in settings) {
        for (const payload_prop in payload) {
          if (payload_prop === prop) {
            settings[prop] = payload[payload_prop];
          }
        }
      }
    }

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
          if (this.SetupType === 1) {
            this._nextForm();
          } else {
            this.finalstep = true;
          }
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

  SkipForm(event: any) {
    this._nextForm();
  }

  _nextForm() {
   
    if (this.stepIndex < this.child_prop_steps.length - 1) {
      this.stepIndex++;
      this.renderForm(
        this.primary_prop_steps[this.stepIndex],
        this.child_prop_steps[this.stepIndex]
      );
    } else {
      this.finalstep = true;
      // setup completed
      this.dataService.SetupCompleted();
    }
  }
}
