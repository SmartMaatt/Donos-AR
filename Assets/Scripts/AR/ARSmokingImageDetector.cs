using System;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARTrackedImageManager))]
public class ARSmokingImageDetector : MonoBehaviour
{
    /*------->>> Parameters <<<-------*/
    #region Parameters
    [Header("References")]
    [SerializeField] private GameObject smokingPrefab;

    [Header("Settings")]
    [SerializeField] private string imageName = "Smoking Sign";

    private ARTrackedImageManager _trackedImagesManager;
    private GameObject _smokingObject;
    private bool _isDetected = false;
    #endregion


    /*------->>> Getters/Setters <<<-------*/
    #region Getters/Setters
    public bool IsDetected { get => _isDetected; }
    #endregion


    /*------->>> Events <<<-------*/
    #region Events
    public static event Action OnSmokingSignVisible;
    public static event Action OnSmokingSignInvisible;
    #endregion


    /*------->>> Unity methods <<<-------*/
    #region Unity methods
    private void Awake()
    {
        _trackedImagesManager = GetComponent<ARTrackedImageManager>();
    }

    private void OnEnable()
    {
        _trackedImagesManager.trackedImagesChanged += OnTrackedImagesChanged;
    }

    private void OnDisable()
    {
        _trackedImagesManager.trackedImagesChanged -= OnTrackedImagesChanged;
    }

    private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        if (!_isDetected)
        {
            // Adding prefab
            foreach (ARTrackedImage trackedImage in eventArgs.added)
            {
                string detectedImageName = trackedImage.referenceImage.name;
                if (IsImageSmokingSign(detectedImageName))
                {
                    _smokingObject = Instantiate(smokingPrefab, trackedImage.transform);
                    OnSmokingSignVisible?.Invoke();
                    _isDetected = true;
                }
            }
        }
        else
        {
            // Update prefab
            foreach (ARTrackedImage trackedImage in eventArgs.updated)
            {
                string detectedImageName = trackedImage.referenceImage.name;
                if (IsImageSmokingSign(detectedImageName))
                {
                    bool updateState = trackedImage.trackingState == TrackingState.Tracking;
                    _smokingObject.SetActive(updateState);

                    if (updateState) { OnSmokingSignVisible?.Invoke(); }
                    else { OnSmokingSignInvisible?.Invoke(); }
                }
            }

            // Destroy prefab
            foreach (ARTrackedImage trackedImage in eventArgs.removed)
            {
                string detectedImageName = trackedImage.referenceImage.name;
                if (IsImageSmokingSign(detectedImageName))
                {
                    Destroy(_smokingObject);
                    OnSmokingSignInvisible?.Invoke();
                    _smokingObject = null;
                    _isDetected = false;
                }
            }
        }
    }
    #endregion


    /*------->>> Validation methods <<<-------*/
    #region Validation methods
    private bool IsImageSmokingSign(string detectedImageName)
    {
        return (string.Compare(imageName, detectedImageName, StringComparison.OrdinalIgnoreCase) == 0);
    }
    #endregion
}