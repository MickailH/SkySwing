using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    public GameManager gm;
    void OnTriggerEnter2D(Collider2D other){
        print("LEVEL shift level");
        if(other.CompareTag("Player")){
            gm.ShiftLevel();
        }
    }
}
