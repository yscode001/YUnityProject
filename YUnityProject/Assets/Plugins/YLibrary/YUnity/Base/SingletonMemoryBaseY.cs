using System;
using System.Reflection;

namespace YUnity
{
    /// <summary>
    /// 单例基类(仅存在于内存中，不继承MonoBehaviour)
    /// </summary>
    public abstract partial class SingletonMemoryBaseY<T> where T : SingletonMemoryBaseY<T>
    {
        protected SingletonMemoryBaseY() { }

        private static readonly Lazy<T> _instance = new Lazy<T>(() =>
        {
            var ctors = typeof(T).GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic);
            var ctor = Array.Find(ctors, c => c.GetParameters().Length == 0);
            if (ctor == null)
            {
                throw new Exception("Non-public ctor() not found");
            }
            T data = ctor.Invoke(null) as T;
            return data;
        });

        public static T Instance => _instance.Value;
    }
}