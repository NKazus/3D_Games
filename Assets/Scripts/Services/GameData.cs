using UnityEngine;

namespace FitTheSize.GameServices
{
    public enum GameResources
    {
        HighScore,
        ForceScale,
        RouteSpeed,
        ScaleSpeed,
        BoostMult
    }

    public class GameData : MonoBehaviour
    {
        [SerializeField] private GameDataViewer score;

        [SerializeField] private float defaultRouteSpeed = -0.1f;
        [SerializeField] private float defaultScaleSpeed = 0.05f;
        [SerializeField] private float defaultBoostMultiplyer = 1.5f;
        [SerializeField] private int defaultForceScaleUses = 3;

        private int highScore;
        private float routeSpeed;
        private float scaleSpeed;
        private float boostMultiplyer;
        private int extraForceScaleUses;

        private void OnEnable()
        {
            highScore = 5;// (PlayerPrefs.HasKey("DATA_HighScore")) ? PlayerPrefs.GetInt("DATA_HighScore") : 0;
            extraForceScaleUses = 2;// (PlayerPrefs.HasKey("DATA_ForceScale")) ? PlayerPrefs.GetInt("DATA_ForceScale") : 0;

            routeSpeed = (PlayerPrefs.HasKey("DATA_RouteSpeed")) ? PlayerPrefs.GetFloat("DATA_RouteSpeed") : defaultRouteSpeed;
            scaleSpeed = (PlayerPrefs.HasKey("DATA_ScaleSpeed")) ? PlayerPrefs.GetFloat("DATA_ScaleSpeed") : defaultScaleSpeed;
            boostMultiplyer = (PlayerPrefs.HasKey("DATA_BoostMult")) ? PlayerPrefs.GetFloat("DATA_BoostMult") : defaultBoostMultiplyer;
        }

        private void OnDisable()
        {
            PlayerPrefs.SetInt("DATA_HighScore", highScore);
            PlayerPrefs.SetInt("DATA_ForceScale", extraForceScaleUses);

            PlayerPrefs.SetFloat("DATA_RouteSpeed", routeSpeed);
            PlayerPrefs.SetFloat("DATA_ScaleSpeed", scaleSpeed);
            PlayerPrefs.SetFloat("DATA_BoostMult", boostMultiplyer);
        }

        public bool UpdateHighScore(int value, bool forceRefresh = false)
        {
            //Debug.Log("high score update:"+ value);
            if(!forceRefresh && value <= highScore)
            {
                return false;
            }

            highScore = value;
            score.UpdateHighScore(highScore);
            return true;
        }

        public void RefreshHighScore()
        {
            //Debug.Log("hs refresh:"+highScore);
            score.UpdateHighScore(highScore);
        }

        public void ResetRouteSpeed()
        {
            routeSpeed = defaultRouteSpeed;
            //Debug.Log("reset speed");
        }

        public void ResetTempResources()
        {
            boostMultiplyer = defaultBoostMultiplyer;
            scaleSpeed = defaultScaleSpeed;
            extraForceScaleUses = 0;
            //Debug.Log("reset temp");
        }

        public void UpdateRes(GameResources id, float value)
        {
            //Debug.Log("update");
            switch (id)
            {
                case GameResources.BoostMult: boostMultiplyer += value; break;//setup
                case GameResources.ForceScale:
                    if (extraForceScaleUses <= 0 && value < 0)
                    {
                        break;                        
                    }
                    extraForceScaleUses += (int)value;
                    //Debug.Log("fc value:"+extraForceScaleUses);
                    break;//setup && runner
                case GameResources.RouteSpeed: routeSpeed += value; Debug.Log("route speed:"+routeSpeed); break;//runner
                case GameResources.ScaleSpeed: scaleSpeed *= value; break;//setup
                default: throw new System.NotSupportedException();
            }
        }

        public bool GetResourceStatus(GameResources id)
        {
            switch (id)
            {
                case GameResources.BoostMult: return boostMultiplyer == defaultBoostMultiplyer;
                case GameResources.RouteSpeed: return routeSpeed != defaultRouteSpeed;
                case GameResources.ScaleSpeed: return scaleSpeed == defaultScaleSpeed;
                case GameResources.ForceScale: return true;
                default: throw new System.NotSupportedException();
            }
        }

        public float GetResourceValue(GameResources id)
        {
            switch (id)
            {
                case GameResources.BoostMult: return boostMultiplyer;
                case GameResources.RouteSpeed: return routeSpeed;
                case GameResources.ScaleSpeed: return scaleSpeed;
                case GameResources.ForceScale: return defaultForceScaleUses + extraForceScaleUses;
                case GameResources.HighScore: return highScore;
                default: throw new System.NotSupportedException();
            }
        }
    }
}
