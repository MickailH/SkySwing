using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    public Rigidbody2D rb;
    public TrajectoryController trajectory;
    public SpringJoint2D joint;

    public LineRenderer grappleLine;
    public Vector2 hookPos;
    private Vector2 hookDir;
    private float hookThrowDist;
    public bool retracting;
    public float retractionSpeed;
    public bool boosting;
    public SwingState state; 
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        trajectory = GetComponent<TrajectoryController>();
    }

    // Update is called once per frame
    void Update()
    {
        trajectory.UpdatePlot(rb.position, rb.velocity);

        switch (state)
        {
            case SwingState.Flying:
                if(Input.GetMouseButtonDown(0)){
                    // hookDir = (getMousePos()-rb.position).normalized;
                    // Hook();
                    hookPos = getMousePos();
                    AttachHook(hookPos);
                    state = SwingState.Hooking;
                }     
            break;

            case SwingState.Hooking:
                grappleLine.SetPosition(0, rb.position);   
                if(Input.GetMouseButtonUp(0)){
                    state = SwingState.Swinging;
                }
            break;

            case SwingState.Swinging:
                grappleLine.SetPosition(0, rb.position);
                if(Input.GetMouseButtonDown(0)) retracting = true;

                if(Input.GetMouseButtonUp(0)){
                    state = SwingState.Flying;
                    retracting = false;
                    DeattachHook();
                }

            break;
            
            default:
            break;
        }

        boosting = Input.GetMouseButton(1);

        // if(Input.GetMouseButton(1)){
        //     boosting = true;
        // }

        if(retracting) joint.distance -= retractionSpeed * Time.deltaTime;
    }

    public void Hook(){
        // Vector2 hookPos = Physics2D.Linecast(rb.position, rb.position + hookDir * hookThrowDist).point;
        RaycastHit2D hit = Physics2D.Linecast(rb.position, rb.position + hookDir * hookThrowDist);
        if(hit){
            state = SwingState.Hooking;
            hookPos = Physics2D.Linecast(rb.position, rb.position + hookDir * hookThrowDist).point;
            grappleLine.enabled = true;
            grappleLine.SetPosition(1, hookPos);
        }
    }

    public void AttachHook(Vector2 globalPos){
        joint.enabled = true;
        joint.connectedAnchor = globalPos;// - rb.position;
        grappleLine.enabled = true;
        grappleLine.SetPosition(1, globalPos);
    }

    public void DeattachHook(){
        joint.enabled = false;
        grappleLine.enabled = false;
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
    Swinging
}