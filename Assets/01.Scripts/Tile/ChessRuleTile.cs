using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "2D/Tiles/ChessRuleTile")]
public class ChessRuleTile : RuleTile
{
	public List<Sprite> _blackSprite;
	public List<Sprite> _whiteSprite;

	public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
	{
		base.GetTileData(position, tilemap, ref tileData);

		if((position.x + position.y) % 2 == 0)
		{
			tileData.sprite = _blackSprite[Random.Range(0, _blackSprite.Count)];
		}
		else
		{
			tileData.sprite = _whiteSprite[Random.Range(0, _whiteSprite.Count)];
		}
	}
}