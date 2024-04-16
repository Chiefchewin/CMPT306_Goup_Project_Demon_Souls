using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGenerator : MonoBehaviour
{
    public Transform player;
    public Tilemap tilemap;
    public TileBase[] tileVarieties;
    public GameObject[] props;
    public Vector2Int areaSize = new Vector2Int(10, 10);
    public float prefabSpawnChance = 0.1f;

    private Vector3 _lastPlayerPosition;
    private Dictionary<Vector3, float> occupiedCells = new Dictionary<Vector3, float>();

    void Start()
    {
        _lastPlayerPosition = player.position;
        UpdateArea();
    }

    void Update()
    {
        Vector3 playerDelta = player.position - _lastPlayerPosition;

        // Check if the player has moved significantly
        if (Mathf.Abs(playerDelta.x) >= 1 || Mathf.Abs(playerDelta.y) >= 1)
        {
            UpdateArea();
            _lastPlayerPosition = player.position;
        }
    }

    void UpdateArea()
    {
        BoundsInt bounds = new BoundsInt(
            Mathf.FloorToInt(player.position.x - areaSize.x / 2),
            Mathf.FloorToInt(player.position.y - areaSize.y / 2),
            0,
            areaSize.x,
            areaSize.y,
            1
        );

        for (int x = bounds.x; x < bounds.x + bounds.size.x; x++)
        {
            for (int y = bounds.y; y < bounds.y + bounds.size.y; y++)
            {
                Vector3Int cellPosition = new Vector3Int(x, y, 0);

                if (tilemap.GetTile(cellPosition) == null)
                {
                    // The tile is empty, fill it with a random variety from tileVarieties.
                    TileBase randomTile = tileVarieties[Random.Range(0, tileVarieties.Length)];
                    tilemap.SetTile(cellPosition, randomTile);

                    // Check if we should spawn a prefab in this cell based on the chance.
                    if (Random.value < prefabSpawnChance)
                    {
                        int randomPrefabIndex = Random.Range(0, props.Length);
                        Vector3 spawnPosition = tilemap.GetCellCenterWorld(cellPosition);

                        // Check if the spawn position is already occupied by a prefab.
                        if (!IsSpawnPositionOccupied(spawnPosition, props[randomPrefabIndex]))
                        {
                            Instantiate(props[randomPrefabIndex], spawnPosition, Quaternion.identity);
                            float prefabSize = CalculatePrefabSize(props[randomPrefabIndex]);
                            MarkOccupiedCells(spawnPosition, prefabSize);
                        }
                    }
                }
            }
        }
    }

    bool IsSpawnPositionOccupied(Vector3 spawnPosition, GameObject prefab)
    {
        // Check if the spawn position is too close to an existing prefab.
        foreach (var occupiedCell in occupiedCells)
        {
            float distance = Vector3.Distance(occupiedCell.Key, spawnPosition);
            float requiredDistance = (occupiedCell.Value + CalculatePrefabSize(prefab)) / 2.0f;
            if (distance < requiredDistance)
            {
                return true;
            }
        }
        return false;
    }

    float CalculatePrefabSize(GameObject prefab)
    {
        // Calculate the size based on the BoxCollider2D of the prefab.
        BoxCollider2D collider = prefab.GetComponent<BoxCollider2D>();
        if (collider != null)
        {
            return Mathf.Max(collider.size.x, collider.size.y);
        }
        else
        {
            // If the prefab doesn't have a BoxCollider2D, return a default size (you can adjust this).
            return 1.0f;
        }
    }

    void MarkOccupiedCells(Vector3 center, float size)
    {
        // Mark the cells around the prefab as occupied.
        int cellRadius = Mathf.CeilToInt(size);
        for (int x = -cellRadius; x <= cellRadius; x++)
        {
            for (int y = -cellRadius; y <= cellRadius; y++)
            {
                Vector3 cellPosition = new Vector3(center.x + x, center.y + y, center.z);
                occupiedCells[cellPosition] = size;
            }
        }
    }
}