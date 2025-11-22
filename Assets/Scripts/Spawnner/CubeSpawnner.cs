using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Rendering;

public class CubeSpawnner : MonoBehaviour
{
    [SerializeField] private GameObject platformPrefab;
    private bool canSpawnCube;
    [SerializeField] EventSO eventSO;
    int count = 0;


    [Header("COLOR VARIABLES")]
    public List<Gradient> gradientList;
    private Gradient currentGRadient;
    public float gradientPosition = 0;
    public float colorStep = 0.02f;

    [SerializeField]MeshRenderer startCube;

    private void Start()
    {
        HandleGradient();
        startCube.material.color = currentGRadient.Evaluate(Random.Range(0, 1));
    }
    void HandleGradient()
    {
        currentGRadient = gradientList[Random.Range(0, gradientList.Count)];
    }


    private void Awake()
    {
        eventSO.OnGameStarted += EventSO_OnGameStarted;
        eventSO.OnGameEnded += EventSO_OnGameEnded;
        eventSO.OnScoreIncrement += EventSO_OnScoreIncrement;
    }
    private void OnDestroy()
    {
        eventSO.OnGameStarted -= EventSO_OnGameStarted;
        eventSO.OnGameEnded -= EventSO_OnGameEnded;
        eventSO.OnScoreIncrement -= EventSO_OnScoreIncrement;
    }

    private void EventSO_OnSpawnRequest()
    {
        if (canSpawnCube)
        {
            SpawnNewBlock();
        }
    }

    void SpawnNewBlock()
    {
        float spawnPositionY = 0;
        if (count == 0)
        {
            spawnPositionY = 0.63f;
        }
        else
        {
            spawnPositionY = MovingCube.currentCube.transform.position.y + platformPrefab.transform.localScale.y;
        }

        if (count % 2 == 0)
        {
            Vector3 spawnPosition = new Vector3(MovingCube.previousCube.transform.position.x, spawnPositionY, 1.45f);
            var obj = Instantiate(platformPrefab, spawnPosition, Quaternion.identity);
            if (count != 0)
            {
                obj.transform.localScale = new Vector3(MovingCube.previousCube.transform.localScale.x, MovingCube.previousCube.transform.localScale.y, MovingCube.previousCube.transform.localScale.z);
            }
            var movingCube = obj.GetComponent<MovingCube>();
            movingCube.cubeType = cubeSpawnType.zaxis;
            movingCube.material.color = SetColor();
        }
        else
        {
            Vector3 spawnPosition = new Vector3(-1.45f, spawnPositionY, MovingCube.previousCube.transform.position.z);
            var obj = Instantiate(platformPrefab, spawnPosition, Quaternion.identity);
            obj.transform.localScale = new Vector3(MovingCube.previousCube.transform.localScale.x, MovingCube.previousCube.transform.localScale.y, MovingCube.previousCube.transform.localScale.z);
            var movingCube = obj.GetComponent<MovingCube>();
            movingCube.cubeType = cubeSpawnType.xaxis;
            movingCube.material.color = SetColor();
        }

    }

    Color SetColor()
    {
        gradientPosition += colorStep;
        if (gradientPosition > 1)
        {
            gradientPosition = 0;
            HandleGradient();
        }
        return currentGRadient.Evaluate(gradientPosition);
    }

    private void EventSO_OnGameEnded()
    {
        canSpawnCube = false;
    }

    private void EventSO_OnGameStarted()
    {
        canSpawnCube = true;
        EventSO_OnSpawnRequest();
    }
    private void EventSO_OnScoreIncrement()
    {
        canSpawnCube = true;
        count++;
        EventSO_OnSpawnRequest();
    }
}
