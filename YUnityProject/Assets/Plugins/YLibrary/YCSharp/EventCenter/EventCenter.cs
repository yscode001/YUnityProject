// Author：yaoshuai
// Email：yscode@126.com
// Date：2021-11-25
// ------------------------------
using System;
using System.Collections.Generic;

namespace YCSharp
{
    public partial class EventCenter
    {
        private EventCenter() { }
        private static readonly Dictionary<string, Delegate> eventTable = new Dictionary<string, Delegate>();
    }

    public partial class EventCenter
    {
        private static void OnListenerAdding(string eventType, Delegate eventMethod)
        {
            if (string.IsNullOrWhiteSpace(eventType))
            {
                throw new Exception("EventCenter AddListener Error，事件名称不能为空");
            }
            if (eventMethod == null)
            {
                throw new Exception("EventCenter AddListener Error，事件不能为空");
            }
            if (!eventTable.ContainsKey(eventType))
            {
                eventTable.Add(eventType, null);
            }
            Delegate d = eventTable[eventType];
            if (d != null && d.GetType() != eventMethod.GetType())
            {
                throw new Exception(string.Format("EventCenter AddListener Error，事件：{0}，队列委托类型：{1}，要添加的委托类型：{2}", eventType, d.GetType(), eventMethod.GetType()));
            }
        }
        private static void OnListenerRemoving(string eventType, Delegate eventMethod)
        {
            if (string.IsNullOrWhiteSpace(eventType))
            {
                throw new Exception("EventCenter RemoveListener Error，事件名称不能为空");
            }
            if (eventMethod == null)
            {
                throw new Exception("EventCenter RemoveListener Error，事件不能为空");
            }
            if (eventTable.ContainsKey(eventType))
            {
                Delegate d = eventTable[eventType];
                if (d == null)
                {
                    throw new Exception(string.Format("EventCenter RemoveListener Error，事件{0}没有对应的委托", eventType));
                }
                else if (d.GetType() != eventMethod.GetType())
                {
                    throw new Exception(string.Format("EventCenter RemoveListener Error，事件：{0}，队列委托类型：{1}，要移除的委托类型：{2}", eventType, d.GetType(), eventMethod.GetType()));
                }
            }
            else
            {
                throw new Exception(string.Format("EventCenter RemoveListener Error，没有注册事件：{0}", eventType));
            }
        }
        private static void OnListenerRemoved(string eventType)
        {
            if (string.IsNullOrWhiteSpace(eventType))
            {
                throw new Exception("EventCenter RemoveListener Error，事件名称不能为空");
            }
            if (eventTable.ContainsKey(eventType) && eventTable[eventType] == null)
            {
                eventTable.Remove(eventType);
            }
        }
    }

    #region 没有参数
    public partial class EventCenter
    {
        public static void AddListener(string eventType, EventDelegate.DelegateMethod eventMethod)
        {
            OnListenerAdding(eventType, eventMethod);
            eventTable[eventType] = (EventDelegate.DelegateMethod)eventTable[eventType] + eventMethod;
        }
        public static void AddListenerAndExecuteOnceImmediate(string eventType, EventDelegate.DelegateMethod eventMethod)
        {
            AddListener(eventType, eventMethod);
            eventMethod?.Invoke();
        }
        public static void RemoveListener(string eventType, EventDelegate.DelegateMethod eventMethod)
        {
            OnListenerRemoving(eventType, eventMethod);
            eventTable[eventType] = (EventDelegate.DelegateMethod)eventTable[eventType] - eventMethod;
            OnListenerRemoved(eventType);
        }
        public static void Broadcast(string eventType)
        {
            if (eventTable.TryGetValue(eventType, out Delegate d) && d != null)
            {
                (d as EventDelegate.DelegateMethod).Invoke();
            }
        }
    }
    #endregion

    #region 1个参数
    public partial class EventCenter
    {
        public static void AddListener<T>(string eventType, EventDelegate.DelegateMethod<T> eventMethod)
        {
            OnListenerAdding(eventType, eventMethod);
            eventTable[eventType] = (EventDelegate.DelegateMethod<T>)eventTable[eventType] + eventMethod;
        }
        public static void AddListenerAndExecuteOnceImmediate<T>(string eventType, EventDelegate.DelegateMethod<T> eventMethod, T t)
        {
            AddListener<T>(eventType, eventMethod);
            eventMethod?.Invoke(t);
        }
        public static void RemoveListener<T>(string eventType, EventDelegate.DelegateMethod<T> eventMethod)
        {
            OnListenerRemoving(eventType, eventMethod);
            eventTable[eventType] = (EventDelegate.DelegateMethod<T>)eventTable[eventType] - eventMethod;
            OnListenerRemoved(eventType);
        }
        public static void Broadcast<T>(string eventType, T arg)
        {
            if (eventTable.TryGetValue(eventType, out Delegate d) && d != null)
            {
                (d as EventDelegate.DelegateMethod<T>).Invoke(arg);
            }
        }
    }
    #endregion

