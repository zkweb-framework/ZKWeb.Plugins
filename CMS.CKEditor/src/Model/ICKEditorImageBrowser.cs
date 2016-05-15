using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Plugins.Common.Base.src.Model;

namespace ZKWeb.Plugins.CMS.CKEditor.src.Model {
	/// <summary>
	/// CKEditor使用的图片浏览器接口
	/// </summary>
	public interface ICKEditorImageBrowser {
		/// <summary>
		/// 保存图片文件
		/// </summary>
		/// <param name="image">图片对象</param>
		/// <param name="name">图片名称，不应该包含后缀</param>
		void Save(Image image, string name);

		/// <summary>
		/// 删除图片文件
		/// </summary>
		/// <param name="name">图片名称，不应该包含后缀</param>
		void Remove(string name);

		/// <summary>
		/// 搜索图片文件
		/// </summary>
		/// <param name="request">搜索请求</param>
		/// <returns></returns>
		AjaxTableSearchResponse Search(AjaxTableSearchRequest request);
	}
}
