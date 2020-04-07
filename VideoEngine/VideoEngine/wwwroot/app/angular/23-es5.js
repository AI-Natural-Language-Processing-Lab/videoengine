function _defineProperties(t,e){for(var i=0;i<e.length;i++){var n=e[i];n.enumerable=n.enumerable||!1,n.configurable=!0,"value"in n&&(n.writable=!0),Object.defineProperty(t,n.key,n)}}function _createClass(t,e,i){return e&&_defineProperties(t.prototype,e),i&&_defineProperties(t,i),t}function _classCallCheck(t,e){if(!(t instanceof e))throw new TypeError("Cannot call a class as a function")}(window.webpackJsonp=window.webpackJsonp||[]).push([[23],{fpYc:function(t,e,i){"use strict";i.r(e);var n,o,s,a=i("1kSV"),r=i("ofXK"),c=i("3Pt+"),l=i("tyNb"),d=i("fXoL"),u=i("QYT1"),b=((n=function t(){_classCallCheck(this,t),this.isAdmin=!0}).\u0275fac=function(t){return new(t||n)},n.\u0275cmp=d.Gb({type:n,selectors:[["ng-component"]],decls:1,vars:1,consts:[[3,"isAdmin"]],template:function(t,e){1&t&&d.Nb(0,"app-mainvideo-list",0),2&t&&d.nc("isAdmin",e.isAdmin)},directives:[u.a],encapsulation:2}),n),p=i("VHDF"),h=i("si/t"),f=i("ow1T"),m=i("+P6t"),v=i("o+qO"),g=((o=function t(){_classCallCheck(this,t)}).\u0275mod=d.Kb({type:o}),o.\u0275inj=d.Jb({factory:function(t){return new(t||o)},providers:[p.a,h.a,f.a,m.a],imports:[[r.c,v.a,l.g,c.i]]}),o),y=i("mrSG"),I=i("Usal"),S=i("pdjO"),C=i("nD3/"),A=i("LEd3"),R=i("+kG/"),w=((s=function(){function t(e,i,n,o,s){_classCallCheck(this,t),this.activeModal=e,this.service=i,this.dataService=n,this.coreService=o,this.coreActions=s,this.showLoader=!1,this.Categories=[],this.list=[]}return _createClass(t,[{key:"ngOnInit",value:function(){var t=this;this.title=this.Info.title,0===this.Info.viewType?this.controls=this.service.getCoverControls(this.Info.data,this.Info.auth):1===this.Info.viewType?this.categories$.subscribe((function(e){t.Categories=e,t.controls=t.service.getVideoEditControls(t.Info.data,!0),t.updateCategories()})):this.controls=this.service.getControls(this.Info.data,this.Info.auth)}},{key:"updateCategories",value:function(){this.coreService.updateCategories(this.controls,this.Categories)}},{key:"SubmitForm",value:function(t){var e=this;void 0===this.Info.isActionGranded||this.Info.isActionGranded?0===this.Info.viewType?(this.Info.data.video_thumbs=t.video_thumbs,this.activeModal.close({data:this.Info.data})):1===this.Info.viewType&&(t.id=this.Info.data.id,t.categories=this.coreService.returnSelectedCategoryArray(t.categories),this.showLoader=!0,this.dataService.EditRecord([t]).subscribe((function(t){"error"===t.status?e.coreActions.Notify({title:t.message,text:"",css:"bg-error"}):(e.coreActions.Notify({title:"Record Updated Successfully",text:"",css:"bg-success"}),e.activeModal.close({data:t.record,isenabled:"Updated"})),e.showLoader=!1}),(function(t){e.showLoader=!1,e.coreActions.Notify({title:"Error Occured",text:"",css:"bg-danger"})}))):this.coreActions.Notify({title:"Permission Denied",text:"",css:"bg-danger"})}},{key:"close",value:function(){this.activeModal.dismiss("Cancel Clicked")}}]),t}()).\u0275fac=function(t){return new(t||s)(d.Mb(a.a),d.Mb(f.a),d.Mb(h.a),d.Mb(C.a),d.Mb(A.a))},s.\u0275cmp=d.Gb({type:s,selectors:[["viewmodal"]],inputs:{Info:"Info"},features:[d.yb([f.a,h.a])],decls:7,vars:3,consts:[[1,"modal-header"],[1,"modal-title"],["type","button","aria-label","Close",1,"close",3,"click"],["aria-hidden","true"],[3,"controls","showLoader","OnSubmit","OnClose"]],template:function(t,e){1&t&&(d.Sb(0,"div",0),d.Sb(1,"h4",1),d.Ic(2),d.Rb(),d.Sb(3,"button",2),d.ec("click",(function(){return e.activeModal.dismiss("Cross click")})),d.Sb(4,"span",3),d.Ic(5,"\xd7"),d.Rb(),d.Rb(),d.Rb(),d.Sb(6,"dynamic-modal-form",4),d.ec("OnSubmit",(function(t){return e.SubmitForm(t)}))("OnClose",(function(){return e.close()})),d.Rb()),2&t&&(d.zb(2),d.Jc(e.title),d.zb(4),d.nc("controls",e.controls)("showLoader",e.showLoader))},directives:[R.a],encapsulation:2}),Object(y.a)([Object(I.e)(["videos","categories"])],s.prototype,"categories$",void 0),s),k=i("Hjmt"),_=i("RDfn"),G=i("4HLj"),T=i("jhN1");function O(t,e){if(1&t&&(d.Sb(0,"div"),d.Sb(1,"video",17),d.Nb(2,"source",18),d.Rb(),d.Rb()),2&t){var i=d.gc();d.zb(2),d.oc("src",i.Info.player.url,d.Bc)}}function M(t,e){if(1&t&&d.Nb(0,"iframe",19),2&t){var i=d.gc();d.nc("src",i.Info.player.youtubeid,d.Ac)}}var N=function(t){return[t]};function z(t,e){if(1&t&&(d.Sb(0,"span"),d.Sb(1,"a",22),d.Ic(2),d.Rb(),d.Rb()),2&t){var i=e.$implicit;d.zb(1),d.nc("routerLink",d.sc(2,N,"/blogs/category/"+i.category.term)),d.zb(1),d.Jc(i.category.title)}}function L(t,e){if(1&t&&(d.Sb(0,"div"),d.Sb(1,"div",20),d.Ic(2," Categories "),d.Rb(),d.Sb(3,"div",20),d.Gc(4,z,3,4,"span",21),d.Rb(),d.Nb(5,"hr"),d.Rb()),2&t){var i=d.gc();d.zb(4),d.nc("ngForOf",i.Info.category_list)}}function V(t,e){if(1&t&&(d.Sb(0,"span"),d.Sb(1,"a",23),d.Ic(2),d.Rb(),d.Rb()),2&t){var i=e.$implicit;d.zb(1),d.nc("routerLink",d.sc(2,N,"/videos/tag/"+i.slug)),d.zb(1),d.Jc(i.title)}}function D(t,e){if(1&t&&(d.Sb(0,"div"),d.Sb(1,"div",20),d.Ic(2," Tags: "),d.Rb(),d.Gc(3,V,3,4,"span",21),d.Rb()),2&t){var i=d.gc();d.zb(3),d.nc("ngForOf",i.Info.tags_arr)}}function F(t,e){1&t&&(d.Sb(0,"span",24),d.Ic(1,"Blocked"),d.Rb())}function j(t,e){1&t&&(d.Sb(0,"span",25),d.Ic(1,"Active"),d.Rb())}function U(t,e){1&t&&(d.Sb(0,"span",26),d.Ic(1,"Approved"),d.Rb())}function P(t,e){1&t&&(d.Sb(0,"span",27),d.Ic(1,"Under Review"),d.Rb())}function x(t,e){1&t&&(d.Sb(0,"span",28),d.Ic(1,"Featured"),d.Rb())}function J(t,e){1&t&&(d.Sb(0,"span",28),d.Ic(1,"Not Featured"),d.Rb())}function K(t,e){1&t&&(d.Sb(0,"span",24),d.Ic(1,"Adult"),d.Rb())}function E(t,e){1&t&&(d.Sb(0,"span",24),d.Ic(1,"Restrited"),d.Rb())}function $(t,e){1&t&&(d.Sb(0,"span",27),d.Ic(1,"Unlisted"),d.Rb())}function H(t,e){1&t&&(d.Sb(0,"span",25),d.Ic(1,"Public"),d.Rb())}var B,Y=((B=function(){function t(e,i,n,o){_classCallCheck(this,t),this.modalService=e,this.settingService=i,this.dataService=n,this.sanitizer=o,this.Info={},this.Author_FullName=""}return _createClass(t,[{key:"ngOnInit",value:function(){null!==this.Info.player.youtubeid&&(this.Info.player.youtubeid=this.sanitizer.bypassSecurityTrustResourceUrl(this.Info.player.youtubeid))}},{key:"edit",value:function(){this.TriggleModal()}},{key:"TriggleModal",value:function(){var t=this,e=this.modalService.open(w,{backdrop:!1});e.componentInstance.Info={title:"Edit Video Information",data:this.Info,viewType:1},e.result.then((function(e){t.Info.title=e.data.title,t.Info.description=e.data.description}),(function(t){console.log("dismissed")}))}},{key:"youtubeURL",value:function(){console.log(this.Info.player.youtubeid)}}]),t}()).\u0275fac=function(t){return new(t||B)(d.Mb(a.b),d.Mb(p.a),d.Mb(h.a),d.Mb(T.b))},B.\u0275cmp=d.Gb({type:B,selectors:[["app-video-info"]],inputs:{Info:"Info",Author_FullName:"Author_FullName"},decls:46,vars:31,consts:[[1,"card"],[1,"card-body"],["clas","m-t-10 m-b-10"],[4,"ngIf"],["width","800","height","640","frameborder","0","allow","autoplay; encrypted-media","allowfullscreen","",3,"src",4,"ngIf"],[3,"innerHTML"],[3,"routerLink"],[1,"text-muted"],[1,"fa","fa-clock-o"],[1,"btn","btn-primary",3,"routerLink"],["class","badge badge-danger  m-r-5",4,"ngIf"],["class","badge badge-success  m-r-5",4,"ngIf"],["class","badge badge-info  m-r-5",4,"ngIf"],["class","badge badge-warning  m-r-5",4,"ngIf"],["class","badge badge-primary  m-r-5",4,"ngIf"],[1,"m-b-10","m-t-10"],[1,"btn","btn-danger",3,"click"],["controls","",2,"width","100%","height","auto"],["type","video/mp4",3,"src"],["width","800","height","640","frameborder","0","allow","autoplay; encrypted-media","allowfullscreen","",3,"src"],[1,"m-b-10"],[4,"ngFor","ngForOf"],[1,"btn","btn-info","m-r-5",3,"routerLink"],[1,"btn","btn-danger","m-r-5",3,"routerLink"],[1,"badge","badge-danger","m-r-5"],[1,"badge","badge-success","m-r-5"],[1,"badge","badge-info","m-r-5"],[1,"badge","badge-warning","m-r-5"],[1,"badge","badge-primary","m-r-5"]],template:function(t,e){1&t&&(d.Sb(0,"div",0),d.Sb(1,"div",1),d.Sb(2,"h3"),d.Ic(3),d.Rb(),d.Sb(4,"div",2),d.Gc(5,O,3,1,"div",3),d.Gc(6,M,1,1,"iframe",4),d.Rb(),d.Nb(7,"hr"),d.Nb(8,"div",5),d.Nb(9,"hr"),d.Sb(10,"p"),d.Ic(11," Author: "),d.Sb(12,"a",6),d.Ic(13),d.Rb(),d.Rb(),d.Sb(14,"p"),d.Ic(15," Uploaded: "),d.Sb(16,"span",7),d.Nb(17,"i",8),d.Rb(),d.Ic(18),d.hc(19,"date"),d.Rb(),d.Nb(20,"hr"),d.Gc(21,L,6,1,"div",3),d.Gc(22,D,4,1,"div",3),d.Nb(23,"hr"),d.Sb(24,"a",9),d.Ic(25),d.Rb(),d.Nb(26,"hr"),d.Sb(27,"p"),d.Ic(28),d.Rb(),d.Sb(29,"p"),d.Ic(30),d.Rb(),d.Nb(31,"hr"),d.Sb(32,"div"),d.Gc(33,F,2,0,"span",10),d.Gc(34,j,2,0,"span",11),d.Gc(35,U,2,0,"span",12),d.Gc(36,P,2,0,"span",13),d.Gc(37,x,2,0,"span",14),d.Gc(38,J,2,0,"span",14),d.Gc(39,K,2,0,"span",10),d.Gc(40,E,2,0,"span",10),d.Gc(41,$,2,0,"span",13),d.Gc(42,H,2,0,"span",11),d.Rb(),d.Sb(43,"div",15),d.Sb(44,"button",16),d.ec("click",(function(){return e.edit()})),d.Ic(45,"Edit"),d.Rb(),d.Rb(),d.Rb(),d.Rb()),2&t&&(d.zb(3),d.Jc(e.Info.title),d.zb(2),d.nc("ngIf",null!==e.Info.player.url),d.zb(1),d.nc("ngIf",null!==e.Info.youtubeid),d.zb(2),d.nc("innerHTML",e.Info.description,d.zc),d.zb(4),d.nc("routerLink",d.sc(27,N,"/users/profile/"+e.Info.author.id)),d.zb(1),d.Lc("",e.Author_FullName," - (",e.Info.username,")"),d.zb(5),d.Kc(" ",d.jc(19,24,e.Info.created_at,"fullDate")," "),d.zb(3),d.nc("ngIf",e.Info.category_list.length>0),d.zb(1),d.nc("ngIf",e.Info.tags_arr.length>0),d.zb(2),d.nc("routerLink",d.sc(29,N,"/videos/user/"+e.Info.username)),d.zb(1),d.Kc("Browse all ",e.Info.username," videos..."),d.zb(3),d.Kc("Views: ",e.Info.views,""),d.zb(2),d.Kc("Liked: ",e.Info.liked,""),d.zb(3),d.nc("ngIf",0==e.Info.isenabled),d.zb(1),d.nc("ngIf",1==e.Info.isenabled),d.zb(1),d.nc("ngIf",1==e.Info.isapproved),d.zb(1),d.nc("ngIf",0==e.Info.isapproved),d.zb(1),d.nc("ngIf",1==e.Info.isfeatured),d.zb(1),d.nc("ngIf",0==e.Info.isfeatured),d.zb(1),d.nc("ngIf",1==e.Info.isadult),d.zb(1),d.nc("ngIf",1==e.Info.isprivate),d.zb(1),d.nc("ngIf",2==e.Info.isprivate),d.zb(1),d.nc("ngIf",0==e.Info.isprivate))},directives:[r.o,l.f,r.n],pipes:[r.e],encapsulation:2}),B);function X(t,e){1&t&&(d.Sb(0,"div"),d.Sb(1,"h2",1),d.Ic(2,"Access Denied"),d.Rb(),d.Rb())}function W(t,e){1&t&&d.Nb(0,"app-loader")}function q(t,e){if(1&t){var i=d.Tb();d.Sb(0,"div",15),d.Sb(1,"a",16),d.ec("click",(function(t){d.yc(i);var n=e.$implicit;return d.gc(4).updateSelectedThumb(n,t),!1})),d.Nb(2,"img",17),d.Rb(),d.Rb()}if(2&t){var n=e.$implicit;d.zb(2),d.oc("src",n.filename,d.Bc),d.nc("ngClass",n.selected?"selected":"")}}function Q(t,e){if(1&t){var i=d.Tb();d.Sb(0,"div",9),d.Sb(1,"div",10),d.Sb(2,"h4",11),d.Ic(3,"Update Video Thumb"),d.Rb(),d.Rb(),d.Sb(4,"div",6),d.Sb(5,"div",2),d.Gc(6,q,3,2,"div",12),d.Rb(),d.Rb(),d.Sb(7,"div",13),d.Sb(8,"button",14),d.ec("click",(function(){return d.yc(i),d.gc(3).updateThumb()})),d.Ic(9," Save Changes "),d.Rb(),d.Rb(),d.Rb()}if(2&t){var n=d.gc(3);d.zb(6),d.nc("ngForOf",n.Info.video_thumbs)}}function Z(t,e){if(1&t){var i=d.Tb();d.Sb(0,"div"),d.Sb(1,"div",4),d.Sb(2,"app-toolbar-v2",5),d.ec("Action",(function(t){return d.yc(i),d.gc(2).toolbaraction(t)})),d.Rb(),d.Sb(3,"div",6),d.Nb(4,"app-video-info",7),d.Rb(),d.Rb(),d.Gc(5,Q,10,1,"div",8),d.Rb()}if(2&t){var n=d.gc(2);d.zb(2),d.nc("Options",n.ToolbarOptions)("isItemsSelected",n.isItemsSelected),d.zb(2),d.nc("Author_FullName",n.Author_FullName)("Info",n.Info),d.zb(1),d.nc("ngIf",n.Info.video_thumbs.length>0)}}function tt(t,e){if(1&t&&(d.Sb(0,"div"),d.Sb(1,"div",2),d.Sb(2,"div",3),d.Gc(3,W,1,0,"app-loader",0),d.Gc(4,Z,6,5,"div",0),d.Rb(),d.Rb(),d.Rb()),2&t){var i=d.gc();d.zb(3),d.nc("ngIf",i.showLoader),d.zb(1),d.nc("ngIf",!i.showLoader)}}var et,it,nt,ot,st,at,rt,ct,lt,dt,ut,bt,pt,ht,ft,mt,vt,gt,yt,It,St=((et=function(){function t(e,i,n,o,s,a,r){_classCallCheck(this,t),this.settingService=e,this.dataService=i,this.coreActions=n,this.route=o,this.modalService=s,this.permission=a,this.router=r,this.isItemsSelected=!0,this.RecordID=0,this.FilterOptions={},this.controls=[],this.showLoader=!1,this.formHeading="Video Detail",this.submitText="Submit",this.Info={},this.uploadedFiles=[],this.SelectedItems=[],this.Author_FullName="",this.isAccessGranted=!1,this.isActionGranded=!1}return _createClass(t,[{key:"ngOnInit",value:function(){var t=this;this.auth$.subscribe((function(e){t.permission.GrandResourceAccess(!1,"1521153486644","1521395130448",e.Role)&&(t.isAccessGranted=!0,t.permission.GrandResourceAction("1521153486644",e.Role)&&(t.isActionGranded=!0))})),this.ToolbarOptions=this.settingService.getToolbarOptions(0,!0),this.ToolbarOptions.showtoolbar=!1,this.ToolbarOptions.showcheckAll=!1,this.ToolbarOptions.showsecondarytoolbar=!0,this.ToolbarOptions.ProfileView=!0,this.route.params.subscribe((function(e){t.RecordID=Number.parseInt(e.id,10),isNaN(t.RecordID)&&(t.RecordID=0),t.RecordID>0&&t.LoadInfo()}))}},{key:"LoadInfo",value:function(){var t=this;this.showLoader=!0,this.dataService.GetInfo(this.RecordID).subscribe((function(e){"error"===e.status?(t.coreActions.Notify({title:e.message,text:"",css:"bg-success"}),t.router.navigate(["/videos"])):(t.Info=e.post,t.Author_FullName=null===t.Info.author.firstname||""===t.Info.author.firstname?t.Info.author.username:t.Info.author.firstname+" "+t.Info.author.lastname,t.Info.tags_arr=null!==t.Info.tags&&""!==t.Info.tags?t.ProcessCategories(t.Info.tags.split(",")):[],t.SelectedItems=[],t.SelectedItems.push(t.Info),t.processUpdateThumbs()),t.showLoader=!1}))}},{key:"ProcessCategories",value:function(t){var e=[],i=!0,n=!1,o=void 0;try{for(var s,a=t[Symbol.iterator]();!(i=(s=a.next()).done);i=!0){var r=s.value;e.push({title:r.trim(),slug:r.trim().replace(/\s+/g,"-").toLowerCase()})}}catch(c){n=!0,o=c}finally{try{i||null==a.return||a.return()}finally{if(n)throw o}}return e}},{key:"toolbaraction",value:function(t){switch(t.action){case"m_markas":this.ProcessActions(t.value);break;case"add":return void this.Trigger_Modal({title:"Create Account",isActionGranded:this.isActionGranded,data:this.settingService.getInitObject(),viewType:1});case"edit":return void this.Trigger_Modal({title:"Edit User Profile",isActionGranded:this.isActionGranded,data:this.Info,viewType:2})}}},{key:"Trigger_Modal",value:function(t){var e=this.modalService.open(w,{backdrop:!1});e.componentInstance.Info=t,e.result.then((function(t){}),(function(t){console.log("dismissed")}))}},{key:"ProcessActions",value:function(t){if(this.isActionGranded){if(this.SelectedItems.length>0){var e=!0,i=!1,n=void 0;try{for(var o,s=this.SelectedItems[Symbol.iterator]();!(e=(o=s.next()).done);e=!0)o.value.actionstatus=t.actionstatus}catch(a){i=!0,n=a}finally{try{e||null==s.return||s.return()}finally{if(i)throw n}}this.dataService.ProcessActions(this.SelectedItems,t,0)}}else this.coreActions.Notify({title:"Permission Denied",text:"",css:"bg-danger"})}},{key:"processUpdateThumbs",value:function(){if(console.log(this.Info),0===this.Info.isexternal){this.Info.video_thumbs=[];for(var t=1;t<=10;t++){var e,i;i=(e=t<=9?this.Info.picturename+"_00"+t+".jpg":this.Info.picturename+"_0"+t+".jpg")===this.Info.thumb_url,this.Info.video_thumbs.push({id:t,filename:e,selected:i})}}}},{key:"updateSelectedThumb",value:function(t,e){if(this.Info.video_thumbs.length>0){var i=!0,n=!1,o=void 0;try{for(var s,a=this.Info.video_thumbs[Symbol.iterator]();!(i=(s=a.next()).done);i=!0){var r=s.value;r.selected=r.id===t.id}}catch(c){n=!0,o=c}finally{try{i||null==a.return||a.return()}finally{if(n)throw o}}e.stopPropagation()}}},{key:"updateThumb",value:function(){var t=this,e={id:this.Info.id,username:this.Info.id},i=!0,n=!1,o=void 0;try{for(var s,a=this.Info.video_thumbs[Symbol.iterator]();!(i=(s=a.next()).done);i=!0){var r=s.value;r.selected&&(e.picturename=r.filename)}}catch(c){n=!0,o=c}finally{try{i||null==a.return||a.return()}finally{if(n)throw o}}this.dataService.UpdateThumbnail(e).subscribe((function(e){t.coreActions.Notify("error"===e.status?{title:e.message,text:"",css:"bg-error"}:{title:e.message,text:"",css:"bg-success"})}),(function(e){t.coreActions.Notify({title:"Error Occured",text:"",css:"bg-danger"})}))}}]),t}()).\u0275fac=function(t){return new(t||et)(d.Mb(p.a),d.Mb(h.a),d.Mb(A.a),d.Mb(l.a),d.Mb(a.b),d.Mb(k.a),d.Mb(l.c))},et.\u0275cmp=d.Gb({type:et,selectors:[["ng-component"]],hostVars:1,hostBindings:function(t,e){2&t&&d.Mc("@fadeInAnimation",void 0)},decls:2,vars:2,consts:[[4,"ngIf"],[1,"m-b-40","m-t-40","text-center"],[1,"row"],[1,"col-md-9"],[1,"card"],[3,"Options","isItemsSelected","Action"],[1,"card-body"],[3,"Author_FullName","Info"],["class","card m-t-10",4,"ngIf"],[1,"card","m-t-10"],[1,"card-header"],[1,"m-b-0"],["class","col-md-3 m-b-10",4,"ngFor","ngForOf"],[1,"card-footer"],[1,"btn","btn-primary",3,"click"],[1,"col-md-3","m-b-10"],["href","#",3,"click"],[1,"img-responsive",3,"ngClass","src"]],template:function(t,e){1&t&&(d.Gc(0,X,3,0,"div",0),d.Gc(1,tt,5,2,"div",0)),2&t&&(d.nc("ngIf",!e.isAccessGranted),d.zb(1),d.nc("ngIf",e.isAccessGranted))},directives:[r.o,_.a,G.a,Y,r.n,r.m],encapsulation:2,data:{animation:[S.a]}}),Object(y.a)([Object(I.e)(["users","auth"])],et.prototype,"auth$",void 0),et),Ct=i("wHF2"),At=((it=function t(){_classCallCheck(this,t),this.isAdmin=!0}).\u0275fac=function(t){return new(t||it)},it.\u0275cmp=d.Gb({type:it,selectors:[["ng-component"]],decls:1,vars:2,consts:[[3,"isAdmin","route_path"]],template:function(t,e){1&t&&d.Nb(0,"app-direct-uploader",0),2&t&&d.nc("isAdmin",e.isAdmin)("route_path","/videos/")},directives:[Ct.a],encapsulation:2}),it),Rt=i("HNhL"),wt=((nt=function t(){_classCallCheck(this,t)}).\u0275mod=d.Kb({type:nt}),nt.\u0275inj=d.Jb({factory:function(t){return new(t||nt)},imports:[[r.c,v.a,l.g,c.i,c.u,Rt.a]]}),nt),kt=i("jL45"),_t=((st=function t(){_classCallCheck(this,t),this.isAdmin=!0}).\u0275fac=function(t){return new(t||st)},st.\u0275cmp=d.Gb({type:st,selectors:[["ng-component"]],decls:1,vars:1,consts:[[3,"isAdmin"]],template:function(t,e){1&t&&d.Nb(0,"app-youtube-uploader",0),2&t&&d.nc("isAdmin",e.isAdmin)},directives:[kt.a],encapsulation:2}),st),Gt=((ot=function t(){_classCallCheck(this,t)}).\u0275mod=d.Kb({type:ot}),ot.\u0275inj=d.Jb({factory:function(t){return new(t||ot)},imports:[[r.c,v.a,l.g,c.i,c.u,Rt.a]]}),ot),Tt=i("+K8V"),Ot=((rt=function t(){_classCallCheck(this,t),this.isAdmin=!0}).\u0275fac=function(t){return new(t||rt)},rt.\u0275cmp=d.Gb({type:rt,selectors:[["ng-component"]],decls:1,vars:2,consts:[[3,"isAdmin","route_path"]],template:function(t,e){1&t&&d.Nb(0,"app-ffmpeg-uploader",0),2&t&&d.nc("isAdmin",e.isAdmin)("route_path","/videos/")},directives:[Tt.a],encapsulation:2}),rt),Mt=((at=function t(){_classCallCheck(this,t)}).\u0275mod=d.Kb({type:at}),at.\u0275inj=d.Jb({factory:function(t){return new(t||at)},imports:[[r.c,v.a,l.g,c.i,c.u,Rt.a]]}),at),Nt=i("v6bp"),zt=((ut=function t(){_classCallCheck(this,t),this.isAdmin=!0}).\u0275fac=function(t){return new(t||ut)},ut.\u0275cmp=d.Gb({type:ut,selectors:[["ng-component"]],decls:1,vars:2,consts:[[3,"isAdmin","route_path"]],template:function(t,e){1&t&&d.Nb(0,"app-movie-uploader",0),2&t&&d.nc("isAdmin",e.isAdmin)("route_path","/videos/")},directives:[Nt.a],encapsulation:2}),ut),Lt=((dt=function t(){_classCallCheck(this,t)}).\u0275mod=d.Kb({type:dt}),dt.\u0275inj=d.Jb({factory:function(t){return new(t||dt)},imports:[[r.c,v.a,l.g,c.i,c.u,Rt.a]]}),dt),Vt=((lt=function t(){_classCallCheck(this,t),this.isAdmin=!0}).\u0275fac=function(t){return new(t||lt)},lt.\u0275cmp=d.Gb({type:lt,selectors:[["ng-component"]],decls:1,vars:3,consts:[[3,"isAdmin","route_path","uploadType"]],template:function(t,e){1&t&&d.Nb(0,"app-movie-uploader",0),2&t&&d.nc("isAdmin",e.isAdmin)("route_path","/videos/")("uploadType",1)},directives:[Nt.a],encapsulation:2}),lt),Dt=((ct=function t(){_classCallCheck(this,t)}).\u0275mod=d.Kb({type:ct}),ct.\u0275inj=d.Jb({factory:function(t){return new(t||ct)},imports:[[r.c,v.a,l.g,c.i,c.u,Rt.a]]}),ct),Ft=i("eXLi"),jt=((bt=function t(){_classCallCheck(this,t),this.isAdmin=!0}).\u0275fac=function(t){return new(t||bt)},bt.\u0275cmp=d.Gb({type:bt,selectors:[["ng-component"]],decls:1,vars:2,consts:[[3,"isAdmin","route_path"]],template:function(t,e){1&t&&d.Nb(0,"app-aws-uploader",0),2&t&&d.nc("isAdmin",e.isAdmin)("route_path","/videos/")},directives:[Ft.a],encapsulation:2}),bt),Ut=i("Hk5R"),Pt=((mt=function t(){_classCallCheck(this,t),this.isAdmin=!0}).\u0275fac=function(t){return new(t||mt)},mt.\u0275cmp=d.Gb({type:mt,selectors:[["ng-component"]],decls:1,vars:2,consts:[[3,"isAdmin","route_path"]],template:function(t,e){1&t&&d.Nb(0,"app-general-uploader",0),2&t&&d.nc("isAdmin",e.isAdmin)("route_path","/videos/")},directives:[Ut.a],encapsulation:2}),mt),xt=((ft=function t(){_classCallCheck(this,t)}).\u0275mod=d.Kb({type:ft}),ft.\u0275inj=d.Jb({factory:function(t){return new(t||ft)},imports:[[r.c,v.a,l.g,c.i,c.u,Rt.a]]}),ft),Jt=((ht=function t(){_classCallCheck(this,t)}).\u0275mod=d.Kb({type:ht}),ht.\u0275inj=d.Jb({factory:function(t){return new(t||ht)},imports:[[r.c,v.a,l.g,c.i,c.u,Rt.a]]}),ht),Kt=((pt=function t(){_classCallCheck(this,t)}).\u0275mod=d.Kb({type:pt}),pt.\u0275inj=d.Jb({factory:function(t){return new(t||pt)},imports:[[r.c,v.a,l.g,c.i,Rt.a]]}),pt),Et=i("+CAv"),$t=((gt=function t(){_classCallCheck(this,t),this.isAdmin=!0}).\u0275fac=function(t){return new(t||gt)},gt.\u0275cmp=d.Gb({type:gt,selectors:[["ng-component"]],decls:1,vars:2,consts:[[3,"isAdmin","route_path"]],template:function(t,e){1&t&&d.Nb(0,"app-video-proc",0),2&t&&d.nc("isAdmin",e.isAdmin)("route_path","/my-videos/")},directives:[Et.a],encapsulation:2}),gt),Ht=((vt=function t(){_classCallCheck(this,t)}).\u0275mod=d.Kb({type:vt}),vt.\u0275inj=d.Jb({factory:function(t){return new(t||vt)},imports:[[r.c,v.a,l.g,c.i,Rt.a]]}),vt),Bt=i("6Fzr"),Yt=((yt=function t(){_classCallCheck(this,t),this.isAdmin=!0}).\u0275fac=function(t){return new(t||yt)},yt.\u0275cmp=d.Gb({type:yt,selectors:[["ng-component"]],decls:1,vars:2,consts:[[3,"isAdmin","route_path"]],template:function(t,e){1&t&&d.Nb(0,"app-video-updatethumb",0),2&t&&d.nc("isAdmin",e.isAdmin)("route_path","/my-videos/")},directives:[Bt.a],encapsulation:2}),yt),Xt=i("OPJD"),Wt=((It=function t(){_classCallCheck(this,t)}).\u0275mod=d.Kb({type:It}),It.\u0275inj=d.Jb({factory:function(t){return new(t||It)},providers:[p.a,h.a,f.a,m.a],imports:[[r.c,v.a,l.g,c.i,Xt.b]]}),It);function qt(t,e){1&t&&(d.Sb(0,"div"),d.Sb(1,"h2",1),d.Ic(2,"Access Denied"),d.Rb(),d.Rb())}function Qt(t,e){1&t&&d.Nb(0,"app-loader")}function Zt(t,e){if(1&t&&(d.Sb(0,"div"),d.Nb(1,"google-chart",5),d.Rb()),2&t){var i=d.gc(2);d.zb(1),d.nc("data",i.tooltipChart)}}function te(t,e){if(1&t&&(d.Sb(0,"div"),d.Sb(1,"div",6),d.Sb(2,"h3",7),d.Ic(3),d.Rb(),d.Rb(),d.Rb()),2&t){var i=d.gc(2);d.zb(3),d.Kc(" ",i.message," ")}}function ee(t,e){if(1&t){var i=d.Tb();d.Sb(0,"div"),d.Sb(1,"div",2),d.Sb(2,"div",3),d.Sb(3,"app-toolbar-v2",4),d.ec("Action",(function(t){return d.yc(i),d.gc().toolbaraction(t)})),d.Rb(),d.Gc(4,Qt,1,0,"app-loader",0),d.Gc(5,Zt,2,1,"div",0),d.Gc(6,te,4,1,"div",0),d.Rb(),d.Rb(),d.Rb()}if(2&t){var n=d.gc();d.zb(3),d.nc("isAdmin",n.isAdmin)("Options",n.ToolbarOptions),d.zb(1),d.nc("ngIf",n.showLoader),d.zb(1),d.nc("ngIf",!n.showLoader&&n.optionSelected),d.zb(1),d.nc("ngIf",!n.showLoader&&!n.optionSelected)}}var ie,ne=((ie=function(){function t(e,i,n,o,s,a,r,c,l){_classCallCheck(this,t),this.settingService=e,this.dataService=i,this.coreService=n,this.coreActions=o,this.actions=s,this.route=a,this.formService=r,this.permission=c,this.router=l,this.FilterOptions={},this.showLoader=!1,this.formHeading="Video Reports",this.isAdmin=!0,this.optionSelected=!1,this.ChartType="ColumnChart",this.message="Please select report type to generate!",this.isAccessGranted=!1,this.isActionGranded=!1,this.tooltipChart={chartType:"ColumnChart",dataTable:[],options:{title:"Videos Uploaded",legend:"none",width:1e3,height:500,is3D:!0}}}return _createClass(t,[{key:"ngOnInit",value:function(){var t=this;this.auth$.subscribe((function(e){t.permission.GrandResourceAccess(!1,"1521396112858","1521396141248",e.Role)&&(t.isAccessGranted=!0,t.permission.GrandResourceAction("1521396112858",e.Role)&&(t.isActionGranded=!0))})),this.filteroptions$.subscribe((function(e){t.FilterOptions=e})),this.ToolbarOptions=this.settingService.getToolbarOptions(0,!0),this.ToolbarOptions.showtoolbar=!0,this.ToolbarOptions.showcheckAll=!1,this.ToolbarOptions.showsecondarytoolbar=!1,this.ToolbarOptions.left_options=this.prepareLeftOptions(),this.ToolbarOptions.right_options=this.prepareRightOptions(),this.ToolbarOptions.right_options[0].title=this.ChartType}},{key:"prepareLeftOptions",value:function(){return[{title:"Select Type",ismultiple:!0,Options:[{id:1,title:"Yearly",value:0,isclick:!0,clickevent:"reporty_type",tooltip:"Generate yearly report"},{id:2,title:"Monthly (Last 12 Months)",value:1,isclick:!0,clickevent:"reporty_type",tooltip:"Generate monthly report"},{id:3,title:"Daily (Current Month)",value:2,isclick:!0,clickevent:"reporty_type",tooltip:"Generate daily report"}]}]}},{key:"prepareRightOptions",value:function(){return[{title:"Chart Type",ismultiple:!0,Options:[{id:1,title:"ColumnChart",value:"ColumnChart",isclick:!0,clickevent:"chart_type",tooltip:"Generate Column Chart"},{id:2,title:"BarChart",value:"BarChart",isclick:!0,clickevent:"chart_type",tooltip:"Generate Bar Chart"},{id:3,title:"LineChart",value:"LineChart",isclick:!0,clickevent:"chart_type",tooltip:"Generate Line Chart"},{id:3,title:"PieChart",value:"PieChart",isclick:!0,clickevent:"chart_type",tooltip:"Generate Pie Chart"}]}]}},{key:"toolbaraction",value:function(t){if(this.isActionGranded){switch(t.action){case"chart_type":this.ChartType=t.value,console.log("chart type is "+this.ChartType),this.showLoader=!0,this.ToolbarOptions.right_options[0].title=this.ChartType,this.FilterOptions.chartType=this.ChartType,this.showLoader=!1;break;case"reporty_type":this.FilterOptions.reporttype=t.value}this.LoadReports()}else this.coreActions.Notify({title:"Permission Denied",text:"",css:"bg-danger"})}},{key:"LoadReports",value:function(){var t=this;this.showLoader=!0,this.dataService.LoadReports(this.FilterOptions).subscribe((function(e){t.tooltipChart.chartType=t.ChartType,e.data.dataTable.length>1?(t.tooltipChart.dataTable=e.data.dataTable,t.optionSelected=!0):(t.message="No Data Available!",t.optionSelected=!1),t.showLoader=!1}))}}]),t}()).\u0275fac=function(t){return new(t||ie)(d.Mb(p.a),d.Mb(h.a),d.Mb(C.a),d.Mb(A.a),d.Mb(m.a),d.Mb(l.a),d.Mb(f.a),d.Mb(k.a),d.Mb(l.c))},ie.\u0275cmp=d.Gb({type:ie,selectors:[["ng-component"]],hostVars:1,hostBindings:function(t,e){2&t&&d.Mc("@fadeInAnimation",void 0)},decls:2,vars:2,consts:[[4,"ngIf"],[1,"m-b-40","m-t-40","text-center"],[1,"row"],[1,"col-md-12"],[3,"isAdmin","Options","Action"],[3,"data"],[2,"padding","80px 0px"],[2,"text-align","center"]],template:function(t,e){1&t&&(d.Gc(0,qt,3,0,"div",0),d.Gc(1,ee,7,5,"div",0)),2&t&&(d.nc("ngIf",!e.isAccessGranted),d.zb(1),d.nc("ngIf",e.isAccessGranted))},directives:[r.o,G.a,_.a,Xt.a],encapsulation:2,data:{animation:[S.a]}}),Object(y.a)([Object(I.e)(["videos","filteroptions"])],ie.prototype,"filteroptions$",void 0),Object(y.a)([Object(I.e)(["users","auth"])],ie.prototype,"auth$",void 0),ie);i.d(e,"VideosModule",(function(){return ae}));var oe,se=[{path:"",data:{title:"Videos Management",urls:[{title:"Dashboard",url:"/"},{title:"Videos",url:"/videos"},{title:"Management"}]},component:b},{path:"tag/:tagname",data:{title:"Videos Management",urls:[{title:"Dashboard",url:"/"},{title:"Videos",url:"/videos"},{title:"Management"}]},component:b},{path:"category/:catname",data:{title:"Videos Management",urls:[{title:"Dashboard",url:"/"},{title:"Videos",url:"/videos"},{title:"Management"}]},component:b},{path:"user/:uname",data:{title:"Videos Management",urls:[{title:"Dashboard",url:"/"},{title:"Videos",url:"/videos"},{title:"Management"}]},component:b},{path:"filter/:abuse",data:{title:"Videos Management (Reported Videos)",urls:[{title:"Dashboard",url:"/"},{title:"Videos",url:"/videos"},{title:"Reported Videos"}]},component:b},{path:"profile/:id",data:{title:"Video Information",urls:[{title:"Dashboard",url:"/"},{title:"Videos",url:"/videos"},{title:"Video Information"}]},component:St},{path:"reports",data:{title:"Reports Overview",urls:[{title:"Dashboard",url:"/"},{title:"Videos",url:"/videos"},{title:"Reports Overview"}]},component:ne},{path:"process/:id",data:{title:"My Account",urls:[{title:"My Account",url:"/"},{title:"Videos",url:"/my-videos"},{title:"Edit Video Information"}]},component:$t},{path:"updatethumbnail/:id",data:{title:"My Account",urls:[{title:"My Account",url:"/"},{title:"Videos",url:"/my-videos"},{title:"Update Video Thumbnail"}]},component:Yt},{path:"directuploader/:id",data:{title:"Direct Video Uploaders",urls:[{title:"Dashboard",url:"/"},{title:"Videos",url:"/videos"},{title:"Direct Video Uploader"}]},component:At},{path:"uploads",data:{title:"Upload Videos",urls:[{title:"Dashboard",url:"/"},{title:"Videos",url:"/videos"},{title:"Upload Videos"}]},component:Pt},{path:"youtube",data:{title:"Youtube Uploader",urls:[{title:"Dashboard",url:"/"},{title:"Videos",url:"/videos"},{title:"Upload Youtube Videos"}]},component:_t},{path:"movie",data:{title:"Movie Uploader",urls:[{title:"Dashboard",url:"/"},{title:"Videos",url:"/videos"},{title:"Upload Movie"}]},component:zt},{path:"embed",data:{title:"Embed Movie / Video",urls:[{title:"Dashboard",url:"/"},{title:"Videos",url:"/videos"},{title:"Embed Movie / Video"}]},component:Vt},{path:"ffmpeg",data:{title:"FFMPEG Uploader",urls:[{title:"Dashboard",url:"/"},{title:"Videos",url:"/videos"},{title:"FFMPEG Video Uploader"}]},component:Ot},{path:"aws",data:{title:"AWS Video Uploader",urls:[{title:"Dashboard",url:"/"},{title:"Videos",url:"/videos"},{title:"AWS Upload Videos"}]},component:jt}],ae=((oe=function t(){_classCallCheck(this,t)}).\u0275mod=d.Kb({type:oe}),oe.\u0275inj=d.Jb({factory:function(t){return new(t||oe)},imports:[[r.c,c.i,v.a,g,wt,xt,Gt,Mt,Jt,a.c,Rt.a,Kt,Wt,Dt,Lt,Ht,l.g.forChild(se)]]}),oe)}}]);