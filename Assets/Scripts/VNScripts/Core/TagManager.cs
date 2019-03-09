using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TagManager : MonoBehaviour 
{
    public static void Inject(ref string s)
    {
        if (!s.Contains("["))
            return;

        //replace the mainCharName tag with the actual tag
        s = s.Replace("[mainCharName]", "Chris"); 

  
    }


	public static string[] SplitByTags(string targetText)
	{
		return targetText.Split(new char[2]{'<','>'});
	}
}
