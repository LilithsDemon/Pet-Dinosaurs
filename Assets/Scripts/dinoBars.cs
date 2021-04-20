using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class dinoBars : MonoBehaviour
{

    public GameObject self;

    public GameObject player;

    public Text nameLabel;


    // Update is called once per frame
    void Update()
    {

        Vector3 namePose = Camera.main.WorldToScreenPoint(this.transform.position);
        nameLabel.transform.position = namePose;

        bool in_list = false;

        Collider[] hitColliders = Physics.OverlapSphere(player.transform.position, 10);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.gameObject == self)
            {
                in_list = true;
                
            }
        }

        if (in_list) {
            nameLabel.gameObject.SetActive(true);
        } 
        else {
            nameLabel.gameObject.SetActive(false);
        }
        
    }
}