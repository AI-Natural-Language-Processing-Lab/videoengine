/* -------------------------------------------------------------------------- */
/*                          Product Name: VideoEngine                         */
/*                            Author: Mediasoftpro                            */
/*                       Email: support@mediasoftpro.com                      */
/*       License: Read license.txt located on root of your application.       */
/*                     Copyright 2007 - 2020 @Mediasoftpro                    */
/* -------------------------------------------------------------------------- */

import { Injectable } from "@angular/core";

import * as Controls from "../../../../partials/components/dynamicform/model/elements";
import { FormBase } from "../../../../partials/components/dynamicform/model/base";
import { NormalRegex } from "../../../../configs/settings";

@Injectable()
export class FormService {
  getControls(entity: any, prop: string, child_prop: string, iswizard = false) {
    switch (prop) {
      case "general":
        switch (child_prop) {
          case "dbsetup":
            return this.prepareDatabaseSettingControls(entity);
          case "dbusersetup":
            return this.prepareDatabaseUserSettingControls(entity);
          case "general":
            return this.prepareGeneralSettingControls(entity, iswizard);
          case "premium":
            return this.preparePremiumSettingControls(entity, iswizard);
          case "media":
            return this.prepareMediaSettingControls(entity, iswizard);
          case "features":
            return this.prepareFeatureSettingControls(entity, iswizard);
          case "listings":
            return this.prepareListingSettingControls(entity, iswizard);
          case "authentication":
            return this.prepareAuthenticationSettingControls(entity, iswizard);
          case "registration":
            return this.prepareRegistrationSettingControls(entity, iswizard);
          case "aws":
            return this.prepareAWSSettingControls(entity, iswizard);
          case "social":
            return this.prepareSocialSettingControls(entity, iswizard);
          case "contact":
            return this.prepareContactSettingControls(entity, iswizard);
          case "smtp":
            return this.prepareSmtpSettingControls(entity, iswizard);
          case "comment":
            return this.prepareCommentSettingControls(entity, iswizard);
          case "rechapcha":
            return this.prepareRechapchaSettingControls(entity, iswizard);
          default:
            return [];
        }
        break;
      case "videos":
        switch (child_prop) {
          case "general":
            return this.prepareVideosGeneralControls(entity);
          case "aws":
            return this.prepareVideosAWSControls(entity);
          case "ffmpeg":
            return this.prepareVideosFFMPEGControls(entity);
          case "youtube":
            return this.prepareVideosYoutubeControls(entity);
          case "direct":
            return this.prepareVideosDirectControls(entity);
          case "movie":
            return this.prepareVideosMovieControls(entity);
          case "player":
            return this.prepareVideosPlayerControls(entity);
          default:
            return [];
        }
        break;
      default:
        return [];
    }
  }

  prepareDatabaseSettingControls(entity: any) {
    const controls: FormBase<any>[] = [];

    controls.push(
      new Controls.Textbox({
        key: "host",
        label: "Host Name",
        value: entity.host,
        colsize: "col-md-12",
        required: true,
        helpblock: "Database host url with port e.g example.com,1433",
        order: 0
      })
    );

    controls.push(
      new Controls.Textbox({
        key: "database",
        label: "Database Name",
        value: entity.database,
        colsize: "col-md-12",
        required: true,
        helpblock: "",
        order: 2
      })
    );

    controls.push(
      new Controls.Textbox({
        key: "userid",
        label: "User Name",
        value: entity.userid,
        colsize: "col-md-12",
        required: true,
        helpblock: "",
        order: 3
      })
    );

    controls.push(
      new Controls.Textbox({
        key: "password",
        label: "Password",
        type: "password",
        minLength: 5,
        maxLength: 20,
        required: true,
        pattern: NormalRegex.PASSWORD_REGEX,
        colsize: "col-md-12",
        value: entity.password,
        helpblock: "",
        order: 4
      })
    );

    return controls.sort((a, b) => a.order - b.order);
  }

  prepareDatabaseUserSettingControls(entity: any) {
    const controls: FormBase<any>[] = [];

    controls.push(
      new Controls.Textbox({
        key: "username",
        label: "User Name",
        value: entity.username,
        colsize: "col-md-12",
        required: true,
        helpblock: "",
        order: 0
      })
    );

    controls.push(
      new Controls.Textbox({
        key: "email",
        label: "Email Address",
        value: entity.email,
        colsize: "col-md-12",
        required: true,
        email: true,
        order: 1
      })
    );

    controls.push(
      new Controls.Textbox({
        key: "firstname",
        label: "First Name",
        value: entity.firstname,
        colsize: "col-md-12",
        helpblock: "",
        order: 2
      })
    );

    controls.push(
      new Controls.Textbox({
        key: "lastname",
        label: "Last Name",
        value: entity.lastname,
        colsize: "col-md-12",
        helpblock: "",
        order: 3
      })
    );

    controls.push(
      new Controls.Textbox({
        key: "password",
        label: "Password",
        type: "password",
        value: entity.password,
        minLength: 5,
        maxLength: 20,
        required: true,
        pattern: NormalRegex.PASSWORD_REGEX,
        colsize: "col-md-12",
        helpblock: "",
        order: 4
      })
    );

    return controls.sort((a, b) => a.order - b.order);
  }

