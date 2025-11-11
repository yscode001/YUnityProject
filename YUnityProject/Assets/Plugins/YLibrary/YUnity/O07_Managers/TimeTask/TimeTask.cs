using System;
using System.Collections.Generic;

namespace YUnity
{
    #region 各种属性定义
    public partial class TimeTask
    {
        // 全局任务ID，添加定时任务时新任务ID为此值+1
        private uint globalTaskID = 0;
        // 全局任务ID的锁，防止多线程添加定时任务出错(使用场景：添加新任务时、回收废弃任务ID时)
        private static readonly string lockGlobalTaskID = "lockTaskID";

        // 计算机元年
        private readonly DateTime startDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);
        // 现在的时间(现在到元年之间的毫秒数)
        private double nowTime;

        // 任务ID集合
        private readonly List<uint> taskIDList = new List<uint>();
        // 将要回收的任务ID集合
        private readonly List<uint> recycleTaskIDList = new List<uint>();

        // 定时任务相关
        private static readonly string lockTime = "lockTime";
        private readonly List<TimeTaskInfo> taskTimeList = new List<TimeTaskInfo>(); // 定时任务列表
        private readonly List<TimeTaskInfo> tmpAddTimeList = new List<TimeTaskInfo>(); // 定时任务添加时的临时列表
        private readonly List<uint> tmpDeleteTimeList = new List<uint>(); // 定时任务删除时的临时列表

        // 定时帧任务相关
        private static readonly string lockFrame = "lockFrame";
        private uint frameCounter; // 帧计数器
        private readonly List<FrameTaskInfo> taskFrameList = new List<FrameTaskInfo>(); // 定时帧任务列表
        private readonly List<FrameTaskInfo> tmpAddFrameList = new List<FrameTaskInfo>(); // 定时帧任务添加时的临时列表
        private readonly List<uint> tmpDeleteFrameList = new List<uint>(); // 定时帧任务删除时的临时列表

