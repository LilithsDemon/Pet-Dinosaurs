using System;
using System.Collections;
using UnityEngine;


public class decorationManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] placeableObjectPrefabsPreview;
    [SerializeField]
    private GameObject[] placeableObjectPrefabs;

    private GameObject currentPlaceableObject;

    private float mouseWheelRotation;
    private int currentPrefabIndex = -1;

    private bool build = false;

    public Camera cam;

    private void Update()
    {
        //HandleNewObjectHotkey();
        handlePlaceObject();

        if (currentPlaceableObject != null)
        {
            MoveCurrentObjectToMouse();
            RotateFromMouseWheel();
            //ReleaseIfClicked();
        }
    }

    /*
    private void HandleNewObjectHotkey()
    {
        for (int i = 0; i < placeableObjectPrefabs.Length; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha0 + 1 + i))
            {
                if (PressedKeyOfCurrentPrefab(i))
                {
                    Destroy(currentPlaceableObject);
                    currentPrefabIndex = -1;
                }
                else
                {
                    if (currentPlaceableObject != null)
                    {
                        Destroy(currentPlaceableObject);
                    }
                    currentPlaceableObject = Instantiate(placeableObjectPrefabsPreview[i]);
                    currentPrefabIndex = i;
                }

                break;
            }
        }
    }
    */

    public void handlePlaceObject() 
    {
        if (build == true) 
        {
            if(currentPlaceableObject != null)
            {
                Destroy(currentPlaceableObject);
            }
            currentPlaceableObject = Instantiate(placeableObjectPrefabsPreview[SaveManager.Instance.current_decoration_selected()]);
            currentPrefabIndex = SaveManager.Instance.current_decoration_selected();
        }
    }

    public void toggleBuildMode()
    {
        if (build == true) 
        {
            build = false;
        }
        else 
        {
            build = true;
        }
    }

    public void stopPlacing()
    {
        Destroy(currentPlaceableObject);
        currentPrefabIndex = -1;
        build = false;
        
    }

    public void placeObject() 
    {
        Instantiate(placeableObjectPrefabs[currentPrefabIndex], currentPlaceableObject.transform.position, currentPlaceableObject.transform.rotation);
        Destroy(currentPlaceableObject);
            
        currentPlaceableObject = null;
        FindObjectOfType<playerUI>().stop_placing_decoration();
    }

    /*
    private bool PressedKeyOfCurrentPrefab(int i)
    {
        return currentPlaceableObject != null && currentPrefabIndex == i;
    }
    */

    private void MoveCurrentObjectToMouse()
    {
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));

        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo))
        {
            currentPlaceableObject.transform.position = hitInfo.point;
            currentPlaceableObject.transform.rotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
        }
    }

    private void RotateFromMouseWheel()
    {
        mouseWheelRotation += Input.mouseScrollDelta.y * 15f;
        currentPlaceableObject.transform.Rotate(Vector3.up, mouseWheelRotation);
    }


    public void rotateLeft() 
    {
        mouseWheelRotation += 15;
    }

    public void rotateRight() 
    {
        mouseWheelRotation -= 15;
    }

    /*
    private void ReleaseIfClicked()
    {
        if (Input.GetMouseButtonDown(0))
        {
           
            Instantiate(placeableObjectPrefabs[currentPrefabIndex], currentPlaceableObject.transform.position, currentPlaceableObject.transform.rotation);
            Destroy(currentPlaceableObject);
            
            currentPlaceableObject = null;
        }
    }
    */
}