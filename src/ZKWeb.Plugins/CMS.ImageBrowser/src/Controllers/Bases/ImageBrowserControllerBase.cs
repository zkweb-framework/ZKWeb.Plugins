using System.Collections.Generic;
using System.IO;
using System.Linq;
using ZKWeb.Localize;
using ZKWebStandard.Extensions;
using ZKWebStandard.Utils;
using ZKWeb.Web;
using ZKWeb.Web.ActionResults;
using ZKWebStandard.Web;
using ZKWeb.Plugins.Common.Admin.src.Components.PrivilegeProviders;
using ZKWeb.Plugins.Common.Base.src.Controllers.Bases;
using ZKWeb.Plugins.Common.Admin.src.Domain.Entities.Interfaces;
using System;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms.Interfaces;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms.Attributes;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms;
using ZKWeb.Plugins.Common.Base.src.UIComponents.AjaxTable;
using ZKWeb.Plugins.CMS.ImageBrowser.src.Domain.Services;
using ZKWeb.Plugins.Common.Base.src.UIComponents.BaseTable.Extensions;
using ZKWeb.Plugins.Common.Admin.src.Domain.Services;
using ZKWeb.Plugins.Common.Base.src.Controllers.Extensions;
using ZKWeb.Plugins.Common.Base.src.Components.Exceptions;
using ZKWeb.Plugins.Common.Base.src.UIComponents.Forms.Extensions;

