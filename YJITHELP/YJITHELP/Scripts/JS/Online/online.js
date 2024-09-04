$(function () {	

	if (_fnToNull($("#Session_PICID").val()) == "") {
		window.location = window.location.origin;
	}

	// 현재 날짜를 YYYY-MM-DD 형식으로 설정
	var today = new Date();
	var day = String(today.getDate()).padStart(2, '0');
	var month = String(today.getMonth() + 1).padStart(2, '0');
	var year = today.getFullYear();
	var currentDate = year + '-' + month + '-' + day;

	// 종료일에 현재 날짜 설정
	$('#TODT').val(currentDate);

	// 1주일 전 날짜 계산
	var lastWeekDate = new Date();
	lastWeekDate.setDate(today.getDate() - 7);
	var lastWeekDay = String(lastWeekDate.getDate()).padStart(2, '0');
	var lastWeekMonth = String(lastWeekDate.getMonth() + 1).padStart(2, '0');
	var lastWeekYear = lastWeekDate.getFullYear();
	var startDateValue = lastWeekYear + '-' + lastWeekMonth + '-' + lastWeekDay;

	// start_date에 1주일 전 날짜 설정
	$('.start_date .date').val(startDateValue);

	// 날짜 선택기 설정 함수
	function initializeDatePicker(selector, options) {
		$(selector).datetimepicker({
			timepicker: false,
			format: 'Y-m-d',
			...options
		});
	}

	// 달력플러그인 Type1 - 단독
	var calDate = $(".cal_date");
	if (calDate.length > 0) {
		calDate.each(function (index, item) {
			var $this = $(this);
			initializeDatePicker($this, {
				onSelectDate: function (dp, $input) {
					var str = $input.val();
					$this.find(".date").val(str.substr(0, 10));
				}
			});
		});
	}

	// 달력플러그인 Type2 - 시작일~종료일
	var sDate = $(".start_date");
	var eDate = $(".end_date");

	if (sDate.length > 0) {
		initializeDatePicker(sDate, {
			onShow: function (ct) {
				this.setOptions({
					maxDate: eDate.find(".date").val() || false
				});
			},
			onSelectDate: function (dp, $input) {
				sDate.find(".date").val($input.val().substr(0, 10));
			}
		});
	}

	if (eDate.length > 0) {
		initializeDatePicker(eDate, {
			onShow: function (ct) {
				this.setOptions({
					minDate: sDate.find(".date").val() || false
				});
			},
			onSelectDate: function (dp, $input) {
				eDate.find(".date").val($input.val().substr(0, 10));
			}
		});
	}

	var getParameter = function (param) {
		var returnValue = '';
		// 파라미터 파싱
		var url = location.href;
		var params = (url.slice(url.indexOf('?') + 1, url.length)).split('&');
		for (var i = 0; i < params.length; i++) {
			var varName = params[i].split('=')[0];
			//파라미터 값이 같으면 해당 값을 리턴한다
			if (varName.toUpperCase() == param.toUpperCase()) {
				returnValue = _fnToNull(params[i].split('=')[1]);
				if (returnValue == "") {
					returnValue = "none"
				}
				return decodeURIComponent(returnValue);
			}
		}
		return returnValue;
	}

	var objJsonData = new Object();
	if (getParameter("userId") != "") {
		objJsonData.CUST_CD = getParameter('userId');
		objJsonData.CRN = getParameter('userPw');
		objJsonData.EMAIL = getParameter('email');
		objJsonData.TEL = getParameter('tel');
		objJsonData.PICNM = getParameter('picnm');

		$.ajax({
			type: "GET",
			url: "/Online/AutoUserLogin",
			async: false,
			dataType: "json",
			data: { "vjsonData": _fnMakeJson(objJsonData) },
			success: function (result) {
				if (result.Result[0].trxCode == "Y") {
					$("#Session_CUST_CD").val(getParameter('userId'));
					$("#Session_EMAIL").val(getParameter('email'));
					$("#Session_TEL").val(getParameter('tel'));
					$("#Session_PICNM").val(getParameter('picnm'));
				} else {
					_fnAlertMsg("양재아이티로 문의주시기 바랍니다.(1522-7422)")
				}
			}
		})
	} else {
		if ($("#Session_CUST_CD").val() == "") {
			location.href = "/";
		}
    }

	

	//업무구분1 가져오기
	$.ajax({
		type: "POST",
		url: "/Online/SearchList",
		async: false,
		dataType: "json",
		success: function (result) {
			if (result.Result[0].trxCode == "Y") {
				fnMakeSelectList(result);
			} else {
				alert('ㄴㄴ');
			}
		}
	})

	//업무구분2 가져오기

	var objJsonData = new Object();
	objJsonData.REQ_SVC = $("#REQ_SVC").val();
	$.ajax({
		type: "POST",              
		url: "/Online/SearchList2",
		async: false,
		dataType: "json",
		data: { "vjsonData": _fnMakeJson(objJsonData) },
		success: function (result) {
			if (result.Result[0].trxCode == "Y") {
				fnMakeSelectList2(result);
			} else {
				fnMakeSelectList2NoData(result);
			}
		}
	})
	////처리상태 가져오기2
	//var objJsonData = new Object();
	//objJsonData.STATUS = $("#H_STATUS").val();
	//$.ajax({
	//	type: "POST",
	//	url: "/Online/SearchStatus",
	//	async: false,
	//	dataType: "json",
	//	data: { "vjsonData": _fnMakeJson(objJsonData) },
	//	success: function (result) {
	//		if (result.Result[0].trxCode == "Y") {
	//			fnMakeStatus2(result);
	//		} else {

	//		}
	//	}
	//})
	////처리상태 가져오기
	//var objJsonData = new Object();
	//objJsonData.STATUS = $("#STATUS").val();
	//$.ajax({
	//	type: "POST",
	//	url: "/Online/SearchStatus",
	//	async: false,
	//	dataType: "json",
	//	data: { "vjsonData": _fnMakeJson(objJsonData) },
	//	success: function (result) {
	//		if (result.Result[0].trxCode == "Y") {
	//			fnMakeStatus(result);
	//		} else {
				
	//		}
	//	}
	//})

	if ($("#REQ_SVC").val() == "") {
		$("#REQ_SVC2").val("");
	}
	
	//$("#REQ_SVC2, #STATUS").prop("disabled", true);
});

