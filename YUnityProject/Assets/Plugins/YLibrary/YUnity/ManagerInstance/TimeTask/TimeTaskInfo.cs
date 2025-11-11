using System;

namespace YUnity
{
    /// <summary>
    /// 定时任务模型信息
    /// </summary>
    public class TimeTaskInfo
    {
        /// <summary>
        /// 定时任务ID
        /// </summary>
        public uint taskID;

        /// <summary>
        /// 定时任务委托(参数是定时任务ID)
        /// </summary>
        public Action<uint> callback;

        /// <summary>
        /// 执行时间，单位毫秒
        /// </summary>
        public double destTime;

        /// <summary>
        /// 重复执行时间间隔
        /// </summary>
        public double repeatInterval;

        /// <summary>
        /// 重复执行次数，0表示无限重复执行
        /// </summary>
        public uint repeatCount;

        private TimeTaskInfo() { }

        /// <summary>
        /// 定时任务模型构造函数
        /// </summary>
        /// <param name="taskID">定时任务ID</param>
        /// <param name="callback">定时任务回调(参数是定时任务ID)</param>
        /// <param name="destTime">执行时间，单位毫秒</param>
        /// <param name="repeatInterval">重复执行时间间隔</param>
        /// <param name="repeatCount">重复执行次数，0表示无限重复执行</param>
        public TimeTaskInfo(uint taskID, Action<uint> callback, double destTime, double repeatInterval, uint repeatCount)
        {
            this.taskID = taskID;
            this.callback = callback;
            this.destTime = destTime;
            this.repeatInterval = repeatInterval;
            this.repeatCount = repeatCount;
        }
    }

    /// <summary>
    /// 定时帧任务模型信息
    /// </summary>
    public class FrameTaskInfo
    {
        /// <summary>
        /// 定时帧任务ID
        /// </summary>
        public uint taskID;

        /// <summary>
        /// 定时帧任务委托(参数是定时帧任务ID)
        /// </summary>
        public Action<uint> callback;

        /// <summary>
        /// 执行目标帧
        /// </summary>
        public uint destFrame;

        /// <summary>
        /// 重复执行帧间隔
        /// </summary>
        public uint repeatInterval;

        /// <summary>
        /// 重复执行次数，0表示无限重复执行
        /// </summary>
        public uint repeatCount;

        private FrameTaskInfo() { }

        /// <summary>
        /// 定时帧任务构造函数
        /// </summary>
        /// <param name="taskID">定时帧任务ID</param>
        /// <param name="callback">定时帧任务委托(参数是定时帧任务ID)</param>
        /// <param name="destFrame">执行目标帧</param>
        /// <param name="repeatInterval">重复执行帧间隔</param>
        /// <param name="repeatCount">重复执行次数，0表示无限重复执行</param>
        public FrameTaskInfo(uint taskID, Action<uint> callback, uint destFrame, uint repeatInterval, uint repeatCount)
        {
            this.taskID = taskID;
            this.callback = callback;
            this.destFrame = destFrame;
            this.repeatInterval = repeatInterval;
            this.repeatCount = repeatCount;
        }
    }
}