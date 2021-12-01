using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    public Slider slider;
    public Rigidbody2D rb;
    public TrajectoryController trajectory;
    public SpringJoint2D joint;

    public LineRenderer grappleLine;
    private Vector2 hookPos;
    private Vector2 hookDir;
    private float hookThrowDist;
    public float energyAmount;
    public bool retracting;
    public float retractionSpeed;
    public float retractUseRate;
    public bool boosting;
    public float boostAccel;
    public float boostUseRate;

    public SwingState state; 
    void Start()
    {
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
                    state = SwingState.Grappling;
                }     
            break;

            case SwingState.Grappling:
                grappleLine.SetPosition(0, rb.position);   
                if(Input.GetMouseButtonUp(0)){
                    state = SwingState.Swinging;
                }
            break;

            case SwingState.Swinging:
                grappleLine.SetPosition(0, rb.position);
                if(Input.GetMouseButtonDown(0) && energyAmount > 0) retracting = true;

                if(Input.GetMouseButtonUp(0)){
                    state = SwingState.Flying;
                    retracting = false;
                    DeattachHook();
                }
            break;
            
            default:
            break;
        }

        if(retracting) Retract();
        if(Input.GetMouseButton(1)) TryBoosting();  boosting = false;
    }

    void FixedUpdate(){
        if(boosting) rb.velocity += rb.velocity.normalized * boostAccel * Time.deltaTime;
    }

    public void TryBoosting(){
        if (energyAmount > 0){
            ChangeEnergy(-boostUseRate * Time.deltaTime);
            boosting = true;
        }
    }

    public void Retract(){
        if (energyAmount > 0){
            ChangeEnergy(-retractUseRate * Time.deltaTime);
            joint.distance -= retractionSpeed * Time.deltaTime;
        }
    }

    public void ChangeEnergy(float boostChange){
        energyAmount = Mathf.Clamp(energyAmount + boostChange, 0, 100);
        slider.value = energyAmount;
    }

    public void Hook(){
        RaycastHit2D hit = Physics2D.Linecast(rb.position, rb.position + hookDir * hookThrowDist);
        if(hit){
            state = SwingState.Grappling;
            hookPos = Physics2D.Linecast(rb.position, rb.position + hookDir * hookThrowDist).point;
            grappleLine.enabled = true;
            grappleLine.SetPosition(1, hookPos);
        }
    }

    public void AttachHook(Vector2 globalPos){
        joint.connectedAnchor = globalPos;
        joint.enabled = true;

        grappleLine.SetPosition(0, rb.position);
        grappleLine.SetPosition(1, globalPos);
        grappleLine.enabled = true;
    }

    public void DeattachHook(){
        joint.enabled = false;
        grappleLine.enabled = false;
    }

    public void ResetPlayer(Vector2 resetPos){
        DeattachHook();
        rb.velocity = Vector2.zero;
        rb.position = resetPos;
        ChangeEnergy(100-energyAmount);   
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
    Grappling,
    Swinging
}