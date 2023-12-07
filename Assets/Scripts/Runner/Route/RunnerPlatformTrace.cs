using UnityEngine;
using Zenject;

namespace FitTheSize.Route
{
    public class RunnerPlatformTrace : RunnerPlatform
    {
        [SerializeField] private Transform[] traces;

        private Vector3[] tracePositions;

        [Inject] private readonly Randomizer randomizer;

        protected override void Awake()
        {
            tracePositions = new Vector3[traces.Length];
            for (int i = 0; i < traces.Length; i++)
            {
                tracePositions[i] = traces[i].localPosition;
            }
            base.Awake();
        }

        public override void SetupPlatform()
        {
            //Debug.Log("trace");

            randomizer.RandomizeArray(tracePositions);
            for (int i = 0; i < traces.Length; i++)
            {
                traces[i].localPosition = tracePositions[i];
            }
        }
    }
}
