/* -------------------------------------------------------------------------- */
/*                          Product Name: VideoEngine                         */
/*                            Author: Mediasoftpro                            */
/*                       Email: support@mediasoftpro.com                      */
/*       License: Read license.txt located on root of your application.       */
/*                     Copyright 2007 - 2020 @Mediasoftpro                    */
/* -------------------------------------------------------------------------- */
import {
  Component,
  Input,
  Output,
  OnChanges,
  EventEmitter
} from "@angular/core";
@Component({
  selector: "app-multiple-options",
  templateUrl: "./multioptions.html",
  providers: []
})
export class MultiOptionsComponent implements OnChanges {

  @Input() options: any = [];

  @Output() onChange = new EventEmitter<any>();

  constructor() {}

  ngOnChanges() {
     console.log('changes detected');
     console.log(this.options);
  }

  AddOption(event: any) {
      this.options.push({id: this.options.length + 1, value: "", isnew: true});
      this.onChange.emit(this.options);
      event.stopPropagation();
  }

  removeOption(option, index, event) {
    this.options.splice(index, 1);
    this.onChange.emit(this.options)
    event.stopPropagation();
  }

  onSearchChange(id: number, searchValue: string): void {  
    for (let item of this.options) {
      if (item.id === id) {
         item.value = searchValue;
      }
      this.onChange.emit(this.options)
    }
  }

}
