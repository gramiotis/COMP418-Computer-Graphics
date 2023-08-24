using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System.Text.RegularExpressions;

public class UIManager : MonoBehaviour
{
    public GameObject uiPanel;
    public GameObject creditsPanel;
    public GameObject mapSelectPanel;
    public GameObject gameManualPanel;

    public void Button_StartGame()
    {
        SceneManager.LoadScene("Level1");
    }

    public void Button_Credits()
    {
        uiPanel.SetActive(!uiPanel.activeSelf);
        creditsPanel.SetActive(!creditsPanel.activeSelf);

    }

    public void Button_Game_Manual()
    {
        uiPanel.SetActive(!uiPanel.activeSelf);
        gameManualPanel.SetActive(!gameManualPanel.activeSelf);

    }

    public void Select_Map()
    {
        uiPanel.SetActive(!uiPanel.activeSelf);
        mapSelectPanel.SetActive(!mapSelectPanel.activeSelf);
        mapSelectPanel.transform.GetChild(3).gameObject.SetActive(!mapSelectPanel.transform.GetChild(3).gameObject.activeSelf);
    }

    public void loadLevel()
    {
        string buttonName = EventSystem.current.currentSelectedGameObject.name;
        int level = int.Parse(Regex.Match(buttonName, @"\d+").Value);
        SceneManager.LoadScene("Level" + level);
    }

    public void Button_QuitGame()
    {
        Application.Quit();
    }
}
