using System.ComponentModel;
using System.Diagnostics.Tracing;
using System.Drawing;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class MovingCube : MonoBehaviour
{
    [Range(0, 10)]
    public float moveSpeed;

    public cubeSpawnType cubeType;

    [SerializeField] Vector2 moveRange;
    private Vector3 direction;
    public static MovingCube currentCube { get; private set; }
    public static MovingCube previousCube { get; private set; }

    public Material material;


    private int perfectPlacementStreak;
    [Range(0, 30)]
    [SerializeField] private int perfectPlacementThreshold;


    private void OnEnable()
    {
        material = GetComponent<MeshRenderer>().material;
        if (previousCube == null)
        {
            previousCube = GameObject.Find("StartCube").GetComponent<MovingCube>();
        }
        currentCube = this;
    }

    internal void Stop()
    {
        moveSpeed = 0;
        if (cubeType == cubeSpawnType.zaxis)
        {
            float difference = transform.position.z - previousCube.transform.position.z;
            if (difference > previousCube.transform.localScale.z * 0.95f || previousCube.transform.localScale.z<0.03f)
            {
                GameOver();
                return;
            }
            if (Mathf.Abs(difference) * 1000 < perfectPlacementThreshold)
            {
                PerfectPlacemement();
            }
            else
            {
                perfectPlacementStreak = 0;
                float direction = difference > 0 ? 1 : -1;
                SplitCubeZaxis(difference, direction);
            }
        }
        else if (cubeType == cubeSpawnType.xaxis)
        {
            float difference = transform.position.x - previousCube.transform.position.x;
            if (difference > previousCube.transform.localScale.x * 0.95f || previousCube.transform.localScale.x < 0.03f)
            {
                GameOver();
                return;
            }
            if (Mathf.Abs(difference) * 1000 < perfectPlacementThreshold)
            {
                PerfectPlacemement();
            }
            else
            {
                perfectPlacementStreak = 0;
                float direction = difference > 0 ? 1 : -1;
                SplitCubeXaxis(difference, direction);
            }
        }
    }

    void PerfectPlacemement()
    {
        Material targetMaterial = currentCube.material;
        targetMaterial.EnableKeyword("_EMISSION");
        targetMaterial.SetColor("_EmissionColor", targetMaterial.color * 0.7f);
        DynamicGI.SetEmissive(GetComponent<Renderer>(), targetMaterial.color * 0.7f);
        SoundManager.Instance.PlayPerfectLand(perfectPlacementStreak);
        perfectPlacementStreak++;
        MovingCube.currentCube.transform.position = new Vector3(previousCube.transform.position.x, transform.position.y, previousCube.transform.position.z);
        MovingCube.currentCube.transform.localScale = new Vector3(previousCube.transform.localScale.x, transform.localScale.y, previousCube.transform.localScale.z);
        MovingCube.previousCube = this;
        Gamemanager.Instance.RaiseEventSO(3);
    }

    void GameOver()
    {
        currentCube.AddComponent<Rigidbody>();
        previousCube = null;
        Gamemanager.Instance.RaiseEventSO(1);
    }

    #region Cube Z axis
    void SplitCubeZaxis(float difference, float direction)
    {
        float size = previousCube.transform.localScale.z - Mathf.Abs(difference);
        float fallingCubeSizeZ = previousCube.transform.localScale.z - size;

        float position = previousCube.transform.position.z + (difference / 2);
        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, size);
        transform.position = new Vector3(transform.position.x, transform.position.y, position);

        float edge = transform.position.z + size / 2 * direction;
        float fallingCubePos = edge + fallingCubeSizeZ / 2 * direction;

        SpawnDropCubeZ(fallingCubePos, fallingCubeSizeZ);
    }
    void SpawnDropCubeZ(float zPosition, float zSize)
    {
        SoundManager.Instance.PlayCubeCut();
        var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, zSize);
        cube.transform.position = new Vector3(transform.position.x, transform.position.y, zPosition);
        cube.AddComponent<Rigidbody>();
        cube.GetComponent<MeshRenderer>().sharedMaterial = material;
        MovingCube.previousCube = this;
        Gamemanager.Instance.RaiseEventSO(3);
        Destroy(cube, 2f);
    }

    #endregion

    #region Cube X axis
    void SplitCubeXaxis(float difference, float direction)
    {

        float size = previousCube.transform.localScale.x - Mathf.Abs(difference);
        float fallingCubeSizeX = previousCube.transform.localScale.x - size;

        float position = previousCube.transform.position.x + (difference / 2);
        transform.localScale = new Vector3(size, transform.localScale.y, transform.localScale.z);
        transform.position = new Vector3(position, transform.position.y, transform.position.z);

        float edge = transform.position.x + size / 2 * direction;
        float fallingCubePos = edge + fallingCubeSizeX / 2 * direction;
        SpawnDropCubeX(fallingCubePos, fallingCubeSizeX);
    }
    void SpawnDropCubeX(float xPosition, float xSize)
    {
        SoundManager.Instance.PlayCubeCut();
        var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.localScale = new Vector3(xSize, transform.localScale.y, transform.localScale.z);
        cube.transform.position = new Vector3(xPosition, transform.position.y, transform.position.z);

        cube.AddComponent<Rigidbody>();
        cube.GetComponent<MeshRenderer>().sharedMaterial= material;
        MovingCube.previousCube = this;
        Gamemanager.Instance.RaiseEventSO(3);
        Destroy(cube, 2f);
    }
    #endregion

    void Start()
    {
        switch (cubeType)
        {
            case cubeSpawnType.zaxis:
                direction = transform.forward;
                break;
            case cubeSpawnType.xaxis:
                direction = transform.right;
                break;
            default:
                break;

        }
    }

    void Update()
    {
        transform.position += direction * moveSpeed * Time.deltaTime;
        HandleSwitchDirection();
    }
    private void HandleSwitchDirection()
    {
        switch (cubeType)
        {
            case cubeSpawnType.zaxis:
                if (transform.localPosition.z > moveRange[1])
                {
                    direction = -Vector3.forward;
                }
                if (transform.localPosition.z < moveRange[0])
                {
                    direction = Vector3.forward;
                }
                break;
            case cubeSpawnType.xaxis:
                if (transform.localPosition.x > moveRange[1])
                {
                    direction = -Vector3.right;
                }
                if (transform.localPosition.x < moveRange[0])
                {
                    direction = Vector3.right;
                }
                break;
            default:
                break;
        }
    }
}
public enum cubeSpawnType
{
    xaxis,
    zaxis,
}