    #region 2个参数
    public partial class EventCenter
    {
        public static void AddListener<T1, T2>(string eventType, EventDelegate.DelegateMethod<T1, T2> eventMethod)
        {
            OnListenerAdding(eventType, eventMethod);
            eventTable[eventType] = (EventDelegate.DelegateMethod<T1, T2>)eventTable[eventType] + eventMethod;
        }
        public static void AddListenerAndExecuteOnceImmediate<T1, T2>(string eventType, EventDelegate.DelegateMethod<T1, T2> eventMethod, T1 t1, T2 t2)
        {
            AddListener<T1, T2>(eventType, eventMethod);
            eventMethod?.Invoke(t1, t2);
        }
        public static void RemoveListener<T1, T2>(string eventType, EventDelegate.DelegateMethod<T1, T2> eventMethod)
        {
            OnListenerRemoving(eventType, eventMethod);
            eventTable[eventType] = (EventDelegate.DelegateMethod<T1, T2>)eventTable[eventType] - eventMethod;
            OnListenerRemoved(eventType);
        }
        public static void Broadcast<T1, T2>(string eventType, T1 arg1, T2 arg2)
        {
            if (eventTable.TryGetValue(eventType, out Delegate d) && d != null)
            {
                (d as EventDelegate.DelegateMethod<T1, T2>).Invoke(arg1, arg2);
            }
        }
    }
    #endregion

    #region 3个参数
    public partial class EventCenter
    {
        public static void AddListener<T1, T2, T3>(string eventType, EventDelegate.DelegateMethod<T1, T2, T3> eventMethod)
        {
            OnListenerAdding(eventType, eventMethod);
            eventTable[eventType] = (EventDelegate.DelegateMethod<T1, T2, T3>)eventTable[eventType] + eventMethod;
        }
        public static void AddListenerAndExecuteOnceImmediate<T1, T2, T3>(string eventType, EventDelegate.DelegateMethod<T1, T2, T3> eventMethod, T1 t1, T2 t2, T3 t3)
        {
            AddListener<T1, T2, T3>(eventType, eventMethod);
            eventMethod?.Invoke(t1, t2, t3);
        }
        public static void RemoveListener<T1, T2, T3>(string eventType, EventDelegate.DelegateMethod<T1, T2, T3> eventMethod)
        {
            OnListenerRemoving(eventType, eventMethod);
            eventTable[eventType] = (EventDelegate.DelegateMethod<T1, T2, T3>)eventTable[eventType] - eventMethod;
            OnListenerRemoved(eventType);
        }
        public static void Broadcast<T1, T2, T3>(string eventType, T1 arg1, T2 arg2, T3 arg3)
        {
            if (eventTable.TryGetValue(eventType, out Delegate d) && d != null)
            {
                (d as EventDelegate.DelegateMethod<T1, T2, T3>).Invoke(arg1, arg2, arg3);
            }
        }
    }
    #endregion

    #region 4个参数
    public partial class EventCenter
    {
        public static void AddListener<T1, T2, T3, T4>(string eventType, EventDelegate.DelegateMethod<T1, T2, T3, T4> eventMethod)
        {
            OnListenerAdding(eventType, eventMethod);
            eventTable[eventType] = (EventDelegate.DelegateMethod<T1, T2, T3, T4>)eventTable[eventType] + eventMethod;
        }
        public static void AddListenerAndExecuteOnceImmediate<T1, T2, T3, T4>(string eventType, EventDelegate.DelegateMethod<T1, T2, T3, T4> eventMethod, T1 t1, T2 t2, T3 t3, T4 t4)
        {
            AddListener<T1, T2, T3, T4>(eventType, eventMethod);
            eventMethod?.Invoke(t1, t2, t3, t4);
        }
        public static void RemoveListener<T1, T2, T3, T4>(string eventType, EventDelegate.DelegateMethod<T1, T2, T3, T4> eventMethod)
        {
            OnListenerRemoving(eventType, eventMethod);
            eventTable[eventType] = (EventDelegate.DelegateMethod<T1, T2, T3, T4>)eventTable[eventType] - eventMethod;
            OnListenerRemoved(eventType);
        }
        public static void Broadcast<T1, T2, T3, T4>(string eventType, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            if (eventTable.TryGetValue(eventType, out Delegate d) && d != null)
            {
                (d as EventDelegate.DelegateMethod<T1, T2, T3, T4>).Invoke(arg1, arg2, arg3, arg4);
            }
        }
    }
    #endregion

