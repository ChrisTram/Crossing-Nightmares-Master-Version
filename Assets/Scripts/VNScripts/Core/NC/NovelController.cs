using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NovelController : MonoBehaviour
{
    public static NovelController instance;
    /// <summary> The lines of data loaded directly from a chapter file.	/// </summary>
    List<string> data = new List<string>();
    /// <summary> the progress in the current data list.	/// </summary>
    int progress = 0;

    
    //GameObject[] clickables = null;
    bool canNext = true;

    private void Awake()
    {
        instance = this;
    }

    // Use this for initialization
    void Start()
    {
        //clickables = GameObject.FindGameObjectsWithTag("Clickable"); //Clickable objects of the scene

        LoadTestChapterFile("Intro-Fr");
        //LoadChapterFile("Chapter0_start");
    }

    // Update is called once per frame
    void Update()
    {
        //testing
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Next();
        }
    }

    public void LoadChapterFile(string fileName)
    {
        data = FileManager.LoadFile(FileManager.savPath + "Resources/Story/" + fileName);
        cachedLastSpeaker = "";
        if (handlingChapterFile != null)
            StopCoroutine(handlingChapterFile);
        handlingChapterFile = StartCoroutine(HandlingChapterFile());

        //auto start chapter
        Next();
    }

    public void LoadTestChapterFile(string fileName)
    {
        data = FileManager.LoadFile("Y:/Nightmare-Walk-Ressources/Levels/" + fileName);
        cachedLastSpeaker = "";
        if (handlingChapterFile != null)
            StopCoroutine(handlingChapterFile);
        handlingChapterFile = StartCoroutine(HandlingChapterFile());

        //auto start chapter
        Next();
    }

    bool _next = false;
    public void Next()
    {
        _next = true;
    }

    public bool isHandlingChapterFile { get { return handlingChapterFile != null; } }
    Coroutine handlingChapterFile = null;
    [HideInInspector] private int chapterProgress = 0;
    IEnumerator HandlingChapterFile()
    {
        //the progress through the lines in the chapter
        chapterProgress = 0;

        while (chapterProgress < data.Count)
        {
            //we need a way of knowing when the payer wants to advance. A next Trigger, that can work from keyboard or mouse.
            if (_next)
            {
                string line = data[chapterProgress];

                //Choice
                if(line.StartsWith("choice"))
                {
                    yield return HandlingChoiceLine(line);
                    chapterProgress++;
                }
                //Normal line
                else
                {
                    HandleLine(line);
                    chapterProgress++;
                    while (isHandlingLine)
                    {
                        yield return new WaitForEndOfFrame();
                    }
                }

            }
            yield return new WaitForEndOfFrame();
        }

        handlingChapterFile = null;
    }

    IEnumerator HandlingChoiceLine(string line)
    {
        string title = line.Split('"')[1];
        List<string> choices = new List<string>();
        List<string> actions = new List<string>();

        bool gatheringChoices = true;

        while(gatheringChoices)
        {
            chapterProgress++;
            line = data[chapterProgress];

            if (line == "{")
                continue;

            line = line.Replace("\t", "");//remove the tabs that have become quad spaces.

            if (line != "}")
            {
                choices.Add(line.Split('"')[1]);
                actions.Add(data[chapterProgress + 1].Replace("\t", ""));
                chapterProgress++;
            }
            else
            {
                gatheringChoices = false;
            }
        }

        //display choices if 
        if (choices.Count > 0)
        {
            ChoiceScreen.Show(title, choices.ToArray()); yield return new WaitForEndOfFrame();
            while (ChoiceScreen.isWaitingForChoiceToBeMade)
                yield return new WaitForEndOfFrame();

            //choice is made
            string action = actions[ChoiceScreen.lastChoiceMade.index];
            HandleLine(action);

            while (isHandlingLine)
                yield return new WaitForEndOfFrame();
        }
        else
        {
            Debug.LogError("Invalid choice operation. No choices were found.");
        }
    }

    void HandleLine(string rawLine)
    {
        CLM.LINE line = CLM.Interpret(rawLine);

        StopHandlingLine();
        handlingLine = StartCoroutine(HandlingLine(line));
    }

    void StopHandlingLine()
    {
        if (isHandlingLine)
            StopCoroutine(handlingLine);
        handlingLine = null;
    }
    [HideInInspector] public string cachedLastSpeaker = "";

    public bool isHandlingLine { get { return handlingLine != null; } }
    Coroutine handlingLine = null;
    IEnumerator HandlingLine(CLM.LINE line)
    {
        //next trigger controls also the progression through a line by its segments, so it must be reset
        _next = false;
        int lineProgress = 0; //Progress through the segments of a line

        while (lineProgress < line.segments.Count)
        {
            _next = false;//reset at the start of cheap loop
            CLM.LINE.SEGMENT segment = line.segments[lineProgress];

            //always run the first segment automatically
            if (lineProgress > 0)
            {
                if (segment.trigger == CLM.LINE.SEGMENT.TRIGGER.autoDelay)  //if it's a segment that need a yield {
                {
                    for (float timer = segment.autoDelay; timer >= 0; timer -= Time.deltaTime)
                    {
                        yield return new WaitForEndOfFrame();
                        if (_next)
                            break; //Because the user must be able to skip a wait time if he wants to here
                    }
                }
                else if(segment.trigger == CLM.LINE.SEGMENT.TRIGGER.autoDelayBlock) //same, but the user won't be able to skip the wait
                {
                    for (float timer = segment.autoDelay; timer >= 0; timer -= Time.deltaTime)
                    {
                        yield return new WaitForEndOfFrame();
                    }
                }
                else
                {
                    while (!_next)
                        yield return new WaitForEndOfFrame(); //wait until the player trigger the next segment
                }
            }
            _next = false;

            //the segment now needs to build and run.
            segment.Run();

            while (segment.isRunning)
            {
                yield return new WaitForEndOfFrame();
                if (_next)
                {
                    if (!segment.architect.skip)
                        segment.architect.skip = true;
                    else
                        segment.ForceFinish(); //for the finish if the user triggered 
                    _next = false;
                }
            }

            lineProgress++;

            yield return new WaitForEndOfFrame();
        }
        //Handling all actions set a the end of the line
        for (int i = 0; i < line.actions.Count; i++)
        {
            HandleAction(line.actions[i]);
        }
        handlingLine = null;
    }

    /// <summary>
    /// Used as a fallback when no speaker is given.
    /// </summary>



    //ACTIONS

    public void HandleAction(string action)
    {
        print("execute command - " + action);
        string[] data = action.Split('(', ')');

        switch (data[0])
        {
            case "enter":
                Command_Enter(data[1]);
                break;

            case "exit":
                Command_Exit(data[1]);
                break;

            case ("setBackground"):
                Command_SetLayerImage(data[1], BCFC.instance.background);
                break;

            case ("setCinematic"):
                Command_SetLayerImage(data[1], BCFC.instance.cinematic);
                break;

            case ("setForeground"):
                Command_SetLayerImage(data[1], BCFC.instance.foreground);
                break;

            case ("playSound"):
                Command_PlaySound(data[1]);
                break;

            case ("playMusic"):
                Command_PlayMusic(data[1]);
                break;

            case ("move"):
                Command_MoveCharacter(data[1]);
                break;

            case ("setPosition"):
                Command_SetPosition(data[1]);
                break;

            case ("setFace"):
                Command_SetFace(data[1]);
                break;

            case ("setBody"):
                Command_SetBody(data[1]);
                break;

            case ("flip"):
                Command_Flip(data[1]);
                break;

            case ("faceLeft"):
                Command_FaceLeft(data[1]);
                break;

            case ("faceRight"):
                Command_FaceRight(data[1]);
                break;

            case ("transBackground"):
                Command_TransLayer(BCFC.instance.background, data[1]);
                break;

            case ("transCinematic"):
                Command_TransLayer(BCFC.instance.cinematic, data[1]);
                break;

            case ("transForeground"):
                Command_TransLayer(BCFC.instance.foreground, data[1]);
                break;

            case ("showScene"):
                Command_ShowScene(data[1]);
                break;

            case ("Load"):
                Command_Load(data[1]);
                break;

            case ("block"): //Block the users interractions by switching activations of the clickables button 
                Command_Block();
                break;
        }
    }

    void Command_Load(string chapterName)
    {
        NovelController.instance.LoadChapterFile(chapterName);
    }

    void Command_Block()
    {
        switchCanNext();
        //switchClickablesButton();
        
    }

    public void switchCanNext()
    {
        NovelController.instance.canNext = (!NovelController.instance.canNext);
    }
    /* Pourra peut être servir
    public void switchClickablesButton()
    {
        NovelController.instance.canNext = (!NovelController.instance.canNext);
        Debug.Log("Can Next is " + NovelController.instance.canNext);

        foreach (GameObject clickable in NovelController.instance.clickables)
        {
            clickable.SetActive(NovelController.instance.canNext);
        }

    }*/

    void Command_SetLayerImage(string data, BCFC.LAYER layer)
    {
        string texName = data.Contains(",") ? data.Split(',')[0] : data;
        Texture2D tex = texName == "null" ? null : Resources.Load("Images/UI/Backdrops/" + texName) as Texture2D;
        float spd = 2f;
        bool smooth = false;

        if (data.Contains(","))
        {
            string[] parameters = data.Split(',');
            foreach (string p in parameters)
            {
                float fVal = 0;
                bool bVal = false;
                if (float.TryParse(p, out fVal))
                {
                    spd = fVal; continue;
                }
                if (bool.TryParse(p, out bVal))
                {
                    smooth = bVal; continue;
                }
            }
        }

        layer.TransitionToTexture(tex, spd, smooth);
    }

    void Command_PlaySound(string data)
    {
        AudioClip clip = Resources.Load("Audio/SFX/" + data) as AudioClip;

        if (clip != null)
            AudioManager.instance.PlaySFX(clip);
        else
            Debug.LogError("Clip does not exist - " + data);
    }

    void Command_PlayMusic(string data)
    {
        AudioClip clip = Resources.Load("Audio/Music/" + data) as AudioClip;

        AudioManager.instance.PlaySong(clip);
    }

    void Command_MoveCharacter(string data)
    {
        string[] parameters = data.Split(',');
        string character = parameters[0];
        float locationX = float.Parse(parameters[1]);
        float locationY = parameters.Length >= 3 ? float.Parse(parameters[2]) : 0;
        float speed = parameters.Length >= 4 ? float.Parse(parameters[3]) : 7f;
        bool smooth = parameters.Length == 5 ? bool.Parse(parameters[4]) : true;

        Character c = CharacterManager.instance.GetCharacter(character);
        c.MoveTo(new Vector2(locationX, locationY), speed, smooth);
    }

    void Command_SetPosition(string data)
    {
        string[] parameters = data.Split(',');
        string character = parameters[0];
        float locationX = float.Parse(parameters[1]);
        float locationY = parameters.Length == 3 ? float.Parse(parameters[2]) : 0;

        Character c = CharacterManager.instance.GetCharacter(character);
        c.SetPosition(new Vector2(locationX, locationY));

        print("set " + c.characterName + " position to " + locationX + "," + locationY);
    }

    void Command_SetFace(string data)
    {
        string[] parameters = data.Split(',');
        string character = parameters[0];
        string expression = parameters[1];
        float speed = parameters.Length == 3 ? float.Parse(parameters[2]) : 3f;

        Character c = CharacterManager.instance.GetCharacter(character);
        Sprite sprite = c.GetSprite(expression);

        c.TransitionExpression(sprite, speed, false);
    }

    void Command_SetBody(string data)
    {
        string[] parameters = data.Split(',');
        string character = parameters[0];
        string expression = parameters[1];
        float speed = parameters.Length == 3 ? float.Parse(parameters[2]) : 3f;

        Character c = CharacterManager.instance.GetCharacter(character);
        Sprite sprite = c.GetSprite(expression);

        c.TransitionBody(sprite, speed, false);
    }

    void Command_Flip(string data)
    {
        string[] characters = data.Split(';');

        foreach (string s in characters)
        {
            Character c = CharacterManager.instance.GetCharacter(s);
            c.Flip();
        }
    }

    void Command_FaceLeft(string data)
    {
        string[] characters = data.Split(';');

        foreach (string s in characters)
        {
            Character c = CharacterManager.instance.GetCharacter(s);
            c.FaceLeft();
        }
    }

    void Command_FaceRight(string data)
    {
        string[] characters = data.Split(';');

        foreach (string s in characters)
        {
            Character c = CharacterManager.instance.GetCharacter(s);
            c.FaceRight();
        }
    }

    void Command_Exit(string data)
    {
        string[] parameters = data.Split(',');
        string[] characters = parameters[0].Split(';');
        float speed = 3;
        bool smooth = false;
        for (int i = 1; i < parameters.Length; i++)
        {
            float fVal = 0; bool bVal = false;
            if (float.TryParse(parameters[i], out fVal))
            { speed = fVal; continue; }
            if (bool.TryParse(parameters[i], out bVal))
            { smooth = bVal; continue; }
        }

        foreach (string s in characters)
        {
            Character c = CharacterManager.instance.GetCharacter(s);
            c.FadeOut(speed, smooth);
        }
    }

    void Command_Enter(string data)
    {
        string[] parameters = data.Split(',');
        string[] characters = parameters[0].Split(';');
        float speed = 3;
        bool smooth = false;
        for (int i = 1; i < parameters.Length; i++)
        {
            float fVal = 0; bool bVal = false;
            if (float.TryParse(parameters[i], out fVal))
            { speed = fVal; continue; }
            if (bool.TryParse(parameters[i], out bVal))
            { smooth = bVal; continue; }
        }

        foreach (string s in characters)
        {
            Character c = CharacterManager.instance.GetCharacter(s, true, false);
            if (!c.enabled)
            {
                c.renderers.bodyRenderer.color = new Color(1, 1, 1, 0);
                c.renderers.expressionRenderer.color = new Color(1, 1, 1, 0);
                c.enabled = true;

                c.TransitionBody(c.renderers.bodyRenderer.sprite, speed, smooth);
                c.TransitionExpression(c.renderers.expressionRenderer.sprite, speed, smooth);
            }
            else
                c.FadeIn(speed, smooth);
        }
    }

    void Command_TransLayer(BCFC.LAYER layer, string data)
    {
        string[] parameters = data.Split(',');

        string texName = parameters[0];
        string transTexName = parameters[1];
        Texture2D tex = texName == "null" ? null : Resources.Load("Images/UI/Backdrops/" + texName) as Texture2D;
        Texture2D transTex = Resources.Load("Images/TransitionEffects/" + transTexName) as Texture2D;

        float spd = 2f;
        bool smooth = false;

        for (int i = 2; i < parameters.Length; i++)
        {
            string p = parameters[i];
            float fVal = 0;
            bool bVal = false;
            if (float.TryParse(p, out fVal))
            { spd = fVal; continue; }
            if (bool.TryParse(p, out bVal))
            { smooth = bVal; continue; }
        }

        TransitionMaster.TransitionLayer(layer, tex, transTex, spd, smooth);
    }

    void Command_ShowScene(string data)
    {
        string[] parameters = data.Split(',');
        bool show = bool.Parse(parameters[0]);
        string texName = parameters[1];
        Texture2D transTex = Resources.Load("Images/TransitionEffects/" + texName) as Texture2D;
        float spd = 2f;
        bool smooth = false;

        for (int i = 2; i < parameters.Length; i++)
        {
            string p = parameters[i];
            float fVal = 0;
            bool bVal = false;
            if (float.TryParse(p, out fVal))
            { spd = fVal; continue; }
            if (bool.TryParse(p, out bVal))
            { smooth = bVal; continue; }
        }

        TransitionMaster.ShowScene(show, spd, smooth, transTex);
    }
}
