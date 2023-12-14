using System.Collections.Generic;
using UnityEngine;
using Zenject;
using FitTheSize.GameServices;

namespace FitTheSize.Route
{
    public class PlatformHandler : MonoBehaviour
    {
        [SerializeField] private GameObject[] spawnedPlatforms;

        [SerializeField] private Transform poolParent;
        [SerializeField] private Transform routeParent;

        [SerializeField] private Transform spawnPosition;
        [SerializeField] private Transform despawnPosition;
        [SerializeField] private Transform[] initialRoute;

        private bool initSpawn;// do not respawn if initial route hasn't been moved

        private float routeSpeed;

        private int platformSpawnIndex;

        private List<RunnerPlatform> activePlatforms = new List<RunnerPlatform>();
        private bool isMoving;

        private Transform currentSpawnPosition;

        private System.Action DespawnCallback;

        [Inject] private readonly GamePool gamePool;
        [Inject] private readonly GameUpdateHandler updateHandler;

        private void OnEnable()
        {
            isMoving = false;
        }

        private void OnDisable()
        {
            if (isMoving)
            {
                StopRoute();
            }

            while (activePlatforms.Count > 0)
            {
                Despawn(activePlatforms[0].gameObject);
            }

            activePlatforms.Clear();
        }

        private void ChangeSpawnIndex()
        {
            platformSpawnIndex++;

            if (platformSpawnIndex >= spawnedPlatforms.Length)
            {
                platformSpawnIndex = 0;
            }
        }

        private void SpawnNext()//when spawn trigger
        {
            //Debug.Log("spawn");
            bool spawnStatusNew;
            //Debug.Log("SPAWN_INDEX:" + platformSpawnIndex);
            GameObject target = gamePool.GetGameObjectFromPool(spawnedPlatforms[platformSpawnIndex], out spawnStatusNew);
            target.transform.parent = routeParent;

            RunnerPlatform targetComponent = target.GetComponent<RunnerPlatform>();
            targetComponent.SetPosition(currentSpawnPosition.position);
            targetComponent.SetSpeed(routeSpeed);
            activePlatforms.Add(targetComponent);
            //Debug.Log("active platforms:"+activePlatforms.Count);
            targetComponent.SetupPlatform();
            targetComponent.SwitchColliders(true);

            if (spawnStatusNew)
            {
                targetComponent.SetupMovement(updateHandler, despawnPosition.position.z, TriggerSpawn);
            }

            if (isMoving)
            {
                targetComponent.SwitchMovement(true);
            }

            ChangeSpawnIndex();
        }

        private void Despawn(GameObject target)
        {
            //Debug.Log("despawn");
            RunnerPlatform targetComponent = target.GetComponent<RunnerPlatform>();
            targetComponent.SwitchColliders(false);

            activePlatforms.Remove(targetComponent);
            //Debug.Log("active platforms:" + activePlatforms.Count);
            target.transform.parent = poolParent;
            gamePool.PutGameObjectToPool(target);
        }

        private void TriggerSpawn(GameObject target)
        {
            Despawn(target);
            SpawnNext();

            if (DespawnCallback != null)
            {
                DespawnCallback();
            }
        }

        private void SpawnFirst()
        {
            platformSpawnIndex = 0;

            for (int i = 0; i < initialRoute.Length; i++)
            {
                currentSpawnPosition = initialRoute[i];
                SpawnNext();
            }

            currentSpawnPosition = spawnPosition;
        }

        public void SetRouteSpeed(float targetSpeed)
        {
            routeSpeed = targetSpeed;
        }

        public void SetDespawnCallback(System.Action callback)
        {
            DespawnCallback = callback;
        }

        public void ResetInitialSpawnFlag()
        {
            initSpawn = false;
        }

        public void MoveRoute()
        {
            //Debug.Log("MOVE");
            initSpawn = false;
            isMoving = true;

            for (int i = 0; i < activePlatforms.Count; i++)
            {
                activePlatforms[i].SwitchMovement(true);
            }
        }

        public void StopRoute()
        {
            //Debug.Log("STOP");
            isMoving = false;

            for (int i = 0; i < activePlatforms.Count; i++)
            {
                activePlatforms[i].SwitchMovement(false);
            }
        }

        public void ResetRoute()
        {
            if (initSpawn)
            {
                return;
            }

            if (isMoving)
            {
                StopRoute();
            }

            while (activePlatforms.Count > 0)
            {
                Despawn(activePlatforms[0].gameObject);
            }

            SpawnFirst();
            initSpawn = true;

            //Debug.Log("reset:"+activePlatforms.Count);
        }
    }
}
