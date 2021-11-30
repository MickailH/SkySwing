using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public TileManager tileManager;
    public GameObject playerPrefab;
    public Transform playerSpawnLocation;
    public Transform spawnLineSpawn;
    public Transform wallSpawn;
    public Transform spawnLine;
    public Transform wall;
    public Rigidbody2D playerRB;



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
        // Destroy(playerRB.gameObject);
        // playerRB = Instantiate(playerPrefab, playerSpawnLocation.position, Quaternion.identity).GetComponent<Rigidbody2D>();
        tileManager.ResetTiles();
        playerRB.GetComponent<PlayerController>().ResetPlayer(playerSpawnLocation.position);
        spawnLine.position = spawnLineSpawn.position;
        wall.position = wallSpawn.position;
    }
}
