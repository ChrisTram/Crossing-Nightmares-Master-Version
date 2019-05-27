using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{

    public static UIManager instance;
    public GameObject UITitle;
    public GameObject UIItemInfo;
    /*
    public TMP_Text UITitleText;
    public TMPro UIItemInfoText;
    */
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

    }

    public void showTitle()
    {
        UITitle.SetActive(true);
        StartCoroutine(RemoveAfterSeconds(3));

    }
    IEnumerator RemoveAfterSeconds(int seconds)
    {
        yield return new WaitForSeconds(seconds);
        UITitle.SetActive(false);
    }

    public void showItemInfo(string itemName)
    {
        UIItemInfo.GetComponentInChildren<TMP_Text>().text = itemName;
        //UIItemInfo.GetComponent<LocalizedText>().UpdateText();
        UIItemInfo.SetActive(true);
    }

    public void disableItemInfo()
    {
        UIItemInfo.SetActive(false);
    }
}
        /*
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            UITitle.SetActive(true);
        }
    }*/

