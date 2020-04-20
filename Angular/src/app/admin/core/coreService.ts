/* -------------------------------------------------------------------------- */
/*                          Product Name: VideoEngine                         */
/*                            Author: Mediasoftpro                            */
/*                       Email: support@mediasoftpro.com                      */
/*       License: Read license.txt located on root of your application.       */
/*                     Copyright 2007 - 2020 @Mediasoftpro                    */
/* -------------------------------------------------------------------------- */

import { Injectable } from "@angular/core";
import * as crypto from "crypto-js";
import * as Controls from "../../partials/components/dynamicform/model/elements";

@Injectable()
export class CoreService {
  constructor() {}

  // set secret pharase with your own keyword (to encrypt / decrypt sensetive values)
  pharase = "SECPHARSE4535";

  showMessage(alert: string, message: string, heading: string) {
    return ""; // removed this functionality
  }

  getSettingsNavList() {
    return [
      {
        id: 7,
        title: "Categories",
        css: "",
        url: "/settings/categories",
        icon: "icon-droplet"
      },
      {
        id: 8,
        title: "Tags",
        css: "",
        url: "/settings/tags",
        icon: "icon-droplet"
      },
      {
        id: 8,
        title: "Configurations",
        css: "",
        url: "/settings/configurations",
        icon: "icon-droplet"
      },
      {
        id: 0,
        title: "Advertisement",
        css: "",
        url: "/settings/advertisements",
        icon: "icon-droplet"
      },
      {
        id: 1,
        title: "Block IP",
        css: "",
        url: "/settings/blockip",
        icon: "icon-shutter"
      },
      {
        id: 2,
        title: "Dictionary",
        css: "",
        url: "/settings/dictionary",
        icon: "icon-book"
      },
      {
        id: 3,
        title: "Language",
        css: "",
        url: "/settings/language",
        icon: "icon-books"
      },
      {
        id: 4,
        title: "Log",
        css: "",
        url: "/settings/log",
        icon: "icon-file-empty"
      },
      {
        id: 5,
        title: "Mail Templates",
        css: "",
        url: "/settings/mailtemplates",
        icon: "icon-mailbox"
      },
      {
        id: 6,
        title: "Roles",
        css: "",
        url: "/settings/roles",
        icon: "icon-cash3"
      },
      {
        id: 6,
        title: "Packages",
        css: "",
        url: "/settings/packages",
        icon: "icon-cash3"
      }
    ];
  }

  getPaginationSettings() {
    return {
      title: "Page Size",
      ismultiple: true,
      icon: "",
      Options: [
        {
          id: "0",
          title: "10",
          value: 10,
          isclick: true,
          clickevent: "pagesize",
          tooltip: "Load 10 records per page"
        },
        {
          id: "1",
          title: "20",
          value: 20,
          isclick: true,
          clickevent: "pagesize",
          tooltip: "Load 20 records per page"
        },
        {
          id: "3",
          title: "50",
          value: 50,
          isclick: true,
          clickevent: "pagesize",
          tooltip: "Load 50 records per page"
        },
        {
          id: "5",
          title: "100",
          value: 100,
          isclick: true,
          clickevent: "pagesize",
          tooltip: "Load 100 records per page"
        }
      ]
    };
  }

  encrypt(value) {
    const encrypt = crypto.AES.encrypt(value.toString(), this.pharase);
    return this.replaceAll(encrypt.toString(), "/", "**");
  }

  decrypt(value) {
    // const decrypt_text =  value.toString().replace('**', '/');
    if (value !== undefined && value !== "" && value !== "0") {
      const decrypt_text = this.replaceAll(value.toString(), "**", "/");
      const decrypt = crypto.AES.decrypt(decrypt_text, this.pharase);
      return decrypt.toString(crypto.enc.Utf8);
    } else {
      return 0;
    }
  }

  replaceAll(target, search, replacement) {
    // return target.replace(new RegExp(search, 'g'), replacement);
    return target.split(search).join(replacement);
  }

  updateCategories(controls: any, categories: any) {
    if (controls !== undefined) {
      if (controls.length > 0) {
        for (const control of controls) {
          if (control.key === "categories") {
            let values:any = [];
            for (const category of categories) {
              values.push({
                key: category.id,
                value: category.title
              });
              
            }
            setTimeout(() => {
              control.multiselectOptions.dropdownList = values;
            }, 1000);
           
          }
        }
      }
    }
  }

  // selected list of category items to array of category ids (supported by api) etc. [34, 43]
  returnSelectedCategoryArray(categories: any) {
    let categorylist = [];
    if (categories !== undefined && categories.length > 0) {
      for (let category of categories) {
        categorylist.push(category.key);
      }
    }
    return categorylist;
  }

  /* -------------------------------------------------------------------------- */
  /*                  ng-multiselect-dropdown dropdown settings                 */
  /* -------------------------------------------------------------------------- */
  getMultiCategorySettings() {
    return {
      placeholder: "Select Categories",
      dropdownList: [],
      dropdownSettings: {
        singleSelection: false,
        idField: "key",
        textField: "value",
        selectAllText: "Select All",
        unSelectAllText: "UnSelect All",
        itemsShowLimit: 5,
        allowSearchFilter: true
      }
    };
  }