$(document).on("click", "#R_CloseLayer", function () {
	$("#R_SUBJECT").val("");
	$("#R_CONTENTS").val("");
	$("#R_GridFile").empty();
	$("#Regist .input__box button").hide();
})

$(document).on('change', "#file-uploader", function () {
	//var files = $(this).prop('files');
	//var fileNames = [];

	//var existingNames = $("#FILE_ATTACH").val().split(', ');

	//fileNames = existingNames.filter(name => name !== '첨부파일');

	//for (var i = 0; i < files.length; i++) {
	//	fileNames.push(files[i].name);
	//}

	//$("#FILE_ATTACH").val(fileNames.join(', '));
	const fileList = event.target.files;
	const fileContainer = $('#GridFile');

	// 기존 파일 목록을 지우지 않으려면 empty()를 제거하세요
	// fileContainer.empty(); // 기존 리스트 초기화

	$.each(fileList, function (index, file) {
		const fileItem = $('<div class="upload__list"></div>').text(file.name);

		// 버튼 추가
		const removeButton = $('<button></button>').on('click', function () {
			$(this).parent().remove(); // 해당 파일 항목 삭제
		});

		// 파일 이름과 삭제 버튼을 컨테이너에 추가
		fileItem.append(removeButton).appendTo(fileContainer);
	});
});

$(document).on('change', '#r_file-uploader', function (event) {
	const fileList = event.target.files;
	const fileContainer = $('#R_GridFile');

	// 기존 파일 목록을 지우지 않으려면 empty()를 제거하세요
	// fileContainer.empty(); // 기존 리스트 초기화

	$.each(fileList, function (index, file) {
		const fileItem = $('<div class="upload__list"></div>').text(file.name);

		// 버튼 추가
		const removeButton = $('<button></button>').on('click', function () {
			$(this).parent().remove(); // 해당 파일 항목 삭제
		});

		// 파일 이름과 삭제 버튼을 컨테이너에 추가
		fileItem.append(removeButton).appendTo(fileContainer);
	});
});


$(document).on('click', '#request', function () {
    $(this).attr('checked', true);
    $("#answer").attr('checked', false);
})
$(document).on('click', '#answer', function () {
    $(this).attr('checked', true);
    $("#request").attr('checked', false);
})

