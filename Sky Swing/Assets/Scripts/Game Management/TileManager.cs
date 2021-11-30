using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    public GameObject brick;
    public GameObject window;
    public GameObject boostPrefab;
    public GameObject background;
    public GameObject floor;
    public Transform tiles;
    public Transform floors;
    public Transform backgrounds;
    public int tilePointer;

    public int windowChance;
    public int BuildUpChance;

    public int buildWidthMin;
    public int buildWidthMax;

    public int brickLenMin1;
    public int brickLenMax1;

    public int WinLenMin;
    public int WinLenMax;

    public int brickLenMin2;
    public int brickLenMax2;


    public void InitLevel(){
        for (int i = 0; i < 10; i++)    AddTile();
    }

    public void AddTile(){
        Transform temp;
        temp = (new GameObject(tilePointer.ToString())).transform;
        temp.SetParent(tiles);
        MakeBuilding(tilePointer, temp);
        MakeBackground(tilePointer, backgrounds);
        MakeFloor(tilePointer, floors);
        tilePointer++;
    }

    public void ResetTiles(){
        foreach (Transform tile in tiles)   Destroy(tile.gameObject);
        foreach (Transform floor in floors)   Destroy(floor.gameObject);
        foreach (Transform background in backgrounds)   Destroy(background.gameObject);
        tilePointer = 2;
        InitLevel();
    }

    public Transform MakeBuilding(int tilePointer, Transform tileParent){
        int buildDir = Random.Range(0,100) < BuildUpChance ? 1:-1;
        float buildpointer = buildDir == 1  ? -50 : 50;
        int buildWidth = Random.Range(buildWidthMin, buildWidthMax);

        float buildLen1 = Random.Range(brickLenMin1, brickLenMax1);
        Instantiate(brick, new Vector2(50 * (tilePointer), buildpointer + buildDir * buildLen1 / 2), Quaternion.identity, tileParent).transform.localScale = new Vector3(buildWidth, buildLen1, 1);
        buildpointer+= buildLen1 * buildDir;
        
        // if(Random.Range(0,100) < windowChance){
        if(tilePointer % 2 == 0){
            float windLen = Random.Range(WinLenMin, WinLenMax);
            Vector2 WindowPos = new Vector2(50 * (tilePointer), buildpointer + buildDir * windLen / 2);
            Instantiate(window, WindowPos, Quaternion.identity, tileParent).transform.localScale = new Vector3(buildWidth, windLen, 1);
            Instantiate(boostPrefab, WindowPos, Quaternion.identity, tileParent);
            buildpointer += windLen * buildDir;
            
        //     float buildLen2 = Random.Range(brickLenMin2, brickLenMax2);
        //     Instantiate(brick, new Vector2(50 * (tilePointer-2), buildpointer + buildDir * buildLen2 / 2), Quaternion.identity, tileParent).transform.localScale = new Vector3(buildWidth, buildLen2, 1);
        }
        float buildLen2 = Random.Range(brickLenMin2, brickLenMax2);
        Instantiate(brick, new Vector2(50 * (tilePointer), buildpointer + buildDir * buildLen2 / 2), Quaternion.identity, tileParent).transform.localScale = new Vector3(buildWidth, buildLen2, 1);
        
        return tileParent;
    }

    public void MakeBackground(int tilePointer, Transform tileParent){
        Instantiate(background, new Vector2(50 * tilePointer, 0), Quaternion.identity, tileParent);
    }

    public void MakeFloor(int tilePointer, Transform tileParent){
        Instantiate(floor, new Vector2(50 * tilePointer, -50), Quaternion.identity, tileParent);
    }
}
