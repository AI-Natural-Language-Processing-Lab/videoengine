(function ( $ ) {
    $.fn.jComment = function( options ) {
        var settings = $.extend({
            showAvator: false,
            avatorUrl: "#",
			avatorPictureName: "#",
			defaultAvator: "#",
			avatorWidth: 65,
			contentID: 0,
			contentUrl: "#",
			contentTitle: "",
			contentType: 0,
			userid: "#",
			tComments: 0,
			autoCount: false,
			width: 500,
			postRows: 2,
			placeHolder: "Share your thoughts",
			headingCaption: "All Comments",
			profileUrl: "#",
			avatorCss: "img-responsive img-rounded",
			avatorTooltip: "",
			postBtnCss: "btn btn-xs btn-primary",
			postBtnCaption: "Post",
			loginMsg : "Sign In to Post Comment",
			norecordMsg : "No records found",
			requiredMsg: "Please enter comment",
			reviewMsg: "Your comment has been posted and need to be reivewed",
			loaderMsg: "Loading...",
			rootUrl: "#",
			dirPath: "/apps/jcomments",
			loadHandler: "load",
			processHandler: "handler",
			thumbDirectoryPath: "#",
			isMemberDirectory: true,
			enableEnter: true,
			enablePostBtn: true,
			autoApproved: true,
			isAdmin: false,
			loaderFileName: 'assets/img/loader.gif',
			paginationType: 1,
			paginationLinks: 10,
			selectedPage: 1,
			pageSize: 20,
			showReply: true,
			showDate: true,
			showUser: true,
			replyLevel: 3,
			showLike: true,
			showDislike: true,
			showFlag: true,
			likeStyle: 0,
			authorStatus: false
		}, options );
		$(this).addClass('jcomment');
		$(this).html(preparePost(settings, 0, 0));
		if(settings.autoCount) {
            cCmts(settings, this);
		} else {
			// initialize
			init(settings, this);
		}
		$(this).on({
            click: function (e) {
				var repid = $(this).closest('.jct_post').data('id');
				var level = $(this).closest('.jct_post').data('level');
				cpost(settings, repid, level);
                return false;
            }
        }, '.jbtn');
		$('#jct_list').on({
            click: function (e) {
				var repid = $(this).data('rid');
				var data = $('#pfull_' + repid).html();
                if (data === '')
                    Comments(settings, repid, 1, 0);
				$(this).closest(".creplies").find(".icon-chevron-down").addClass("icon-chevron-up");
				$(this).closest(".creplies").find(".icon-chevron-down").removeClass("icon-chevron-down");
				$(this).removeClass("vrep");
				$(this).addClass("hrep");
				$(this).html('Hide replies');
				$('#p_' + repid).hide();
				$('#pfull_' + repid).show();
                return false;
            }
        }, '.vrep');
		$('#jct_list').on({
            click: function (e) {
				var repid = $(this).data('rid');
				var replies = $(this).data('replies');
				$(this).closest(".creplies").find(".icon-chevron-up").addClass("icon-chevron-down");
				$(this).closest(".creplies").find(".icon-chevron-up").removeClass("icon-chevron-up");
				$(this).removeClass("hrep");
				$(this).addClass("vrep");
				$(this).html('Show all ' + replies + ' replies');
				$('#p_' + repid).show();
				$('#pfull_' + repid).hide();
                return false;
            }
        }, '.hrep');
		$('#jct_list').on({
            click: function (e) {
				cremove(settings, this);
                return false;
            }
        }, '.remove');
		$('#jct_page').on({
            click: function (e) {
				var pid = $(this).data('id');
				settings.selectedPage = pid;
				// reset comment loader section
				$('#jct_list').html('');
				// load comments
				lComments(settings, 0, 0, 0);
                return false;
            }
        }, '.pgcss');
		$('#jct_page').on({
            click: function (e) {
				settings.selectedPage++;
				// load more comments
				lComments(settings, 0, 0, 1);
                return false;
            }
        }, '.lmore');
				
		$('#jct_list').on({
            click: function (e) {
				cvote(settings, this);
                return false;
            }
        }, '.vteh,.vte');
		$('#jct_list').on({
            click: function (e) {
				Display_Message('#jct_msg', settings.loginMsg, 0);
                return false;
            }
        }, '.signvte');
		$('#jct_list').on({
            click: function (e) {
				$(this).addClass('voteerror');
                return false;
            }
        }, '.ownvte');
		
		$('#jct_list').on({
            click: function (e) {
				var repid = $(this).parents().data('id');
				var level = $(this).parents().data('level');
				$('#pst_' + repid).html(preparePost(settings, repid, level));
                return false;
            }
        }, '.preply');
		
		$(this).on({
            keypress: function (e) {
                if (settings.enableEnter) {
                    var keycode = (e.keyCode ? e.keyCode : e.which);
                    if (keycode === '13') {
                        var repid = $(this).closest('.jct_post').data('id');
                        var level = $(this).closest('.jct_post').data('level');
                        cpost(settings, repid, level);
                        return false;
                    }
                }
            }
        }, '.jctpost');

        $('#jct_list').on('mouseenter', ".citem-inner", function () {
            $(this).find(".vote").addClass("voteh");
            $(this).find(".vote").removeClass("vote");
            $(this).find(".remove").show();
            $(this).find("#pflag").show();
        }).on('mouseleave', ".citem-inner", function () {
            $(this).find(".voteh").addClass("vote");
            $(this).find(".voteh").removeClass("voteh");
            $(this).find(".remove").hide();
            $(this).find("#pflag").hide();
        });
    };
	
}( jQuery ));

