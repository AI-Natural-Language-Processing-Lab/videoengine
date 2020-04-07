/* -------------------------------------------------------------------------- */
/*                          Product Name: VideoEngine                         */
/*                            Author: Mediasoftpro                            */
/*                       Email: support@mediasoftpro.com                      */
/*       License: Read license.txt located on root of your application.       */
/*                     Copyright 2007 - 2020 @Mediasoftpro                    */
/* -------------------------------------------------------------------------- */

import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { NgbModule } from "@ng-bootstrap/ng-bootstrap";

import { SettingsComponent } from "./settings.component";
import { DashboardModule } from "./dashboard/dashboard.module";
import { AdverisementModule } from "./advertisements/advertisement.module";
import { BlockIPModule } from "./blockip/blockip.module";
import { CategoriesModule } from "./categories/categories.module";
import { ProcCategoriesModule } from "./categories/process/process.module";

import { ConfigurationsModule } from "./configurations/configuration.module";
import { DictionaryModule } from "./dictionary/dictionary.module";

import { RoleModule } from "./roles/roles.module";
import { LanguagesModule } from "./language/language.module";
import { LogModule } from "./log/log.module";
import { MailTemplatesModule } from "./mailtemplates/mailtemplates.module";
import { ProcMailTemplateModule } from "./mailtemplates/process/process.module";
import { PackagesModule } from "./packages/package.module";
import { ProcPackagesModule } from "./packages/process/process.module";

import { TagsModule } from "./tags/tags.module";
import { SettingsRoutingModule } from "./settings.routing.module";

import { ProcRoleModule } from "./roles/process/process.module";

@NgModule({
  imports: [
    CommonModule,
    NgbModule,
    DashboardModule,
    AdverisementModule,
    BlockIPModule,
    CategoriesModule,
    ConfigurationsModule,
    DictionaryModule,
    LanguagesModule,
    LogModule,
    MailTemplatesModule,
    PackagesModule,
    TagsModule,
    SettingsRoutingModule,
    RoleModule,
    ProcMailTemplateModule,
    ProcPackagesModule,
    ProcCategoriesModule,
    ProcRoleModule
  ],
  declarations: [SettingsComponent]
})
export class SettingsModule {}
