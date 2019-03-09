using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CLM : MonoBehaviour {

    public static LINE Interpret(string rawLine)
    {
        return new LINE(rawLine);
    }

    public class LINE
    {
        /// <summary>Who is speaking on this line. </summary>
        public string speaker = "";

        /// <summary>The segments on this line that make up each piece of dialogue. </summary>
        public List<SEGMENT> segments = new List<SEGMENT>();
        /// <summary> All the actions to be called during or at the end of this line. </summary>
        public List<string> actions = new List<string>();

        public string lastSegmentsWholeDialogue = "";

        public LINE(string rawLine)
        {
            string[] dialogueAndActions = rawLine.Split('"');
            char actionSplitter = ' ';
            string[] actionsArr = dialogueAndActions.Length == 3 ? dialogueAndActions[2].Split(actionSplitter) : dialogueAndActions[0].Split(actionSplitter);
            
            if (dialogueAndActions.Length == 3)//contains dialogue
            {
                speaker = dialogueAndActions[0] == "" ? NovelController.instance.cachedLastSpeaker : dialogueAndActions[0];
                if (speaker[speaker.Length - 1] == ' ')
                    speaker = speaker.Remove(speaker.Length - 1);




                //cache the speaker
                NovelController.instance.cachedLastSpeaker = speaker;

                //segment the dialogue.
                SegmentDialogue(dialogueAndActions[1]);
            }

            //now handle actions. Just capture all actions into the line. They will be handled later.
            for (int i = 0; i < actionsArr.Length; i++)
            {
                actions.Add(actionsArr[i]);
            }

            //the line is now segmented, the actions are loaded, and it is ready to be used.
        }

        /// <summary>
        /// Segments a line of dialogue into a list of individual parts separated by input delays.
        /// </summary>
        /// <param name="dialogue">Dialogue.</param>
        void SegmentDialogue(string dialogue)
        {
            segments.Clear();
            string[] parts = dialogue.Split('{', '}');

            for (int i = 0; i < parts.Length; i++)
            {
                SEGMENT segment = new SEGMENT();
                bool isOdd = i % 2 != 0;

                //commands are odd indexed. Dialogue is always even.
                if (isOdd)
                {
                    //commands and data are split by spaces
                    string[] commandData = parts[i].Split(' ');
                    switch (commandData[0])
                    {
                        case "c"://wait for input and clear.
                            segment.trigger = SEGMENT.TRIGGER.waitClickClear;
                            break;
                        case "a"://wait for input and append.
                            segment.trigger = SEGMENT.TRIGGER.waitClick;
                            //appending requires fetching the text of the previous segment to be the pre text
                            segment.pretext = segments.Count > 0 ? segments[segments.Count - 1].dialogue : "";
                            break;
                        case "w"://wait for set time and clear.
                            segment.trigger = SEGMENT.TRIGGER.autoDelay;
                            segment.autoDelay = float.Parse(commandData[1]);
                            break;
                        case "wb"://wait and block for set time and clear.
                            segment.trigger = SEGMENT.TRIGGER.autoDelayBlock;
                            segment.autoDelay = float.Parse(commandData[1]);
                            break;
                        case "wa"://wait for set time and append.
                            segment.trigger = SEGMENT.TRIGGER.autoDelay;
                            segment.autoDelay = float.Parse(commandData[1]);
                            //appending requires fetching the text of the previous segment to be the pre text
                            segment.pretext = segments.Count > 0 ? segments[segments.Count - 1].dialogue : "";
                            break;
                        case "wba"://wait for set time, block and append.
                            segment.trigger = SEGMENT.TRIGGER.autoDelayBlock;
                            segment.autoDelay = float.Parse(commandData[1]);
                            //appending requires fetching the text of the previous segment to be the pre text
                            segment.pretext = segments.Count > 0 ? segments[segments.Count - 1].dialogue : "";
                            break;
                    }
                    i++;//increment so we move past the command and to the next bit of dialogue.
                }

                segment.dialogue = parts[i];
                segment.line = this;

                segments.Add(segment);
            }
        }

        public class SEGMENT
        {
            public LINE line;
            public string dialogue = "";
            public string pretext = "";
            public enum TRIGGER { waitClick, waitClickClear, autoDelay, autoDelayBlock }
            public TRIGGER trigger = TRIGGER.waitClickClear;

            public float autoDelay = 0;

            /// <summary>
            /// Run the segment and watch the dialogue build and display on the screen.
            /// </summary>
            public void Run()
            {
                if (running != null)
                    NovelController.instance.StopCoroutine(running);
                running = NovelController.instance.StartCoroutine(Running());
            }

            public bool isRunning { get { return running != null; } }
            Coroutine running = null;
            public TextArchitect architect = null;
            List<string> allCurrentlyExecutedEvents = new List<string>();
            IEnumerator Running()
            {
                allCurrentlyExecutedEvents.Clear();
                //take care of any tags that must be injected into the dialogue before we worry about events.
                
                TagManager.Inject(ref dialogue); 

                //split the dialogue by the event characters.
                string[] parts = dialogue.Split('[', ']');

                for (int i = 0; i < parts.Length; i++)
                {
                    //events will always be odd indexed. Execute an event.
                    bool isOdd = i % 2 != 0;
                    if (isOdd)
                    {
                        DialogueEvents.HandleEvent(parts[i], this);
                        allCurrentlyExecutedEvents.Add(parts[i]);
                        i++;
                    }

                    string targDialogue = parts[i];

                    if (line.speaker != "narrator" && !line.speaker.Contains("*"))
                    {
                        print(line.speaker);
                        Character character = CharacterManager.instance.GetCharacter(line.speaker);
                        //This is a valid character that can show up on the screen. Get the character and make them speak.
                        //if (character != null)
                        character.Say(targDialogue, i > 0 ? true : pretext != "");
                        //This is a character that has no images to display. Only show the name and the dialogue.
                        //else
                        //DialogueSystem.instance.Say(targDialogue, line.speaker, i > 0 ? true : pretext != ""); does not work yet
                    }
                    else
                    {
                        DialogueSystem.instance.Say(targDialogue, line.speaker, i > 0 ? true : pretext != "");
                    }

                    //yield while the dialogue system's architect is constructing the dialogue.
                    architect = DialogueSystem.instance.currentArchitect; //TODO

                    while (architect.isConstructing)
                        yield return new WaitForEndOfFrame();
                }

                running = null;
            }

            public void ForceFinish()
            {
                if (running != null)
                    NovelController.instance.StopCoroutine(running);
                running = null;

                if (architect != null)
                {
                    architect.ForceFinish();

                    //time to complete the entire dialogue string since an interrupted segment with events will only autocomplete to the next event.
                    if (pretext == "")
                        line.lastSegmentsWholeDialogue = "";

                    //execute all actions that have not been made yet.
                    string[] parts = dialogue.Split('[', ']');
                    for (int i = 0; i < parts.Length; i++)
                    {
                        bool isOdd = i % 2 != 0;
                        if (isOdd)
                        {
                            string e = parts[i];
                            if (allCurrentlyExecutedEvents.Contains(e))
                            {
                                allCurrentlyExecutedEvents.Remove(e);
                            }
                            else
                            {
                                DialogueEvents.HandleEvent(e, this);
                            }
                            i++;
                        }
                        //append only the dialogue to the whole dialogue and make the architect show it.
                        line.lastSegmentsWholeDialogue += parts[i];
                    }

                    //show the whole dialogue on the architect.
                    architect.ShowText(line.lastSegmentsWholeDialogue);
                }
            }
        }
    }
}
