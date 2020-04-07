/* -------------------------------------------------------------------------- */
/*                          Product Name: VideoEngine                         */
/*                            Author: Mediasoftpro                            */
/*                       Email: support@mediasoftpro.com                      */
/*       License: Read license.txt located on root of your application.       */
/*                     Copyright 2007 - 2020 @Mediasoftpro                    */
/* -------------------------------------------------------------------------- */

import { Component } from "@angular/core";

@Component({
  selector: "app-spinner01",
  template: `
    <div class="spinner"></div>
  `,
  styles: [
    `
      .spinner {
        width: 40px;
        height: 40px;
        background-color: #fb008b;
        margin: 100px auto;
        -webkit-animation: sk-rotateplane 1.2s infinite ease-in-out;
        animation: sk-rotateplane 1.2s infinite ease-in-out;
      }

      @-webkit-keyframes sk-rotateplane {
        0% {
          -webkit-transform: perspective(120px);
        }
        50% {
          -webkit-transform: perspective(120px) rotateY(180deg);
        }
        100% {
          -webkit-transform: perspective(120px) rotateY(180deg) rotateX(180deg);
        }
      }

      @keyframes sk-rotateplane {
        0% {
          transform: perspective(120px) rotateX(0deg) rotateY(0deg);
          -webkit-transform: perspective(120px) rotateX(0deg) rotateY(0deg);
        }
        50% {
          transform: perspective(120px) rotateX(-180.1deg) rotateY(0deg);
          -webkit-transform: perspective(120px) rotateX(-180.1deg) rotateY(0deg);
        }
        100% {
          transform: perspective(120px) rotateX(-180deg) rotateY(-179.9deg);
          -webkit-transform: perspective(120px) rotateX(-180deg)
            rotateY(-179.9deg);
        }
      }
    `
  ]
})
export class Spinner1Component {}
