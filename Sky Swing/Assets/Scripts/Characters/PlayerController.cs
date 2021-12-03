using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called before the first frame update
    public ParticleSystem partSys;
    public Slider slider;
    public Rigidbody2D rb;
    public TrajectoryController trajectory;
    public SpringJoint2D joint;

    public LineRenderer grappleLine;
    private Vector2 hookPos;
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
        BoostPartOff();
    }

    // Update is called once per frame
    void Update()
    {
        trajectory.UpdatePlot(rb.position, rb.velocity);

        switch (state)
        {
            case SwingState.Flying:
                if(Input.GetMouseButtonDown(0)){
                    if(HookPosFromMousePos(getMousePos())){
                        AttachHook(hookPos);
                        state = SwingState.Grappling;
                    }
                }     
            break;

            case SwingState.Grappling:
                grappleLine.SetPosition(0, rb.position);   
                if(Input.GetMouseButtonUp(0))   state = SwingState.Swinging;
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

        if(retracting) Retract();
        if(Input.GetMouseButtonDown(1)) boosting = true;
        if(Input.GetMouseButtonUp(1)){
            boosting = false;
            BoostPartOff();
        }
    }

    void FixedUpdate(){
        if(boosting)  Boost();
    }

    public void ResetPlayer(Vector2 resetPos){
        DeattachHook();
        rb.velocity = Vector2.zero;
        rb.position = resetPos;
        ChangeEnergy(100-energyAmount);   
    }

    public void ChangeEnergy(float boostChange){
        energyAmount = Mathf.Clamp(energyAmount + boostChange, 0, 100);
        slider.value = energyAmount;
    }

    private void Retract(){
        if (energyAmount > 0){
            ChangeEnergy(-retractUseRate * Time.deltaTime);
            joint.distance -= retractionSpeed * Time.deltaTime;
        }
    }

    private void Boost(){
        if (energyAmount > 0){
            ChangeEnergy(-boostUseRate * Time.deltaTime);
            rb.velocity += rb.velocity.normalized * boostAccel * Time.deltaTime;
            BoostPartOn();
        }
        else BoostPartOff();
    }

    private void BoostPartOn(){
        // var emission = partSys.emission; // Stores the module in a local variable
        // emission.enabled = true; // Applies the new value directly to the Particle System
        partSys.Play();
    }

    private void BoostPartOff(){
        partSys.Pause();
    }

    private bool HookPosFromMousePos(Vector2 mousepos){
        //max dist in 16:9 is 178:100, max diagonal dist 204
        //player will almost always be in centre of the screen, max dist from 89:100 is 134
        //I want it to get the closest point to the mousepos along mouse Dir Vector that is on a building collider
        Vector2 player2mouse = mousepos - rb.position;

        List<RaycastHit2D> castResults = new List<RaycastHit2D>();
        // castResults.Add(Physics2D.Raycast(mousepos, mouse2player, mouse2player.magnitude, LayerMask.GetMask("Brick")));// distance limited to not go behind the player
        castResults.Add(Physics2D.Linecast(mousepos, rb.position, LayerMask.GetMask("Brick")));// distance limited to not go behind the player
        castResults.Add(Physics2D.Raycast(mousepos, player2mouse, 130f, LayerMask.GetMask("Brick")));
        
        var possibleCastResults =  castResults.Where(cast => cast.collider != null);
        if(possibleCastResults.Count() > 0){
            hookPos = possibleCastResults.OrderBy(cast => cast.distance).First().point;
            return true;
        }
        return false;
    }

    // private bool TempHook(Vector2 mousepos){
    //     hookPos = mousepos;
    //     return true;
    // }

    private void AttachHook(Vector2 globalPos){
        joint.connectedAnchor = globalPos;
        joint.enabled = true;

        grappleLine.SetPosition(0, rb.position);
        grappleLine.SetPosition(1, globalPos);
        grappleLine.enabled = true;
    }

    private void DeattachHook(){
        joint.enabled = false;
        grappleLine.enabled = false;
    }

    private Vector2 getMousePos()
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