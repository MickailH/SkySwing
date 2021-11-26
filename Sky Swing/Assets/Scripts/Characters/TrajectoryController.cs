using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TrajectoryController : MonoBehaviour
{
    // public bool project;
    [SerializeField] private Rigidbody2D projectile;
    [SerializeField] private Transform _obstaclesParent; //parent of all obstacles that need to be simulated for collisions etc.
    [SerializeField] private LineRenderer _line;
    [SerializeField] private int _maxPhysicsFrameIterations;


    private Scene _simulationScene;
    private PhysicsScene2D _physicsScene2D;
    private readonly Dictionary<Transform, Transform> _spawnedObjects = new Dictionary<Transform, Transform>();

    private Rigidbody2D ghostProj;

    // Start is called before the first frame update
    void Start()
    {
        projectile = GetComponent<Rigidbody2D>();
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


    void Update()
    {
        // PhysicalPlot(projectile, projectile.position, projectile.velocity);
    }

    public void PhysicalPlot(Vector3 pos, Vector3 velocity){
        foreach (var item in _spawnedObjects) {
            item.Value.position = item.Key.position;
            item.Value.rotation = item.Key.rotation;
        }

        ghostProj.gameObject.SetActive(true);
        ghostProj.transform.position = pos;
        ghostProj.velocity = velocity;

        _line.positionCount = _maxPhysicsFrameIterations;
        for (var i = 0; i < _maxPhysicsFrameIterations; i++) {
            _physicsScene2D.Simulate(Time.fixedDeltaTime);
            _line.SetPosition(i, ghostProj.transform.position);
        }
        ghostProj.gameObject.SetActive(false);
    }

    private void Create2DPhysicsScene() {
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