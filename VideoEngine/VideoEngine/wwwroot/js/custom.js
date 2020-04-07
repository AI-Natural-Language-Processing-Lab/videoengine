var newURL = window.location.protocol + "://" + window.location.host;
$(function () {

    //$(".flexnav").flexNav();

    /* search panel */
    $(".search-box").on({
        click: function (e) {
            var icon = $(this).data('icon');
            var placeholder = $(this).data('placeholder');
            var search = $(this).data('search');
            $('#select-search-icon').removeClass().addClass('fa ' + icon);
            $(".search-toggle").dropdown('toggle');
            $('.main-search-input').attr("placeholder", "Search " + placeholder + " ...");
            $("#SearchType").val(search);
            return false;
        }
    }, '.search-option');
 
    $(".actionbar").on({
        click: function (e) {
            $("#shw_sign_lgn").show();
            return false;
        }
    }, '.nologincss');

    var tg = 0;
    $(".vdinfo").on({
        click: function (e) {
            console.log('hit');
            if (tg === 0) {
                $(".vinfoico").removeClass('fa-chevron-circle-down');
                $(".vinfoico").addClass('fa-chevron-circle-up');
                $(".vinfotxt").html("show less");
                $(".vsminfo").hide();
                $(".vfullinfo").show();
                tg = 1;
            }
            else {
                $(".vinfoico").removeClass('fa-chevron-circle-up');
                $(".vinfoico").addClass('fa-chevron-circle-down');
                $(".vinfotxt").html("show more");
                $(".vsminfo").show();
                $(".vfullinfo").hide();
                tg = 0;
            }
            return false;
        }
    }, '.vinfobtn');
});

var timer,
count = 1,
cycle = function (el) {
    var s = el.attr('src'),
        root = s.substring(0, s.lastIndexOf('/') + 1);
    count++;
    if (count > 10)
        count = 1;
    var fn = '00' + count;
    if (count >= 10)
        fn = '0' + count;
    el.attr('src', root + "img_" + fn + ".jpg");
};
$('.thumbpreview').hover(function () {
    var $this = $(this);
    cycle($this);
    timer = setInterval(function () { cycle($this); }, 1000);
}, function () {
    clearInterval(timer);
});

$(".videopreview").on("mouseover", function(event) {
    var preview = $(this).data('preview');
    if (preview !== undefined && preview !== null && preview !== "") {
        console.log('preview triggered');
        var id = $(this).data('id');
        var img_id = $(this).data('imgid');
        showPreview(img_id, id, preview);
    }
  }).on('mouseout', function(event) {
      var preview = $(this).data('preview');
      if (preview !== undefined && preview !== null && preview !== "") {
          console.log('preview triggered');
          var id = $(this).data('id');
          var img_id = $(this).data('imgid');
          hidePreview(img_id, id);
      }
});
function showPreview(img_id, id, preview) {
    // $("#" + id).css("display", "none");
    // $("#" + id).css("display", "block");
    $('#' + img_id).hide();
    $('#' + id).show();
    $("#" + id).attr("src", preview);
}
function hidePreview(img_id, id) {
    // $("#" + id).css("display", "none");
    // $("#" + id).css("display", "block");
    $('#' + img_id).show();
    $('#' + id).hide();
}
//* Ajax Related Operations
function Ajax_Process(path, params, id, tp) {
    $.ajax({
        type: tp,
        url: path,
        data: params,
        async: true,
        cache: true,
        success: function (msg) {
            $(id).html(msg);
        }

    });
}

function Ajax_Process_Append(path, params, id, tp) {
    $.ajax({
        type: tp,
        url: path,
        data: params,
        async: true,
        cache: true,
        success: function (msg) {
            $(id).append(msg);
        }
    });
}
function Ajax_Process_PreAppend(path, params, id, tp) {
    $.ajax({
        type: tp,
        url: path,
        data: params,
        async: true,
        cache: true,
        success: function (msg) {
            $(id).prepend(msg);
        }
    });
}
function Ajax_Process_v2(path, params, id, tp, loadingid) {
    $.ajax({
        type: tp,
        url: path,
        data: params,
        async: true,
        cache: true,
        success: function (msg) {
            $(id).html(msg);
            $('#' + loadingid).html('loading');
            ShowHide(2, '#' + loadingid);
        }
    });
}
function Ajax_Process_v3(path, params, id, tp, loadingid) {
    var message = '';
    $.ajax({
        type: tp,
        url: path,
        data: params,
        async: true,
        cache: true,
        success: function (msg) {
            ShowHide(2, '#' + loadingid);
            message = msg;
        }
    });
    return message;
}

