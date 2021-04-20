using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class objectsNearPlayer : MonoBehaviour
{

    public List<GameObject> find_objects_near_player()
    {
        Collider[] hitColliders = Physics.OverlapSphere(this.transform.position, 5);
        List<GameObject> items = new List<GameObject>();
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.gameObject.tag == "Dinosaur" || hitCollider.gameObject.tag == "Shop" || hitCollider.gameObject.tag == "Crop")
            {
                items.Add(hitCollider.gameObject);
            }
        }
        return items;
    }

    public void utilise_button_press()
    {
        List<GameObject> items = new List<GameObject>();
        items = find_objects_near_player();

        FindObjectOfType<playerUI>().button_press(items[0]);
    }

    public void Update()
    {
        Collider[] hitColliders = Physics.OverlapSphere(this.transform.position, 5);
        List<GameObject> items = new List<GameObject>();
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.gameObject.tag == "Dinosaur" || hitCollider.gameObject.tag == "Shop" || hitCollider.gameObject.tag == "Crop")
            {
                items.Add(hitCollider.gameObject);
            }
        }

        if (items.Count > 0)
        {
            if (items[0].tag != "Crop")
            {
                FindObjectOfType<playerUI>().turn_on_button(items[0].name, items[0]);
            } 
            else {
                if (SaveManager.Instance.current_held() == 0 || SaveManager.Instance.current_held() == 2)
                {
                    FindObjectOfType<playerUI>().turn_on_button("Pepper", items[0]);
                } 
                else if (SaveManager.Instance.current_held() == 1 || SaveManager.Instance.current_held() == 3) 
                {
                    FindObjectOfType<playerUI>().turn_on_button("Mango", items[0]);
                }
            }
        } 
        else {
            FindObjectOfType<playerUI>().turn_off_button();
        }
    }
}
