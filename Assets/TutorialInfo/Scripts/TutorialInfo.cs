using System;
using UnityEngine;
using UnityEngine.UI;

// Hi! This script presents the overlay info for our tutorial content, linking you back to the relevant page.
public class TutorialInfo : MonoBehaviour 
{
    public Toggle toggle3D;
    public Toggle toggle2D;
    public Toggle toggleVisualOn;
    public Toggle toggleVisualOff;
    public Toggle toggleHapticOn;
    public Toggle toggleHapticOff;

    public InputField idInput;

    public Text ErrorMessage;
    public GameObject ErrorWindow;   

    Done_GameController gameController;

	// allow user to choose whether to show this menu 
	public bool showAtStart = true;

	// location that Visit Tutorial button sends the user
	public string url;

	// store the GameObject which renders the overlay info
	public GameObject overlay;

	// store a reference to the UI toggle which allows users to switch it off for future plays

	// string to store Prefs Key with name of preference for showing the overlay info
	public static string showAtStartPrefsKey = "showLaunchScreen";


    void validation()
    {

    }
    void InitializeUI()
    {
        toggle3D.isOn = false;
        toggle2D.isOn = true;
        


        toggleHapticOn.isOn = false;
        toggleHapticOff.isOn = true;

        toggleVisualOn.isOn = false;
        toggleVisualOff.isOn = true;


    }

    private void Start()
    {
        gameController = GameObject.Find("Game Controller").GetComponent<Done_GameController>();
    }

    void Awake()
	{
        InitializeUI();

        
        // Check player prefs for show at start preference
        overlay.SetActive(true);
        ShowLaunchScreen();

	}

	// show overlay info, pausing game time, disabling the audio listener 
	// and enabling the overlay info parent game object
	public void ShowLaunchScreen()
	{
		Time.timeScale = 0f;
        AudioListener.volume = 0f;
	    overlay.SetActive (true);
	}

	// open the stored URL for this content in a web browser
	public void LaunchTutorial()
	{
		Application.OpenURL (url);
	}

	// continue to play, by ensuring the preference is set correctly, the overlay is not active, 
	// and that the audio listener is enabled, and time scale is 1 (normal)
	public void StartGame()
	{
        SetData();
        Done_GameController.timeOver = false;
		overlay.SetActive (false);
        AudioListener.volume = 1f;
        Time.timeScale = 1f;
	}

    private void SetData()
    {
        Done_GameController.EnabledHapticMode = toggleHapticOn.isOn;
        Done_GameController.EnabledVisualMode = toggleVisualOn.isOn;
        Done_GameController.Enabled3DMode = toggle3D.isOn;
        Done_GameController.UserId = idInput.text;

        gameController.SetCameraMode(toggle3D.isOn );
    }

    // set the boolean storing show at start status to equal the UI toggle's status


    internal void ShowErrorMessage(string message)
    {
        ErrorWindow.SetActive(true);
        ErrorMessage.text = message;
                       
    }
}   
