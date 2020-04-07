/* -------------------------------------------------------------------------- */
/*                          Product Name: VideoEngine                         */
/*                            Author: Mediasoftpro                            */
/*                       Email: support@mediasoftpro.com                      */
/*       License: Read license.txt located on root of your application.       */
/*                     Copyright 2007 - 2020 @Mediasoftpro                    */
/* -------------------------------------------------------------------------- */

import { combineReducers } from "redux";
import { composeReducers, defaultFormReducer } from "@angular-redux/form";
import { routerReducer } from "@angular-redux/router";


import { createConfigReducer } from "../configs/reducer";
import { createUsersReducer } from "../users/reducer";
import { createCoreReducer } from "../core/reducer";
import { createVideosReducer } from "../videos/reducer";
import { createAdvertisementReducer } from "../settings/advertisements/reducer";
import { createBlockIPReducer } from "../settings/blockip/reducer";
import { createCategoriesReducer } from "../settings/categories/reducer";
import { createConfigurationsReducer } from "../settings/configurations/reducer";
import { createDictionaryReducer } from "../settings/dictionary/reducer";
import { createLanguageReducer } from "../settings/language/reducer";
import { createLogReducer } from "../settings/log/reducer";
import { createMailTemplateReducer } from "../settings/mailtemplates/reducer";
import { createPackagesReducer } from "../settings/packages/reducer";
import { createTagsReducer } from "../settings/tags/reducer";
import { createRoleReducer } from "../settings/roles/reducer";
import { createAbuseReportReducer } from "../reports/abuse/reducer";

// myaccount reducers
import { createAccountPackagesReducer } from "../account/packages/reducer";
import { createAccountHistoryReducer } from "../account/history/reducer";

// Define the global store shape by combining our application's
// reducers together into a given structure.
export const rootReducer = composeReducers(
  defaultFormReducer(),
  combineReducers({
    configuration: createConfigReducer(),
    users: createUsersReducer(),
    core: createCoreReducer(),
    videos: createVideosReducer(),
    advertisement: createAdvertisementReducer(),
    blockip: createBlockIPReducer(),
    categories: createCategoriesReducer(),
    configurations: createConfigurationsReducer(),
    dictionary: createDictionaryReducer(),
    language: createLanguageReducer(),
    log: createLogReducer(),
    mailtemplates: createMailTemplateReducer(),
    packages: createPackagesReducer(),
    tags: createTagsReducer(),
    roles: createRoleReducer(),
    abuse: createAbuseReportReducer(),
    accountpackages: createAccountPackagesReducer(),
    accounthistory: createAccountHistoryReducer(),
    router: routerReducer
  })
);
