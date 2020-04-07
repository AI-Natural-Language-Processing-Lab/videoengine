/* -------------------------------------------------------------------------- */
/*                          Product Name: VideoEngine                         */
/*                            Author: Mediasoftpro                            */
/*                       Email: support@mediasoftpro.com                      */
/*       License: Read license.txt located on root of your application.       */
/*                     Copyright 2007 - 2020 @Mediasoftpro                    */
/* -------------------------------------------------------------------------- */

import { Component, OnInit, Input, Output, EventEmitter } from "@angular/core";

@Component({
  selector: "app-searchnavigation",
  templateUrl: "./navigation.html"
})
export class NavigationComponent implements OnInit {
  @Input() Type = 0; // 0: Horizontal Filter i: Top Filter
  @Input() Options: any;
  @Input() FilterOptions: any;
  @Output() OnFilterSelected = new EventEmitter<any>();

  showFilterOptions = false;
  ngOnInit() {}

  prepareSearchOptions() {
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

    // category selection
    if (this.Options.categories.length > 0) {
      this.FilterOptions.category_ids = [];
      for (let category of this.Options.categories) {
        if (category.selected) {
          this.FilterOptions.category_ids.push(category.key);
        }
      }
    }

    this.OnFilterSelected.emit(this.FilterOptions);
  }

  choose(filter: any, event: any) {
    filter.value = event.target.value;
    this.prepareSearchOptions();
  }

  togglerFilter() {
    if (this.showFilterOptions) {
      this.showFilterOptions = false;
    } else {
      this.showFilterOptions = true;
    }
  }

  selectOption(option: any, nav: any, event: any) {
    option.value = nav.value;
    for (let item of this.Options.NavList) {
      for (var prop in this.FilterOptions) {
        if (prop === item.attr) {
          this.FilterOptions[prop] = item.value;
        }
      }
    }
    this.OnFilterSelected.emit(this.FilterOptions);
    this.showFilterOptions = false;
    event.stopPropagation();
  }

}
