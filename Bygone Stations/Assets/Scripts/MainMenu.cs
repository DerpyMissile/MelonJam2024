using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public Button startGame;
    public Image screen;

    public void Start(){
        Color temp = screen.color;
        temp.a = 1.0f;
        screen.color = temp;
        startGame.onClick.AddListener(OnClick);
    }

    public void OnClick(){
        StartCoroutine(switchScene());
        PlayerStats.RestoreHp();
        PlayerStats.RestoreFlow();
    }

    IEnumerator switchScene()
    {
        yield return new WaitForSeconds(0f);
        // fade to black
        float fadeDuration = 1.0f;
        float elapsedTime = 0.0f;
        Color temp = screen.color;

        while (elapsedTime < fadeDuration)
        {
            yield return null;
            elapsedTime += Time.deltaTime;
            temp.a = Mathf.Lerp(1.0f, 0.0f, elapsedTime / fadeDuration);
            screen.color = temp;
        }

        temp.a = 0.0f;
        screen.color = temp;

        // switch scene
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Station-1");

        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        SceneManager.UnloadSceneAsync("MainMenu");
    }
}
