$(function () {
	$(document).on("click", ".product-view .product-add-bookmark .add-bookmark", function () {
		var $this = $(this);
		var productId = $this.data("product-id");
		$.post("/api/product_bookmarks/add", { productId: productId }, function (data) {
			$.handleAjaxResult(data);
			if (data.bookmarked) {
				var $bookmarked = $this.closest(".product-add-bookmark").find(".bookmarked");
				$this.addClass("hide");
				$bookmarked.removeClass("hide");
			}
		});
	});
});
