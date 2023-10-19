using UnityEngine;
using Zenject;

public class BezierPath : MonoBehaviour
{
    [SerializeField] private Curve[] curves;
    [SerializeField] private bool looped;

    private int curveId;

    private float tParam;

    private Vector3 targetPosition;
    private float targetSpeed;
    private Transform targetTransform;
    private Vector3 targetDirection;

    private Vector3 p0;
    private Vector3 p1;
    private Vector3 p2;
    private Vector3 p3;

    private bool isFollowing;
    private System.Action PathCallback;

    [Inject] private readonly GlobalUpdate globalUpdate;

    private void PathUpdate()
    {      
        if (tParam < 1)
        {
            tParam += Time.deltaTime * targetSpeed;
            targetPosition = Mathf.Pow(1 - tParam, 3) * p0 + 3 * Mathf.Pow(1 - tParam, 2) * tParam * p1
                + 3 * (1 - tParam) * Mathf.Pow(tParam, 2) * p2 + Mathf.Pow(tParam, 3) * p3;

            targetDirection = targetPosition - targetTransform.position;
            targetTransform.forward = targetDirection;

            targetTransform.position = targetPosition;
        }
        else
        {
            globalUpdate.GlobalUpdateEvent -= PathUpdate;
            CompleteCurve();
        }        
    }

    private void InitCurve(int id)
    {
        curves[id].GetControlPoints(out p0, out p1, out p2, out p3);
    }

    private void CompleteCurve()
    {
        if (!isFollowing)
        {
            return;
        }

        tParam = 0;
        curveId += 1;

        if (curveId >= curves.Length)
        {
            if (looped)
            {
                curveId = 0;
                InitCurve(curveId);
                globalUpdate.GlobalUpdateEvent += PathUpdate;
            }
            else
            {
                isFollowing = false;
                if (PathCallback != null)
                {
                    PathCallback();
                }                
            }
        }
        else
        {
            InitCurve(curveId);
            globalUpdate.GlobalUpdateEvent += PathUpdate;
        }
    }

    public void FollowPath()
    {
        if (isFollowing)
        {
            return;
        }

        isFollowing = true;
        globalUpdate.GlobalUpdateEvent += PathUpdate;
        Debug.Log("follow" + targetTransform.gameObject.name);
    }

    public void UnfollowPath()
    {
        if (!isFollowing)
        {
            return;
        }

        isFollowing = false;
        globalUpdate.GlobalUpdateEvent -= PathUpdate;
        Debug.Log("unfollow" + targetTransform.gameObject.name);
    }

    public void ResetPath()
    {
        curveId = 0;
        InitCurve(curveId);
        tParam = 0f;

        targetTransform.position = curves[0].GetControlPoint(0);
        targetTransform.rotation = Quaternion.Euler(Vector3.forward);
    }

    public void SetTarget(Transform targetT)
    {
        targetTransform = targetT;        
    }

    public void SetSpeed(float targetS)
    {
        targetSpeed = targetS;
    }

    public void SetCallback(System.Action callback)
    {
        PathCallback = callback;
    }
}
