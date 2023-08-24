using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using TMPro;

public class StorytellerLogic : MonoBehaviour
{
    public TextMeshProUGUI dialogue_box;

    // Start is called before the first frame update
    void Start()
    {
        char lvl = SceneManager.GetActiveScene().name[SceneManager.GetActiveScene().name.Length - 1];
        int level = lvl - '0';
        StartCoroutine("StartLevelDialogue", level);
    }

    // Update is called once per frame
    void Update()
    {

    }

    // an intro to the current level
    IEnumerator StartLevelDialogue(int level)
    {
        Camera.main.GetComponent<CameraLogic>().enabled = false;

        if (level == 1)
        {
            yield return new WaitForSeconds(2);
            dialogue_box.text = "Village's Chieftess - Welcome soldier to our humble village....";
            yield return new WaitForSeconds(5);
            dialogue_box.text = "....as you may be informed the orcs threaten to invade our lands....";
            yield return new WaitForSeconds(5);
            dialogue_box.text = "....as an act of revenge for the hunt of their kin our foolish king unleashed.";
            yield return new WaitForSeconds(5);
            dialogue_box.text = "Now it rests up to you to save us from the orc invasion and with the help of our angel protector we may succeed.";
            yield return new WaitForSeconds(6);
            dialogue_box.text = "First we need to build our army, gather resources from the nearest forest or pile of rocks and build 2 barracks and 1 mausoleum.";
            yield return new WaitForSeconds(6);
            dialogue_box.text = "";
        }

        if(level == 2)
        {
            dialogue_box.text = "Village's Chieftess - For the next phase we need to train some soldiers and kill all the enemies that have surrounded our village.";
            yield return new WaitForSeconds(7);
            dialogue_box.text = "";
        }

        if (level == 3)
        {
            dialogue_box.text = "Village's Chieftess - This is it, the final stride to victory. Train some soldiers and go hunt the orc boss. Remember you can power up the soldiers through the Mausoleum";
            yield return new WaitForSeconds(7);
            dialogue_box.text = "";
        }

        Camera.main.GetComponent<CameraLogic>().enabled = true;
    }
}
