using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Zenject;

public class Garden : MonoBehaviour
{
    [SerializeField] private Transform directionalLight;
    [SerializeField] private Material groundMaterial;
    [SerializeField] private Material plantMaterial;

    [SerializeField] private Transform soil;
    [SerializeField] private Vector3 normalSoil;
    [SerializeField] private Vector3 wetSoil;
    [SerializeField] private Vector3 drySoil;
    [SerializeField] private Material wetMaterial;
    [SerializeField] private Material dryMaterial;

    [SerializeField] private Transform stick;
    [SerializeField] private ParticleSystem rain;

    private Light lightComponent;

    private MeshRenderer soilRenderer;
    private TextureShift tShift;

    private float stickScaleY;

    private void Awake()
    {
        lightComponent = directionalLight.GetComponent<Light>();
        soilRenderer = soil.GetComponent<MeshRenderer>();
        tShift = soil.GetComponent<TextureShift>();
        tShift.enabled = false;
        stickScaleY = stick.localScale.y;
    }

    public void UpdateGroundMaterial(float smoothness)
    {
        groundMaterial.DOFloat(smoothness, "_Glossiness", 0.5f);
        groundMaterial.DOFloat(smoothness, "_Metallic", 0.5f);
        plantMaterial.DOFloat(smoothness, "_Glossiness", 0.5f);
        plantMaterial.DOFloat(smoothness, "_Metallic", 0.5f);
    }

    public void UpdateLighting(float rotation, Color color)
    {
        Quaternion targetQuat = Quaternion.Euler(rotation, 0, 0);
        directionalLight.DOLocalRotateQuaternion(targetQuat, 0.5f);
        lightComponent.DOColor(color, 0.5f);
    }

    public void UpdateRootPlane(GardenState state)
    {
        Vector3 targetPosition = Vector3.zero;
        Material targetMaterial = dryMaterial;

        switch (state)
        {
            case GardenState.Rain: targetMaterial = wetMaterial; targetPosition = wetSoil; tShift.enabled = true; break;
            case GardenState.Heat: targetMaterial = dryMaterial; targetPosition = drySoil; tShift.enabled = false; break;
            case GardenState.Normal: targetPosition = normalSoil; tShift.enabled = false; break;
            default: throw new NotSupportedException();
        }

        soil.DOLocalMove(normalSoil, 0.5f).OnComplete(() => {
            soilRenderer.material = targetMaterial;
            soil.DOLocalMove(targetPosition, 0.7f); });
    }

    public void UpdatePropState(bool isActive)
    {
        stick.DOScale(isActive ? stickScaleY : 0f, 0.6f);
    }

    public void UpdateRain(bool isActive)
    {
        if (isActive)
        {
            rain.Play();
        }
        else
        {
            rain.Stop();
        }
    }
}
