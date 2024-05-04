using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            if(SceneManager.GetActiveScene().name == "Level One")
            {
                FindObjectOfType<SceneTransitioner>().TransitionScene("Level Two");
            }
            if (SceneManager.GetActiveScene().name == "Level Two")
            {
                FindObjectOfType<SceneTransitioner>().TransitionScene("Level Three");
            }
            if (SceneManager.GetActiveScene().name == "Level Three")
            {
                FindObjectOfType<SceneTransitioner>().TransitionScene("Win");
            }
        }
    }
}
