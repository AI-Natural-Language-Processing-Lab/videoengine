/* -------------------------------------------------------------------------- */
/*                          Product Name: VideoEngine                         */
/*                            Author: Mediasoftpro                            */
/*                       Email: support@mediasoftpro.com                      */
/*       License: Read license.txt located on root of your application.       */
/*                     Copyright 2007 - 2020 @Mediasoftpro                    */
/* -------------------------------------------------------------------------- */

export interface iStyleConfig {
  tabular: boolean;
}

export interface iUploadOptions {
  handlerpath: string;
  pickfilecaption: string;
  uploadfilecaption: string;
  max_file_size: string;
  chunksize: string;
  plupload_root: string;
  headers: any;
  extensiontitle: string;
  extensions: string;
  filepath?: string;
  removehandler?: string;
}