namespace ZKWeb.Plugins.CMS.ImageBrowser.src.Controllers.Bases {
	/// <summary>
	/// 图片浏览器的控制器基础类
	/// </summary>
	public abstract class ImageBrowserControllerBase :
		ControllerBase,
		IPrivilegesProvider, IWebsiteStartHandler {
		/// <summary>
		/// 图片类别
		/// 保存到文件系统时使用小写
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
		/// Ajax图片列表使用的模板路径
		/// </summary>
		public virtual string AjaxTableTemplatePath {
			get { return "/static/cms.imagebrowser.tmpl/imageList.tmpl"; }
		}
		/// <summary>
		/// 默认需要管理员权限
		/// </summary>
		public virtual Type RequiredUserType { get { return typeof(IAmAdmin); } }
		/// <summary>
		/// 默认需要按类别生成的权限
		/// </summary>
		public virtual string[] RequiredPrivileges { get { return new[] { "ImageManage:" + Category }; } }
		/// <summary>
		/// 获取图片上传表单
		/// </summary>
		/// <returns></returns>
		public virtual IModelFormBuilder GetForm() {
			return new Form(CategoryLower, new FileUploaderFieldAttribute(""), UploadUrl);
		}

		/// <summary>
		/// 浏览图片
		/// </summary>
		/// <returns></returns>
		public virtual IActionResult Action() {
			var form = GetForm();
			var table = Application.Ioc.Resolve<AjaxTableBuilder>();
			table.Id = TableId;
			table.Target = SearchUrl;
			table.Template = AjaxTableTemplatePath;
			var searchBar = Application.Ioc.Resolve<AjaxTableSearchBarBuilder>();
			searchBar.TableId = TableId;
			searchBar.KeywordPlaceHolder = new T("Name");
			return new TemplateResult(TemplatePath,
				new { form, table, searchBar, removeUrl = RemoveUrl });
		}

		/// <summary>
		/// 搜索图片
		/// </summary>
		/// <returns></returns>
		public virtual IActionResult SearchAction() {
			// 获取搜索请求
			var json = Request.Get<string>("json");
			var request = AjaxTableSearchRequest.FromJson(json);
			// 搜索图片列表
			// 分页时如果没有结果，使用最后一页的结果
			var imageManager = Application.Ioc.Resolve<ImageManager>();
			var names = imageManager.Query(CategoryLower);
			if (!string.IsNullOrEmpty(request.Keyword)) {
				names = names.Where(name => name.Contains(request.Keyword)).ToList();
			}
			var response = new AjaxTableSearchResponse();
			var result = response.Pagination.Paging(request, names.AsQueryable());
			// 返回搜索结果
			response.PageNo = request.PageNo;
			response.PageSize = request.PageSize;
			response.Rows.AddRange(result.Select(name => {
				var path = imageManager.GetImageWebPath(
					CategoryLower, name, imageManager.ImageExtension);
				var thumbnailPath = imageManager.GetImageWebPath(
					CategoryLower, name, imageManager.ImageThumbnailExtension);
				var storagePath = imageManager.GetImageStoragePath(
					CategoryLower, name, imageManager.ImageExtension);
				var fileInfo = new FileInfo(storagePath);
				var lastWriteTime = fileInfo.LastWriteTimeUtc.ToClientTimeString();
				var fileSize = FileUtils.GetSizeDisplayName(fileInfo.Length);
				return new Dictionary<string, object>() {
					{ "name", name },
					{ "path", path },
					{ "thumbnailPath", thumbnailPath },
					{ "lastWriteTime", lastWriteTime },
					{ "fileSize", fileSize }
				};
			}));
			return new JsonResult(response);
		}

		/// <summary>
		/// 上传图片
		/// </summary>
		/// <returns></returns>
		public virtual IActionResult UploadAction() {
			this.RequireAjaxRequest();
			var form = GetForm();
			return new JsonResult(form.Submit());
		}

		/// <summary>
		/// 删除图片
		/// </summary>
		/// <returns></returns>
		public virtual IActionResult RemoveAction() {
			this.RequireAjaxRequest();
			var imageManager = Application.Ioc.Resolve<ImageManager>();
			var name = Request.Get<string>("name");
			imageManager.Remove(CategoryLower, name);
			return new JsonResult(new { message = new T("Remove Successfully") });
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
		/// 对处理函数进行包装
		/// </summary>
		/// <param name="action">处理函数</param>
		/// <returns></returns>
		protected virtual Func<IActionResult> WrapAction(Func<IActionResult> action) {
			return () => {
				// 检查权限
				var privilegeManager = Application.Ioc.Resolve<PrivilegeManager>();
				privilegeManager.Check(RequiredUserType, RequiredPrivileges);
				return action();
			};
		}

		/// <summary>
		/// 网站启动时的处理
		/// </summary>
		public virtual void OnWebsiteStart() {
			var controllerManager = Application.Ioc.Resolve<ControllerManager>();
			controllerManager.RegisterAction(Url, HttpMethods.GET, WrapAction(Action));
			controllerManager.RegisterAction(SearchUrl, HttpMethods.POST, WrapAction(SearchAction));
			controllerManager.RegisterAction(UploadUrl, HttpMethods.POST, WrapAction(UploadAction));
			controllerManager.RegisterAction(RemoveUrl, HttpMethods.POST, WrapAction(RemoveAction));
		}

		/// <summary>
		/// 图片上传表单
		/// 需要支持直接上传，这里不开启CSRF校验（上面会检查是否ajax请求）
		/// </summary>
		[Form("ImageUploadForm", EnableCsrfToken = false)]
		public class Form : ModelFormBuilder {
			/// <summary>
			/// 图片文件
			/// </summary>
			[FileUploaderField("Image")]
			public IHttpPostedFile Image { get; set; }
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
			/// <param name="uploadAttribute">上传属性</param>
			/// <param name="uploadUrl">上传地址</param>
			public Form(
				string category, FileUploaderFieldAttribute uploadAttribute, string uploadUrl) {
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
				// 如果是直接上传，文件大小和后缀不会经过事先的检查，这里手动再检查一遍
				var files = Request.GetPostedFiles().ToList();
				var imageFile = files.Count <= 0 ? null : files[0].Second;
				if (imageFile == null) {
					throw new BadRequestException(new T("Please select image file"));
				}
				((FileUploaderFieldAttribute)this.Form.Fields.First(
					f => f.Attribute.Name == "Image").Attribute).Check(imageFile);
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
				imageManager.Save(imageFile.OpenReadStream(), Category, name);
				return new {
					message = new T("Upload Successfully"),
					path = imageManager.GetImageWebPath(Category, name, imageManager.ImageExtension)
				};
			}
		}
	}
}
