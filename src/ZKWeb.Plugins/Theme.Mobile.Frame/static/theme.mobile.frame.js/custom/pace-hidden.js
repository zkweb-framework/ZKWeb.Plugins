/* 页面载入完成后隐藏pace */
$(function () {
	Pace.stop();
	$("body").removeClass("pace-running");
});
