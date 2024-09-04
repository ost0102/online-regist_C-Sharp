$(document).ready(function () {
    $('#CODE').on('keydown', function (event) {
        if (event.key === 'Enter') {
            $("#CRN").focus();
        }
    });
    $('#CRN').on('keydown', function (event) {
        if (event.key === 'Enter') {
            $("#EnConfirm").click();
        }
    });
    $('#PIC_CD').on('keydown', function (event) {
        if (event.key === 'Enter') {
            $("#PIC_RANK").focus();
        }
    });
    $('#PIC_RANK').on('keydown', function (event) {
        if (event.key === 'Enter') {
            $("#PIC_TEL").focus();
        }
    });
    $('#PIC_TEL').on('keydown', function (event) {
        if (event.key === 'Enter') {
            $("#PIC_EMAIL").focus();
        }
    });
    $('#PIC_EMAIL').on('keydown', function (event) {
        if (event.key === 'Enter') {
            $("#PIC_ID").focus();
        }
    });
    $('#PIC_ID').on('keydown', function (event) {
        if (event.key === 'Enter') {
            $("#PIC_PWD").focus();
        }
    });

    $('#PIC_ID, #CODE').on('input', function () {
        // 현재 입력값을 대문자로 변환하고, 대문자 알파벳만 남김
        this.value = this.value.toUpperCase().replace(/[^A-Z]/g, '');
    });
});
var isEnConfirmClicked = false;

function validate() {
    var code = $("#CODE").val().toUpperCase();
    var crn = $("#CRN").val();
    if (code == null || code == "") {
        _fnAlertMsg("세관코드를 입력해 주세요.");
        return false;
    } else if (crn == null || crn == "") {
        _fnAlertMsg("사업자 번호를 입력해주세요.");
        return false;
    }

    // 검색조건
    var objJsonData = new Object();
    objJsonData.CODE = code;
    objJsonData.CRN = crn.replace(/-/gi,'');

    $.ajax({
        type: "POST",
        url: "/Join/SearchCrn",
        async: false,
        dataType: "json",
        data: { "vjsonData": _fnMakeJson(objJsonData) },
        success: function (result) {
            if (result.Result[0].trxCode == "Y") {
                $("#CUST_CD").val(result.Table1[0].CUST_CD);
                $("#OFFICE_CD").val(code);
                $("#PIC_CD").attr("disabled", false);
                $("#PIC_EMAIL").attr("disabled", false);
                $("#PIC_ID").attr("disabled", false);
                $("#PIC_TEL").attr("disabled", false);
                $("#PIC_PWD").attr("disabled", false);
                $("#PIC_RANK").attr("disabled", false);
                $("#CODE").attr("disabled", true);
                $("#CRN").attr("disabled", true);
                $("#PIC_CD").focus();
                isEnConfirmClicked = true;
            } else {
                _fnAlertMsg("세관코드 혹은 사업자번호가 일치하지 않습니다.");
                $("body").addClass("layer_on");
            }
        }, error: function (xhr, status, error) {
            _fnAlertMsg('확인되지 않은 업체입니다.');
        }
    })
}



function UserIn() {
    //JSON 객체 만들기
    var objJsonData = new Object();

    // 검색조건
    objJsonData.PIC_CD = $("#PIC_CD").val();
    objJsonData.PIC_RANK = $("#PIC_RANK").val();
    objJsonData.PIC_TEL = $("#PIC_TEL").val();
    objJsonData.PIC_EMAIL = $("#PIC_EMAIL").val();
    objJsonData.PIC_ID = $("#PIC_ID").val();
    objJsonData.PIC_PWD = $("#PIC_PWD").val();
    objJsonData.OFFICE_CD = $("#OFFICE_CD").val();

    $.ajax({
        type: "POST",
        url: "/Join/fnRegister",
        async: true,
        dataType: "json",
        data: { "vjsonData": _fnMakeJson(objJsonData) },
        success: function (result) {
            if (result.Result[0].trxCode == "Y") {
                _fnAlertMsg2('승인 요청 되었습니다.');
            } else {                
                _fnAlertMsg('승인 요청 실패했습니다.');
            }
        }
    })
}

$(document).on('keydown', '#CRN', function (e) {
    var key = e.charCode || e.keyCode || 0;
    var $text = $(this);

    if (e.ctrlKey && (key === 65 || key === 67 || key === 86)) {
        return true;
    }

    if (key === 8 || key === 9 || key === 46 || key === 37 || key === 39) {
        return true;
    }

    if (key >= 48 && key <= 57) {
       
        if ($text.val().length === 3 || $text.val().length === 6) {
            $text.val($text.val() + '-');
        }
        return true;
    }
    return false;
});


$(document).on('keyup', '#PIC_TEL', function () {
    var input = $(this).val().replace(/\D/g, '');
    var formatted = '';

    if (input.length <= 5) {
        formatted = input.replace(/(\d{2})(\d{1,3})/, '$1-$2');
    } else if (input.length <= 9) {
        formatted = input.replace(/(\d{2})(\d{3})(\d{1,4})/, '$1-$2-$3');
    } else {
        formatted = input.replace(/(\d{3})(\d{4})(\d{1,4})/, '$1-$2-$3');
    }

    $(this).val(formatted);
});


$(document).on('click', '#EnConfirm', function () {
    validate();
});

var emailPattern = /^[a-zA-Z]+[a-zA-Z0-9._%+-]*@[a-zA-Z]+[a-zA-Z0-9.-]*\.[a-zA-Z]{2,}$/;

// 영어만 입력받도록 설정
$(document).on("input", "#PIC_EMAIL", function () {
    let inputValue = $(this).val();
    inputValue = inputValue.replace(/[^a-zA-Z0-9._%+-@]/g, ""); // 이메일 형식에 맞게 수정
    $(this).val(inputValue);
});

$(document).on('click', '#userEn', function () {
    if ($("#CODE").val() == "") {
        _fnAlertMsg("세관코드를 입력해 주세요.");
    }
    else if ($("#CRN").val() == "") {
        _fnAlertMsg("사업자 번호를 입력해 주세요.");
    }
    else if ($("#PIC_CD").val() == "") {
        _fnAlertMsg("담당자 명을 입력해 주세요.");
    }
    else if ($("#PIC_RANK").val() == "Empty") {
        _fnAlertMsg("담당자 직위를 선택해 주세요.");
    }
    else if ($("#PIC_TEL").val() == "") {
        _fnAlertMsg("전화번호를 입력해 주세요.");
    }
    else if ($("#PIC_EMAIL").val() == "") {
        _fnAlertMsg("이메일을 입력해 주세요.");
    }
    else if ($("#PIC_ID").val() == "") {
        _fnAlertMsg("아이디 입력해 주세요.");
    }
    else if ($("#PIC_PWD").val() == "") {
        _fnAlertMsg("비밀번호를 입력해 주세요.");
    }
    else if (!isEnConfirmClicked) {
        _fnAlertMsg("업체를 확인해 주세요.");
    }
    else if (!emailPattern.test($("#PIC_EMAIL").val())) { // 정규식 검증
        _fnAlertMsg("이메일 형식이 옳바르지 않습니다.");
    }
    else if ((($("#CRN, #PIC_CD, #PIC_TEL, #PIC_EMAIL, #PIC_ID, #PIC_PWD").val() !== "") && $("#PIC_RANK").val() !== "Empty") && (isEnConfirmClicked = true)) {
        if ($("#agree").is(":checked") == false) {
            _fnAlertMsg("정보 동의에 체크해 주세요");
        }
        else if ($("#agree").is(":checked") == true) {
            UserIn();
        }
    }
});