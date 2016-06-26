using System.Collections.Generic;
using ZKWeb.Localize;
using ZKWebStandard.Extensions;
using ZKWebStandard.Ioc;

namespace ZKWeb.Plugins.Finance.Payment.src.Translates {
	/// <summary>
	/// 日本语翻译
	/// </summary>
	[ExportMany, SingletonReuse]
	public class ja_JP : ITranslateProvider {
		private static HashSet<string> Codes = new HashSet<string>() { "ja-JP" };
		private static Dictionary<string, string> Translates = new Dictionary<string, string>()
		{
			{ "PaymentApi", "支払い方法" },
			{ "PaymentTransaction", "支払い取引" },
			{ "Support receive money through payment api", "お金を支払うためのインタフェースをユーザーに提供する" },
			{ "PaymentApiManage", "支払い方法管理" },
			{ "TestPayment", "支払いテスト" },
			{ "TestApi", "テスト用の支払い方法" },
			{ "Name/Owner/Remark", "名称/所有者/リマーク" },
			{ "PaymentTransactionRecords", "支払い取引記録" },
			{ "PaymentApiType", "支払い方法のタイプ" },
			{ "NextStep", "次へ" },
			{ "Payment api type not specificed", "支払い方法のタイプが指定されていません" },
			{ "Owner", "所有者" },
			{ "Please enter owner's username, keep empty if owned by system",
				"所有者のユーザー名を入力してください、システムが所有するばあいは空のままにしてください" },
			{ "Specificed payment api owner not exist", "指定された所有者が見つかりませんでした" },
			{ "Cannot change type after payment api created", "支払い方法は作成したあとからタイプを変更することができません" },
			{ "Cannot change owner after payment api created", "支払い方法は作成したあとから所有者を変更することができません" },
			{ "SupportTransactionTypes", "使用できる取引のタイプ" },
			{ "TestTransaction", "テスト用の取引" },
			{ "PaymentPassword", "支払いパスワード" },
			{ "Password required to pay transactions", "支払う際に要求されるパスワード" },
			{ "Add transaction from admin panel is not supported", "管理者パネルから取引を追加することはできません" },
			{ "Serial/Payer/Payee/Description/Remark", "シリアル/支払人/受取人/説明/リマーク" },
			{ "Serial", "シリアル" },
			{ "ExternalSerial", "外部シリアル" },
			{ "ApiName", "名称" },
			{ "Amount", "金額" },
			{ "Payer", "支払人" },
			{ "Payee", "受取人" },
			{ "State", "状態" },
			{ "Test Payment Api", "テスト用の支払い方法" },
			{ "Test payment api is usable", "支払い方法が正常に使用できるかをテストできます" },
			{ "Payment api not exist", "支払い方法が存在していません" },
			{ "Unknown payment transaction type {0}", "未知の支払い取引タイプ{0}" },
			{ "Unknown payment api type {0}", "未知の支払い方法タイプ{0}" },
			{ "Transaction amount must large than zero", "取引金額は0以上でなければなりません" },
			{ "Transaction description is required", "取引説明を入力してください" },
			{ "Selected payment api not support this transaction", "選択された支払い方法は現在の取引に使用することができません" },
			{ "Selected payment api is deleted", "選択された支払い方法は削除済みです、使用できません" },
			{ "Transaction Created", "取引開始" },
			{ "Payment transaction not found", "支払い取引が見つかりませんでした" },
			{ "Payment transaction {0} error: {1}", "支払い取引{0}エラー: {1}" },
			{ "Payer of transaction not logged in", "支払人がログインしていません" },
			{ "Transaction not waiting for pay", "取引の状態は支払い待ちではありません" },
			{ "Transaction not at initial state, can't set to waiting paying", "取引が初期状態ではないため、支払い待ちに設定することはできません" },
			{ "Transaction not waiting for pay or confirm, can't set to success", "取引が支払い待ちまたは確認待ちではないため、取引成功に設定することはできません" },
			{ "Transaction already aborted, can't process again", "取引はすでに中止されています、重複処理ができません" },
			{ "Only secured paid transaction can call send goods api", "担保付取引のみ配達APIを呼び出すことができます" },
			{ "Unsupported transaction state: {0}", "サポートされていない取引状態: {0}" },
			{ "InitialState", "初期状態" },
			{ "WaitingPaying", "支払い待ち" },
			{ "SecuredPaid", "担保付き支払い済み" },
			{ "TransactionSuccess", "取引成功" },
			{ "TransactionAborted", "取引中止" },
			{ "Change transaction state to {0}", "取引状態は{0}に変更されました" },
			{ "Notify goods sent to payment api success", "支払いプラットフォームに配達済みであることを知らせる" },
			{ "Create test transaction success, redirecting to payment page...",
				"テスト用の取引の作成に成功しました、支払いページへ移動します……" },
			{ "Pay", "支払う" },
			{ "Use test api to pay transaction created with other api type is not allowed",
				"テスト用の支払い方法で別の支払い方法を選んだ取引を支払うことは許可されていません" },
			{ "TestApi Pay", "テスト用の支払い方法で支払う" },
			{ "You need provide the password entered at test api creation, after submit transaction will be paid",
				"テスト用の支払い方法を作成する際に記入したパスワードをここで入力してください、送信したあと取引は支払い済みになります" },
			{ "PayType", "支払いタイプ" },
			{ "ImmediateArrival", "即時入金" },
			{ "SecuredTransaction", "担保付取引" },
			{ "Incorrect payment password", "支払いパスワードが違います" },
			{ "Redirecting to payment transaction records...", "取引の記録ページへ移動します……" },
			{ "Payment Successfully, Redirecting to result page...", "支払いに成功しました、結果ページへ移動します……" },
			{ "LastError", "最後に起きたエラー" },
			{ "DetailRecords", "詳細記録" },
			{ "Creator", "作成者" },
			{ "Contents", "内容" },
			{ "PayResult", "支払い結果" },
			{ "Test", "テスト" },
			{ "PaymentFee", "支払い手数料" },
			{ "Selected payment api does not exist", "選択した支払方法は存在しません" }
		};

		public bool CanTranslate(string code) {
			return Codes.Contains(code);
		}

		public string Translate(string text) {
			return Translates.GetOrDefault(text);
		}
	}
}
