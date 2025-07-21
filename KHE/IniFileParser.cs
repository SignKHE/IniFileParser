using System.IO; // 파일 존재 확인용
using System.Runtime.InteropServices; // dll 임포트 용
using System.Text; // 문자열 생성용
using UnityEngine; // 유니티 로그 작성용

//작성자 : 길한얼
//파일버전 : 2025-07-21-00
namespace KHE
{
    /// <summary>
    /// ini 파일 파싱 클래스
    /// </summary>
    public static class IniFileParser
    {
        #region DLL_Imports
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        static extern bool WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);
        #endregion
        
        /// <summary>
        /// ini 파일 데이터 값 쓰기
        /// </summary>
        /// <param name="path">ini 파일 경로</param>
        /// <param name="section">섹션 이름</param>
        /// <param name="key">키 이름</param>
        /// <param name="value">값</param>
        /// <returns>성공?</returns>
        public static bool Write(string path, string section, string key, string value)
        {
            if (!File.Exists(path))
            {
                Debug.LogWarning($"경로에 INI 파일이 없습니다. ini를 생성합니다. ({key}:{value})");
                File.WriteAllText(path, "");
            }

            if (WritePrivateProfileString(section, key, value, path))
            {
                Debug.Log($"INI 쓰기 성공 ({key}:{value})");
                return true;
            }
            else
            {
                Debug.LogError($"INI 쓰기 실패 ({key}:{value}) : {Marshal.GetLastWin32Error()}");
                return false;
            }
        }

        /// <summary>
        /// ini 파일 데이터 값 읽기
        /// </summary>
        /// <param name="path">ini 파일 경로</param>
        /// <param name="section">섹션 이름</param>
        /// <param name="key">키 이름</param>
        /// <param name="defaultValue">기본 값</param>
        /// <param name="bufferSize">버퍼 사이즈</param>
        /// <returns>값</returns>
        public static string Read(string path, string section, string key, string defaultValue = "Error", int bufferSize = 256)
        {
            if (!File.Exists(path))
            {
                Debug.LogError($"경로에 INI 파일이 없습니다. ({key}:{defaultValue})");
                return defaultValue;
            }

            StringBuilder value = new StringBuilder(bufferSize);
            if (GetPrivateProfileString(section, key, defaultValue, value, value.Capacity, path) == bufferSize-1)
            {
                Debug.LogError($"INI 읽기 실패, 재시도 ({key}:{value}) : {Marshal.GetLastWin32Error()}");
                return Read(path, section, key, defaultValue, bufferSize * 2);
            }
            else
            {
                Debug.Log($"INI 읽기 성공 ({key}:{value})");
                return value.ToString();
            }
        }
        
        /// <summary>
        /// ini 파일 키 제거
        /// </summary>
        /// <param name="path">ini 파일 경로</param>
        /// <param name="section">섹션 이름</param>
        /// <param name="key">키 이름</param>
        /// <returns>성공?</returns>
        public static bool DeleteKey(string path, string section, string key)
            => WritePrivateProfileString(section, key, null, path);

        /// <summary>
        /// ini 파일 섹션 제거
        /// </summary>
        /// <param name="path">ini 파일 경로</param>
        /// <param name="section">섹션 이름</param>
        /// <returns>성공?</returns>
        public static bool DeleteSection(string path, string section)
            => WritePrivateProfileString(section, null, null, path);
    }
}