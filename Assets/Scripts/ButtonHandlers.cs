using System.Collections;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Management;

public class ButtonHandlers : MonoBehaviour
{
	int width = Screen.width;   // for Taking Picture
	int height = Screen.height; // for Taking Picture

	public void LaunchScene(string sceneName)
    {
		Debug.Log("LoadScene: " + sceneName);
		var xrManagerSettings = XRGeneralSettings.Instance.Manager;
		xrManagerSettings.DeinitializeLoader();
		SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
		xrManagerSettings.InitializeLoaderSync();
	}
	public void TakePicture()
    {
		Debug.Log("TakePicture()");

		if (Application.platform == RuntimePlatform.Android)
		{
			if (!Permission.HasUserAuthorizedPermission(Permission.ExternalStorageWrite))
			{
				Permission.RequestUserPermission(Permission.ExternalStorageWrite);
				if (!Permission.HasUserAuthorizedPermission(Permission.ExternalStorageWrite)) 
					return;
				else 
					StartCoroutine(CaptureScreen());
			}
			else
			{
				StartCoroutine(CaptureScreen());
			}
		}
		else
		{
			StartCoroutine(CaptureScreen());
		}
	}

	public IEnumerator CaptureScreen()
	{
		Debug.Log("CaptureScreen()");
		yield return null; // Wait till the last possible moment before screen rendering to hide the UI

		GameObject.Find("AppInfoCanvas").GetComponent<Canvas>().enabled = false;
		GameObject.Find("AppInfoCanvas").GetComponent<AudioSource>().Play(0);
		yield return new WaitForEndOfFrame(); // Wait for screen rendering to complete
		Texture2D tex = new Texture2D(width, height);
		tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
		tex.Apply();

		yield return tex;
		string screenShotName = "SamoborNT-AR-" + System.DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss");
		string path = SaveImageToGallery(tex, screenShotName, "Samobor N&T AR Picture");
		Debug.Log("Picture saved at:\n" + path);
		GameObject.Find("AppInfoCanvas").GetComponent<Canvas>().enabled = true; // Show UI after we're done

		string[] paths = new string[1];
		paths[0] = path;
		ScanFile(paths);
	}
	void ScanFile(string[] paths)
	{
		using (AndroidJavaClass PlayerActivity = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
		{
			AndroidJavaObject playerActivity = PlayerActivity.GetStatic<AndroidJavaObject>("currentActivity");
			using (AndroidJavaObject Conn = new AndroidJavaObject("android.media.MediaScannerConnection", playerActivity, null))
			{
				Conn.CallStatic("scanFile", playerActivity, paths, null, null);
			}
		}
	}

	protected const string MEDIA_STORE_IMAGE_MEDIA = "android.provider.MediaStore$Images$Media";
	protected static AndroidJavaObject m_Activity;

	protected static string SaveImageToGallery(Texture2D a_Texture, string a_Title, string a_Description)
	{
		using (AndroidJavaClass mediaClass = new AndroidJavaClass(MEDIA_STORE_IMAGE_MEDIA))
		{
			using (AndroidJavaObject contentResolver = Activity.Call<AndroidJavaObject>("getContentResolver"))
			{
				AndroidJavaObject image = Texture2DToAndroidBitmap(a_Texture);
				return mediaClass.CallStatic<string>("insertImage", contentResolver, image, a_Title, a_Description);
			}
		}
	}

	protected static AndroidJavaObject Texture2DToAndroidBitmap(Texture2D a_Texture)
	{
		byte[] encodedTexture = a_Texture.EncodeToJPG();
		sbyte[] signedEncodedTexture = new sbyte[encodedTexture.Length];
        System.Buffer.BlockCopy(encodedTexture, 0, signedEncodedTexture, 0, encodedTexture.Length);

		using (AndroidJavaClass bitmapFactory = new AndroidJavaClass("android.graphics.BitmapFactory"))
		{
			return bitmapFactory.CallStatic<AndroidJavaObject>("decodeByteArray", signedEncodedTexture, 0, encodedTexture.Length);
		}
	}

	protected static AndroidJavaObject Activity
	{
		get
		{
			if (m_Activity == null)
			{
				AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
				m_Activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
			}
			return m_Activity;
		}
	}
}
