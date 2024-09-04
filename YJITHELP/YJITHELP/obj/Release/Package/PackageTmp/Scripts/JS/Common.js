$(document).ready(function () {
   

    $('#UserId').on('input', function () {
        // 현재 입력값을 대문자로 변환하고, 대문자 알파벳만 남김
        this.value = this.value.toUpperCase().replace(/[^A-Z]/g, '');
    });

    $('#UserId').on('keydown', function (event) {
        if (event.key === 'Enter') {
            $("#UserPwd").focus();
        }
    });
    $('#UserPwd').on('keydown', function (event) {
        if (event.key === 'Enter') {
            $("#Login").focus();
        }
    });

    // 페이지 로드 시 입력 필드의 값 확인
    $('#Regist .input__box input').each(function () {
        if ($(this).val().trim() !== '') {
            // 입력값이 있을 경우 버튼 표시
            $(this).next('.input__box button').css('display', 'block');
        } else {
            // 입력값이 없을 경우 버튼 숨김
            $(this).next('.input__box button').css('display', 'none');
        }
    });

});
$(document).on("click", 'a[name = "LoginRequired"]', function () {
    if (_fnToNull($("#Session_PICID").val()) == "") {
        $('#LoginPop').fadeIn(200);
        $("body").addClass("layer_on");
        $("button[name='CloseLayer']").off('click').on('click', function () {
            $('body').addClass("layer_on2");
        });
    } else {
        location.replace("/Online");
    }
})
// 입력 이벤트 처리
$(document).on('input', '.input__box input[type="text"]', function () {
    if ($(this).val().trim() !== '') {
        $(this).siblings('.input__box button').css('display', 'block');
    } else {
        $(this).siblings('.input__box button').css('display', 'none');
    }
});
function userLogin() {
    var objJsonData = new Object();
    objJsonData.USR_ID = $("#UserId").val();
    objJsonData.PSWD = $("#UserPwd").val();

    $.ajax({
        type: "POST",
        url: "/Home/fnLogin",
        async: false,
        dataType: "json",
        data: { "vJsonData": _fnMakeJson(objJsonData) },
        success: function (result) {
            if (result.Result[0].trxCode == "Y") {
                $.ajax({
                    type: "POST",
                    url: "/Home/SaveLogin",
                    async: false,
                    data: { "vJsonData": _fnMakeJson(result) },
                    success: function (result) {
                        window.location = window.location.origin + "/Online"
                    }
                })
            } else {
                _fnAlertMsg3("아이디 또는 비밀번호가 일치하지 않습니다.");
            }
        }, error: function (xhr) {
            console.log("시스템 사정으로 요청하신 작업을 처리할 수 없습니다.");
            console.log(xhr);
            return;
        }
    });
}

function Mo_userLogin() {


    var objJsonData = new Object();
    objJsonData.USR_ID = $("#Mo_Id").val().toUpperCase();
    objJsonData.PSWD = $("#Mo_Pwd").val();

    $.ajax({
        type: "POST",
        url: "/Home/fnLogin",
        async: false,
        dataType: "json",
        data: { "vJsonData": _fnMakeJson(objJsonData) },
        success: function (result) {

            if (result.Result[0].trxCode == "Y") {

                $.ajax({
                    type: "POST",
                    url: "/Home/SaveLogin",
                    async: false,
                    data: { "vJsonData": _fnMakeJson(result) },
                    success: function (result) {
                        window.location = window.location.origin + "/Online"
                    }
                })
            }
        }, error: function (xhr) {
            console.log("시스템 사정으로 요청하신 작업을 처리할 수 없습니다.");
            console.log(xhr);
            return;
        }
    });
}


function _fnMakeJson(data) {
    if (data != undefined) {
        var str = JSON.stringify(data);
        if (str.indexOf("[") == -1) {
            str = "[" + str + "]";
        }
        return str;
    }
}

//Null 값 ""
function _fnToNull(data) {
    // undifined나 null을 null string으로 변환하는 함수. 
    if (String(data) == 'undefined' || String(data) == 'null') {
        return ''
    } else {
        return data
    }
}

