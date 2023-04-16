using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheBossController : MonoBehaviour
{
    [SerializeField] float waitTime;
    [SerializeField] float walkTime;
    [SerializeField] float walkDistance;
    [SerializeField] Vector3 testEndPoint;
    [SerializeField] Transform lookAtObject;

    Animator animator;

    void Start()
    {
        SpawnTheBoss(testEndPoint);
    }

    void SpawnTheBoss(Vector3 endPoint)
    {
        animator = GetComponent<Animator>();

        transform.position = endPoint;

        Vector3 lookPos = lookAtObject.position - transform.position;
        lookPos.y = 0;
        Quaternion rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = rotation;

        transform.position += rotation * Vector3.back * walkDistance;

        StartCoroutine(LerpPosition(endPoint, walkTime));
    }

    IEnumerator LerpPosition(Vector3 targetPosition, float duration)
    {
        animator.SetBool("Stop", true);

        yield return new WaitForSeconds(waitTime);

        animator.SetBool("Stop", false);

        float time = 0;
        Vector3 startPosition = transform.position;

        while (time < duration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        animator.SetBool("Stop", true);
    }
}
