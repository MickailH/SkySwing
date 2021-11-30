using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupController : MonoBehaviour
{
    public float fizzleTimer;
    public float activeTimer;
    protected bool active;
    protected float timer;

    void Update()
    {
        if (!active)
        {
            fizzleTimer -= Time.deltaTime;
            if (fizzleTimer <= 0) Destroy(gameObject);
        }

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController player = other.GetComponent<PlayerController>();

        if(other.CompareTag("Player"))
        {
            StartCoroutine(Pickup(other));
        }
    }


    public virtual IEnumerator Pickup(Collider2D playerCol)
    {
        yield break;
    }
    
}
