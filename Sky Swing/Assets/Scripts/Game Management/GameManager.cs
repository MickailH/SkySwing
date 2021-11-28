using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public TileManager tileManager;
    public Rigidbody2D playerRB;
    public Transform playerSpawnLocation;

    public Transform spawnLineSpawn;
    public Transform wallSpawn;

    public Transform spawnLine;
    public Transform wall;



    // Start is called before the first frame update
    void Start()
    {
        tileManager.InitLevel();
    }

    public void ShiftLevel(){
        Vector3 shiftAmount = Vector3.right * 50f;
        spawnLine.position += shiftAmount;
        wall.position += shiftAmount;
        tileManager.AddTile();
    }

    public void ResetLevel(){
        foreach (Transform tile in tileManager.tiles)
        {
            Destroy(tile.gameObject);
        }
        playerRB.position = playerSpawnLocation.position;
        playerRB.velocity = Vector2.zero;
        spawnLine.position = spawnLineSpawn.position;
        wall.position = wallSpawn.position;
        tileManager.InitLevel();
    }
}
