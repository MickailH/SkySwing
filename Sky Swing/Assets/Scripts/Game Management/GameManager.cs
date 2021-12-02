using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public TileManager tileManager;
    public GameObject playerPrefab;
    public Transform CameraConfiner;
    public Transform ConfinerSpawn;
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

    void Update(){
        if(Input.GetKeyDown(KeyCode.Escape)) ResetLevel();
    }

    public void ShiftLevel(){
        print("GM: shift level");
        Vector3 shiftAmount = Vector3.right * 50f;
        spawnLine.position += shiftAmount;
        wall.position += shiftAmount;
        CameraConfiner.position += shiftAmount;
        tileManager.AddTile();
        tileManager.DeleteOldTile();
    }

    public void ResetLevel(){
        print("GM: Reset Level");
        CameraConfiner.position = ConfinerSpawn.position;
        tileManager.ResetTiles();
        playerRB.GetComponent<PlayerController>().ResetPlayer(playerSpawnLocation.position);
        spawnLine.position = spawnLineSpawn.position;
        wall.position = wallSpawn.position;
    }
}
