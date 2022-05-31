using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scooterTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    movement_scooter scooterScript;
    void Start()
    {
        scooterScript = transform.parent.GetComponent<movement_scooter>();
    }

    private void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        scooterScript.PlayerOnScooterCheck(collision);
    }
}
