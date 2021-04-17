using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR.Management;
using UnityEngine.XR.ARFoundation;

public class InteractionScript : MonoBehaviour
{
    private bool hasExtra;
    private AndroidJavaObject extras;
    private AndroidJavaObject intent;
    private AndroidJavaClass UnityPlayer;
    private AndroidJavaObject currentActivitty;
    private string sceneName;
    [SerializeField] private ARSession arSession;
    public static ARSession activeArSession;

    void Start()
    {
        /*
        Debug.Log("Start");
        if (activeArSession == null)
        {
            if (arSession != null)
            {
                activeArSession = arSession;
                Debug.Log("activeArSession set");
            }
        }
        if (arSession != null)
        {
            arSession.Reset();
            Debug.Log("arSession Reset");
            if (activeArSession != arSession)
            {
                activeArSession.Reset();
                Debug.Log("activeArSession Reset");
            }
        }*/
        if (activeArSession != null)
        {
            activeArSession = arSession;
            Debug.Log("activeArSession set");
        }
        hasExtra = false;
        Debug.Log("Scene started: " + SceneManager.GetActiveScene().name);
        try
        {
            UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            currentActivitty = UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            intent = currentActivitty.Call<AndroidJavaObject>("getIntent");
            hasExtra = intent.Call<bool>("hasExtra", "sceneName");
            Debug.Log("hasExtra: " + hasExtra);
        }
        catch
        {
            Debug.Log("hasExtra - catch: " + false);
        }
    }
    private void Update()
    {
        if (hasExtra)
        {
            extras = intent.Call<AndroidJavaObject>("getExtras");
            sceneName = extras.Call<string>("getString", "sceneName");
            intent.Call("removeExtra", "sceneName");
            if (SceneManager.GetActiveScene().name != sceneName)
            {
                if (arSession != null)
                    Destroy(activeArSession);
                Debug.Log("LoadScene: " + sceneName);
                SceneManager.LoadScene(sceneName);
            }
            hasExtra = false;
        }
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                Debug.Log("Quit");
                //ARSession session = goARCoreDevice.GetComponent<ARCoreSession>();
                //ARSession.Reset();
                if (arSession != null)
                    Destroy(activeArSession);
                Application.Quit();
                //currentActivitty.Call<bool>("moveTaskToBack", true);
                //code for calling Android function
                //AndroidJavaClass UnityHolderActivity = new AndroidJavaClass("com.strukovnasamobor.samobornt.UnityHolderActivity");
                //UnityHolderActivity.CallStatic("Call", currentActivitty);
            }
        }
    }
}
