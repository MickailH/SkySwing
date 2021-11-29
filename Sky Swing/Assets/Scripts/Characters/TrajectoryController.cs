using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TrajectoryController : MonoBehaviour
{

    [SerializeField] private Rigidbody2D projectile;
    [SerializeField] private LineRenderer trajectorLine;
    [SerializeField] private Transform _obstaclesParent; //parent of all obstacles that need to be simulated for collisions etc.
    public bool virtualSim;
    [SerializeField] private int _maxPhysicsFrameIterations;
    [SerializeField] private int _maxVirtualFrameIterations;


    private Scene _simulationScene;
    private PhysicsScene2D _physicsScene2D;
    private readonly Dictionary<Transform, Transform> _spawnedObjects = new Dictionary<Transform, Transform>();

    private Rigidbody2D ghostProj;

    // Start is called before the first frame update
    void Start()
    {
        projectile = GetComponent<Rigidbody2D>();

        if(!virtualSim)
        {
            InitPhysicalPlot();
        }
    }

    public void UpdatePlot(Vector2 pos, Vector2 velocity){
        if(virtualSim) VirtualPlot(pos, velocity);
        else PhysicalPlot(pos, velocity);
    }

    public void VirtualPlot(Vector2 pos, Vector2 velocity)
    {
        trajectorLine.positionCount = _maxVirtualFrameIterations;

        float timestep = Time.fixedDeltaTime / Physics2D.velocityIterations;
        Vector2 gravityAccel = Physics2D.gravity * projectile.gravityScale * timestep * timestep;
        float drag = 1f - timestep * projectile.drag;
        Vector2 moveStep = velocity * timestep;
    
        for (int i = 0; i < _maxVirtualFrameIterations; i++)
        {
            moveStep += gravityAccel;
            moveStep *= drag;
            pos += moveStep;
            trajectorLine.SetPosition(i, pos);
            //results[i] = pos;
        }
    }


    public void PhysicalPlot(Vector3 pos, Vector3 velocity){
        foreach (var item in _spawnedObjects) {
            item.Value.position = item.Key.position;
            item.Value.rotation = item.Key.rotation;
        }

        ghostProj.gameObject.SetActive(true);
        ghostProj.transform.position = pos;
        ghostProj.velocity = velocity;

        trajectorLine.positionCount = _maxPhysicsFrameIterations;
        for (var i = 0; i < _maxPhysicsFrameIterations; i++) {
            _physicsScene2D.Simulate(Time.fixedDeltaTime);
            trajectorLine.SetPosition(i, ghostProj.transform.position);
        }
        ghostProj.gameObject.SetActive(false);
    }


    public void InitPhysicalPlot()
    {
        _obstaclesParent = GameObject.Find("Obstacles").transform;
        Create2DPhysicsScene();
        ghostProj = Instantiate(projectile).GetComponent<Rigidbody2D>();
        SceneManager.MoveGameObjectToScene(ghostProj.gameObject, _simulationScene);

        Destroy(ghostProj.GetComponent<Renderer>());
        Destroy(ghostProj.GetComponent<LineRenderer>());
        Destroy(ghostProj.GetComponent<TrajectoryController>());
        Destroy(ghostProj.GetComponent<PlayerController>());

        ghostProj.gameObject.SetActive(false);
    }


    public void Create2DPhysicsScene() {
        _simulationScene = SceneManager.CreateScene("Trajectory Simulation", new CreateSceneParameters(LocalPhysicsMode.Physics2D));
        _physicsScene2D = _simulationScene.GetPhysicsScene2D();

        foreach (Transform obj in _obstaclesParent) {
            var ghostObj = Instantiate(obj.gameObject, obj.position, obj.rotation);
            ghostObj.GetComponent<Renderer>().enabled = false;
            SceneManager.MoveGameObjectToScene(ghostObj, _simulationScene);
            if (!ghostObj.isStatic) _spawnedObjects.Add(obj, ghostObj.transform);
        }  
    }
}