using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class LightwayManager : MonoBehaviour
{
    [SerializeField] private Route[] routes;
    [SerializeField] private Gem[] gems;

    [SerializeField] private Button startButton;

    [SerializeField] private Button pick1Button;
    [SerializeField] private Button pick2Button;
    [SerializeField] private Button pick3Button;

    private int winRoute;
    private int gemNumber;

    private List<Gem> route1Gems;
    private List<Gem> route2Gems;
    private List<Gem> route3Gems;

    [Inject] private readonly GlobalEventManager events;
    [Inject] private readonly DataHandler dataHandler;
    [Inject] private readonly RandomGenerator random;

    private void Awake()
    {
        gemNumber = gems.Length;

        route1Gems = new List<Gem>();
        route2Gems = new List<Gem>();
        route3Gems = new List<Gem>();
    }

    private void OnEnable()
    {
        events.GameStateEvent += ChangeState;
        dataHandler.UpdateGlobalScore(0);

        for (int i = 0; i < gemNumber; i++)
        {
            gems[i].InitGem();
            gems[i].ShowGem(false);
        }

        startButton.onClick.AddListener(Play);

        pick1Button.onClick.AddListener(() => PickRoute(0));
        pick2Button.onClick.AddListener(() => PickRoute(1));
        pick3Button.onClick.AddListener(() => PickRoute(2));
    }

    private void OnDisable()
    {
        pick1Button.onClick.RemoveAllListeners();
        pick2Button.onClick.RemoveAllListeners();
        pick3Button.onClick.RemoveAllListeners();
        pick1Button.gameObject.SetActive(false);
        pick2Button.gameObject.SetActive(false);
        pick3Button.gameObject.SetActive(false);

        startButton.onClick.RemoveListener(Play);

        events.GameStateEvent -= ChangeState;
    }

    private void ChangeState(bool activate)
    {
        if (activate)
        {
            startButton.gameObject.SetActive(true);
            for (int i = 0; i < routes.Length; i++)
            {
                routes[i].ResetRoute();
            }
            for (int i = 0; i < gemNumber; i++)
            {
                gems[i].ShowGem(false);
            }
            events.HighlightEvent += CheckHighlights;
        }
        else
        {
            events.HighlightEvent -= CheckHighlights;
            for (int i = 0; i < routes.Length; i++)
            {
                routes[i].ActivateRoute(false);
            }
        }
    }

    private void Play()
    {
        startButton.gameObject.SetActive(false);

        GenerateGems();
        //staff


        pick1Button.gameObject.SetActive(true);
        pick2Button.gameObject.SetActive(true);
        pick3Button.gameObject.SetActive(true);
        if (dataHandler.Highlights > 0)
        {
            for (int i = 0; i < routes.Length; i++)
            {
                routes[i].ActivateRoute(true);
            }
        }
    }

    private void PickRoute(int routeId)
    {
        pick1Button.gameObject.SetActive(false);
        pick2Button.gameObject.SetActive(false);
        pick3Button.gameObject.SetActive(false);

        for (int i = 0; i < gemNumber; i++)
        {
            gems[i].ShowGem(true);
        }

        if(routeId == winRoute)
        {
            events.DoWin(10);
        }
        events.SwitchGameState(false);
    }

    private void GenerateGems()
    {
        int arrayLength = routes.Length;
        int[] gemNumbers = new int[arrayLength];

        do
        {
            gemNumbers[0] = random.GenerateInt(1, gemNumber - 2);
        }
        while (gemNumbers[0] == 3 || gemNumbers[0] % 2 ==0);
        gemNumbers[1] = (gemNumber - gemNumbers[0]) / 2;
        gemNumbers[2] = gemNumber - gemNumbers[0] - gemNumbers[1];

        random.RandomizeArray(gemNumbers);

        winRoute = 0;
        int maxValue = gemNumbers[0];

        for(int i = 0; i < gemNumbers.Length; i++)
        {
            if(maxValue < gemNumbers[i])
            {
                maxValue = gemNumbers[i];
                winRoute = i;
            }
        }

        route1Gems.Clear();
        route2Gems.Clear();
        route3Gems.Clear();

        int counter = 0;
        while (counter < gemNumbers[0])
        {
            route1Gems.Add(gems[counter]);
            counter++;
        }
        while (counter < gemNumbers[0] + gemNumbers[1])
        {
            route2Gems.Add(gems[counter]);
            counter++;
        }
        while (counter < gemNumber)
        {
            route3Gems.Add(gems[counter]);
            counter++;
        }

        routes[0].SetGems(route1Gems);
        routes[1].SetGems(route2Gems);
        routes[2].SetGems(route3Gems);
    }

    private void CheckHighlights()
    {
        dataHandler.UpdateHighlights(-1);
        if(dataHandler.Highlights <= 0)
        {
            for (int i = 0; i < routes.Length; i++)
            {
                routes[i].ActivateRoute(false);
            }
        }
    }
}
