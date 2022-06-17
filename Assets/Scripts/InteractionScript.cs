using UnityEngine;
using UnityEngine.SceneManagement;
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

    void Start()
    {
        Debug.Log("Scene started: " + SceneManager.GetActiveScene().name);
        if (arSession != null)
        {
            arSession.Reset();
            Debug.Log("arSession Reset");
        }
        hasExtra = false;
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
                Debug.Log("LoadScene: " + sceneName);
                var xrManagerSettings = XRGeneralSettings.Instance.Manager;
                xrManagerSettings.DeinitializeLoader();
                SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
                xrManagerSettings.InitializeLoaderSync();
            }
            hasExtra = false;
        }
        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                var xrManagerSettings = XRGeneralSettings.Instance.Manager;
                xrManagerSettings.DeinitializeLoader();
                Application.Unload();
                
                /*
                if (SceneManager.GetActiveScene().name == "LauncherScreen")
                    Application.Unload();
                else
                {
                    Debug.Log("Return");
                    var xrManagerSettings = XRGeneralSettings.Instance.Manager;
                    xrManagerSettings.DeinitializeLoader();
                    SceneManager.LoadScene("LauncherScreen", LoadSceneMode.Single);
                    xrManagerSettings.InitializeLoaderSync();
                }
                */

                //currentActivitty.Call<bool>("moveTaskToBack", true);
                //code for calling Android function
                //AndroidJavaClass UnityHolderActivity = new AndroidJavaClass("com.strukovnasamobor.samobornt.UnityHolderActivity");
                //UnityHolderActivity.CallStatic("Call", currentActivitty);
            }
        }
    }
}
