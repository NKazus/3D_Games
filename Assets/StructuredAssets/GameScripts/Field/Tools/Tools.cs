using UnityEngine;
using UnityEngine.UI;

public class Tools : MonoBehaviour
{
    [SerializeField] private ToolsSubmenu[] submenus;

    [SerializeField] private Button houseButton;
    [SerializeField] private Button shopButton;
    [SerializeField] private Button parkButton;
    [SerializeField] private Button recreationButton;
    [SerializeField] private Button destroyButton;

    private ToolsSubmenu currentActive;

    private System.Action<UnitType> ToolCallback;

    private void OnEnable()
    {
        houseButton.onClick.AddListener(() => { ToolCallback(UnitType.House); });
        shopButton.onClick.AddListener(() => { ToolCallback(UnitType.Shop); });
        parkButton.onClick.AddListener(() => { ToolCallback(UnitType.Park); });
        recreationButton.onClick.AddListener(() => { ToolCallback(UnitType.Recreation); });
        destroyButton.onClick.AddListener(() => { ToolCallback(UnitType.None); });
    }

    private void OnDisable()
    {
        houseButton.onClick.RemoveAllListeners();
        shopButton.onClick.RemoveAllListeners();
        parkButton.onClick.RemoveAllListeners();
        recreationButton.onClick.RemoveAllListeners();
        destroyButton.onClick.RemoveAllListeners();
    }

    private void ShowTools(SubmenuType type)
    {
        //Debug.Log("tool show");
        for (int i = 0; i < submenus.Length; i++)
        {
            if (submenus[i].GetSubmenuType() == type)
            {
                submenus[i].Show();
                currentActive = submenus[i];
            }
        }
    }

    public void SetCallback(System.Action<UnitType> callback)
    {
        ToolCallback = callback;
    }

    public void ResetTools()
    {
        currentActive = null;

        for(int i = 0; i < submenus.Length; i++)
        {
            submenus[i].Reset();
        }
    }   

    public void RefreshTools(bool showNew, SubmenuType type = SubmenuType.Create)
    {
        //Debug.Log(type);
        
        if (currentActive != null)
        {
            if (!showNew)
            {
                //Debug.Log("tools hide");
                currentActive.Hide();
                currentActive = null;
                return;
            }
            if (currentActive.GetSubmenuType() == type)
            {
                //Debug.Log("remains");
                return;
            }
            //Debug.Log("refresh tools anew");
            currentActive.Hide(ShowTools, type);
        }
        else
        {
            if (showNew)
            {
                ShowTools(type);
            }            
        }
    }
}
