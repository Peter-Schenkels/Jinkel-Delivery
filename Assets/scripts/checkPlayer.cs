using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checkPlayer : MonoBehaviour
{
    public GameObject PizzaUI;
    public GameObject Scooter;
    movement_scooter stats;
    public PlayerStats playerStats;

    // Start is called before the first frame update
    void Start()
    {
       stats = Scooter.GetComponent<movement_scooter>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && !playerStats.mainMenu.enabled)
        {
            if (!stats.PlayerOnScooter)
            {
                PizzaUI.active = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PizzaUI.active = false;
        }
    }
}
