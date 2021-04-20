using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Encyclopedia : MonoBehaviour
{

    public Image dinosaur;
    public Text dinoName;
    public Text fictionText;
    public Text habitatInfo;
    public Text generalInfo;
    public GameObject dinoButtonContainer;
    public Animator animation;

    public GameObject dinoSelection;
    public GameObject dinoInfo;

    public Sprite[] dinoImages;

    public string[] dinoNames = {"Velociraptor", "Saltiovenator", "Triceratops", "Cuteasaur", "Spikeator", "T-rex", "Dragon", "Alien Beast"};

    public int[] fiction = {0, 0, 0, 1, 1, 0, 1, 1};

    public string[] habitat = {"Desert", "Beaches", "Dry forests", "Vivid forests", "Beaches, dry forests and grasslands", "Grass lands and forests", "Rocky areas", "Vivid and light places"};

    public string[] general = {"The velociraptor was 1.8 meters long wighing 25kg.\nVelociraptors use to work toegther to get food.\nThey were very smart!",
                             "The Saltiovenator was 7.5 meter long and weighed 1,000 kg!\nThey lived approximetely 198 million years ago.",
                             "The trieratops is famous for it's horns whcih it used for defence.\nThey weighed the same as a truck!\nIt had extremely thick skin to protect itself.",
                             "The cuteasaur is a really small dinosaur that loves everything.\nIt likes to play in flowers and look at pretty colours.",
                             "The Spikeator was quite a small dinosaur that uses tress to sratch itself.\nIt has feet suitable for multiple different terrains.",
                             "The T-rex can run very fast at 20mph.\nThey like to hide in bushes and in forests\nThey had quite a large brain",
                             "The dragon likes to be isolated living on cliffs/nIt does not breath fire but loves to sleep\nVery attracted to shiny objects.",
                             "The alien beast needs light to live. It is quite shy so tries to stay away but is very calm."};

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        dinoSelection.SetActive(true);
        dinoInfo.SetActive(false);
        int children = dinoButtonContainer.transform.childCount;
        for(int x = 0; x != children; x++)
        {
            int index = x;
            GameObject button = dinoButtonContainer.transform.GetChild(index).gameObject;
            button.GetComponent<Button>().onClick.AddListener(delegate {go_to_dino(index);});
        }
    }

    public void go_to_dino(int index)
    {
        Debug.Log(dinoNames[index]);
        dinosaur.sprite = dinoImages[index];
        dinoName.text = dinoNames[index];
        if (fiction[index] == 0)
        {
            fictionText.text = "Fiction";
        } else
        {
            fictionText.text = "Non-Fiction";
        }
        habitatInfo.text = habitat[index];
        generalInfo.text = general[index];

        animation.SetTrigger("dinos");
    }

    public void go_to_dino_selection()
    {
        animation.SetTrigger("selection");
    }

    public void go_to_main_menu()
    {
        SceneManager.LoadScene(0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
