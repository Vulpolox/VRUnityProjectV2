using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelector : MonoBehaviour
{
    public string levelToLoad;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Entered");
        if (other.CompareTag("Player"))
        {
            SceneManager.LoadScene(levelToLoad, LoadSceneMode.Single);
        }
    }
}
