using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Zenject;

public class Pixie : MonoBehaviour
{
    private Material targetMaterial;

    private Color[] colors;

    private int currentColor;

    [Inject]
    public void InitColors(PixieColors colorAsset)
    {
        colors = colorAsset.AllColors;
    }

    private void Awake()
    {
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        targetMaterial = Instantiate(meshRenderer.material);
        meshRenderer.material = targetMaterial;
    }

    private void OnEnable()
    {
        targetMaterial.color = colors[0];
        currentColor = 0;
        Switch();
    }

    private void Switch()
    {
        currentColor++;
        if(currentColor >= colors.Length)
        {
            currentColor = 0;
        }
        targetMaterial.DOColor(colors[currentColor], 1f)
            .SetId("pixie_color")
            .OnComplete(() =>
            {          
                Switch();
            });
    }
}
