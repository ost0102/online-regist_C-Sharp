$(function () {
	
	var objJsonData = new Object();
	objJsonData.TITLE = $("#TITLE").val();
	
	$.ajax({
		type: "POST",
		url: "/Faq/FaqList",
		async: false,
		dataType: "json",
		data: { "vjsonData": _fnMakeJson(objJsonData) },
		success: function (result) {
			if (result.Result[0].trxCode == "Y") {
				fnMakeFaqList(result);
				viewContentList("G1903080004");
			} else {
				alert('실패');
			}
		}
	})

	if (_fnToNull(sessionStorage.getItem("FAQ")) != "") {
		$(".tab").removeClass("on");
		console.log(_fnToNull(sessionStorage.getItem("FAQ")));
		$("#" + _fnToNull(sessionStorage.getItem("FAQ"))).parent().click();
		sessionStorage.setItem("FAQ", "");
    }
});

function fnMakeFaqList(vJsonData) {
	var vHTML = "";

	try {
		var vResult = vJsonData.FaqList;
		if (vResult == undefined) {
			$("#NoData").show();
		} else if (vResult.length > 0) {
			$(".faq-contents").empty();
			vHTML = "";
			var parent_node_id = _fnToNull(vResult[0]["PARENT_NODE_ID"]);
			$.each(vResult, function (i) {
				if (i == 0) {
					vHTML += "<div class='faq-contents' name=\"" + parent_node_id + "\" >";
					vHTML += "	<div class='faq-contents__header'>";
					vHTML += "		<p>" + _fnToNull(vResult[i]["GRP_NM"]) + "</p>";
					vHTML += "		<div class='faq-search'>";
					vHTML += "			<input type='text' id='sch_" + parent_node_id+"' placeholder='검색하세요'>";
					vHTML += "			<button type='button' class='search_btn' ><img src='/Images/icn_search2.png'></button>";
					vHTML += "		</div>";
					vHTML += "	</div>";

					vHTML += "	<div class='faq-contents__body'>";
					vHTML += "		<div class='no_data'>데이터가 없습니다.</div>";
					vHTML += "		<div class='faq-cont'>";
					vHTML += "			<div class='faq-cont__list' id='" + vResult[i]["NODE_ID"] +"'>";
					vHTML += "				<div class='faq__list__title'>";
					vHTML += "					<p value='" + _fnToNull(vResult[i]["NODE_ID"]) + "'>" + _fnToNull(vResult[i]["TITLE"]) + "</p>"
					vHTML += "				</div>";
					vHTML += "			</div>";
					
				}
				else {
					if (parent_node_id != vResult[i]["PARENT_NODE_ID"]) { // 그룹코드 다를때 새로운영역 시작
						vHTML += "		</div>";
						vHTML += "	</div>";
						vHTML += "</div>";
						parent_node_id = vResult[i]["PARENT_NODE_ID"];
						vHTML += "<div class='faq-contents' name=\"" + parent_node_id + "\" >";
						vHTML += "	<div class='faq-contents__header'>";
						vHTML += "		<p>" + _fnToNull(vResult[i]["GRP_NM"]) + "</p>";
						vHTML += "		<div class='faq-search'>";
						vHTML += "			<input type='text' id='sch_" + parent_node_id +"' placeholder='검색하세요'>";
						vHTML += "			<button type='button' class='search_btn'><img src='/Images/icn_search2.png'></button>";
						vHTML += "		</div>";
						vHTML += "	</div>";

						vHTML += "	<div class='faq-contents__body'>";
						vHTML += "			<div class='no_data'>데이터가 없습니다.</div>";
						vHTML += "		<div class='faq-cont'>";
						vHTML += "			<div class='faq-cont__list' id='" + vResult[i]["NODE_ID"]+"'>";
						vHTML += "				<div class='faq__list__title'>";
						vHTML += "					<p value='" + _fnToNull(vResult[i]["NODE_ID"]) + "'>" + _fnToNull(vResult[i]["TITLE"]) + "</p>"
						vHTML += "				</div>";
						vHTML += "			</div>";
					}
					else { // 같을 때 
						vHTML += "			<div class='faq-cont__list' id='" + vResult[i]["NODE_ID"] +"'>";
						vHTML += "				<div class='faq__list__title'>";
						vHTML += "					<p value='" + _fnToNull(vResult[i]["NODE_ID"]) + "'>" + _fnToNull(vResult[i]["TITLE"]) + "</p>"
						vHTML += "				</div>";
						vHTML += "			</div>";
                    }

                }
					
				//vHTML += "<div class='faq-cont__list'>";
				//vHTML += "	<div class='faq__list__title'>";
				//vHTML += "		<p>" + _fnToNull(vResult[i]["TITLE"]) + "</p>"
				//vHTML += "	</div>";
				//vHTML += "</div>";
			});
		}
		//$(".faq-cont").append(vHTML);
		$(".faq-section2").append(vHTML);

	} catch (e) {
		console.log(e.message);
    }
}

$(document).on("click", ".tab", function () {
	$(".tab").removeClass("on");
	$(this).addClass("on");
	$(".faq-search input").val("");
	$(".search_btn").click();
	$(".no_data").hide();
	var tab_value = $(this).children("input").val();
	viewContentList(tab_value);
});

$(document).on("click", ".faq-cont__list", function () {
	var NODE = $(this).attr("id");
	sessionStorage.setItem("FAQ", $(".tab.on").children("input").attr("id"));
	var GRP = $(this).parents(".faq-contents").attr("name");
	location.href = "/FAQ/View?MNGT=" + NODE + "&GRP="+GRP;
	//goView(NODE);
})



function viewContentList(grp_code) {
	$('.faq-contents').hide();
	$('div [name=' + grp_code + ']').show(); 
}

$(document).on('click', '.search_btn', function () {
	var button = $(this); // 클릭된 버튼 참조
	$(this).parent().parent().siblings().children().children().hide();
	var type = _fnToNull($(this).siblings()[0].id).replace("sch_", "");
	fnSearchFaq(button, type); // 버튼을 인자로 전달
});

$(document).on('keydown', '.faq-search input', function (event) {
	if (event.key === 'Enter') {
		var button = $(this).siblings();
		$(this).parents(".faq-contents__header").siblings().children(".faq-cont").children(".faq-cont__list").hide();
		var type = _fnToNull($(this)[0].id).replace("sch_", "");
		fnSearchFaq(button, type);
	}
});

function fnSearchFaq(button, sch_type) {
	var objJsonData = new Object();

	objJsonData.title = sch_type;
	objJsonData.content = $(button).siblings().val(); // 버튼을 사용하여 값 가져오기

	$.ajax({
		type: "POST",
		url: "/FAQ/SearchFaq",
		async: false,
		dataType: "json",
		data: { "vjsonData": _fnMakeJson(objJsonData) },
		success: function (result) {
			if (result.Result[0].trxCode == "Y") {
				for (var cnt = 0; cnt < result.SearchFaq.length; cnt++) {
					$("#" + result.SearchFaq[cnt].NODE_ID + "").show();
				}
				fnMakeFaqList(result);
				viewContentList(sch_type);
				$(".no_data").hide();
			} else {
				$(".no_data").show();
			}
		}
	});
}