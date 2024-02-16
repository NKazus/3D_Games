using DG.Tweening;
using UnityEngine;

public class UnitAnimation : MonoBehaviour
{
    [SerializeField] private ParticleSystem effectParticles;
    private Transform scaleTransform;

    private Vector3 defaultScale = new Vector3(1f, 1f, 1f);
    private Vector3 damageScale = new Vector3(1f, 2f, 1f);
    private Vector3 hpScale = new Vector3(1.5f, 1f, 1.5f);

    private void OnDisable()
    {
        effectParticles.Stop();
    }

    public void Init()
    {
        scaleTransform = transform;
    }

    public void ResetScale(bool animate)
    {
        if (animate)
        {
            scaleTransform.DOScale(defaultScale, 0.4f)
            .SetId("game_unit");
        }
        else
        {
            scaleTransform.localScale = defaultScale;
        }
    }

    public void ShakeScale()
    {
        DOTween.Sequence()
            .SetId("game_unit")
            .Append(scaleTransform.DOScaleY(0.5f * scaleTransform.localScale.y, 0.5f))
            .Append(scaleTransform.DOScaleY(1f * scaleTransform.localScale.y, 0.5f));
    }

    public void PlayAction()
    {
        effectParticles.Stop();
        effectParticles.Play();

        ShakeScale();
    }

    public void ScaleDamage()
    {
        scaleTransform.DOScale(damageScale, 0.4f)
            .SetId("game_unit");
    }

    public void ScaleHp()
    {
        scaleTransform.DOScale(hpScale, 0.4f)
            .SetId("game_unit");
    }

    public void DescaleUnit(System.Action callback)
    {
        scaleTransform.DOScale(Vector3.zero, 0.5f)
                .SetId("game_unit")
                .OnComplete(() => callback());
    }
}
