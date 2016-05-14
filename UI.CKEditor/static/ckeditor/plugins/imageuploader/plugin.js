CKEDITOR.plugins.add("imageuploader", {
	init: function (editor) {
		editor.config.filebrowserBrowseUrl = "/ckeditor/browse_images";
	}
});
