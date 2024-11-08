using DG.Tweening;
using UnityEngine;

namespace StageMapGenerator
{
	public class Node : MonoBehaviour, ISelectableObject
	{
		public bool canSeledable = false;
		[SerializeField] private float _selectedScale = 1.2f;
		[SerializeField] private float _duration = 0.1f;

		public void OnClick()
		{
			Debug.Log("OnClick");
		}

		public void OnSelectEnter()
		{
			Debug.Log("OnSelectEnter");
			if (canSeledable == false) return;
			transform.DOScale(_selectedScale, _duration);
		}

		public void OnSelect()
		{
			Debug.Log("OnSelect");
		}

		public void OnSelectExit()
		{
			Debug.Log("OnSelectExit");
			if (canSeledable == false) return;
			transform.DOScale(1f, _duration);
		}
	}
}