  prepareGeneralSettingControls(entity: any, iswizard: boolean) {
    const controls: FormBase<any>[] = [];

    controls.push(
      new Controls.Dropdown({
        key: "site_theme",
        label: "Site Theme",
        required: true,
        value: entity.site_theme,
        options: [
          {
            key: "cerulean",
            value: "Cerulean"
          },
          {
            key: "cosmo",
            value: "Cosmo"
          },
          {
            key: "cyborg",
            value: "Cyborg"
          },
          {
            key: "darkly",
            value: "Darkly"
          },
          {
            key: "flatly",
            value: "Flatly"
          },
          {
            key: "journal",
            value: "Journal"
          },
          {
            key: "litera",
            value: "Litera"
          },
          {
            key: "lumen",
            value: "Lumen"
          },
          {
            key: "materia",
            value: "Materia"
          },
          {
            key: "minty",
            value: "Minty"
          },
          {
            key: "pulse",
            value: "Pulse"
          },
          {
            key: "sandstone",
            value: "Sandstone"
          },
          {
            key: "simplex",
            value: "Simplex"
          },
          {
            key: "sketchy",
            value: "Sketchy"
          },
          {
            key: "slate",
            value: "Slate"
          },
          {
            key: "solar",
            value: "Solar"
          },
          {
            key: "spacelab",
            value: "Space Lab"
          },
          {
            key: "superhero",
            value: "Super Hero"
          },
          {
            key: "united",
            value: "United"
          },
          {
            key: "yeti",
            value: "Yeti"
          }
        ],
        helpblock:
          "Choose theme name from list of available themes (bootswatch) unless you used your own custom theme / template",
        order: 0
      })
    );

    controls.push(
      new Controls.Textbox({
        key: "jwt_private_key",
        label: "JWT Private Key",
        value: entity.jwt_private_key,
        colsize: "col-md-12",
        helpblock:
          "Setup / Configure JWT Private Key, check https://mkjwk.org/",
        order: 1
      })
    );
    
    controls.push(
      new Controls.Textbox({
        key: "website_title",
        label: "Website Title",
        value: entity.website_title,
        colsize: "col-md-12",
        helpblock: "Website title used within whole web application",
        order: 1
      })
    );

    if (!iswizard) {
      controls.push(
        new Controls.Textbox({
          key: "website_description",
          label: "Website Description",
          value: entity.website_description,
          colsize: "col-md-12",
          helpblock:
            "General website description or slogan to be used in some part of application",
          order: 2
        })
      );
    }

    controls.push(
      new Controls.Textbox({
        key: "page_caption",
        label: "Page Caption",
        value: entity.page_caption,
        colsize: "col-md-12",
        helpblock:
          "Append title with each page title on dynamic pages e.g [Page Title] | [Page Caption]",
        order: 3
      })
    );

    if (!iswizard) {
      controls.push(
        new Controls.Textbox({
          key: "website_phone",
          label: "Website Phone",
          value: entity.website_phone,
          colsize: "col-md-12",
          helpblock:
            "Phone number used within website for support purpose unless you customize it manually.",
          order: 5
        })
      );
    }

    controls.push(
      new Controls.Textbox({
        key: "admin_mail",
        label: "Admin Email Address",
        value: entity.admin_mail,
        colsize: "col-md-12",
        helpblock:
          "Admin support email used for sending mails and other support purposes within website.",
        order: 6
      })
    );

    controls.push(
      new Controls.Textbox({
        key: "admin_mail_name",
        label: "Admin Email Name",
        value: entity.admin_mail_name,
        colsize: "col-md-12",
        helpblock:
          "Email name used as sender name when sending email to users from website.",
        order: 7
      })
    );

    if (!iswizard) {

      controls.push(
        new Controls.Textbox({
          key: "pagination_links",
          label: "Pagination Links",
          value: entity.pagination_links.toString(),
          colsize: "col-md-6",
          helpblock: "Maxiumum pagination links to be used, 0 for unlimited",
          order: 8
        })
      );
    }

    controls.push(
      new Controls.Dropdown({
        key: "pagination_type",
        label: "Pagination Type",
        required: true,
        value: entity.pagination_type.toString(),
        options: [
          {
            key: 0,
            value: "Normal"
          },
          {
            key: 1,
            value: "Advance"
          },
          {
            key: 2,
            value: "Simple"
          }
        ],
        order: 9
      })
    );
    
    if (!iswizard) {
      controls.push(
        new Controls.Dropdown({
          key: "screen_content",
          label: "Screen Content",
          required: true,
          value: entity.screen_content.toString(),
          options: [
            {
              key: 0,
              value: "Screen"
            },
            {
              key: 1,
              value: "Screen & Replace"
            },
            {
              key: 2,
              value: "None"
            }
          ],
          helpblock:
            "Use type of content screening approach to scan and screen words matched with black listed dictionary words (0: screen, 1: screen & replace 2: None)",
          order: 10
        })
      );
    }

    controls.push(
      new Controls.Dropdown({
        key: "content_approval",
        label: "Content Approval",
        required: true,
        value: entity.content_approval.toString(),
        options: [
          {
            key: 1,
            value: "Automatic Approval"
          },
          {
            key: 0,
            value: "Need Moderator Review"
          }
        ],
        helpblock:
          "Set option to review user posted contents 1: Automatical approval, 0: Need moderator review before approval",
        order: 11
      })
    );

    if (!iswizard) {
      controls.push(
        new Controls.Textbox({
          key: "spam_count",
          label: "Maximum Report Count",
          value: entity.spam_count.toString(),
          colsize: "col-md-12",
          helpblock:
            "Maximum number of reports count before forcing content to block until review.",
          order: 11
        })
      );
    }
  
    controls.push(
      new Controls.Textbox({
        key: "cache_duration",
        label: "Cache Duration",
        value: entity.cache_duration.toString(),
        colsize: "col-md-12",
        pattern: "[0-9]+",
        required: true,
        helpblock:
          "Set duration in seconds to cache content if enabled cache for. 0 for disabled cache.",
        order: 12
      })
    );

   
    controls.push(
      new Controls.Dropdown({
        key: "default_culture",
        label: "Default Culture",
        required: true,
        value: entity.default_culture.toString(),
        options: [
          {
            key: "en-US",
            value: "en-US"
          },
          {
            key: "ar-SA",
            value: "ar-SA"
          },
          {
            key: "de-DE",
            value: "de-DE"
          },
          {
            key: "es-ES",
            value: "es-ES"
          },
          {
            key: "fr-FR",
            value: "fr-FR"
          },
          {
            key: "it-IT",
            value: "it-IT"
          },
          {
            key: "pt-BR",
            value: "pt-BR"
          },
          {
            key: "ru-RU",
            value: "ru-RU"
          },
          {
            key: "tr-TR",
            value: "tr-TR"
          },
          {
            key: "ja-JP",
            value: "ja-JP"
          },
          {
            key: "zh-CHS",
            value: "zh-CHS"
          }
        ],
        helpblock:
          "Set default culture for your website if language functionality enabled or available",
        order: 17
      })
    );
 
    controls.push(
      new Controls.CheckBox({
        key: "store_searches",
        label: "Store Searches",
        value: entity.store_searches,
        checked: entity.store_searches,
        helpblock:
          "Store user types searches for creating auto search labels, auto suggestions and other internal purposes",
        order: 14
      })
    );

    if (!iswizard) {

      controls.push(
        new Controls.Textbox({
          key: "max_cache_pages",
          label: "Maximum Pages to Cache",
          value: entity.max_cache_pages.toString(),
          colsize: "col-md-12",
          pattern: "[0-9]+",
          required: true,
          helpblock:
            "Maximum no of pages to be cached for each type of listing within pagination. 0 for unlimited",
          order: 13
        })
      );

     

      controls.push(
        new Controls.CheckBox({
          key: "store_ipaddress",
          label: "Store Ip Address",
          value: entity.store_ipaddress,
          checked: entity.store_ipaddress,
          helpblock: "Store ip addresses for security and internal use only",
          order: 15
        })
      );

      controls.push(
        new Controls.Textbox({
          key: "maximum_dynamic_link_length",
          label: "Maximum Characters in Dynamic Url",
          value: entity.maximum_dynamic_link_length.toString(),
          colsize: "col-md-12",
          pattern: "[0-9]+",
          required: true,
          helpblock:
            "Maximum characters to generate dynamic urls from content titles. 0 for unlimited",
          order: 16
        })
      );

      controls.push(
        new Controls.Textbox({
          key: "pagesize",
          label: "Page Size",
          value: entity.pagesize.toString(),
          colsize: "col-md-12",
          pattern: "[0-9]+",
          required: true,
          helpblock: "Set no of items display in each page size",
          order: 18
        })
      );

      controls.push(
        new Controls.Dropdown({
          key: "rating_option",
          label: "Rating Options",
          required: true,
          value: entity.rating_option.toString(),
          options: [
            {
              key: 0,
              value: "Like / Dislike"
            },
            {
              key: 1,
              value: "Five Star"
            },
            {
              key: 2,
              value: "Disabled"
            }
          ],
          helpblock:
            "Setup rating option for contents 0: five star, 1: like / dislike, 2: disable rating",
          order: 19
        })
      );

     
      
    }

    return controls.sort((a, b) => a.order - b.order);
  }