        // 任务日志打印委托
        private Action<string> taskLog;
    }
    #endregion

    #region 构造函数、轮询检测函数、重置函数
    public partial class TimeTask
    {
        private TimeTask() { }

        /// <summary>
        /// 客户端构造函数，外界需调用Update函数进行轮询
        /// </summary>
        public TimeTask(Action<string> taskLog)
        {
            this.taskLog = taskLog;
        }

        public void LogicTick()
        {
            CheckTimeTask(); // 检测定时任务
            CheckFrameTask(); // 检测定时帧任务

            DeleteTimeTask(); // 删除定时任务
            DeleteFrameTask(); // 删除定时帧任务

            RecycleTaskID(); // 回收废弃的定时任务ID
        }

        /// <summary>
        /// 重置函数
        /// </summary>
        public void Reset()
        {
            globalTaskID = 0;
            taskIDList.Clear();
            recycleTaskIDList.Clear();

            tmpAddTimeList.Clear();
            taskTimeList.Clear();
            // 删除不用，Update里会清除

            tmpAddFrameList.Clear();
            taskFrameList.Clear();
            // 删除不用，Update里会清除

            frameCounter = 0;

            taskLog = null;
        }
    }
    #endregion

    #region 时间工具函数
    public partial class TimeTask
    {
        /// <summary>
        /// 获取计算机元年到现在的毫秒数
        /// </summary>
        /// <returns></returns>
        public double GetUTCMilliseconds()
        {
            return (DateTime.UtcNow - startDateTime).TotalMilliseconds;
        }

        public DateTime GetLocalDateTime()
        {
            return TimeZoneInfo.ConvertTime(startDateTime.AddMilliseconds(GetUTCMilliseconds()), TimeZoneInfo.Utc, TimeZoneInfo.Local);
        }

        private string GetTimeStr(int time)
        {
            return time < 10 ? "0" + time : time.ToString();
        }

        /// <summary>
        /// 返回小时分钟秒
        /// </summary>
        /// <returns>"小时分钟秒，11:23:32"</returns>
        public string GetLocalTimeStr()
        {
            DateTime dt = GetLocalDateTime();
            return GetTimeStr(dt.Hour) + ":" + GetTimeStr(dt.Minute) + ":" + GetTimeStr(dt.Second);
        }

        public int GetYear()
        {
            return GetLocalDateTime().Year;
        }
        public int GetMonth()
        {
            return GetLocalDateTime().Month;
        }
        public int GetDay()
        {
            return GetLocalDateTime().Day;
        }
        public int GetWeek()
        {
            return (int)GetLocalDateTime().DayOfWeek;
        }
    }
    #endregion

    #region 工具函数：创建新任务TaskID、回收废弃的TaskID
    public partial class TimeTask
    {
        // 创建新任务ID
        private uint CreateNewTaskID()
        {
            lock (lockGlobalTaskID)
            {
                globalTaskID += 1;
                while (true)
                {
                    if (globalTaskID == uint.MaxValue) // 防止溢出
                    {
                        globalTaskID = 0;
                    }
                    bool used = false; // 是否使用过
                    for (int i = 0; i < taskIDList.Count; i++)
                    {
                        if (globalTaskID == taskIDList[i])
                        {
                            used = true;
                            break;
                        }
                    }
                    if (!used) // 没被使用
                    {
                        taskIDList.Add(globalTaskID);
                        break;
                    }
                    else // 被使用了，加1后重新检测
                    {
                        globalTaskID += 1;
                    }
                }
            }
            return globalTaskID;
        }

        // 对废弃任务的ID进行回收
        private void RecycleTaskID()
        {
            if (recycleTaskIDList.Count <= 0) { return; }
            lock (lockGlobalTaskID)
            {
                for (int i = 0; i < recycleTaskIDList.Count; i++)
                {
                    uint recycleTaskID = recycleTaskIDList[i];
                    for (int j = taskIDList.Count - 1; j >= 0; j--)
                    {
                        if (taskIDList[j] == recycleTaskID)
                        {
                            taskIDList.RemoveAt(j);
                            break;
                        }
                    }
                }
                recycleTaskIDList.Clear(); // 回收完成后清理待回收列表
            }
        }
    }
    #endregion

    #region 定时任务的添加、删除、与替换，操作临时表
    public partial class TimeTask
    {
        public uint AddTimeTask(Action<uint> callback, double delayTime, TimeUnit timeUnit, uint repeatCount)
        {
            if (callback == null || delayTime < 0 || (repeatCount > 1 && delayTime <= 0)) { return 0; }
            uint taskID = CreateNewTaskID(); ;
            double delay = TimeTool.GetMillisecond(delayTime, timeUnit);
            nowTime = GetUTCMilliseconds();
            lock (lockTime)
            {
                tmpAddTimeList.Add(new TimeTaskInfo(taskID, callback, nowTime + delay, delay, repeatCount));
            }
            return taskID;
        }
        public void DeleteTimeTask(uint taskID)
        {
            lock (lockTime)
            {
                tmpDeleteTimeList.Add(taskID);
            }
        }
        public bool ReplaceTimeTask(uint taskID, Action<uint> callback, double delayTime, TimeUnit timeUnit, uint repeatCount)
        {
            if (callback == null || delayTime < 0 || (repeatCount > 1 && delayTime <= 0)) { return false; }
            bool isReplaced = false;
            for (int i = 0; i < taskTimeList.Count; i++)
            {
                if (taskTimeList[i].taskID == taskID)
                {
                    double delay = TimeTool.GetMillisecond(delayTime, timeUnit);
                    nowTime = GetUTCMilliseconds();
                    TimeTaskInfo newTask = new TimeTaskInfo(taskID, callback, nowTime + delay, delay, repeatCount);

                    taskTimeList[i] = newTask;
                    isReplaced = true;
                    break;
                }
            }
            if (!isReplaced)
            {
                for (int i = 0; i < tmpAddTimeList.Count; i++)
                {
                    if (tmpAddTimeList[i].taskID == taskID)
                    {
                        double delay = TimeTool.GetMillisecond(delayTime, timeUnit);
                        nowTime = GetUTCMilliseconds();
                        TimeTaskInfo newTask = new TimeTaskInfo(taskID, callback, nowTime + delay, delay, repeatCount);

                        tmpAddTimeList[i] = newTask;
                        isReplaced = true;
                        break;
                    }
                }
            }
            return isReplaced;
        }
    }
    #endregion

    #region 定时帧任务的添加、删除、与替换，操作临时表
    public partial class TimeTask
    {
        public uint AddFrameTask(Action<uint> callback, uint delayFrame, uint repeatCount)
        {
            if (callback == null || delayFrame < 0 || (repeatCount > 1 && delayFrame < 1)) { return 0; }
            uint taskID = CreateNewTaskID();
            lock (lockFrame)
            {
                tmpAddFrameList.Add(new FrameTaskInfo(taskID, callback, frameCounter + delayFrame, delayFrame, repeatCount));
            }
            return taskID;
        }
        public void DeleteFrameTask(uint taskID)
        {
            lock (lockFrame)
            {
                tmpDeleteFrameList.Add(taskID);
            }
        }
        public bool ReplaceFrameTask(uint taskID, Action<uint> callback, uint delayFrame, uint repeatCount)
        {
            if (callback == null || delayFrame < 0 || (repeatCount > 1 && delayFrame < 1)) { return false; }
            bool isReplaced = false;
            for (int i = 0; i < taskFrameList.Count; i++)
            {
                if (taskFrameList[i].taskID == taskID)
                {
                    FrameTaskInfo newTask = new FrameTaskInfo(taskID, callback, frameCounter + delayFrame, delayFrame, repeatCount);

                    taskFrameList[i] = newTask;
                    isReplaced = true;
                    break;
                }
            }
            if (!isReplaced)
            {
                for (int i = 0; i < tmpAddFrameList.Count; i++)
                {
                    if (tmpAddFrameList[i].taskID == taskID)
                    {
                        FrameTaskInfo newTask = new FrameTaskInfo(taskID, callback, frameCounter + delayFrame, delayFrame, repeatCount);

                        tmpAddFrameList[i] = newTask;
                        isReplaced = true;
                        break;
                    }
                }
            }
            return isReplaced;
        }
    }
    #endregion

    #region 具体的检测、删除定时任务功能的实现
    public partial class TimeTask
    {
        private void CheckTimeTask()
        {
            if (tmpAddTimeList.Count > 0) //加入缓存区中的定时任务
            {
                lock (lockTime)
                {
                    for (int tmpIndex = 0; tmpIndex < tmpAddTimeList.Count; tmpIndex++)
                    {
                        taskTimeList.Add(tmpAddTimeList[tmpIndex]);
                    }
                    tmpAddTimeList.Clear();
                }
            }

            //遍历检测任务是否达到执行条件
            nowTime = GetUTCMilliseconds();
            for (int index = 0; index < taskTimeList.Count; index++)
            {
                TimeTaskInfo task = taskTimeList[index];
                if (nowTime.CompareTo(task.destTime) < 0) // 时间未到，不执行
                {
                    continue;
                }
                else // 时间已到，执行
                {
                    Action<uint> cb = task.callback;
                    if (cb != null)
                    {
                        try
                        {
                            cb.Invoke(task.taskID);
                        }
                        catch (Exception e)
                        {
                            taskLog?.Invoke($"定时任务执行时出错了，出错函数：CheckTimeTask，任务ID：{task.taskID}，出错信息：{e}");
                            DeleteTimeTask(task.taskID);
                        }
                    }
                    if (task.repeatCount == 1) // 移除已经完成的任务
                    {
                        taskTimeList.RemoveAt(index);
                        index--;
                        recycleTaskIDList.Add(task.taskID);
                    }
                    else
                    {
                        if (task.repeatCount != 0) // 0是无限重复循环
                        {
                            task.repeatCount -= 1;
                        }
                        task.destTime += task.repeatInterval;
                    }
                }
            }
        }

        private void CheckFrameTask()
        {
            if (tmpAddFrameList.Count > 0) //加入缓存区中的定时任务
            {
                lock (lockFrame)
                {
                    for (int tmpIndex = 0; tmpIndex < tmpAddFrameList.Count; tmpIndex++)
                    {
                        taskFrameList.Add(tmpAddFrameList[tmpIndex]);
                    }
                    tmpAddFrameList.Clear();
                }
            }

            frameCounter += 1;

            // 遍历检测任务是否达到执行条件
            for (int index = 0; index < taskFrameList.Count; index++)
            {
                FrameTaskInfo task = taskFrameList[index];
                if (frameCounter < task.destFrame) // 帧未到，不执行
                {
                    continue;
                }
                else // 帧已到，执行
                {
                    Action<uint> cb = task.callback;
                    if (cb != null)
                    {
                        try
                        {
                            cb.Invoke(task.taskID);
                        }
                        catch (Exception e)
                        {
                            taskLog?.Invoke($"定时任务执行时出错了，出错函数：CheckFrameTask，任务ID：{task.taskID}，出错信息：{e}");
                            DeleteFrameTask(task.taskID);
                        }
                    }
                    if (task.repeatCount == 1) // 移除已经完成的任务
                    {
                        taskFrameList.RemoveAt(index);
                        index--;
                        recycleTaskIDList.Add(task.taskID);
                    }
                    else
                    {
                        if (task.repeatCount != 0) // 0是无限重复循环
                        {
                            task.repeatCount -= 1;
                        }
                        task.destFrame += task.repeatInterval;
                    }
                }
            }
        }

        private void DeleteTimeTask()
        {
            if (tmpDeleteTimeList.Count > 0)
            {
                lock (lockTime)
                {
                    for (int i = 0; i < tmpDeleteTimeList.Count; i++)
                    {
                        bool isDel = false;
                        uint delTid = tmpDeleteTimeList[i];
                        for (int j = taskTimeList.Count - 1; j >= 0; j--)
                        {
                            TimeTaskInfo task = taskTimeList[j];
                            if (task.taskID == delTid)
                            {
                                isDel = true;
                                taskTimeList.RemoveAt(j);
                                recycleTaskIDList.Add(delTid);
                                break;
                            }
                        }

                        if (isDel)
                        {
                            continue;
                        }

                        for (int j = tmpAddTimeList.Count - 1; j >= 0; j--)
                        {
                            TimeTaskInfo task = tmpAddTimeList[j];
                            if (task.taskID == delTid)
                            {
                                tmpAddTimeList.RemoveAt(j);
                                recycleTaskIDList.Add(delTid);
                                break;
                            }
                        }
                    }
                    tmpDeleteTimeList.Clear();
                }
            }
        }

        private void DeleteFrameTask()
        {
            if (tmpDeleteFrameList.Count > 0)
            {
                lock (lockFrame)
                {
                    for (int i = 0; i < tmpDeleteFrameList.Count; i++)
                    {
                        bool isDel = false;
                        uint delTid = tmpDeleteFrameList[i];
                        for (int j = taskFrameList.Count - 1; j >= 0; j--)
                        {
                            FrameTaskInfo task = taskFrameList[j];
                            if (task.taskID == delTid)
                            {
                                isDel = true;
                                taskFrameList.RemoveAt(j);
                                recycleTaskIDList.Add(delTid);
                                break;
                            }
                        }

                        if (isDel)
                            continue;

                        for (int j = tmpAddFrameList.Count - 1; j >= 0; j--)
                        {
                            FrameTaskInfo task = tmpAddFrameList[j];
                            if (task.taskID == delTid)
                            {
                                tmpAddFrameList.RemoveAt(j);
                                recycleTaskIDList.Add(delTid);
                                break;
                            }
                        }
                    }
                    tmpDeleteFrameList.Clear();
                }
            }
        }
    }
    #endregion
}