function Logout() {
    $.ajax({
        type: "POST",
        url: "/Home/LogOut",
        async: false,
        success: function (result, status, xhr) {

            $("#Session_PICNM").val("");
            $("#Session_TEL ").val("");
            $("#Session_EMAIL").val("");
            $("#Session_PICID").val("");
            $("#Session_CUST_CD").val("");

            location.replace("/");
        }
    });
}

/* 레이어팝업 */
var layerPopup = function (obj) {
    var $laybtn = $(obj),
        $glayer_zone = $(".layer_zone");
    if ($glayer_zone.length === 0) { return; }
    $("body").addClass("layer_on");   // ★본문스크롤 제거
    $laybtn.fadeIn(200);
};

function _fnAlertMsg(msg, id) {
    $(".alert_cont .inner p").html("");
    $(".alert_cont .inner p").html(msg);
    if (_fnToNull(id) != "") {
        
    } else {
        layerPopup('#alert01');
    }
}

var layerPopup2 = function (obj) {
    var $laybtn = $(obj),
        $glayer_zone = $(".layer_zone");
    if ($glayer_zone.length === 0) { return; }
    $("body").addClass("layer_on");   // ★본문스크롤 제거
    $laybtn.fadeIn(200);
};

function _fnAlertMsg2(msg, id) {
    $(".alert_cont .inner p").html("");
    $(".alert_cont .inner p").html(msg);
    if (_fnToNull(id) != "") {

    } else {
        layerPopup('#alert02');
    }
}

var layerPopup3 = function (obj) {
    var $laybtn = $(obj),
        $glayer_zone = $(".layer_zone");
    if ($glayer_zone.length === 0) { return; }
    $("body").addClass("layer_on");   // ★본문스크롤 제거
    $laybtn.fadeIn(200);
};

function _fnAlertMsg3(msg, id) {
    $("#alert03 .alert_cont .inner p").html("");
    $("#alert03 .alert_cont .inner p").html(msg);
    if (_fnToNull(id) != "") {

    } else {
        layerPopup('#alert03');
    }
}


$(document).on("click", "#Logout", function () {
    Logout();
})
$(document).on("click", "#Mo_Logout", function () {
    Logout();
})



$(document).on('click', 'button[name = "CloseLayer"]', function () {
    $(this).parents('.layer_zone').fadeOut(200);
    
    if ($('#Regist').css("display") == "block") {
        return;
    } else {
        $("body").removeClass("layer_on");
    }
})

$(document).on('click', '#CloseLayer2', function () {
    location.reload();
})

$(document).on('click', '#R_CloseLayer', function () {
    $(this).parents('.layer_zone').fadeOut(200);
    $("body").removeClass("layer_on");
})

$(document).on("click", "#hamburger", function () {
    $('.mo__menu').toggleClass('show');
    $('body').addClass('layer_on');
    $('body').addClass('show');
});
$(document).on("click", "#CloseMenu", function () {
    $('.mo__menu').removeClass('show');
    $('body').removeClass('layer_on');
    $('body').removeClass('layer_on2');
    $('body').removeClass('show');
})

$(document).on("click", ".header__button--user", function () {
    location.href = "/Join";
});

$(document).on("click", "#Help-KOR", function () {
    location.href = "http://down.logis21.com/ETC/TeamViewerQS.zip";
});

$(document).on("click", "#Help-ENG", function () {
    location.href = "http://down.logis21.com/ETC/TeamViewerQS_Eng.zip";
});

$(document).on("click", "#Help-SERVER", function () {
    location.href = "http://down.logis21.com/ETC/TeamViewer_Host_Setup.zip";
});

$(document).on("click", "#Help-ONLINE", function () {
    window.open("https://939.co.kr/yjit", "_black");
});

$(document).on("click", "#Login", function () {
    userLogin();
})

$(document).on("click", "#Mo_Login", function () {
    Mo_userLogin();
})


$(document).on("click", ".input__box button", function () {
    var intBox = $(this).closest(".input__box");
    intBox.find("input[type='text']").val('').focus();
    intBox.find("button").hide();
});


function _fnGetParam(sname) {
    var params = location.search.substr(location.search.indexOf("?") + 1);
    var sval = "";
    params = params.split("&");
    for (var i = 0; i < params.length; i++) {
        temp = params[i].split("=");
        if ([temp[0]] == sname) { sval = temp[1]; }
    }
    return sval;
}