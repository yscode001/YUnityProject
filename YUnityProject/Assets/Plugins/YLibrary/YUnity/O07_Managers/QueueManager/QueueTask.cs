using System;
using System.Collections.Generic;
using System.Timers;

namespace YUnity
{
    #region 属性定义
    public partial class QueueTask
    {
        // 计算机元年
        private DateTime startDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0);
        // 现在的时间(现在到元年之间的毫秒数)
        private double nowTime;

        // 用于服务器的定时器，客户端不需要(定时方法会在新开辟的子线程中执行)
        private Timer serverTimer;

        // 任务相关
        private static readonly string lockTask = "lockTask";
        // 任务列表
        private List<QueueTimeInfo> taskList = new List<QueueTimeInfo>();

        // 帧任务相关
        private static readonly string lockFrame = "lockFrame";
        // 帧计数器
        private uint frameCounter = 0;
        // 帧任务列表
        private List<QueueFrameInfo> taskFrameList = new List<QueueFrameInfo>();

        // 任务日志打印委托
        private Action<string> taskLog;
    }
    #endregion

    #region 构造函数、轮询检测函数、重置函数
    public partial class QueueTask
    {
        private QueueTask() { }

        /// <summary>
        /// 客户端构造函数，外界需调用Update函数进行轮询
        /// </summary>
        public QueueTask(Action<string> taskLog)
        {
            this.taskLog = taskLog;
        }

        /// <summary>
        /// 服务器构造函数
        /// </summary>
        /// <param name="interval">子线程多少毫秒轮询一次</param>
        public QueueTask(Action<string> taskLog, uint repeatTimeInterval)
        {
            this.taskLog = taskLog;
            if (repeatTimeInterval > 0)
            {
                serverTimer = new Timer(repeatTimeInterval)
                {
                    AutoReset = true
                };
                serverTimer.Elapsed += (object sender, ElapsedEventArgs args) =>
                {
                    LogicTick();
                };
                serverTimer.Start();
            }
        }

        public void LogicTick()
        {
            CheckTimeTask(); // 检测任务
            CheckFrameTask(); // 检测帧任务
        }

        /// <summary>
        /// 重置函数
        /// </summary>
        public void Reset()
        {
            taskList.Clear();
            taskFrameList.Clear();
            frameCounter = 0;
            taskLog = null;
            serverTimer?.Stop();
        }
    }
    #endregion

    #region 线程处理Handle
    public partial class QueueTask
    {
        /*
         使用服务器serverTimer时，callback的执行是在子线程内。使用taskHandleThread，把callback传出去，外界可以把callback保存起来，然后放到主线程中去执行。
         */
        private Action<Action> taskHandleThread;
        public void SetTaskHandleThread(Action<Action> taskHandleThread)
        {
            this.taskHandleThread = taskHandleThread;
        }
    }
    #endregion

    #region 时间工具函数
    public partial class QueueTask
    {
        /// <summary>
        /// 获取计算机元年到现在的毫秒数
        /// </summary>
        /// <returns></returns>
        public double GetUTCMilliseconds()
        {
            return (DateTime.UtcNow - startDateTime).TotalMilliseconds;
        }
    }
    #endregion

    #region 任务的添加与检测
    public partial class QueueTask
    {
        public void AddTimeTask(Action callback, double delayTime, TimeUnit timeUnit)
        {
            if (callback == null || delayTime < 0) { return; }
            double delay = TimeTool.GetMillisecond(delayTime, timeUnit);
            nowTime = GetUTCMilliseconds();
            lock (lockTask)
            {
                taskList.Add(new QueueTimeInfo(callback, nowTime + delay));
            }
        }

        private void CheckTimeTask()
        {
            //遍历检测任务是否达到执行条件
            nowTime = GetUTCMilliseconds();
            for (int index = 0; index < taskList.Count; index++)
            {
                QueueTimeInfo task = taskList[index];
                if (nowTime.CompareTo(task.destTime) < 0) // 时间未到，不执行
                {
                    continue;
                }
                else // 时间已到，执行
                {
                    Action cb = task.callback;
                    if (cb != null)
                    {
                        try
                        {
                            if (taskHandleThread != null)
                            {
                                taskHandleThread(cb);
                            }
                            else
                            {
                                cb.Invoke();
                            }
                        }
                        catch (Exception e)
                        {
                            taskLog?.Invoke($"队列任务执行时出错了，出错函数：CheckTask，出错信息：{e}");
                        }
                        finally
                        {
                            lock (lockTask)
                            {
                                taskList.RemoveAt(index);
                                index--;
                            }
                        }
                    }
                }
            }
        }
    }
    #endregion

    #region 帧任务的添加与检测
    public partial class QueueTask
    {
        public void AddFrameTask(Action callback, uint delayFrame)
        {
            if (callback == null || delayFrame < 0) { return; }
            lock (lockFrame)
            {
                taskFrameList.Add(new QueueFrameInfo(callback, frameCounter + delayFrame));
            }
        }

        private void CheckFrameTask()
        {
            frameCounter += 1;

            // 遍历检测任务是否达到执行条件
            for (int index = 0; index < taskFrameList.Count; index++)
            {
                QueueFrameInfo task = taskFrameList[index];
                if (frameCounter < task.destFrame) // 帧未到，不执行
                {
                    continue;
                }
                else // 帧已到，执行
                {
                    Action cb = task.callback;
                    if (cb != null)
                    {
                        try
                        {
                            if (taskHandleThread != null)
                            {
                                taskHandleThread(cb);
                            }
                            else
                            {
                                cb.Invoke();
                            }
                        }
                        catch (Exception e)
                        {
                            taskLog?.Invoke($"队列任务执行时出错了，出错函数：CheckFrame，出错信息：{e}");
                        }
                        finally
                        {
                            lock (lockFrame)
                            {
                                taskFrameList.RemoveAt(index);
                                index--;
                            }
                        }
                    }
                }
            }
        }
    }
    #endregion
}