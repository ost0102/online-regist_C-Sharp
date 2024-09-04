$(function () {

	var id = _fnGetParam("No");
	if (_fnToNull(id) != "") {
		goView(id);
	}
	else {
	}
});

function goView(NOTICE_BOARD_ID) {
	var objJsonData = new Object();

	objJsonData.NOTICE_BOARD_ID = NOTICE_BOARD_ID;

	$.ajax({
		type: "POST",
		url: "/Notice/NoticeView",
		async: false,
		dataType: "json",
		data: { "vjsonData": _fnMakeJson(objJsonData) },
		success: function (result) {
			if (result.Result[0].trxCode == "Y") {				
				fnShowNotice(result);
			} else {
				alert('ㄴㄴ');
			}
		}
	})
};

function fnShowNotice(vJsonData) {
	var vHTML = "";

	try {
		var vResult = vJsonData.NoticeView;
		if (vResult == undefined) {
			$("#NoData").show();
		}
		else if (vResult.length > 0) {
			vHTML = "";
			$.each(vResult, function (i) {
				vHTML += "<div class='notice-section'>";
				vHTML += "	<div class='page__title'>";
				vHTML += "		<p>공지사항</p>";
				vHTML += "	</div>";
				vHTML += "	<section class='view-section1'>";
				vHTML += "		<div class='view__detail'>";
				vHTML += "			<div class='view__detail__title'>";
				vHTML += "				<p>" + _fnToNull(vResult[i]["TITLE"]) + "</p>";
				vHTML += "				<div class='title__info'>";
				vHTML += "					<p>양재아이티</p>";
				vHTML += "					<p>" + _fnToNull(vResult[i]["INS_YMD"]) +"</p>";
				vHTML += "				</div>";				
				vHTML += "			</div>";				
				vHTML += "		</div>";
				vHTML += "		<div class='view__detail-body'>";
				vHTML += "			<div class='view__detail-body-cont'>";
				var notice_content = _fnToNull(vResult[i]["BOARD_BODY"]).replace(/img src=\"/gi, "img src =\"http://110.45.209.46:9632/WCF/Notice/Data/")
				vHTML += "					" + notice_content + "";
				vHTML += "			</div>";
				vHTML += "		</div>";
				vHTML += "	</section>";
				vHTML += "	<section class='view-section2'>";
				vHTML += "		<div class='faq__list'>";
				vHTML += "			<a href='/Notice'>목록</a>";
				vHTML += "		</div>";
				vHTML += "	</section>";
				vHTML += "	<section class='view-section3'>";
				vHTML += "		<div class='move__faq'>";
				if (_fnToNull(vResult[i]["PREV_TITLE"]) == "") {
					vHTML += "			<div class='move__faq__write prev no_data'>";
					vHTML += "				<p class='move__value'>이전</p>";
					vHTML += "				<p class='move__faq__title'>이전 글이 없습니다.</p>";
					vHTML += "			</div>";
					vHTML += "			<div class='move__faq__write next' id='" + _fnToNull(vResult[i]["NEXT_BOARD_ID"]) + "'>";
					vHTML += "				<p class='move__value'>다음</p>";
					vHTML += "				<p class='move__faq__title'>" + _fnToNull(vResult[i]["NEXT_TITLE"]) + "</p>";
					vHTML += "			</div>";
				}
				else if (_fnToNull(vResult[i]["NEXT_TITLE"]) == "") {
					vHTML += "			<div class='move__faq__write prev' id='" + _fnToNull(vResult[i]["PREV_BOARD_ID"]) + "'>";
					vHTML += "				<p class='move__value'>이전</p>";
					vHTML += "				<p class='move__faq__title'>" + _fnToNull(vResult[i]["PREV_TITLE"]) + "</p>";
					vHTML += "			</div>";
					vHTML += "			<div class='move__faq__write next no_data'>";
					vHTML += "				<p class='move__value'>다음</p>";
					vHTML += "				<p class='move__faq__title'>다음 글이 없습니다.</p>";
					vHTML += "			</div>";
				}
				else if (_fnToNull(vResult[i]["PREV_TITLE"]) != "" && _fnToNull(vResult[i]["NEXT_TITLE"]) != "") {
					vHTML += "			<div class='move__faq__write prev' id='" + _fnToNull(vResult[i]["PREV_BOARD_ID"]) + "'>";
					vHTML += "				<p class='move__value'>이전</p>";
					vHTML += "				<p class='move__faq__title'>" + _fnToNull(vResult[i]["PREV_TITLE"]) + "</p>";
					vHTML += "			</div>";
					vHTML += "			<div class='move__faq__write next' id='" + _fnToNull(vResult[i]["NEXT_BOARD_ID"]) + "'>";
					vHTML += "				<p class='move__value'>다음</p>";
					vHTML += "				<p class='move__faq__title'>" + _fnToNull(vResult[i]["NEXT_TITLE"]) + "</p>";
					vHTML += "			</div>";
					vHTML += "		</div>";
				}
				vHTML += "</div>";
				
			})
		}
		$(".container").append(vHTML);

	} catch (e) {
		console.log(e.message);
	}
}

$(document).on("click", ".move__faq__write", function () {
	if ($(this).hasClass("no_data")) {
		return;
	}
	var BOARD = $(this).attr("id");
	location.href = "/Notice/View?No=" + BOARD;
})