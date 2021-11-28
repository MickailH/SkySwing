using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Rigidbody2D playerRB;
    public GameObject hor;
    public GameObject ver;



    public Transform spawnLine;
    public Transform wall;

    public Transform tiles;
    public int tilePointer;

    // Start is called before the first frame update
    void Start()
    {
        
        // for (int i = 0; i < 10; i++)
        // {
        //     SpawnTile();
        // }
    }

    public void ShiftLevel(){
        print("gm shift level");
        Vector3 shiftAmount = Vector3.right * 50f;
        spawnLine.position += shiftAmount;
        wall.position += shiftAmount;
        SpawnTile();
    }

    public void SpawnTile(){
        int offset = 2;
        Instantiate(tilePointer%2 == 0 ? hor : ver, new Vector2(50 * (tilePointer-offset), 75f), Quaternion.identity, tiles);
        tilePointer++;
    }

}
