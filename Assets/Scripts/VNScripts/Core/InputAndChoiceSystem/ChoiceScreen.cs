using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChoiceScreen : MonoBehaviour {

    public static ChoiceScreen instance;

    public GameObject root;

    public TitleHeader _header;

    public static TitleHeader header { get { return instance._header; } }

    public ChoiceButton choicePrefab;

    static List<ChoiceButton> choices = new List<ChoiceButton>();

    public VerticalLayoutGroup LayoutGroup;

    private void Awake()
    {
        instance = this;
        Hide();
    }

    public static void Show(string title,params string[] choices)
    {
        instance.root.SetActive(true);

        if (title != "")
            header.Show(title);
        else
            header.Hide();

        if (isShowingChoices)
            instance.StopCoroutine(showingChoices);

        ClearAllCurrentChoices();

        showingChoices = instance.StartCoroutine(ShowingChoices(choices));
    }

    static void ClearAllCurrentChoices()
    {
        foreach(ChoiceButton b in choices)
        {
            DestroyImmediate(b.gameObject);
        }
        choices.Clear();
    }

    public static void Hide()
    {
        if (isShowingChoices)
            instance.StopCoroutine(showingChoices);
        showingChoices = null;

        header.Hide();

        ClearAllCurrentChoices();

        instance.root.SetActive(false);
    }


    public static bool isWaitingForChoiceToBeMade { get { return isShowingChoices && !lastChoiceMade.hasBeenMade; } }
    public static bool isShowingChoices {  get { return showingChoices != null; } }
    static Coroutine showingChoices = null;
    public static IEnumerator ShowingChoices(string[] choices)
    {
        yield return new WaitForEndOfFrame();//Waiting the header to begin appearing
        lastChoiceMade.Reset();

        while(header.isRevealing)
            yield return new WaitForEndOfFrame();

        for(int i = 0; i < choices.Length; i++)
        {
            CreateChoice(choices[i]);
        }

        SetLayoutSpacing();

        while (isWaitingForChoiceToBeMade)
            yield return new WaitForEndOfFrame();

        Hide();
    }

    static void SetLayoutSpacing()
    {
        int i = choices.Count;
        if (i <= 3)
            instance.LayoutGroup.spacing = 20;
        else if (i >= 7)
            instance.LayoutGroup.spacing = 1;
        else
        {
            switch(i)
            {
                case 4:
                    instance.LayoutGroup.spacing = 15;
                    break;
                case 5:
                    instance.LayoutGroup.spacing = 10;
                    break;
                case 6:
                    instance.LayoutGroup.spacing = 5;
                    break;

            }
        }
    }

    static void CreateChoice(string choice)
    {
        GameObject ob = Instantiate(instance.choicePrefab.gameObject, instance.choicePrefab.transform.parent);
        ob.SetActive(true);
        ChoiceButton b = ob.GetComponent<ChoiceButton>();

        b.text = choice;
        b.choiceIndex = choices.Count;

        choices.Add(b);
    }

    [System.Serializable]
    public class CHOICE
    {
        public bool hasBeenMade { get { return title != "" && index != -1; } }
        public string title = "";
        public int index = -1;

        public void Reset()
        {
            title = "";
            index = -1;
        }
    }
    public CHOICE choice = new CHOICE();
    public static CHOICE lastChoiceMade { get { return instance.choice; } }

    public void MakeChoice (ChoiceButton button)
    {
        choice.index = button.choiceIndex;
        choice.title = button.text;
    }

    //Can be use ?
    /* 
    public void MakeChoice(string choiceTitle)
    {
        foreach (ChoiceButton b in choices)
        {
            if (b.text.ToLower() == choiceTitle.ToLower())
            {
                MakeChoice(b);
                return;
            }
        }
    }

    public void MakeChoice(int choiceIndex)
    {
        if (choices.Count > choiceIndex)
            MakeChoice(choices[choiceIndex]);
    }*/
}
