using System.Collections.Generic;

public class SaveScript
{
    public int coins = 6;
    public int level = 1;
    public int xp = 0;
    public int pepperSeeds = 1;
    public int mangoSeeds = 1;
    public int peppers = 1;
    public int mangos = 1;
    public int itemIndex = 4;
    public int currentHeld = 0;

    public int growSpeedLevel = 0;
    public int extraSeedChance = 0;

    public int dinosOwned = 1;
    public int currentDinoSelected = -1;

    public int plotsOwned = 24;
    public int numberOfPlotsOwned = 2;
    public int currentPlotSelected = -1;

    public int[] dinoMatValues = { 0, 0, 0, 0, 0, 0, 0, 0 };

    public List<int> questsCompleted = new List<int>();
    public int currentQuest = -1;
    public int currentQuestCompleteNum = 0;

    public int dinoToPlayWith = 4;

    public int[] numOfDecorations = {0,0,0,0,0};
    public int decorationToPlace = 0;
    public int currentDecorationSelected = -1;

    public int timeAngle = 0;
    
    public int sceneToLoad = 0;
}