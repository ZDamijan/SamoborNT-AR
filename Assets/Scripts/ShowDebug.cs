using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowDebug : MonoBehaviour
{
    //#if !UNITY_EDITOR
    private string output;
    private string stack;
    public Text txtDebug;

    void OnEnable()
    {
        Application.logMessageReceived += Log;
    }

    void OnDisable()
    {
        Application.logMessageReceived -= Log;
    }

    public void Log(string logString, string stackTrace, LogType type)
    {
        output = logString;
        stack = stackTrace;
        txtDebug.text = output + "\n" + txtDebug.text;
        if (txtDebug.text.Length > 5000)
        {
            txtDebug.text = txtDebug.text.Substring(0, 4000);
        }
    }
    //#endif
}
