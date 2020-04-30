/* -------------------------------------------------------------------------- */
/*                          Product Name: VideoEngine                    */
/*                            Author: Mediasoftpro                            */
/*                       Email: support@mediasoftpro.com                      */
/*       License: Read license.txt located on root of your application.       */
/*                     Copyright 2007 - 2020 @Mediasoftpro                    */
/* -------------------------------------------------------------------------- */

import { Component, OnInit, Input } from "@angular/core";

// import { DataService } from "../services/data.service";
import { FormService } from "../services/form.service";
import { NgbActiveModal } from "@ng-bootstrap/ng-bootstrap";
// import { CoreAPIActions } from "../../../reducers/core/actions";

@Component({
  templateUrl: "./modal.html"
})
export class ViewComponent implements OnInit {
  @Input() Info: any;
  title: string;
  data: any;

  showLoader = false;
  heading: string;
  controls: any = [];

  list: any[] = [];
  constructor(
    public activeModal: NgbActiveModal,
    private service: FormService
  ) {}

  ngOnInit() {
    this.title = this.Info.title;
    switch(this.Info.viewtype) {
       case 0:
          // template
          this.controls = this.service.getTemplateControls(this.Info.data);
         break;
       case 1:
          // section
          this.controls = this.service.getTemplateSectionControls(this.Info.data);
         break;
       case 2:
          // attribute
          this.controls = this.service.getAttributeControls(this.Info.data);
          break;
    }
   
  }

  SubmitForm(payload) {
    // permission check
    /*if (this.Info.isActionGranded !== undefined) {
      if (!this.Info.isActionGranded) {
        this.coreActions.Notify({
          title: "Permission Denied",
          text: "",
          css: "bg-danger"
        });
        return;
      }
    }*/
    payload.id = this.Info.data.id;
    if (payload.id === 0) {
      payload.uniqueid = new Date().valueOf();
    } else {
      payload.uniqueid = "";
    }

    switch(this.Info.viewtype) {
      case 0:
         // template
         payload.sections = this.Info.data.sections;
        break;
      case 1:
         // section
         payload.templateid = this.Info.data.templateid;
        break;
      case 2:
         // attributes
         payload.attr_type = this.Info.data.attr_type;
         break;
   }

    this.activeModal.close({
      data: payload
    });
  }

  close() {
    this.activeModal.dismiss("Cancel Clicked");
  }
}