$(document).on('click', "#btnSave", function () {
	SaveData();
})
$(document).on('click', "#R_btnSave", function () {
	var 담당자 = $("#R_OP_CD").val();
	var 이메일 = $("#R_EMAIL").val();
	var 업무구분 = $("#R_REQ_SVC").val();
	var 제목 = $("#R_SUBJECT").val();
	var 내용 = $("#R_CONTENTS").val();

	// 필드 값 체크
	if (!담당자) {
		_fnAlertMsg("담당자가 없습니다.");
		return;
	}
	if (!이메일) {
		_fnAlertMsg("이메일이 없습니다.");
		return;
	}
	if (!업무구분) {
		_fnAlertMsg("업무구분이 선택되지 않았습니다.");
		return;
	}
	if (!제목) {
		_fnAlertMsg("제목이 없습니다.");
		return;
	}
	if (!내용) {
		_fnAlertMsg("내용이 없습니다.");
		return;
	}
	SaveData2();
})

function SaveData() {
	var objJsonData = new Object();
	objJsonData.TITLE = $("#TITLE").val();
	objJsonData.SUBJECT = $("#SUBJECT").val();
	objJsonData.TEL = $("#HpNum").val();
	objJsonData.EMAIL = $("#EMAIL").val();
	objJsonData.OP_CD = $("#OP_CD").val();
	objJsonData.MNGT_NO = $("#D_MNGT_NO").val();
	objJsonData.REQ_SVC = $("#REQ_SVC").val();
	objJsonData.REQ_SVC2 = $("#REQ_SVC2").val();
	objJsonData.UserCust = $("#Session_CUST_CD").val();
	objJsonData.PICNM = $("#Session_PICNM").val();
	objJsonData.CONTENT = $("#CONTENTS").val();
	objJsonData.PICID = $("#Session_PICID").val();

	$.ajax({
		type: "POST",
		url: "/Online/SaveOnline",
		async: false,
		dataType: "json",
		data: { "vjsonData": _fnMakeJson(objJsonData) },
		success: function (result) {
			if (result.Result[0].trxCode == "Y") {
				_fnAlertMsg("저장되었습니다.");
				SearchData();
				SearchDetail(result.Table1[0]["MNGT_NO"]);
			} else {
				alert('저장안됨');
			}
		}
	})
}
function SaveData2() {
	try {
		var vfileData;
		vfileData = new FormData(); //Form 초기화

		vfileData.append("TITLE", $("#R_TITLE").val());
		vfileData.append("SUBJECT", $("#R_SUBJECT").val());
		vfileData.append("TEL", $("#R_HpNum").val());
		vfileData.append("EMAIL", $("#R_EMAIL").val());
		vfileData.append("OP_CD", $("#R_OP_CD").val());
		vfileData.append("MNGT_NO", $("#Regist input[name='MNGT_NO']").val());
		vfileData.append("REQ_SVC", $("#R_REQ_SVC").val());
		vfileData.append("REQ_SVC2", $("#R_REQ_SVC2").val());
		vfileData.append("UserCust", $("#Session_CUST_CD").val());
		vfileData.append("PICNM", $("#Session_PICNM").val());
		vfileData.append("CONTENT", $("#R_CONTENTS").val());
		vfileData.append("PICID", $("#Session_PICID").val());

		for (var i = 0; i < $("#r_file-uploader").get(0).files.length; i++) {
			vfileData.append("fileInput", $("#r_file-uploader").get(0).files[i]);
		}

		$.ajax({
			type: "POST",
			url: "/Online/OnlineUpload",
			dataType: "json",
			async: true,
			contentType: false, // Not to set any content header
			processData: false, // Not to process data
			data: vfileData,
			success: function (result, status, xhr) {
				if (result != null) {
					if (JSON.parse(result).Result[0]["trxCode"] == "Y") {
						$("#Regist").hide();
						_fnAlertMsg("접수가 완료 되었습니다.");
						$("#R_OP_CD").val($("#Session_PICNM").val());
						$("#R_HpNum").val($("#Session_TEL").val());
						$("#R_EMAIL").val($("#Session_EMAIL").val());
						$("#R_REQ_SVC").val("");
						$("#R_REQ_SVC2").val("");
						$("#R_SUBJECT").val("");
						$("#R_CONTENTS").val("");
						$("#R_FILE_ATTACH").val("");
						$("#btnSearch").click();
					} else {
						alert('저장안됨');
					}
				}
			},
			error: function (xhr, status, error) {
				alert('서버 오류: ' + error);
			}
		});
	}
	catch (err) {
		console.log("[Error - fnSaveData]" + err.message);
	}
}

function SearchOnline(result) {
	_fnAlertMsg("저장되었습니다.");
}

let copiedData = {};

