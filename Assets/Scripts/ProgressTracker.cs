using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ProgressTracker : MonoBehaviour
{
    // Check how many enemies are left in the level
    public void checkProgress()
    {
        if (GameObject.FindWithTag("Enemy") == null)
        {
            SceneManager.LoadScene(2, LoadSceneMode.Additive);
            SceneManager.LoadScene(3, LoadSceneMode.Additive);
        }
    }
}
