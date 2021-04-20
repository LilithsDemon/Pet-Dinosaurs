using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class raceGameManager : MonoBehaviour
{
    public GameObject[] dinos;

    public GameObject[] enemyDinos;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1f;
        int i = 0;
        int index = 0;
        Cursor.lockState = CursorLockMode.Locked;
        //FindObjectOfType<playerUI>().dino_start_mats(dinos);

        foreach (GameObject dino in dinos) 
        {
            if (i == SaveManager.Instance.current_dino_to_play_with()) { 
                dino.SetActive(true);
                dino.transform.position = new Vector3(640,0,259);
                index = i;
            } else {
                dino.SetActive(false);
                enemyDinos[index] = dino;
                index++;
                dino.GetComponent<dinoRacePhoneMove>().enabled = false;
                dino.GetComponent<playerMove>().enabled = false;
                dino.GetComponent<raceUI>().enabled = false;
                dino.GetComponent<raceUI>().enabled = false;
                dino.GetComponent<CharacterController>().enabled = false;
                dino.transform.Find("Camera").gameObject.SetActive(false);
            }
            i++;
        }

        System.Random rnd = new System.Random();
        int enemyDino = rnd.Next(0, enemyDinos.Length);
        FindObjectOfType<marker>().dino = enemyDinos[enemyDino];
        enemyDinos[enemyDino].SetActive(true);
        enemyDinos[enemyDino].transform.position = new Vector3(647, 0, 260);
        enemyDinos[enemyDino].tag = "Enemy";
        foreach(Transform child in enemyDinos[enemyDino].transform) 
        {
            child.tag = "Enemy";
        }

    }

    public void go_home()
    {
        SaveManager.Instance.set_scene_to_load(2);
        SceneManager.LoadScene(1);
    }

    public void restart() 
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
