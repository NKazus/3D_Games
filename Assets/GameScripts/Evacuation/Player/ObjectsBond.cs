using UnityEngine;
using Zenject;
using MEGame.Interactions;

namespace MEGame.Player
{
    public class ObjectsBond : MonoBehaviour
    {
        [SerializeField] private BondColors bondColors;
        [SerializeField] private LineRenderer bondRenderer;
        [SerializeField] private Transform obj1;
        [SerializeField] private Transform obj2;

        [SerializeField] private float maxLength;
        [SerializeField] private float initialLength;

        private float currentTargetLength;

        [Inject] private readonly GameUpdateHandler updateManager;
        [Inject] private readonly GameGlobalEvents globalEvents;

        private void Awake()
        {
            bondRenderer.positionCount = 5;
            currentTargetLength = maxLength;
        }

        private void OnEnable()
        {
            LocalUpdate();
            globalEvents.EvacuationEvent += SwitchBond;
        }

        private void Start()
        {
            bondRenderer.colorGradient = bondColors.GetColorGradient();
        }

        private void OnDisable()
        {
            globalEvents.EvacuationEvent -= SwitchBond;
        }

        private void SwitchBond(bool activate)
        {
            if (activate)
            {
                updateManager.UpdateEvent += LocalUpdate;
            }
            else
            {
                updateManager.UpdateEvent -= LocalUpdate;
            }
        }

        private void LocalUpdate()
        {
            Vector3 startPos = obj1.position;
            Vector3 endPos = obj2.position;

            Vector3 direction = endPos - startPos;

            bondRenderer.SetPosition(0, startPos);
            bondRenderer.SetPosition(1, startPos + 0.2f * direction);
            bondRenderer.SetPosition(2, startPos + 0.5f * direction);
            bondRenderer.SetPosition(3, startPos + 0.8f * direction);
            bondRenderer.SetPosition(4, endPos);

            float currentMagnitude = direction.magnitude;
            bondColors.SetMidAlteredGradient(bondRenderer, (currentMagnitude - initialLength) / (currentTargetLength - initialLength));

            if (currentMagnitude > currentTargetLength)
            {
                //Debug.Log("oops");
                globalEvents.FinishGame(FinishCondition.Bond);
                globalEvents.SwitchGame(false);
            }
        }

        public void UpdateBondLength(float value)
        {
            currentTargetLength = maxLength + value;
            //Debug.Log("length:" + currentTargetLength);
        }
    }
}
