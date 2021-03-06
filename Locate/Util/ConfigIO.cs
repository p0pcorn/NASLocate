﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using NASLocate.Util;

namespace NASLocate.Util
{
    public class ConfigIO
    {
        //声明API函数

        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);
        /// <summary> 
        /// 构造方法 
        /// </summary> 
        /// <param name="INIPath">文件路径</param> 
       
        public ConfigIO() { }

        /// <summary> 
        /// 写入INI文件 
        /// </summary> 
        /// <param name="Section">项目名称(如 [TypeName] )</param> 
        /// <param name="Key">键</param> 
        /// <param name="Value">值</param> 
        public static void ConfigWriteValue(string ConfigFilePath, string Section, string Key, string Value, bool ShouldEncrypt = false)
        {
            if (ShouldEncrypt)
                Value = AESEncrypt.Encrypt(Value);
            WritePrivateProfileString(Section, Key, Value, ConfigFilePath);
        }
        /// <summary> 
        /// 读出INI文件 
        /// </summary> 
        /// <param name="Section">项目名称(如 [TypeName] )</param> 
        /// <param name="Key">键</param> 
        public static string ConfigReadValue(string ConfigFilePath, string Section, string Key, bool IsEncrypted = false)
        {
            StringBuilder temp = new StringBuilder(500);
            int i = GetPrivateProfileString(Section, Key, "", temp, 500, ConfigFilePath);
            if (IsEncrypted)
                return AESEncrypt.Decrypt(temp.ToString());

            return temp.ToString();
        }
        /// <summary> 
        /// 验证文件是否存在 
        /// </summary> 
        /// <returns>布尔值</returns> 
        public static bool ExistConfigFile(string ConfigFilePath)
        {
            return File.Exists(ConfigFilePath);
        }

        public static void DeleteConfigFile(string ConfigFilePath)
        {
            if (ExistConfigFile(ConfigFilePath))
                File.Delete(ConfigFilePath);
        }

    }
}