function init(settings, obj) {
   lComments(settings, 0, 0, 0);
   loader('jct_loader', '', settings, true);
}

function cCmts(settings, obj) {
    loader('jct_loader', 'loading...', settings, true);
	$.ajax({
		type: 'GET',
		url: settings.dirPath + "/" + settings.loadHandler,
		data: 'cid=' + settings.contentID + '&ctp=' + settings.contentType + '&act=cnt',
        success: function (msg) {
            loader('jct_loader', '', settings, true);
            if (msg.indexOf("error") !== -1)
                Display_Message('#jct_msg', msg, 0);
            else {
                var output = msg[0];
                //var output = JSON.parse(msg);
                if (output.status ==='error') {
                    Display_Message('#jct_msg', output.message, 0);
                } else {
                    $('#jct_counter').html(output.comments);
                    settings.tComments = output.comments;
                    //initialize
                    if (output.comments > 0)
                        init(settings, obj);
                    else
                        $('#jct_list').html("<h3>" + settings.norecordMsg + "</h3>");
                }
            }
        },
        error: function (xhr, ajaxOptions, thrownError) {
            Display_Message("#jct_msg", xhr.responseText, 0);
        }
	});
}
function cremove(settings, obj) {
	var id = $(obj).parents().data('id');
	var usr = $(obj).parents().data('user');
	var votes = $(obj).parents().data('votes');
	var csrfparam = '';
    if (usr === settings.userid) {
        $.ajax({
            type: 'GET',
            url: settings.dirPath + '/' + settings.processHandler,
            data: 'cid=' + settings.contentID + '&tp=' + settings.contentType + '&tcmts=' + votes + '&id=' + id + '&usr=' + usr + '&isadm=' + settings.isAdmin + '&act=rem' + csrfparam,
            success: function (msg) {
                var output = msg[0];
                //var output = JSON.parse(msg);
                if (output.status ==='error') {
                    Display_Message('#jct_msg', output.message, 0);
                } else {
                    $('#' + id).hide();
                    Display_Message('#jct_msg', output.message, 1);
                }
            },
            error: function (xhr, ajaxOptions, thrownError) {
                Display_Message("#jct_msg", xhr.responseText, 0);
            }
        });
    }
}

