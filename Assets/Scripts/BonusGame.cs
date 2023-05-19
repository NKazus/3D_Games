using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BonusGame : MonoBehaviour
{
    [SerializeField] private GameObject dotPrefab;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private int dotNumber = 8;
    [SerializeField] private int winDotNumber = 2;
    [SerializeField] private DataHandler data;
    [Header("Score:")]
    [SerializeField] private Image bonusProgressBar;
    [SerializeField] private Text endMessage;

    private int allDotsCount;
    private int collectedDotsCount;

    private List<int> currentSpawnPoints = new List<int>();
    private System.Random rand = new System.Random();

    private void OnEnable()
    {
        ResetScoreUI();
        currentSpawnPoints.Clear();
        GlobalEventManager.BonusScoreEvent += ChangeScore;
        GlobalEventManager.SwitchBonusState(true);
        GeneratePoints();
        SpawnDots();
    }

    private void OnDisable()
    {
        endMessage.enabled = false;
        GlobalEventManager.SwitchBonusState(false);
        GlobalEventManager.BonusScoreEvent -= ChangeScore;
        GlobalEventManager.HidePlatforms();
    }

    private void ChangeScore(bool isCollected)
    {
        allDotsCount++;
        if (isCollected)
        {
            collectedDotsCount++;
            bonusProgressBar.fillAmount = ((float) collectedDotsCount % winDotNumber) / winDotNumber;
        }
        if(allDotsCount >= dotNumber)
        {
            GlobalEventManager.BonusScoreEvent -= ChangeScore;
            CheckBonus();
        }
    }

    private void CheckBonus()
    {
        data.ResetDots();
        int bonusDots = collectedDotsCount / winDotNumber;
        data.Dots -= bonusDots;
        if(bonusDots > 0)
        {            
            endMessage.text = "Now you have to visit " + data.Dots.ToString() + " planets to win!";
            endMessage.enabled = true;
        }
    }

    private void GeneratePoints()
    {
        int index;
        while (currentSpawnPoints.Count < dotNumber)
        {
            index = rand.Next(0, spawnPoints.Length);
            if (!currentSpawnPoints.Contains(index))
            {
                currentSpawnPoints.Add(index);
            }
        }
    }

    private void SpawnDots()
    {
        GameObject dot;
        for(int i = 0; i < currentSpawnPoints.Count; i++)
        {
            dot = PoolManager.GetGameObjectFromPool(dotPrefab);
            dot.GetComponent<Dot>().Setup(spawnPoints[currentSpawnPoints[i]].position);            
        }
    }

    private void ResetScoreUI()
    {
        allDotsCount = collectedDotsCount = 0;
        bonusProgressBar.fillAmount = 0f;
    }
}
