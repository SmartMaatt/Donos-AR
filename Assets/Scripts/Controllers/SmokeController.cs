using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeController : MonoBehaviour
{
    [SerializeField] Vector3 constantRotation;

    void Update()
    {
        transform.rotation = Quaternion.Euler(constantRotation);
    }
}
