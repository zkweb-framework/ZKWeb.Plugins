namespace ZKWeb.Plugins.Common.SerialGenerate.src.Components.SerialGenerateHandlers.Interfaces {
	/// <summary>
	/// 生成序列号时的处理器
	/// 可用于修改默认生成的序列号
	/// </summary>
	public interface ISerialGenerateHandler {
		/// <summary>
		/// 生成时的处理
		/// </summary>
		/// <param name="data">数据</param>
		/// <param name="serial">原有的序列号</param>
		void OnGenerate(object data, ref string serial);
	}
}
