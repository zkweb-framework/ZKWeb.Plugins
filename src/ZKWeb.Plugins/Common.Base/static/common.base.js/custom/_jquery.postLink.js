/*
	用于支持post数据的链接
	例子
	<a post-href="/user/logout"></a>
	<a post-href="/api/example_api" ajax="true"></a>
*/

$(function () {
	var setup = function ($elements) {
		$elements.each(function () {
			var $this = $(this);
			var action = $this.attr("post-href");
			var $form = $("<form>").attr({ class: "inline", action: action, method: "post" });
			$this.prepend($form);
			if ($this.attr("ajax") == "true") {
				$form.attr("ajax", "true");
				$form.commonAjaxForm(function (data) { $.handleAjaxResult.call($this, data); });
			}
			$this.on("click", function () { $form.trigger("submit"); });
		});
	};
	var rule = "a[post-href]";
	setup($(rule));
	$(document).on("dynamicLoaded", function (e, contents) {
		setup($(contents).find(rule));
	});
});
