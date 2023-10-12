using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Scanner : MonoBehaviour
{
    [SerializeField] private int scansNumber;
    [SerializeField] private Text scanUses;

    [SerializeField] private Text scanValue;
    [SerializeField] private Text scanStatus;
    [SerializeField] private string statusActive;
    [SerializeField] private string statusInactive;
    [SerializeField] private Color colorActive;
    [SerializeField] private Color colorInactive;

    [SerializeField] private int hitDefaultValue;
    [SerializeField] private int missDefaultValue;
    [SerializeField] private int activeRange;
    [SerializeField] private int inactiveRange;

    private bool isActive;
    private int currentUse;

    public bool Scan(Randomizer random, bool rightSection)
    {
        int initialValue = rightSection ? hitDefaultValue : missDefaultValue;
        int range = isActive ? activeRange : inactiveRange;
        int value = Mathf.Clamp(random.GetInt(initialValue - range, initialValue + range), 0, 100);
        scanValue.DOText(value.ToString(), 0.4f);
        currentUse++;
        scanUses.text = currentUse.ToString();
        return currentUse < scansNumber;
    }

    public void ResetScanner(bool active, bool hard = false)
    {
        
        isActive = active;
        scanStatus.text = isActive ? statusActive : statusInactive;
        scanStatus.color = isActive ? colorActive : colorInactive;

        if (hard)
        {
            scanValue.text = "0";
            currentUse = 0;
            scanUses.text = currentUse.ToString();
        }
    }
}
