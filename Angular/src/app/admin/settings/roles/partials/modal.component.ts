/* -------------------------------------------------------------------------- */
/*                          Product Name: VideoEngine                         */
/*                            Author: Mediasoftpro                            */
/*                       Email: support@mediasoftpro.com                      */
/*       License: Read license.txt located on root of your application.       */
/*                     Copyright 2007 - 2020 @Mediasoftpro                    */
/* -------------------------------------------------------------------------- */

import { Component, OnInit, Input } from "@angular/core";
import { FormService } from "../services/form.service";
import { DataService } from "../services/data.service";
import { NgbActiveModal } from "@ng-bootstrap/ng-bootstrap";
import { CoreAPIActions } from "../../../../reducers/core/actions";
@Component({
  selector: "viewmodal",
  templateUrl: "./modal.html",
  providers: [FormService, DataService]
})
export class ViewComponent implements OnInit {
  @Input() Info: any;
  title: string;
  data: any;
  ViewType = 1; // 1: add / edit categories, 2: view code
  showLoader = false;
  heading: string;
  controls: any[];

  list: any[] = [];
  constructor(
    public activeModal: NgbActiveModal,
    private coreActions: CoreAPIActions,
    private service: FormService,
    private dataService: DataService
  ) {}

  ngOnInit() {
    this.title = this.Info.title;
    this.ViewType = this.Info.viewtype;
    if (this.Info.viewtype === 1) {
      // add role
      this.controls = this.service.getRoleControls(this.Info.data);
    } else {
      // add object
      this.controls = this.service.getObjectControls(this.Info.data);
    }
    console.log(this.controls);
  }

  SubmitForm(payload) {
    // permission check
    if (this.Info.isActionGranded !== undefined) {
      if (!this.Info.isActionGranded) {
        this.coreActions.Notify({
          title: "Permission Denied",
          text: "",
          css: "bg-danger"
        });
        return;
      }
    }
    payload.id = this.Info.data.id;
    if (payload.id === 0) {
      payload.uniqueid = new Date().valueOf();
    } else {
      payload.uniqueid = "";
    }
    this.activeModal.close({
      data: payload,
      viewtype: this.Info.viewtype
    });
  }

  close() {
    this.activeModal.dismiss("Cancel Clicked");
  }
}
