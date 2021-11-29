using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorController : MonoBehaviour
{

    void OnTriggerEnter2D(Collider2D other){
        print("floor sent reset to gm");
        if(other.CompareTag("Player")){
            // gm.ResetLevel();
            GameObject.FindObjectOfType<GameManager>().ResetLevel();
        }
    }
}
