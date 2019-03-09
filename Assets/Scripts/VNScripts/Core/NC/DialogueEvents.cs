using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueEvents : MonoBehaviour {

	public static void HandleEvent(string _event, CLM.LINE.SEGMENT segment)
    {

        if (_event.Contains("("))
        {
            string[] actions = _event.Split(' ');
            for (int i = 0; i < actions.Length; i++)
            {
                NovelController.instance.HandleAction(actions[i]);
            }
            return;
        }

        string[] eventData = _event.Split(' '); //Separate the name and the data 

        switch(eventData[0])
        {
            case "txtSpd":
                EVENT_TxtSpd(eventData[1], segment);
                break;
            case "/txtSpd":
                segment.architect.speed = 1;
                segment.architect.charactersPerFrame = 1;
                break;
        }
    }

    static void EVENT_TxtSpd(string data, CLM.LINE.SEGMENT seg)
    {
        string[] parts = data.Split(',');
        float delay = float.Parse(parts[0]);
        int charactersPerFrame = int.Parse(parts[1]);

        seg.architect.speed = delay;
        seg.architect.charactersPerFrame = charactersPerFrame;
    }
}
