using UnityEngine;
using Zenject;

namespace FitTheSize.Route
{
    public class RunnerPlatformWall : RunnerPlatform
    {
        [SerializeField] private Transform[] walls;
        [SerializeField] private Transform leftBorder;
        [SerializeField] private Transform rightBorder;

        private float platformRange;
        private float inBetweenDistance;

        [Inject] private readonly Randomizer randomizer;

        protected override void Awake()
        {
            platformRange = rightBorder.localPosition.x - leftBorder.localPosition.x;
            inBetweenDistance = platformRange / (walls.Length + 1);
            base.Awake();
        }

        public override void SetupPlatform()
        {
            //Debug.Log("wall");

            for (int i = 0; i < walls.Length; i++)
            {
                float delta = randomizer.GenerateFloat((-inBetweenDistance) / 2f, inBetweenDistance / 2f);
                                //Debug.Log("delta:" + delta);
                float newX = leftBorder.localPosition.x + (inBetweenDistance * (i + 1))
                    + delta;
                //Debug.Log("newX:"+newX);
                walls[i].localPosition = new Vector3(newX,
                    walls[i].localPosition.y, walls[i].localPosition.z);
            }
        }
    }
}
