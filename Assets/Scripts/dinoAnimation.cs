using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dinoAnimation : MonoBehaviour
{
    public Animator animator;

    void Start() 
    {
        StartCoroutine(CalcVelocity());
    }

    IEnumerator CalcVelocity()
    {
        while(Application.isPlaying)
        {
            Vector3 lastPos = transform.position;
            yield return new WaitForFixedUpdate();
            int movesSpeed = Mathf.RoundToInt(Vector3.Distance(transform.position, lastPos)/Time.fixedDeltaTime);
            animator.speed = movesSpeed/8;
        }
     }
}
