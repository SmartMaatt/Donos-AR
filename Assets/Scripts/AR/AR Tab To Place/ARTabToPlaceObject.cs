using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;


[RequireComponent(typeof(ARRaycastManager))]
public abstract class ARTabToPlaceObject : MonoBehaviour
{
    /*------->>> Parameters <<<-------*/
    #region Parameters
    [Header("References")]
    [SerializeField] protected GameObject gameObjectToInstantiate;

    protected GameObject _spawnedObject;
    protected ARRaycastManager _arRaycastManager;
    protected Vector2 _touchPosition;

    protected static List<ARRaycastHit> hits = new List<ARRaycastHit>();
    #endregion


    /*------->>> Events <<<-------*/
    #region Events
    public static event Action OnObjectPlaced;
    #endregion


    /*------->>> Unity methods <<<-------*/
    #region Unity methods
    protected void Awake()
    {
        _arRaycastManager = GetComponent<ARRaycastManager>();
    }

    protected void Update()
    {
        if (!TryGetTouchPosition(out Vector2 touchPosition))
        {
            return;
        }

        if (PhysicRayCastBlockedByUI(touchPosition))
        {
            PlaceObject();
            OnObjectPlaced?.Invoke();
        }
    }
    #endregion


    /*------->>> Placing methods <<<-------*/
    #region Placing methods
    protected abstract void PlaceObject();
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