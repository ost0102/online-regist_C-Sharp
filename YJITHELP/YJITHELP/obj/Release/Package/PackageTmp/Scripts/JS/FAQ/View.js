$(document).ready(function () {
	// 초기 TD의 nowrap 속성 제거
	$('td[nowrap]').removeAttr('nowrap');
	$('table').removeAttr('width');

	//개발자 도구에선 실행 되지만 코드로는 실행되지 않을 때
	// MutationObserver 설정
	//DOMNodeInserted 구식이벤트, 성능 문제를 일으킬 수 있으므로 사용권장 X 대신 MutationObserver를 사용
	const observer = new MutationObserver(function (mutations) {
		mutations.forEach(function (mutation) {
			if (mutation.type === 'childList') {
				// 새로 추가된 노드에서 nowrap 제거
				$(mutation.addedNodes).find('td[nowrap]').removeAttr('nowrap');
				$(mutation.addedNodes).find('table').removeAttr('width');
			}
		});
	});

	// 감시할 대상을 설정
	observer.observe(document.body, {
		childList: true,
		subtree: true
	});
});

$(function () {	
	var id = _fnGetParam("MNGT");
	var grp = _fnGetParam("GRP");
	if (_fnToNull(id) != "") {
		goView(id, grp);
	}
	else {
    }
});

$(document).on("click", ".faq__list a", function () {
	location.href = '/FAQ';
})

function goView(NODE_ID, PARENT_NODE_ID) {
	var objJsonData = new Object();

	objJsonData.NODE_ID = NODE_ID;
	objJsonData.PARENT_NODE_ID = PARENT_NODE_ID;

	$.ajax({
		type: "POST",
		url: "/Faq/FaqView",
		async: false,
		dataType: "json",
		data: { "vjsonData": _fnMakeJson(objJsonData) },
		success: function (result) {
			if (result.Result[0].trxCode == "Y") {
				fnShowFaq(result);
			} else {

			}
		}
	})
};

function fnShowFaq(vJsonData) {
	//location.href = "/FAQ/view";
	var vHTML = "";
	var currentIndex = 0; // 현재 인덱스
	

	try {
		var vResult = vJsonData.FaqView;
		if (vResult == undefined) {
			$("#NoData").show();
		} 
		else if (vResult.length > 0) {			
			vHTML = "";
			$.each(vResult, function (i) {
				var parent_node_id = _fnToNull(vResult[i]["PARENT_NODE_ID"]);
				if (_fnToNull(vResult[i]["FLAG"]) == "NOW") {
					vHTML += ""
					vHTML += ""
					vHTML += "<div class='faq-section'>";
					vHTML += "	<div class='page__title'>";
					vHTML += "		<p>" + _fnToNull(vResult[i]["GRP_NM"]) + "</p>";
					vHTML += "	</div>";
					vHTML += "	<section class='view-section1'>";
					vHTML += "		<div class='view__detail'>";
					vHTML += "			<div class='view__detail__title'>";
					vHTML += "				<p>" + _fnToNull(vResult[i]["TITLE"]) + "</p>";
					vHTML += "			</div>";
					vHTML += "			<div class='view__detail-body'>";
					vHTML += "				<div class='view__detail-body-cont'>";
					var faq_contet = _fnToNull(vResult[i]["CONTENT"]).replace(/img src=\"/gi, "img src =\"http://110.45.209.46:9632/WCF/FAQ/Data/")
					vHTML += "					" + faq_contet + "";
					vHTML += "				</div>";
					vHTML += "			</div>";
					vHTML += "		</div>";
					vHTML += "	</section>";
					vHTML += "	<section class='view-section2'>";
					vHTML += "		<div class='faq__list'>";
					vHTML += "			<a>목록</a>";
					vHTML += "	  </div>";
					vHTML += "	</section>";
					vHTML += "</div>";
					vHTML += "<section class='view-section3'>"
					vHTML += "	<div class='move__faq' name=\"" + parent_node_id + "\">";

					if (vResult.length == 1) {
						vHTML += "		<div class='move__faq__write prev no_data'>";
						vHTML += "			<p class='move__value'>이전</p>";
						vHTML += "			<p class='move__faq__title'>이전글이 없습니다.</p>";
						vHTML += "		</div>";
						vHTML += "		<div class='move__faq__write next no_data'>";
						vHTML += "			<p class='move__value'>이전</p>";
						vHTML += "			<p class='move__faq__title'>다음글이 없습니다.</p>";
						vHTML += "		</div>";
						vHTML += "	</div>";
						vHTML += "</section>";
                        
					}
					if (vResult.length == 2) {
						if (_fnToNull(vResult[1]["FLAG"]) == "PREV") {
							vHTML += "		<div class='move__faq__write prev' id='" + _fnToNull(vResult[1]["NODE_ID"]) + "'>";
							vHTML += "			<p class='move__value'>이전</p>";
							vHTML += "			<p class='move__faq__title' value='" + _fnToNull(vResult[1]["NODE_ID"]) + "'>" + _fnToNull(vResult[1]["TITLE"]) + "</p>";
							vHTML += "		</div>";
							vHTML += "		<div class='move__faq__write next no_data'>";
							vHTML += "			<p class='move__value'>다음</p>";
							vHTML += "			<p class='move__faq__title'>다음글이 없습니다.</p>";
							vHTML += "		</div>";
							vHTML += "	</div>";
							vHTML += "</section>";
						}
						if (_fnToNull(vResult[1]["FLAG"]) == "NEXT") {
							vHTML += "		<div class='move__faq__write prev no_data'>";
							vHTML += "			<p class='move__value'>이전</p>";
							vHTML += "			<p class='move__faq__title'>이전글이 없습니다.</p>";
							vHTML += "		</div>";
							vHTML += "		<div class='move__faq__write next' id='" + _fnToNull(vResult[1]["NODE_ID"]) + "'>";
							vHTML += "			<p class='move__value'>다음</p>";
							vHTML += "			<p class='move__faq__title' value='" + _fnToNull(vResult[1]["NODE_ID"]) + "'>" + _fnToNull(vResult[1]["TITLE"]) + "</p>";
							vHTML += "		</div>";
							vHTML += "	</div>";
							vHTML += "</section>";
                        }
					}
					else if (vResult.length == 3) {
						vHTML += "		<div class='move__faq__write prev' id='" + _fnToNull(vResult[1]["NODE_ID"]) + "'>";
						vHTML += "			<p class='move__value'>이전</p>";
						vHTML += "			<p class='move__faq__title' value='" + _fnToNull(vResult[1]["NODE_ID"]) + "'>" + _fnToNull(vResult[1]["TITLE"]) + "</p>";
						vHTML += "		</div>";
						vHTML += "		<div class='move__faq__write next' id='" + _fnToNull(vResult[2]["NODE_ID"]) + "'>";
						vHTML += "			<p class='move__value'>다음</p>";
						vHTML += "			<p class='move__faq__title' value='" + _fnToNull(vResult[2]["NODE_ID"]) + "'>" + _fnToNull(vResult[2]["TITLE"]) + "</p>";
						vHTML += "		</div>";
						vHTML += "	</div>";
						vHTML += "</section>";
					}
                }
				
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
	var NODE = $(this).attr("id");
	var GRP = $(this).parent().attr("name");
	location.href = "/FAQ/View?MNGT=" + NODE + "&GRP=" + GRP;
})
