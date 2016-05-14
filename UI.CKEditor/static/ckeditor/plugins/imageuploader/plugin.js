CKEDITOR.plugins.add("imageuploader", {
	requires: "filebrowser",

	init: function (editor) {
		// set image browse url if image upload category is specificed
		var imageUploadCategory = editor.config.imageUploadCategory;
		if (imageUploadCategory) {
			editor.config.filebrowserImageBrowseUrl = "/ckeditor/browse_images?category=" + imageUploadCategory;
		}
	}
});