function cvote(settings, obj) {
	var tp = $(obj).data('type');
	var id = $(obj).parents().data('id');
	var votes = $(obj).parents().data('votes');
	var user = $(obj).parents().data('user');
	var act = 0;
	switch(tp)
	{
		case 'vup':
		act = 0;
		break;
		case 'vdown':
		act = 1;
		break;
		case 'flg':
		act = 2;
		break;
	}
	$('.vteh').removeClass('voteactive');
	var csrfparam = '';
	$.ajax({
		type: 'GET',
		url: settings.dirPath + '/' + settings.processHandler,
		data: 'val=' + act + '&id=' + id + '&vt=' + votes + '&usr=' + user + '&isadm=' + settings.isAdmin + '&act=vote' + csrfparam,
        success: function (msg) {
            var output = msg[0];
            if (output.status === 'error') {
                $(obj).addClass('voteerror');
            } else {
                $(obj).addClass('voteactive');
                if (act !== 2) {
                    if (act === 0) {
                        votes++;
                        $('#repcnt_' + id).addClass('voteup');
                    } else {
                        votes--;
                        $('#repcnt_' + id).addClass('votedown');
                    }
                    $('#repcnt_' + id).html(votes);
                }
            }
            $(obj).attr('title', output.message);
            $(obj).popover();
        },
        error: function (xhr, ajaxOptions, thrownError) {
            Display_Message("#jct_msg", xhr.responseText, 0);
        }
	});
}
function cpost(settings, repid, level) {
	if(settings.userid ==='') {
		Display_Message('#jct_msg', settings.loginMsg, 0);
		return;
	} 
	var elid = "jct_post";
    if (repid > 0)
        elid = elid + "_" + repid;
	var data = $('#' + elid).val();
	if(data ==="")
	{
		Display_Message('#jct_msg', settings.requiredMsg, 0);
		return;
	}
	pComments(settings, data, repid, level);
	jCmt_Reset(repid);
}
function preparePost(settings, repid, level) {
	var str = "<div class=\"jct_post\" data-level=\"" + level + "\" data-id=\"" + repid + "\">";
	var pWidth = settings.width;
	
	var cgap = settings.avatorWith + 15;
    if (repid === 0) {
        // message handler	
        str += "<div id=\"jct_msg\"></div>";
        // comment stats
        str += "<div class=\"heading\">" + settings.headingCaption + " (<span id=\"jct_counter\">" + settings.tComments + "</span>)</div>";
    } else {
        if (level > 0)
            cgap = (settings.avatorWith - 1);
        pWidth = pWidth - (cgap * (level + 1));
    }
	// prepare post
    if (!settings.showAvator) {
        pWidth = settings.width;
        if (repid > 0)
            pWidth = pWidth - 8;
        if (level > 0)
            pWidth = pWidth - (cgap * (level) + 10);
    }
	str += "<div class=\"postcss\" style=\"width:" + pWidth + "px;\" ><fieldset>";
	if(settings.showAvator) {
		var tWidth = (settings.width - settings.avatorWith) - 10;
		var lWidth = settings.avatorWidth;
		if(repid > 0)
		{			
			lWidth = lWidth - 25;
			tWidth = pWidth - (lWidth + 10);
		}
		var pUrl = preparerUrl(settings.profileUrl, settings.userid);
		str += "<div><div class=\"pull-left\" style=\"width:" + lWidth + "px;\">\n";
		str += "<a href=\"" + pUrl + "\">\n";
		str += "<img class=\"" + settings.avatorCss + "\" src=\"" + settings.avatorUrl + "\" alt=\"" + settings.avatorTooltip + "\">\n";
		str += "</a>\n";
		str += "</div>"; // close pull-left
		str += "<div class=\"pull-right\" style=\"width:" + tWidth + "px;\">\n";
	}
	var pid = "jct_post";
    if (repid > 0) {
        // remove previous reply post
        $('.postcnt').html('');
        pid = "jct_post_" + repid;
    }
	str += "<label class=\"sr-only\" for=\"" + pid + "\">Post Comment</label>\n";
    str += "<textarea id=\"" + pid + "\" name=\"" + pid + "\" class=\"form-control jctpost\" placeholder=\"" + settings.placeHolder + "\" rows=\"" + settings.postRows + "\"></textarea>\n";
	if(settings.enablePostBtn) {
		str += "<div class=\"btncss\">\n";
		str += "<button id=\"jct_btn\" class=\"" + settings.postBtnCss + " jbtn\">" + settings.postBtnCaption + "</button>\n";
		str += "</div>\n";
	}
	if(settings.showAvator)
		str += "</div></div>"; // close pull-right and div
    str += "</fieldset></div>"; // close post div
	str += "</div>\n"; // close post container
	if(repid ===0) {
		str += "<div style=\"width:" + settings.width + "px;\">";
		// main loader
		str += "<div id=\"jct_loader\"></div>";
		// list
		str += "<div id=\"jct_list\"></div>";
		// small loader
		str += "<div id=\"jct_smld\"></div>";
		// pagination
		str += "<div id=\"jct_page\"></div>";
        str += "</div>";
	}
    return str;
}
function pComments(settings, data, rid, level) {
	loader('jct_loader', 'processing...',settings, false);
	var csrfparam = '';
	$.ajax({
		type: 'POST',
		url: settings.dirPath + '/' + settings.processHandler,
		data: 'val=' + data + '&cid=' + settings.contentID + '&ctp=' + settings.contentType + '&rid=' + rid + '&isadm=' + settings.isAdmin + '&tcmts=' + settings.tComments + '&apr=' + settings.autoApproved + '&level=' + level + '&ausr=' + settings.userid + '&curl=' + settings.contentUrl + '&ctitle=' + settings.contentTitle + '&act=post' + csrfparam,
		dataType: "json",
        success: function (msg) {
            var output = msg[0];
            //var output = JSON.parse(msg);
            loader('jct_loader', '', settings, false);
            if (settings.autoApproved) {
                getComment(output, settings, 1, rid, 0);
            } else
                Display_Message('#jct_msg', settings.reviewMsg, 1);
            updateStats(settings);
        },
        error: function (xhr, ajaxOptions, thrownError) {
            Display_Message("#jct_msg", xhr.responseText, 0);
        }
	});
}
function lComments(settings, repid, isall, lmore) {
	var cPage = settings.selectedPage;
    if (repid > 0)
        cPage = 1;
    if (lmore === 1)
        loader('jct_smld', 'loading comments...', settings, false);
    else if (repid === 0)
        loader('jct_loader', 'loading comments...', settings, false);
	$.ajax({
		type: 'GET',
		url: settings.dirPath + '/' + settings.loadHandler,
		data: 'cid=' + settings.contentID + '&ctp=' + settings.contentType + '&isa=' + settings.showAvator + "&rid=" + repid + "&isall=" + isall + "&ps=" + settings.pageSize + "&p=" + cPage,
		dataType: "json",
        success: function (msg) {
            var cmts = msg[0];
            //var cmts = JSON.parse(msg);
            if (lmore === 1)
                loader('jct_smld', '', settings, false);
            else if (repid === 0)
                loader('jct_loader', '', settings, false);
            if (typeof cmts.status !== 'undefined')
                Display_Message('#jct_msg', cmts.message, 0);
            else if (cmts.length ===0 && repid ===0) {
                $('#jct_list').html("<h3>" + settings.norecordMsg + "</h3>");
            } else {
                for (var i = 0; i < cmts.length; i++) {
                    getComment(cmts[i], settings, 0, repid, isall);
                    if (cmts[i].replies > 0) {
                        lComments(settings, cmts[i].id, isall, 0);
                    }
                }
                // refresh pagination
                if (repid === 0)
                    sPagination(settings);
                $('.vote,.voteh').tooltip();
            }
        },
        error: function (xhr, ajaxOptions, thrownError) {
            Display_Message("#jct_msg", xhr.responseText, 0);
        }
	}); 
}

