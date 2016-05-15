// Author: 303248153@github
// License: MIT License
//

CKEDITOR.plugins.add("imagebrowser", {
	requires: "filebrowser",

	init: function (editor) {
		// set image browse url if image upload category is specificed
		var imageBrowserUrl = editor.config.imageBrowserUrl;
		if (imageBrowserUrl) {
			editor.config.filebrowserImageBrowseUrl = imageBrowserUrl;
			editor.config.filebrowserImageUploadUrl = imageBrowserUrl + "/upload";
		}
	}
});
