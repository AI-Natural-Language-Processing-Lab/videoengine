/* -------------------------------------------------------------------------- */
/*                          Product Name: VideoEngine                         */
/*                            Author: Mediasoftpro                            */
/*                       Email: support@mediasoftpro.com                      */
/*       License: Read license.txt located on root of your application.       */
/*                     Copyright 2007 - 2020 @Mediasoftpro                    */
/* -------------------------------------------------------------------------- */

import { Component, OnInit, Input } from "@angular/core";
import { ActivatedRoute, Router } from "@angular/router";

// services
import { SettingsService } from "../../attr/services/settings.service";
import { DataService } from "../../attr/services/data.service";
import { FormService } from "../../attr/services/form.service";

// shared services
import { CoreService } from "../../../admin/core/coreService";
import { CoreAPIActions } from "../../../reducers/core/actions";

// reducer actions
import { fadeInAnimation } from "../../../animations/core";

/* modal popup */
import { NgbModal, NgbModalOptions } from "@ng-bootstrap/ng-bootstrap";
import { ViewComponent } from "../../attr/partials/modal.component";

@Component({
  selector: "app-dynamic-settings",
  templateUrl: "./settings.html",
  animations: [fadeInAnimation],
  host: { "[@fadeInAnimation]": "" }
})
export class DynamicSettingsComponent implements OnInit {
  constructor(
    private settingService: SettingsService,
    private dataService: DataService,
    private coreService: CoreService,
    private coreActions: CoreAPIActions,
    private route: ActivatedRoute,
    private formService: FormService,
    private router: Router,
    private modalService: NgbModal
  ) {}

  @Input() Attr_Type = 0; // 0: Ad, 1: Agency / Company, 2: Artist, 3: User Profile
  @Input() Skip_Template = false;
  controls: any = [];
  showLoader = false;
  formHeading = "Manage Templates and Settings (Dynamic Attributes)";
  Settings = [];
  CategoryType = 1;
  FilterOptions: any;
  IsLoaded = false; // check data is loaded for first time
  Url = "";
  ngOnInit() {
    switch(this.Attr_Type) {
       case 0:
         this.Url = '/classified/ads/attributes/';
         break;
       case 1:
         this.Url = '/classified/agencies/attributes/';
         break;
       case 2:
         this.Url = '/artists/attributes/';
         break;
       case 3:
         this.Url = '/users/attributes/';
         break;
    }
    this.LoadSettings();
  }

  LoadSettings() {
    this.showLoader = true;
    this.dataService
      .LoadTemplates({
        attr_type: this.Attr_Type,
        skip_template: this.Skip_Template
      })
      .subscribe(
        (data: any) => {
          console.log("data returned");
          this.Settings = data.posts;
          this.showLoader = false;
        },
        err => {
          this.coreActions.Notify({
            title: "Load Failed",
            text: "",
            css: "bg-danger"
          });
        }
      );
  }

  AddTemplate() {
    this._processTemplate(
      {
        id: 0,
        attr_type: this.Attr_Type,
        title: "",
        sections: []
      },
      0,
      "Add Template"
    );
  }

  EditTemplate(obj: any, event: any) {
    this._processTemplate(obj, 0, "Update Template");
    event.stopPropagation();
  }

  AddSection(templateid: number, event: any) {
    this._processTemplate(
      {
        id: 0,
        title: "",
        attr_type: this.Attr_Type,
        templateid: templateid,
        priority: 0,
        showsection: 0,
      },
      1,
      "Add Section"
    );
    event.stopPropagation();
  }

  EditSection(section: any, event: any) {
    this._processTemplate(section, 1, "Update Section");
    event.stopPropagation();
  }

  _processTemplate(obj: any, viewtype: number, title: string) {
    const _options: NgbModalOptions = {
      backdrop: false
    };
    const modalRef = this.modalService.open(ViewComponent, _options);
    modalRef.componentInstance.Info = {
      title: title,
      viewtype: viewtype,
      data: obj
    };
    modalRef.result.then(
      result => {
        let isUpdate = false;
        let Url = "";
        switch (viewtype) {
          case 0:
            // template
            if (result.data.id > 0) {
              isUpdate = true;
            }
            Url = this.settingService.getApiOptions().proc_template;
            break;
          case 1:
            // section
            if (result.data.id > 0) {
              isUpdate = true;
            }

            Url = this.settingService.getApiOptions().proc_section;
            break;
        }
        this.UpdateDatabase(result.data, viewtype, isUpdate, Url);
      },
      dismissed => {
        console.log("dismissed");
      }
    );
  }

  UpdateDatabase(data: any, viewtype: number, isUpdate: boolean, url: string) {
    data.attr_type = this.Attr_Type;
    this.showLoader = true;
    this.dataService.AddTemplate(data, url).subscribe(
      (data: any) => {
        switch (viewtype) {
          case 0:
            // template
            if (!isUpdate) {
              this.Settings.push(data.record);
            } else {
              // update
              for (let setting of this.Settings) {
                if (setting.id === data.record.id) {
                  setting.title = data.record.title;
                }
              }
            }
            break;
          case 1:
            // section
            if (!isUpdate) {
              if (this.Skip_Template) {
                this.Settings.push(data.record);
              } else {
                for (let setting of this.Settings) {
                  if (setting.id === data.record.templateid) {
                    setting.sections.push(data.record);
                  }
                }
              }
            } else {
              // update
              if (this.Skip_Template) {
                for (let setting of this.Settings) {
                    if (setting.id === data.record.id) {
                      setting.title = data.record.title;
                      setting.priority = data.record.priority;
                    }
                  }
              } else {
                for (let setting of this.Settings) {
                    if (setting.id === data.record.templateid) {
                      console.log("matched");
                      for (let section of setting.sections) {
                        if (section.id === data.record.id) {
                          section.title = data.record.title;
                          section.priority = data.record.priority;
                        }
                      }
                    }
                  }
              }
              
            }
            break;
        }
        this.showLoader = false;

        this.coreActions.Notify({
          title: "Record Saved",
          text: "",
          css: "bg-success"
        });
      },
      err => {
        this.coreActions.Notify({
          title: "Record not Saved",
          text: "",
          css: "bg-danger"
        });
      }
    );
  }

  RemoveTemplate(obj: any, index: number, event: any) {
    this.Settings.splice(index, 1);
    this.dataService.DeleteTemplate(
      [obj],
      this.settingService.getApiOptions().delete_template
    );
    event.stopPropagation();
  }

  RemoveSection(obj: any, arr: any, index: number, event: any) {
    arr.splice(index, 1);
    this.dataService.DeleteTemplate(
      [obj],
      this.settingService.getApiOptions().delete_section
    );
    event.stopPropagation();
  }
}
