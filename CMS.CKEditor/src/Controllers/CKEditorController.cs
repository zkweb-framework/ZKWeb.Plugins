using DryIocAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Web.ActionResults;
using ZKWeb.Web.Interfaces;

namespace ZKWeb.Plugins.CMS.CKEditor.src.Controllers {
	/// <summary>
	/// CKEditor使用的控制器
	/// 上传图片功能
	///		上传图片，可指定文件名
	///		浏览图片
	///		删除图片
	///		按文件名搜索图片
	/// 
	/// 图片文件系统
	///		定义各项操作的接口
	///		按传入的文件系统ID获取指定的文件系统
	/// </summary>
	[ExportMany]
	public class CKEditorController : IController {
		/// <summary>
		/// 浏览图片
		/// </summary>
		/// <returns></returns>
		[Action("ckeditor/browse_images")]
		public IActionResult BrowseImages() {
			return new PlainResult("browse images");
		}
	}
}