  preparePremiumSettingControls(entity: any, iswizard: boolean) {
    const controls: FormBase<any>[] = [];

    controls.push(
      new Controls.CheckBox({
        key: "enable_payment",
        label: "Enable Payment",
        value: entity.enable_payment,
        checked: entity.enable_payment,
        helpblock: "Enable payment functionality within your application ",
        order: 0
      })
    );

    controls.push(
      new Controls.Dropdown({
        key: "premium_option",
        label: "Premium Options",
        required: true,
        value: entity.premium_option.toString(),
        options: [
          {
            key: 0,
            value: "Credit Based"
          },
          {
            key: 1,
            value: "Monthly Subscription"
          }
        ],
        helpblock: "Enable premium options from available modules",
        order: 1
      })
    );

    controls.push(
      new Controls.Dropdown({
        key: "premium_redirect_option",
        label: "Premium Redirect Option",
        required: true,
        value: entity.premium_redirect_option.toString(),
        options: [
          {
            key: 0,
            value: "Redirect to Account Page"
          },
          {
            key: 1,
            value: "Redirect to Purchase Page"
          }
        ],
        helpblock:
          "Enable redirection option after signup process unless you customize with your own way.",
        order: 2
      })
    );

    if (!iswizard) {
      controls.push(
        new Controls.CheckBox({
          key: "enable_paypal",
          label: "Enable Paypal",
          value: entity.enable_paypal,
          checked: entity.enable_paypal,
          helpblock: "Enable paypal as payment gateway.",
          order: 3
        })
      );

      controls.push(
        new Controls.Textbox({
          key: "paypal_receiver_email",
          label: "Receiver Email Address",
          value: entity.paypal_receiver_email,
          colsize: "col-md-12",
          helpblock: "Website title used within whole web application",
          order: 4
        })
      );

      controls.push(
        new Controls.CheckBox({
          key: "sandbox_mode",
          label: "Sandbox Mode",
          value: entity.sandbox_mode,
          checked: entity.sandbox_mode,
          helpblock: "Toggle on | off sandbox mode",
          order: 5
        })
      );
    }

    return controls.sort((a, b) => a.order - b.order);
  }

  prepareMediaSettingControls(entity: any, iswizard: boolean) {
    const controls: FormBase<any>[] = [];

    controls.push(
      new Controls.Textbox({
        key: "user_thumbnail_width",
        label: "User Thumbnail Width",
        value: entity.user_thumbnail_width.toString(),
        colsize: "col-md-12",
        pattern: "[0-9]+",
        required: true,
        helpblock: "Set thumbnail width for user avators.",
        order: 0
      })
    );

    controls.push(
      new Controls.Textbox({
        key: "user_thumbnail_height",
        label: "User Thumbnail Height",
        value: entity.user_thumbnail_height.toString(),
        colsize: "col-md-12",
        pattern: "[0-9]+",
        required: true,
        helpblock: "Set thumbnail height for user avators",
        order: 1
      })
    );

    controls.push(
      new Controls.Textbox({
        key: "category_thumbnail_width",
        label: "Category Thumbnail Width",
        value: entity.category_thumbnail_width.toString(),
        colsize: "col-md-12",
        pattern: "[0-9]+",
        required: true,
        helpblock: "Set category image thumbnail width",
        order: 2
      })
    );

    controls.push(
      new Controls.Textbox({
        key: "category_thumbnail_height",
        label: "Category Thumbnail Height",
        value: entity.category_thumbnail_height.toString(),
        colsize: "col-md-12",
        pattern: "[0-9]+",
        required: true,
        helpblock: "Setup category image thumbnail height",
        order: 3
      })
    );

    /*controls.push(
      new Controls.Textbox({
        key: "gamify_badge_width",
        label: "Gamify Badge Width",
        value: entity.gamify_badge_width,
        colsize: "col-md-12",
        pattern: "[0-9]+",
        required: true,
        helpblock:
          "Setup maximum width to be used by application for generating gamify badge",
        order: 4
      })
    );

    controls.push(
      new Controls.Textbox({
        key: "gamify_badge_height",
        label: "Gamify Badge Height",
        value: entity.gamify_badge_height,
        colsize: "col-md-12",
        pattern: "[0-9]+",
        required: true,
        helpblock:
          "Setup maximum height to be used by application for generating gamify badge",
        order: 5
      })
    );*/

    controls.push(
      new Controls.Textbox({
        key: "quality",
        label: "Quality",
        value: entity.quality.toString(),
        colsize: "col-md-12",
        required: true,
        pattern: "[0-9]+",
        helpblock:
          "Setup quality for generated thumbnails in percentage e.g 70",
        order: 6
      })
    );

    controls.push(
      new Controls.Textbox({
        key: "photo_extensions",
        label: "Allowed Photo Extensions",
        value: entity.photo_extensions,
        required: true,
        colsize: "col-md-12",
        helpblock: "Allowed photo extension to be uploaded to website",
        order: 7
      })
    );

    controls.push(
      new Controls.Textbox({
        key: "photo_max_size",
        label: "Max Photo Size (mb)",
        value: entity.photo_max_size.toString(),
        required: true,
        colsize: "col-md-6",
        helpblock: "Maximum allowed image size (in mb e.g 11mb)",
        order: 8
      })
    );

    controls.push(
      new Controls.Textbox({
        key: "logo_path",
        label: "Logo Path",
        value: entity.logo_path,
        colsize: "col-md-12",
        required: true,
        helpblock:
          "Setup default logo path either direct url or relative path within project.",
        order: 9
      })
    );

    controls.push(
      new Controls.Textbox({
        key: "user_default_path",
        label: "User Default Path",
        value: entity.user_default_path,
        colsize: "col-md-12",
        required: true,
        helpblock:
          "Setup default user avator path (to be used if user not updated its avator).",
        order: 10
      })
    );

    controls.push(
      new Controls.Textbox({
        key: "category_default_path",
        label: "Category Default Path",
        value: entity.category_default_path,
        colsize: "col-md-12",
        required: true,
        helpblock:
          "Setup default category image path (to be used if category have no photo)",
        order: 11
      })
    );

    /*controls.push(
      new Controls.Textbox({
        key: "gamify_default_path",
        label: "Gamify Default Path",
        value: entity.gamify_default_path,
        colsize: "col-md-12",
        required: true,
        helpblock:
          "Setup default gamify badge image path (to be used if badge have no image)",
        order: 12
      })
    );*/

    return controls.sort((a, b) => a.order - b.order);
  }

  prepareFeatureSettingControls(entity: any, iswizard: boolean) {
    const controls: FormBase<any>[] = [];

    controls.push(
      new Controls.CheckBox({
        key: "enable_videos",
        label: "Enable Videos",
        value: entity.enable_videos,
        checked: entity.enable_videos,
        helpblock:
          "Enable video functionality in application if module available.",
        order: 0
      })
    );
    
    controls.push(
      new Controls.SectionHeader({
        key: "config_section_01",
        label: "Inner Modules",
        order: 10
      })
    );

    controls.push(
      new Controls.CheckBox({
        key: "enable_categories",
        label: "Enable Categories",
        value: entity.enable_categories,
        checked: entity.enable_categories,
        helpblock: "Toggle on | off categorizing contents and listing",
        order: 11
      })
    );

    controls.push(
      new Controls.CheckBox({
        key: "enable_tags",
        label: "Enable Tags or Labels",
        value: entity.enable_tags,
        checked: entity.enable_tags,
        helpblock: "Toggle on | off labeling or tagging contents and listing",
        order: 12
      })
    );

    if (!iswizard) {
      controls.push(
        new Controls.CheckBox({
          key: "showLabelCounter",
          label: "Display Counter",
          value: entity.showLabelCounter,
          checked: entity.showLabelCounter,
          helpblock:
            "Display (total number of public records) counter with category or tag links",
          order: 13
        })
      );

      controls.push(
        new Controls.CheckBox({
          key: "enable_archives",
          label: "Enable Archives",
          value: entity.enable_archives,
          checked: entity.enable_archives,
          helpblock: " Toggle on | off archiving contents and listing",
          order: 13
        })
      );

      /*controls.push(
        new Controls.CheckBox({
          key: "enable_gamify",
          label: "Enable Gamify",
          value: entity.enable_gamify,
          checked: entity.enable_gamify,
          helpblock:
            "Enable gamify functionality in application if module available.",
          order: 14
        })
      );*/

      controls.push(
        new Controls.CheckBox({
          key: "enable_date_filter",
          label: "Enable Date Filter",
          value: entity.enable_date_filter,
          checked: entity.enable_date_filter,
          helpblock:
            "Toggle on | off group by contents based on today / this week / this month / all time date filters",
          order: 16
        })
      );

      controls.push(
        new Controls.CheckBox({
          key: "enable_advertisement",
          label: "Enable Advertisement",
          value: entity.enable_advertisement,
          checked: entity.enable_advertisement,
          helpblock: "Toggle on | off advertisement within application",
          order: 17
        })
      );

      controls.push(
        new Controls.CheckBox({
          key: "enable_adult_veritifcation",
          label: "Enable Adult Verification",
          value: entity.enable_adult_veritifcation,
          checked: entity.enable_adult_veritifcation,
          helpblock: "Toggle on | off adult content verification warning",
          order: 18
        })
      );

      controls.push(
        new Controls.CheckBox({
          key: "enable_adult_veritifcation",
          label: "Enable Adult Verification",
          value: entity.enable_languages,
          checked: entity.enable_languages,
          helpblock:
            "Toggle on | off enabling multiple language support within application",
          order: 19
        })
      );
    }

    return controls.sort((a, b) => a.order - b.order);
  }

