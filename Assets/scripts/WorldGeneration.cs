using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class WorldGeneration : MonoBehaviour
{
    public float difficulty = 1f;
    public SpriteShapeController worldShape;
    public float startingAreaSize;
    
    public float worldSize;
    public float blockSize;
    public float heightConstant;
    public int heightModes;

    float worldCenter;
    Vector3 startWorldPosition;
    Vector3 endWorldPosition;

    public float spawnRate = 0.5f;

    public GameObject pizzaPalace;
    public GameObject player;
    public GameObject scooter;
    public GameObject camera;
    public GameObject housesGameObject;
    public GameObject treesOneGameObject;
    public GameObject treesTwoGameObject;
    public GameObject treesThreeGameObject;
    public GameObject pizzas;

    Vector3 pizzaPalaceInitialPosition;
    Vector3 playerInitialPosition;
    Vector3 scooterInitialPosition;
    Vector3 cameraInitialPosition;

    public AudioSource mainMusic;

    public List<GameObject> housePrefabs;
    public List<GameObject> treePrefabs;
    public Vector3 houseOffset;
    public Vector3 treeOffset;

    public float orderRate;
    public PlayerStats playerStats;
    public float terrainLayerPosition { get; private set; } = -4.5f;

    public List<GameObject> houses;
    bool cameraReset = false;
    bool cameraStart = false;

    int magicNumber { get; } = 0;


    // Start is called before the first frame update
    void Start()
    {
        worldShape = GetComponent<SpriteShapeController>();
    }

    public void WorldClean()
    {
        housesGameObject.SetActive(false);
        treesOneGameObject.SetActive(false);
        Destroy(housesGameObject);
        Destroy(treesOneGameObject);
        treesOneGameObject = Instantiate(new GameObject());
        housesGameObject = Instantiate(new GameObject());
        int childCount = pizzas.transform.childCount;
        for (int i = childCount-1; i >= 0; i--) 
        {
            Destroy(pizzas.transform.GetChild(i).gameObject);
        }
    }

    public void setInitialSpawnPoints()
    {
        pizzaPalaceInitialPosition = new Vector3(pizzaPalace.transform.position.x, pizzaPalace.transform.position.y, pizzaPalace.transform.position.z);
        playerInitialPosition = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z);
        scooterInitialPosition = new Vector3(scooter.transform.position.x, scooter.transform.position.y, scooter.transform.position.z);
        cameraInitialPosition = new Vector3(camera.transform.position.x, camera.transform.position.y, camera.transform.position.z);
    }

    void WorldStart(float difficulty)
    {
        houses = new List<GameObject>();
        SplineGeneration();
        playerStats.worldGenerated = true;
        if(!playerStats.mainMenu.enabled)
        {
            GameStart();
        }

    }

    public void GameStart()
    {
        setDifficulty(difficulty);
        cameraReset = true;
        player.GetComponent<PizzaHolding>().reset();
        mainMusic.Play();
    }

    public void StartShift(PlayerStats.GameDifficulty difficulty)
    {
        worldSize = difficulty.WorldSize;
        WorldClean();
        playerStats.difficulty = difficulty.levelDifficulty;
        WorldStart(difficulty.levelDifficulty);
        //Physics2D.simulationMode = SimulationMode2D.Update;
    }

    public int EndShift()
    {
        int missedDeliveries = 0;
        foreach(GameObject house in houses)
        {
            WindowManager windowManagerInstance = house.GetComponent<WindowManager>();
            missedDeliveries += windowManagerInstance.endOfShift();
        }
        return missedDeliveries;
    }


    List<Vector2> getPointsBetweenVectors(uint nrOfPoints, Vector2 a, Vector2 b)
    {
        bool onePoint = (nrOfPoints == 1);
        List<Vector2> points = new List<Vector2>();
        Vector2 incrementVector = new Vector2((b.x - a.x) / (nrOfPoints + 1), (b.y - a.y) / (nrOfPoints + 1));
        for (uint i = 1; i < nrOfPoints+1; i++)
        {
            Vector2 generatedPoint = a + incrementVector * i;
            points.Add(new Vector2(generatedPoint.x, generatedPoint.y));
        }
        return points;
    }

    void GenerateTrees(GameObject parent, List<Vector2> treePoints)
    {
        foreach (Vector3 position in treePoints)
        {
            GameObject tree = Instantiate(treePrefabs[(int)Random.Range(0, treePrefabs.Count)]);
            tree.transform.position = position + treeOffset;
            tree.transform.parent = parent.transform;
        }
    }

    void SplineGeneration()
    {
        worldShape.spline.Clear();

        Vector2 pivoter = new Vector2(-worldSize, 0);
        int nrOfBlocks = (int)(worldSize / blockSize);
        int middleGroundIndex = nrOfBlocks / 2;

        // Generaten Spline
        for(int index = 0; index < nrOfBlocks; index++)
        {
            if (index != middleGroundIndex)
            { 
                int heightFactor = Random.Range(-heightModes / 2, (heightModes+1) / 2);         // Generate Relief         
                pivoter += new Vector2(blockSize, heightConstant * heightFactor);
            }
            else
            {
                pivoter += new Vector2(blockSize, 0);                                           // Generate Spawing area
            }
            worldShape.spline.InsertPointAt(index, new Vector3(pivoter.x, pivoter.y, -2f));     // Create a new point in the spline with generated position
            worldShape.spline.SetTangentMode(index, ShapeTangentMode.Continuous);               // insert tangent to Smooth out the terrain

            //TODO: make better tangent generation for smoother terrain
            worldShape.spline.SetLeftTangent(index, new Vector3(-blockSize/4, 0, 0));
            worldShape.spline.SetRightTangent(index, new Vector3(blockSize/4, 0, 0));
        }

        // Setup spawn area
        Vector3 startPos = worldShape.spline.GetPosition(middleGroundIndex);
        worldCenter = startPos.x;
        pizzaPalace.transform.position  = pizzaPalaceInitialPosition + new Vector3(startPos.x, startPos.y, 0);
        player.transform.position       = playerInitialPosition + new Vector3(startPos.x, startPos.y, 0);
        scooter.transform.position      = scooterInitialPosition + new Vector3(startPos.x, startPos.y, 0);
        camera.transform.position       = cameraInitialPosition + new Vector3(player.transform.position.x, player.transform.position.y, 0);
        // End setup spawn area

        // Generate houses based on difficulty
        for (int index = 0; index < nrOfBlocks; index++)
        {
            if (index != middleGroundIndex && index != (middleGroundIndex-1) && index > 3 && index < (nrOfBlocks - 3))
            {
                Vector3 possibleHousePos = worldShape.spline.GetPosition(index);
                GenerateTrees(treesOneGameObject, getPointsBetweenVectors((uint)Random.Range(0, 5f), possibleHousePos, worldShape.spline.GetPosition(index + 1)));
                GameObject house = Instantiate(housePrefabs[(int)Random.Range(0, housePrefabs.Count)]);
                house.transform.position = possibleHousePos + houseOffset;
                house.transform.parent = housesGameObject.transform;
                houses.Add(house);   
            }
        }
        playerStats.nrOfHouses = houses.Count;
        // End generate houses based on difficulty
        setBorders();

        // Move terrain behind houses.
        transform.position = new Vector3(transform.position.x, transform.position.y, terrainLayerPosition);

        //Update Spline shape
        updateSplineShape();

    }

    void updateSplineShape()
    {
        worldShape.UpdateSpriteShapeParameters();
        worldShape.BakeMesh();
        worldShape.RefreshSpriteShape();
        worldShape.BakeCollider();
    }

    void setBorders()
    {
        endWorldPosition = worldShape.spline.GetPosition(3);
        startWorldPosition = worldShape.spline.GetPosition((int)(worldSize / blockSize) - 3);
        GameObject leftBorder = GameObject.Find("roadblock Left");
        GameObject rightBorder = GameObject.Find("roadblock Right");
        leftBorder.transform.position = new Vector3(endWorldPosition.x - 2*blockSize, endWorldPosition.y+ 0.5f, -6.1f);
        rightBorder.transform.position = new Vector3(startWorldPosition.x - 2*blockSize, startWorldPosition.y + 0.5f, -6.1f);
    }

    void setDifficulty(float difficulty)
    {
        this.difficulty = difficulty;
        foreach(GameObject house in houses)
        {
            WindowManager windowManagerInstance = house.GetComponent<WindowManager>();
            windowManagerInstance.worldSize     = worldSize;
            windowManagerInstance.worldCenter   = worldCenter;
            windowManagerInstance.difficulty    = difficulty;
            windowManagerInstance.spawnRate     = orderRate;
            windowManagerInstance.determineWindowStats();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (cameraReset)
        {
            camera.GetComponent<Camera>().orthographicSize = 200;
            cameraReset = false;
            cameraStart = true;
        }
        else if (cameraStart)
        {
            camera.GetComponent<Camera>().orthographicSize = 5;
            cameraStart = false;
        }
    }
}