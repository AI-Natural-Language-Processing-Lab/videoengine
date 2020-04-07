/* -------------------------------------------------------------------------- */
/*                          Product Name: VideoEngine                         */
/*                            Author: Mediasoftpro                            */
/*                       Email: support@mediasoftpro.com                      */
/*       License: Read license.txt located on root of your application.       */
/*                     Copyright 2007 - 2020 @Mediasoftpro                    */
/* -------------------------------------------------------------------------- */

import { Component, OnInit, ViewEncapsulation } from "@angular/core";
import { ActivatedRoute, Router } from "@angular/router";
import { select } from "@angular-redux/store";
import { Observable } from "rxjs/Observable";

// services
import { SettingsService } from "../../../admin/users/services/settings.service";
import { DataService } from "../../../admin/users/services/data.service";
import { FormService } from "../../../admin/users/services/form.service";

// shared services
import { CoreService } from "../../core/coreService";
import { CoreAPIActions } from "../../../reducers/core/actions";

// reducer actions
import { UserAPIActions } from "../../../reducers/users/actions";
import { fadeInAnimation } from "../../../animations/core";

import { PermissionService } from "../../../admin/users/services/permission.service";

import { GoogleChartInterface } from "ng2-google-charts/google-charts-interfaces";

@Component({
  templateUrl: "./reports.html",
  encapsulation: ViewEncapsulation.None,
  animations: [fadeInAnimation],
  host: { "[@fadeInAnimation]": "" }
})
export class UserReportsComponent implements OnInit {
  constructor(
    private settingService: SettingsService,
    private dataService: DataService,
    private coreService: CoreService,
    private coreActions: CoreAPIActions,
    private actions: UserAPIActions,
    private route: ActivatedRoute,
    private formService: FormService,
    private permission: PermissionService,
    private router: Router
  ) {}

  ToolbarOptions: any;
  FilterOptions: any = {};
  showLoader = false;
  formHeading = "User Reports";
  isAdmin = true;
  optionSelected = false;
  ChartType = "ColumnChart";
  message = "Please select report type to generate!";

  @select(["users", "filteroptions"])
  readonly filteroptions$: Observable<any>;

  @select(["users", "auth"])
  readonly auth$: Observable<any>;

  // permission logic
  isAccessGranted = false; // Granc access on resource that can be full access or read only access with no action rights
  isActionGranded = false; // Grand action on resources like add / edit /delete

  public tooltipChart: GoogleChartInterface = {
    chartType: "ColumnChart",
    dataTable: [],
    options: {
      title: "Videos Uploaded",
      legend: "none",
      width: 1000,
      height: 500,
      is3D: true
    }
  };

  ngOnInit() {
    // user authentication & access right management
    // full resource access key and readonly key can be generated via roles management
    this.auth$.subscribe((auth: any) => {
      const FullAccessID = "1521396112858";
      const ReadOnlyAccessID = "1521396141248";
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

    this.filteroptions$.subscribe(options => {
      this.FilterOptions = options;
    });

    this.ToolbarOptions = this.settingService.getToolbarOptions();
    this.ToolbarOptions.showtoolbar = true;
    this.ToolbarOptions.showcheckAll = false;
    this.ToolbarOptions.showsecondarytoolbar = false;
    this.ToolbarOptions.left_options = this.prepareLeftOptions();
    this.ToolbarOptions.right_options = this.prepareRightOptions();
    this.ToolbarOptions.right_options[0].title = this.ChartType;
  }

  prepareLeftOptions() {
    return [
      {
        title: "Select Type",
        ismultiple: true,
        Options: [
          {
            id: 1,
            title: "Yearly",
            value: 0,
            isclick: true,
            clickevent: "reporty_type",
            tooltip: "Generate yearly report"
          },
          {
            id: 2,
            title: "Monthly (Last 12 Months)",
            value: 1,
            isclick: true,
            clickevent: "reporty_type",
            tooltip: "Generate monthly report"
          },
          {
            id: 3,
            title: "Daily (Current Month)",
            value: 2,
            isclick: true,
            clickevent: "reporty_type",
            tooltip: "Generate daily report"
          }
        ]
      }
    ];
  }

  prepareRightOptions() {
    return [
      {
        title: "Chart Type",
        ismultiple: true,
        Options: [
          {
            id: 1,
            title: "ColumnChart",
            value: "ColumnChart",
            isclick: true,
            clickevent: "chart_type",
            tooltip: "Generate Column Chart"
          },
          {
            id: 2,
            title: "BarChart",
            value: "BarChart",
            isclick: true,
            clickevent: "chart_type",
            tooltip: "Generate Bar Chart"
          },
          {
            id: 3,
            title: "LineChart",
            value: "LineChart",
            isclick: true,
            clickevent: "chart_type",
            tooltip: "Generate Line Chart"
          },
          {
            id: 3,
            title: "PieChart",
            value: "PieChart",
            isclick: true,
            clickevent: "chart_type",
            tooltip: "Generate Pie Chart"
          }
        ]
      }
    ];
  }

  toolbaraction(selection: any) {
    if (!this.isActionGranded) {
      this.coreActions.Notify({
        title: "Permission Denied",
        text: "",
        css: "bg-danger"
      });
      return;
    }
    switch (selection.action) {
     
      case "chart_type":

        this.ChartType = selection.value;
        console.log('chart type is ' + this.ChartType);
        this.showLoader = true;
        this.ToolbarOptions.right_options[0].title = this.ChartType;
        this.FilterOptions.chartType = this.ChartType;
        this.showLoader = false;
        /*if (!this.optionSelected)
           this.LoadReports();
        */
        break;
      case "reporty_type":
        this.FilterOptions.reporttype = selection.value;
        break;
    }

    this.LoadReports();

    
  }

  LoadReports() {
    this.showLoader = true;
    this.dataService.LoadReports(this.FilterOptions).subscribe((report: any) => {
        this.tooltipChart.chartType = this.ChartType;
        if (report.data.dataTable.length > 1) {
          this.tooltipChart.dataTable = report.data.dataTable;
          this.optionSelected = true;
        } else {
          this.message = "No Data Available!";
          this.optionSelected = false;
        }
        this.showLoader = false;
        
    });
  }

}
