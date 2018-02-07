using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PluginTest : MonoBehaviour {

	const string pluginName = "com.cwgtech.unity.MyPlugin";

	class AlertViewCallback : AndroidJavaProxy
	{
		private System.Action<int> alertHandler;

		public AlertViewCallback(System.Action<int>alertHandlerIn) : base (pluginName + "$AlertViewCallback")
		{
			alertHandler = alertHandlerIn;
		}
		public void onButtonTapped(int index)
		{
			Debug.Log("Button tapped: " + index);
			if (alertHandler != null)
				alertHandler(index);
		}
	}

	static AndroidJavaClass _pluginClass;
	static AndroidJavaObject _pluginInstance;

	public static AndroidJavaClass PluginClass
	{
		get {
			if (_pluginClass==null)
			{
				_pluginClass = new AndroidJavaClass(pluginName);
				AndroidJavaClass playerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
				AndroidJavaObject activity = playerClass.GetStatic<AndroidJavaObject>("currentActivity");
				_pluginClass.SetStatic<AndroidJavaObject>("mainActivity",activity);
			}
			return _pluginClass;
		}
	}

	public static AndroidJavaObject PluginInstance
	{
		get {
			if (_pluginInstance==null)
			{
				_pluginInstance = PluginClass.CallStatic<AndroidJavaObject>("getInstance");
			}
			return _pluginInstance;
		}
	}

	// Use this for initialization
	void Start () {

		Debug.Log("Elapsed Time: " + getElapsedTime());
		
	}

	float elapsedTime = 0;
	// Update is called once per frame
	void Update () {
		elapsedTime += Time.deltaTime;
		if (elapsedTime>=5)
		{
			elapsedTime -= 5;
			Debug.Log("Tick: " + getElapsedTime());
		}

		if (Input.GetMouseButtonDown(0))
		{
			showAlertDialog(new string[]{"Alert Title","Alert Message","Button 1","Button 2"},(int obj) => {
				Debug.Log("Local Handler called: " + obj);
			});
		}
		
	}

	double getElapsedTime()
	{
		if (Application.platform == RuntimePlatform.Android)
			return PluginInstance.Call<double>("getElapsedTime");
		Debug.LogWarning("Wrong platform");
		return 0;
	}

	void showAlertDialog(string[] strings, System.Action<int>handler = null)
	{
		if (strings.Length<3)
		{
			Debug.LogError("AlertView requires at least 3 strings");
			return;
		}

		if (Application.platform == RuntimePlatform.Android)
			PluginInstance.Call("showAlertView", new object[] { strings, new AlertViewCallback(handler) });
		else
			Debug.LogWarning("AlertView not supported on this platform");
	}
}
