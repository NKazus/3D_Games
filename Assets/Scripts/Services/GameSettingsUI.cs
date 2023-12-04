using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace FitTheSize.GameServices
{
    public class GameSettingsUI : MonoBehaviour
    {
        [SerializeField] private RectTransform handleRectTransform;

        private Toggle toggle;
        private Vector2 handlePosition;

        #region MONO
        private void Awake()
        {
            toggle = GetComponent<Toggle>();
            handlePosition = handleRectTransform.anchoredPosition;
            if (toggle.isOn)
            {
                Switch(true, false);
            }
        }

        private void OnEnable()
        {
            toggle.onValueChanged.AddListener((bool isOn) => Switch(isOn));
        }

        private void OnDisable()
        {
            toggle.onValueChanged.RemoveAllListeners();
            DOTween.Kill("toggle");
        }
        #endregion

        private void Switch(bool isOn, bool animate = true)
        {
            if (!animate)
            {
                handleRectTransform.anchoredPosition = isOn ? handlePosition * (-1) : handlePosition;
            }
            else
            {
                handleRectTransform.DOAnchorPos(isOn ? handlePosition * (-1) : handlePosition, .4f).SetId(this)
                    .SetId("toggle")
                    .SetEase(Ease.InOutBack);
            }
        }
    }
}
