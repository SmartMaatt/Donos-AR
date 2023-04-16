using UnityEngine;

public class ARTabToPlaceTheBoss : ARTabToPlaceObject
{
    /*------->>> Parameters <<<-------*/
    #region Parameters
    private TheBossController _bossController;
    #endregion


    /*------->>> Unity methods <<<-------*/
    #region Unity methods
    protected void Awake()
    {
        base.Awake();
    }

    protected void Update()
    {
        base.Update();
    }
    #endregion


    /*------->>> Placing methods <<<-------*/
    #region Placing methods
    protected override void PlaceObject()
    {
        Pose hitPose = hits[0].pose;
        if (_spawnedObject == null)
        {
            _spawnedObject = Instantiate(gameObjectToInstantiate, Vector3.zero, Quaternion.identity);
            _bossController = _spawnedObject.GetComponent<TheBossController>();
            
            // Invalid prefab
            if (_bossController == null)
            {
                Debug.Log("Given prefab doesn't have TheBossController component!");
                enabled = false;
            }

            _bossController.LookAtObject = Camera.main.transform;
            _bossController.SetDestinationPoint(hitPose.position);
        }
        else
        {
            _bossController.SetDestinationPoint(hitPose.position);
        }
    }
    #endregion
}