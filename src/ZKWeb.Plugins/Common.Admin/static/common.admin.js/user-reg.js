﻿$(function () {
	$("[name=UserRegForm]").on("success", function () {
		setTimeout(function () { location.href = "/home"; }, 1500);
	});
});
