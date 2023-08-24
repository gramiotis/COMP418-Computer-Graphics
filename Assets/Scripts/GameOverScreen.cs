using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverScreen : MonoBehaviour
{
    public void Setup()
    {
        gameObject.transform.GetComponentInChildren<TextMeshProUGUI>().text = "GAME OVER";

        gameObject.SetActive(true);
    }   

    public void NextLevelButton()
    {
        char name = SceneManager.GetActiveScene().name[SceneManager.GetActiveScene().name.Length - 1];
        int level = name - '0';

        if(level<3)
            SceneManager.LoadScene("Level" + ++level);
    }

    public void MainMenuButton()
    {
        SceneManager.LoadScene("StartMenu");
    }
}