  prepareListingSettingControls(entity: any, iswizard: boolean) {
    const controls: FormBase<any>[] = [];

    controls.push(
      new Controls.CheckBox({
        key: "enable_views",
        label: "Enable Views",
        value: entity.enable_views,
        checked: entity.enable_views,
        helpblock:
          "Toggle on | off views option in listing items unless you add customization manually.",
        order: 0
      })
    );

    controls.push(
      new Controls.CheckBox({
        key: "enable_date",
        label: "Enable Date",
        value: entity.enable_date,
        checked: entity.enable_date,
        helpblock:
          "Toggle on | off date option in listing items unless you add customization manually.",
        order: 1
      })
    );

    controls.push(
      new Controls.CheckBox({
        key: "enable_username",
        label: "Enable UserName",
        value: entity.enable_username,
        checked: entity.enable_username,
        helpblock:
          "Toggle on | off username option in listing items unless you add customization manually.",
        order: 2
      })
    );

    controls.push(
      new Controls.CheckBox({
        key: "enable_rating",
        label: "Enable Rating",
        value: entity.enable_rating,
        checked: entity.enable_rating,
        helpblock:
          "Toggle on | off like / dislike option in listing items unless you add customization manually.",
        order: 3
      })
    );

    controls.push(
      new Controls.CheckBox({
        key: "enable_likedislike",
        label: "Enable Like / Dislike",
        value: entity.enable_likedislike,
        checked: entity.enable_likedislike,
        helpblock:
          "Toggle on | off like / dislike option in listing items unless you add customization manually.",
        order: 4
      })
    );

    return controls.sort((a, b) => a.order - b.order);
  }

  prepareAuthenticationSettingControls(entity: any, iswizard: boolean) {
    const controls: FormBase<any>[] = [];

    controls.push(
      new Controls.CheckBox({
        key: "enable_facebook",
        label: "Enable Facebook",
        value: entity.enable_facebook,
        checked: entity.enable_facebook,
        helpblock:
          "Toggle on | of facebook as additional authentication provider",
        order: 0
      })
    );

    controls.push(
      new Controls.Textbox({
        key: "fb_appId",
        label: "Facebook App Id",
        value: entity.fb_appId,
        colsize: "col-md-12",
        helpblock: "If facebook enable, enter required Facebook App ID",
        order: 1
      })
    );
    controls.push(
      new Controls.Textbox({
        key: "fb_appSecrete",
        label: "Facebook App Secrete",
        value: entity.fb_appSecrete,
        colsize: "col-md-12",
        helpblock: "If facebook enable, enter required Facebook App Secrete",
        order: 2
      })
    );
    controls.push(
      new Controls.CheckBox({
        key: "enable_twitter",
        label: "Enable Twitter",
        value: entity.enable_twitter,
        checked: entity.enable_twitter,
        helpblock:
          "Toggle on | of twitter as additional authentication provider",
        order: 3
      })
    );

    controls.push(
      new Controls.Textbox({
        key: "tw_consumer_key",
        label: "Twitter Consumer Key",
        value: entity.tw_consumer_key,
        colsize: "col-md-12",
        helpblock:
          "If twitter enable, enter required twitter consumer key here",
        order: 4
      })
    );
    controls.push(
      new Controls.Textbox({
        key: "tw_consumer_secrete",
        label: "Facebook App Secrete",
        value: entity.tw_consumer_secrete,
        colsize: "col-md-12",
        helpblock:
          "If twitter enable, enter required twitter consumer secrete here",
        order: 5
      })
    );
    controls.push(
      new Controls.CheckBox({
        key: "enable_google",
        label: "Enable Google",
        value: entity.enable_google,
        checked: entity.enable_google,
        helpblock:
          "Toggle on | of google as additional authentication provider",
        order: 6
      })
    );

    controls.push(
      new Controls.Textbox({
        key: "google_clientid",
        label: "Google Client Id",
        value: entity.google_clientid,
        colsize: "col-md-12",
        helpblock: "If google enable, enter required google client id",
        order: 7
      })
    );
    controls.push(
      new Controls.Textbox({
        key: "google_clientsecrete",
        label: "Google Client Secrete",
        value: entity.google_clientsecrete,
        colsize: "col-md-12",
        helpblock:
          "If google enable, enter required  google client secrete here",
        order: 8
      })
    );
    return controls.sort((a, b) => a.order - b.order);
  }

  prepareRegistrationSettingControls(entity: any, iswizard: boolean) {
    const controls: FormBase<any>[] = [];

    controls.push(
      new Controls.CheckBox({
        key: "enable",
        label: "Enable Registration",
        value: entity.enable,
        checked: entity.enable,
        helpblock: "Toggle on | off registeration process within application",
        order: 0
      })
    );
    console.log('unique field options are ' + entity.uniqueFieldOption);
    controls.push(
      new Controls.Dropdown({
        key: "uniqueFieldOption",
        label: "Login Option",
        required: true,
        value: entity.uniqueFieldOption.toString(),
        options: [
          {
            key: 0,
            value: "Both UserName, Email"
          },
          {
            key: 1,
            value: "Email Address"
          }
        ],
        helpblock:
          "Choose your verification option for authentication, 0: Both UserName, Email, 1: Email Address",
        order: 4
      })
    );

    console.log('enable field options are ' + entity.enableNameField);
    controls.push(
      new Controls.CheckBox({
        key: "enableNameField",
        label: "Enable Name Fields",
        value: entity.enableNameField,
        checked: entity.enableNameField,
        helpblock: "Enable first name, last name fields on registeration form.",
        order: 5
      })
    );

    controls.push(
      new Controls.CheckBox({
        key: "enablePrivacyCheck",
        label: "Enable Privacy Check",
        value: entity.enablePrivacyCheck,
        checked: entity.enablePrivacyCheck,
        helpblock:
          " Toggle on | off showing enable privacy check option unless you customize registeration process manually",
        order: 6
      })
    );

    return controls.sort((a, b) => a.order - b.order);
  }
  
