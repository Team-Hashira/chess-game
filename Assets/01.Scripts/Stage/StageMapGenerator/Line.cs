using UnityEngine;

namespace StageMap
{
	public class Line : MonoBehaviour
	{
		[SerializeField] private SpriteRenderer _visual;
		[SerializeField] private int _oneUnitPerCount = 10;
		[SerializeField] private float _speed = 1f;
		private int _lineCountPropertyID = Shader.PropertyToID("_Count");
		private int _speedPropertyID = Shader.PropertyToID("_Speed");
		private bool _canMove;
		public bool CanMove
		{
			get
			{
				return _canMove;
			}
			set
			{
				_canMove = value;
				_visual.material.SetFloat(_speedPropertyID, _speed);
			}
		}

		public void TargetTo(Vector3 targetPos)
		{
			float distance = Vector3.Distance(transform.position, targetPos);
			_visual.material.SetFloat(_lineCountPropertyID, (int)(_oneUnitPerCount * distance));
			Vector2 dir = (targetPos - transform.position).normalized;
			transform.localScale = new Vector3(1, distance, 1);
			transform.up = dir;
		}
	}
}