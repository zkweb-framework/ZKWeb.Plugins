using DryIocAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZKWeb.Localize.Interfaces;
using ZKWeb.Utils.Extensions;

namespace ZKWeb.Plugins.Common.Admin.src.Translates {
	/// <summary>
	/// 韩语翻译
	/// </summary>
	[ExportMany, SingletonReuse]
	public class ko_KR : ITranslateProvider {
		private static HashSet<string> Codes = new HashSet<string>() { "ko-KR" };
		private static Dictionary<string, string> Translates = new Dictionary<string, string>()
		{
			{ "Admin Login", "관리자 로그인" },
			{ "Please enter username", "사용자 이름을 입력하세요" },
			{ "Please enter password", "비밀번호를 입력하세요" },
			{ "Username", "아이디" },
			{ "Password", "암호" },
			{ "Login", "착륙" },
			{ "Register new user", "신규 등록" },
			{ "RememberLogin", "로그인 정보를 기억" },
			{ "Register", "등록" },
			{ "ConfirmPassword", "비밀번호 확인" },
			{ "Please repeat the password exactly", "이전 채우기 암호를 반복" },
			{ "OldPassword", "이전 암호" },
			{ "Please enter old password", "이전 암호를 입력하세요" },
			{ "User Registration", "사용자 등록" },
			{ "User Login", "사용자 로그인" },
			{ "Username is already taken, please choose other username", "사용자 이름이 이미 사용, 다른 사용자 이름 선택" },
			{ "You have registered successfully, thanks for you registration",
				"등록 된 사용자가 성공, 감사 합니다 귀하의 등록에 대 한" },
			{ "User Panel", "사용자 센터" },
			{ "Login successful", "로그인 성공" },
			{ "Welcome to ", "환영 합니다" },
			{ "Logout", "출구 로그인" },
			{ "Register for free", "무료 가입" },
			{ "Website has no admin yet, the first login user will become super admin.",
				"현재, 아무 관리자, 사용자가 처음으로 로그온 한 것입니다 슈퍼 관리자" },
			{ "You have already logged in, continue will replace the logged in user.",
				"당신이 로그인을 계속 현재 로그온된 한 사용자를 대체할 것 이다" },
			{ "Sorry, You have no privileges to use admin panel.", "죄송 합니다, 당신은 권한이 없는 관리자를 사용 하 여" },
			{ "Incorrect username or password", "잘못 된 사용자 이름이 나 암호를 입력 해 주십시오는" },
			{ "Apps", "응용 프로그램" },
			{ "Workspace", "워크숍" },
			{ "Website Index", "웹 사이트 홈" },
			{ "About Me", "나에 대해" },
			{ "About Website", "웹 사이트에 대 한" },
			{ "Admin Panel", "관리의 배경" },
			{ "My Apps", "내 응용 프로그램" },
			{ "Action require {0}, and {1} privileges", "운영 요구 사항 id {0} 및 {1} 오른쪽" },
			{ "Action require {0}", "작업은 {0} 정체성을 필요 합니다" },
			{ "User", "사용자" },
			{ "UserType", "사용자 유형" },
			{ "Admin", "관리자" },
			{ "SuperAdmin", "슈퍼 관리자" },
			{ "CooperationPartner", "파트너" },
			{ "CreateTime", "만든 시간" },
			{ "Admin Manage", "관리자 관리" },
			{ "User Manage", "사용자 관리" },
			{ "Role Manage", "역할 관리" },
			{ "Role", "역할" },
			{ "Roles", "역할" },
			{ "UserRole", "역할" },
			{ "View", "보기" },
			{ "Edit", "편집기" },
			{ "Delete", "삭제" },
			{ "DeleteForever", "영구적으로 삭제" },
			{ "Please enter name", "이름을 기입 하 여 주십시오" },
			{ "Remark", "노트" },
			{ "Please enter remark", "덧 글에 작성 하시기 바랍니다" },
			{ "Saved Successfully", "성공적으로 저장" },
			{ "Keep empty if you don't want to change", "수정 하려는 경우 비워 둡니다" },
			{ "Name/Remark", "이름/노트" },
			{ "Name", "이름" },
			{ "Value", "값" },
			{ "DirectoryName", "디렉터리 이름" },
			{ "Description", "설명" },
			{ "LastUpdated", "업데이트 시간" },
			{ "Add {0}", "{0} 추가" },
			{ "Edit {0}", "{0} 편집" },
			{ "Delete {0}", "{0} 삭제" },
			{ "Please enter password when creating admin", "필요한 관리자 암호 만들기" },
			{ "Please enter password when creating user", "만들 때 사용자 암호를 작성 해야" },
			{ "You can't downgrade yourself to normal admin", "그들의 최고 관리자 권한을 취소할 수 없습니다" },
			{ "Privileges", "사용 권한" },
			{ "Recycle Bin", "휴지통" },
			{ "Batch Delete", "일괄 삭제" },
			{ "Please select {0} to delete", "{0} 삭제를 선택 필요 하시기 바랍니다" },
			{ "Sure to delete following {0}?", "다음 {0}의 삭제를 확인？" },
			{ "Batch Recover", "批量恢复" },
			{ "Please select {0} to recover", "{0}의 원하는 복구 선택" },
			{ "Sure to recover following {0}?", "복원 된 다음 {0}을 확인？" },
			{ "Batch Delete Forever", "영구적으로 삭제 하는 대량" },
			{ "Sure to delete following {0} forever?", "다음 {0}의 영구 삭제를 확인？이 작업은 복원 되지 않습니다！" },
			{ "Delete yourself is not allowed", "자신의 사용자를 삭제할 수 없습니다" },
			{ "Non ajax request batch action is not secure", "대량 작업에 의해 비-Ajax 보안을 제출 하지는, 코드 수정" },
			{ "Action {0} not exist", "{0}에 대 한 원하는 작업을 찾을 수 없습니다" },
			{ "Delete Successful", "성공적으로 삭제" },
			{ "Batch Delete Successful", "대량 삭제 성공" },
			{ "Batch Recover Successful", "일괄 복구 성공" },
			{ "Batch Delete Forever Successful", "성공적으로 삭제 되었습니다 영구적으로 로트 크기" },
			{ "Change Password", "비밀 번호 수정" },
			{ "Change Avatar", "변경 아바타" },
			{ "Avatar", "아바타" },
			{ "DeleteAvatar", "아바타 삭제" },
			{ "Please select avatar file", "아바타 파일 선택 하십시오" },
			{ "Parse uploaded image failed", "구문 분석 업로드 이미지 실패" },
			{ "User not found", "사용자 찾을 수 없습니다" },
			{ "Incorrect old password", "오래 된 암호가 올바르지 않습니다, 작성해 주시기 바랍니다는" },
			{ "No Role", "역할" },
			{ "Website Name", "웹사이트 이름" },
			{ "Default Language", "기본 언어" },
			{ "Default Timezone", "기본 시간대" },
			{ "Hosting Information", "서버 정보" },
			{ "Plugin List", "플러그인 목록" },
			{ "Admin panel and users management", "관리자 패널 및 사용자 관리" },
			{ "Clear Cache", "캐시 지우기" },
			{ "Clear Cache Successfully", "성공적으로 캐시 지우기" }
		};

		public bool CanTranslate(string code) {
			return Codes.Contains(code);
		}

		public string Translate(string text) {
			return Translates.GetOrDefault(text);
		}
	}
}
