using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class Trading : MonoBehaviour
{
    [SerializeField] private Button tradeButton;

    [Inject] private readonly GameResourceHandler resources;

    private void OnEnable()
    {
        CheckTreasure();
    }

    private void OnDisable()
    {
        tradeButton.onClick.RemoveListener(Trade);
    }

    private void Trade()
    {
        tradeButton.onClick.RemoveListener(Trade);
        resources.UpdateTreasure(-1);
        resources.UpdateSticks(5);
        CheckTreasure();
    }

    private bool CheckTreasure()
    {
        if(resources.TreasureScore <= 0)
        {
            tradeButton.image.color = Color.gray;
            return false;
        }

        tradeButton.onClick.AddListener(Trade);
        tradeButton.image.color = Color.white;
        return true;
    }
}
