using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class PlaneDetectionController : MonoBehaviour
{
	/*------->>> Parameters <<<-------*/
	#region Parameters
	[Header("References")]
	[SerializeField] private ARPlaneManager arPlaneManager;
	[SerializeField] private ARRaycastManager arRaycastManager;
	[SerializeField] private ARTabToPlaceObject arTabToPlaceObject;

	// State machine
	private bool planeDetectionState = false;
    #endregion


    /*------->>> Unity methods <<<-------*/
    #region Unity methods
    private void Start()
    {
		ActivatePlaneDetection(planeDetectionState);
    }
    #endregion


    /*------->>> Activation methods <<<-------*/
    #region Activation methods
    public void TogglePlaneDetection()
	{
		ActivatePlaneDetection(!planeDetectionState);
	}

	public void ActivatePlaneDetection(bool activate)
	{
        arPlaneManager.enabled = activate;
        arRaycastManager.enabled = activate;
		arTabToPlaceObject.enabled = activate;
		planeDetectionState = activate;
	}

	#endregion
}
