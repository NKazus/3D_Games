using UnityEngine;
using FitTheSize.GameServices;

namespace FitTheSize.Main
{
    public enum SwipeDirection
    {
        Left,
        Right
    }

    public class PlayerSwipe : MonoBehaviour
    {
        private Vector3 firstTouch;
        private Vector3 lastTouch;
        private float dragDistance; //minimum distance for a swipe to be registered

        private bool isActive;

        private System.Action<SwipeDirection> SwipeCallback;
        private GameUpdateHandler updateHandler;

        private void Start()
        {
            dragDistance = Screen.width * 0.2f;
        }

        private void OnDisable()
        {
            ActivateSwiping(false);
        }

        private void HandleSwipe()
        {
            if (Input.touchCount == 1) // user is touching the screen with a single touch
            {
                Touch touch = Input.GetTouch(0); // get the touch
                if (touch.phase == TouchPhase.Began) //check for the first touch
                {
                    //Debug.Log("began");
                    isActive = true;
                    firstTouch = touch.position;
                    lastTouch = touch.position;
                }
                else if (touch.phase == TouchPhase.Ended && isActive)
                {
                    //Debug.Log("ended");
                    lastTouch = touch.position;

                    if (Mathf.Abs(lastTouch.x - firstTouch.x) > dragDistance && SwipeCallback != null)
                    {
                        if ((lastTouch.x > firstTouch.x))
                        {
                            SwipeCallback(SwipeDirection.Right);
                            //Debug.Log("Right Swipe");
                        }
                        else
                        {
                            SwipeCallback(SwipeDirection.Left);
                            //Debug.Log("Left Swipe");
                        }
                    }
                    isActive = false;
                }
            }
        }

        public void ActivateSwiping(bool activate)
        {
            if (activate)
            {
                updateHandler.GlobalUpdateEvent += HandleSwipe;
            }
            else
            {
                updateHandler.GlobalUpdateEvent -= HandleSwipe;
            }
        }

        public void SetupSwipe(System.Action<SwipeDirection> callback, GameUpdateHandler update)
        {
            SwipeCallback = callback;
            updateHandler = update;
        }

        public void ResetSwipe()
        {
            isActive = false;
        }
    }
}
