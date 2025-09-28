using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    [SerializeField] GameObject fencePrefab;
    [SerializeField] GameObject speedBoostPrefab;
    [SerializeField] GameObject energyCapPrefab;

    [SerializeField] float speedBoostSpawnChance = 0.3f;
    [SerializeField] float energyCapSpawnChance = 0.5f;

    [SerializeField] float energyCapSeperationLen = 2f;
    [SerializeField] float[] lanes = { -2.5f, 0f, 2.5f };

    LevelGenerator levelGenerator;
    ScoreManager scoreManager;

    List<int> availableLanes = new List<int> { 0, 1, 2 };

    void Start()
    {
        SpawnFence();
        SpawnSpeedBoost();
        SpawnEnergyCap();
    }

    public void Init(LevelGenerator levelGenerator, ScoreManager scoreManager)
    {
        this.levelGenerator = levelGenerator;
        this.scoreManager = scoreManager;
    }

    void SpawnFence()
    {

        if (levelGenerator != null && levelGenerator.GetSpawnedChunkCount() <= 15)
            return;

        int fenceToSpawn = UnityEngine.Random.Range(0, lanes.Length);

        for (int i = 0; i < fenceToSpawn; i++)
        {
            if (availableLanes.Count <= 0) break;

            int selectedLane = SelectLane();

            Vector3 spawnPosition = new Vector3(lanes[selectedLane], transform.position.y, transform.position.z);
            Instantiate(fencePrefab, spawnPosition, Quaternion.identity, this.transform);
        }

    }

    void SpawnSpeedBoost()
    {   
        if (levelGenerator != null && levelGenerator.GetSpawnedChunkCount() <= 15)
            return;

        if (UnityEngine.Random.value > speedBoostSpawnChance || availableLanes.Count <= 0) return;

        int selectedLane = SelectLane();

        Vector3 spawnPosition = new Vector3(lanes[selectedLane], transform.position.y, transform.position.z);
        SpeedBoost newSpeedBoost = Instantiate(speedBoostPrefab, spawnPosition, Quaternion.identity, this.transform).GetComponent<SpeedBoost>();
        newSpeedBoost.Init(levelGenerator);

    }

    void SpawnEnergyCap()
    {   

        if (UnityEngine.Random.value > energyCapSpawnChance || availableLanes.Count <= 0) return;

        int selectedLane = SelectLane();

        int energyCapToSpawn = UnityEngine.Random.Range(1, 5);

        float zTopChunk = transform.position.z + (energyCapSeperationLen * 2f);

        for (int i = 0; i < energyCapToSpawn; i++)
        {
            float spawnPositionZ = zTopChunk - (i * energyCapSeperationLen);
            Vector3 spawnPosition = new Vector3(lanes[selectedLane], transform.position.y, spawnPositionZ);
            EnergyCap newEnergyCap = Instantiate(energyCapPrefab, spawnPosition, Quaternion.identity, this.transform).GetComponent<EnergyCap>();
            newEnergyCap.Init(scoreManager);
        }

    }

    private int SelectLane()
    {
        int randomLaneIndex = UnityEngine.Random.Range(0, availableLanes.Count);
        int selectedLane = availableLanes[randomLaneIndex];
        availableLanes.RemoveAt(randomLaneIndex);
        return selectedLane;
    }

}
