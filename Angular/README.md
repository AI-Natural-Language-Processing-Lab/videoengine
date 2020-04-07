### Angular Appliation Deployment Guide

This project include angular application located in `Angular` directory on root of project that server functionality for multiple project areas including Admin, Account, Search, Application Installation etc.

Project uses compiled version of this application, if you need changes you can only be done via this project to be open separately in separate IDE (Visual Code IDE Recommened)

It's built using Angular CLI and have various ways to run and start working into it.

### Build Application

When you execute command this will build and publish application under `/wwwroot/app/angular` directory of core Project.

#### Steps to follow

1. open project in any IDE and run command 'npm install' to install all required project dependencies.
2. open angular.json and remove styles / scripts as its already set within application (do changes with your requirements)

```css
"src/assets/css/bootswatch/flatly/bootstrap.min.css",
"src/assets/css/bootswatch/flatly/theme.css",
"node_modules/font-awesome/css/font-awesome.min.css",
"src/assets/css/style.css",
```

```javascript
"src/assets/plugins/bootstrap/js/popper.min.js",
"src/assets/js/bootstrap.min.js",
```

3. open app.module.ts and update  translator path '/app/angular/assets/i18n/' (when using within website), './assets/i18n' (when using as stand-alone application)

4. execute command "ng build --base-href=/ --deploy-url /app/angular/ --prod --output-hashing none" to publish application in directory setup via angular.json

### Deploy as Standalone App:

If you want to deploy this app as standalone e.g in /angular/ directory of your application. just execute the following command

   ```bash
 ng build --base-href=/angular/ --deploy-url /angular/ --prod
```

output will be generated in angular.json -> outputPath (path setting directory)

### Working locally within Application

If you want to run application locally for making changes in functionality, do the following steps

1. Open `index.html` (root application html file) and adjust URL (API URL). Make sure api url running and accessible.

2. Put both script and css files removed at time of application building again in angular.json file.

3. Run command `ng serve --open` to build and run application