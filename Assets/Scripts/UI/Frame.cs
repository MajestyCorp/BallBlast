using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BallBlast.UI
{
    public enum EFrameAnimation { None, Popup }

    [RequireComponent(typeof(CanvasGroup))]
    public abstract class Frame : MonoBehaviour
    {
        public const float AnimationTime = 0.2f;

        [SerializeField]
        private EFrameAnimation frameAnimation = EFrameAnimation.None;

        public bool IsVisible { get; private set; }

        private Coroutine _animationCoroutine;
        private CanvasGroup _canvasGroup;
        private RectTransform _rect;
        private AnimationCurve _curve;

        protected virtual void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            _rect = GetComponent<RectTransform>();

            InitCurve();

            if (!IsVisible)
                gameObject.SetActive(false);
        }

        private void InitCurve()
        {
            _curve = new AnimationCurve();

            switch (frameAnimation)
            {
                case EFrameAnimation.None:
                    _curve.AddKey(0, 1);
                    break;
                case EFrameAnimation.Popup:
                    _curve.AddKey(0, 0.8f);
                    _curve.AddKey(0.8f, 1.1f);
                    _curve.AddKey(1f, 1f);
                    break;
            }
        }

        public void Toggle(bool show)
        {
            if (show)
                Show();
            else
                Hide();
        }

        public void Show()
        {
            if (IsVisible)
                return;
            IsVisible = true;

            if (_animationCoroutine != null)
                StopCoroutine(_animationCoroutine);

            gameObject.SetActive(true);
            _animationCoroutine = StartCoroutine(ShowInternal());

            AfterShow();
        }

        protected virtual void AfterShow()
        {

        }

        public void Hide()
        {
            if (!IsVisible)
                return;

            if (_animationCoroutine != null)
                StopCoroutine(_animationCoroutine);

            _animationCoroutine = StartCoroutine(HideInternal());
            IsVisible = false;

            AfterHide();
        }

        protected virtual void AfterHide()
        {

        }

        private IEnumerator HideInternal()
        {
            var speed = 1f / AnimationTime;

            _canvasGroup.alpha = 1f;
            _canvasGroup.interactable = false;

            while (_canvasGroup.alpha > 0f)
            {
                _canvasGroup.alpha = Mathf.Max(0f, _canvasGroup.alpha - speed * Time.unscaledDeltaTime);
                var value = _curve.Evaluate(_canvasGroup.alpha);
                _rect.localScale = value * Vector3.one;
                yield return null;
            }

            gameObject.SetActive(false);
        }

        private IEnumerator ShowInternal()
        {
            var speed = 1f / AnimationTime;

            _canvasGroup.alpha = 0f;

            while (_canvasGroup.alpha < 1f)
            {
                _canvasGroup.alpha = Mathf.Min(1f, _canvasGroup.alpha + speed * Time.unscaledDeltaTime);
                var value = _curve.Evaluate(_canvasGroup.alpha);
                _rect.localScale = value * Vector3.one;
                yield return null;
            }

            _canvasGroup.interactable = true;
        }

    }
}