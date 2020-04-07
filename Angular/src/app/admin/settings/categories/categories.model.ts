/* -------------------------------------------------------------------------- */
/*                          Product Name: VideoEngine                         */
/*                            Author: Mediasoftpro                            */
/*                       Email: support@mediasoftpro.com                      */
/*       License: Read license.txt located on root of your application.       */
/*                     Copyright 2007 - 2020 @Mediasoftpro                    */
/* -------------------------------------------------------------------------- */

export interface IAPIOptions {
  load: string;
  getinfo: string;
  action: string;
  proc: string;
  load_dropdown: string;
}

export interface CategoriesEntity {
  id: number;
  title: string;
  term: string;
  description: string;
  parentid: number;
  priority: number;
  mode: number;
  isenabled: number;
  type: number;
  picturename: string;
  icon: string;
  files: any;
  img_url: string;
}
