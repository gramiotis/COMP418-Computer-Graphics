using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

public class GameMaster : MonoBehaviour
{
    public GameObject House;
    public GameObject Barracks;
    public GameObject Mausoleum;
    public GameObject pausePanel;
    public GameObject builder; // human builder
    public GameObject lineRenderer;
    public GameObject credits;
    public GameObject endingPanel;

    public bool buildClicked; // true if the build button was clicked

    public static int wood;
    public static int stone;
    public static int population;
    public static float gold;

    public static int woodInterval = 5;
    public static int stoneInterval = 5;
    public static int populationInterval = 10;
    public static float goldInterval = 10f;

    public TextMeshProUGUI textElement; // resource text
    public TextMeshProUGUI objectiveName;
    public TextMeshProUGUI gameMessageText;
    public Button buildButton;
    public TMP_Dropdown dropdownMenu;

    public Camera cam;
    public LayerMask Ground;

    public string currentScene;
    public GameOverScreen gameOverScreen;
    public NavMeshSurface surface;

    public static GameMaster _instance;
    public static GameMaster Instance { get { return _instance; } }

    void Awake()
    {
        _instance = this;
        population = 0;
        wood = 0;
        stone = 0;
        gold = 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;

        buildClicked = false;

        dropdownMenu.options.Clear();
        dropdownMenu.options.Add(new TMP_Dropdown.OptionData() { text = "House" });
        dropdownMenu.options.Add(new TMP_Dropdown.OptionData() { text = "Barracks" });
        dropdownMenu.options.Add(new TMP_Dropdown.OptionData() { text = "Mausoleum" });

        WriteObjective(SceneManager.GetActiveScene().name);
    }

    // Update is called once per frame
    void Update()
    {
        textElement.text = "= " + wood + "\n\n= " + stone + "\n\n= " + population + "\n\n= " + gold;

        StartCoroutine(CheckGameOver());

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }


