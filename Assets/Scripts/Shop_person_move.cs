using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop_person_move : MonoBehaviour
{

    public GameObject player;
    public float turnRate;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 targetDelta = player.transform.position - transform.position;
        float angleToTarget = Vector3.Angle(transform.forward, targetDelta);
        Vector3 turnAxis = Vector3.Cross(transform.forward, targetDelta);

        transform.RotateAround(transform.position, turnAxis, Time.deltaTime * turnRate * angleToTarget);
    }
}
