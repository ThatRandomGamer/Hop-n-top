using System.Collections.Generic;
using UnityEngine;

public class ChunkGenerator : MonoBehaviour
{
    [Header("Chunk Prefabs")]
    public GameObject startingChunk;
    public List<GameObject> easyChunks;
    public List<GameObject> mediumChunks;
    public List<GameObject> hardChunks;

    [Header("Progression Thresholds")]
    public float mediumThreshold = 100f;
    public float hardThreshold = 300f;

    [Header("Spawn Settings")]
    public int startingChunkRepeats = 3;
    public float startY = 0f;
    public float fixedX = 0f;
    public float fixedZ = 0f;
    public float spawnDistanceAhead = 50f; // Distance ahead of the player to trigger a spawn

    [Header("Player")]
    public Transform player;
    private PlayerController playerScript;

    // Current Y spawn
    private float currentY;

    // Active chunks
    private Queue<GameObject> activeChunks = new Queue<GameObject>();

    void Start()
    {
        currentY = startY;

        if (player != null)
        {
            playerScript = player.GetComponent<PlayerController>();
        }
        // Spawn initial buffer of chunks
        for (int i = 0; i < 5; i++)
        {
            SpawnChunk();
        }
    }

    void Update()
    {
        // Spawn more when player climbs close to the current highest point
        if (player.position.y > currentY - spawnDistanceAhead)
        {
            SpawnChunk();
        }
    }

    void SpawnChunk()
    {
        GameObject chunkToSpawn;

        // Determine if we are still spawning starter chunks
        if (activeChunks.Count < startingChunkRepeats)

        {
            chunkToSpawn = startingChunk;
        }
        else
        {

            float currentScore = playerScript != null ? playerScript.Distance : 0f;


            List<GameObject> currentActiveList;

            if (currentScore >= hardThreshold)
            {
                currentActiveList = hardChunks;
                Debug.Log("Current mode: hard");
            }
            else if (currentScore >= mediumThreshold)
            {
                currentActiveList = mediumChunks;
                Debug.Log("Current mode: medium");

            }
            else
            {
                currentActiveList = easyChunks;
                Debug.Log("Current mode: easy");

            }

            //Fallback in case a list is empty to prevent errors
            if (currentActiveList.Count == 0)
            {
                Debug.LogWarning("The selected difficulty list is empty! Defaulting to starting chunk.");
                currentActiveList = new List<GameObject> { startingChunk };
            }

            // Pick a random prefab from the chosen difficulty list
            int randomIndex = Random.Range(0, currentActiveList.Count);
            chunkToSpawn = currentActiveList[randomIndex];
        }


        GameObject chunk = Instantiate(
            chunkToSpawn,
            new Vector3(fixedX, currentY, fixedZ),
            Quaternion.identity
        );

        activeChunks.Enqueue(chunk);

        // Find EndPoint
        Transform endPoint = chunk.transform.Find("EndPoint");

        if (endPoint == null)
        {
            Debug.LogError($"EndPoint missing on prefab: {chunk.name}! Ensure it is a direct child.");
            return;
        }

        // Set the Y coordinate for the NEXT chunk to be the top of THIS chunk
        currentY = endPoint.position.y;
    }


}