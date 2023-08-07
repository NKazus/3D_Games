using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class RandomBonusItem : BonusItem
{
    [SerializeField] private Material[] reagents;
    [SerializeField] private Material defaultMaterial;

    private MeshRenderer meshRenderer;
    private System.Random rand = new System.Random();

    protected override void Awake()
    {
        base.Awake();
        meshRenderer = GetComponent<MeshRenderer>();
    }

    protected override void OnEnable()
    {
        meshRenderer.material = defaultMaterial;
        base.OnEnable();
    }

    protected override void PickItem(PointerEventData data)
    {
        if (dataHandler.Potions >= itemPrice)
        {
            int reagent = rand.Next(0, reagents.Length);
            dataHandler.TradePotions(itemPrice, reagent);
            DOTween.Sequence()
                .SetId(this)
                .Append(localTransform.DOScaleZ(0, 0.1f).OnComplete(() => { meshRenderer.material = reagents[reagent]; }))
                .Append(localTransform.DOScaleZ(scale, 0.1f));
            eventManager.PlayCoins();
        }
        else
        {
            eventManager.PlayVibro();
        }
    }
}
