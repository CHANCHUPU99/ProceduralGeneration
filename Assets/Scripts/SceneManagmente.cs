using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagmente : MonoBehaviour
{
    [SerializeField] string gameplayScene = "Gameplay";
    [SerializeField] string mainMenuScene = "MainMenu";

    public void reloadScene() {
        SceneManager.LoadScene(gameplayScene);
    }

    public void loadMainMenuScene() {
        SceneManager.LoadScene(mainMenuScene);
    }
}
