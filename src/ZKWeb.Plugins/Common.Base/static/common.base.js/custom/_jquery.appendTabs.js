/*
	支持延迟添加标签
	把内容合并移动到data-target对应的元素下
	一般用于通过模板模块给一个现有的标签组添加新标签

	例子
	<div data-toggle="append-tabs" data-target=".target-tabs">
		<ul class="nav nav-tabs">
			<li>
				<a href="#target_tab_contents" data-toggle="tab">TabName</a>
			</li>
		</ul>
		<div class="tab-content">
			<div id="target_tab_contents" class="tab-pane">
				<div class="contents">
				</div>
			</div>
		</div>
	</div>

	注意: 如果标签组下已经有相同的标签，则原相同的标签会被移除
*/

$(function () {
	var setup = function (element) {
		$(element).find("[data-toggle='append-tabs']").each(function () {
			var $this = $(this);
			var $target = $($this.attr("data-target"));
			var $targetTabs = $target.find("> ul");
			var $targetTabContents = $target.find(".tab-content");
			$this.find("> ul > li").each(function () {
				var $tab = $(this);
				var href = $tab.find("[data-toggle='tab']").attr("href");
				// 移除相同的标签
				$targetTabs.find("> li").each(function () {
					var $existTab = $(this);
					var existHref = $existTab.find("[data-toggle='tab']").attr("href");
					if (existHref == href) {
						$existTab.remove();
						$targetTabs.find(existHref).remove();
					}
				});
				// 把标签和指向的内容移动到目标
				$tab.detach().appendTo($targetTabs);
				$this.find(href).detach().appendTo($targetTabContents);
			});
		});
	};
	setup(document);
	$(document).on("dynamicLoaded", function (e, contents) { setup(contents); });
});
