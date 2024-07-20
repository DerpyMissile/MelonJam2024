using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class SceneTransition : MonoBehaviour
{
    private Scene scene;
    public Image screen;
    public GameObject exit;

    public void Start(){
        scene = SceneManager.GetActiveScene();
        Color temp = screen.color;
        // temp.a = 0.0f;
        float fadeDuration = 1.0f;
        float elapsedTime = 0.0f;
        screen.color = temp;

        while (elapsedTime < fadeDuration){
            elapsedTime += Time.deltaTime;
            temp.a = Mathf.Lerp(1.0f, 0.0f, elapsedTime / fadeDuration);
            screen.color = temp;
        }
    }

    public void GetScene(InputAction.CallbackContext context){
        if(context.performed && PlayerStats.touchingExit){
            StartCoroutine(switchScene(scene.name));
        }
    }

    IEnumerator switchScene(string whichScene){
        yield return new WaitForSeconds(0f);
        // fade to black
        float fadeDuration = 1.0f;
        float elapsedTime = 0.0f;
        Color temp = screen.color;

        while (elapsedTime < fadeDuration)
        {
            yield return null;
            elapsedTime += Time.deltaTime;
            temp.a = Mathf.Lerp(0.0f, 1.0f, elapsedTime / fadeDuration);
            screen.color = temp;
        }

        // temp.a = 0.0f;
        // screen.color = temp;

        // switch scene
        string nextScene = "";
        switch(whichScene){
            case "":
                break;
            case "Station-1":
                nextScene = "Puzzle-1";
                break;
            case "Puzzle-1":
                nextScene = "Boss-1";
                break;
            case "Boss-1":
                nextScene = "Win";
                break;
        }
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(nextScene);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        SceneManager.UnloadSceneAsync(whichScene);
    }
}
