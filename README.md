# IniFileParser
Parsing ini file Asset for unity

## 개요
Windows 환경의 Unity/C# 프로젝트에서 외부 라이브러리 없이 INI 설정 파일을 손쉽게 읽고 쓰기 위한 유틸리티 클래스입니다.  
Win32 API의 `GetPrivateProfileString`/`WritePrivateProfileString` 함수를 P/Invoke로 감싸서, 직관적인 메서드 호출만으로 설정을 관리할 수 있습니다.

## 주요기능
- 섹션·키 단위로 설정 읽기·쓰기
- 키 및 섹션 삭제

## 요구 사항
- Unity 2018 이상 (C# 7.0 이상)
- Windows 플랫폼 (Win32 API 사용)

## 사용 방법
### 네임스페이스 선언
```
using KHE
```

### 메서드 호출
```
// ini 파일 경로 생성
string path = Path.Combine(Application.persistentDataPath, "config.ini");
// 값 쓰기
bool ok = IniFileParser.Write(path, "Audio", "masterVolume", "0.8");
// 값 읽기
string vol = IniFileParser.Read(path, "Audio", "masterVolume", "1.0");
// 키 삭제
IniFileParser.DeleteKey(path, "Audio", "masterVolume");
// 섹션 삭제
IniFileParser.DeleteSection(path, "Audio");
```
## 라이센스
The Unlicense
상업적 이용 배포 수정 가능합니다.
