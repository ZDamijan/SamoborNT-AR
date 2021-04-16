using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR.Management;

public class InteractionScript : MonoBehaviour
{
    private bool hasExtra;
    private AndroidJavaObject extras;
    private AndroidJavaObject intent;
    private AndroidJavaClass UnityPlayer;
    private AndroidJavaObject currentActivitty;
    private string arguments;

    void Start()
    {
        try
        {
            UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            currentActivitty = UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            intent = currentActivitty.Call<AndroidJavaObject>("getIntent");
            hasExtra = intent.Call<bool>("hasExtra", "arguments");
        }
        catch
        {
            hasExtra = false;
        }
    }
    private void Update()
    {
        if (hasExtra)
        {
            extras = intent.Call<AndroidJavaObject>("getExtras");
            arguments = extras.Call<string>("getString", "arguments");
            if (SceneManager.GetActiveScene().name != arguments)
            {
                SceneManager.LoadScene(arguments);
            }
        }
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                Application.Quit();
                //currentActivitty.Call<bool>("moveTaskToBack", true);
                //code for calling Android function
                //AndroidJavaClass UnityHolderActivity = new AndroidJavaClass("com.strukovnasamobor.samobornt.UnityHolderActivity");
                //UnityHolderActivity.CallStatic("Call", currentActivitty);
            }
        }
    }
}
