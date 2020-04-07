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
import { Router } from "@angular/router";
import { CoreAPIActions } from "../../../reducers/core/actions";
import { UserAPIActions } from "../../../reducers/users/actions";

@Component({
  selector: "viewmodal",
  templateUrl: "./modal.html",
  providers: [FormService, DataService]
})
export class ViewComponent implements OnInit {
  @Input() Info: any;
  title: string;
  data: any;
  viewType: 1; // 1: create account, 2: edit profile, 3: change email, 4: change password, 5: change user type
  showLoader = false;
  heading: string;
  controls: any = [];

  list: any[] = [];
  constructor(
    public activeModal: NgbActiveModal,
    private service: FormService,
    private dataService: DataService,
    private coreActions: CoreAPIActions,
    private router: Router,
    private actions: UserAPIActions
  ) {}

  ngOnInit() {
    this.title = this.Info.title;
    this.viewType = this.Info.viewType;
    this.controls = this.service.getControls(this.Info.data, this.viewType);
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
    // custom validation
    if (this.Info.viewType === 1 || this.Info.viewType === 4) {
      if (payload.password !== payload.cpassword) {
        this.coreActions.Notify({
          title: "Password Not Matched",
          text: "",
          css: "bg-danger"
        });
      }
    }

    this.showLoader = true;
    // adjust values with actual object
    if (this.Info.viewType !== 2) {
      for (const prop of Object.keys(this.Info.data)) {
        for (const payload_prop of Object.keys(payload)) {
          if (prop === payload_prop) {
            this.Info.data[prop] = payload[payload_prop];
          }
        }
      }
    }
    this.dataService.AddRecord(this.Info.data).subscribe(
      (data: any) => {
        if (data.status === "error") {
          this.coreActions.Notify({
            title: data.message,
            text: "",
            css: "bg-danger"
          });
        } else {
          let message = "Account Created Successfully";
          this.actions.addRecord(data.record);

          this.coreActions.Notify({
            title: message,
            text: "",
            css: "bg-success"
          });

          if (this.Info.viewType === 1) {
            this.router.navigate(["/users/profile/" + data.record.id]);
          }

          this.activeModal.close({
            data: data.record
          });
          
        }
        this.showLoader = false;
      },
      err => {
        this.showLoader = false;
        this.coreActions.Notify({
          title: "Error Occured",
          text: "",
          css: "bg-danger"
        });
      }
    );
  }
  close() {
    this.activeModal.dismiss("Cancel Clicked");
  }
}
