using System;
using UnityEngine;

public class Ships : MonoBehaviour
{
    [SerializeField] private SpaceShip[] ships;

    private SpaceShip currentShip;

    private Action callback;
    private System.Random rand = new System.Random();

    private void HideCallback()
    {
        currentShip = ships[rand.Next(0, ships.Length)];
        currentShip.ShowShip(ShowCallback);
    }

    private void ShowCallback()
    {
        callback();
    }

    public void ActivateShip(Action shipCallback)
    {
        callback = shipCallback;

        if(currentShip != null)
        {
            currentShip.HideShip(HideCallback);
        }
        else
        {
            HideCallback();
        }
    }

    public void ResetShip()
    {
        if(currentShip != null)
        {
            currentShip.ResetShip();
        }
    }
}
