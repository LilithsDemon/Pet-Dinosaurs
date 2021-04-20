using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class mainMenu : MonoBehaviour
{
   
    void Start()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.None;
    }

    public void load_park()
    {
        SaveManager.Instance.set_scene_to_load(2);
        SceneManager.LoadScene(1);
    }

    public void load_encyclopedia()
    {
        SceneManager.LoadScene(5);
    }

}
