using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LauncherButtonHandler : MonoBehaviour
{
    public void LaunchScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
