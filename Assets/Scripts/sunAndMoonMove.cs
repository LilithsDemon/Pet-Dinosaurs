using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sunAndMoonMove : MonoBehaviour
{

    //To calculate the amount of time per day do 360/number of seconds per day
    public float timePerDay = (float)(360f/300f); //5 minutes per day

    public int lastHour = 12;
    public int hour = 12;
    public int minute;

    //in 360 degress there are 1,440 minutes that means each 0.25 degrees in a minute each 7.5 degress degress is half an hour and 15 degress per hour
    
    //Rotation --
    //0 - 180 degrees = 12PM - 12AM
    //-180 - 0 = 12AM - 12PM

    
    void Start()
    {
        transform.rotation = Quaternion.Euler(SaveManager.Instance.time_rotation(), 0, 0);
    }

    void Update()
    {
        transform.RotateAround(Vector3.zero, Vector3.right,timePerDay*Time.deltaTime);
        transform.LookAt(Vector3.zero);
        float rotation = transform.rotation.eulerAngles.x;
        hour = ((int)rotation / 15);
        if (lastHour != hour)
        {
            SaveManager.Instance.set_time_rotation((int)(rotation));
            lastHour = hour;
            Debug.Log(transform.rotation.eulerAngles.x);
        }
        if (rotation % 15 > 7)
        {
            minute = 30;
        } else {
            minute = 0;
        }
    }
}
