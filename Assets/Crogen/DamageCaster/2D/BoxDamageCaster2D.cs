using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxDamageCaster2D : DamageCaster2D
{
	[Header("BoxCaster2D")]
	public Vector2 center;
	public Vector2 size = Vector2.one;

	private Vector2 GetScaledSize(Vector2 size)
	{
		Vector2 finalVec;
		finalVec.x = size.x * transform.lossyScale.x;
		finalVec.y = size.y * transform.lossyScale.y;

		return finalVec;
	}

	public override void CastOverlap()
	{
		Physics2D.OverlapBox(transform.position + transform.rotation * GetFinalCenter(center),
			GetScaledSize(size), transform.rotation.z, new ContactFilter2D(){useLayerMask = true, layerMask = _whatIsCastable}, _castColliders);
	}

	private void OnDrawGizmos()
	{
		if (excluded) Gizmos.color = Color.red;
		else Gizmos.color = Color.green;
		Matrix4x4 oldMatrix = Gizmos.matrix;
		Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);
		Gizmos.DrawWireCube(GetFinalCenter(center), GetScaledSize(size));
		Gizmos.matrix = oldMatrix;
		Gizmos.color = Color.white;
	}
}
