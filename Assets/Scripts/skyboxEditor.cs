using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class skyboxEditor : MonoBehaviour
{
    public List<Material> skyboxes;

    public void change_sky_box(int index)
    {
        RenderSettings.skybox = skyboxes[index];
    }
}
