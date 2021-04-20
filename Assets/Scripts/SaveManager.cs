using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { set; get; }
    public SaveScript state;

    private void Awake()
    {
        //ResetSave();
        DontDestroyOnLoad(gameObject);
        Instance = this;
        Load();
    }

    public void Save()
    {
        PlayerPrefs.SetString("save", Helper.Serialize<SaveScript>(state));
    }

    public void Load()
    {
        if(PlayerPrefs.HasKey("save"))
        {
            state = Helper.Deserialize<SaveScript>(PlayerPrefs.GetString("save"));
        }
        else
        {
            state = new SaveScript();
            Save();
            SceneManager.LoadScene(2);
        }
    }

    public int current_scene_to_load()
    {
        return state.sceneToLoad;
    }

    public void set_scene_to_load(int index)
    {
        state.sceneToLoad = index;
        Save();
    }

    public void set_time_rotation(int rotation)
    {
        state.timeAngle = rotation;
        Save();
    }

    public int time_rotation()
    {
        return state.timeAngle;
    }

    public void set_decoration_selected(int i) {
        state.currentDecorationSelected = i;
        Save();
    }

    public int current_decoration_selected() {
        return(state.currentDecorationSelected);
    }

    public int num_of_decoration(int i) {
        return(state.numOfDecorations[i]);
        Save();
    }

    public void add_decoration(int index, int amount)
    {
        state.numOfDecorations[index] += amount;
        Save();
    }

    public void set_decoration_to_place(int i) {
        state.decorationToPlace = i;
        Save();
    }

    public int current_dino_to_play_with() {
        return (state.dinoToPlayWith);
    }

    public void set_dino_to_play_with(int index) {
        state.dinoToPlayWith = index;
        Save();
    }

    public int current_quest_complete_num()
    {
        return (state.currentQuestCompleteNum);
    }

    public void set_current_quest_complete_num(int value)
    {
        state.currentQuestCompleteNum = value;
        Save();
    }

    public int current_quest()
    {
        return (state.currentQuest);
    }

    public void set_current_quest(int value)
    {
        state.currentQuest = value;
        Save();
    }

    public List<int> quests_completed()
    {
        return (state.questsCompleted);
    }

    public void edit_quests_completed(int value)
    {
        state.questsCompleted.Add(value);
        Save();
    }

    public int[] dino_mat_values()
    {
        return (state.dinoMatValues);
    }

    public void edit_dino_mat_values(int index, int matValue)
    {
        state.dinoMatValues[index] = matValue;
        Save();
    }

    public int number_of_plots_owned()
    {
        return (state.numberOfPlotsOwned);
    }

    public void set_number_of_plots_owned()
    {
        state.numberOfPlotsOwned += 1;
    }


    public int plots_owned()
    {
        return (state.plotsOwned);
    }

    public void add_plots_owned(int value)
    {
        state.plotsOwned += value;
        Save();
    }

    public int current_plot_selected()
    {
        return (state.currentPlotSelected);
    }

    public void set_plot_selected(int value)
    {
        state.currentPlotSelected = value;
    }

    public int dinos_owned()
    {
        return (state.dinosOwned);
    }

    public void add_dinos_owned(int value)
    {
        state.dinosOwned += value;
        Save();
    }

    public int current_dino_selected()
    {
        return (state.currentDinoSelected);
    }

    public void set_dino_selected(int index)
    {
        state.currentDinoSelected = index;
        Save();
    }

    public int extra_seed_chance()
    {
        return (state.extraSeedChance);
    }

    public void add_extra_seed_chance()
    {
        state.extraSeedChance += 1;
        Save();
    }

    public int grow_time_level()
    {
        return (state.growSpeedLevel);
    }

    public void add_grow_time_level()
    {
        state.growSpeedLevel += 1;
        Save();
    }

    public int current_held()
    {
        return (state.currentHeld);
    }

    public void change_current_held(int value)
    {
        state.currentHeld = value;
    }

    public int xp()
    {
        return (state.xp);
    }

    public void set_xp(int value)
    {
        state.xp = value;
        Save();
    }

    public int level()
    {
        return (state.level);
    }

    public void level_up()
    {
        state.level += 1;
        Save();
    }

    public int itemIndex()
    {
        return (state.itemIndex);
    }

    public int changeItemIndex(int value)
    {
        state.itemIndex = value;
        Save();
        return value;
    }

    public int coins()
    {
        return (state.coins);
    }

    public void add_coins(int value)
    {
        state.coins += value;
        Save();
    }

    public int pepperSeeds()
    {
        return (state.pepperSeeds);
    }

    public int mangoSeeds()
    {
        return (state.mangoSeeds);
    }

    public int peppers()
    {
        return (state.peppers);
    }

    public int mangos()
    {
        return (state.mangos);
    }

    public void add_items(int index, int amount)
    {
        if (index == 0)
        {
            state.pepperSeeds += amount;
        }
        else if (index == 1)
        {
            state.mangoSeeds += amount;
        }
        else if (index == 2)
        {
            state.peppers += amount;
        }
        else if(index == 3)
        {
            state.mangos += amount;
        }
        Save();
    }

    public void ResetSave()
    {
        PlayerPrefs.DeleteKey("save");
    }
}
