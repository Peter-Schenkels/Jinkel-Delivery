using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Levels
{
    public uint currentLevel = 1;
    public WorldGeneration world;
    List<PlayerStats.PlayerStatsEnd> statsList;
    bool levelEnded = false;
    public WindowManager windowManager;

    public Levels(WorldGeneration world)
    {
        this.world = world;
        world.setInitialSpawnPoints();
        statsList = new List<PlayerStats.PlayerStatsEnd>();
    }

    public void GoNextLevel(PlayerStats.PlayerStatsEnd stats)
    {
        currentLevel++;
        statsList.Add((PlayerStats.PlayerStatsEnd)stats.Clone());
        stats = new PlayerStats.PlayerStatsEnd();
        start();
    }

    public void ReplayLevel()
    {
        start();
    }

    public void start()
    {
        PlayerStats.GameDifficulty difficulty = DetermineDifficulty(currentLevel);
        world.StartShift(difficulty);
    }

    private PlayerStats.GameDifficulty DetermineDifficulty(uint level)
    {
        PlayerStats.GameDifficulty returnObject = new PlayerStats.GameDifficulty();
        returnObject.levelDifficulty = level;
        returnObject.WorldSize = (uint)(150f + 100f * (level + 1) * 0.5f);
        return returnObject;
    }

};


public class PlayerStats : MonoBehaviour
{
    public class PlayerStatsEnd
    {

        public int nrOfPizzaNotInTime;
        public int nrOfMissedOrders;
        public int nrOfDroppedPizza;
        public int nrOfDeliveredOrders;
        public float wage;
        public float profit;
        public uint levelNr = 0;

        public object Clone()
        {
            return this.MemberwiseClone();
        }

    };

    public class GameDifficulty
    {
        public uint WorldSize;
        public float levelDifficulty;
    };

    public Text timeText;
    public Text moneyText;

    public int nrOfHouses = 0;
    public int nrOfPizzaNotInTime = 0;
    public int nrOfMissedOrders = 0;
    public int nrOfDroppedPizza = 0;
    public int nrOfDeliveredOrders = 0;
    public float difficulty;
    public float wage = 0;
    public float profit = 0;
    PlayerStatsEnd stats;
    public bool worldGenerated = false;
    public WorldGeneration world;
    public WindowManager windowManager;
    public bool gameEnded = true;
    public AudioSource mainMenuTheme;
    public float time = 0;

    public ScoreBoard scoreBoard;
    public movement playerMovement;
    public movement_scooter playerScooter;
    public Levels levels;
    public Image mainMenu;

    // Start is called before the first frame update
    void Start()
    {
        levels = new Levels(world);
        levels.windowManager = windowManager;
        levels.start();
    }

    // Update is called once per frame
    void Update() 
    {
        if (!Input.GetMouseButtonDown(0) && Input.anyKeyDown && mainMenu.enabled && worldGenerated)
        {
            mainMenu.enabled = false;
            gameEnded = false;
            world.GameStart();
            mainMenuTheme.Stop();
        }
        if (!mainMenu.enabled)
        {
            displayTime();
            displayMoney();
            if(Input.GetKeyDown(KeyCode.G))
            {
                time += 10;
            }
        }
    }

    float calculateWage()
    {
        wage = profit * 0.2f - nrOfMissedOrders;
        return wage;
    }

    public void addMoney(int money)
    {
        this.profit += money;
    }

    void displayTime()
    {
        time += Time.deltaTime;
        int hours = (int)(18 + time / 60);
        int minutes = (int)(time % 60);
        if (minutes < 10)
        {
            timeText.text = hours + ":0" + minutes;
        }
        else
        {
            timeText.text = hours + ":" + minutes;
        }
        if(hours == 20)
        {
            nrOfMissedOrders = world.EndShift();
            gameEnded = true;
            stats = new PlayerStatsEnd();
            stats.nrOfPizzaNotInTime = nrOfPizzaNotInTime;
            stats.nrOfMissedOrders = nrOfMissedOrders;
            stats.nrOfDroppedPizza = nrOfDroppedPizza;
            stats.nrOfDeliveredOrders = nrOfDeliveredOrders;
            stats.wage = calculateWage();
            stats.profit = profit;
            stats.levelNr = levels.currentLevel;
            if(wage > 0)
            {
                scoreBoard.continueButton.GetComponentInChildren<Text>().text = "Next Level";
            }
            else
            {
                scoreBoard.continueButton.GetComponentInChildren<Text>().text = "Replay Level";
            }

            scoreBoard.showScore(stats);
            playerMovement.enabled = false;
            playerScooter.enabled = false;
            

        }
    }

    public void LevelContinue()
    {
        nrOfPizzaNotInTime  = 0;
        nrOfMissedOrders    = 0;
        nrOfDroppedPizza    = 0;
        nrOfDeliveredOrders = 0;
        profit              = 0;
        time                = 0;
        scoreBoard.hideScore();
        if (wage > 0)
        {
            levels.GoNextLevel(stats);
            gameEnded = false;
        }
        else
        {
            levels.ReplayLevel();
            gameEnded = false;
        }
        playerMovement.enabled = true;
        playerScooter.enabled = true;
    }

    void displayMoney()
    {
        moneyText.text = profit.ToString();
    }
}
