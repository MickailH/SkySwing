using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrajectoryExample : MonoBehaviour
{
    public float power = 5f;   

    Rigidbody2D rb;
    LineRenderer lr;

    Vector2 DragStartPos;
    Vector2 DragEndPos;

    Vector2 velocity;

    Vector2[] trajectory;
    Vector3[] positions;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        lr = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0)){
            DragStartPos = getMousePos();
        }

        if(Input.GetMouseButton(0)){
            DragEndPos = getMousePos();
            velocity = (DragEndPos - DragStartPos) * power;

            trajectory = Plot(rb, (Vector2) transform.position, velocity, 500);
            lr.positionCount = trajectory.Length;

            positions = new Vector3[trajectory.Length];
            for (int i = 0; i < trajectory.Length; i++)
            {
                positions[i] = trajectory[i];
            }

            lr.SetPositions(positions);
            
        }
    
        if(Input.GetMouseButtonUp(0)){
            DragEndPos = getMousePos();
            velocity = (DragEndPos - DragStartPos) * power;
        }


    }

    public static Vector2[] Plot(Rigidbody2D rigidbody, Vector2 pos, Vector2 velocity, int steps)
    {
        Vector2[] results = new Vector2[steps];
    
        float timestep = Time.fixedDeltaTime / Physics2D.velocityIterations;
        Vector2 gravityAccel = Physics2D.gravity * rigidbody.gravityScale * timestep * timestep;
        float drag = 1f - timestep * rigidbody.drag;
        Vector2 moveStep = velocity * timestep;
    
        for (int i = 0; i < steps; ++i)
        {
            moveStep += gravityAccel;
            moveStep *= drag;
            pos += moveStep;
            results[i] = pos;
        }
    
        return results;
    }

    public Vector2 getMousePos()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.nearClipPlane;
        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(mousePos);

        return worldPosition;
    }
}
