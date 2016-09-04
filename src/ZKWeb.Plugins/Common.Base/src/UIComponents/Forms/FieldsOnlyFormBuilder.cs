using System.Text;

namespace ZKWeb.Plugins.Common.Base.src.UIComponents.Forms {
	/// <summary>
	/// 只包含字段内容的表单构建器
	/// </summary>
	public class FieldsOnlyFormBuilder : FormBuilder {
		/// <summary>
		/// 获取表单html
		/// </summary>
		/// <returns></returns>
		public override string ToString() {
			var html = new StringBuilder();
			foreach (var field in Fields) {
				RenderFormField(html, field);
			}
			return html.ToString();
		}
	}
}
