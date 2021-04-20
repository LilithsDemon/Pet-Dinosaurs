using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class foodGrowth : MonoBehaviour
{
    public float maxSize;
    private float growTime = 45;
    private float timer;

    void Start()
    {
        
    }

    public void reset()
    {
        transform.localScale -= new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }


    void Update()
    {
        if (maxSize > transform.localScale.x)
        {
            timer += Time.deltaTime;
            transform.localScale += new Vector3(1, 1, 50) * Time.deltaTime * (maxSize/growTime) * (1 - ((SaveManager.Instance.grow_time_level() *5) / 100 ));
        }
    }
}