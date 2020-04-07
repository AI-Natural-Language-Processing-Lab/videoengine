/* -------------------------------------------------------------------------- */
/*                          Product Name: VideoEngine                         */
/*                            Author: Mediasoftpro                            */
/*                       Email: support@mediasoftpro.com                      */
/*       License: Read license.txt located on root of your application.       */
/*                     Copyright 2007 - 2020 @Mediasoftpro                    */
/* -------------------------------------------------------------------------- */

import { Component, OnInit, Input } from "@angular/core";
import { FormService } from "../../services/form.service";
import { DataService } from "../../services/data.service";
import { NgbActiveModal } from "@ng-bootstrap/ng-bootstrap";
import { CoreAPIActions } from "../../../../reducers/core/actions";
import { CoreService } from "../../../../admin/core/coreService";
import { select } from "@angular-redux/store";
import { Observable } from "rxjs/Observable";

@Component({
  selector: "viewmodal",
  templateUrl: "./model.html",
  providers: [FormService, DataService]
})
export class ViewComponent implements OnInit {
  @Input() Info: any;
  title: string;
  data: any;

  showLoader = false;
  heading: string;
  controls: any[];
  Categories: any = [];

  list: any[] = [];
  constructor(
    public activeModal: NgbActiveModal,
    private service: FormService,
    private dataService: DataService,
    private coreService: CoreService,
    private coreActions: CoreAPIActions
  ) {}

  @select(["videos", "categories"])
  readonly categories$: Observable<any>;
  ngOnInit() {
    this.title = this.Info.title;
    if (this.Info.viewType === 0) {
      // upload video thumbnails (direct uploader)
      this.controls = this.service.getCoverControls(
        this.Info.data,
        this.Info.auth
      );
    } else if (this.Info.viewType === 1) {
      this.categories$.subscribe((categories: any) => {
        this.Categories = categories;
        // edit video
        this.controls = this.service.getVideoEditControls(this.Info.data, true);
        this.updateCategories();
      });
    } else {
      this.controls = this.service.getControls(this.Info.data, this.Info.auth);
    }
  }
  updateCategories() {
    this.coreService.updateCategories(this.controls, this.Categories);
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
    if (this.Info.viewType === 0) {
      this.Info.data.video_thumbs = payload.video_thumbs;
      // this.showLoader = true;
      this.activeModal.close({
        data: this.Info.data
      });
    } else if (this.Info.viewType === 1) {
      // edit video information
      payload.id = this.Info.data.id;
      payload.categories = this.coreService.returnSelectedCategoryArray(
        payload.categories
      );
      const _status = "Updated";
      this.showLoader = true;
      this.dataService.EditRecord([payload]).subscribe(
        (data: any) => {
          if (data.status === "error") {
            this.coreActions.Notify({
              title: data.message,
              text: "",
              css: "bg-error"
            });
          } else {
            this.coreActions.Notify({
              title: "Record " + _status + " Successfully",
              text: "",
              css: "bg-success"
            });

            this.activeModal.close({
              data: data.record,
              isenabled: _status
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
  }
  close() {
    this.activeModal.dismiss("Cancel Clicked");
  }
}
