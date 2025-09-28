using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    [SerializeField] GameObject checkPointPrefab;
    [SerializeField] float obstacleDecreaseTimeAmount = .2f;
    GameManager gameManager;
    ObstacleSpawner obstacleSpawner;

    const string playerString = "Player";

    void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();
        obstacleSpawner = FindFirstObjectByType<ObstacleSpawner>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerString))
        {
            gameManager.CheckPointTimeIncrease();
            obstacleSpawner.DecreaseObstacleSpawnTime(obstacleDecreaseTimeAmount);
        }
    }
}
