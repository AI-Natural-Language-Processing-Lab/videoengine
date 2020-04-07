import { Component, Input, Output, OnInit, EventEmitter } from "@angular/core";
@Component({
  selector: "app-display-file",
  templateUrl: "./file.html",
  providers: []
})
export class DisplayFileComponent implements OnInit {
  @Input() files: any = [];
  @Output() onRemove = new EventEmitter<any>();

  constructor() {}

  ngOnInit() {}

  remove(obj, index, event) {
    if (index > -1) {
      this.files.splice(index, 1);
      this.onRemove.emit({ files: this.files, removedItems: obj });
    }
    event.stopPropagation();
  }
}
