// Author：yaoshuai
// Email：yscode@126.com
// Date：2022-2-10
// ------------------------------
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;

namespace YCSharp
{
    public static class StringExt
    {
        public static byte[] ToByteArray(this string str, Encoding encoding)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return null;
            }
            return encoding.GetBytes(str);
        }

        public static string MD5(this string str)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return "";
            }
            byte[] result = Encoding.Default.GetBytes(str);
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] output = md5.ComputeHash(result);
            return BitConverter.ToString(output).Replace("-", "");
        }

        public static T ToModel<T>(this string str) where T : class
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return null;
            }
            try
            {
                // 配置序列化设置
                var settings = new JsonSerializerSettings
                {
                    // 忽略 null 值
                    NullValueHandling = NullValueHandling.Ignore,
                    // 为 null 值设置默认值
                    DefaultValueHandling = DefaultValueHandling.Populate,
                    // 遇到错误时继续处理
                    Error = (sender, args) => args.ErrorContext.Handled = true
                };
                return JsonConvert.DeserializeObject<T>(str, settings);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 将字符串分割成字符串集合，item如果是空值将会被忽略
        /// </summary>
        /// <param name="str"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static List<string> ToStringList(this string str, string separator)
        {
            if (string.IsNullOrWhiteSpace(str) || string.IsNullOrWhiteSpace(separator))
            {
                return null;
            }
            List<string> list = new List<string>();
            if (str.Contains(separator))
            {
                string[] strArray = str.Split(new string[] { separator }, StringSplitOptions.None);
                if (strArray != null && strArray.Length > 0)
                {
                    foreach (var itemStr in strArray)
                    {
                        if (string.IsNullOrWhiteSpace(itemStr))
                        {
                            continue;
                        }
                        list.Add(itemStr);
                    }
                }
            }
            else
            {
                list.Add(str);
            }
            return list;
        }

        public static List<int> ToIntList(this string str, string separator)
        {
            List<string> originalList = str.ToStringList(separator);
            if (originalList == null || originalList.Count <= 0)
            {
                return null;
            }
            List<int> intList = new List<int>();
            foreach (var itemStr in originalList)
            {
                try
                {
                    int intValue = Convert.ToInt32(itemStr);
                    intList.Add(intValue);
                }
                catch { }
            }
            return intList;
        }

        public static List<int> ToIntList(this string str, string separator, int exceptValue)
        {
            List<string> originalList = str.ToStringList(separator);
            if (originalList == null || originalList.Count <= 0)
            {
                return null;
            }
            List<int> intList = new List<int>();
            foreach (var itemStr in originalList)
            {
                try
                {
                    int intValue = Convert.ToInt32(itemStr);
                    if (intValue != exceptValue)
                    {
                        intList.Add(intValue);
                    }
                }
                catch { }
            }
            return intList;
        }

        public static List<int> ToIntList(this string str, string separator, List<int> exceptValueList)
        {
            List<string> originalList = str.ToStringList(separator);
            if (originalList == null || originalList.Count <= 0)
            {
                return null;
            }
            List<int> intList = new List<int>();
            foreach (var itemStr in originalList)
            {
                try
                {
                    int intValue = Convert.ToInt32(itemStr);
                    if (exceptValueList != null && exceptValueList.Contains(intValue))
                    {
                        continue;
                    }
                    intList.Add(intValue);
                }
                catch { }
            }
            return intList;
        }

        public static List<float> ToFloatList(this string str, string separator)
        {
            List<string> originalList = str.ToStringList(separator);
            if (originalList == null || originalList.Count <= 0)
            {
                return null;
            }
            List<float> floatList = new List<float>();
            foreach (var itemStr in originalList)
            {
                try
                {
                    float floatValue = (float)Convert.ToDouble(itemStr);
                    floatList.Add(floatValue);
                }
                catch { }
            }
            return floatList;
        }

        public static List<float> ToFloatList(this string str, string separator, float exceptValue)
        {
            List<string> originalList = str.ToStringList(separator);
            if (originalList == null || originalList.Count <= 0)
            {
                return null;
            }
            List<float> floatList = new List<float>();
            foreach (var itemStr in originalList)
            {
                try
                {
                    float floatValue = (float)Convert.ToDouble(itemStr);
                    if (floatValue != exceptValue)
                    {
                        floatList.Add(floatValue);
                    }
                }
                catch { }
            }
            return floatList;
        }

        public static List<float> ToFloatList(this string str, string separator, List<float> exceptValueList)
        {
            List<string> originalList = str.ToStringList(separator);
            if (originalList == null || originalList.Count <= 0)
            {
                return null;
            }
            List<float> floatList = new List<float>();
            foreach (var itemStr in originalList)
            {
                try
                {
                    float floatValue = (float)Convert.ToDouble(itemStr);
                    if (exceptValueList != null && exceptValueList.Contains(floatValue))
                    {
                        continue;
                    }
                    floatList.Add(floatValue);
                }
                catch { }
            }
            return floatList;
        }

        public static List<double> ToDoubleList(this string str, string separator)
        {
            List<string> originalList = str.ToStringList(separator);
            if (originalList == null || originalList.Count <= 0)
            {
                return null;
            }
            List<double> doubleList = new List<double>();
            foreach (var itemStr in originalList)
            {
                try
                {
                    double doubleValue = Convert.ToDouble(itemStr);
                    doubleList.Add(doubleValue);
                }
                catch { }
            }
            return doubleList;
        }

        public static List<double> ToDoubleList(this string str, string separator, double exceptValue)
        {
            List<string> originalList = str.ToStringList(separator);
            if (originalList == null || originalList.Count <= 0)
            {
                return null;
            }
            List<double> doubleList = new List<double>();
            foreach (var itemStr in originalList)
            {
                try
                {
                    double doubleValue = Convert.ToDouble(itemStr);
                    if (doubleValue != exceptValue)
                    {
                        doubleList.Add(doubleValue);
                    }
                }
                catch { }
            }
            return doubleList;
        }

        public static List<double> ToDoubleList(this string str, string separator, List<double> exceptValueList)
        {
            List<string> originalList = str.ToStringList(separator);
            if (originalList == null || originalList.Count <= 0)
            {
                return null;
            }
            List<double> doubleList = new List<double>();
            foreach (var itemStr in originalList)
            {
                try
                {
                    double doubleValue = Convert.ToDouble(itemStr);
                    if (exceptValueList != null && exceptValueList.Contains(doubleValue))
                    {
                        continue;
                    }
                    doubleList.Add(doubleValue);
                }
                catch { }
            }
            return doubleList;
        }
    }
}