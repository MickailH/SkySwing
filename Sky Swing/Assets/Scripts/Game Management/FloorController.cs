using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorController : MonoBehaviour
{
    public GameManager gm;
    void OnTriggerEnter2D(Collider2D other){
        print("sent reset to gm");
        if(other.CompareTag("Player")){
            gm.ResetLevel();
        }
    }
}
