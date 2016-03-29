using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HelperFunctions : MonoBehaviour {
	public static long scheduleRequestCount = 0; 
	static HelperFunctions _instance;
	public static HelperFunctions instance
	{
		get
		{
			if (_instance == null)
			{
				GameObject obj = new GameObject();
				obj.name = "HelperFunctions";
				_instance = obj.AddComponent<HelperFunctions>();
				DontDestroyOnLoad(obj);
			}
			return _instance;
		}
	}
	
	public static void doCoroutine(IEnumerator routine)
	{
		instance.StartCoroutine(routine);
	}
	
	public static void endCoRoutine(IEnumerator routine)
	{
		instance.StopCoroutine(routine);
	}
	
	public delegate void Callback();
	public static void DelayCallback(float delay, Callback callback)
	{
		instance.StartCoroutine(DelayCallbackCoroutine(delay, callback));
	}
	
	static IEnumerator DelayCallbackCoroutine(float delay, Callback callback)
	{
		yield return new WaitForSeconds(delay);
		callback();
	}
	
	// A delay of 1 will be the same frame but at the end of it, 2 will be end of next frame, etc.
	public static void DelayCallbackForFrames(int delay, Callback callback)
	{
		instance.StartCoroutine(DelayCallbackCoroutineForFrames(delay, callback));
	}
	
	static IEnumerator DelayCallbackCoroutineForFrames(int delay, Callback callback)
	{
		for (int i = 0; i < delay; ++i)
		{
			yield return new WaitForEndOfFrame();
		}
		callback();
	}
	
	public static void tick(float duration, int count, Callback callback, bool doNow = false)
	{
		instance.StartCoroutine(tickCoroutine(duration, count, callback, doNow));
	}
	
	static IEnumerator tickCoroutine(float delay, int count, Callback callback, bool doNow = false)
	{
		if (doNow) callback();
		
		while (count-- >= 0)
		{
			yield return new WaitForSeconds(delay);
			callback();
		}
	}
	
	public static void waitLoadScene(float waitTime, string sceneName)
	{
		instance.StartCoroutine(waitLoadSceneRoutine(waitTime, sceneName));
	}
	
	static IEnumerator waitLoadSceneRoutine(float waitTime, string sceneName)
	{
		yield return new WaitForSeconds(waitTime);
		Application.LoadLevel(sceneName);
	}

    // hours:minutes: seconds
    public static string getTimeString(int seconds, bool alwaysShowHours = false)
    {
        string result = "";
        int hours = seconds / 3600;
        seconds -= (hours * 3600);

        int minutes = seconds / 60;
        seconds -= (minutes * 60);

        // add seconds to string
        result = seconds.ToString();
        if (seconds < 10) result = "0" + result;

        // add minutes
        result = minutes.ToString() + ":" + result;
        if(minutes < 10) result = "0" + result;

        // add hours
        if(hours > 0 || alwaysShowHours)
        {
            result = hours.ToString() + ":" + result;
            if (hours < 10) result = "0" + result;
        }

        return result;
    }
	
	/**
     * Randomize List element order in-place.
     * Using Fisher-Yates shuffle algorithm.
    */
	public static List<T> shuffleList<T>(List<T> inputList){
		for (int i = inputList.Count - 1; i > 0; --i)
		{
			int j = Mathf.FloorToInt(UnityEngine.Random.value * (i + 1));
			T temp = inputList[i];
			inputList[i] = inputList[j];
			inputList[j] = temp;
		}
		
		return inputList;
	}
}