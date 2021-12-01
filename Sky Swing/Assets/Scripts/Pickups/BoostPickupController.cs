using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostPickupController : MonoBehaviour
{
    public float energyAmount;

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            Pickup(other.GetComponent<PlayerController>());
        }
    }

    public void Pickup(PlayerController playerCol)
    {
        playerCol.GetComponent<PlayerController>().ChangeEnergy(energyAmount);
        Destroy(gameObject);

    }

}
