/* -------------------------------------------------------------------------- */
/*                          Product Name: VideoEngine                         */
/*                            Author: Mediasoftpro                            */
/*                       Email: support@mediasoftpro.com                      */
/*       License: Read license.txt located on root of your application.       */
/*                     Copyright 2007 - 2020 @Mediasoftpro                    */
/* -------------------------------------------------------------------------- */

import { IUserPage, USER_INITIAL_STATE } from "../users/model";
import { ICoreState, CORE_INITIAL_STATE } from "../core/model";
import { VIDEOS_INITIAL_STATE, IVideoState } from "../videos/model";

import {
  IAdvertisementState,
  ADS_INITIAL_STATE
} from "../settings/advertisements/model";
import {
  IBlockIPState,
  BLOCK_IP_INITIAL_STATE
} from "../settings/blockip/model";
import {
  ICategoriesState,
  CATEGORIES_INITIAL_STATE
} from "../settings/categories/model";

import {
  IConfigurationState,
  CONFIG_INITIAL_STATE
} from "../settings/configurations/model";
import {
  IDictionaryState,
  DICTIONARY_INITIAL_STATE
} from "../settings/dictionary/model";
import {
  ILanguageState,
  LANGUAGE_INITIAL_STATE
} from "../settings/language/model";
import { ILogState, LOG_INITIAL_STATE } from "../settings/log/model";
import {
  IMailtemplateStates,
  MAIL_INITIAL_STATE
} from "../settings/mailtemplates/model";
import {
  IPackageState,
  PACKAGE_INITIAL_STATE
} from "../settings/packages/model";
import { IRoles, ROLE_INITIAL_STATE } from "../settings/roles/model";
import { ITagState, TAGS_INITIAL_STATE } from "../settings/tags/model";
import { IConfigState, CNF_INITIAL_STATE } from "../configs/model";

// account application
import {
  IPackages,
  ACCOUNT_PACKAGES_INITIAL_STATE
} from "../account/packages/model";
import {
  IHistory,
  ACCOUNT_HISTORY_INITIAL_STATE
} from "../account/history/model";

import { IAbuseReportStats, ABUSE_INITIAL_STATE } from "../reports/abuse/model";

export type AppState = { [key in any]: any } &
  Partial<{
    configuration: IConfigState;
    configurations: IConfigurationState;
    users: IUserPage;
    core: ICoreState;
    videos: IVideoState;
    advertisement: IAdvertisementState;
    blockip: IBlockIPState;
    categories: ICategoriesState;
    dictionary: IDictionaryState;
    language: ILanguageState;
    log: ILogState;
    mailtemplates: IMailtemplateStates;
    packages: IPackageState;
    tags: ITagState;
    accountpackages: IPackages;
    accounthistory: IHistory;
    roles: IRoles;
    abuse: IAbuseReportStats;
    routes: string;
    feedback: unknown;
  }>;

export function initialAppState() {
  return {
    users: USER_INITIAL_STATE,
    configuration: CNF_INITIAL_STATE,
    configurations: CONFIG_INITIAL_STATE,
    core: CORE_INITIAL_STATE,
    videos: VIDEOS_INITIAL_STATE,
    advertisement: ADS_INITIAL_STATE,
    blockip: BLOCK_IP_INITIAL_STATE,
    categories: CATEGORIES_INITIAL_STATE,
    dictionary: DICTIONARY_INITIAL_STATE,
    language: LANGUAGE_INITIAL_STATE,
    log: LOG_INITIAL_STATE,
    mailtemplates: MAIL_INITIAL_STATE,
    packages: PACKAGE_INITIAL_STATE,
    tags: TAGS_INITIAL_STATE,
    roles: ROLE_INITIAL_STATE,
    abuse: ABUSE_INITIAL_STATE,
    accountpackages: ACCOUNT_PACKAGES_INITIAL_STATE,
    accounthistory: ACCOUNT_HISTORY_INITIAL_STATE
  };
}
