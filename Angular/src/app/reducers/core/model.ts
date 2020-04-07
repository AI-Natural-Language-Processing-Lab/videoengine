/* -------------------------------------------------------------------------- */
/*                          Product Name: VideoEngine                         */
/*                            Author: Mediasoftpro                            */
/*                       Email: support@mediasoftpro.com                      */
/*       License: Read license.txt located on root of your application.       */
/*                     Copyright 2007 - 2020 @Mediasoftpro                    */
/* -------------------------------------------------------------------------- */

export interface ICoreState {
  message: any;
  notify: any;
  auth_failed: any;
  liststats: any; // component showing list stats e.g showing 1 to 20 of 1234 records.
  event: any; // any data normally passed through Output() to parent element
  loader: boolean; // global loader
}

export const CORE_INITIAL_STATE: ICoreState = {
  message: {
    alert: "danger",
    heading: "",
    message: ""
  },
  notify: {
    title: "",
    text: "",
    css: ""
  },
  auth_failed: {
     title: ""
  },
  liststats: {
    first_boundary: 0,
    last_boundary: 0,
    totalrecords: 0
  },
  event: {},
  loader: false
};