  /* -------------------------------------------------------------------------- */
  /*             Return category array with ng-multiselect-dropdown             */
  /* -------------------------------------------------------------------------- */
  prepareSelectedItems(catorylist: any) {
    let selectedItems = [];
    if (catorylist !== undefined && catorylist.length > 0) {
      for (let item of catorylist) {
        selectedItems.push({
          key: item.category.id,
          value: item.category.title
        });
      }
    }
    return selectedItems;
  }

  /* -------------------------------------------------------------------------- */
  /*                          Tinymice Limited Settings                         */
  /* -------------------------------------------------------------------------- */
  prepareInitEditorSettings() {
    return {
      base_url: "/tinymce", // Root for resources
      suffix: ".min", // Suffix to use when loading resources
      height: 450,
      plugins: "lists advlist",
      toolbar: "undo redo | bold italic | bullist numlist outdent indent"
    };
  }

  /* -------------------------------------------------------------------------- */
  /*                          Tinymice Advance Settings                         */
  /* -------------------------------------------------------------------------- */
  prepareInitAdvacneEditorSettings() {
    return {
      base_url: "/tinymce", // Root for resources
      suffix: ".min", // Suffix to use when loading resources
      plugins:
        "print preview paste importcss searchreplace autolink autosave save directionality code visualblocks visualchars fullscreen image link media template codesample table charmap hr pagebreak nonbreaking anchor toc insertdatetime advlist lists wordcount imagetools textpattern noneditable help charmap quickbars emoticons",
      menubar: "file edit view insert format tools table help",
      toolbar:
        "undo redo | bold italic underline strikethrough | fontselect fontsizeselect formatselect | alignleft aligncenter alignright alignjustify | outdent indent |  numlist bullist | forecolor backcolor removeformat | pagebreak | charmap emoticons | fullscreen  preview save print | insertfile image media template link anchor codesample | ltr rtl",
      toolbar_sticky: true,
      autosave_ask_before_unload: true,
      autosave_interval: "30s",
      autosave_prefix: "{path}{query}-{id}-",
      autosave_restore_when_empty: false,
      autosave_retention: "2m",
      image_advtab: true,
      content_css: [
        "//fonts.googleapis.com/css?family=Lato:300,300i,400,400i",
        "//www.tiny.cloud/css/codepen.min.css"
      ],
      importcss_append: true,
      height: 600,
      template_cdate_format: "[Date Created (CDATE): %m/%d/%Y : %H:%M:%S]",
      template_mdate_format: "[Date Modified (MDATE): %m/%d/%Y : %H:%M:%S]",
      image_caption: true,
      quickbars_selection_toolbar:
        "bold italic | quicklink h2 h3 blockquote quickimage quicktable",
      noneditable_noneditable_class: "mceNonEditable",
      toolbar_drawer: "sliding"
    };
  }