function updateStats(settings) {
	if(settings.autoApproved) {
		settings.tComments++;
		$('#jct_counter').html(settings.tComments);
	}
}
function getComment(obj, settings, dir, repid, isall) {
	var elid = "jct_list";
    if (isall === 1)
        elid = "pfull_" + repid;
    else if (repid > 0)
        elid = "p_" + repid;
    if (dir === 0)
        $('#' + elid).append(setLayout(obj, settings));
    else
        $('#' + elid).prepend(setLayout(obj, settings));
}
function setLayout(obj, settings) {
   
	var cmargin = "";
	var cgap = 0;
	var diff = 10;
    if (obj.lcount > 0) {
        if (obj.lcount > 1)
            diff = 0;
        cgap = (settings.avatorWidth + diff) * (obj.lcount);
        cmargin = " style=\"margin-left:" + cgap + "px;\"";
    }
	var str = "<div" + cmargin + " id=\"" + obj.id + "\" class=\"citem\">";
	var pUrl = preparerUrl(settings.profileUrl, obj.userid);
	if(settings.showAvator) {
		var tWidth = (settings.width - settings.avatorWith) - 15;
		var lWidth = settings.avatorWidth;
		if(obj.lcount > 0)
		{	
			lWidth = lWidth - 25;
			tWidth = $(".citem").width() - (cgap + lWidth + 5);
		}
		var pName = '';
		var aPath = settings.thumbDirectoryPath;
        if (typeof obj.picturename !== 'undefined') {
            if (obj.picturename !== 'none' && obj.picturename !== '')
                pName = obj.picturename;
        }
        else if (settings.userid === obj.userid)
            pName = settings.avatorPictureName;
		
        if (pName === '')
            aPath = settings.defaultAvator;
        else {
            if (settings.isMemberDirectory)
                aPath += obj.userid + '/';
            aPath += 'photos/thumbs/' + pName;
        }
		str += "<div class=\"pull-left\" style=\"width:" + lWidth + "px;\">\n";
		str += "<a href=\"" + pUrl + "\">\n";
		str += "<img class=\"" + settings.avatorCss + "\" src=\"" + aPath + "\" alt=\"" + settings.avatorTooltip + "\">\n";
		str += "</a>\n";
		str += "</div>"; // close pull-left
		str += "<div class=\"pull-right\" style=\"width:" + tWidth + "px;\">\n";
	}
	str += "<div class=\"citem-inner\" data-id=\"" + obj.id + "\" data-level=\"" + obj.lcount + "\" data-votes=\"" + obj.points + "\" data-user=\"" + obj.userid + "\">\n";
	// author info
    if (settings.showUser) {
        str += "<a href=\"" + pUrl + "\" class=\"author\">" + obj.userid + "</a>\n";
        if (settings.authorStatus) {
            if (obj.userid === settings.userid)
                str += "<span class=\"label label-default\">Author</span>\n";
        }
    }
	// date info
    if (settings.showDate)
        str += "<span class=\"pdate\">" + obj.cdate + "</span>\n";
	// message
    str += "<div class=\"pcomment\">" + obj.message + "</div>";
	// reply link
	if(settings.showReply) {
        if (settings.replyLevel > 0) {
            var level = obj.lcount + 1;
            if (level < settings.replyLevel)
                str += "<a href=\"#\" class=\"preply\">Reply</a>\n";
        } else {
            str += "<a href=\"#\" class=\"preply\">Reply</a>\n";
        }
	}
	// reply counter
    if (obj.points !== 0) {
        var repCss = "voteup";
        if (obj.points < 0)
            repCss = "votedown";
        str += "<span class=\"" + repCss + "\" id=\"repcnt_" + obj.id + "\">" + obj.points + "</span>\n";
    } else {
        // empty placeholder
        str += "<span id=\"repcnt_" + obj.id + "\"></span>\n";
    }
	var acss = 'vte';
	var ownact = '';
    if (settings.userid === '')
        acss = 'signvte';
    else if (settings.userid === obj.userid) {
        acss = 'ownvte';
        ownact = ' data-toggle="tooltip" data-placement="top" title="You can\'t perform this action on your own comment"';
    }
	// thumbs up / down
    if (settings.showLike) {
        if (settings.likeStyle === 0)
            str += "<a" + ownact + " href=\"#\" data-type=\"vup\" class=\"" + acss + " vote\"><i class=\"icon icon-thumbs-up\"></i></a>\n";
        else
            str += "<a" + ownact + " href=\"#\" data-type=\"vup\" class=\"" + acss + " vote\">Like</a>\n";
    }
    if (settings.showDislike) {
        str += "<a" + ownact + " href=\"#\"  data-type=\"vdown\" class=\"" + acss + " vote\"><i class=\"icon icon-thumbs-down\"></i></a>\n";
    }
	// flag
    if (settings.showFlag) {
        str += "<a" + ownact + " style=\"display:none;\" data-type=\"flg\" id=\"pflag\" href=\"#\" class=\"" + acss + " vote\" title=\"report spam or abuse\"><i class=\"icon icon-flag\"></i></a>\n";
    }
	// remove
    if (settings.userid === obj.userid) {
        str += "<a style=\"display:none;\" href=\"#\" class=\"remove\" title=\"remove comment\"><i class=\"icon icon-times\"></i></a>\n";
    }
	str += '</div>\n'; // close citem-inner
	
	// post container
	str += "<div class=\"postcnt\" id=\"pst_" + obj.id + "\"></div>\n";
	if(settings.showAvator)
		str += "</div>"; // close pull-right and div
	str += "<div class=\"clear\"></div>\n";
	str += "</div>";
	// view all reply option
    if (obj.replies > 2 && obj.lcount < 1) {
        cgap = (settings.avatorWidth + diff);
        cmargin = " style=\"margin-left:" + cgap + "px;\"";
        str += "<div class=\"creplies\"><a " + cmargin + " class=\"author vrep\" data-replies=\"" + obj.replies + "\" data-rid=\"" + obj.id + "\" href=\"#\">View all " + obj.replies + " replies</a> <i class=\"icon icon-chevron-down\"></i></a>\n";
    }
	// reply container
	str += "<div class=\"repcnt\" id=\"pfull_" + obj.id + "\"></div>\n";
	str += "<div class=\"repcnt\" id=\"p_" + obj.id + "\"></div>\n";
	return str;
}
function sPagination(settings) {
    if (settings.tComments === 0)
        return "";
    if (settings.paginationType === 3) {
        var totalpages = Math.ceil(settings.tComments / settings.pageSize);
        if (settings.selectedPage < totalpages)
            $('#jct_page').html("<a href=\"#\" class=\"lmore\">Load more</a> <i class=\"icon icon-chevron-down\"></i>\n");
        else
            $('#jct_page').html('');
    } else {
        if (settings.tComments > settings.pageSize)
            $('#jct_page').html(bPg(settings));
    }
}

