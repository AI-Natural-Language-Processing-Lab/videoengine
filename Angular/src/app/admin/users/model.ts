/* -------------------------------------------------------------------------- */
/*                          Product Name: VideoEngine                         */
/*                            Author: Mediasoftpro                            */
/*                       Email: support@mediasoftpro.com                      */
/*       License: Read license.txt located on root of your application.       */
/*                     Copyright 2007 - 2020 @Mediasoftpro                    */
/* -------------------------------------------------------------------------- */

export interface IAPIOptions {
  load: string;
  load_reports: string;
  getinfo: string;
  getinfo_auth: string;
  action: string;
  proc: string;
  updatethumb: string;
  cemail: string;
  chpass: string;
  ctype: string;
  userlog: string;
  authenticate: string;
  updaterole: string;
  updateavator: string;
  archive: string;
}

export interface LOGINENTITY {
  username: string;
  password: string;
  rememberme: boolean;
}
export interface UserEntity {
  Id: string;
  role_name: string;
  username: string;
  email: string;
  firstname: string;
  lastname: string;
  password: string;
  cpassword: string;
  opassword: string;
  aboutme: string;
  gender: string;
  website: string;
  isallowbirthday: number;
  relationshipstatus: string;
  hometown: string;
  currentcity: string;
  zipcode: string;
  countryname: string;
  description: string;
  files: any;
  picture: string;
}
