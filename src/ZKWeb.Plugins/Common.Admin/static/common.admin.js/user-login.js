$(function () {
	$("[name=UserLoginForm]").on("success", function () {
		var url = $(this).find("[name=RedirectAfterLogin]").val();
		setTimeout(function () { location.href = url; }, 1500);
	});
});
