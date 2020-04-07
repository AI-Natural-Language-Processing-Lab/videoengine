using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Jugnoon.Core
{
    public static class RouteConfig
    {
        public static IEndpointRouteBuilder Use(IEndpointRouteBuilder routeBuilder)
        {
          
            routeBuilder.MapControllerRoute(
                null,
                "media/{pid}/{title}",
                defaults: new { controller = "media", action = "Index" }
            );

            routeBuilder.MapControllerRoute(
                null,
                "videoframe/{pid}/{title}",
                defaults: new { controller = "videoframe", action = "Index" }
            );
         
            #region User Profile
            routeBuilder.MapControllerRoute(
                    null,
                    "user/profile/{username}",
                    defaults: new { controller = "user", action = "profile" }

                );

            routeBuilder.MapControllerRoute(
              null,
              "user/videos/{username}",
              defaults: new { controller = "user", action = "videos" }

          );
            routeBuilder.MapControllerRoute(
                 null,
                 "user/videos/{username}/{id}",
                 defaults: new { controller = "user", action = "videos" }

             );
            routeBuilder.MapControllerRoute(
                null,
                "user/{username}",
                defaults: new { controller = "user", action = "Index" }

             );
            #endregion
       

            #region Videos

            // video category processing routes
            routeBuilder.MapControllerRoute(
                     null,
                     "videos/category/filter/{title}/{filter}",
                     defaults: new { controller = "videos", action = "category" }
                     
              );
            routeBuilder.MapControllerRoute(
                   null,
                   "videos/category/filter/{title}/{filter}/{pagenumber}",
                   defaults: new { controller = "videos", action = "category" }
                   
            );

            routeBuilder.MapControllerRoute(
                      null,
                      "videos/category/filter/{title}/{filter}/{order}",
                      defaults: new { controller = "videos", action = "category" }
                      
            );

            routeBuilder.MapControllerRoute(
                  null,
                  "videos/category/filter/{title}/{filter}/{order}/{pagenumber}",
                  defaults: new { controller = "videos", action = "category" }
                  
            );

            routeBuilder.MapControllerRoute(
                    null,
                    "videos/category/{title}/{order}",
                    defaults: new { controller = "videos", action = "category" }
                    
              );
            routeBuilder.MapControllerRoute(
                  null,
                  "videos/category/{title}/{order}/{pagenumber}",
                  defaults: new { controller = "videos", action = "category" }
                  
            );

            routeBuilder.MapControllerRoute(
                   null,
                   "videos/category/{title}",
                   defaults: new { controller = "videos", action = "category" }
                   
            );

            routeBuilder.MapControllerRoute(
                 null,
                 "videos/category/{title}/{pagenumber}",
                 defaults: new { controller = "videos", action = "category" }
                 
            );

            // video tag processing routes
            routeBuilder.MapControllerRoute(
                  null,
                  "videos/label/filter/{title}/{filter}",
                  defaults: new { controller = "videos", action = "label" }
                  
            );
            routeBuilder.MapControllerRoute(
                  null,
                  "videos/label/filter/{title}/{filter}/{pagenumber}",
                  defaults: new { controller = "videos", action = "label" }
                  
            );
            routeBuilder.MapControllerRoute(
                 null,
                 "videos/label/filter/{title}/{filter}/{order}",
                 defaults: new { controller = "videos", action = "label" }
                 
            );
            routeBuilder.MapControllerRoute(
                  null,
                  "videos/label/filter/{title}/{filter}/{order}/{pagenumber}",
                  defaults: new { controller = "videos", action = "label" }
                  
            );
            routeBuilder.MapControllerRoute(
               null,
               "videos/label/{title}/{order}/{pagenumber}",
               defaults: new { controller = "videos", action = "label" }
               
            );
            routeBuilder.MapControllerRoute(
               null,
               "videos/label/{title}/{order}",
               defaults: new { controller = "videos", action = "label" }
               
            );
            routeBuilder.MapControllerRoute(
               null,
               "videos/label/{title}/{order}/{pagenumber}",
               defaults: new { controller = "videos", action = "label" }
               
            );
            routeBuilder.MapControllerRoute(
               null,
               "videos/label/{title}",
               defaults: new { controller = "videos", action = "label" }
               
            );

            routeBuilder.MapControllerRoute(
               null,
               "videos/label/{title}/{pagenumber}",
               defaults: new { controller = "videos", action = "label" }
               
            );

            // video archive processing routes

            routeBuilder.MapControllerRoute(
               null,
               "videos/archive/{month}/{year}/{order}",
               defaults: new { controller = "videos", action = "archive" }
               
            );

            routeBuilder.MapControllerRoute(
               null,
               "videos/archive/{month}/{year}/{order}/{pagenumber}",
               defaults: new { controller = "videos", action = "archive" }
               
            );

            routeBuilder.MapControllerRoute(
              null,
              "videos/archive/{month}/{year}",
              defaults: new { controller = "videos", action = "archive" }
              
           );

            routeBuilder.MapControllerRoute(
               null,
               "videos/archive/{month}/{year}/{pagenumber}",
               defaults: new { controller = "videos", action = "archive" }
               
            );
            
            routeBuilder.MapControllerRoute(
                  null,
                  "videos/categories",
                  defaults: new { controller = "videos", action = "categories" }
                  
            );
            routeBuilder.MapControllerRoute(
                  null,
                  "videos/categories/{pagenumber}",
                  defaults: new { controller = "videos", action = "categories" }
                  
            );
          
            routeBuilder.MapControllerRoute(
                  null,
                  "videos/archivelist",
                  defaults: new { controller = "videos", action = "archivelist" }
            );

            // video taglist routes
            routeBuilder.MapControllerRoute(
                  null,
                  "videos/labels",
                  defaults: new { controller = "videos", action = "labels" }
                  
            );
            routeBuilder.MapControllerRoute(
                  null,
                  "videos/labels/{pagenumber}",
                  defaults: new { controller = "videos", action = "labels" }
                  
            );
            routeBuilder.MapControllerRoute(
                  null,
                  "videos/labels/search/{term}",
                  defaults: new { controller = "videos", action = "labels" }
                  
            );
            routeBuilder.MapControllerRoute(
                  null,
                  "videos/labels/search/{term}/{pagenumber}",
                  defaults: new { controller = "videos", action = "labels" }
                  
            );
            // search routes 
            routeBuilder.MapControllerRoute(
                   null,
                   "videos/queryresult",
                   defaults: new { controller = "videos", action = "queryresult" }
                   
            );

            routeBuilder.MapControllerRoute(
                  null,
                  "videos/search/filter/{filter}/{term}",
                  defaults: new { controller = "videos", action = "search" }
                  
              );

            routeBuilder.MapControllerRoute(
                null,
                "videos/search/filter/{filter}/{term}/{pagenumber}",
                defaults: new { controller = "videos", action = "search" }
                
            );

            routeBuilder.MapControllerRoute(
                null,
                "videos/search/{term}",
                defaults: new { controller = "videos", action = "search" }
                
            );

            routeBuilder.MapControllerRoute(
                null,
                "videos/search/{term}/{pagenumber}",
                defaults: new { controller = "videos", action = "search" }
                
            );


            routeBuilder.MapControllerRoute(
                null,
                "videos/page/{pagenumber}",
                defaults: new { controller = "videos", action = "Index" }
                
            );

            routeBuilder.MapControllerRoute(
                null,
                "videos/{order}/{filter}",
                defaults: new { controller = "videos", action = "Index" }
                
            );

            routeBuilder.MapControllerRoute(
                   null,
                   "videos/{order}/{filter}/{pagenumber}",
                   defaults: new { controller = "videos", action = "Index" }
                   
             );

            routeBuilder.MapControllerRoute(
                 null,
                 "videos/{order}",
                 defaults: new { controller = "videos", action = "Index" }
                 
            );

            routeBuilder.MapControllerRoute(
                 null,
                 "videos/{order}/{pagenumber}",
                 defaults: new { controller = "videos", action = "Index" }
            );

            routeBuilder.MapControllerRoute(
               null,
               "videos/",
               defaults: new { controller = "videos", action = "Index" }
            );

            #endregion


            routeBuilder.MapControllerRoute(
                name: "ActionApi",
                pattern: "api/{controller}/{action}/{name?}"
            );

            routeBuilder.MapControllerRoute(
                 null,
                 "/account/{*url}",
                 defaults: new { controller = "account", action = "Index" }
            );

            routeBuilder.MapControllerRoute(
              null,
              "/admin/{*url}",
              defaults: new { controller = "admin", action = "Index" }
            );

            routeBuilder.MapFallbackToController("account/", "Index", "account");
            routeBuilder.MapFallbackToController("admin/", "Index", "admin");

            // default root
            routeBuilder.MapControllerRoute(
                 null,
                 "/{page}",
                 defaults: new { controller = "Home", action = "Index" }
            );

            routeBuilder.MapControllerRoute(
                   name: "default",
                   pattern: "{controller=Home}/{action=Index}/{id?}");


            return routeBuilder;
        }
    }
    
    public static class InitRouteControllerConfig
    {
        public static IEndpointRouteBuilder Use(IEndpointRouteBuilder routeBuilder)
        {
            routeBuilder.MapControllerRoute(
                            name: "default",
                            pattern: "{controller=Installation}/{action=Index}/{id?}");

            return routeBuilder;
        }
    }
}
