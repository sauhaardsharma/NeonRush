using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] CameraController cameraController;
    [SerializeField] GameObject[] chunkPrefabs;
    [SerializeField] GameObject checkpointPrefab;
    [SerializeField] Transform chunkParent;
    [SerializeField] ScoreManager scoreManager;

    [SerializeField] int chunkAmount = 10;
    [SerializeField] int checkpointInterval = 10;
    [SerializeField] float chunkLength = 12f;
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float minMoveSpeed = 6f;
    [SerializeField] float maxMoveSpeed = 20f;

    List<GameObject> chunks = new List<GameObject>();

    int chunksSpawned = 0;

    void Start()
    {
        SpawnStartingChunks();
    }

    void Update()
    {
        MoveChunks();
    }

    public void ChangeChunkMoveSpeed(float speedAmount)
    {
        float newMoveSpeed = moveSpeed + speedAmount;
        newMoveSpeed = Mathf.Clamp(newMoveSpeed, minMoveSpeed, maxMoveSpeed);

        if (newMoveSpeed != moveSpeed)
        {
            moveSpeed = newMoveSpeed;

            cameraController.ChangeCameraFOV(speedAmount);
        }

    }
    
    void SpawnStartingChunks()
    {
        for (int i = 0; i < chunkAmount; i++)
        {
            SpawnChunk();
        }
    }

    void SpawnChunk()
    {
        float spawnPositionZ = CalculateSpawnPosZ();
        Vector3 spawnPos = new Vector3(transform.position.x, transform.position.y, spawnPositionZ);
        GameObject chunkToSpawn = ChooseChunkToSpawn();
        GameObject newChunkGO = Instantiate(chunkToSpawn, spawnPos, quaternion.identity, chunkParent);
        chunks.Add(newChunkGO);
        Chunk newChunk = newChunkGO.GetComponent<Chunk>();
        newChunk.Init(this, scoreManager);

        chunksSpawned++;
    }

    public int GetSpawnedChunkCount()
    {
        return chunksSpawned;
    }

    private GameObject ChooseChunkToSpawn()
    {
        GameObject chunkToSpawn;

        if (chunksSpawned % checkpointInterval == 0 && chunksSpawned != 0)
        {
            chunkToSpawn = checkpointPrefab;
        }
        else
        {
            chunkToSpawn = chunkPrefabs[UnityEngine.Random.Range(0, chunkPrefabs.Length)];
        }

        return chunkToSpawn;
    }

    float CalculateSpawnPosZ()
    {
        float spawnPositionZ;
        if (chunks.Count == 0)
        {
            spawnPositionZ = transform.position.z;
        }
        else
        {
            spawnPositionZ = chunks[chunks.Count - 1].transform.position.z + chunkLength;
        }

        return spawnPositionZ;
    }

    void MoveChunks()
    {
        for (int i = 0; i < chunks.Count; i++)
        {
            GameObject chunk = chunks[i];
            chunks[i].transform.Translate(Vector3.back * (moveSpeed * Time.deltaTime));

            if (chunk.transform.position.z <= Camera.main.transform.position.z - chunkLength)
            {
                chunks.Remove(chunk);
                Destroy(chunk);
                SpawnChunk();
            }
        }
    }
}
