using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class introScript : MonoBehaviour
{

    public GameObject cam;
    public float speed = 100;
    int current_position = 0;
    bool move = false;

    public void move_camera()
    {
        if (((int)cam.transform.position.x + 25) % 25 == 0)
        {
            if (current_position < 100)
            {
                current_position += 25;
                move = true;
            } else
            {
                SceneManager.LoadScene(0);
            }
            
        }
        
    }

    public void FixedUpdate()
    {
        if (move == true)
        {
            if (!((int)cam.transform.position.x == current_position))
            {
                Vector3 new_position = new Vector3(current_position, cam.transform.position.y, cam.transform.position.z);
                cam.transform.position = Vector3.MoveTowards(cam.transform.position, new_position, speed * Time.deltaTime);
            } else
            {
                move = false;
            }
        }
        
    }
}
