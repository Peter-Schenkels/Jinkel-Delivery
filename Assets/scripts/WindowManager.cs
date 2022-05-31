using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;



public class WindowManager : MonoBehaviour
{
    public Window windowLeftUp;
    public Window windowRightUp;
    public Window windowLeftDown;
    public Window windowRightDown;

    public float difficulty;
    public float worldSize;
    public PlayerStats playerStats;

    public float worldCenter;

    public float spawnLikeliness;   // Everytime Spawnrate is called this percentage is how likely it is to be spawned.
    public float spawnRate;         // in minutes

    float timer = 0f;

    public void determineWindowStats()
    {
        windowLeftDown = transform.GetChild(0).gameObject.GetComponent<Window>();
        windowRightDown = transform.GetChild(1).gameObject.GetComponent<Window>();
        windowLeftUp = transform.GetChild(2).gameObject.GetComponent<Window>();
        if(transform.childCount > 3)
        {
            windowRightUp = transform.GetChild(3).gameObject.GetComponent<Window>();
        }
        determineStatsFromDifficulty();
    }

    public void destroy()
    {
        Destroy(windowLeftDown);
        Destroy(windowRightDown);
        Destroy(windowLeftUp);
        Destroy(windowRightUp);
    }


    void determineStatsFromDifficulty()
    {
        if(playerStats != null)
        {
            spawnLikeliness = ((5f / 60f) * (playerStats.difficulty / playerStats.nrOfHouses)) * 50f;
            spawnRate = UnityEngine.Random.Range(5, 20);
        }
        else
        {
            playerStats = GameObject.Find("PlayerStats").GetComponent<PlayerStats>();
            if(playerStats != null)
            {
                determineStatsFromDifficulty();
            }
            else
            {
                Debug.LogError("No player stats");
            }
        }

    }

    void updateTimer()
    {
        if (!playerStats.gameEnded)
        {
            if (timer <= 0)
            {
                timer = spawnRate;
                determineWindowPizza(windowLeftDown);
                determineWindowPizza(windowLeftUp);
                determineWindowPizza(windowRightDown);
                determineWindowPizza(windowRightUp);
            }
            else
            {
                timer -= Time.deltaTime;
            }
        }
    }

    private void Start()
    {
        playerStats = GameObject.Find("PlayerStats").GetComponent<PlayerStats>();
    }

    void determineWindowPizza(Window window)
    {
        if(window != null)
        {

            if(UnityEngine.Random.Range(0, 100) < spawnLikeliness && !window.wantPizza)
            {
                window.orderPizza(45 + UnityEngine.Random.Range(0, 2));
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        updateTimer();
    }

    public int endOfShift()
    {
        if (transform.childCount == 3)
        {
            return Convert.ToInt32(windowLeftDown.wantPizza) + Convert.ToInt32(windowLeftUp.wantPizza) + Convert.ToInt32(windowRightDown.wantPizza);
        }
        else
        {
            return Convert.ToInt32(windowLeftDown.wantPizza) + Convert.ToInt32(windowLeftUp.wantPizza) + Convert.ToInt32(windowRightDown.wantPizza) + Convert.ToInt32(windowRightUp.wantPizza);
        }
    }
}