  /* -------------------------------------------------------------------------- */
  /*                      dynamic form attribute processing                     */
  /* -------------------------------------------------------------------------- */
  renderDynamicControls(controls: any, options: any, isedit: boolean) {
    if (options.length > 0) {
      let orderIndex = 100;
      for (let option of options) {
        console.log('show section is ' + option.showsection);
        if (option.showsection === 1 || option.showsection === 2) {
          controls.push(
            new Controls.SectionHeader({
              key: "section_" + orderIndex,
              label: option.title,
              order: orderIndex
            })
          );
        }

        // attributes
        for (let attr of option.attributes) {
          orderIndex = orderIndex + 1;
          let isRquired = false;
          if (attr.isrequired === 1) {
            isRquired = true;
          }
          let pattern = "";
          if (attr.variable_type === 1) {
            pattern = "[0-9]+";
          }

          let PlaceHolder = "";
          let ElementValue = "";

          if (isedit) {
            ElementValue = attr.value;
          } else {
            PlaceHolder = attr.value;
          }

          let Order = orderIndex;
          // if group by elements by section is disabled, use priority as order index to arrange your elements with your settings
          if (option.showsection === 0) {
            Order = attr.priority;
          }
          switch (attr.element_type) {
            case 0:
              // text box
              const control_obj: any = {
                key: "attr_" + attr.id,
                label: attr.title,
                placeholder: PlaceHolder,
                value: ElementValue,
                required: isRquired,
                order: Order,
                pattern: pattern,
                helpblock: attr.helpblock
              };
              if (attr.min > 0) {
                control_obj.minLength = attr.min;
              }
              if (attr.max > 0) {
                control_obj.minLength = attr.max;
              }
              controls.push(new Controls.Textbox(control_obj));
              break;
            case 1:
              // text area
              const text_area_control_obj: any = {
                key: "attr_" + attr.id,
                label: attr.title,
                placeholder: PlaceHolder,
                value: ElementValue,
                required: isRquired,
                order: Order,
                pattern: pattern,
                helpblock: attr.helpblock
              };
              if (attr.min > 0) {
                text_area_control_obj.minLength = attr.min;
              }
              if (attr.max > 0) {
                text_area_control_obj.maxLength = attr.max;
              }
              controls.push(new Controls.TextArea(text_area_control_obj));
              break;
            case 5:
              // rich text area
              const rich_text_control_obj: any = {
                key: "attr_" + attr.id,
                label: attr.title,
                value: ElementValue,
                required: isRquired,
                tinymiceOptions: this.prepareInitEditorSettings(),
                order: Order,
                helpblock: attr.helpblock
              };
              if (attr.min > 0) {
                rich_text_control_obj.minLength = attr.min;
              }
              if (attr.max > 0) {
                rich_text_control_obj.maxLength = attr.max;
              }
              controls.push(new Controls.TinyMyceEditor(rich_text_control_obj));
              break;
            case 2:
              // dropdown
              let options = [];
              options.push({ key: "", value: "Select " + attr.title });
              if (attr.variable_type === 2) {
                  // year dropdown
                  let max_year: number = attr.max;
                  let min_year: number = attr.min;
                  if (attr.max === 0) {
                     max_year = new Date().getFullYear()
                  }
                  if (attr.min === 0) {
                    min_year = max_year - 20;
                  }
                 
                  for (let i = max_year; i >= min_year; i--) {
                    options.push({ key: i.toString(), value: i.toString() });
                  }
                  
              } else {
                if (attr.options !== "") {
                  var nameArr = attr.options.split(",");
                  for (let item of nameArr) {
                    options.push({ key: item, value: item });
                  }
                }
              }
              controls.push(
                new Controls.Dropdown({
                  key: "attr_" + attr.id,
                  label: attr.title,
                  required: isRquired,
                  value: ElementValue,
                  options: options,
                  order: Order,
                  helpblock: attr.helpblock
                })
              );

              break;
            case 3:
              // checkbox
              let _value = ElementValue == "true" ? true : false;
              controls.push(
                new Controls.CheckBox({
                  key: "attr_" + attr.id,
                  label: attr.title,
                  value: _value,
                  checked: _value,
                  required: isRquired,
                  order: Order,
                  helpblock: attr.helpblock
                })
              );
              break;
            case 4:
              // radio button list
              if (attr.options !== "") {
                var nameArr = attr.options.split(",");
                let options = [];
                options.push({ key: "", value: "Select " + attr.title });
                for (let item of nameArr) {
                  options.push({ key: item, value: item });
                }
                controls.push(
                  new Controls.RadioButtonList({
                    key: "attr_" + attr.id,
                    label: attr.title,
                    required: isRquired,
                    value: ElementValue,
                    options: options,
                    order: Order,
                    helpblock: attr.helpblock
                  })
                );
              }
              break;
          }
        }
        // close attributes
        orderIndex = orderIndex + 100;
      }
    }
  }

  /* -------------------------------------------------------------------------- */
  /*              map saved data with available dynamic attributes              */
  /* -------------------------------------------------------------------------- */
  prepareDynamicControlData(info:any) {
    if (info.options.length > 0) {
      for (let option of info.options) {
        for (let attr of option.attributes) {
          for (let value of info.attr_values) {
            if (value.attr_id === attr.id) {
              attr.value = value.value;
            }
          }
        }
      }
    }
  }

  /* -------------------------------------------------------------------------- */
  /*                 process submit dynamic data for submission                 */
  /* -------------------------------------------------------------------------- */
  processDynamicControlsData(payload:any, info: any) {
    const arr = [];
    for (const prop of Object.keys(payload)) {
      if (prop.includes("attr_")) {
        const id = parseInt(prop.replace("attr_", ""), 10);
        const obj: any = {
          id: 0,
          value: payload[prop]
        };

        if (info.attr_values.length > 0) {
          // edit case
          for (const attr of info.attr_values) {
            if (attr.attr_id === id) {
              obj.id = attr.id;
              obj.title = attr.title;
              obj.priority = attr.priority;
            }
          }
        } else {
          // insert case
          for (const option of info.options) {
            for (const attr of option.attributes) {
              if (attr.id === id) {
                obj.attr_id = id;
                obj.title = attr.title;
                obj.priority = attr.priority;
              }
            }
          }
        }
        arr.push(obj);
      }
    }
    return arr;
  }

  /* -------------------------------------------------------------------------- */
  /*                         render abuse report button                         */
  /* -------------------------------------------------------------------------- */
  renderAbuseReportBtn(searchOptions: any, loadabusereports: boolean = false) {
      for(let action of searchOptions) {
        if (loadabusereports) {
            if (action.id === 100) {
                action.title = "Normal List";
                action.event = "normallist";
                action.toolbar = "Back to normal list";
            }
        } else {
            if (action.id === 100) {
                action.title = "Abuse Reports";
                action.event = "abuse";
                action.toolbar = "Load Reported Records";
            }
        }
        
      }
  }

  /* -------------------------------------------------------------------------- */
  /*                             generate random id                             */
  /* -------------------------------------------------------------------------- */
  makeid() {
    let text = "";
    const possible =
      "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

    for (let i = 0; i < 5; i++) {
      text += possible.charAt(Math.floor(Math.random() * possible.length));
    }
    return text;
  }
}
