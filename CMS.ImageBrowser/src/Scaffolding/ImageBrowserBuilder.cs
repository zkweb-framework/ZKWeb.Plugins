using DryIoc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using ZKWeb.Localize;
using ZKWeb.Plugins.CMS.ImageBrowser.src.Managers;
using ZKWeb.Plugins.Common.Admin.src.Managers;
using ZKWeb.Plugins.Common.Admin.src.Model;
using ZKWeb.Plugins.Common.Base.src.HtmlBuilder;
using ZKWeb.Plugins.Common.Base.src.Model;
using ZKWeb.Utils.Functions;
using ZKWeb.Web;
using ZKWeb.Web.ActionResults;
using ZKWeb.Web.Interfaces;

namespace ZKWeb.Plugins.CMS.ImageBrowser.src.Scaffolding {
	/// <summary>
	/// 图片浏览器的构建器
	/// 例：
	///	TODO: 待补充
	/// </summary>
	public abstract class ImageBrowserBuilder : IPrivilegesProvider, IWebsiteStartHandler {
		/// <summary>
		/// 图片类别
		/// </summary>
		public abstract string Category { get; }
		public string CategoryLower { get { return Category.ToLower(); } }
		/// <summary>
		/// 基础Url
		/// </summary>
		public virtual string Url { get { return "/image_browser/" + CategoryLower; } }
		/// <summary>
		/// 搜索图片的Url
		/// </summary>
		public virtual string SearchUrl { get { return Url + "/search"; } }
		/// <summary>
		/// 上传图片的Url
		/// </summary>
		public virtual string UploadUrl { get { return Url + "/upload"; } }
		/// <summary>
		/// 删除图片的Url
		/// </summary>
		public virtual string RemoveUrl { get { return Url + "/remove"; } }
		/// <summary>
		/// 表格Id
		/// </summary>
		public virtual string TableId { get { return Category.Replace(" ", "") + "ImageList"; } }
		/// <summary>
		/// 模板路径
		/// </summary>
		public virtual string TemplatePath { get { return "cms.imagebrowser/image_list.html"; } }
		/// <summary>
		/// 默认需要管理员权限
		/// </summary>
		public virtual UserTypes[] AllowedUserTypes { get { return UserTypesGroup.Admin; } }
		/// <summary>
		/// 默认需要按类别生成的权限
		/// </summary>
		public virtual string[] RequiredPrivileges { get { return new[] { "ImageManage:" + Category }; } }

		/// <summary>
		/// 浏览图片
		/// </summary>
		/// <returns></returns>
		public virtual IActionResult Action() {
			// 检查权限
			PrivilegesChecker.Check(AllowedUserTypes, RequiredPrivileges);
			// 返回模板页
			var form = new Form(CategoryLower, UploadUrl);
			var table = Application.Ioc.Resolve<AjaxTableBuilder>();
			table.Id = TableId;
			table.Target = SearchUrl;
			return new TemplateResult(TemplatePath, new { form, table });
		}

		/// <summary>
		/// 搜索图片
		/// </summary>
		/// <returns></returns>
		public virtual IActionResult SearchAction() {
			// 检查权限
			PrivilegesChecker.Check(AllowedUserTypes, RequiredPrivileges);
			// 返回搜索结果
			throw new NotImplementedException();
		}

		/// <summary>
		/// 上传图片
		/// </summary>
		/// <returns></returns>
		public virtual IActionResult UploadAction() {
			// 检查权限
			PrivilegesChecker.Check(AllowedUserTypes, RequiredPrivileges);
			// 返回上传结果
			var form = new Form(CategoryLower, UploadUrl);
			return new JsonResult(form.Submit());
		}

		/// <summary>
		/// 删除图片
		/// </summary>
		/// <returns></returns>
		public virtual IActionResult RemoveAction() {
			// 检查权限
			PrivilegesChecker.Check(AllowedUserTypes, RequiredPrivileges);
			// 返回删除结果
			throw new NotImplementedException();
		}

		/// <summary>
		/// 获取权限列表
		/// </summary>
		/// <returns></returns>
		public virtual IEnumerable<string> GetPrivileges() {
			foreach (var privilege in RequiredPrivileges) {
				yield return privilege;
			}
		}

		/// <summary>
		/// 网站启动时的处理
		/// </summary>
		public virtual void OnWebsiteStart() {
			var controllerManager = Application.Ioc.Resolve<ControllerManager>();
			controllerManager.RegisterAction(Url, HttpMethods.GET, Action);
			controllerManager.RegisterAction(SearchUrl, HttpMethods.POST, SearchAction);
			controllerManager.RegisterAction(UploadUrl, HttpMethods.POST, UploadAction);
			controllerManager.RegisterAction(RemoveUrl, HttpMethods.POST, RemoveAction);
		}

		/// <summary>
		/// 图片上传表单
		/// 需要支持直接上传，这里不开启CSRF校验
		/// </summary>
		[Form("ImageUploadForm", EnableCsrfToken = false)]
		public class Form : ModelFormBuilder {
			/// <summary>
			/// 图片文件
			/// </summary>
			[FileUploaderField("Image")]
			public HttpPostedFileBase Image { get; set; }
			/// <summary>
			/// 自定义名称
			/// </summary>
			[TextBoxField("CustomName")]
			public string CustomName { get; set; }
			/// <summary>
			/// 图片类别
			/// </summary>
			public string Category { get; set; }

			/// <summary>
			/// 初始化
			/// </summary>
			/// <param name="category">图片类别</param>
			/// <param name="uploadUrl">上传地址</param>
			public Form(string category, string uploadUrl) {
				Category = category;
				Form.Attribute.Action = uploadUrl;
			}

			/// <summary>
			/// 绑定表单
			/// </summary>
			protected override void OnBind() { }

			/// <summary>
			/// 提交表单
			/// </summary>
			/// <returns></returns>
			protected override object OnSubmit() {
				// 需要支持直接上传，这里不根据名称获取
				var imageFile = HttpContextUtils.CurrentContext.Request.Files[0];
				if (imageFile == null) {
					throw new HttpException(400, new T("Please select image file"));
				}
				var filename = string.IsNullOrEmpty(CustomName) ? imageFile.FileName : CustomName;
				// 调用管理器保存
				// 文件名有重复时自动向后添加(数字)
				var imageManager = Application.Ioc.Resolve<ImageManager>();
				var originalName = Path.GetFileNameWithoutExtension(filename);
				var name = originalName;
				var count = 1;
				while (imageManager.Exists(Category, name)) {
					name = string.Format("{0} ({1})", originalName, count++);
				}
				imageManager.Save(imageFile.InputStream, Category, name);
				return new {
					message = new T("Upload Successfully"),
					path = imageManager.GetImageWebPath(Category, name, imageManager.ImageExtension)
				};
			}
		}
	}
}
