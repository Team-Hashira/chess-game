using DG.Tweening;
using UnityEngine;

namespace StageMap
{
	public class Node : MonoBehaviour, ISelectableObject
	{
		public bool canSeledable = false;
		[SerializeField] private float _selectedScale = 1.2f;
		[SerializeField] private float _duration = 0.1f;
		public Stage stageData;

		public void OnClick()
		{
			StageController.Instance.SetCurSelectedStage(stageData);
		}

		public void OnSelectEnter()
		{
			if (canSeledable == false) return;
			transform.DOScale(_selectedScale, _duration);
		}

		public void OnSelect()
		{
		}

		public void OnSelectExit()
		{
			if (canSeledable == false) return;
			transform.DOScale(1f, _duration);
		}
	}
}