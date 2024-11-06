using UnityEngine;

namespace StageMapGenerator
{
	public class Line : MonoBehaviour
	{
		[SerializeField] private SpriteRenderer _visual;
		[SerializeField] private int _oneUnitPerCount = 10;
		private int _lineCountPropertyID = Shader.PropertyToID("_Count");
		public void TargetTo(Vector3 targetPos)
		{
			float distance = Vector3.Distance(transform.position, targetPos);
			_visual.material.SetFloat(_lineCountPropertyID, (int)(_oneUnitPerCount*distance));
			Vector2 dir = (targetPos - transform.position).normalized;
			transform.localScale = new Vector3 (1, distance, 1);
			transform.up = dir;
		}
	}
}