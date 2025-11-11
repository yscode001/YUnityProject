using System;

namespace YUnity
{
    public class QueueTimeInfo
    {
        public Action callback;

        /// <summary>
        /// 执行时间，单位毫秒
        /// </summary>
        public double destTime;

        private QueueTimeInfo() { }

        public QueueTimeInfo(Action callback, double destTime)
        {
            this.callback = callback;
            this.destTime = destTime;
        }
    }

    public class QueueFrameInfo
    {
        public Action callback;

        /// <summary>
        /// 执行目标帧
        /// </summary>
        public uint destFrame;

        private QueueFrameInfo() { }

        public QueueFrameInfo(Action callback, uint destFrame)
        {
            this.callback = callback;
            this.destFrame = destFrame;
        }
    }
}