        if (buildClicked)
        {
            Debug.Log(dropdownMenu.options[dropdownMenu.value].text);
            Vector3 mousePos = Input.mousePosition;
            mousePos.z += 20.0f;       // we want 20m away from the camera position
            Vector3 objectPos = cam.ScreenToWorldPoint(mousePos);

            // draw sample line corresponding to building's dimensions
            lineRenderer.GetComponent<BuildingLineRenderer>().Draw(objectPos, dropdownMenu.options[dropdownMenu.value].text);

            // left mouse click to build a building
            if (Input.GetMouseButtonDown(0))
            {
                if (dropdownMenu.options[dropdownMenu.value].text == "House" && wood >= 50)
                {
                    Vector3 HouseBox = House.GetComponent<BoxCollider>().size * 0.5f; // halfextents for overlapbox
                    Collider[] cols = Physics.OverlapBox(objectPos, HouseBox);

                    int buildingsFound = 0;
                    foreach (var col in cols)
                    {
                        if (col.tag == "House" || col.tag == "Barracks" || col.tag == "Mausoleum" || col.tag == "HealingStatue")
                            buildingsFound += 1;
                    }

                    if (buildingsFound == 0)
                    {
                        objectPos.y = 2f;
                        builder.GetComponent<HumanBuilderLogic>().AddToQueue(objectPos, House); // add to building queue
                        GameMaster.wood -= 50;
                    }
                    else
                        StartCoroutine(GameMessage("Cannot Build Here"));
                }
                else if (dropdownMenu.options[dropdownMenu.value].text == "House")
                {
                    StartCoroutine(GameMessage("You require 50 wood for a House"));
                }

                if (dropdownMenu.options[dropdownMenu.value].text == "Barracks" && wood >= 50 && stone >= 30)
                {
                    Vector3 BarrackBox = Barracks.GetComponent<BoxCollider>().size * 0.5f;
                    Collider[] cols = Physics.OverlapBox(objectPos, BarrackBox);

                    int buildingsFound = 0;
                    foreach(var col in cols)
                    {
                        if (col.tag == "House" || col.tag == "Barracks" || col.tag == "Mausoleum" || col.tag == "HealingStatue")
                            buildingsFound += 1;
                    }

                    if (buildingsFound == 0)
                    {
                        objectPos.y = -0.02304095f;
                        builder.GetComponent<HumanBuilderLogic>().AddToQueue(objectPos, Barracks);
                        GameMaster.wood -= 50;
                        GameMaster.stone -= 30;
                    }
                    else
                        StartCoroutine(GameMessage("Cannot Build Here"));
                }
                else if (dropdownMenu.options[dropdownMenu.value].text == "Barracks")
                {
                    StartCoroutine(GameMessage("You require 50 wood and 30 stone for a Barrack"));
                }

                if (dropdownMenu.options[dropdownMenu.value].text == "Mausoleum" && stone >= 50)
                {
                    Vector3 MausoleumBox = Mausoleum.GetComponent<BoxCollider>().size * 0.5f;
                    Collider[] cols = Physics.OverlapBox(objectPos, MausoleumBox);

                    int buildingsFound = 0;
                    foreach (var col in cols)
                    {
                        if (col.tag == "House" || col.tag == "Barracks" || col.tag == "Mausoleum" || col.tag == "HealingStatue")
                            buildingsFound += 1;
                    }
                    if (buildingsFound == 0)
                    {
                        objectPos.y = 3.251616f;
                        builder.GetComponent<HumanBuilderLogic>().AddToQueue(objectPos, Mausoleum);
                        GameMaster.stone -= 50;
                    }
                    else
                        StartCoroutine(GameMessage("Cannot Build Here"));
                }
                else if (dropdownMenu.options[dropdownMenu.value].text == "Mausoleum")
                {
                    StartCoroutine(GameMessage("You require 50 stone for a Mausoleum"));
                }

                buildClicked = false; // after building make the bool for the button false
            }
            else if (Input.GetKeyDown(KeyCode.Mouse2)) // pressed mouse wheel to cancel building
            {
                buildClicked = false;
            }
        }
        else
        {
            lineRenderer.GetComponent<BuildingLineRenderer>().ClearLines();
        }
    }

    // build button was pressed
    void BuildButtonClick()
    {
        buildClicked = true;
    }

    // objective text based on level
    void WriteObjective(string name)
    {

        if (name == "Level1")
        {
            objectiveName.text = "Main Objective: Gather wood and stone and build 2 barracks and 1 mausoleum";
        }

        if (name == "Level2")
        {
            objectiveName.text = "Main Objective: Build some soldiers and kill all the enemys";
        }

        if (name == "Level3")
        {

            objectiveName.text = "Main Objective: Build your army and Kill the final Boss";
        }
    }

    // game notifications
    public IEnumerator GameMessage(string message)
    {
        gameMessageText.text = message;
        yield return new WaitForSeconds(4f);
        gameMessageText.text = "";
    }

    // check if level requirements were complete
    IEnumerator CheckGameOver()
    {
        if (SceneManager.GetActiveScene().name == "Level1")
        {
            if (GameObject.FindGameObjectsWithTag("Barracks").Length >= 2 && GameObject.FindGameObjectsWithTag("Mausoleum").Length >= 1)
            {
                gameOverScreen.Setup();
            }
        }
        else if(SceneManager.GetActiveScene().name == "Level2")
        {
            if (GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
            {
                gameOverScreen.Setup();
            }
        }
        else if(SceneManager.GetActiveScene().name == "Level3")
        {
            if (GameObject.FindGameObjectWithTag("Boss") == null)
            {
                yield return new WaitForSeconds(3);

                endingPanel.SetActive(true);
            }
        }
    }

    public void PauseGame()
    {
        if (Time.timeScale != 0)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }

        pausePanel.SetActive(!pausePanel.activeSelf);
    }
    public void RestartButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void MainMenuButton()
    {
        SceneManager.LoadScene("StartMenu");
    }

    public void UpdateSurface()
    {
        surface.BuildNavMesh();
    }
}