/* Process Like | Dislike */
function Process_Advice(path, params, id, actionid, actiontype) {
    //toggle_panel(1, '#shw_lgn');
    // start posting ajax
    Ajax_Process(path, params, id, "GET");
    // disable like or dislike button
    if (actiontype === 0) {
        $(actionid).removeClass("ui-adv-icon-good");
        $(actionid).removeClass("ui-adv-icon-gd_hover");
        $(actionid).addClass("ui-fixed ui-adv-icon-good");

    } else {
        $(actionid).removeClass("ui-adv-icon-bad");
        $(actionid).removeClass("ui-adv-icon-bd_hover");
        $(actionid).addClass("ui-fixed ui-adv-icon-bad");
    }
    // disable action
}
/* Process Abuse Report */
/*function Process_Req(path, params, id, type) {
    toggle_panel(1, '#shw_lgn');
    // start posting ajax
    Ajax_Process(path, params, id, type);
}*/

/* Process Ajax Request With Loading Message*/
function Process_Req(path, params, id, type, loadingid) {
    Display_Message(id, "Loading...", 2, 50);
    //ShowHide(1, '#' + loadingid);
    // start posting ajax
    Ajax_Process_v2(path, params, id, type, loadingid);
}

/* Process Ajax Request with Animation */
function Process_Req_Animate(path, params, id, type, loadingid) {
    //$(id).hide("slide", { direction: "left" }, 500);
    //$(id).show("slide", { direction: "right" }, 500);
    Process_Req(path, params, id, type, loadingid);

}

/* Display processing message */
function Display_Processing(id) {
    $(id).html("<div style='padding:4px 0px;'>Processing....</div>");
}


/* Display Message */
function Display_Message(id, msg, tp, width) {
    switch (tp) {
        case 0:
            $(id).html(return_message('alert-danger', msg));
            break;
        case 1:
            $(id).html(return_message('alert-success', msg));
            break;
        case 2:
            $(id).html(return_message('alert-info', msg));
            break;
    }
}
function Display_Message_Pre(id, msg, tp, width) {
    switch (tp) {
        case 0:
            $(id).prepend(return_message('alert-danger', msg));
            break;
        case 1:
            $(id).prepend(return_message('alert-success', msg));
            break;
        case 2:
            $(id).prepend(return_message('alert-info', msg));
            break;
    }
}
function return_message(cls, msg) {
    return "<div class='alert " + cls + "'><button type='button' class='close' data-dismiss='alert'>&times;</button>" + msg + "</div>"
}
function loadingmessage(id, message) {
    if (message === "")
        $("#" + id).html("");
    else
        $("#" + id).html("<span class='label label-success'>" + message + "</span>");
}

function loadingtext(id) {
    var str = "";
    str += '<div class="modal-dialog">';
    str += '<div class="modal-content">';
    str += '<div class="modal-header">';
    str += '<button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>';
    str += '<h4 class="modal-title" id="actxt">Facebook Login</h4>';
    str += '</div>';
    str += '<div class="model-body">';
    str += '<div class="pd_10">';
    str += "<div class=\"item_pad_4_c\">";
    str += "<img src='..\/images\/loading\/loading7.gif\' style=\"width: 100px;   height: 100px;\" \/>";
    str += "<\/div>";
    str += '<\/div>';
    str += '<\/div>';
    str += "<\/div>";
    str += "<\/div>";

    $(id).html(str);
}
function ShowMsg(id, message, messagetype, icontype) {
    var str = "";
    var message_class = "ui-state-error";
    var icon_message = "Alert";
    var icon_class = "ui-icon-alert";
    switch (messagetype) {
        case 0:
            message_class = "ui-state-error";
            break;
        case 1:
            message_class = "ui-state-highlight";
            break;
    }
    switch (icontype) {
        case 0:
            icon_class = "ui-icon-alert";
            icon_message = "Alert:";
            break;
        case 1:
            icon_class = "ui-icon-info";
            icon_message = "Info:";
            break;
        case 2:
            icon_class = "ui-icon-check";
            icon_message = "Success:";
            break;
    }
    str += "<div class=\"item_pad_2 ui-corner-all\"><div class=\"" + message_class + " ui-corner-all\">";
    str += "<div style=\"float:left; width:85%;\">";
    str += "<p><span class=\"ui-icon " + icon_class + "\" style=\"float: left; margin-right: .3em;\"></span>";
    str += "<strong>" + icon_message + "</strong>";
    str += " " + message + "<\/p><\/div><div style=\"float:right; width:10%; text-align:right;\"><a href=\"javascript:void(0)\" onclick=\"toggle_panel(2,'#" + id + "');\">close<\/a><\/div><div class=\"clear\"><\/div>";
    str += "<\/div><\/div>";
    $('#' + id).html(str);
}

function ConvertSize(size) {
    var csize = "";
    if (size > 1000000000) {
        csize = Math.round(size / 1000000000) + "G";
    } else if (size > 1000000) {
        csize = Math.round(size / 1000000) + "M";
    }
    else if (size > 1000) {
        csize = Math.round(size / 1000) + "K";
    }
    else {
        csize = size + "B";
    }

    return csize;
}

/* New Action Module */
function ProcessLK(handler, params, action, actionid, action_box) {
    $(action_box).html("loading...");
    params = params + "&act=" + action;
    Process_Advice(handler, params, action_box, actionid, action);
    $(action_box).show();
}

function ActProcess(handler, params, action_box) {
    $(action_box).html("loading...");
    Process_Req(handler, params, action_box, 'GET');
    $(action_box).show();
}

