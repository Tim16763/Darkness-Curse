using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Start_script : MonoBehaviour
{
    // Start is called before the first frame update
    public void Play()
    {
        SceneManager.UnloadScene(0);
        SceneManager.LoadScene(1);
    }
    public void ToMenu()
    {
        SceneManager.UnloadScene(1);
        SceneManager.LoadScene(0);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
