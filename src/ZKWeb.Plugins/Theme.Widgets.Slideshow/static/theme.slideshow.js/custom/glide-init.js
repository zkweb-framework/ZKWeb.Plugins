/*
	自动初始化轮播图

	例:
	<div class="glide slideshow" data-trigger="glide">
		<div class="glide__arrows">
			<button class="glide__arrow prev" data-glide-dir="<">prev</button>
			<button class="glide__arrow next" data-glide-dir=">">next</button>
		</div>
		<div class="glide__wrapper">
			<ul class="glide__track">
				<li class="glide__slide"></li>
				<li class="glide__slide"></li>
				<li class="glide__slide"></li>
			</ul>
		</div>
		<div class="glide__bullets"></div>
	</div>
*/

$(function () {
	var initializedKey = "glide-initialized";
	var setup = function (elements) {
		$(elements).each(function () {
			var $element = $(this);
			if ($element.data(initializedKey)) {
				return;
			}
			var options = $.merge({
				// 默认选项
			}, $element.data("options") || {});
			$element.glide(options);
			$element.data(initializedKey, true);
		});
	};
	// 页面载入时自动初始化
	var rule = "[data-trigger='glide']";
	setup($(rule), document);
	$(document).on("dynamicLoaded", function (e, contents) {
		setup($(contents).find(rule), contents);
	});
});
