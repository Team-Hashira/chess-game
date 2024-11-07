using UnityEngine;

public static class Utils
{
    public static Vector3Int Add(this Vector3Int v1, Vector2Int v2)
    {
        return new Vector3Int(v1.x + v2.x, v1.y + v2.y, v1.z);
    }
    public static Vector2Int Add(this Vector2Int v1, Vector3Int v2)
    {
        return new Vector2Int(v1.x + v2.x, v1.y + v2.y);
    }
}