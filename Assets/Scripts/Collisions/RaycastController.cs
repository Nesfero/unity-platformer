﻿using UnityEngine;
using System.Collections;

namespace UnityPlatformer {
  [RequireComponent (typeof (BoxCollider2D))]
  public class RaycastController : MonoBehaviour {

    public LayerMask collisionMask;

    public float skinWidth = 0.10f;
    public int horizontalRayCount = 4;
    public int verticalRayCount = 4;

    [HideInInspector]
    public float horizontalRaySpacing;
    [HideInInspector]
    public float verticalRaySpacing;

    [HideInInspector]
    public BoxCollider2D box;
    public RaycastOrigins raycastOrigins;

    public virtual void Awake() {
      box = GetComponent<BoxCollider2D> ();
    }

    public virtual void Start() {
      CalculateRaySpacing ();
    }

    public void UpdateRaycastOrigins() {
      Bounds bounds = box.bounds;
      bounds.Expand (skinWidth * -2);

      // cache
      Vector3 min = bounds.min;
      Vector3 max = bounds.max;

      raycastOrigins.bottomLeft = new Vector2 (min.x, min.y);
      raycastOrigins.bottomCenter = new Vector2 (min.x + bounds.size.x * 0.5f, min.y);
      raycastOrigins.bottomRight = new Vector2 (max.x, min.y);
      raycastOrigins.topLeft = new Vector2 (min.x, max.y);
      raycastOrigins.topRight = new Vector2 (max.x, max.y);
    }

     public RaycastHit2D Raycast(Vector2 origin, Vector2 direction, float rayLength, int mask, Color? color = null) {
      Debug.DrawRay(origin, direction * rayLength, color ?? Color.red);
       return Physics2D.Raycast(origin, direction, rayLength, mask);
     }

    public void CalculateRaySpacing() {
      Bounds bounds = box.bounds;
      bounds.Expand (skinWidth * -2);

      horizontalRayCount = Mathf.Clamp (horizontalRayCount, 2, int.MaxValue);
      verticalRayCount = Mathf.Clamp (verticalRayCount, 2, int.MaxValue);

      horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
      verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
    }

    public struct RaycastOrigins {
      public Vector2 topLeft, bottomCenter, topRight;
      public Vector2 bottomLeft, bottomRight;
    }

    public RaycastHit2D DoVerticalRay(float directionY, int i, float rayLength, ref Vector3 velocity) {
        Vector2 rayOrigin = (directionY == -1) ?
          raycastOrigins.bottomLeft :
          raycastOrigins.topLeft;

        rayOrigin += Vector2.right * (verticalRaySpacing * i + velocity.x);
        RaycastHit2D hit = Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMask, Color.red);

        return hit;
    }

    public RaycastHit2D DoFeetRay(float rayLength, LayerMask mask) {
      RaycastHit2D hit = Raycast(raycastOrigins.bottomCenter, Vector2.down, rayLength, mask, Color.blue);

      return hit;
    }
  }
}