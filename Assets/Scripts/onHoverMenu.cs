using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class onHoverMenu : MonoBehaviour
{

    public Animator onHover;

    public void PointerEnter()
    {
        onHover.SetTrigger("Hover");
        Debug.Log("Mouse is over");
    }

    public void PointerExit()
    {
        onHover.SetTrigger("StopHover");
        Debug.Log("Exit Hover");
    }
}