function bPg(settings) {
  //var arr = genPgLinks(settings);
  var lst = "";
  var firstbound =0;
  var lastbound =0;
  var tooltip = "";
  var showfirst = true;
  var defaultUrl = "#";
  var paginationUrl = "#";
  var fontAwsome = true;
  var prevCss = "";
  var paginationcss = "";
    if (settings.tComments > settings.pageSize) {
        var totalpages = Math.ceil(settings.tComments / settings.pageSize);
        if (settings.selectedPage > 1) {
            if (showfirst && settings.paginationType !== 2) {

                firstbound = 1;
                lastbound = firstbound + settings.pageSize - 1;
                tooltip = "showing " + firstbound + " - " + lastbound + " records of " + settings.tComments + " records";
                // First Link
                backwardIcone = "icon icon-backward";
                if (!fontAwsome)
                    backwardIcon = "glyphicon glyphicon-backward";
                lst += "<li><a data-id=\"1\" href=\"" + defaultUrl + "\" class=\"pgcss\" data-toggle=\"tooltip\" title=\"" + tooltip + "\"><i class=\"" + backwardIcone + "\"></i></a></li>\n";
            }

            firstbound = ((totalpages - 1) * settings.pageSize);
            lastbound = firstbound + settings.pageSize - 1;
            if (lastbound > settings.tComments)
                lastbound = settings.tComments;

            tooltip = "showing " + firstbound + " - " + lastbound + " records of " + settings.tComments + " records";

            // Previous Link Enabled
            var pid = (settings.selectedPage - 1);
            if (pid < 1) pid = 1;
            var prevPageCss = "";
            var leftIcon = "icon icon-chevron-left";
            if (!fontAwsome)
                leftIcon = "glyphicon glyphicon-chevron-left";
            var prevIcon = "<i class=\"" + leftIcon + "\"></i>";
            if (settings.paginationType === 2) {
                if (prevCss !== "")
                    prevPageCss = " class=\"" + prevCss + "\"";
                prevIcon = "&larr; Previous";
            }
            lst += "<li" + prevPageCss + "><a data-id=\"" + pid + "\" href=\"" + paginationUrl + "\" class=\"pgcss\" title=\"" + tooltip + "\">" + prevIcon + "</a></li>\n";

            // Normal Links
            if (settings.paginationType !== 2)
                lst += g_pagination_links(settings, totalpages);

            if (settings.selectedPage < totalpages)
                lst += g_prev_last_links(settings, totalpages);
        }
        else {
            // Normal Links
            if (settings.paginationType !== 2)
                lst += g_pagination_links(settings, totalpages);
            // Next Last Links
            lst += g_prev_last_links(settings, totalpages);
        }
    }
		var paginationCss = "pagination " + paginationcss;
    if (settings.paginationType === 2)
        paginationCss = "pager";
    return "<ul class=\"" + paginationCss + "\">\n" + lst + "</ul>\n";
}

