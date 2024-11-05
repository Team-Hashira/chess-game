using UnityEngine;

public class Piece : MonoBehaviour
{
    [SerializeField] private TextAsset _textAsset;
    [field:SerializeField] public PieceType PieceType { get; private set; }
    [field:SerializeField] public TeamType Team { get; private set; }
    public int ID { get; private set; }

    public int[,]       MovableTiles { get; private set; }
    public Vector2Int   CenterPos { get; private set; }
    public void Init()
    {
        ID = (int)PieceType + (int)Team;

        string[] lines = _textAsset.text.Split('\n');

        MovableTiles = new int[lines.Length, lines[0].Length-1];

        for (int i = 0; i < lines.Length; i++)
        {
            for (int j = 0; j < lines[0].Length-1; j++)
            {
                MovableTiles[i, j] = lines[i][j] - 48;
				if(MovableTiles[i, j] == 2)
					CenterPos = new Vector2Int(i, j);
			}
        }
    }
    
    public void MoveTo(Vector2Int pos)
    {
        Vector3 targetPos = new Vector3(pos.x, pos.y, 0);

        transform.position = targetPos;
    }

	private void OnDrawGizmos()
	{
        if (MovableTiles == null) return;
		for (int i = 0; i < MovableTiles.GetLength(0); i++)
		{
			for (int j = 0; j < MovableTiles.GetLength(1); j++)
			{
				CheckGizemoDraw(i, j);
			}
		}
	}

    private void CheckGizemoDraw(int i, int j)
    {
		Vector3 size = new Vector3(Mathf.FloorToInt(MovableTiles.GetLength(0) / 2f), Mathf.FloorToInt(MovableTiles.GetLength(1) / 2f));
		switch (MovableTiles[i, j])
		{
			case 0:
			{
				//
			}
			break;
			case 1:
			{
				Gizmos.color = new Color(1, 0, 0, 0.5f);
				Gizmos.DrawCube(transform.position + new Vector3(i, j) - size, Vector2.one);
				Gizmos.color = Color.white;
			}
			break;
			case 2:
			{
				Gizmos.color = new Color(0, 1, 0, 0.5f);
				Gizmos.DrawCube(transform.position + new Vector3(i, j) - size, Vector2.one);
				Gizmos.color = Color.white;
			}
			break;

		}
	}
}
