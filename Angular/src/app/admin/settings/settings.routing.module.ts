/* -------------------------------------------------------------------------- */
/*                          Product Name: VideoEngine                         */
/*                            Author: Mediasoftpro                            */
/*                       Email: support@mediasoftpro.com                      */
/*       License: Read license.txt located on root of your application.       */
/*                     Copyright 2007 - 2020 @Mediasoftpro                    */
/* -------------------------------------------------------------------------- */

import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";

import { SettingsComponent } from "./settings.component";
import { SettingsDashboardComponent } from "./dashboard/dashboard.component";
import { AdvertisemntComponent } from "./advertisements/advertisement.component";
import { BlockIPComponent } from "./blockip/blockip.component";
import { CategoriesComponent } from "./categories/categories.component";
import { ProcCategoriesComponent } from "./categories/process/process.component";
import { ConfigurationComponent } from "./configurations/configuration.component";
import { RolesComponent } from "./roles/roles.components";
import { ProcRoleComponent } from "./roles/process/process.component";

import { DictionaryComponent } from "./dictionary/dictionary.component";

import { LanguageComponent } from "./language/language.component";
import { LogComponent } from "./log/log.component";

import { MailTemplatesComponent } from "./mailtemplates/mailtemplates.component";
import { ProcMailTemplateComponent } from "./mailtemplates/process/process.component";

import { PackagesComponent } from "./packages/package.component";
import { ProcPackageComponent } from "./packages/process/process.component";

import { TagsComponent } from "./tags/tags.components";

const settingRoutes: Routes = [
  {
    path: "",
    component: SettingsComponent,
    data: {
      title: "Settings Management",
      urls: [
        { title: "Dashboard", url: "/" },
        { title: "Settings", url: "/settings" },
        { title: "Management" }
      ]
    },
    children: [
      { path: "", component: SettingsDashboardComponent },
      {
        path: "blockip",
        children: [{ path: "", component: BlockIPComponent }],
        data: {
          title: "BlockIP Management",
          urls: [
            { title: "Dashboard", url: "/" },
            { title: "Settings", url: "/settings" },
            { title: "Block IP", url: "/settings/blockip" },
            { title: "Management" }
          ]
        }
      },
      {
        path: "categories",
        children: [
          { path: "", component: CategoriesComponent },
          { path: "process/:id", component: ProcCategoriesComponent },
          { path: "process/add/:parentid", component: ProcCategoriesComponent },
          { path: "process", component: ProcCategoriesComponent }
        ],
        data: {
          title: "Categories Management",
          urls: [
            { title: "Dashboard", url: "/" },
            { title: "Settings", url: "/settings" },
            { title: "Categories", url: "/settings/categories" },
            { title: "Management" }
          ]
        }
      },
      {
        path: "configurations",
        children: [
          { path: "", component: ConfigurationComponent },
          { path: "filter/:type", component: ConfigurationComponent }
        ],
        data: {
          title: "Configuration Management",
          urls: [
            { title: "Dashboard", url: "/" },
            { title: "Settings", url: "/settings" },
            { title: "Configurations", url: "/settings/configurations" },
            { title: "Management" }
          ]
        }
      },
      {
        path: "dictionary",
        children: [{ path: "", component: DictionaryComponent }],
        data: {
          title: "Dictionary Management",
          urls: [
            { title: "Dashboard", url: "/" },
            { title: "Settings", url: "/settings" },
            { title: "Dictionary", url: "/settings/dictionary" },
            { title: "Management" }
          ]
        }
      },
      {
        path: "roles",
        children: [
          { path: "", component: RolesComponent },
          { path: "process/:id", component: ProcRoleComponent }
        ],
        data: {
          title: "Roles Management",
          urls: [
            { title: "Dashboard", url: "/" },
            { title: "Settings", url: "/settings" },
            { title: "Roles", url: "/settings/roles" },
            { title: "Management" }
          ]
        }
      },
      {
        path: "language",
        children: [{ path: "", component: LanguageComponent }],
        data: {
          title: "Language Management",
          urls: [
            { title: "Dashboard", url: "/" },
            { title: "Settings", url: "/settings" },
            { title: "Language", url: "/settings/language" },
            { title: "Management" }
          ]
        }
      },
      {
        path: "log",
        children: [{ path: "", component: LogComponent }],
        data: {
          title: "Log Management",
          urls: [
            { title: "Dashboard", url: "/" },
            { title: "Settings", url: "/settings" },
            { title: "Log", url: "/settings/log" },
            { title: "Management" }
          ]
        }
      },
      {
        path: "mailtemplates",
        children: [
          { path: "", component: MailTemplatesComponent },
          { path: "process/:id", component: ProcMailTemplateComponent },
          { path: "process", component: ProcMailTemplateComponent }
        ],
        data: {
          title: "Mailtemplates Management",
          urls: [
            { title: "Dashboard", url: "/" },
            { title: "Settings", url: "/settings" },
            { title: "Mail Templates", url: "/settings/mailtemplates" },
            { title: "Management" }
          ]
        }
      },
      {
        path: "packages",
        children: [
          { path: "", component: PackagesComponent },
          { path: "process/:id", component: ProcPackageComponent },
          { path: "process", component: ProcPackageComponent }
        ],
        data: {
          title: "Packages Management",
          urls: [
            { title: "Dashboard", url: "/" },
            { title: "Settings", url: "/settings" },
            { title: "Packages", url: "/settings/packages" },
            { title: "Management" }
          ]
        }
      },
      {
        path: "tags",
        children: [{ path: "", component: TagsComponent }],
        data: {
          title: "Tags Management",
          urls: [
            { title: "Dashboard", url: "/" },
            { title: "Settings", url: "/settings" },
            { title: "Tags", url: "/settings/tags" },
            { title: "Management" }
          ]
        }
      },
      {
        path: "advertisements",
        data: {
          title: "Advertisement Management",
          urls: [
            { title: "Dashboard", url: "/" },
            { title: "Settings", url: "/settings" },
            { title: "Advertisement", url: "/settings/advertisements" },
            { title: "Management" }
          ]
        },
        children: [{ path: "", component: AdvertisemntComponent }]
      }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(settingRoutes)],
  exports: [RouterModule]
})
export class SettingsRoutingModule {}
