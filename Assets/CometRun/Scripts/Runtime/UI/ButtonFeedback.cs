using CometRun.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CometRun.UI
{
    // Goes on every button. It shrinks a bit when you press it and bounces
    // back when you let go, makes the buttons feel more like real buttons.
    public class ButtonFeedback : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
    {
        public float pressedScale = 0.94f;
        [Tooltip("Normal size, the selected ship card uses 1.05")]
        public float restScale = 1f;

        Selectable selectable;
        bool pressed;

        void Awake()
        {
            selectable = GetComponent<Selectable>();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (selectable != null && !selectable.interactable) return;
            pressed = true;
            transform.ScaleTo(Vector3.one * restScale * pressedScale, 0.08f, Ease.OutQuad);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            Release();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Release();
        }

        void Release()
        {
            if (!pressed) return;
            pressed = false;
            transform.ScaleTo(Vector3.one * restScale, 0.25f, Ease.OutBack);
        }

        void OnDisable()
        {
            pressed = false;
            TweenRunner.Kill(transform, "scale");
            transform.localScale = Vector3.one * restScale;
        }
    }
}
