// Author：yaoshuai
// Email：yscode@126.com
// Date：2022-2-10
// ------------------------------
using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace YCSharp
{
    public static class DictionaryExt
    {
        public static void AddOrUpdate<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, TValue value)
        {
            if (dict.ContainsKey(key))
            {
                dict[key] = value;
            }
            else
            {
                dict.Add(key, value);
            }
        }
        public static void For<TKey, TValue>(this Dictionary<TKey, TValue> dict, Predicate<(TKey key, TValue value)> condition, Action<(TKey key, TValue value)> doAction)
        {
            foreach (var item in dict)
            {
                if (condition == null)
                {
                    doAction?.Invoke((item.Key, item.Value));
                }
                else if (condition.Invoke((item.Key, item.Value)))
                {
                    doAction?.Invoke((item.Key, item.Value));
                }
            }
        }
        public static Dictionary<TKey, TValue> DeepCopy<TKey, TValue>(this Dictionary<TKey, TValue> dict, Predicate<(TKey key, TValue value)> condition)
        {
            Dictionary<TKey, TValue> newDict = new Dictionary<TKey, TValue>();
            dict.For(condition, data =>
            {
                newDict.AddOrUpdate(data.key, data.value);
            });
            return newDict;
        }

        public static string ToJsonString<TKey, TValue>(this Dictionary<TKey, TValue> dict)
        {
            if (dict == null || dict.Count == 0)
            {
                return null;
            }
            try
            {
                return JsonConvert.SerializeObject(dict);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static T ToModel<T, TKey, TValue>(this Dictionary<TKey, TValue> dict) where T : class
        {
            if (dict == null || dict.Count == 0)
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
                string jsonString = JsonConvert.SerializeObject(dict);
                return JsonConvert.DeserializeObject<T>(jsonString, settings);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 创建并返回一个新的字典，新字典是2个参数字典的并集，dict1优先级低，dict2优先级高，key如果重复，优先使用dict2的value
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dict1"></param>
        /// <param name="dict2"></param>
        /// <returns></returns>
        public static Dictionary<TKey, TValue> Merge<TKey, TValue>(this Dictionary<TKey, TValue> dict1, Dictionary<TKey, TValue> dict2)
        {
            Dictionary<TKey, TValue> newDict = new Dictionary<TKey, TValue>();
            if (dict1 != null)
            {
                foreach (var item in dict1)
                {
                    newDict.AddOrUpdate(item.Key, item.Value);
                }
            }
            if (dict2 != null)
            {
                foreach (var item in dict2)
                {
                    newDict.AddOrUpdate(item.Key, item.Value);
                }
            }
            return newDict;
        }
    }
}