  prepareRechapchaSettingControls(entity: any, iswizard: boolean) {
    const controls: FormBase<any>[] = [];

    controls.push(
      new Controls.Textbox({
        key: "SiteKey",
        label: "Site Key",
        value: entity.SiteKey,
        colsize: "col-md-12",
        helpblock:
          "Rechapcha Authorization Site Key",
        order: 0
      })
    );

    controls.push(
      new Controls.Textbox({
        key: "SecretKey",
        label: "Site Secrete",
        value: entity.SecretKey,
        colsize: "col-md-12",
        helpblock:
          "Rechapcha Authorization Site Secrete",
        order: 0
      })
    );

    controls.push(
      new Controls.Dropdown({
        key: "Version",
        label: "Version",
        required: true,
        value: entity.version.toString(),
        options: [
          {
            key: 'v2',
            value: "V2"
          },
          {
            key: 'v3',
            value: "V3"
          }
        ],
        order: 3
      })
    );

   

    return controls.sort((a, b) => a.order - b.order);
  }

  prepareAWSSettingControls(entity: any, iswizard: boolean) {
    const controls: FormBase<any>[] = [];

    controls.push(
      new Controls.CheckBox({
        key: "enable",
        label: "Enable AWS",
        value: entity.enable,
        checked: entity.enable,
        helpblock:
          " Toggle on | off aws cloud for storage, hosting and other purposes",
        order: 0
      })
    );

    controls.push(
      new Controls.Textbox({
        key: "accessKey",
        label: "Access Key",
        value: entity.accessKey,
        colsize: "col-md-12",
        helpblock: "AWS Access Key required for verification purpose",
        order: 1
      })
    );

    controls.push(
      new Controls.Textbox({
        key: "secretKey",
        label: "Secrete Key",
        value: entity.secretKey,
        colsize: "col-md-12",
        helpblock: "AWS Secrete Key required for verification purpose",
        order: 2
      })
    );

    controls.push(
      new Controls.Textbox({
        key: "region",
        label: "Region",
        value: entity.region,
        colsize: "col-md-12",
        helpblock:
          "AWS Region (Preferred geolocial location for your content to be stored and stream)",
        order: 3
      })
    );

    controls.push(
      new Controls.Textbox({
        key: "bucket",
        label: "Bucket Name",
        value: entity.bucket,
        colsize: "col-md-12",
        helpblock:
          "Setup bucketname for saving general media files e.g users or categories etc media",
        order: 4
      })
    );

    controls.push(
      new Controls.Textbox({
        key: "user_photos_directory",
        label: "User Photos Directory",
        value: entity.user_photos_directory,
        colsize: "col-md-12",
        helpblock:
          'Setup directory  (within bucket)  for saving users avator photos e.g "photos/"',
        order: 5
      })
    );

    controls.push(
      new Controls.Textbox({
        key: "category_photos_directory",
        label: "Category Photos Directory",
        value: entity.category_photos_directory,
        colsize: "col-md-12",
        helpblock:
          'Setup directory  (within bucket)  for saving category photos e.g "category/"',
        order: 6
      })
    );

    controls.push(
      new Controls.Textbox({
        key: "gamify_badges_directory",
        label: "Gamify Badges Directory",
        value: entity.gamify_badges_directory,
        colsize: "col-md-12",
        helpblock:
          'Setup directory (within bucket) for saving gamify badges e.g "badges/"',
        order: 7
      })
    );

    controls.push(
      new Controls.Textbox({
        key: "cdn_URL",
        label: "CDN URL (Cloud-Front)",
        value: entity.cdn_URL,
        colsize: "col-md-12",
        helpblock:
          "Setup public accessible cloudfront distribution url for streaming photos (categories, users, gamify etc)",
        order: 8
      })
    );

    return controls.sort((a, b) => a.order - b.order);
  }

  prepareSocialSettingControls(entity: any, iswizard: boolean) {
    const controls: FormBase<any>[] = [];

    controls.push(
      new Controls.Textbox({
        key: "facebook_url",
        label: "Facebook Url",
        value: entity.facebook_url,
        colsize: "col-md-12",
        helpblock: "Setup facebook url to be appear in website",
        order: 0
      })
    );

    controls.push(
      new Controls.Textbox({
        key: "twitter_url",
        label: "Twitter Url",
        value: entity.twitter_url,
        colsize: "col-md-12",
        helpblock: "Setup twitter url to be appear in website",
        order: 1
      })
    );

    controls.push(
      new Controls.Textbox({
        key: "flickr_url",
        label: "Flickr Url",
        value: entity.flickr_url,
        colsize: "col-md-12",
        helpblock: "Setup flickr url to be appear in website",
        order: 2
      })
    );

    controls.push(
      new Controls.Textbox({
        key: "linkedin_url",
        label: "Linkedin Url",
        value: entity.linkedin_url,
        colsize: "col-md-12",
        helpblock: "Setup linkedin url to be appear in website",
        order: 3
      })
    );

    controls.push(
      new Controls.Textbox({
        key: "thumblr_url",
        label: "Thumblr Url",
        value: entity.thumblr_url,
        colsize: "col-md-12",
        helpblock: "Setup thumblr url to be appear in website",
        order: 4
      })
    );

    controls.push(
      new Controls.Textbox({
        key: "google_url",
        label: "Google Url",
        value: entity.google_url,
        colsize: "col-md-12",
        helpblock: "Setup google url to be appear in website",
        order: 5
      })
    );

    controls.push(
      new Controls.Textbox({
        key: "youtube_url",
        label: "Youtube Url",
        value: entity.youtube_url,
        colsize: "col-md-12",
        helpblock: "Setup youtube url to be appear in website",
        order: 6
      })
    );

    controls.push(
      new Controls.Textbox({
        key: "vimeo_url",
        label: "Vimeo Url",
        value: entity.vimeo_url,
        colsize: "col-md-12",
        helpblock: "Setup vimeo url to be appear in website",
        order: 7
      })
    );

    controls.push(
      new Controls.Textbox({
        key: "pinterest_url",
        label: "Pinterest Url",
        value: entity.pinterest_url,
        colsize: "col-md-12",
        helpblock: "Setup pinterest url to be appear in website",
        order: 8
      })
    );

    controls.push(
      new Controls.Textbox({
        key: "instagram_url",
        label: "Instagram Url",
        value: entity.instagram_url,
        colsize: "col-md-12",
        helpblock: "Setup instagram url to be appear in website",
        order: 9
      })
    );

    controls.push(
      new Controls.Textbox({
        key: "github_url",
        label: "GitHub Url",
        value: entity.github_url,
        colsize: "col-md-12",
        helpblock: "Setup github url to be appear in website",
        order: 10
      })
    );

    controls.push(
      new Controls.Textbox({
        key: "rss_url",
        label: "RSS Url",
        value: entity.rss_url,
        colsize: "col-md-12",
        helpblock: "Setup rss url to be appear in website",
        order: 11
      })
    );

    controls.push(
      new Controls.SectionHeader({
        key: "config_section_01",
        label: "Content Sharing Third Party Plugin Settings",
        order: 12
      })
    );

    controls.push(
      new Controls.Textbox({
        key: "addthis_pubid",
        label: "Addthis PubId",
        value: entity.addthis_pubid,
        colsize: "col-md-12",
        helpblock:
          "Enable addthis plugin for sharing content option for more information visit: https://www.addthis.com/",
        order: 13
      })
    );

    controls.push(
      new Controls.Textbox({
        key: "sharethis_propertyId",
        label: "ShareThis PropertyId",
        value: entity.sharethis_propertyId,
        colsize: "col-md-12",
        helpblock:
          "Enable sharethis property id for sharing content option for more information visit: https://sharethis.com/",
        order: 14
      })
    );

    controls.push(
      new Controls.Textbox({
        key: "fb_appId",
        label: "Facebook AppId",
        value: entity.sharethis_propertyId,
        colsize: "col-md-12",
        helpblock:
          "This will include Facebook Javascript SDK, that can be used with variety of facebook apps including facebook comments, like box and others",
        order: 15
      })
    );
    return controls.sort((a, b) => a.order - b.order);
  }

