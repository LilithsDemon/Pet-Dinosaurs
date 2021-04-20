using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class playerUI : MonoBehaviour
{
    //Variables needed

    //200, 1, 400, 400, 200

    //UI Text
    public Text heldItem;
    private int currentHeld;
    public Text psText;
    public Text msText;
    public Text pepperText;
    public Text mangoText;
    public List<Text> moneyTexts;

    //UI hotbar
    public Canvas uiCanvas;
    public List<string> items;
    public List<Button> itemBackground;

    //settings
    public Canvas Settings;

    //UI controls
    public GameObject utiliseGO;
    public Button utiliseButton;
    private string currentTag;

    //Shop UI
    public Canvas Shop;
    public Text buyButton;

    //XP gauge
    public Text levelText;
    public Image xpGauge;
    public int[] xpNeeded = { 0, 100, 120, 140, 170, 210, 250, 300, 360, 500};
    public Text xpText;
    public Text neededXPText;

    //Dino Shop
    public Button dinoBuyButton;
    public GameObject dinoButton;
    public GameObject dinoNotOwnedButton;
    public GameObject dinoButtonContainer;
    private int[] dinoPrices = { 0, 20, 50, 100, 150, 225, 300, 350 };
    private int[] dinoLevel = { 1, 1, 2, 2, 3, 5, 10, 10 };
    public GameObject[] dinos;

    public Text[] dinoTexts;


    // Plots
    public Button[] plotButtons;
    public GameObject[] plots;
    private int[] plotPrices = { 0, 0, 30, 70, 120, 180, 250, 320 };
    public Button plotBuyButton;

    private int[] upgradePrices = { 20, 50, 80, 110, 150 };
    public Text[] upgradeTexts;

    public Canvas inspector;
    public Button[] colors;
    public Material[] dinoMats;


    //Title, Text, number to do, reward num, reward index.
    public List<int> quests = new List<int>() {0, 1, 2, 3};
    private List<string> quest0 = new List<string>() { "Buy a dinosaur", "You must buy 1 dinosaur from the shop.", "1", "2", "0" };
    private List<string> quest1 = new List<string>() { "Buy 2 seeds", "You must buy 2 seeds from the shop.", "2", "1", "4" };
    private List<string> quest2 = new List<string>() { "Buy a mango", "You must buy 1 mango from the shop.", "1", "2", "2" };
    private List<string> quest3 = new List<string>() { "Buy a pepper", "You must buy 1 pepper from the shop.", "1", "2", "1" };

    //Quest panels
    public Text title;
    public Text description;
    public Text numToDo;
    public Text rewardNum;
    public Image rewardImg;
    public GameObject claimButton;
    public Sprite[] rewardImages;

    //Build menu
    public Canvas buildCanvas;
    public GameObject decoratonButton;
    public GameObject decorationButtonContainer;
    public Button decorationBuyButton;

    public GameObject[] hotBarParts;
    public Button[] buildButtons;
    public List<GameObject> decorationsTexts = new List<GameObject>();


    //For computer
    public int hotBarValue = 0;
    public float mouseWheelRotation = 0;

    //quit function
    public void quit()
    {
        Application.Quit();
    }





    //set texts
    public void set_xp()
    {
        xpGauge.fillAmount = SaveManager.Instance.xp() / Convert.ToSingle(xpNeeded[SaveManager.Instance.level()]);
        xpText.text = SaveManager.Instance.xp().ToString();
        neededXPText.text = xpNeeded[SaveManager.Instance.level()].ToString();
    }

    public void set_level()
    {
        levelText.text = "Level: " + SaveManager.Instance.level();
    }

    public void set_items()
    {
        psText.text = SaveManager.Instance.pepperSeeds().ToString();
        msText.text = SaveManager.Instance.mangoSeeds().ToString();
        pepperText.text = SaveManager.Instance.peppers().ToString();
        mangoText.text = SaveManager.Instance.mangos().ToString();
    }

    public void set_money()
    {
        string money = SaveManager.Instance.coins().ToString();
        foreach (Text textField in moneyTexts)
        {
            textField.text = money;
        }
    }





    //Settings
    public void toggle_on_settings()
    {
        Cursor.lockState = CursorLockMode.None;
        Settings.enabled = true;
        uiCanvas.enabled = false;
        Time.timeScale = 0.00001f;
    }

    public void toggle_off_settings()
    {
        Cursor.lockState = CursorLockMode.Locked;
        set_money();
        FindObjectOfType<TabGroup>().close_shop();
        Settings.enabled = false;
        Shop.enabled = false;
        inspector.enabled = false;
        uiCanvas.enabled = true;
        Time.timeScale = 1;
    }





    //UI button logic
    public void button_press(GameObject item)
    {
        if (item.tag == "Shop")
        {
            Cursor.lockState = CursorLockMode.None;
            Shop.enabled = true;
            uiCanvas.enabled = false;
            Time.timeScale = 0.00001f;
        }
        else if (item.tag == "Dinosaur")
        {

            if (SaveManager.Instance.current_held() == 2 && SaveManager.Instance.peppers() > 0)
            {
                dinosuar_feed(item, 2);
            }
            else if (SaveManager.Instance.current_held() == 3 && SaveManager.Instance.mangos() > 0)
            {
                dinosuar_feed(item, 3);
            } 
            else if (SaveManager.Instance.current_held() == 4)
            {
                toggle_inspector();
            }
        }
        else if(item.tag == "Crop")
        {
            Transform trans = item.transform;
            Transform mango = trans.Find("Mango");
            Transform pepper = trans.Find("Pepper");
            if (SaveManager.Instance.current_held() == 0 && mango.gameObject.transform.localScale.x == 0 && pepper.gameObject.transform.localScale.x == 0) {
                SaveManager.Instance.add_items(0, -1);
                pepper.gameObject.GetComponent<foodGrowth>().enabled = true;
                set_items();
            }
            else if (SaveManager.Instance.current_held() == 1 && mango.gameObject.transform.localScale.x == 0 && pepper.gameObject.transform.localScale.x == 0) {
                SaveManager.Instance.add_items(1, -1);
                mango.gameObject.GetComponent<foodGrowth>().enabled = true;
                set_items();
            } 
            else if (mango.gameObject.transform.localScale.x >= 0.005) {
                mango.gameObject.GetComponent<foodGrowth>().reset();
                mango.gameObject.GetComponent<foodGrowth>().enabled = false;
                SaveManager.Instance.add_items(3, 1);
                System.Random rnd = new System.Random();
                int chance = rnd.Next(1, 100);
                if (chance <= 15 + SaveManager.Instance.extra_seed_chance() * 5)
                {
                    SaveManager.Instance.add_items(1, 1);
                } 
                set_items();
            }
            else if (pepper.gameObject.transform.localScale.x >= 0.005)
            {
                pepper.gameObject.GetComponent<foodGrowth>().reset();
                pepper.gameObject.GetComponent<foodGrowth>().enabled = false;
                SaveManager.Instance.add_items(2, 1);
                System.Random rnd = new System.Random();
                int chance = rnd.Next(1, 100);
                if (chance <= 15 + SaveManager.Instance.extra_seed_chance() * 5)
                {
                    SaveManager.Instance.add_items(0, 1);
                }
                set_items();
            }
        }
    }

    public void turn_off_button()
    {
        utiliseGO.SetActive(false);
    }

    public void turn_on_button(string textForButton, GameObject item)
    {
        string currentTag = item.tag;
        if (currentTag == "Dinosaur")
        {
            if (currentHeld > 1 && !(currentHeld == 4))
            {
                utiliseGO.SetActive(true);
                utiliseButton.GetComponentInChildren<Text>().text = "Feed " + textForButton;
            } else if (currentHeld == 4)
            {
                utiliseGO.SetActive(true);
                utiliseButton.GetComponentInChildren<Text>().text = "Interact with " + textForButton;
            }
            else
            {
                turn_off_button();
            }
        }
        else if (currentTag == "Shop")
        {
            utiliseGO.SetActive(true);
            utiliseButton.GetComponentInChildren<Text>().text = "Shop";
        }
        else if (currentTag == "Crop")
        {
            Transform trans = item.transform;
            Transform mango = trans.Find("Mango");
            Transform pepper = trans.Find("Pepper");
            if (mango.gameObject.transform.localScale.x == 0 && pepper.gameObject.transform.localScale.x == 0)
            {
                if (currentHeld == 0 && SaveManager.Instance.pepperSeeds() != 0)
                {
                    utiliseGO.SetActive(true);
                    utiliseButton.GetComponentInChildren<Text>().text = "Plant " + textForButton;
                } 
                else if (currentHeld == 1 && SaveManager.Instance.mangoSeeds() != 0)
                {
                    utiliseGO.SetActive(true);
                    utiliseButton.GetComponentInChildren<Text>().text = "Plant " + textForButton;
                } else
                {
                    utiliseGO.SetActive(false);
                }
            } 
            else if (mango.gameObject.transform.localScale.x >= 0.005 || pepper.gameObject.transform.localScale.x >= 0.005)
            {
                if (mango.gameObject.transform.localScale.x >= 0.005)
                {
                    utiliseGO.SetActive(true);
                    utiliseButton.GetComponentInChildren<Text>().text = "Harvest Mango";
                } 
                else {
                    utiliseGO.SetActive(true);
                    utiliseButton.GetComponentInChildren<Text>().text = "Harvest Pepper";
                }
            } else
            {
                utiliseGO.SetActive(false);
            }
            
        }
    }





    //Hot bar Logic

    //Selects the item that the player is holding
    public void change_item_held(int index)
    {
        hotBarValue = index;
        heldItem.text = items[index];
        currentHeld = index;
        SaveManager.Instance.change_current_held(index);
        int i = 0;
        foreach (Button values in itemBackground)
        {
            if (i == index)
            {
                turn_blue(i);
            }
            else
            {
                turn_black(i);
            }
            i++;
        }
        turn_off_button();
    }

    public void turn_black(int index)
    {
        ColorBlock colors = itemBackground[index].colors;
        colors.normalColor = Color.black;
        colors.highlightedColor = new Color32(0, 0, 0, 255);
        itemBackground[index].colors = colors;
    }

    public void turn_blue(int index)
    {
        ColorBlock colors = itemBackground[index].colors;
        colors.normalColor = Color.blue;
        colors.highlightedColor = new Color32(0, 25, 255, 255);
        itemBackground[index].colors = colors;
    }





    //Shop Logic

    //This sets the text to the Shop button to buy
    public string buy_button_text()
    {
        if (SaveManager.Instance.itemIndex() == 0)
        {
            return "Buy Pepper Seed";
        }
        else if (SaveManager.Instance.itemIndex() == 1)
        {
            return "Buy Mango Seed";
        }
        else if (SaveManager.Instance.itemIndex() == 2)
        {
            return "Buy Pepper";
        }
        else if (SaveManager.Instance.itemIndex() == 3)
        {
            return "Buy Mango";
        }
        else
        {
            return "Buy Button";
        }
    }

    //This runs if you select an item
    public void buyitems(int itemval)
    {
        if (itemval < 4)
        {
            SaveManager.Instance.changeItemIndex(itemval);
            buyButton.text = buy_button_text();
        }
        else
        {
            itemval = 4;
        }
    }

    //This happens once you press buy for that item
    public void buy()
    {
        if (SaveManager.Instance.itemIndex() == 0)
        {
            if (SaveManager.Instance.coins() >= 1)
            {
                SaveManager.Instance.add_coins(-1);
                SaveManager.Instance.add_items(0, 1);
            }
            if (SaveManager.Instance.current_quest() == 1)
            {
                SaveManager.Instance.set_current_quest_complete_num(SaveManager.Instance.current_quest_complete_num() + 1);
                setup_quest(SaveManager.Instance.current_quest());
            }
        }
        else if (SaveManager.Instance.itemIndex() == 1)
        {
            if (SaveManager.Instance.coins() >= 1)
            {
                SaveManager.Instance.add_coins(-1);
                SaveManager.Instance.add_items(1, 1);
            }
            if (SaveManager.Instance.current_quest() == 1)
            {
                SaveManager.Instance.set_current_quest_complete_num(SaveManager.Instance.current_quest_complete_num() + 1);
                setup_quest(SaveManager.Instance.current_quest());
            }
        }
        else if (SaveManager.Instance.itemIndex() == 2)
        {
            if (SaveManager.Instance.coins() >= 5)
            {
                SaveManager.Instance.add_coins(-5);
                SaveManager.Instance.add_items(2, 1);
            }
            if (SaveManager.Instance.current_quest() == 3)
            {
                SaveManager.Instance.set_current_quest_complete_num(SaveManager.Instance.current_quest_complete_num() + 1);
                setup_quest(SaveManager.Instance.current_quest());
            }
        }
        else if (SaveManager.Instance.itemIndex() == 3)
        {
            if (SaveManager.Instance.coins() >= 5)
            {
                SaveManager.Instance.add_coins(-5);
                SaveManager.Instance.add_items(3, 1);
            }
            if (SaveManager.Instance.current_quest() == 2)
            {
                SaveManager.Instance.set_current_quest_complete_num(SaveManager.Instance.current_quest_complete_num() + 1);
                setup_quest(SaveManager.Instance.current_quest());
            }
        }
        set_money();
        set_items();
    }




    
    //Xp Logic
    public void update_xp(int value)
    {
        int currentXP = SaveManager.Instance.xp() + value;
        while (currentXP >= xpNeeded[SaveManager.Instance.level()] && SaveManager.Instance.level() < 10) {
            currentXP -= xpNeeded[SaveManager.Instance.level()];
            SaveManager.Instance.level_up();
            set_level();
            set_dino_shop();
        }
        SaveManager.Instance.set_xp(currentXP);
        set_xp();
    }





    // Dinosaur Logic
    public void dinosuar_feed(GameObject item, int currentItemHeld)
    {
        //gives player coins and then sets the players money text
        SaveManager.Instance.add_coins(2);
        set_money();

        //gives player 5xp for now per feed
        update_xp(5);

        //takes away one of that item and then prints it.
        SaveManager.Instance.add_items(currentItemHeld, -1);
        set_items();

        FindObjectOfType<audio_manager>().play_random_happy_sound();
    }


    public void spawn_dinos()
    {
        int i = 0;
        foreach(GameObject dino in dinos)
        {
            if ((SaveManager.Instance.dinos_owned() & 1 << i) == 1 << i)
            {
                dino.SetActive(true);
                dinoTexts[i].enabled = true;
            } else
            {
                dino.SetActive(false);
                dinoTexts[i].enabled = false;
            }
            i++;
        }
    }

    public void dinosaur_buy_button()
    {
        if (!((SaveManager.Instance.dinos_owned() & 1 << SaveManager.Instance.current_dino_selected()) == 1 << SaveManager.Instance.current_dino_selected()))
        {
            if (SaveManager.Instance.coins() >= dinoPrices[SaveManager.Instance.current_dino_selected()] && SaveManager.Instance.level() >= dinoLevel[SaveManager.Instance.current_dino_selected()])
            {
                SaveManager.Instance.add_dinos_owned(1 << SaveManager.Instance.current_dino_selected());
                SaveManager.Instance.add_coins(-dinoPrices[SaveManager.Instance.current_dino_selected()]);
                set_money();
                dinosaur_buy_setup(SaveManager.Instance.current_dino_selected());
                set_dino_shop();
                spawn_dinos();

                if (SaveManager.Instance.current_quest() == 0)
                {
                    SaveManager.Instance.set_current_quest_complete_num(SaveManager.Instance.current_quest_complete_num() + 1);
                    setup_quest(SaveManager.Instance.current_quest());
                }

            } else
            {
            }
        }
    }

    public void dinosaur_buy_setup(int i)
    {
        if (i == -1)
        {
            dinoBuyButton.GetComponentInChildren<Text>().text = "Please select a dinosaur";
        }
        else
        {
            if ((SaveManager.Instance.dinos_owned() & 1 << i) == 1 << i)
            {
                dinoBuyButton.GetComponentInChildren<Text>().text = "You already own this Dinosaur";
            }
            else
            {
                dinoBuyButton.GetComponentInChildren<Text>().text = "Buy for " + dinoPrices[i] + " Coins";
            }
            SaveManager.Instance.set_dino_selected(i);
        }
        
    }

    public void set_dino_shop()
    {
        foreach(Transform child in dinoButtonContainer.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        Sprite[] textures = Resources.LoadAll<Sprite>("dinoShop");
        int i = 0;
        foreach (Sprite texture in textures)
        {
            if ((SaveManager.Instance.dinos_owned() & 1 << i) == 1 << i)
            {
                GameObject container = Instantiate(dinoButton) as GameObject;
                container.GetComponent<Image>().sprite = texture;
                container.transform.SetParent(dinoButtonContainer.transform, false);
                int index = i;
                container.GetComponent<Button>().onClick.AddListener(() => dinosaur_buy_setup(index));
            }
            else
            {
                GameObject container = Instantiate(dinoNotOwnedButton) as GameObject;
                container.GetComponent<Image>().sprite = texture;
                Text moneyInButton = container.transform.GetChild(0).GetChild(1).gameObject.GetComponent<Text>();
                moneyInButton.text = dinoPrices[i].ToString();
                Text levelInButton = container.transform.GetChild(0).GetChild(2).gameObject.GetComponent<Text>();
                if (SaveManager.Instance.level() < dinoLevel[i])
                {
                    levelInButton.text = "Level: " + dinoLevel[i].ToString();
                } else
                {
                    levelInButton.text = "";
                }
                container.transform.SetParent(dinoButtonContainer.transform, false);
                int index = i;
                container.GetComponent<Button>().onClick.AddListener(() => dinosaur_buy_setup(index));
            }
            i++;
        }
    }

    public void plot_buy_button()
    {
        if (SaveManager.Instance.current_plot_selected() < 8 && SaveManager.Instance.current_plot_selected() != -1)
        {
            if (!((SaveManager.Instance.plots_owned() & 1 << SaveManager.Instance.current_plot_selected()) == 1 << SaveManager.Instance.current_plot_selected()))
            {
                if (SaveManager.Instance.coins() >= plotPrices[SaveManager.Instance.number_of_plots_owned()])
                {
                    SaveManager.Instance.add_plots_owned(1 << SaveManager.Instance.current_plot_selected());
                    SaveManager.Instance.add_coins(-(plotPrices[SaveManager.Instance.number_of_plots_owned()]));
                    SaveManager.Instance.set_number_of_plots_owned();
                    plot_button_press(SaveManager.Instance.current_plot_selected());
                    plot_setup();
                    set_money();
                }
                else
                {
                }
            }
        }
        else if (SaveManager.Instance.current_plot_selected() == 8 && SaveManager.Instance.grow_time_level() < 5 && SaveManager.Instance.coins() >= upgradePrices[SaveManager.Instance.grow_time_level()])
        {
            SaveManager.Instance.add_coins(-(upgradePrices[SaveManager.Instance.grow_time_level()]));
            SaveManager.Instance.add_grow_time_level();
            plot_button_press(SaveManager.Instance.current_plot_selected());
            upgrade_texts(0);
            plot_setup();
            set_money();
        }
        else if (SaveManager.Instance.current_plot_selected() == 9 && SaveManager.Instance.extra_seed_chance() < 5 && SaveManager.Instance.coins() >= upgradePrices[SaveManager.Instance.extra_seed_chance()])
        {
            SaveManager.Instance.add_coins(-(upgradePrices[SaveManager.Instance.extra_seed_chance()]));
            SaveManager.Instance.add_extra_seed_chance();
            plot_button_press(SaveManager.Instance.current_plot_selected());
            upgrade_texts(1);
            plot_setup();
            set_money();
        }
    }

    public void plot_button_press(int i)
    {
        if (i == -1)
        {
            plotBuyButton.GetComponentInChildren<Text>().text = "Please select a plot or upgrade";
        }
        else if (i < 8)
        {
            if ((SaveManager.Instance.plots_owned() & 1 << i) == 1 << i)
            {
                plotBuyButton.GetComponentInChildren<Text>().text = "You already own this plot";
            }
            else
            {                
                plotBuyButton.GetComponentInChildren<Text>().text = "Buy for " + plotPrices[SaveManager.Instance.number_of_plots_owned()] + " Coins";
            }
            SaveManager.Instance.set_plot_selected(i);
        }else if (i == 8)
        {
            if (SaveManager.Instance.grow_time_level() < 5)
            {
                plotBuyButton.GetComponentInChildren<Text>().text = "Buy grow time: level: " + (SaveManager.Instance.grow_time_level() + 1) + " : " + upgradePrices[SaveManager.Instance.grow_time_level()];
            }
            else
            {
                plotBuyButton.GetComponentInChildren<Text>().text = "This upgrade is at full level";
            }
            SaveManager.Instance.set_plot_selected(i);
        }
        else
        {
            if (SaveManager.Instance.extra_seed_chance() < 5)
            {
                plotBuyButton.GetComponentInChildren<Text>().text = "Buy extra seed chance: level: " + (SaveManager.Instance.extra_seed_chance() + 1) + " : " + upgradePrices[SaveManager.Instance.extra_seed_chance()];
            }
            else
            {
                plotBuyButton.GetComponentInChildren<Text>().text = "This upgrade is at full level";
            }
            SaveManager.Instance.set_plot_selected(i);
        }
        
    }

    public void plot_setup()
    {

        for (int i = 0; i < plotButtons.Length; i++)
        {
            if (i < 8)
            {
                if ((SaveManager.Instance.plots_owned() & 1 << i) == 1 << i)
                {
                    plotButtons[i].GetComponent<Image>().color = Color.green;
                    plots[i].SetActive(true);
                }
                else
                {
                    plotButtons[i].GetComponent<Image>().color = Color.red;
                    plots[i].SetActive(false);
                }
            }
            int index = i;
            plotButtons[i].onClick.AddListener(() => plot_button_press(index));
        }
    }

    public void upgrade_texts(int index)
    {
        if (index == 0)
        {
            upgradeTexts[0].text = "Level: " + SaveManager.Instance.grow_time_level() + "/5"; 
        } else
        {
            upgradeTexts[1].text = "Level: " + SaveManager.Instance.extra_seed_chance() + "/5";
        }
    }

    public void toggle_inspector()
    {
        Cursor.lockState = CursorLockMode.None;
        inspector.enabled = true;
        uiCanvas.enabled = false;
        Time.timeScale = 0.00001f;
    }

    public void change_dino_color(int index)
    {
        int i = 0;
        foreach(Button colorButton in colors)
        {
            Color color = colorButton.GetComponent<Image>().color;
            if (index == i)
            {
                color.a = 1;
                List<GameObject> items = new List<GameObject>();
                items = FindObjectOfType<objectsNearPlayer>().find_objects_near_player();
                int dino = 0;
                foreach (GameObject dinosaurs in dinos)
                {
                    if (items[0] == dinosaurs)
                    {
                        if (dino == 0)
                        {
                            Material[] oldTextures;
                            oldTextures = dinosaurs.transform.GetChild(0).gameObject.GetComponent<Renderer>().materials;
                            oldTextures[3] = dinoMats[index];
                            dinosaurs.transform.GetChild(0).gameObject.GetComponent<Renderer>().materials = oldTextures;
                            SaveManager.Instance.edit_dino_mat_values(dino, index);
                        } 
                        else if (dino == 1)
                        {
                            Material[] oldTextures;
                            oldTextures = dinosaurs.transform.GetChild(1).gameObject.GetComponent<Renderer>().materials;
                            oldTextures[0] = dinoMats[index];
                            dinosaurs.transform.GetChild(1).gameObject.GetComponent<Renderer>().materials = oldTextures;
                            SaveManager.Instance.edit_dino_mat_values(dino, index);
                        } 
                        else if (dino == 2)
                        {
                            Material[] oldTextures;
                            oldTextures = dinosaurs.transform.GetChild(1).gameObject.GetComponent<Renderer>().materials;
                            oldTextures[0] = dinoMats[index];
                            dinosaurs.transform.GetChild(1).gameObject.GetComponent<Renderer>().materials = oldTextures;
                            SaveManager.Instance.edit_dino_mat_values(dino, index);
                        }
                        else if (dino == 3)
                        {
                            Material[] oldTextures;
                            oldTextures = dinosaurs.transform.GetChild(1).gameObject.GetComponent<Renderer>().materials;
                            oldTextures[0] = dinoMats[index];
                            dinosaurs.transform.GetChild(1).gameObject.GetComponent<Renderer>().materials = oldTextures;
                            SaveManager.Instance.edit_dino_mat_values(dino, index);
                        }
                        else if (dino == 4)
                        {
                            Material[] oldTextures;
                            oldTextures = dinosaurs.transform.GetChild(1).gameObject.GetComponent<Renderer>().materials;
                            oldTextures[0] = dinoMats[index];
                            dinosaurs.transform.GetChild(1).gameObject.GetComponent<Renderer>().materials = oldTextures;
                            SaveManager.Instance.edit_dino_mat_values(dino, index);
                        }
                        else if (dino == 5)
                        {
                            Material[] oldTextures;
                            oldTextures = dinosaurs.transform.GetChild(1).gameObject.GetComponent<Renderer>().materials;
                            oldTextures[0] = dinoMats[index];
                            dinosaurs.transform.GetChild(1).gameObject.GetComponent<Renderer>().materials = oldTextures;
                            SaveManager.Instance.edit_dino_mat_values(dino, index);
                        }
                        else if (dino == 6)
                        {
                            Material[] oldTextures;
                            oldTextures = dinosaurs.transform.GetChild(1).gameObject.GetComponent<Renderer>().materials;
                            oldTextures[2] = dinoMats[index];
                            dinosaurs.transform.GetChild(1).gameObject.GetComponent<Renderer>().materials = oldTextures;
                            SaveManager.Instance.edit_dino_mat_values(dino, index);
                        }
                        else if (dino == 7)
                        {
                            Material[] oldTextures;
                            oldTextures = dinosaurs.transform.GetChild(1).gameObject.GetComponent<Renderer>().materials;
                            oldTextures[0] = dinoMats[index];
                            dinosaurs.transform.GetChild(1).gameObject.GetComponent<Renderer>().materials = oldTextures;
                            SaveManager.Instance.edit_dino_mat_values(dino, index);
                        }

                    }
                    dino++;
                }
            } else
            {
                color.a = 0.4f;
            }
            colorButton.GetComponent<Image>().color = color;
            i++;
        }
    }

    public void dino_start_mats(GameObject[] dinos)
    {
        int dino = 0;
        foreach (GameObject dinosaurs in dinos)
        {
            // oldTextures[x] = dinoMats[SaveManager.Instance.dino_mat_values()[dino]];
            if (dino == 0)
            {
                Material[] oldTextures;
                oldTextures = dinosaurs.transform.GetChild(0).gameObject.GetComponent<Renderer>().materials;
                oldTextures[3] = dinoMats[SaveManager.Instance.dino_mat_values()[dino]];
                dinosaurs.transform.GetChild(0).gameObject.GetComponent<Renderer>().materials = oldTextures;
            }
            else if (dino == 1)
            {
                Material[] oldTextures;
                oldTextures = dinosaurs.transform.GetChild(1).gameObject.GetComponent<Renderer>().materials;
                oldTextures[0] = dinoMats[SaveManager.Instance.dino_mat_values()[dino]];
                dinosaurs.transform.GetChild(1).gameObject.GetComponent<Renderer>().materials = oldTextures;
            }
            else if (dino == 2)
            {
                Material[] oldTextures;
                oldTextures = dinosaurs.transform.GetChild(1).gameObject.GetComponent<Renderer>().materials;
                oldTextures[0] = dinoMats[SaveManager.Instance.dino_mat_values()[dino]];
                dinosaurs.transform.GetChild(1).gameObject.GetComponent<Renderer>().materials = oldTextures;
            }
            else if (dino == 3)
            {
                Material[] oldTextures;
                oldTextures = dinosaurs.transform.GetChild(1).gameObject.GetComponent<Renderer>().materials;
                oldTextures[0] = dinoMats[SaveManager.Instance.dino_mat_values()[dino]];
                dinosaurs.transform.GetChild(1).gameObject.GetComponent<Renderer>().materials = oldTextures;
            }
            else if (dino == 4)
            {
                Material[] oldTextures;
                oldTextures = dinosaurs.transform.GetChild(1).gameObject.GetComponent<Renderer>().materials;
                oldTextures[0] = dinoMats[SaveManager.Instance.dino_mat_values()[dino]];
                dinosaurs.transform.GetChild(1).gameObject.GetComponent<Renderer>().materials = oldTextures;
            }
            else if (dino == 5)
            {
                Material[] oldTextures;
                oldTextures = dinosaurs.transform.GetChild(1).gameObject.GetComponent<Renderer>().materials;
                oldTextures[0] = dinoMats[SaveManager.Instance.dino_mat_values()[dino]];
                dinosaurs.transform.GetChild(1).gameObject.GetComponent<Renderer>().materials = oldTextures;
            }
            else if (dino == 6)
            {
                Material[] oldTextures;
                oldTextures = dinosaurs.transform.GetChild(1).gameObject.GetComponent<Renderer>().materials;
                oldTextures[2] = dinoMats[SaveManager.Instance.dino_mat_values()[dino]];
                dinosaurs.transform.GetChild(1).gameObject.GetComponent<Renderer>().materials = oldTextures;
            }
            else if (dino == 7)
            {
                Material[] oldTextures;
                oldTextures = dinosaurs.transform.GetChild(1).gameObject.GetComponent<Renderer>().materials;
                oldTextures[0] = dinoMats[SaveManager.Instance.dino_mat_values()[dino]];
                dinosaurs.transform.GetChild(1).gameObject.GetComponent<Renderer>().materials = oldTextures;
            }
            dino++;
        }
    }

    public void complete_task()
    {
        SaveManager.Instance.edit_quests_completed(SaveManager.Instance.current_quest());
        if (SaveManager.Instance.current_quest() == 0)
        {
            //give 2 pepper seeds
            SaveManager.Instance.add_items(0, 2);
            set_items();
        } else if (SaveManager.Instance.current_quest() == 1)
        {
            //give 1 coin
            SaveManager.Instance.add_coins(1);
            set_money();
        } else if(SaveManager.Instance.current_quest() == 2)
        {
            //give 2 peppers
            SaveManager.Instance.add_items(2, 2);
            set_items();
        } else if(SaveManager.Instance.current_quest() == 3)
        {
            //give 2 melon seeds
            SaveManager.Instance.add_items(1, 2);
            set_items();
        }

        start_quests();
    }

    public void tasks_all_done()
    {
        title.text = "Congratulations";
        description.text = "You have completed all the tasks";
        numToDo.text = "";
        rewardNum.text = "";
    }

    public void setup_quest(int index)
    {
        List<string> quest = new List<string>();
        if (index == -1)
        {
            start_quests();
        }
        else if (index == 0)
        {
            quest = quest0;
        } else if (index == 1)
        {
            quest = quest1;
        } else if (index == 2)
        {
            quest = quest2;
        } else if (index == 3)
        {
            quest = quest3;
        }
        title.text = quest[0];
        description.text = quest[1];
        numToDo.text = SaveManager.Instance.current_quest_complete_num() + "/" + quest[2];
        rewardNum.text = quest[3];
        rewardImg.sprite = rewardImages[Int32.Parse(quest[4])];
        if (SaveManager.Instance.current_quest_complete_num() >= Int32.Parse(quest[2]))
        {
            claimButton.SetActive(true);
        } else
        {
            claimButton.SetActive(false);
        }
    }

    public void start_quests()
    {
        SaveManager.Instance.set_current_quest_complete_num(0);
        List<int> questsDone = SaveManager.Instance.quests_completed();
        if (questsDone.Count < quests.Count + 2)
        {
            foreach (int quest in questsDone)
            {
                quests.Remove(quest);
            }
            System.Random rd = new System.Random();
            int randomNum = rd.Next(0, (quests.Count));
            SaveManager.Instance.set_current_quest(quests[randomNum]);
            setup_quest(quests[randomNum]);
        }
        else
        {
            tasks_all_done();
        }
    }



    public void open_decorations() 
    {
        buildCanvas.enabled = true;
        uiCanvas.enabled = false;
    }

    public void close_decorations()    
    {
        buildCanvas.enabled = false;
        uiCanvas.enabled = true;
    }



    public void set_decorationss()
    {

        Sprite[] textures = Resources.LoadAll<Sprite>("Decorations");
        int i = 0;
        foreach (Sprite texture in textures)
        {
            GameObject container = Instantiate(decoratonButton) as GameObject;
            container.GetComponent<Image>().sprite = texture;
            container.transform.SetParent(decorationButtonContainer.transform, false);
            int index = i;
            container.GetComponent<Button>().onClick.AddListener(() => decoration_button_setup(index));
            GameObject numText = container.transform.GetChild(0).GetChild(0).gameObject;
            decorationsTexts.Add(numText);
            numText.GetComponent<Text>().text = SaveManager.Instance.num_of_decoration(i).ToString();
            i++;
        }
    }

    public void decoration_button_setup(int i)
    {
        if (i == -1)
        {
            decorationBuyButton.GetComponentInChildren<Text>().text = "Select a decoration";
        }
        else
        {
            if (SaveManager.Instance.num_of_decoration(i) >= 1) {
                decorationBuyButton.GetComponentInChildren<Text>().text = "Place this decoration";
            } else {
                decorationBuyButton.GetComponentInChildren<Text>().text = "Sorry you do not have any";
            }
            
            SaveManager.Instance.set_decoration_selected(i);
        }
        
    }

    public void decoration_start() 
    {
        if (SaveManager.Instance.num_of_decoration(SaveManager.Instance.current_decoration_selected()) > 0) 
        {
            FindObjectOfType<decorationManager>().toggleBuildMode();
            close_decorations();
            foreach (Button buildBtn in buildButtons)
            {
                buildBtn.gameObject.SetActive(true);
                buildBtn.GetComponent<Button>().enabled = true;
            }
            foreach(GameObject part in hotBarParts) 
            {
                part.SetActive(false);
            }
        }
    }

    public void stop_placing_decoration()
    {
        FindObjectOfType<decorationManager>().stopPlacing();
        foreach (Button buildBtn in buildButtons)
        {
            buildBtn.gameObject.SetActive(false);
        }
        foreach(GameObject part in hotBarParts) 
        {
            part.SetActive(true);
        }
    }

    public void place_decoration()
    {
        FindObjectOfType<decorationManager>().placeObject();
        foreach (Button buildBtn in buildButtons)
        {
            buildBtn.gameObject.SetActive(false);
        }
        int currentDecoration = SaveManager.Instance.current_decoration_selected();
        SaveManager.Instance.add_decoration(currentDecoration, -1);
        foreach(GameObject part in hotBarParts) 
        {
            part.SetActive(true);
        }
        decorationsTexts[currentDecoration].GetComponent<Text>().text = SaveManager.Instance.num_of_decoration(currentDecoration).ToString();
    }


    public void play_race() 
    {
        int dino = 0;
        List<GameObject> items = new List<GameObject>();
        items = FindObjectOfType<objectsNearPlayer>().find_objects_near_player();
        foreach (GameObject dinosaur in dinos) 
        {
            if(dinosaur == items[0]) 
            {
                SaveManager.Instance.set_dino_to_play_with(dino);
                SceneManager.LoadScene(4);
            }
            dino++;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Settings.enabled = false;
        uiCanvas.enabled = true;
        Shop.enabled = false;
        inspector.enabled = false;
        buildCanvas.enabled = false;
        currentHeld = 0;
        heldItem.text = items[0];
        buyButton.text = buy_button_text();
        SaveManager.Instance.set_dino_selected(-1);
        
        turn_blue(0);
        set_money();
        set_items();
        update_xp(0);
        set_level();
        set_xp();

        plot_setup();
        SaveManager.Instance.set_plot_selected(-1);
        plot_button_press(-1);

        upgrade_texts(0);
        upgrade_texts(1);

        

        spawn_dinos();

        setup_quest(SaveManager.Instance.current_quest());

        set_decorationss();
        decoration_button_setup(-1);
        SaveManager.Instance.set_decoration_selected(-1);

        dino_start_mats(dinos);
        set_dino_shop();
        dinosaur_buy_setup(-1);
        foreach (Button buildBtn in buildButtons)
        {
            buildBtn.gameObject.SetActive(false);
        }
    }

    public void go_to_main_menu()
    {
        toggle_off_settings();
        SceneManager.LoadScene(0);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            FindObjectOfType<objectsNearPlayer>().utilise_button_press();
        }
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            toggle_on_settings();
        }
        if(Input.GetKeyDown("1"))
        {
            change_item_held(0);
        }
        if(Input.GetKeyDown("2"))
        {
            change_item_held(1);
        }
        if(Input.GetKeyDown("3"))
        {
            change_item_held(2);
        }
        if(Input.GetKeyDown("4"))
        {
            change_item_held(3);
        }
        if(Input.GetKeyDown("5"))
        {
            change_item_held(4);
        }
        /*
        if(Input.mouseScrollDelta.y > mouseWheelRotation)
        {
            mouseWheelRotation = Input.mouseScrollDelta.y;
            hotBarValue += 1;
            if (hotBarValue > 4)
            {
                hotBarValue = 0;
            }
            change_item_held(hotBarValue);
        }else if( Input.mouseScrollDelta.y < mouseWheelRotation)
        {
            mouseWheelRotation = Input.mouseScrollDelta.y;
            hotBarValue -= 1;
            if (hotBarValue < 0)
            {
                hotBarValue = 4;
            }
            change_item_held(hotBarValue);
        }
        */
        if(Input.mouseScrollDelta.y != mouseWheelRotation)
        {
            mouseWheelRotation = Input.mouseScrollDelta.y;
            hotBarValue -= (int)Input.mouseScrollDelta.y;
            if (hotBarValue < 0)
            {
                hotBarValue = 4;
            } else if(hotBarValue > 4)
            {
                hotBarValue = 0;
            }

            change_item_held(hotBarValue);
        }
    }
        
}