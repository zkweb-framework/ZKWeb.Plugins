/* 修复Element不存在的问题 */
if (!window.Element) {
	Element = function() {};
}
