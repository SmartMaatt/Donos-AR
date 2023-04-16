using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;


[RequireComponent(typeof(ARRaycastManager))]
public class ARTabToPlaceObject : MonoBehaviour
{
    /*------->>> Parameters <<<-------*/
    #region Parameters
    [Header("References")]
    [SerializeField] private GameObject gameObjectToInstantiate;

    private GameObject _spawnedObject;
    private ARRaycastManager _arRaycastManager;
    private Vector2 _touchPosition;

    static List<ARRaycastHit> hits = new List<ARRaycastHit>();
    #endregion


    /*------->>> Events <<<-------*/
    #region Events
    public static event Action OnObjectPlaced;
    #endregion


    /*------->>> Unity methods <<<-------*/
    #region Unity methods
    private void Awake()
    {
        _arRaycastManager = GetComponent<ARRaycastManager>();
    }

    private void Update()
    {
        if (!TryGetTouchPosition(out Vector2 touchPosition))
        {
            return;
        }

        if (PhysicRayCastBlockedByUI(touchPosition))
        {
            Pose hitPose = hits[0].pose;
            if (_spawnedObject == null)
            {
                _spawnedObject = Instantiate(gameObjectToInstantiate, hitPose.position, hitPose.rotation);
            }
            else
            {
                _spawnedObject.transform.position = hitPose.position;
            }
            OnObjectPlaced?.Invoke();
        }
    }
    #endregion


    /*------->>> Detection methods <<<-------*/
    #region Detection methods
    private bool TryGetTouchPosition(out Vector2 touchPosition)
    {
        if (Input.touchCount > 0)
        {
            touchPosition = Input.GetTouch(0).position;
            return true;
        }

        touchPosition = Vector2.zero;
        return false;
    }

    private bool PhysicRayCastBlockedByUI(Vector2 touchPosition)
    {
        if (PointerOverUI.IsPointerOverUIObject(touchPosition))
        {
            return false;
        }

        return _arRaycastManager.Raycast(touchPosition, hits, TrackableType.PlaneWithinPolygon);
    }
    #endregion
}