$(document).on("click", "#CopyOnline", function () {
	// 입력 필드의 값 복사
	copiedData.OP_CD = $("#OP_CD").val();
	copiedData.HpNum = $("#HpNum").val();
	copiedData.EMAIL = $("#EMAIL").val();
	copiedData.STATUS = $("#STATUS").val();
	copiedData.REQ_SVC = $("#REQ_SVC").val();
	copiedData.REQ_SVC2 = $("#REQ_SVC2").val();
	copiedData.SUBJECT = $("#SUBJECT").val();
	copiedData.CONTENTS = $("#CONTENTS").val();

	$("#Regist").show();
	CopyNewRegist();
});

function CopyNewRegist() {

	$("#R_OP_CD").val(copiedData.OP_CD || $("#Session_PICNM").val());  // 복사된 값이 없으면 빈 문자열
	$("#R_HpNum").val(copiedData.HpNum || $("#Session_TEL").val());  // 복사된 값이 없으면 빈 문자열
	$("#R_EMAIL").val(copiedData.EMAIL || $("#Session_EMAIL").val());   // 복사된 값이 없으면 빈 문자열
	$("#R_REQ_SVC").val(copiedData.REQ_SVC || ""); // 복사된 값이 없으면 빈 문자열
	$("#R_SUBJECT").val(copiedData.SUBJECT || ""); // 복사된 값이 없으면 빈 문자열
	$("#R_CONTENTS").val(copiedData.CONTENTS || ""); // 복사된 값이 없으면 빈 문자열
	$("body").addClass("layer_on");
}

$(document).on('click', '#NewRegist', function () {
	$("#R_OP_CD").val($("#Session_PICNM").val());
	$("#R_HpNum").val($("#Session_TEL").val());  
	$("#R_EMAIL").val( $("#Session_EMAIL").val());   
	$("#R_REQ_SVC").val("");
	$("#R_SUBJECT").val(""); 
	$("#R_CONTENTS").val("");

	$("#Regist").fadeIn(200);
	$("body").addClass("layer_on");

		
	var objJsonData = new Object();
	objJsonData.REQ_SVC = $("#R_REQ_SVC").val();
	//objJsonData.REQ_SVC = "T";
	$.ajax({
		type: "POST",
		url: "/Online/SearchList2",
		async: false,
		dataType: "json",
		data: { "vjsonData": _fnMakeJson(objJsonData) },
		success: function (result) {
			if (result.Result[0].trxCode == "Y") {
				fnMakeSelectList2(result, $("#R_REQ_SVC").val());
			} else {
				fnMakeSelectList2NoData(result);
			}
		}
	})
})

