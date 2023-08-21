using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private Text scoreUI;
    [SerializeField] private Image progressBar;


    private int scoreCount;
    private float progress;

    private List<string> visitedDots = new List<string>();
}
