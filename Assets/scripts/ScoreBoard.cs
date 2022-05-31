using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreBoard : MonoBehaviour
{
    // Start is called before the first frame update
    public Text nrOfDroppedPizzaText;
    public Text nrOfDeliveredPizzaText;
    public Text nrOfDeliveredPizzaNotOnTimeText;
    public Text nrOfDeliveredCustomersIgnoredText;
    public Text wageText;
    public Text profitText;
    public Button continueButton;
    public PlayerStats stats;
    public Text levelNr;

    private void Start()
    {
        continueButton.onClick.AddListener(stats.LevelContinue);

    }
    public void showScore(PlayerStats.PlayerStatsEnd stats)
    {
        nrOfDroppedPizzaText.text   = "Amount of pizza's dropped: " + stats.nrOfDroppedPizza.ToString();
        nrOfDeliveredPizzaText.text = "Amount of pizza's sold: " + stats.nrOfDeliveredOrders.ToString();
        //nrOfDeliveredPizzaNotOnTimeText.text = "Amount of orders not in time: " + stats.nrOfPizzaNotInTime.ToString();
        nrOfDeliveredCustomersIgnoredText.text = "Amount of missed orders: " + stats.nrOfMissedOrders.ToString();
        wageText.text = "Wage: " + stats.wage.ToString();
        profitText.text = "Profits: " + stats.profit.ToString();
        levelNr.text = "Level: " + stats.levelNr.ToString();
        transform.GetChild(0).gameObject.active = true;
        transform.GetChild(1).gameObject.active = false;
    }

    public void hideScore()
    {
        transform.GetChild(0).gameObject.active = false;
        transform.GetChild(1).gameObject.active = true;
    }

    

}
