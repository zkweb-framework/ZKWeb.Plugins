using System.Collections.Generic;
using ZKWeb.Localize;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Finance.Payment.src.Components.Translates {
	/// <summary>
	/// 韩语翻译
	/// </summary>
	[ExportMany, SingletonReuse]
	public class ko_KR : ITranslateProvider {
		private static HashSet<string> Codes = new HashSet<string>() { "ko-KR" };
		private static Dictionary<string, string> Translates = new Dictionary<string, string>()
		{
			{ "PaymentApi", "지불 게이트웨이" },
			{ "PaymentTransaction", "결제 거래" },
			{ "Support receive money through payment api", "그것은 지불 인터페이스를 통해 결제를 지원합니다" },
			{ "PaymentApiManage", "지불 인터페이스 관리" },
			{ "TestPayment", "테스트 지불" },
			{ "TestApi", "테스트 인터페이스" },
			{ "Name/Owner/Remark", "이름/사람/비고 속" },
			{ "PaymentTransactionRecords", "결제 거래" },
			{ "PaymentApiType", "결제 인터페이스 유형" },
			{ "NextStep", "다음" },
			{ "Payment api type not specificed", "결제 인터페이스 유형이 지정되지 않은" },
			{ "Owner", "당신의 사람들에게" },
			{ "Please enter owner's username, keep empty if owned by system",
				"이름이 사람에 속하는 사용자 해주세요, 빈 남겨주세요 시스템에 속하는" },
			{ "Specificed payment api owner not exist", "존재하지 않는 지정된 인터페이스를 지불 속한다" },
			{ "Cannot change type after payment api created", "당신이 만드는 인터페이스의 유형을 변경할 수 없습니다 게시물을 지불" },
			{ "Cannot change owner after payment api created", "결제 인터페이스가 만든 후에는 사람들에게 수정할 수 없습니다" },
			{ "SupportTransactionTypes", "지원 거래 유형" },
			{ "TestTransaction", "테스트 트랜잭션" },
			{ "PaymentPassword", "결제 비밀번호" },
			{ "Password required to pay transactions", " 암호가 지불해야 될 때" },
			{ "Add transaction from admin panel is not supported", "당신은 배경에서 직접 지불 거래를 추가 할 수 없습니다" },
			{ "Serial/Payer/Payee/Description/Remark", "직렬/지불/수취인/설명/비고" },
			{ "Serial", "일련 번호" },
			{ "ExternalSerial", "외부 직렬" },
			{ "ApiName", "인터페이스 이름" },
			{ "Amount", "돈" },
			{ "Payer", "지불인" },
			{ "Payee", "수취인" },
			{ "State", "상태" },
			{ "Test Payment Api", "테스트 지불 인터페이스" },
			{ "Test payment api is usable", "테스트 지불 인터페이스를 사용할 수 있습니다" },
			{ "Payment api not exist", "결제 인터페이스가 존재하지 않는다" },
			{ "Unknown payment transaction type {0}", "지불 거래의 알 수없는 유형 {0}" },
			{ "Unknown payment api type {0}", "알 수없는 지불 트랜잭션 인터페이스 유형 {0}" },
			{ "Transaction amount must large than zero", "거래 금액이 0보다 커야" },
			{ "Transaction description is required", "거래 설명을 비워 둘 수 없습니다" },
			{ "Selected payment api not support this transaction", "현재 트랜잭션을 지원하지 않습니다 지불 인터페이스를 선택" },
			{ "Selected payment api is deleted", "선택 지불 게이트웨이 제거되었습니다, 당신은 사용할 수 없습니다" },
			{ "Transaction Created", "전시회 생성 된" },
			{ "Payment transaction not found", "지불 트랜잭션이 존재하지 않습니다" },
			{ "Payment transaction {0} error: {1}", "결제 거래 {0} 오류 : {1}" },
			{ "Payer of transaction not logged in", "지불 트랜잭션에 기록" },
			{ "Transaction not waiting for pay", "무역 결제를 기다리고되지 않습니다" },
			{ "Transaction not at initial state, can't set to waiting paying",
				"거래는 초기 상태에서는, 지불 기다릴 설정할 수없는되지" },
			{ "Transaction not waiting for pay or confirm, can't set to success",
				"거래는 입금 확인을 기다리고 있지 않거나 성공적인 거래로 설정할 수 없습니다" },
			{ "Transaction already aborted, can't process again", " 트랜잭션이 중단 된 프로세스가 반복 될 수 없다" },
			{ "Only secured paid transaction can call send goods api",
				"거래 지불 만 보안 트랜잭션이 전달 인터페이스를 호출 할 수 있습니다" },
			{ "Unsupported transaction state: {0}", "지원되지 않는 트랜잭션 상태: {0}" },
			{ "InitialState", "초기 상태" },
			{ "WaitingPaying", "결제 대기 중" },
			{ "SecuredPaid", "지불 보안 트랜잭션" },
			{ "TransactionSuccess", "무역 성공" },
			{ "TransactionAborted", "트랜잭션 종료" },
			{ "Change transaction state to {0}", "에 트랜잭션 상태를 전환 {0}" },
			{ "Notify goods shipped to payment api success", "알림 결제 인터페이스 성공적 발송 한" },
			{ "Create test transaction success, redirecting to payment page...",
				"테스트 트랜잭션이 성공적으로 작성하면 결제 페이지로 이동하는 것입니다……" },
			{ "Pay", "지불" },
			{ "Use test api to pay transaction created with other api type is not allowed",
				"당신은 테스트 인터페이스 결제 옵션 다른 인터페이스 계약을 사용할 수 없습니다" },
			{ "TestApi Pay", "테스트 인터페이스 결제를 사용하여" },
			{ "You need provide the password entered at test api creation, after submit transaction will be paid",
				"당신이 박람회를 제출 한 후, 테스트 인터페이스 암호를 만들 때 지불되고있다 기입해야합니다" },
			{ "PayType", "결제 유형" },
			{ "ImmediateArrival", "인스턴트 신용" },
			{ "SecuredTransaction", "보안 트랜잭션" },
			{ "Incorrect payment password", "결제 암호가 올바르지 않습니다" },
			{ "Redirecting to payment transaction records...", "페이지 거래에 점프 기록……" },
			{ "Payment Successfully, Redirecting to result page...", "성공적인 지불, 결과 페이지에 점프입니다……" },
			{ "LastError", "마지막 오류가 발생했습니다" },
			{ "DetailRecords", "자세한 기록" },
			{ "Creator", "작성" },
			{ "Contents", "함유량" },
			{ "PayResult", "지불의 결과" },
			{ "Test", "테스트" },
			{ "PaymentFee", "결제 수수료" },
			{ "Selected payment api does not exist", "선택한 결제 인터페이스가 존재하지 않습니다" },
			{ "Finance Manage", "재무 관리" },
			{ "ReleatedTransactions", "특수 관계자 거래" },
			{ "TransactionSerial", "交易流水号" },
			{ "Change transaction state to {0} failed: {1}", "切换交易状态到{0}失败: {1}" },
			{ "Transaction is aborted", "거래 일련 번호" },
			{ "Can't process waiting paying on child transaction {0}, reason is {1}",
				"하위 거래 {0}에 대한 지불 대기를 처리 할 수 없습니다. 이유는 {1}입니다" },
			{ "Can't process secured paid on child transaction {0}, reason is {1}",
				"보안 트랜잭션 서브 트랜잭션을 처리 할 수없는 ({0}) 지불 된, 그 이유는 {1} 인" },
			{ "Can't process success on child transaction {0}, reason is {1}",
				"서브 트랜잭션을 처리 할 수없는 ({0})은 트랜잭션이 성공적이며, 그 이유는 {1}" },
			{ "Some child transaction have different currency type",
				"일부 하위 트랜잭션의 통화 유형이 다릅니다" },
			{ "Some child transaction have different payment api",
				"결제 인터페이스는 파튼 전시회를 불일치" },
			{ "Some child transaction have different payer", "지불 파튼 트랜잭션 일관성을" },
			{ "Some child transaction have different payee", "수취인 파튼 트랜잭션 일관성을" },
			{ "Some child transaction is not payable", "파튼 거래는 지불 할 수" }
		};

		public bool CanTranslate(string code) {
			return Codes.Contains(code);
		}

		public string Translate(string text) {
			return Translates.GetOrDefault(text);
		}
	}
}
