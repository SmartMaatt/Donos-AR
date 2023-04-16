using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainCanvasController : MonoBehaviour
{
    /*------->>> Parameters <<<-------*/
    #region Parameters
    [Header("Canvas states")]
    [SerializeField] private Button summonFellowButton;
    [SerializeField] private GameObject fellowOptions;

    [Header("Message box")]
    [SerializeField] private TMP_Text messageLabel;
    [SerializeField] private Image messageBackground;

    // State machine
    private MainCanvasState state = MainCanvasState.Empty;
    #endregion


    /*------->>> Unity methods <<<-------*/
    #region Unity methods
    private void Start()
    {
        EnableUIByState(MainCanvasState.Empty);
    }

    private void OnEnable()
    {
        ARSmokingImageDetector.OnSmokingSignVisible += OnSmokingSignVisibleHander;
        ARSmokingImageDetector.OnSmokingSignInvisible += OnSmokingSignInvisibleHander;
        ARTabToPlaceObject.OnObjectPlaced += OnFellowPlacedHandler;
    }

    private void OnDisable()
    {
        ARSmokingImageDetector.OnSmokingSignVisible -= OnSmokingSignVisibleHander;
        ARSmokingImageDetector.OnSmokingSignInvisible -= OnSmokingSignInvisibleHander;
        ARTabToPlaceObject.OnObjectPlaced -= OnFellowPlacedHandler;
    }
    #endregion


    /*------->>> Activate Methods <<<-------*/
    #region Activate Methods
    public void EnableUIByState(MainCanvasState newState)
    {
        switch (newState)
        {
            case MainCanvasState.Empty:
                ActivateSummonFellowBtn(false);
                ActivateFellowOptions(false);
                break;

            case MainCanvasState.SummonFellowBtn:
                ActivateSummonFellowBtn(true);
                break;

            case MainCanvasState.FellowOptions:
                ActivateFellowOptions(true);
                break;
        }

        if (state != newState)
        {
            DisableUIByState(state);
        }

        state = newState;
    }

    public void DisableUIByState(MainCanvasState newState)
    {
        if (newState != state)
        {
            return;
        }

        switch (newState)
        {
            case MainCanvasState.Empty:
                break;

            case MainCanvasState.SummonFellowBtn:
                ActivateSummonFellowBtn(false);
                break;

            case MainCanvasState.FellowOptions:
                ActivateFellowOptions(false);
                break;
        }

        state = MainCanvasState.Empty;
    }

    private void ActivateSummonFellowBtn(bool activate)
    {
        summonFellowButton.gameObject.SetActive(activate);
    }

    private void ActivateFellowOptions(bool activate)
    {
        fellowOptions.SetActive(activate);
    }
    #endregion


    /*------->>> Debug methods <<<-------*/
    #region Debug methods
    public void DisplayMessage(string message)
    {
        messageLabel.text = message;
        messageBackground.gameObject.SetActive(true);
    }

    public void ClearMessage()
    {
        DisplayMessage(string.Empty);
        messageBackground.gameObject.SetActive(false);
    }
    #endregion


    /*------->>> Event handers <<<-------*/
    #region Event handers
    private void OnSmokingSignVisibleHander()
    {
        EnableUIByState(MainCanvasState.SummonFellowBtn);
    }

    private void OnSmokingSignInvisibleHander()
    {
        DisableUIByState(MainCanvasState.SummonFellowBtn);
    }

    private void OnFellowPlacedHandler()
    {
        EnableUIByState(MainCanvasState.FellowOptions);
        DisplayMessage("<Fellow>: Do you have any problem?");
    }
    #endregion
}

[System.Serializable]
public enum MainCanvasState
{
    Empty = 0,
    SummonFellowBtn = 1,
    FellowOptions = 2
}
