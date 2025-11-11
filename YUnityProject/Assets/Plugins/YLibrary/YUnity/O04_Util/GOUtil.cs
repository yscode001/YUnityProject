using System;
using UnityEngine;

namespace YUnity
{
    public class GOUtil
    {
        public static GameObject CreateEmptyGO(Transform parentT, string name)
        {
            GameObject go;
            if (string.IsNullOrWhiteSpace(name))
            {
                go = new GameObject();
            }
            else
            {
                go = new GameObject(name);
            }
            if (parentT != null)
            {
                go.transform.SetParent(parentT, false);
            }
            go.transform.ResetLocal();
            return go;
        }

        public static T CreateEmptyGO<T>(Transform parentT, string name) where T : Component
        {
            return CreateEmptyGO(parentT, name).AddComponent<T>();
        }

        public static Tuple<T1, T2> CreateEmptyGO<T1, T2>(Transform parentT, string name)
            where T1 : Component
            where T2 : Component
        {
            GameObject go = CreateEmptyGO(parentT, name);
            return new Tuple<T1, T2>(go.AddComponent<T1>(), go.AddComponent<T2>());
        }

        public static Tuple<T1, T2, T3> CreateEmptyGO<T1, T2, T3>(Transform parentT, string name)
            where T1 : Component
            where T2 : Component
            where T3 : Component
        {
            GameObject go = CreateEmptyGO(parentT, name);
            return new Tuple<T1, T2, T3>(go.AddComponent<T1>(), go.AddComponent<T2>(), go.AddComponent<T3>());
        }

        public static Tuple<T1, T2, T3, T4> CreateEmptyGO<T1, T2, T3, T4>(Transform parentT, string name)
            where T1 : Component
            where T2 : Component
            where T3 : Component
            where T4 : Component
        {
            GameObject go = CreateEmptyGO(parentT, name);
            return new Tuple<T1, T2, T3, T4>(go.AddComponent<T1>(), go.AddComponent<T2>(), go.AddComponent<T3>(), go.AddComponent<T4>());
        }

        public static Tuple<T1, T2, T3, T4, T5> CreateEmptyGO<T1, T2, T3, T4, T5>(Transform parentT, string name)
            where T1 : Component
            where T2 : Component
            where T3 : Component
            where T4 : Component
            where T5 : Component
        {
            GameObject go = CreateEmptyGO(parentT, name);
            return new Tuple<T1, T2, T3, T4, T5>(go.AddComponent<T1>(), go.AddComponent<T2>(), go.AddComponent<T3>(), go.AddComponent<T4>(), go.AddComponent<T5>());
        }
    }
}