using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class marker : MonoBehaviour
{
   public Transform[] mark;
   public GameObject markerContainer;
   public int markCount;
   public bool AIrun = false;
   private int lapCount;
   public GameObject dino;
   public int speed;
   public float turnRate;

    private void Start() 
    {
        int i = 0;
        foreach(Transform child in markerContainer.transform) 
        {
            mark[i] = child;
            i++;
        }
    }

    private void Update() 
    {
        if(AIrun) 
        {
            gameObject.transform.position = mark[markCount].transform.position;
            dino.transform.position = Vector3.MoveTowards(dino.transform.position, gameObject.transform.position, Time.deltaTime * speed);
            Vector3 targetDirection = gameObject.transform.position - dino.transform.position;
            Vector3 newDirection = Vector3.RotateTowards(dino.transform.forward, targetDirection, Time.deltaTime * turnRate, 0.0f);
            dino.transform.rotation = Quaternion.LookRotation(newDirection);
        }    
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Enemy") 
        {
            if(markCount == mark.Length - 1) 
            {
                markCount = 0;
                lapCount++;
                if(lapCount == 2) 
                {
                    FindObjectOfType<raceUI>().end_race(0);
                }
            }else 
            {
                markCount += 1;
            }
        }
    }

}
