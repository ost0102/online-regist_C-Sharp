$(function () {

	var objJsonData = new Object();
	objJsonData.NOTICE_CS = _fnToNull($("#select_Notice option:selected").val());
	objJsonData.NOTICE_CS_RESULT = _fnToNull($("#input_Notice").val());

	$.ajax({
		type: "POST",
		url: "/Notice/GetNoticeList",
		async: false,
		dataType: "json",
		data: { "vjsonData": _fnMakeJson(objJsonData) },
		success: function (result) {
			if (result.Result[0].trxCode == "Y") {
				fnMakeNoticeList(result);
			} else {
				
			}
		}
	})

});

function fnMakeNoticeList(vJsonData) {
	var vHTML = "";

	try {
		var vResult = vJsonData.NoticeList;
		if (vResult == undefined) {
			$("#NoData").show();
		} else if (vResult.length > 0) {
			vHTML = "";
			$(".notice-contents__body__body").empty();
			$.each(vResult, function (i) {
				vHTML += "<div class='notice-contents__body__body__list'>";
				vHTML += "	<div class='notice-contents__body__body__list-row'>";
				vHTML += "		<input type='hidden' id='" + _fnToNull(vResult[i]["BOARD_ID"]) + "'>";
				vHTML += "		<p name='TITLE'>" + _fnToNull(vResult[i]["TITLE"]) +"</p>";
				vHTML += "		<p name='WRITER'>양재아이티</p>";
				vHTML += "		<p name='W_DATE'>" + _fnToNull(vResult[i]["INS_YMD"]) +"</p>";
				vHTML += "	</div>";
				vHTML += "</div>";
			});
		}
		
		$(".notice-contents__body__body").append(vHTML);

	} catch (e) {
		console.log(e.message);
	}
}

function fnMakeNoticeSearch(vJsonData) {
	var vHTML = "";

	try {
		var vResult = vJsonData.NoticeSearch;
		if (vResult == undefined) {
			$("#NoData").show();
		} else if (vResult.length > 0) {
			vHTML = "";
			$(".notice-contents__body__body").empty();
			$.each(vResult, function (i) {
				vHTML += "<div class='notice-contents__body__body__list'>";
				vHTML += "	<div class='notice-contents__body__body__list-row'>";
				vHTML += "		<input type='hidden' id='" + _fnToNull(vResult[i]["BOARD_ID"]) + "'>";
				vHTML += "		<p name='TITLE'>" + _fnToNull(vResult[i]["TITLE"]) + "</p>";
				vHTML += "		<p name='WRITER'>양재아이티</p>";
				vHTML += "		<p name='W_DATE'>" + _fnToNull(vResult[i]["INS_YMD"]) + "</p>";
				vHTML += "	</div>";
				vHTML += "</div>";
			});
		}

		$(".notice-contents__body__body").append(vHTML);

	} catch (e) {
		console.log(e.message);
	}
}

$(document).on("click", "#btn_Search", function () {	
	fnNoticeSearch();
});

$(document).on("keyup", "#input_Notice", function (e) {
	if (e.keyCode == 13) {
		fnNoticeSearch(0);
	}
});

function fnNoticeSearch() {
	try {
		var objJsonData = new Object();

		objJsonData.NOTICE_CS = _fnToNull($("#select_Notice option:selected").val());
		objJsonData.NOTICE_CS_RESULT = _fnToNull($("#input_Notice").val());

		$.ajax({
			type: "POST",
			url: "/Notice/GetNoticeSearch",
			dataType: "JSON",
			async: false,
			data: { "vJsonData": _fnMakeJson(objJsonData) },
			success: function (result) {
				if (result.Result[0]["trxCode"] == "Y") {
					$("#NoData").hide();
					fnMakeNoticeSearch(result);
				}
				else {
					$(".notice-contents__body__body").empty();
					$("#NoData").show();
                }
			},
			error: function (xhr, status, error) {
				alert("담당자에게 문의 하세요.");
				console.log(error);
				$("#loading-image").hide();
			}, beforeSend: function () {
				$("#loading-image").css("display", "flex"); //프로그래스 바
			},
			complete: function () {
				$("#loading-image").hide(); //프로그래스 바
			}

		});
	}
	catch (err) {
		console.log("[Error - fnNoticeSearch()]" + err.message);
	}
}

$(document).on("click", ".notice-contents__body__body__list", function () {
	var BOARD = $(this).find('input').attr("id");
	location.href = "/Notice/View?No=" + BOARD;
})