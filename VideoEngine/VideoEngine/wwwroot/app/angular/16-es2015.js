(window.webpackJsonp=window.webpackJsonp||[]).push([[16],{SoHB:function(t,e,s){"use strict";s.r(e);var o=s("ofXK"),i=s("3Pt+"),r=s("tyNb"),n=s("zwrK"),a=s("7NWl"),c=s("pz8m"),b=s("5zJ1"),u=s("pdjO"),d=s("ReVU"),m=s("fXoL"),h=s("nD3/"),l=s("LEd3"),f=s("RDfn"),g=s("+kG/");function p(t,e){1&t&&m.Nb(0,"app-loader")}function w(t,e){if(1&t){const t=m.Tb();m.Sb(0,"dynamic-modal-form",7),m.ec("OnSubmit",(function(e){return m.yc(t),m.gc().SubmitForm(e)})),m.Rb()}if(2&t){const t=m.gc();m.nc("controls",t.controls)("showLoader",t.showLoader)("showCancel",!1)("showModal",!1)("submitText",t.submitText)("submitCss",t.submitCss)}}let S=(()=>{class t{constructor(t,e,s,o,i,r,n,a){this.settingService=t,this.dataService=e,this.coreService=s,this.coreActions=o,this.actions=i,this.formService=r,this.router=n,this.cookieService=a,this.controls=[],this.showLoader=!1,this.submitText="Login",this.submitCss="btn btn-info btn-lg btn-block text-uppercase "}ngOnInit(){this.controls=this.formService.getLoginControls({username:"",password:"",rememberme:!1})}SubmitForm(t){this.showLoader=!0,console.log(t),this.dataService.Authenticate(t).subscribe(e=>{if("error"===e.status)this.coreActions.Notify({title:e.message,text:"",css:"bg-error"});else{void 0===e.user&&this.coreActions.Notify({title:"User Object Missing",text:"",css:"bg-error"}),this.coreActions.Notify({title:"Authenticated...",text:"",css:"bg-success"});const s={userid:e.user.userid,username:e.user.username,email:e.user.email,picturename:e.user.img_url,firstname:e.user.firstname,lastname:e.user.lastname};t.rememberme&&this.cookieService.set("_AUTH",JSON.stringify(s)),this.actions.Authenticate({isAuthenticated:!0,User:s}),this.router.navigate(["dashboard"])}this.showLoader=!1},t=>{this.showLoader=!1,this.coreActions.Notify({title:"Error Occured",text:"",css:"bg-danger"})})}}return t.\u0275fac=function(e){return new(e||t)(m.Mb(n.a),m.Mb(a.a),m.Mb(h.a),m.Mb(l.a),m.Mb(b.a),m.Mb(c.a),m.Mb(r.c),m.Mb(d.a))},t.\u0275cmp=m.Gb({type:t,selectors:[["app-login"]],hostVars:1,hostBindings:function(t,e){2&t&&m.Mc("@fadeInAnimation",void 0)},features:[m.yb([a.a,n.a,b.a,c.a,d.a])],decls:8,vars:2,consts:[[1,"login-register"],[1,"login-box","card"],[1,"card-body"],[1,"form-horizontal","form-material"],[1,"box-title","m-b-20"],[4,"ngIf"],[3,"controls","showLoader","showCancel","showModal","submitText","submitCss","OnSubmit",4,"ngIf"],[3,"controls","showLoader","showCancel","showModal","submitText","submitCss","OnSubmit"]],template:function(t,e){1&t&&(m.Sb(0,"div",0),m.Sb(1,"div",1),m.Sb(2,"div",2),m.Sb(3,"div",3),m.Sb(4,"h3",4),m.Ic(5,"Sign In"),m.Rb(),m.Gc(6,p,1,0,"app-loader",5),m.Gc(7,w,1,6,"dynamic-modal-form",6),m.Rb(),m.Rb(),m.Rb(),m.Rb()),2&t&&(m.zb(6),m.nc("ngIf",e.showLoader),m.zb(1),m.nc("ngIf",!e.showLoader))},directives:[o.o,f.a,g.a],encapsulation:2,data:{animation:[u.a]}}),t})();var v=s("o+qO");s.d(e,"LoginModule",(function(){return L}));const y=[{path:"",data:{title:"Dashboard",urls:[{title:"Control Panel"}]},component:S}];let L=(()=>{class t{}return t.\u0275mod=m.Kb({type:t}),t.\u0275inj=m.Jb({factory:function(e){return new(e||t)},imports:[[i.i,o.c,v.a,r.g.forChild(y)]]}),t})()},pdjO:function(t,e,s){"use strict";s.d(e,"a",(function(){return i}));var o=s("R0Ic");const i=Object(o.k)("fadeInAnimation",[Object(o.j)(":enter",[Object(o.i)({opacity:0,background:"#ff0000"}),Object(o.e)(".8s",Object(o.i)({opacity:1}))])])}}]);