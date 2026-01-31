using UnityEngine;

public class MinimapDotFollow : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private Transform mapPlane;
    [SerializeField] private Transform dot;

    [Header("World Settings")]
    [SerializeField] private Vector2 worldSize = new Vector2(100f, 100f);
    [SerializeField] private Vector2 worldCenter = Vector2.zero;

    [Header("Map Settings")]
    [SerializeField] private Vector2 mapSize = new Vector2(10f, 10f);
    [SerializeField] private float dotHeightOffset = 0.05f;

    void Update()
    {
        Vector2 playerXZ = new Vector2(
            player.position.x,
            player.position.z
        );

        // 0–1 normalized position
        Vector2 dot01 = MinimapMath.WorldToMinimap01(
            playerXZ,
            worldCenter,
            worldSize
        );

        // Convert to map local space
        Vector2 mapPos = MinimapMath.Minimap01ToMapPosition(
            dot01,
            mapSize
        );

        dot.position = new Vector3(
            mapPlane.position.x + mapPos.x,
            mapPlane.position.y + dotHeightOffset,
            mapPlane.position.z + mapPos.y
        );
    }
}


public static class MinimapMath
{
    /// <summary>
    /// Converts a world position into normalized minimap coordinates (0–1).
    /// </summary>
    public static Vector2 WorldToMinimap01(
            Vector2 worldPosition,
            Vector2 worldCenter,
            Vector2 worldSize
        )
    {
        float x01 = Mathf.InverseLerp(
            worldCenter.x - worldSize.x * 0.5f,
            worldCenter.x + worldSize.x * 0.5f,
            worldPosition.x
        );

        float y01 = Mathf.InverseLerp(
            worldCenter.y - worldSize.y * 0.5f,
            worldCenter.y + worldSize.y * 0.5f,
            worldPosition.y
        );

        return new Vector2(x01, y01);
    }


    /// <summary>
    /// Converts normalized minimap coordinates (0–1) to map-local space.
    /// </summary>
    public static Vector2 Minimap01ToMapPosition(
        Vector2 normalized,
        Vector2 mapSize
    )
    {
        return new Vector2(
            (normalized.x - 0.5f) * mapSize.x,
            (normalized.y - 0.5f) * mapSize.y
        );
    }
}