  prepareContactSettingControls(entity: any, iswizard: boolean) {
    const controls: FormBase<any>[] = [];

    controls.push(
      new Controls.Textbox({
        key: "address",
        label: "Address",
        value: entity.address,
        colsize: "col-md-12",
        helpblock:
          "Setup address information to be used in contact information",
        order: 0
      })
    );

    controls.push(
      new Controls.Textbox({
        key: "tel1",
        label: "Telephone 1",
        value: entity.tel1,
        colsize: "col-md-12",
        helpblock:
          "Setup telephone information to be used in contact information",
        order: 1
      })
    );

    controls.push(
      new Controls.Textbox({
        key: "tel2",
        label: "Telephone 2",
        value: entity.tel2,
        colsize: "col-md-12",
        helpblock:
          "Setup telephone information (alternative) to be used in contact information",
        order: 2
      })
    );

    controls.push(
      new Controls.Textbox({
        key: "fax",
        label: "Fax",
        value: entity.fax,
        colsize: "col-md-12",
        helpblock: "Setup email information to be used in contact information",
        order: 3
      })
    );

    controls.push(
      new Controls.Textbox({
        key: "email",
        label: "Email",
        value: entity.email,
        colsize: "col-md-12",
        helpblock: "Setup email information to be used in contact information",
        order: 4
      })
    );

    controls.push(
      new Controls.TextArea({
        key: "detail_info",
        label: "Detail Information",
        value: entity.detail_info,
        colsize: "col-md-12",
        helpblock:
          "Setup detail information for contact section unless you customize this part manually",
        order: 5
      })
    );

    controls.push(
      new Controls.CheckBox({
        key: "enable_contact_form",
        label: "Enable Contact Form",
        value: entity.enable_contact_form,
        checked: entity.enable_contact_form,
        helpblock: "Toggle on | off contact form",
        order: 6
      })
    );

    return controls.sort((a, b) => a.order - b.order);
  }

  prepareSmtpSettingControls(entity: any, iswizard: boolean) {
    const controls: FormBase<any>[] = [];

    controls.push(
      new Controls.CheckBox({
        key: "enable_email",
        label: "Enable Email",
        value: entity.enable_email,
        checked: entity.enable_email,
        helpblock: "Toggle on | off email functionality within website",
        order: 0
      })
    );

    controls.push(
      new Controls.CheckBox({
        key: "enable_mandril",
        label: "Enable Mandril",
        value: entity.enable_mandril,
        checked: entity.enable_mandril,
        helpblock: "Enable mandril as email sending option",
        order: 1
      })
    );

    controls.push(
      new Controls.Textbox({
        key: "mandril_key",
        label: "Mandril Key",
        value: entity.mandril_key,
        colsize: "col-md-12",
        helpblock:
          "If mandril option enable, mandril key will be required to continue",
        order: 2
      })
    );

    controls.push(
      new Controls.CheckBox({
        key: "enable_SES",
        label: "Enable SES",
        value: entity.enable_SES,
        checked: entity.enable_SES,
        helpblock: "Enable AWS SES as core smtp option",
        order: 3
      })
    );

    controls.push(
      new Controls.Textbox({
        key: "ses_host",
        label: "SES Host",
        value: entity.ses_host,
        colsize: "col-md-12",
        helpblock: "If SES enable, host will be needed.",
        order: 4
      })
    );

    controls.push(
      new Controls.Textbox({
        key: "ses_username",
        label: "SES User Name",
        value: entity.ses_username,
        colsize: "col-md-12",
        helpblock: "AWS SES - UserName",
        order: 5
      })
    );

    controls.push(
      new Controls.Textbox({
        key: "ses_password",
        label: "SES Password",
        value: entity.ses_password,
        colsize: "col-md-12",
        helpblock: "AWS SES - Password",
        order: 6
      })
    );

    controls.push(
      new Controls.SectionHeader({
        key: "config_section_01",
        label: "Basic SMTP Settings",
        order: 7
      })
    );

    controls.push(
      new Controls.Textbox({
        key: "server",
        label: "Server",
        value: entity.server,
        colsize: "col-md-12",
        helpblock: "General SMTP Option- Server",
        order: 8
      })
    );

    controls.push(
      new Controls.Textbox({
        key: "port",
        label: "Port",
        value: entity.port,
        colsize: "col-md-12",
        helpblock: "General SMTP Option- Port",
        order: 9
      })
    );

    controls.push(
      new Controls.Textbox({
        key: "fromAddress",
        label: "From Address",
        value: entity.fromAddress,
        colsize: "col-md-12",
        helpblock: " General SMTP Option- From Email Address",
        order: 10
      })
    );

    return controls.sort((a, b) => a.order - b.order);
  }

  prepareCommentSettingControls(entity: any, iswizard: boolean) {
    const controls: FormBase<any>[] = [];

    controls.push(
      new Controls.CheckBox({
        key: "enable",
        label: "Enable Comments",
        value: entity.enable,
        checked: entity.enable,
        helpblock: "Toggle on | off email functionality within website",
        order: 0
      })
    );

    controls.push(
      new Controls.Dropdown({
        key: "comment_option",
        label: "Comment Options",
        required: true,
        value: entity.comment_option.toString(),
        options: [
          {
            key: 0,
            value: "Website Own"
          },
          {
            key: 1,
            value: "Disqus"
          },
          {
            key: 2,
            value: "Facebook Comments"
          }
        ],
        order: 1
      })
    );

    controls.push(
      new Controls.Textbox({
        key: "discus_src",
        label: "Disqus Url",
        value: entity.discus_src,
        colsize: "col-md-12",
        helpblock:
          "If disqus option enabled, please put your discus src your setup via Disqus Dashboard",
        order: 2
      })
    );

    return controls.sort((a, b) => a.order - b.order);
  }


