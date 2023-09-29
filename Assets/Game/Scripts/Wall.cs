using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Zenject;

public class Wall : MonoBehaviour
{
    [SerializeField] private MaterialInstance[] wallMaterials;

    private bool isStatic;
    private bool isShown;

    //moving
    private Texture2D wallTM;
    private Color colorM1;
    private Color colorM2;
    //static
    private Texture2D wallTS;
    private Color colorS1;
    private Color colorS2;

    private Transform wallTransform;

    [Inject]
    public void InitializeWallParams(WallParams wallParams)
    {
        wallTM = wallParams.MovingWallTexture;
        wallTS = wallParams.StaticWallTexture;

        colorM1 = wallParams.MovingColorDefault;
        colorM2 = wallParams.MovingColorActive;

        colorS1 = wallParams.StaticColorDefault;
        colorS2 = wallParams.StaticColorActive;
    }

    public void InitWall()
    {
        wallTransform = transform;
        isShown = isStatic = false;
        for (int i = 0; i < wallMaterials.Length; i++)
        {
            wallMaterials[i].Init();
            wallMaterials[i].SetMaterial(colorM1, wallTM);
        }
    }

    public void Blink()
    {
        Color currentColor = isStatic ? colorS1 : colorM1;
        Color targetColor = isStatic ? colorS2 : colorM2;

        for (int i = 0; i < wallMaterials.Length; i++)
        {
            wallMaterials[i].ShiftColor(targetColor, currentColor);
        }
    }

    public void Show()
    {
        if (isShown)
        {
            return;
        }
        isShown = true;
        wallTransform.DOScaleY(1f, 0.2f);
    }

    public void ResetWall()//full reset
    {
        if (isStatic)
        {
            for (int i = 0; i < wallMaterials.Length; i++)
            {
                wallMaterials[i].SetMaterial(colorM1, wallTM);
            }
        }
        
        wallTransform.DOScaleY(0.1f, 0.2f);
        isShown = isStatic = false;
    }

    public void SetStatic()
    {
        if (isStatic)
        {
            return;
        }
        for (int i = 0; i < wallMaterials.Length; i++)
        {
            wallMaterials[i].SetMaterial(colorS1, wallTS);
        }
        isStatic = true;
    }
}
