/* -------------------------------------------------------------------------- */
/*                          Product Name: VideoEngine                         */
/*                            Author: Mediasoftpro                            */
/*                       Email: support@mediasoftpro.com                      */
/*       License: Read license.txt located on root of your application.       */
/*                     Copyright 2007 - 2020 @Mediasoftpro                    */
/* -------------------------------------------------------------------------- */

import { Routes } from "@angular/router";
import { JMediaLazyGuard } from "./j-media-guard.guard";

export const appRoutes: Routes = [
  
  {
    path: "login",
    loadChildren: () => import('./admin/login/login.module').then(m => m.LoginModule)
    // loadChildren: "./admin/login/login.module#LoginModule"
  },
  {
    path: "settings",
    //canActivate: [JMediaLazyGuard],
    loadChildren: () => import('./admin/settings/settings.module').then(m => m.SettingsModule)
    // loadChildren: "./admin/settings/settings.module#SettingsModule"
  },
  {
    path: "users",
    //canActivate: [JMediaLazyGuard],
    loadChildren: () => import('./admin/users/module').then(m => m.UserModule)
    // loadChildren: "./admin/users/module#UserModule"
  },
  {
    path: "videos",
    //canActivate: [JMediaLazyGuard],
    loadChildren: () => import('./admin/videos/videos.module').then(m => m.VideosModule)
    // loadChildren: "./admin/videos/videos.module#VideosModule"
  },
  // my account routes
  {
    path: "email-options",
    //canActivate: [JMediaLazyGuard],
    loadChildren: () => import('./account/email-options/emailoptions.module').then(m => m.EmailOptionModule)
    //loadChildren:
    //  "./account/email-options/emailoptions.module#EmailOptionModule"
  },
  {
    path: "manage-account",
    //canActivate: [JMediaLazyGuard],
    loadChildren: () => import('./account/manage-account/manageaccount.module').then(m => m.ManageAccountModule)
    //loadChildren:
    //  "./account/manage-account/manageaccount.module#ManageAccountModule"
  },
  {
    path: "payments",
    //canActivate: [JMediaLazyGuard],
    loadChildren: () => import('./account/payments/payments.module').then(m => m.PaymentModule)
    // loadChildren: "./account/payments/payments.module#PaymentModule"
  },
  {
    path: "profile-setup",
    //canActivate: [JMediaLazyGuard],
    loadChildren: () => import('./account/profile-setup/profile.setup.module').then(m => m.ProfileSetupModule)
    //loadChildren:
    //  "./account/profile-setup/profile.setup.module#ProfileSetupModule"
  },
  {
    path: "my-videos",
    //canActivate: [JMediaLazyGuard],
    loadChildren: () => import('./account/videos/videos.module').then(m => m.VideosModule)
    // loadChildren: "./account/videos/videos.module#VideosModule"
  },
  {
    path: "setup",
    loadChildren: () => import('./setup/index/index.module').then(m => m.SetupModule)
    // loadChildren: "./setup/index/index.module#SetupModule"
  },
  {
    path: "",
    loadChildren: () => import('./admin/dashboard/dashboard.module').then(m => m.DashboardModule)
    // loadChildren: "./admin/dashboard/dashboard.module#DashboardModule"
  },
  {
    path: "**",
    loadChildren: () => import('./pages/notfound/notfound.module').then(m => m.NotFoundModule)
    // loadChildren: "./pages/notfound/notfound.module#NotFoundModule"
  }

  
];