function g_pagination_links(settings, totalpages) {
	var lst = "";
    var firstbound = 0;
    var lastbound = 0;
    var tooltip = "";
	var arr = [];
    arr = genPgLinks(settings, totalpages);
    if (arr.length > 0) {
        for (i = 0; i < arr.length; i++) {
            firstbound = ((arr[i] - 1) * settings.pageSize) + 1;
            lastbound = firstbound + settings.pageSize - 1;
            if (lastbound > settings.tComments)
                lastbound = settings.tComments;
            tooltip = "showing " + firstbound + " - " + lastbound + " records  of " + settings.tComments + " records";
            var css = "";
            if (arr[i] === settings.selectedPage)
                css = " class=\"active\"";
            lst += "<li" + css + "><a data-id=\"" + arr[i] + "\" href=\"#\" class=\"pgcss\" title=\"" + tooltip + "\">" + arr[i] + "</a></li>\n";
        }
    }
    return lst;
}
function g_prev_last_links(settings, totalpages) {
	var fontAwsome = true;
	var nextCss = "";
	var lst = "";
	var showlast = true;
    var firstbound = ((settings.selectedPage) * settings.pageSize) + 1;
    var lastbound =  firstbound +  settings.pageSize - 1;
    if (lastbound > settings.tComments)
       lastbound = settings.tComments;
    var tooltip = "showing " + firstbound + " - " + lastbound + " records of " + settings.tComments + " records";
    // Next Link
	pid = (settings.selectedPage + 1);
	if(pid > totalpages) pid = totalpages;
	var nextPageCss = "";
	var rightIcon = "icon icon-chevron-right";
	if(!fontAwsome)
		rightIcon = "glyphicon glyphicon-chevron-right";
	var nextPageIcon = "<i class=\"" + rightIcon + "\"></i>";
    if (settings.paginationType === 2) {
        if (nextCss !== "")
            nextPageCss = " class=\"" + nextCss + "\"";
        nextPageIcon = "Next &rarr;";
    }
    lst += "<li" + nextPageCss + "><a data-id=\"" + pid + "\" href=\"#\" class=\"pgcss\" data-toggle=\"tooltip\" title=\"" + tooltip + "\">" + nextPageIcon + "</a></li>\n";
    if (showlast && settings.paginationType !== 2) {
        // Last Link
        firstbound = ((totalpages - 1) * settings.pageSize) + 1;
        lastbound = firstbound + settings.pageSize - 1;
        if (lastbound > totalpages)
            lastbound = totalpages;
        tooltip = "showing " + firstbound + " - " + lastbound + " records of " + settings.tComments + " records";
        var forwardIcon = "icon icon-forward";
        if (!fontAwsome)
            forwardIcon = "glyphicon glyphicon-forward";
        lst += "<li><a data-id=\"" + totalpages + "\" href=\"#\" class=\"pgcss\" data-toggle=\"tooltip\" title=\"" + tooltip + "\"><i class=\"" + forwardIcon + "\"></i></a></li>\n";
    }
    return lst;
}
function genPgLinks(settings, totalpages) {
  var i = 0;
  var arr = [];
  if (totalpages < settings.paginationLinks) {
      for (i = 1; i <= totalpages; i++) {
          arr[i - 1] = i;
      }
  } else {
     var lowerbound = settings.selectedPage - Math.floor(settings.paginationLinks / 2);
     var upperbound = settings.selectedPage + Math.floor(settings.paginationLinks / 2);
      if (lowerbound < 1) {
          //calculate the difference and increment the upper bound
          upperbound = upperbound + (1 - lowerbound);
          lowerbound = 1;
      }
     //if upperbound is greater than total page is
      if (upperbound > totalpages) {
          //calculate the difference and decrement the lower bound
          lowerbound = lowerbound - (upperbound - totalpages);
          upperbound = totalpages;
      }
      var counter = 0;
      for (i = lowerbound; i <= upperbound; i++) {
          arr[counter] = i;
          counter++;
      }
  }
  return arr;
}
function getFieldValue(obj) {
    if (obj !== undefined)
        return obj;
    else
        return "";
}
function jCmt_Reset(repid) {
	var pid = "jct_post";
    if (repid > 0) {
        pid = "jct_post_" + repid;
    }
    $('#' + pid).val('');
}
function preparerUrl(url, uname) {
   var patt = new RegExp("\{p\}");
    if (patt.test(url))
        url = url.replace("{p}", uname);
    return url;
}
function loader(id, message, settings, showhide){
    if (message === '') {
        if (showhide)
            $('#jct_list').show();
        $("#" + id).html("");
    }
    else {
        var lcss = "loader";
        if (showhide)
            $('#jct_list').hide();
        else
            lcss = "loadersm";
        $("#" + id).html("<div class=\"" + lcss + "\"><img src=\"" + settings.rootUrl + '' + settings.loaderFileName + "\" /><br />" + message + "</div>");
    }
}