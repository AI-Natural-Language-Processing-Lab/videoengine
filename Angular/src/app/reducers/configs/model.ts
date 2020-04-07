/* -------------------------------------------------------------------------- */
/*                          Product Name: VideoEngine                         */
/*                            Author: Mediasoftpro                            */
/*                       Email: support@mediasoftpro.com                      */
/*       License: Read license.txt located on root of your application.       */
/*                     Copyright 2007 - 2020 @Mediasoftpro                    */
/* -------------------------------------------------------------------------- */

export interface IConfigState {
  configs: any;
  selected_value: string;
  loading: boolean;
  error: any;
}

export const CNF_INITIAL_STATE: IConfigState = {
  configs: {},
  selected_value: "",
  loading: false,
  error: null
};
