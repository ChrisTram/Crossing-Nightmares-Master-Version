using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdaptiveText : MonoBehaviour 
{
	Text txt;
	public bool continualUpdate = true;

	public int fontsizeAtDefaultResolution = 24;
	public static float defaultRes = 1658f;

	// Use this for initialization
	void Start () 
	{
		txt = GetComponent<Text>();

		if (continualUpdate)
		{
			InvokeRepeating("Adjust", 0f, Random.Range(0.5f, 2f));
		}
		else
		{
			Adjust();
			enabled = false;
		}
	}
	
	void Adjust()
	{
		if (!enabled || !gameObject.activeInHierarchy)
		{
			print("text is disabled.");
			return;
		}

		int totalRes = Screen.height + Screen.width;

		float perc = (float) totalRes / defaultRes;

		int fontSize = Mathf.RoundToInt ((float) fontsizeAtDefaultResolution * perc);

		txt.fontSize = fontSize;
	}
}
