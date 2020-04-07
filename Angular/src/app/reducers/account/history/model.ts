/* -------------------------------------------------------------------------- */
/*                          Product Name: VideoEngine                         */
/*                            Author: Mediasoftpro                            */
/*                       Email: support@mediasoftpro.com                      */
/*       License: Read license.txt located on root of your application.       */
/*                     Copyright 2007 - 2020 @Mediasoftpro                    */
/* -------------------------------------------------------------------------- */

export interface IHistory {
  posts: any;
  loading: boolean;
  error: any;
  isloaded: boolean;
}

export const ACCOUNT_HISTORY_INITIAL_STATE: IHistory = {
  posts: [],
  loading: false,
  error: null,
  isloaded: false
};
