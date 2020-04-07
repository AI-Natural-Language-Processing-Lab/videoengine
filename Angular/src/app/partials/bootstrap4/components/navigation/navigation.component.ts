/* -------------------------------------------------------------------------- */
/*                          Product Name: VideoEngine                         */
/*                            Author: Mediasoftpro                            */
/*                       Email: support@mediasoftpro.com                      */
/*       License: Read license.txt located on root of your application.       */
/*                     Copyright 2007 - 2020 @Mediasoftpro                    */
/* -------------------------------------------------------------------------- */

import { Component, OnInit, EventEmitter, Input, Output } from "@angular/core";

@Component({
  selector: "app-navigation-v2",
  templateUrl: "./navigation.html"
})
export class BootstrapNavigationComponent implements OnInit {

  @Input() Options: any;
  @Input() ItemsSelected = false;
  @Input() FilterOptions: any;
  @Input() isAdmin = true;
  @Input() SearchBtnText = "Find Records";
  @Output() OnSelection = new EventEmitter<any>();
  @Output() Action = new EventEmitter<any>();
  @Output() SearchSelection = new EventEmitter<any>();
  @Input() AdvanceText = "Advance Search";
  showAdvanceOptions = false;

  ngOnInit() {}

  onSelect(value) {
    this.Options.selectedcategory = value.id;
  }

  toggleSearch(event) {
    if (this.showAdvanceOptions) {
      this.showAdvanceOptions = false;
      this.AdvanceText = "Advance Search";
    } else {
      this.showAdvanceOptions = true;
      this.AdvanceText = "Hide Search";
    }
    event.stopPropagation();
  }
  /* search option selection */
  FindRecords() {
    if (this.FilterOptions !== undefined) {
      this.FilterOptions.term = this.Options.term;
      /* checkbox processing */
      if (this.Options.filters.length > 0) {
        for (const filter of this.Options.filters) {
          for (var prop in this.FilterOptions) {
            if (prop === filter.attr) {
              if (filter.selected) {
                this.FilterOptions[prop] = filter.value;
              } else {
                this.FilterOptions[prop] = filter.default_value;
              }
            }
          }
        }
      }
      /* radio button processing */
      if (this.Options.checkFilters.length > 0) {
        for (const filter of this.Options.checkFilters) {
          for (var prop in this.FilterOptions) {
            if (prop === filter.attr) {
              this.FilterOptions[prop] = filter.value;
            }
          }
        }
      }
      if (this.Options.dropdownFilters.length > 0) {
        for (const filter of this.Options.dropdownFilters) {
          for (var prop in this.FilterOptions) {
            if (prop === filter.attr) {
              this.FilterOptions[prop] = filter.value;
            }
          }
        }
      }
      
      if (this.Options.selectedcategory !== undefined && this.Options.selectedcategory.length > 0) {
          for (let category of this.Options.selectedcategory) {
             this.FilterOptions.category_ids.push(category.key);
          }
         this.FilterOptions.category_ids = this.Options.selectedcategory;
      }

      if (this.Options.datefilter !== undefined) {
        this.FilterOptions.datefilter = this.Options.datefilter;
      }
      
      this.SearchSelection.emit({ filters: this.FilterOptions });
    }
  }
  /* action trigger */
  toolbaraction(action, value, event) {
    this.Action.emit({ action, value });
    event.stopPropagation();
  }

  choose(filter: any, event: any) {
    filter.value = event.target.value;
  }
}
