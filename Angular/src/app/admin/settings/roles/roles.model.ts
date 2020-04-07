/* -------------------------------------------------------------------------- */
/*                          Product Name: VideoEngine                         */
/*                            Author: Mediasoftpro                            */
/*                       Email: support@mediasoftpro.com                      */
/*       License: Read license.txt located on root of your application.       */
/*                     Copyright 2007 - 2020 @Mediasoftpro                    */
/* -------------------------------------------------------------------------- */

export interface IAPIOptions {
  load_roles: string;
  load_objects: string;
  getinfo: string;
  add_role: string;
  add_object: string;
  delete_role: string;
  delete_object: string;
  update_permission: string;
}

export interface RoleEntity {
  id: number;
  rolename: string;
}

export interface RoleObjectEntity {
  id: number;
  objectname: string;
  description: string;
  uniqueid: string;
}

export interface RolePermissionEntity {
  id: number;
  roleid: string;
  objectid: string;
  roleobject: RoleObjectEntity;
}
