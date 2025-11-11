using System;
using System.Collections.Generic;

namespace YCSharp
{
    #region 初始化
    public partial class RandomUtil
    {
        private static Random _random;
        private static Random Random => _random ??= new Random(0);
        public static void ResetSeed(int seed)
        {
            _random = new Random(seed);
        }
    }
    #endregion
    public partial class RandomUtil
    {
        #region RandomValue
        public static int NextValue(int includeMin, int includeMax)
        {
            return Random.Next(includeMin, includeMax + 1);
        }
        public static int NextValue(int includeMin, int includeMax, int exceptValue)
        {
            int result = Random.Next(includeMin, includeMax + 1);
            while (result == exceptValue)
            {
                result = Random.Next(includeMin, includeMax + 1);
            }
            return result;
        }
        #endregion

        #region 百分率随机
        /// <summary>
        /// 随机数是否在最大概率以内
        /// </summary>
        /// <param name="maxPercent">最大概率，0-100的整数值</param>
        /// <returns></returns>
        public static bool Percent(int maxPercent)
        {
            if (maxPercent <= 0) { return false; }
            if (maxPercent >= 100) { return true; }
            return Random.Next(1, 100) <= maxPercent;
        }
        #endregion

        #region 数组随机
        /// <summary>
        /// 将数组打乱后重新排列
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="array">数组</param>
        public static void RandomArray<T>(T[] array)
        {
            if (array.Length <= 1) { return; }
            for (int i = 0; i < array.Length; i++)
            {
                int randomIndex = Random.Next(array.Length);
                T temp = array[i];
                array[i] = array[randomIndex];
                array[randomIndex] = temp;
            }
        }

        /// <summary>
        /// 随机获取数组里的一个元素
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="array">数组</param>
        /// <returns></returns>
        public static T RandomElement<T>(T[] array)
        {
            return array[Random.Next(0, array.Length)];
        }
        #endregion

        #region 集合随机
        /// <summary>
        /// 将集合打乱后重新排列
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="list">集合</param>
        public static void RandomList<T>(List<T> list)
        {
            if (list.Count <= 1) { return; }
            for (int i = 0; i < list.Count; i++)
            {
                int randomIndex = Random.Next(list.Count);
                T temp = list[i];
                list[i] = list[randomIndex];
                list[randomIndex] = temp;
            }
        }

        /// <summary>
        /// 随机获取集合里的一个元素
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="list">集合</param>
        /// <returns></returns>
        public static T RandomElement<T>(List<T> list)
        {
            return list[Random.Next(0, list.Count)];
        }
        #endregion

        #region 随机一个bool值
        /// <summary>
        /// 随机一个bool值
        /// </summary>
        /// <returns></returns>
        public static bool RandomBool()
        {
            return Random.Next(0, 11) % 2 == 0;
        }
        #endregion

        #region 权重随机
        /// <summary>
        /// 根据权重随机出一个权重的索引
        /// </summary>
        /// <param name="weightArray">权重数组</param>
        /// <returns>随机出权重数组中的索引</returns>
        public static int RandomWeightIndex(int[] weightArray)
        {
            // 检查数组是否有效
            if (weightArray == null || weightArray.Length == 0) { throw new Exception("error：权重数组不能为空，且需有元素"); }
            // 计算总权重
            int totalWeight = 0;
            foreach (var item in weightArray)
            {
                if (item < 0) { throw new Exception("error：权重不能为负数"); }
                totalWeight += item;
            }
            // 总权重为0时无法选择
            if (totalWeight == 0) { throw new Exception("error：总权重需大于0"); }
            // 生成随机数
            int randomValue = Random.Next(1, totalWeight + 1);
            // 遍历权重数组，找到随机数所在的区间
            int currentSum = 0;
            for (int i = 0; i < weightArray.Length; i++)
            {
                currentSum += weightArray[i];
                if (randomValue <= currentSum)
                {
                    return i;
                }
            }
            throw new Exception("error：权重随机失败");
        }

        /// <summary>
        /// 根据权重随机出一个权重的索引
        /// </summary>
        /// <param name="weightList">权重集合</param>
        /// <returns>随机出权重集合中的索引</returns>
        public static int RandomWeightIndex(List<int> weightList)
        {
            if (weightList == null || weightList.Count == 0) { throw new Exception("error：权重集合不能为空，且需有元素"); }
            return RandomWeightIndex(weightList.ToArray());
        }
        #endregion
    }
}