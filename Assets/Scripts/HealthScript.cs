using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HealthScript : MonoBehaviour {

    public Image currentHealthBar;
    public Text HPText;

    private float health = 100;
    private float maxHP = 100;

    private void Start()
    {
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        float ratio = health / maxHP;
        currentHealthBar.rectTransform.localScale = new Vector3(ratio, 1, 1); //Si la barre de vie est utilisée
        HPText.text = (ratio * 100).ToString("0")+'%';
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if(health < 0)
        {
            health = 0;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        UpdateHealthBar();
    }

    private void HealDamage(float heal)
    {
        health += heal;
        if (health > maxHP)
        {
            health = maxHP;
            Debug.Log("FullHealth!");
        }

        UpdateHealthBar();
    }
}
