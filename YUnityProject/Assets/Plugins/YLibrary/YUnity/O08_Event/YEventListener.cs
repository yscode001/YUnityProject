using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace YUnity
{
    public enum YEventType
    {
        PointerDown, PointerUp, PointerClick,
        BeginDrag, Draging, onEndDrag,
    }

    public partial class YEventListener : MonoBehaviour
    {
        private Action<PointerEventData> onPointerDown;
        private Action<PointerEventData> onPointerUp;
        private Action<PointerEventData> onPointerClick;

        private Action<PointerEventData> onBeginDrag;
        private Action<PointerEventData> onDrag;
        private Action<PointerEventData> onEndDrag;

        public void SetupAction(YEventType eventType, Action<PointerEventData> action)
        {
            switch (eventType)
            {
                case YEventType.PointerDown:
                    onPointerDown = action; break;
                case YEventType.PointerUp:
                    onPointerUp = action; break;
                case YEventType.PointerClick:
                    onPointerClick = action; break;
                case YEventType.BeginDrag:
                    onBeginDrag = action; break;
                case YEventType.Draging:
                    onDrag = action; break;
                case YEventType.onEndDrag:
                    onEndDrag = action; break;
                default: break;
            }
        }
    }
    public partial class YEventListener : IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
    {
        public void OnPointerDown(PointerEventData eventData)
        {
            onPointerDown?.Invoke(eventData);
        }
        public void OnPointerUp(PointerEventData eventData)
        {
            onPointerUp?.Invoke(eventData);
        }
        public void OnPointerClick(PointerEventData eventData)
        {
            onPointerClick?.Invoke(eventData);
        }
    }
    public partial class YEventListener : IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public void OnBeginDrag(PointerEventData eventData)
        {
            onBeginDrag?.Invoke(eventData);
        }
        public void OnDrag(PointerEventData eventData)
        {
            onDrag?.Invoke(eventData);
        }
        public void OnEndDrag(PointerEventData eventData)
        {
            onEndDrag?.Invoke(eventData);
        }
    }
}