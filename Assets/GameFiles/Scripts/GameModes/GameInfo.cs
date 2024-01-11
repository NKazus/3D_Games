using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public enum MessageType
{
    CheckUnique,
    CheckEqual,
    ForceUnique,
    AdvanceEnabled,
    AdvanceDisabled
}

[System.Serializable]
public struct MessagePreset
{
    public MessageType type;
    public string message;
}

public class GameInfo : MonoBehaviour
{
    [SerializeField] private RectTransform targetPanel;
    [SerializeField] private Text targetText;
    [SerializeField] private MessagePreset[] presets;

    private GameObject target;

    private void Awake()
    {
        target = targetPanel.gameObject;
    }

    private void OnEnable()
    {
        targetPanel.localScale = Vector3.zero;
        target.SetActive(false);
    }

    public void ShowInfo(MessageType type)
    {
        MessagePreset currentSetup = presets[0];
        bool presetFound = false;
        for (int i = 0; i < presets.Length; i++)
        {
            if (presets[i].type == type)
            {
                currentSetup = presets[i];
                presetFound = true;
            }
        }

        if (!presetFound)
        {
            throw new System.NotImplementedException();
        }

        targetText.text = currentSetup.message;

        PlayInfo();
    }

    private void PlayInfo()
    {
        targetPanel.localScale = Vector3.zero;
        target.SetActive(true);

        DOTween.Sequence()
            .SetId("game_info")
            .Append(targetPanel.DOScale(new Vector3(1, 1, 1), 0.5f))
            .AppendInterval(1f)
            .Append(targetPanel.DOScale(Vector3.zero, 0.5f))
            .OnKill(() => target.SetActive(false));
        ;
    }
}
