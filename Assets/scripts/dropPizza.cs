using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class dropPizza : MonoBehaviour
{
    Button button;
    public GameObject pizza;
    PlayerStats playerStats;
    public GameObject player;
    public AudioClip dropPizzaClip;
    public AudioClip noClip;
    public GameObject pizzas;
    AudioSource audioPlayer;
    float timerTime;
    bool buyPizza = true;
    // Start is called before the first frame update
    void Start()
    {
        playerStats = GameObject.Find("PlayerStats").GetComponent<PlayerStats>();
        audioPlayer = gameObject.AddComponent<AudioSource>();
        audioPlayer.volume = 1.2f;
    }


    void updatePizzaDelayTimer()
    {
        if(timerTime < 0)
        {
            timerTime = 1.1f;
            buyPizza = true;
        }
        if(!buyPizza)
        {
            timerTime -= Time.deltaTime;
        }
    }

    void pizzaDrop()
    {
        GameObject droppedPizza = Instantiate(pizza);
        droppedPizza.transform.parent = pizzas.transform;
        droppedPizza.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -7);
        playerStats.addMoney(-3);
        audioPlayer.PlayOneShot(dropPizzaClip);
    }

    // Update is called once per frame
    void Update()
    {
        updatePizzaDelayTimer();
        if (gameObject.transform.parent.parent.parent.gameObject.active)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (buyPizza)
                {
                    pizzaDrop();
                    buyPizza = false;
                }
                else
                {
                    audioPlayer.PlayOneShot(noClip);
                }
            }
            
        }
    }
}
