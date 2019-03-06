using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartupManager : MonoBehaviour
{

    // Use this for initialization
    private IEnumerator Start()
    {
        while (!LocalizationManager.instance.GetIsReady())
        {
            yield return null;
        }
        //TODO Voir si j'en ai vraiment besoin
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}