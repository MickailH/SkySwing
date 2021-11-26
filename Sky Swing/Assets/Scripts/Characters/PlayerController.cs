using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update

    public TrajectoryController trajectory;

    public LineRenderer grapple;
    public Rigidbody2D rb;
    public SwingState state; 
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        trajectory = GetComponent<TrajectoryController>();
    }

    // Update is called once per frame
    void Update()
    {
        trajectory.PhysicalPlot(rb.position, rb.velocity);

        switch (state)
        {
            case SwingState.Flying:
            if(Input.GetMouseButtonDown(0)) Hook(getMousePos());      
            break;

            case SwingState.Hooking:            
            break;

            case SwingState.Retracting:           
            break;
            
            default:
            break;
        }

    }

    public void Hook(Vector2 hookPos){
        
    }

    public Vector2 getMousePos()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.nearClipPlane;
        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(mousePos);

        return worldPosition;
    }
}


public enum SwingState {
    Flying,
    Hooking,
    Retracting,
}