using System.Collections;
using UnityEngine;

public class TheBossController : MonoBehaviour
{
    /*------->>> Parameters <<<-------*/
    #region Parameters
    [Header("References")]
    [SerializeField] private Transform lookAtObject;

    [Header("Settings")]
    [SerializeField] private float waitTime;
    [SerializeField] private float walkTime;
    [SerializeField] private float walkDistance;

    [Space]
    [SerializeField] private Vector3 testEndPoint;

    private Animator _animator;
    private IEnumerator _lerpProcedure;
    #endregion


    /*------->>> Getters/Setters <<<-------*/
    #region Getters/Setters
    public Transform LookAtObject { get => lookAtObject; set => lookAtObject = value; }
    #endregion


    /*------->>> Unity methods <<<-------*/
    #region Unity methods
    private void Start()
    {
        _animator = GetComponent<Animator>();
        _animator.SetBool("Stop", true);
    }
    #endregion


    /*------->>> Positioning methods <<<-------*/
    #region Positioning methods
    [ContextMenu("Use test end point")]
    public void UseTestEndPoint()
    {
        SetDestinationPoint(testEndPoint);
    }

    public void SetDestinationPoint(Vector3 endPoint)
    {
        transform.position = endPoint;

        Vector3 lookPos = lookAtObject.position - transform.position;
        lookPos.y = 0;
        Quaternion rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = rotation;

        transform.position += rotation * Vector3.back * walkDistance;

        // Lerp coroutine
        if (_lerpProcedure != null) { StopCoroutine(_lerpProcedure); }
        _lerpProcedure = LerpPosition(endPoint, walkTime);
        StartCoroutine(_lerpProcedure);
    }

    private IEnumerator LerpPosition(Vector3 targetPosition, float duration)
    {
        _animator.SetBool("Stop", true);

        yield return new WaitForSeconds(waitTime);

        _animator.SetBool("Stop", false);

        float time = 0;
        Vector3 startPosition = transform.position;

        while (time < duration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        _animator.SetBool("Stop", true);
    }
    #endregion
}