  prepareVideosGeneralControls(entity: any) {
    const controls: FormBase<any>[] = [];

    controls.push(
      new Controls.Textbox({
        key: "extensions",
        label: "Allowed Extensions",
        value: entity.extensions,
        required: true,
        colsize: "col-md-12",
        helpblock: "Allowed videos extension to be uploaded to website",
        order: 0
      })
    );

    controls.push(
      new Controls.Textbox({
        key: "max_size",
        label: "Max Size (mb)",
        value: entity.max_size.toString(),
        required: true,
        colsize: "col-md-6",
        helpblock: "Maximum allowed video size (in mb e.g 1000mb)",
        order: 1
      })
    );

    controls.push(
      new Controls.CheckBox({
        key: "delete_original",
        label: "Delete Original Video",
        value: entity.delete_original,
        checked: entity.delete_original,
        helpblock:
          "Toggle on | off option to delete original video after completing publishing",
        order: 2
      })
    );

    controls.push(
      new Controls.Dropdown({
        key: "videoUploader_Type",
        label: "Uploader Type",
        required: true,
        value: entity.videoUploader_Type.toString(),
        options: [
          {
            key: 0,
            value: "FFMPEG"
          },
          {
            key: 1,
            value: "Youtube"
          },
          {
            key: 2,
            value: "Direct"
          },
          {
            key: 3,
            value: "AWS"
          }
        ],
        helpblock:
          "Enable type of uploading option if available for users (0: ffmpeg, 1: youtube, 2: direct, 3: AWS)",
        order: 3
      })
    );

    controls.push(
      new Controls.CheckBox({
        key: "enable_public_uploads",
        label: "Enable Uploads",
        value: entity.enable_public_uploads,
        checked: entity.enable_public_uploads,
        helpblock:
          "Enable normal users to upload videos from his / her own account or restrict this functionality to admin only",
        order: 4
      })
    );

    controls.push(
      new Controls.CheckBox({
        key: "enable_playlists",
        label: "Enable Playlists",
        value: entity.enable_playlists,
        checked: entity.enable_playlists,
        helpblock:
          "Toggle on | off video playlist functionality if module available",
        order: 5
      })
    );

    controls.push(
      new Controls.CheckBox({
        key: "enable_download",
        label: "Enable Downloads",
        value: entity.enable_download,
        checked: entity.enable_download,
        helpblock:
          "Toggle on | off video download functionality if module available",
        order: 6
      })
    );

    controls.push(
      new Controls.CheckBox({
        key: "enable_favorites",
        label: "Enable Favorites",
        value: entity.enable_favorites,
        checked: entity.enable_favorites,
        helpblock: "Toggle on | off add to favorite functionality",
        order: 7
      })
    );

    controls.push(
      new Controls.Dropdown({
        key: "thumbnail_rotator_option",
        label: "Thumbnail Rotator Option",
        required: true,
        value: entity.thumbnail_rotator_option.toString(),
        options: [
          {
            key: 1,
            value: "No Rotator"
          },
          {
            key: 0,
            value: "Thumbs Rotator"
          },
          {
            key: 2,
            value: "Video Preview Rotator"
          }
        ],
        helpblock:
          "Toggle on | off thumbnail rotator functionality ( 1: No Rotator, 0: Thumbs Rotator, 2: Video Preview Rotator)",
        order: 8
      })
    );

    controls.push(
      new Controls.Textbox({
        key: "default_path",
        label: "Default Path",
        value: entity.default_path,
        required: true,
        colsize: "col-md-6",
        helpblock:
          "Setup default picture path that will be used as default image or video cover in listings if no photo available",
        order: 9
      })
    );

    controls.push(
      new Controls.SectionHeader({
        key: "config_section_01",
        label: "Media Settings",
        order: 10
      })
    );

    controls.push(
      new Controls.Textbox({
        key: "thumbnail_width",
        label: "Thumbnail Width",
        value: entity.thumbnail_width.toString(),
        required: true,
        colsize: "col-md-6",
        helpblock:
          "Width of thumbnail image generate from video or via direct cover upload",
        order: 11
      })
    );

    controls.push(
      new Controls.Textbox({
        key: "thumbnail_height",
        label: "Thumbnail Height",
        value: entity.thumbnail_height.toString(),
        required: true,
        colsize: "col-md-6",
        helpblock:
          "Height of thumbnail image generate from video or via direct cover upload",
        order: 12
      })
    );

    return controls.sort((a, b) => a.order - b.order);
  }

  prepareVideosAWSControls(entity: any) {

    const controls: FormBase<any>[] = [];
    controls.push(
      new Controls.CheckBox({
        key: "enable",
        label: "Enable (Account)",
        value: entity.enable,
        checked: entity.enable,
        helpblock: "Toggle on | off aws functionality for videos",
        order: 0
      })
    );

    controls.push(
      new Controls.CheckBox({
        key: "enable_admin",
        label: "Enable (Control Panel)",
        value: entity.enable_admin,
        checked: entity.enable_admin,
        helpblock: "Toggle on | off aws functionality for administrators",
        order: 1
      })
    );

    controls.push(
      new Controls.Textbox({
        key: "bucket",
        label: "Bucket Name",
        value: entity.bucket,
        colsize: "col-md-12",
        helpblock: "Setup bucketname for storing &amp; streaming videos etc",
        order: 1
      })
    );

    controls.push(
      new Controls.Textbox({
        key: "source_directory_path",
        label: "Source Directory Directory",
        value: entity.source_directory_path,
        colsize: "col-md-12",
        helpblock:
          "Setup directory for saving uploaded videos e.g videos/. It will be mostly used for directory where no aws events attached like Elastic Transcoder for auto publishing",
        order: 2
      })
    );

    controls.push(
      new Controls.Textbox({
        key: "publish_directory_path",
        label: "Published Video Directory",
        value: entity.publish_directory_path,
        colsize: "col-md-12",
        helpblock:
          "Setup directory (within bucket) for saving published videos e.g published/",
        order: 3
      })
    );

    controls.push(
      new Controls.Textbox({
        key: "thumbnail_directory_path",
        label: "Video Thumbnail Directory",
        value: entity.thumbnail_directory_path,
        colsize: "col-md-12",
        helpblock:
          "Setup directory (within bucket) for saving generated video thumbnails e.g thumbnails/",
        order: 4
      })
    );

    controls.push(
      new Controls.Textbox({
        key: "elastic_transcoder_directory",
        label: "Elastic Transcoder Directory",
        value: entity.elastic_transcoder_directory,
        colsize: "col-md-12",
        helpblock:
          "Setup directory for using uploading source videos that trigger elastic transcoder even for publishing videos.",
        order: 5
      })
    );

    controls.push(
      new Controls.Textbox({
        key: "public_url",
        label: "Public URL",
        value: entity.public_url,
        colsize: "col-md-12",
        helpblock:
          "Setup public url for accessing public media contents like video thumbnails, clips etc",
        order: 6
      })
    );

    controls.push(
      new Controls.Textbox({
        key: "private_url",
        label: "Private URL",
        value: entity.private_url,
        colsize: "col-md-12",
        helpblock:
          "Setup cloudfront keypair required for generating signed url for protected videos",
        order: 7
      })
    );

    controls.push(
      new Controls.Textbox({
        key: "cloudFront_keypair",
        label: "Cloud Front Key Pair",
        value: entity.cloudFront_keypair,
        colsize: "col-md-12",
        helpblock:
          "Setup cloudfront keypair required for generating signed url for protected videos",
        order: 8
      })
    );

    controls.push(
      new Controls.Textbox({
        key: "cloudFront_keyfilename",
        label: "Cloud Front Private Key FileName",
        value: entity.cloudFront_keyfilename,
        colsize: "col-md-12",
        helpblock:
          'Setup cloudfront keypair filename path required for generating signed url for protected videos, file (.pem) should be within your project e.g "/security/filename.pem"',
        order: 9
      })
    );

    return controls.sort((a, b) => a.order - b.order);
  }

