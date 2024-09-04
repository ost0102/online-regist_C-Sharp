////$(function () {
////	var objJsonData = new Object();
////	$.ajax({
////		type: "POST",
////		url: "/Home/SearchElvis",
////		async: true,
////		dataType: "json",
////		data: { "vjsonData": _fnMakeJson(objJsonData) },
////		success: function (result) {
////			if (result.Result[0].trxCode == "Y") {
////				fnMakeElvisList(result);
////			} else {
////				alert('ㄴㄴ');
////			}
////		}
////	})

////	$.ajax({
////		type: "POST",
////		url: "/Home/SearchElvis21",
////		async: true,
////		dataType: "json",
////		data: { "vjsonData": _fnMakeJson(objJsonData) },
////		success: function (result) {
////			if (result.Result[0].trxCode == "Y") {
////				fnMakeElvis21List(result);
////			} else {
////				alert('ㄴㄴ');
////			}
////		}
////	})
////})

//////function fnMakeElvisList(vJsonData) {
//////	var vHTML = "";
//////	try {
//////		var vResult = vJsonData.ELVIS;
//////		if (vResult == undefined) {
//////			alert("등록 자료 없음");
//////		} else if (vResult.length > 0) {
//////			vHTML = "";
//////			$.each(vResult, function (i) {
//////				vHTML += "<div class='down__list'>";
//////				vHTML += "	<a class='down__list__title'>" + _fnToNull(vResult[i]["FILE_NM"]) + "<img src='/Images/down--off.png' class='down--off' /><img src='/Images/down--on.png' class='down--on' /> ";
//////				vHTML += "		<input type='hidden' name='ELIVS_FILE_PATH' value='" + _fnToNull(vResult[i]["FILE_PATH"]) + "'>";
//////				vHTML += "		<input type='hidden' name='ELVIS_FILE_NM' value='" + _fnToNull(vResult[i]["FILE_NM"]) + "'>";
//////				vHTML += "		<input type='hidden' name='ELVIS_FILE_SEQ' value='" + _fnToNull(vResult[i]["SEQ"]) + "'>";
//////				vHTML += "		<input type='hidden' name='ELVIS_FILE_MNGT_NO' value='" + _fnToNull(vResult[i]["MNGT_NO"]) + "'>";
//////				vHTML += "		<input type='hidden' name='ELVIS_FILE_FORMID' value='" + _fnToNull(vResult[i]["FORM_ID"]) + "'>";
//////				vHTML += "	</a>";
//////				vHTML += "</div>";
//////            })
//////		}
//////		$(".down-cont__list.elvis").append(vHTML);
//////	} catch (e) {
//////		console.log(e.message);
//////	}
//////}

//////function fnMakeElvis21List(vJsonData) {
//////	var vHTML = "";
//////	try {
//////		var vResult = vJsonData.ELVIS21;
//////		if (vResult == undefined) {
//////			alert("등록 자료 없음");
//////		} else if (vResult.length > 0) {
//////			vHTML = "";
//////			$.each(vResult, function (i) {
//////				vHTML += "<div class='down__list'>";
//////				vHTML += "	<a class='down__list__title'>" + _fnToNull(vResult[i]["FILE_NM"]) + "<img src='/Images/down--off.png' class='down--off' /><img src='/Images/down--on.png' class='down--on' /> ";
//////				vHTML += "		<input type='hidden' name='ELIVS_FILE_PATH' value='" + _fnToNull(vResult[i]["FILE_PATH"]) + "'>";
//////				vHTML += "		<input type='hidden' name='ELVIS_FILE_NM' value='" + _fnToNull(vResult[i]["FILE_NM"]) + "'>";
//////				vHTML += "		<input type='hidden' name='ELVIS_FILE_SEQ' value='" + _fnToNull(vResult[i]["SEQ"]) + "'>";
//////				vHTML += "		<input type='hidden' name='ELVIS_FILE_MNGT_NO' value='" + _fnToNull(vResult[i]["MNGT_NO"]) + "'>";
//////				vHTML += "		<input type='hidden' name='ELVIS_FILE_FORMID' value='" + _fnToNull(vResult[i]["FORM_ID"]) + "'>";
//////				vHTML += "	</a>";
//////				vHTML += "</div>";
//////			})
//////		}
//////		$(".down-cont__list.21").append(vHTML);
//////	} catch (e) {
//////		console.log(e.message);
//////	}
//////}

////$(document).on("click", ".down__list", function () {
////	var vFILE_PATH = $(this).children('a').find('input[name=ELIVS_FILE_PATH]').val();
////	var vREAL_FILE_NM = $(this).children('a').find('input[name=ELVIS_FILE_NM]').val();
////	var vFILE_NM = "";
////	var vFILE_SEQ = $(this).children('a').find('input[name=ELVIS_FILE_SEQ]').val();
////	var vFILE_MNGTNO = $(this).children('a').find('input[name=ELVIS_FILE_MNGT_NO]').val();
////	var vFILE_FORMID = $(this).children('a').find('input[name=ELVIS_FILE_FORMID]').val();

////	if (_fnToNull(vFILE_FORMID) == "OnlineHelp") {
////		vFILE_NM = $(this).children('a').find('input[name=ELVIS_FILE_NM]').val();
////	}
////	else if (_fnToNull(vFILE_FORMID) == "") {
////		vFILE_NM = vFILE_MNGTNO + "_" + vFILE_SEQ + "_" + $(this).children('a').find('input[name=ELVIS_FILE_NM]').val();
////	}

////	var vURL = "Home/DownLoad_ELVIS_Menual?FILE_NM=" + vFILE_NM + "&FILE_SEQ=" + vFILE_SEQ + "&FILE_PATH=" + vFILE_PATH + "&FILE_MNGTNO=" + vFILE_MNGTNO + "&FILE_FORMID=" + vFILE_FORMID + "&REAL_FILE_NM=" + vREAL_FILE_NM;
////	window.location = vURL;
////})