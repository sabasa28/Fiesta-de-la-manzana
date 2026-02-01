using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BichosSpawner : MonoBehaviour
{
    [SerializeField] int minTimeForSpawn;
    [SerializeField] int maxTimeForSpawn;
    float sideAx;
    float sideBx;
    float topsideY;
    float bottomSideY;
    [SerializeField] Transform topLeftCorner;
    [SerializeField] Transform bottomRightCorner;
    [SerializeField] Transform climbingSpawnPos;
    float timer = 0.0f;
    float nextSpawnTime = 0;
    [SerializeField] float firstSpawnTime;
    [SerializeField] GameObject[] posibleFlyingSpawns;
    [SerializeField] GameObject[] posibleClimbingSpawns;
    [SerializeField] Transform appleTrans;
    [SerializeField] Transform flowerTrans;
    [SerializeField] Transform[] leafTrans;
    [SerializeField] Transform[] leafMidpointsTrans;


    void Start()
    {
        nextSpawnTime = firstSpawnTime;
        sideAx = topLeftCorner.position.x;
        sideBx = bottomRightCorner.position.x;
        topsideY = topLeftCorner.position.y;
        bottomSideY = bottomRightCorner.position.y;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= nextSpawnTime)
        {
            timer = 0.0f;
            nextSpawnTime = Random.Range(minTimeForSpawn, maxTimeForSpawn);
            int spawnOption = Random.Range(0, posibleFlyingSpawns.Length + posibleClimbingSpawns.Length);
            IBicho spawnedBicho;
            if (spawnOption < posibleFlyingSpawns.Length)
            {
                spawnedBicho = Instantiate(posibleFlyingSpawns[spawnOption], GetRandomFlyingSpawnPos(), Quaternion.identity).GetComponent<IBicho>();
            }
            else
            {
                spawnOption -= posibleFlyingSpawns.Length;
                spawnedBicho = Instantiate(posibleClimbingSpawns[spawnOption], climbingSpawnPos.position, Quaternion.identity).GetComponent<IBicho>();
            }
            spawnedBicho.ReceiveObjective(appleTrans, flowerTrans, leafMidpointsTrans, leafTrans);
        }
    }

    Vector3 GetRandomFlyingSpawnPos()
    {
        int randomSide = Random.Range(0,3);
        Vector3 spawnPos = Vector3.zero; 
        switch (randomSide)
        {
            case 0:
                spawnPos.x = sideAx;
                spawnPos.y = Random.Range(bottomSideY, topsideY);
                break;
            case 1:
                spawnPos.x = sideBx;
                spawnPos.y = Random.Range(bottomSideY, topsideY);
                break;
            case 2:
                spawnPos.x = Random.Range(sideAx, sideBx);
                spawnPos.y = topsideY;
                break;
        }
        return spawnPos;
    }
}
