using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    public GameObject brick;
    public GameObject window;
    public Transform tiles;
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


    
    // Start is called before the first frame update
    void Start()
    {


    }

    public void InitLevel(){
        for (int i = 0; i < 10; i++)    AddTile();
    }

    public void AddTile(){
        Transform temp;
        temp = (new GameObject(tilePointer.ToString())).transform;
        temp.SetParent(tiles);
        MakeTile(tilePointer, temp);
        tilePointer++;
    }

    public Transform MakeTile(int tilePointer, Transform tileParent){
        int buildDir = Random.Range(0,100) < BuildUpChance ? 1:-1;
        float buildpointer = buildDir == 1  ? 0 : 100;
        int buildWidth = Random.Range(buildWidthMin, buildWidthMax);

        float buildLen1 = Random.Range(brickLenMin1, brickLenMax1);
        Instantiate(brick, new Vector2(50 * (tilePointer-2), buildpointer + buildDir * buildLen1 / 2), Quaternion.identity, tileParent).transform.localScale = new Vector3(buildWidth, buildLen1, 1);
        buildpointer+= buildLen1 * buildDir;
        
        // if(Random.Range(0,100) < windowChance){
        if(tilePointer % 2 == 0){
            float windLen = Random.Range(WinLenMin, WinLenMax);
            Instantiate(window, new Vector2(50 * (tilePointer-2), buildpointer + buildDir * windLen / 2), Quaternion.identity, tileParent).transform.localScale = new Vector3(buildWidth, windLen, 1);
            buildpointer += windLen * buildDir;
            
        //     float buildLen2 = Random.Range(brickLenMin2, brickLenMax2);
        //     Instantiate(brick, new Vector2(50 * (tilePointer-2), buildpointer + buildDir * buildLen2 / 2), Quaternion.identity, tileParent).transform.localScale = new Vector3(buildWidth, buildLen2, 1);
        }
        float buildLen2 = Random.Range(brickLenMin2, brickLenMax2);
        Instantiate(brick, new Vector2(50 * (tilePointer-2), buildpointer + buildDir * buildLen2 / 2), Quaternion.identity, tileParent).transform.localScale = new Vector3(buildWidth, buildLen2, 1);
        
        return tileParent;
    }
}
