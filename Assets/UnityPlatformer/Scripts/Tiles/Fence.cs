using UnityEngine;
using System.Collections;
using UnityEngine.Rendering;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UnityPlatformer {
  [RequireComponent (typeof (PolygonCollider2D))]
  public class Fence : MonoBehaviour {
    // cache
    PolygonCollider2D body;
    public bool topDismount = true;
    public bool bottomDismount = true;

    virtual public void Start() {
      body = GetComponent<PolygonCollider2D>();
    }

    virtual public void EnableFence(Character p) {
      p.fence = this;
      p.EnterArea(Areas.Fence);
    }

    virtual public void Dismount(Character p) {
      p.ExitState(States.Fence);
    }

    virtual public void DisableFence(Character p) {
      Dismount(p);
      p.ExitArea(Areas.Fence);
      p.fence = null;
    }

    public virtual void OnTriggerEnter2D(Collider2D o) {
      HitBox h = o.GetComponent<HitBox>();
      if (h && h.type == HitBoxType.EnterAreas) {
        EnableFence(h.owner.GetComponent<Character>());
      }
    }

    public virtual void OnTriggerStay2D(Collider2D o) {
      HitBox h = o.GetComponent<HitBox>();
      if (h && h.type == HitBoxType.EnterAreas) {
        Vector2 pmin = body.bounds.min;
        Vector2 pmax = body.bounds.max;

        // check if the body is completely inside
        if (
          body.Contains(pmin) &&
          body.Contains(pmax) &&
          body.Contains(new Vector2(pmin.x, pmax.y)) &&
          body.Contains(new Vector2(pmax.x, pmin.y))
        ) {
          EnableFence(h.owner.GetComponent<Character>());
        } else {
          DisableFence(h.owner.GetComponent<Character>());
        }
      }
    }

#if UNITY_EDITOR
    [DrawGizmo(GizmoType.InSelectionHierarchy | GizmoType.NotInSelectionHierarchy)]
    void OnDrawGizmos() {
      if (Application.isPlaying) return;

      Vector3 size = body.bounds.size;
      Vector3 pos = transform.position;

      Handles.Label(pos + new Vector3(-size.x * 0.5f, size.y * 0.5f, 0), "Fence");

      Handles.DrawSolidRectangleWithOutline(body.GetWorldPoints3(), new Color(1, 1, 0, 0.05f), new Color(0, 0, 0, 0.5f));
    }
#endif


  }
}