function PlstPost(handler, params, action_box) {
    var value = $("#play_list").val();
    if (value === "") {
        Display_Message("#ply_msg", "Select playlist to add video!", 1, 1);
        return;
    }
    Ajax_Process(handler, params + "&val=" + value, action_box, "POST");
    $(action_box).show();
}

function FlagP(handler, params, action_box) {
    var value = $("#abuse_list").val();
    if (value === "") {
        Display_Message("#flg_msg", "Select reason for report!", 1, 1);
        return;
    }
    Ajax_Process(handler, params + "&val=" + value, action_box, "POST");
    $(action_box).show();
}

"use strict";

// Time Process
function timeSince(date) {

    var seconds = Math.floor((new Date() - date) / 1000);

    var interval = Math.floor(seconds / 31536000);

    if (interval > 1) {
        return interval + " years ago";
    }
    interval = Math.floor(seconds / 2592000);
    if (interval > 1) {
        return interval + " months ago";
    }
    interval = Math.floor(seconds / 86400);
    if (interval > 1) {
        return interval + " days ago";
    }
    interval = Math.floor(seconds / 3600);
    if (interval > 1) {
        return interval + " hours ago";
    }
    interval = Math.floor(seconds / 60);
    if (interval > 1) {
        return interval + " minutes ago";
    }
    return Math.floor(seconds) + " seconds ago";
}

/* data actions */

$(function () {
    $(".actionbar").on({
        click: function (e) {
            var path = $(this).data("path");
            var param = $(this).data("param");
            ProcessLK(path, param, 0, '.ilike', ".abox");
            return false;
        }
    }, '.likeact');
    $(".actionbar").on({
        click: function (e) {
            var path = $(this).data("path");
            var param = $(this).data("param");
            ProcessLK(path, param, 1, '.idislike', ".abox");
            return false;
        }
    }, '.dislikeact');
    $(".actionbar").on({
        click: function (e) {
            var path = $(".current-rating").data("path");
            var params = $(".current-rating").data("param");
            var value = $(this).data("value");
            ActProcess(path, params + "&act=0&val=" + value, ".abox");
            // update rating
            var total_rating = $(".current-rating").data("totalratings");
            var ratings = $(".current-rating").data("ratings");
            total_rating++;
            ratings = ratings + value;
            var avg = Math.floor(ratings / total_rating) * 24;

            $(".current-rating").css('width', avg + 'px');
            return false;
        }
    }, '.rcss');
    $(".actionbar").on({
        click: function (e) {
            var path = $(this).data("path");
            var params = $(this).data("param");
            ActProcess(path, params, ".abox");
            return false;
        }
    }, '.favact');

    $(".actionbar").on({
        click: function (e) {
            var path = $(this).data("path");
            var params = $(this).data("param");
            ActProcess(path, params, ".abox");
            return false;
        }
    }, '.flagact');

    $(".actionbar").on({
        click: function (e) {
            var path = $(this).data("path");
            var params = $(this).data("param");
            ActProcess(path, params, ".abox");
            return false;
        }
    }, '.ishare');
    $(".actionbar").on({
        click: function (e) {
            var path = $(this).data("path");
            var params = $(this).data("param");
            ActProcess(path, params, ".abox");
            return false;
        }
    }, '.iembed');
    $(".actionbar").on({
        click: function (e) {
            var path = $(this).data("path");
            var params = $(this).data("param");

            ActProcess(path, params, ".abox");
            return false;
        }
    }, '.istats');

    $(".actionbar").on({
        click: function (e) {
            var path = $(this).data("path");
            var params = $(this).data("param");
            ActProcess(path, params, ".abox");
            return false;
        }
    }, '.plyact');

    $("#vsk_action_mod").on({
        click: function (e) {
            var path = $(this).data("path");
            var params = $(this).data("param");
            PlstPost(path, params, ".abox");
            return false;
        }
    }, '.ply_sbt');

    $("#vsk_action_mod").on({
        click: function (e) {
            var path = $(this).data("path");
            var params = $(this).data("param");
            FlagP(path, params, ".abox");
            return false;
        }
    }, '.flag_sbt');
    /* $("#vsk_action_mod").on({
         click: function (e) {
             var path = $(this).data("path");
             var params = $(this).data("param");
             ActProcess(path, params, ".abox");
             return false;
         }
     }, '.iembed'); */
    $("#vsk_action_mod").on({
        click: function (e) {
            $(".abox").hide();
            $("#shw_sign_lgn").hide();
            return false;
        }
    }, '#aclose');

    $(".actionbar").on({
        click: function (e) {
            var id = $(this).data("destination");
            var msg = "Please <strong><a href='/signin'>Sign In</a> or <strong><a href='/signup'>Sign Up</a> to complete this action!";
            Display_Message('#actionbar_msg', msg, 0);
            return false;
        }
    }, '.nologincss');
});

function ShowHide(index, pnl) {
    switch (index) {
        case 1:
            $(pnl).show();
            break;
        case 2:
            $(pnl).hide();
            break;
    }
}