    #region 5个参数
    public partial class EventCenter
    {
        public static void AddListener<T1, T2, T3, T4, T5>(string eventType, EventDelegate.DelegateMethod<T1, T2, T3, T4, T5> eventMethod)
        {
            OnListenerAdding(eventType, eventMethod);
            eventTable[eventType] = (EventDelegate.DelegateMethod<T1, T2, T3, T4, T5>)eventTable[eventType] + eventMethod;
        }
        public static void AddListenerAndExecuteOnceImmediate<T1, T2, T3, T4, T5>(string eventType, EventDelegate.DelegateMethod<T1, T2, T3, T4, T5> eventMethod, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5)
        {
            AddListener<T1, T2, T3, T4, T5>(eventType, eventMethod);
            eventMethod?.Invoke(t1, t2, t3, t4, t5);
        }
        public static void RemoveListener<T1, T2, T3, T4, T5>(string eventType, EventDelegate.DelegateMethod<T1, T2, T3, T4, T5> eventMethod)
        {
            OnListenerRemoving(eventType, eventMethod);
            eventTable[eventType] = (EventDelegate.DelegateMethod<T1, T2, T3, T4, T5>)eventTable[eventType] - eventMethod;
            OnListenerRemoved(eventType);
        }
        public static void Broadcast<T1, T2, T3, T4, T5>(string eventType, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        {
            if (eventTable.TryGetValue(eventType, out Delegate d) && d != null)
            {
                (d as EventDelegate.DelegateMethod<T1, T2, T3, T4, T5>).Invoke(arg1, arg2, arg3, arg4, arg5);
            }
        }
    }
    #endregion

    #region 6个参数
    public partial class EventCenter
    {
        public static void AddListener<T1, T2, T3, T4, T5, T6>(string eventType, EventDelegate.DelegateMethod<T1, T2, T3, T4, T5, T6> eventMethod)
        {
            OnListenerAdding(eventType, eventMethod);
            eventTable[eventType] = (EventDelegate.DelegateMethod<T1, T2, T3, T4, T5, T6>)eventTable[eventType] + eventMethod;
        }
        public static void AddListenerAndExecuteOnceImmediate<T1, T2, T3, T4, T5, T6>(string eventType, EventDelegate.DelegateMethod<T1, T2, T3, T4, T5, T6> eventMethod, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6)
        {
            AddListener<T1, T2, T3, T4, T5, T6>(eventType, eventMethod);
            eventMethod?.Invoke(t1, t2, t3, t4, t5, t6);
        }
        public static void RemoveListener<T1, T2, T3, T4, T5, T6>(string eventType, EventDelegate.DelegateMethod<T1, T2, T3, T4, T5, T6> eventMethod)
        {
            OnListenerRemoving(eventType, eventMethod);
            eventTable[eventType] = (EventDelegate.DelegateMethod<T1, T2, T3, T4, T5, T6>)eventTable[eventType] - eventMethod;
            OnListenerRemoved(eventType);
        }
        public static void Broadcast<T1, T2, T3, T4, T5, T6>(string eventType, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
        {
            if (eventTable.TryGetValue(eventType, out Delegate d) && d != null)
            {
                (d as EventDelegate.DelegateMethod<T1, T2, T3, T4, T5, T6>).Invoke(arg1, arg2, arg3, arg4, arg5, arg6);
            }
        }
    }
    #endregion

    #region 7个参数
    public partial class EventCenter
    {
        public static void AddListener<T1, T2, T3, T4, T5, T6, T7>(string eventType, EventDelegate.DelegateMethod<T1, T2, T3, T4, T5, T6, T7> eventMethod)
        {
            OnListenerAdding(eventType, eventMethod);
            eventTable[eventType] = (EventDelegate.DelegateMethod<T1, T2, T3, T4, T5, T6, T7>)eventTable[eventType] + eventMethod;
        }
        public static void AddListenerAndExecuteOnceImmediate<T1, T2, T3, T4, T5, T6, T7>(string eventType, EventDelegate.DelegateMethod<T1, T2, T3, T4, T5, T6, T7> eventMethod, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7)
        {
            AddListener<T1, T2, T3, T4, T5, T6, T7>(eventType, eventMethod);
            eventMethod?.Invoke(t1, t2, t3, t4, t5, t6, t7);
        }
        public static void RemoveListener<T1, T2, T3, T4, T5, T6, T7>(string eventType, EventDelegate.DelegateMethod<T1, T2, T3, T4, T5, T6, T7> eventMethod)
        {
            OnListenerRemoving(eventType, eventMethod);
            eventTable[eventType] = (EventDelegate.DelegateMethod<T1, T2, T3, T4, T5, T6, T7>)eventTable[eventType] - eventMethod;
            OnListenerRemoved(eventType);
        }
        public static void Broadcast<T1, T2, T3, T4, T5, T6, T7>(string eventType, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
        {
            if (eventTable.TryGetValue(eventType, out Delegate d) && d != null)
            {
                (d as EventDelegate.DelegateMethod<T1, T2, T3, T4, T5, T6, T7>).Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7);
            }
        }
    }
    #endregion

