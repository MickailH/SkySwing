using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    public GameObject brick;
    public GameObject window;
    
    // Start is called before the first frame update
    void Start()
    {
        MakeTile();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MakeTile(){

        int buildWidth = 30;
        int buildDir = 1;
        int windChance = 100;
        int buildheight1 = 40;
        int buildpointer = buildDir == 1  ? 0 : 100;

        Instantiate(brick, new Vector2(0, buildpointer + buildDir * buildheight1 / 2), Quaternion.identity).transform.localScale = new Vector3(buildWidth, buildheight1, 1);
        buildpointer+=buildheight1;
        
        if(Random.Range(1,101) < windChance){
            int windHeight = 20;

            Instantiate(window, new Vector2(0, buildpointer + buildDir * windHeight / 2), Quaternion.identity).transform.localScale = new Vector3(buildWidth, windHeight, 1);
            buildpointer += windHeight;
            Instantiate(brick, new Vector2(0, buildpointer + buildDir * buildheight1 / 2), Quaternion.identity).transform.localScale = new Vector3(buildWidth, buildheight1, 1);
        }
    }
}
