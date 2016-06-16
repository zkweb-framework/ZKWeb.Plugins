namespace ZKWeb.Plugins.Shopping.Product.src.Model {
	/// <summary>
	/// 商品属性关联的属性值数据，用于反序列化客户端传回的值
	/// 编辑商品属性时使用
	/// </summary>
	/// <example>
	/// [
	///		{ Id: 0, Name: "红色", Remark: "备注" },
	///		{ Id: 10001, Name: "蓝色", Remark: "备注" },
	///		{ Id: 10002, Name: "绿色", Remark: "备注" }
	/// ]
	/// </example>
	public class ProductPropertyValueForEdit {
		/// <summary>
		/// 属性值Id
		/// 新添加的属性Id等于null，修改原有的属性Id等于原值
		/// </summary>
		public long? Id { get; set; }
		/// <summary>
		/// 属性值名称
		/// </summary>
		public string Name { get; set; }
		/// <summary>
		/// 属性值备注
		/// </summary>
		public string Remark { get; set; }
	}
}
