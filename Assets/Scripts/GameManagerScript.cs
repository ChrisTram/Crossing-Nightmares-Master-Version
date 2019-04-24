using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour {

    public static GameManagerScript instance;
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
    public void LoadNextScene()
    {
        Debug.Log("Load Next Scene");
        PlayerPrefs.SetInt("Last_Level", SceneManager.GetActiveScene().buildIndex + 1); //J'enregistre le niveau sur 
                                                                                        //lequel le joueur pourra reload
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

    }
}
