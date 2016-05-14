CKEDITOR.plugins.add("imageuploader", {
	init: function (editor) {
		editor.config.filebrowserImageBrowseUrl = "/ckeditor/browse_images";
	}
});