    #region 8个参数
    public partial class EventCenter
    {
        public static void AddListener<T1, T2, T3, T4, T5, T6, T7, T8>(string eventType, EventDelegate.DelegateMethod<T1, T2, T3, T4, T5, T6, T7, T8> eventMethod)
        {
            OnListenerAdding(eventType, eventMethod);
            eventTable[eventType] = (EventDelegate.DelegateMethod<T1, T2, T3, T4, T5, T6, T7, T8>)eventTable[eventType] + eventMethod;
        }
        public static void AddListenerAndExecuteOnceImmediate<T1, T2, T3, T4, T5, T6, T7, T8>(string eventType, EventDelegate.DelegateMethod<T1, T2, T3, T4, T5, T6, T7, T8> eventMethod, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8)
        {
            AddListener<T1, T2, T3, T4, T5, T6, T7, T8>(eventType, eventMethod);
            eventMethod?.Invoke(t1, t2, t3, t4, t5, t6, t7, t8);
        }
        public static void RemoveListener<T1, T2, T3, T4, T5, T6, T7, T8>(string eventType, EventDelegate.DelegateMethod<T1, T2, T3, T4, T5, T6, T7, T8> eventMethod)
        {
            OnListenerRemoving(eventType, eventMethod);
            eventTable[eventType] = (EventDelegate.DelegateMethod<T1, T2, T3, T4, T5, T6, T7, T8>)eventTable[eventType] - eventMethod;
            OnListenerRemoved(eventType);
        }
        public static void Broadcast<T1, T2, T3, T4, T5, T6, T7, T8>(string eventType, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8)
        {
            if (eventTable.TryGetValue(eventType, out Delegate d) && d != null)
            {
                (d as EventDelegate.DelegateMethod<T1, T2, T3, T4, T5, T6, T7, T8>).Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
            }
        }
    }
    #endregion

    #region 9个参数
    public partial class EventCenter
    {
        public static void AddListener<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string eventType, EventDelegate.DelegateMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9> eventMethod)
        {
            OnListenerAdding(eventType, eventMethod);
            eventTable[eventType] = (EventDelegate.DelegateMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9>)eventTable[eventType] + eventMethod;
        }
        public static void AddListenerAndExecuteOnceImmediate<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string eventType, EventDelegate.DelegateMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9> eventMethod, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9)
        {
            AddListener<T1, T2, T3, T4, T5, T6, T7, T8, T9>(eventType, eventMethod);
            eventMethod?.Invoke(t1, t2, t3, t4, t5, t6, t7, t8, t9);
        }
        public static void RemoveListener<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string eventType, EventDelegate.DelegateMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9> eventMethod)
        {
            OnListenerRemoving(eventType, eventMethod);
            eventTable[eventType] = (EventDelegate.DelegateMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9>)eventTable[eventType] - eventMethod;
            OnListenerRemoved(eventType);
        }
        public static void Broadcast<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string eventType, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9)
        {
            if (eventTable.TryGetValue(eventType, out Delegate d) && d != null)
            {
                (d as EventDelegate.DelegateMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9>).Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
            }
        }
    }
    #endregion

    #region 10个参数
    public partial class EventCenter
    {
        public static void AddListener<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(string eventType, EventDelegate.DelegateMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> eventMethod)
        {
            OnListenerAdding(eventType, eventMethod);
            eventTable[eventType] = (EventDelegate.DelegateMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>)eventTable[eventType] + eventMethod;
        }
        public static void AddListenerAndExecuteOnceImmediate<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(string eventType, EventDelegate.DelegateMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> eventMethod, T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9, T10 t10)
        {
            AddListener<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(eventType, eventMethod);
            eventMethod?.Invoke(t1, t2, t3, t4, t5, t6, t7, t8, t9, t10);
        }
        public static void RemoveListener<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(string eventType, EventDelegate.DelegateMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> eventMethod)
        {
            OnListenerRemoving(eventType, eventMethod);
            eventTable[eventType] = (EventDelegate.DelegateMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>)eventTable[eventType] - eventMethod;
            OnListenerRemoved(eventType);
        }
        public static void Broadcast<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(string eventType, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10)
        {
            if (eventTable.TryGetValue(eventType, out Delegate d) && d != null)
            {
                (d as EventDelegate.DelegateMethod<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>).Invoke(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
            }
        }
    }
    #endregion
}