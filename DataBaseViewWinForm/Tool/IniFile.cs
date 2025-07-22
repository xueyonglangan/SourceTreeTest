using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DataBaseViewWinForm.Tool
{
    public sealed class IniFile
    {
       
        private readonly string _filePath; // INI 文件路径

        // Windows API 声明
        [DllImport("kernel32", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern long WritePrivateProfileString(string section, string key, string value, string filePath);

        [DllImport("kernel32", CharSet = CharSet.Unicode, SetLastError = true)]
        private static extern int GetPrivateProfileString(string section, string key, string defaultValue, StringBuilder retVal, int size, string filePath);


        // 构造函数：指定 INI 文件路径
        public IniFile(string filePath)
        {
            _filePath = filePath;
        }
        // 写入值到 INI 文件
        public void Write(string section, string key, string value)
        {
            if (WritePrivateProfileString(section, key, value, _filePath)==0)
            {
                throw new IOException($"写入 INI 文件失败: {_filePath}");
            }
        }

        // 从 INI 文件读取值
        public string Read(string section, string key, string defaultValue = "")
        {
            var buffer = new StringBuilder(2048);
            int length = GetPrivateProfileString(section, key, defaultValue, buffer, buffer.Capacity, _filePath);
            return buffer.ToString();
        }

        // 删除指定节中的键
        public void DeleteKey(string section, string key)
        {
            Write(section, key, null); // 写入 null 表示删除键
        }

        // 删除整个节
        public void DeleteSection(string section)
        {
            Write(section, null, null); // 写入 null 节和键表示删除整个节
        }

        // 检查键是否存在
        public bool KeyExists(string section, string key)
        {
            var buffer = new StringBuilder(2048);
            int length = GetPrivateProfileString(section, key, "", buffer, buffer.Capacity, _filePath);
            return length > 0;
        }

    }
}
