using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleDamageCaster2D : DamageCaster2D
{
    [Header("BoxCaster2D")]
    public Vector2 center;
    public float radius = 1f;

    private float GetScaledSize(float radius)
    {
        float finalRadius = Mathf.Max(transform.lossyScale.x * radius, transform.lossyScale.y * radius);
        
        return finalRadius;
    }
    
    public override void CastOverlap()
    {
        Physics2D.OverlapCircle(transform.position + transform.rotation * GetFinalCenter(center),
            GetScaledSize(radius), new ContactFilter2D(){useLayerMask = true, layerMask = _whatIsCastable}, _castColliders);
    }
    
    private void OnDrawGizmos()
    {
        if (excluded) Gizmos.color = Color.red;
        else Gizmos.color = Color.green;
        Matrix4x4 oldMatrix = Gizmos.matrix;
        Gizmos.matrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);
        Gizmos.DrawWireSphere(GetFinalCenter(center), GetScaledSize(radius));
        Gizmos.matrix = oldMatrix;
        Gizmos.color = Color.white;
    }
}
