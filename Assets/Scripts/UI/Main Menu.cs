using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void OnPlayButtonPressed()
    {
        FindObjectOfType<SceneTransitioner>().TransitionScene("Level One");
    }
    public void OnQuitButtonPressed()
    {
        Application.Quit();
    }
}
