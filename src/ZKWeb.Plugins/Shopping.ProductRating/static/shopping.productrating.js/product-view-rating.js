/*
	商品详情页的显示评价列表功能

	评价列表已基于Ajax表格实现，这里做的是第一次点击商品评价标签的时候刷新表格
	如果不点击商品评价标签则不加载数据，减轻服务器负担
*/

$(function () {
	var isFirstTime = true;
	$(document).on("show.bs.tab", "a[href='#tab_product_rating']", function () {
		if (isFirstTime) {
			isFirstTime = false;
			$("#tab_product_rating .ajax-table").ajaxTable().refresh();
		}
	});
});