$(document).on('keyup', '#HpNum, #R_HpNum', function () {
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


//단건 클릭
$(document).on("click", "#OnlineList .list-cont, #MO_OnlineList .list-section-mo__list-cont", function () {
	var test = $(this).find($("input[name='MNGT_NO']")).val();
	SearchDetail(test);
	if ($("#STATUS").val() == "F") {
		//$("#DetailOnline input, #DetailOnline select, #DetailOnline textarea").attr('disabled', true);
		$("label").css('cursor', 'unset');
		$(".button__list").addClass("hide");
	} else {
		//$("#DetailOnline input, #DetailOnline select, #DetailOnline textarea").attr('disabled', false);
		$("label").css('cursor', 'pointer');
		$(".button__list").removeClass("hide");
    }
});

$(document).on('change', '#REQ_SVC', function () {
	if ($(this).val() == "") {
		$(this).parent().siblings().children('select').empty();
	} else {
		ReqChange(this);
    }
});

$(document).on('change', '#R_REQ_SVC', function () {
	if ($(this).val() == "") {
		$(this).parent().siblings().children('select').empty();
	} else {
		ReqChange(this);
	}
});

//$(document).on('change', '#REQ_SVC, #R_REQ_SVC', function () {
//	if (_fnToNull($("#REQ_SVC").val()) == "") {
//		$("#REQ_SVC2 option").empty();
//	}
//	if (_fnToNull($("#R_REQ_SVC").val()) == "") {
//		$("#R_REQ_SVC2 option").empty();
//	}
//	ReqChange(this);
//});

function ReqChange(req) {
	var objJsonData = new Object();
	objJsonData.REQ_SVC = $(req).val();

	$.ajax({
		type: "POST",
		url: "/Online/SearchList2",
		async: false,
		dataType: "json",
		data: { "vjsonData": _fnMakeJson(objJsonData) },
		success: function (result) {
			if (result.Result[0].trxCode == "Y") {
				fnMakeSelectList2(result, req);
				/*$("#REQ_SVC2").prop("disabled", false);*/
				$(req).parent().siblings().children('select').prop("disabled", false);
			} else {
				$("#REQ_SVC2").empty();
			}
		}
	})
}

//서브화면 데이터 바인딩
function SearchDetail(mngt_no) {
	var objJsonData = new Object();
	objJsonData.UserCust = $("#Session_CUST_CD").val();
	objJsonData.G_STATUS = $("#STATUS").val();
	objJsonData.MNGT_NO = mngt_no;
	$("#D_MNGT_NO").val(mngt_no);

	$.ajax({
		type: "POST",
		url: "/Online/SearchDetail",
		async: false,
		dataType: "json",
		data: { "vjsonData": _fnMakeJson(objJsonData) },
		success: function (result) {
			if (result.Result[0].trxCode == "Y") {
				//업무구분2 재검색
				fnMakeDetailList(result);
				$("#REQ_SVC2").prop("disabled", false);
			} else {
				$(".no_data").show();
			}
		}
	})
}

// 업무구분 리스트업 쿼리
function fnMakeDetailList(vJsonData) {
	var vResult = vJsonData.Detail;
	var vResult2 = vJsonData.File;
	
	$('#REQ_SVC').val(_fnToNull(vResult[0].REQ_SVC)).attr("selected", "selected");
	$("#OP_CD").val(_fnToNull(vResult[0].OP_CD));
	$("#HpNum").val(_fnToNull(vResult[0].TEL));
	$("#EMAIL").val(_fnToNull(vResult[0].EMAIL));
	$("#SUBJECT").val(_fnToNull(vResult[0].SUBJECT));
	$("#CONTENTS").val(_fnToNull(vResult[0].CONTENTS));
	$("#ANSWER").val(_fnToNull(vResult[0].ANSWER));
	$("#STATUS").val(_fnToNull(vResult[0].G_STATUS)).attr("selected", "selected");;

	

	//서브리스트  그려주는 함수 
	var objJsonData = new Object();
	objJsonData.REQ_SVC = $("#REQ_SVC").val();

	$.ajax({
		type: "POST",
		url: "/Online/SearchList2",
		async: false,
		dataType: "json",
		data: { "vjsonData": _fnMakeJson(objJsonData) },
		success: function (result) {
			if (result.Result[0].trxCode == "Y") {
				fnMakeSelectList2(result);
			} else {
				
			}
		}
	})
	$("#GridFile p").remove();
	var rtnVal = "";
	if (_fnToNull(vResult2) != "") {
		$.each(vResult2, function (i) {
			rtnVal += "<p>";
			rtnVal += "" + _fnToNull(vResult2[i]["FILE_NM"]) + "";
			rtnVal += "							<input type=\"hidden\" name=\"CRMOnlineRS_SEQ\" value=\"" + _fnToNull(vResult2[i]["SEQ"]) + "\" /> ";
			rtnVal += "							<input type=\"hidden\" name=\"CRMOnlineRS_Name\" value=\"" + _fnToNull(vResult2[i]["FILE_NM"]) + "\" /> ";
			rtnVal += "							<input type=\"hidden\" name=\"CRMOnlineRS_Path\" value=\"" + _fnToNull(vResult2[i]["FILE_PATH"]) + "\" /> ";
			rtnVal += "							<input type=\"hidden\" name=\"CRMOnlineRS_MngtNo\" value=\"" + _fnToNull(vResult2[i]["MNGT_NO"]) + "\" /> ";
			rtnVal += "							<input type=\"hidden\" name=\"CRMOnlineRS_FormId\" value=\"" + _fnToNull(vResult2[i]["FORM_ID"]) + "\" /> ";
			rtnVal += "</p>";
		});
		$("#GridFile").append(rtnVal);
	}
}

//파일 다운로드
$(document).on("click", "#GridFile p", function () {

	var vFILE_PATH = $(this).parent().find('input[name=CRMOnlineRS_Path]').val();
	var vFILE_MNGTNO = $(this).parent().find('input[name=CRMOnlineRS_MngtNo]').val();
	var vFILE_SEQ = $(this).parent().find('input[name=CRMOnlineRS_SEQ]').val();
	var vFILE_NM = "";
	var vREAL_FILE_NM = $(this).parent().find('input[name=CRMOnlineRS_Name]').val();
	var vFILE_FORMID = $(this).parent().find('input[name=CRMOnlineRS_FormId]').val();

	if (_fnToNull(vFILE_FORMID) == "OnlineHelp") {
		vFILE_NM = $(this).parent().find('input[name=CRMOnlineRS_Name]').val();
	}
	else if (_fnToNull(vFILE_FORMID) == "") {
		vFILE_NM = vFILE_MNGTNO + "_" + vFILE_SEQ + "_" + $(this).parent().find('input[name=CRMOnlineRS_Name]').val();
	}

	var vURL = "/Online/DownLoad_Files?FILE_NM=" + vFILE_NM + "&FILE_SEQ=" + vFILE_SEQ + "&FILE_PATH=" + vFILE_PATH + "&FILE_MNGTNO=" + vFILE_MNGTNO + "&FILE_FORMID=" + vFILE_FORMID + "&REAL_FILE_NM=" + vREAL_FILE_NM;

	window.location = vURL;
});

//전역변수 : 업무구분 1
var option = "";

//업무구분1 리스트 불러오기
function fnMakeSelectList(vJsonData) {
	var vResult = vJsonData.List;
	$("#REQ_SVC").empty();
	$("#H_REQ_SVC").empty();
	$("#R_REQ_SVC").empty();

	option += "<option value=''>--select--</option>";
	$.each(vResult, function (i) {
		if (i == 0) {
			option += "<option value=\'" + vResult[i]["CODE"] + "\'>" + vResult[i]["NAME"] + "</option>";
		} else {
			option += "<option value=\'" + vResult[i]["CODE"] + "\'>" + vResult[i]["NAME"] + "</option>";
		}
		
	});
	$("#REQ_SVC").append(option);
	$("#H_REQ_SVC").append(option);
	$("#R_REQ_SVC").append(option);
}

//업무구분2 리스트 불러오기
//req = R_REQ_SVC
function fnMakeSelectList2(vJsonData, req) {
	var opt2 = "";
	var vResult = vJsonData.List2;
	if (_fnToNull(req) != "") {
		if ($(req).attr("id") == "REQ_SVC") {
			$("#REQ_SVC2").empty();
		} else {
			$("#R_REQ_SVC2").empty();
		}
	} else {
		$("#R_REQ_SVC2").empty();
		//$("#REQ_SVC2").empty();
	}


	$.each(vResult, function (i) {
		opt2 += "<option value=\'" + vResult[i]["CODE"] + "\'>" + vResult[i]["NAME"] + "</option>";
	});
	if (_fnToNull(req) != "") {
		if ($(req).attr("id") == "REQ_SVC") {
			$("#REQ_SVC2").append(opt2);
		} else {
			$("#R_REQ_SVC2").append(opt2);
		}
	}
	//else {
	//	$("#REQ_SVC2").append(opt2);
	//}
}

//처리상태 리스트 불러오기
function fnMakeStatus(vJsonData) {
	var vResult = vJsonData.Status;
	$("#STATUS option").remove();
	var option = "";
	$.each(vResult, function (i) {
		option += "<option value=\'" + vResult[i]["CODE"] + "\'>" + vResult[i]["NAME"] + "</option>";
	});
	$("#STATUS").append(option);
}
//처리상태 리스트 불러오기2
function fnMakeStatus(vJsonData) {
	var vResult = vJsonData.Status;
	$("#H_STATUS option").remove();
	var option = "";
	$.each(vResult, function (i) {
		option += "<option value=\'" + vResult[i]["CODE"] + "\'>" + vResult[i]["NAME"] + "</option>";
	});
	$("#H_STATUS").append(option);
}
function fnMakeSelectList2NoData(vJsonData) {
	var vResult = vJsonData.List;
	$("#REQ_SVC2 option").remove();
	
	var option = "<option value=\'\'></option>";
	$("#REQ_SVC2").append(option);
}

$(document).on("click", "#btnSearch", function () {
	SearchData();
});


function SearchData() {
	$("#OnlineList").empty();
	$("#MO_OnlineList").empty();

	var objJsonData = new Object();
	var frdt = $("#FRDT").val();
	var todt = $("#TODT").val();
	objJsonData.frdt = frdt.replace(/-/gi, '');
	objJsonData.todt = todt.replace(/-/gi, '');
	objJsonData.ymd = $("input[name='head_toggle']:checked").val();
	objJsonData.L_title = $("#L_TITLE").val();
	objJsonData.title = $("#TITLE").val();
	objJsonData.UserCust = $("#Session_CUST_CD").val();
	objJsonData.status = $("#H_STATUS").val();
	objJsonData.REQ = $("#H_REQ_SVC").val();
	objJsonData.PROC_TYPE = $("input[name='PROC_TYPE']").val();
	


	if (frdt == "" && todt == "") {
		_fnAlertMsg("날짜를 입력해주세요");
	} else if (frdt == "" && todt !== "") {
		_fnAlertMsg("시작일을 입력해주세요");
		$("#FRDT").focus();
	} else if (frdt !== "" && todt == "") {
		_fnAlertMsg("종료일을 입력해주세요");
		$("#TODT").focus();
	} else if (frdt !== "" && todt !== "") {
		$.ajax({
			type: "POST",
			url: "/Online/SearchOnline",
			async: true,
			dataType: "json",
			data: { "vjsonData": _fnMakeJson(objJsonData) },
			success: function (result) {
				if (result.Result[0].trxCode == "Y") {
					$(".no_data").hide();
					fnMakeOnlineList(result);
				} else {
					$(".no_data").show();
				}
			},
			beforeSend: function () {
				$("#ProgressBar_Loading").show(); //프로그래스바
				$('body').addClass("layer_on");
			},
			complete: function () {
				$("#ProgressBar_Loading").hide(); //프로그래스바
				$('body').removeClass("layer_on");
			}
		})
    }
}

function test(vJsonData) {

}

function formatDate(dateString) {
	return dateString.replace(/(\d{4})(\d{2})(\d{2})/, '$1.$2.$3');
}

function fnMakeOnlineList(vJsonData) {
	var vHTML = "";
	try {
		var vResult = vJsonData.Online;
		if (vResult == undefined) {
			$("#NoData").show();
		} else if (vResult.length > 0) {
			vHTML = "";
			$.each(vResult, function (i) {
				vHTML += "<div class='list-cont'>";
				vHTML += "	<div class='list'>";
				vHTML += "		<input name='MNGT_NO' value='" + _fnToNull(vResult[i]["MNGT_NO"]) + "' style='position:absolute; visibility:hidden;'/>";
				vHTML += "		<input name='PROC_TYPE' value='" + _fnToNull(vResult[i]["PROC_TYPE"]) + "' style='position:absolute; visibility:hidden;'/>";
				vHTML += "		<input value='" + _fnToNull(vResult[i]["REQ_SVC"]) + "' style='position:absolute; visibility:hidden;'/>";
				vHTML += "		<p>" + _fnToNull(vResult[i]["BOARD_TITLE"]) + "</p>";
				if (_fnToNull(vResult[i]["G_STATUS"]) == '처리중') {
					vHTML += "		<p class='ing'>" + _fnToNull(vResult[i]["G_STATUS"]) + "</p>";
				} else {				
					vHTML += "		<p name='status'>" + _fnToNull(vResult[i]["G_STATUS"]) + "</p>";
				}
				vHTML += "		<p>" + formatDate(_fnToNull(vResult[i]["INS_DATE"])) + "</p>";
				vHTML += "		<p>" + formatDate(_fnToNull(vResult[i]["ANS_DATE"])) + "</p>";
				vHTML += "	</div>";
				vHTML += "</div>";
			})
			
		}
		$("#OnlineList").append(vHTML);

		vHTML = "";

		if (vResult == undefined) {
			$("#NoData").show();
		} else if (vResult.length > 0) {
			vHTML = "";
			$.each(vResult, function (i) {
				vHTML += "<div class='list-section-mo__list-cont'>";
				vHTML += "		<input name='MNGT_NO' value='" + _fnToNull(vResult[i]["MNGT_NO"]) + "' style='position:absolute; visibility:hidden;'/>";
				vHTML += "		<input name='PROC_TYPE' value='" + _fnToNull(vResult[i]["PROC_TYPE"]) + "' style='position:absolute; visibility:hidden;'/>";
				vHTML += "		<input value='" + _fnToNull(vResult[i]["REQ_SVC"]) + "' style='position:absolute; visibility:hidden;'/>";
				vHTML += "	<div class='list-section-header'>";
				vHTML += "		<p>제목</p>";
				vHTML += "		<p>" + _fnToNull(vResult[i]["BOARD_TITLE"]) + "</p>";
				vHTML += "	</div>";
				vHTML += "	<div class='list-section-body'>";
				vHTML += "		<div class='list-section-body__value'>";
				vHTML += "		<p>처리상태</p>";
				if (_fnToNull(vResult[i]["G_STATUS"]) == '처리중') {
					vHTML += "		<p class='ing'>" + _fnToNull(vResult[i]["G_STATUS"]) + "</p>";
				} else {
					vHTML += "		<p>" + _fnToNull(vResult[i]["G_STATUS"]) + "</p>";
				}
				vHTML += "		</div>";
				vHTML += "		<div class='list-section-body__value'>";
				vHTML += "			<p>요청일</p>";
				vHTML += "			<p>" + formatDate(_fnToNull(vResult[i]["INS_DATE"])) + "</p>";
				vHTML += "		</div>";
				vHTML += "		<div class='list-section-body__value'>";
				vHTML += "			<p>답변일</p>";
				vHTML += "			<p>" + formatDate(_fnToNull(vResult[i]["ANS_DATE"])) + "</p>";
				vHTML += "		</div>";
				vHTML += "	</div>";
				vHTML += "</div>";
			})

		}
		$("#MO_OnlineList").append(vHTML);

	} catch (e) {
		console.log(e.message);
    }
}


function fnMakeMoOnlineList(vJsonData) {
	var vHTML = "";
	try {
		var vResult = vJsonData.Online;
		if (vResult == undefined) {
			$("#NoData").show();
		} else if (vResult.length > 0) {
			vHTML = "";
			$.each(vResult, function (i) {
				vHTML += "<div class='list-section-mo__list-cont'>";
				vHTML += "		<input name='MNGT_NO' value='" + _fnToNull(vResult[i]["MNGT_NO"]) + "' style='position:absolute; visibility:hidden;'/>";
				vHTML += "		<input name='PROC_TYPE' value='" + _fnToNull(vResult[i]["PROC_TYPE"]) + "' style='position:absolute; visibility:hidden;'/>";
				vHTML += "		<input value='" + _fnToNull(vResult[i]["REQ_SVC"]) + "' style='position:absolute; visibility:hidden;'/>";
				vHTML += "	<div class='list-section-header'>";
				vHTML += "		<p>제목</p>";
				vHTML += "		<p>" + _fnToNull(vResult[i]["BOARD_TITLE"]) + "</p>";
				vHTML += "	</div>";
				vHTML += "	<div class='list-section-body'>";
				vHTML += "		<div class='list-section-body__value'>";
				vHTML += "		<p>처리상태</p>";
				if (_fnToNull(vResult[i]["G_STATUS"]) == '처리중') {
					vHTML += "		<p class='ing'>" + _fnToNull(vResult[i]["G_STATUS"]) + "</p>";
				} else {
					vHTML += "		<p>" + _fnToNull(vResult[i]["G_STATUS"]) + "</p>";
				}
				vHTML += "		</div>";
				vHTML += "		<div class='list-section-body__value'>";
				vHTML += "			<p>요청일</p>";
				vHTML += "			<p>" + formatDate(_fnToNull(vResult[i]["INS_DATE"])) + "</p>";
				vHTML += "		</div>";
				vHTML += "		<div class='list-section-body__value'>";
				vHTML += "			<p>답변일</p>";
				vHTML += "			<p>" + formatDate(_fnToNull(vResult[i]["ANS_DATE"])) + "</p>";
				vHTML += "		</div>";
				vHTML += "	</div>";
				vHTML += "</div>";
			})

		}
		$("#MO_OnlineList").append(vHTML);

	} catch (e) {
		console.log(e.message);
	}
}

$(document).on("click", "#SelfHandle", function () {
	SelfHandle();
});

function SelfHandle() {
	var objJsonData = new Object();	
	var str = "고객사 자체처리 완료";
	objJsonData.CONTENT = $("#CONTENTS").val() + "\n" + str;
	objJsonData.MNGT_NO = $("#D_MNGT_NO").val();

	$.ajax({
		type: "POST",
		url: "/Online/SelfOnline",
		async: false,
		dataType: "json",
		data: { "vjsonData": _fnMakeJson(objJsonData) },
		success: function (result) {
			if (result.Result[0].trxCode == "Y") {
				_fnAlertMsg("자체처리 되었습니다.");
				SearchData();
				SearchDetail($("#D_MNGT_NO").val());
			} else {
				alert('저장안됨');
			}
		},
		beforeSend: function () {
			$("#ProgressBar_Loading").show(); //프로그래스바
			$('body').addClass("layer_on");
		},
		complete: function () {
			$("#ProgressBar_Loading").hide(); //프로그래스바
			$('body').removeClass("layer_on");
		}
	})
}