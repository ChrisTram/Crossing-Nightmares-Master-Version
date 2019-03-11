using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartupManager : MonoBehaviour
{
    private int level_index;
    // Use this for initialization
    private IEnumerator Start()
    {
        while (!LocalizationManager.instance.GetIsReady())
        {
            yield return null;
        }
        //TODO Voir si j'en ai vraiment besoin
        //level_index = PlayerPrefs.GetInt("Last_Level");
        //SceneManager.LoadScene(level_index);
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}