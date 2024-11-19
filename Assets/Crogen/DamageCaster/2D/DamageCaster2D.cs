using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public abstract class DamageCaster2D : MonoBehaviour
{
	public bool excluded;
	public int allocationCount = 32;
	[SerializeField] protected LayerMask _whatIsCastable;
	protected Collider2D[] _castColliders;
	[SerializeField] private bool _usingExcludeCast = true;
	public List<DamageCaster2D> excludedDamageCasterList;

	public event Action OnCasterEvent;
	public event Action OnCasterSuccessEvent;
	public event Action OnDamageCastSuccessEvent;

	protected Vector2 GetFinalCenter(Vector2 center)
	{
		Vector2 finalCenter;
		finalCenter.x = center.x * transform.lossyScale.x;
		finalCenter.y = center.y * transform.lossyScale.y;
		return finalCenter;
	}

	protected virtual void Awake()
	{
		_castColliders = new Collider2D[allocationCount];
	}

	public abstract void CastOverlap();

	public virtual void CastDamage(int damage)
	{
		CastOverlap();

		//제외
		if (_usingExcludeCast)
			ExcludeCast(_castColliders);


		//데미지 입히기
		for (int i = 0; i < _castColliders.Length; ++i)
		{
			if (_castColliders[i] == null)
			{
				break;
			}
			else
			{
				OnDamageCastSuccessEvent?.Invoke();
			}
			if (_castColliders[i].TryGetComponent(out IDamageable damageable))
			{
				damageable.TakeDamage(damage);
			}
			if (_castColliders[i] != null)
			{
				OnCasterSuccessEvent?.Invoke();
			}

		}

		OnCasterEvent?.Invoke();
		//이거 내부적으로 메모리를 직접 초기화해서 가벼움
		Array.Clear(_castColliders, 0, _castColliders.Length);
	}

	protected void ExcludeCast(Collider2D[] colliders)
	{
		foreach (var excludeCaster in excludedDamageCasterList)
		{
			excludeCaster.CastOverlap();
			colliders = colliders.Except(excludeCaster._castColliders).ToArray();
		}
	}

	private void OnValidate()
	{
		if (excludedDamageCasterList == null) return;
		for (int i = 0; i < excludedDamageCasterList.Count; ++i)
		{
			if (excludedDamageCasterList[i] == null) continue;

			if (excludedDamageCasterList[i].excluded == false)
				excludedDamageCasterList[i] = null;
		}
	}
}