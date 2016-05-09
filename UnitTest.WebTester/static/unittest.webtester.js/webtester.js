/*
	网页单元测试器使用的脚本
	功能
		定时向服务器抓取测试信息
		/admin/unit_test/web_tester?action=fetch

		重置测试结果
		/admin/unit_test/web_tester?action=reset_all

		运行指定项或全部项
		/admin/unit_test/web_tester?action=start&assembly=name
		/admin/unit_test/web_tester?action=start_all
*/

$(function () {
	var $tableContainer = $(".unittest-table-container");
	var $table = $tableContainer.find("table");

	// 点击弹出框时不收回去
	$table.on("click", "pre", function () { return false; });

	// 定时向服务器抓取测试信息
	var lastUpdateds = {}; // { 程序集: 最后更新时间 }
	var getLines = function (message) {
		return _.countBy(message, function (c) { return c == '\n'; }).true || 0;
	};
	var timeoutObject = null;
	var fetchTestInformations = function () {
		clearTimeout(timeoutObject); // 直接调用可重新安排定时抓取
		$.post(
			"/admin/unit_test/web_tester",
			{ action: "fetch", lastUpdateds: JSON.stringify(lastUpdateds) }
		).success(function (data) {
			_.each(data.informations, function (info) {
				var $row = $table.find("tr[data-assembly-name='" + info.AssemblyName + "']");
				$row.find(".state").text(info.StateName);
				$row.find(".passed").text(info.Passed);
				$row.find(".skiped a").text(info.Skiped);
				$row.find(".failed a").text(info.Failed);
				$row.find(".skiped pre").text(info.SkipedMessage || "");
				$row.find(".failed pre").text(info.FailedMessage || "");
				$row.find(".error-message a").text(getLines(info.ErrorMessage));
				$row.find(".error-message pre").text(info.ErrorMessage || "");
				$row.find(".debug-message a").text(getLines(info.ErrorMessage));
				$row.find(".debug-message pre").text(info.DebugMessage || "");
				lastUpdateds[info.AssemblyName] = info.LastUpdated;
			});
		}).always(function () {
			timeoutObject = setTimeout(fetchTestInformations, 2000);
		});
	};
	fetchTestInformations();

	// 绑定重置测试结果的按钮
	$table.on("click", ".reset-all-tests", function () {
		$.post("/admin/unit_test/web_tester", { action: "reset_all" }, function (data) {
			$.handleAjaxResult(data);
			fetchTestInformations();
		});
	});

	// 绑定开始全部测试的按钮
	$table.on("click", ".start-all-tests", function () {
		$.post("/admin/unit_test/web_tester", { action: "start_all" }, function (data) {
			$.handleAjaxResult(data);
			fetchTestInformations();
		});
	});

	// 绑定开始单项测试的按钮
	$table.on("click", ".start-test", function () {
		var assemblyName = $(this).closest("tr").data("assembly-name");
		$.post("/admin/unit_test/web_tester", { action: "start", assembly: assemblyName }, function (data) {
			$.handleAjaxResult(data);
			fetchTestInformations();
		});
	});
});
