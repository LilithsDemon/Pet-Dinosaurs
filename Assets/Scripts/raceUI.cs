using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class raceUI : MonoBehaviour
{
    public Text lapNum;
    public Text lapTime;
    public Text countDown;

    public float milli;
    public int sec;
    public int min;

    public string millidisplay;
    public string secdisplay;
    public string mindisplay;

    public int lap = 0;
    public string[] timeStore;

    public bool start = false;

    public bool check1 = false;
    public bool check2 = false;

    public GameObject endPanel;
    public Text winnerText;
    public Text lap1Time;
    public Text lap2Time;



    public IEnumerator countdown() 
    {
        Time.timeScale = 0.00001f;
        countDown.text = "3";
        yield return new WaitForSeconds(0.000005f);
        countDown.text = "2";
        yield return new WaitForSeconds(0.000005f);
        countDown.text = "1";
        yield return new WaitForSeconds(0.000005f);
        countDown.text = "";
        countDown.enabled = false;
        start = true;
        FindObjectOfType<marker>().AIrun = true;
        Time.timeScale = 1f;
       
    }

    private void Start() 
    {
        if (enabled) 
        {
            endPanel.SetActive(false);
            lapNum.text = "Lap: " + (lap + 1).ToString() + "/2";
            StartCoroutine(countdown());
        }
        
    }

    private void Update() {
        if (start) {
            milli += Time.deltaTime * 10;
        }
        if(milli >= 10) {
            milli = 0;
            sec += 1;
        }
        if (sec >= 60) {
            sec = 0;
            min += 1;
        }
        millidisplay = "0" + milli.ToString("F1");
        
        if (sec < 10) {
            secdisplay = "0" + sec.ToString();
        } else {
            secdisplay = sec.ToString();
        }

        if (min < 10) {
            mindisplay = "0" + min.ToString();
        } else {
            mindisplay = min.ToString();
        }
        lapTime.text = " " + mindisplay + ":" + secdisplay + ":" + millidisplay;
    }

    private void OnTriggerEnter(Collider colliderInfo) 
    {
        if(enabled)
        {
            if (colliderInfo.gameObject.name == "Check1") 
            {
                check1 = true;
            }
            if (colliderInfo.gameObject.name == "Check2") 
            {
                check2 = true;
            }
            if(colliderInfo.gameObject.name == "FinishLine" && check1 == true && check2 == true) 
            {
                timeStore[lap] = " " + mindisplay + ":" + secdisplay + ":" + millidisplay;
                milli = 0;
                min = 0;
                sec = 0;
                check1 = false;
                check2 = false;
                lap++;
                if (lap == 2) {
                    end_race(1);
                } else 
                {
                    lapNum.text = "Lap: " + (lap+1).ToString() + "/2";
                }
                
            }
        }
        
    }

    public void end_race(int winner) 
    {
        Time.timeScale = 0.00001f;
        if (winner == 1) {
            winnerText.text = "You won!";
            SaveManager.Instance.add_coins(1);
        } else {
            winnerText.text = "You Lost!";
        }
        lap1Time.text = timeStore[0];
        lap2Time.text = timeStore[1];
        endPanel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
    }
}