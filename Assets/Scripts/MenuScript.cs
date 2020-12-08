using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    public void Play()
    {
        SceneManager.LoadScene(1);
    }

    public void Options()
    {

    }

    public void Credits()
    {

    }

    public void mainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void Quit()
    {

    }
}
