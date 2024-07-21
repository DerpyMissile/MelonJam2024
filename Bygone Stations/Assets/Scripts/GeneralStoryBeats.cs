using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class GeneralStoryBeats : MonoBehaviour
{
    public DialogueRunner dialoguerunner;
    public GameObject player;
    private string sceneName;
    // Start is called before the first frame update
    void Start()
    {
        dialoguerunner.onDialogueComplete.AddListener(givePermsBack);
        player.GetComponent<PlayerInput>().enabled = false;
        sceneName = SceneManager.GetActiveScene().name;
        dialoguerunner.StartDialogue(sceneName);
        // Time.timeScale = 0;
    }

    private void givePermsBack(){
        player.GetComponent<PlayerInput>().enabled = true;
        // Time.timeScale = 1;
    }

    public void StartDialogue(string sceneName){
        player.GetComponent<PlayerInput>().enabled = false;
        dialoguerunner.StartDialogue(sceneName);
    }
}
