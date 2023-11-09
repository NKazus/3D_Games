using DG.Tweening;
using UnityEngine;

namespace MEGame.Player
{
    public class PlayerMesh : MonoBehaviour
    {
        private Transform meshTransform;
        private Vector3 meshInitialScale;

        private void Awake()
        {
            meshTransform = transform;
            meshInitialScale = meshTransform.localScale;
        }

        private void OnEnable()
        {
            meshTransform.localScale = meshInitialScale;
        }

        public void Hide(bool restore)
        {
            meshTransform.DOScale(0f, 0.4f)
                .SetId("player")
                .OnComplete(() =>
                {
                    if (restore)
                    {
                        Show();
                    }
                });
        }

        public void Show()
        {
            meshTransform.DOScale(meshInitialScale.x, 0.4f)
                .SetId("player");
        }
    }
}