  prepareVideosFFMPEGControls(entity: any) {
    const controls: FormBase<any>[] = [];

    controls.push(
      new Controls.CheckBox({
        key: "enable",
        label: "Enable",
        value: entity.enable,
        checked: entity.enable,
        helpblock: "Toggle on | off ffmpeg functionality for videos",
        order: 0
      })
    );

    controls.push(
      new Controls.CheckBox({
        key: "enable_admin",
        label: "Enable (Control Panel)",
        value: entity.enable_admin,
        checked: entity.enable_admin,
        helpblock: "Toggle on | off ffmpeg functionality for administrators",
        order: 1
      })
    );

    controls.push(
      new Controls.Dropdown({
        key: "video_Publishing_Type",
        label: "Publishing Type",
        required: true,
        value: entity.video_Publishing_Type.toString(),
        options: [
          {
            key: 0,
            value: "Sheduled Publishing"
          },
          {
            key: 1,
            value: "Direct Publishing"
          }
        ],
        helpblock:
          "Setup publishing approach based on availability -  0: Sheduled Publishing, 1: Direct Publishing",
        order: 1
      })
    );

    controls.push(
      new Controls.Textbox({
        key: "mp4_240p_Settings",
        label: "240p MP4 Command",
        value: entity.mp4_240p_Settings,
        colsize: "col-md-12",
        helpblock: "Ffmpeg commands specific to mp4 240p transcoding",
        order: 2
      })
    );

    controls.push(
      new Controls.Textbox({
        key: "mp4_360p_Settings",
        label: "360p MP4 Command",
        value: entity.mp4_360p_Settings,
        colsize: "col-md-12",
        helpblock: "Ffmpeg commands specific to mp4 360p transcoding",
        order: 3
      })
    );

    controls.push(
      new Controls.Textbox({
        key: "mp4_480p_Settings",
        label: "480p MP4 Command",
        value: entity.mp4_480p_Settings,
        colsize: "col-md-12",
        helpblock: "Ffmpeg commands specific to mp4 480p transcoding",
        order: 4
      })
    );

    controls.push(
      new Controls.Textbox({
        key: "mp4_720p_Settings",
        label: "720p MP4 Command",
        value: entity.mp4_720p_Settings,
        colsize: "col-md-12",
        helpblock: "Ffmpeg commands specific to mp4 720p transcoding",
        order: 5
      })
    );

    controls.push(
      new Controls.Textbox({
        key: "mp4_1080p_Settings",
        label: "1080p MP4 Command",
        value: entity.mp4_1080p_Settings,
        colsize: "col-md-12",
        helpblock: "Ffmpeg commands specific to mp4 720p transcoding",
        order: 6
      })
    );

    controls.push(
      new Controls.Dropdown({
        key: "encoding_options",
        label: "Encoding Options",
        required: true,
        value: entity.encoding_options.toString(),
        options: [
          {
            key: 0,
            value: "240p"
          },
          {
            key: 1,
            value: "360p"
          },
          {
            key: 2,
            value: "480p"
          },
          {
            key: 3,
            value: "720p"
          },
          {
            key: 4,
            value: "1080p"
          }
        ],
        helpblock:
          "Preferred encoding options for encoding (0: 240p, 1: 360p, 2: 480p, 3: 720p, 4: 1080p)",
        order: 7
      })
    );

    controls.push(
      new Controls.Textbox({
        key: "ffmpeg_path",
        label: "FFMPEG PATH",
        value: entity.ffmpeg_path,
        colsize: "col-md-12",
        helpblock: "Ffmpeg utility path for transcoding",
        order: 8
      })
    );

    controls.push(
      new Controls.Textbox({
        key: "mp4box_path",
        label: "MP4 Box Path",
        value: entity.mp4box_path,
        colsize: "col-md-12",
        helpblock:
          "Mp4box utility path for adding meta information to mp4 videos",
        order: 9
      })
    );

    controls.push(
      new Controls.CheckBox({
        key: "enable_clips",
        label: "Enable Clips",
        value: entity.enable_clips,
        checked: entity.enable_clips,
        helpblock: "Toggle on | off publishing video clip for preview purpose",
        order: 10
      })
    );

    controls.push(
      new Controls.Textbox({
        key: "clip_length",
        label: "Clip Length",
        value: entity.clip_length.toString(),
        colsize: "col-md-12",
        pattern: "[0-9]+",
        required: true,
        helpblock: "Setup clip length in seconds for videos",
        order: 11
      })
    );

    controls.push(
      new Controls.CheckBox({
        key: "enable_preview_video",
        label: "Enable Preview Video",
        value: entity.enable_preview_video,
        checked: entity.enable_preview_video,
        helpblock: "Toggle on | off generating small preview video",
        order: 12
      })
    );

    return controls.sort((a, b) => a.order - b.order);
  }

  prepareVideosYoutubeControls(entity: any) {
    const controls: FormBase<any>[] = [];

    controls.push(
      new Controls.CheckBox({
        key: "enable",
        label: "Enable (Account)",
        value: entity.enable,
        checked: entity.enable,
        helpblock: "Toggle on | off youtube functionality for videos",
        order: 0
      })
    );

    controls.push(
      new Controls.CheckBox({
        key: "enable_admin",
        label: "Enable (Control Panel)",
        value: entity.enable_admin,
        checked: entity.enable_admin,
        helpblock: "Toggle on | off youtube functionality for administrators",
        order: 1
      })
    );


    controls.push(
      new Controls.Textbox({
        key: "key",
        label: "API Key",
        value: entity.key,
        colsize: "col-md-12",
        helpblock:
          "Register your youtube api key generated via google developer console.",
        order: 1
      })
    );

    return controls.sort((a, b) => a.order - b.order);
  }

  prepareVideosDirectControls(entity: any) {
    const controls: FormBase<any>[] = [];

    controls.push(
      new Controls.CheckBox({
        key: "enable",
        label: "Enable (Account)",
        value: entity.enable,
        checked: entity.enable,
        helpblock: "Toggle on | off direct video functionality for videos",
        order: 0
      })
    );

    controls.push(
      new Controls.CheckBox({
        key: "enable_admin",
        label: "Enable (Control Panel)",
        value: entity.enable_admin,
        checked: entity.enable_admin,
        helpblock: "Toggle on | off direct video functionality for administrators",
        order: 1
      })
    );

    return controls.sort((a, b) => a.order - b.order);
  }

  prepareVideosMovieControls(entity: any) {
    const controls: FormBase<any>[] = [];

    controls.push(
      new Controls.CheckBox({
        key: "enable",
        label: "Enable (Account)",
        value: entity.enable,
        checked: entity.enable,
        helpblock: "Toggle on | off movie functionality for videos",
        order: 0
      })
    );

    controls.push(
      new Controls.CheckBox({
        key: "enable_admin",
        label: "Enable (Control Panel)",
        value: entity.enable_admin,
        checked: entity.enable_admin,
        helpblock: "Toggle on | off movie functionality for administrators",
        order: 1
      })
    );

    controls.push(
      new Controls.CheckBox({
        key: "enabled_embed",
        label: "Enable Embed Script (Account)",
        value: entity.enabled_embed,
        checked: entity.enabled_embed,
        helpblock: "Toggle on | off embed video / movie functionality for videos",
        order: 2
      })
    );

    controls.push(
      new Controls.CheckBox({
        key: "enable_embed_admin",
        label: "Enable Embed Script (Control Panel)",
        value: entity.enable_embed_admin,
        checked: entity.enable_embed_admin,
        helpblock: "Toggle on | off embed video / movie functionality for administrators",
        order: 3
      })
    );
    
    return controls.sort((a, b) => a.order - b.order);
  }

  prepareVideosPlayerControls(entity: any) {
    const controls: FormBase<any>[] = [];

    controls.push(
      new Controls.CheckBox({
        key: "enable_videojs",
        label: "Enable Video.JS Player",
        value: entity.enable_videojs,
        checked: entity.enable_videojs,
        helpblock:
          "Toggle on | off video js player unless you configured your own player manually",
        order: 0
      })
    );

    controls.push(
      new Controls.CheckBox({
        key: "auto_play",
        label: "Auto Play",
        value: entity.auto_play,
        checked: entity.auto_play,
        helpblock: "Toggle on | off autoplay functionality for players",
        order: 0
      })
    );

    return controls.sort((a, b) => a.order - b.order);
  }

 
}

/*
 * This file is subject to the terms and conditions defined in
 * file 'LICENSE.txt', which is part of this source code package.
 * Copyright 2007 - 2019 MediaSoftPro
 * For more information email at support@mediasoftpro.com
 */
