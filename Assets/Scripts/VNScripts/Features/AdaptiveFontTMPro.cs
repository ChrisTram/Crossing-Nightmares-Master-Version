using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AdaptiveFontTMPro : MonoBehaviour {

	TextMeshProUGUI txt;
	public bool continualUpdate = true;

	public int fontSizeAtDefaultResolution = 40;
	public static float defaultResolution = 2525f;

	// Use this for initialization
	void Start () 
	{
		txt = GetComponent<TextMeshProUGUI> ();

		if (continualUpdate)
		{
			InvokeRepeating ("Adjust", 0f, Random.Range (0.3f, 1f));
		}
		else
		{
			Adjust ();
			enabled = false;
		}
	}


	//adjust is repeating so it must be checked for active status before running.
	void Adjust()
	{
		if (!enabled || !gameObject.activeInHierarchy)
			return;

		float totalCurrentRes = Screen.height + Screen.width;
		float perc = totalCurrentRes / defaultResolution;
		int fontsize = Mathf.RoundToInt ((float)fontSizeAtDefaultResolution * perc);

		txt.fontSize = fontsize;
	}
}
