using UnityEngine;

public enum ConvertType
{
    Ceil,
    Round
}
public static class Utils
{
    public static Vector3Int Add(this Vector3Int v1, Vector2Int v2)
        => new Vector3Int(v1.x + v2.x, v1.y + v2.y, v1.z);
    public static Vector2Int Add(this Vector2Int v1, Vector3Int v2)
        => new Vector2Int(v1.x + v2.x, v1.y + v2.y);
    public static Vector3Int ConvertToInt(this Vector3 vector, ConvertType type = ConvertType.Ceil)
        => type == ConvertType.Ceil ? new Vector3Int(Mathf.CeilToInt(vector.x), Mathf.CeilToInt(vector.y))
        : new Vector3Int(Mathf.RoundToInt(vector.x), Mathf.RoundToInt(vector.y));
    public static Vector2Int ConvertToInt(this Vector2 vector, ConvertType type = ConvertType.Ceil)
       => type == ConvertType.Ceil ? new Vector2Int(Mathf.CeilToInt(vector.x), Mathf.CeilToInt(vector.y))
       : new Vector2Int(Mathf.RoundToInt(vector.x), Mathf.RoundToInt(vector.y));

}