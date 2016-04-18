using System;
using UnityEngine;
using UnityPlatformer.Actions;
using UnityPlatformer.Tiles;

namespace UnityPlatformer.Characters {
  /// <summary>
  /// Base class for: Players, NPCs, enemies.
  /// </summary>
  [RequireComponent (typeof (PlatformerCollider2D))]
  [RequireComponent (typeof (CharacterHealth))]
  [RequireComponent (typeof (PlatformerInput))]
  public class Character: MonoBehaviour, IUpdateEntity {
    // TODO REVIEW make a decision about it, calc from jump, make it public
    float gravity = -50;

    #region public

    /// <summary>
    /// States in wich the Character can be.
    /// Can be combine
    /// </summary>
    public enum States {
      None = 0,             // 0000000
      OnGround = 1,         // 0000001
      OnMovingPlatform = 3, // 0000011
      OnSlope = 5,          // 0000100
      Jumping = 8,          // 0001000
      Falling = 16,         // 0010000
      FallingFast = 48,     // 0110000
      Ladder = 64,          // 1000000
      //WallSliding,
      //WallSticking,
      //Dashing,
      //Frozen,
      //Slipping,
      //FreedomState
    }

    /// <summary>
    /// Areas in wich the Character can be.
    /// REVIEW can this be used to handle hazardous areas?
    /// </summary>
    public enum Areas {
      None = 0x0,
      Ladder = 0x01
    }

    ///
    /// Actions
    ///

    public Action onEnterArea;
    public Action onExitArea;

    #endregion

    #region ~private
    [HideInInspector]
    public States state = States.None;
    [HideInInspector]
    public Areas area = Areas.None;
    [HideInInspector]
    public BoxCollider2D body;
    [HideInInspector]
    public Ladder ladder;
    [HideInInspector]
    public MovingPlatform platform;
    [HideInInspector]
    public Vector3 velocity;
    [HideInInspector]
    public PlatformerCollider2D controller;
    [HideInInspector]
    public CharacterHealth health;

    #endregion

    #region private

    CharacterAction[] actions;
    CharacterAction lastAction;

    #endregion

    /// <summary>
    /// This method precalculate some vars, but those value could change. This need to be refactored.
    /// Maybe setters are the appropiate method to refactor this.
    /// </summary>
    virtual public void Start() {
      Debug.Log("Start new Character: " + gameObject.name);
      controller = GetComponent<PlatformerCollider2D> ();
      health = GetComponent<CharacterHealth>();
      actions = GetComponents<CharacterAction>();
      body = GetComponent<BoxCollider2D>();

      health.onDeath += OnDeath;
    }

    public void Attach(UpdateManager um) {
    }

    /// <summary>
    /// Managed update called by UpdateManager
    /// Transform Input into platformer magic :)
    /// </summary>
    virtual public void ManagedUpdate(float delta) {
      int prio = 0;
      int tmp;
      CharacterAction action = null;

      foreach (var i in actions) {
        tmp = i.WantsToUpdate(delta);
        if (tmp < 0) {
          i.PerformAction(Time.fixedDeltaTime);
        } else if (prio < tmp) {
          prio = tmp;
          action = i;
        }
      }

      // reset / defaults
      controller.disableWorldCollisions = false;
      PostUpdateActions a = PostUpdateActions.WORLD_COLLISIONS | PostUpdateActions.APPLY_GRAVITY;

      if (action != null) {
        if (lastAction != null && lastAction != action) {
          lastAction.GainControl();
        }

        action.PerformAction(Time.fixedDeltaTime);
        a = action.GetPostUpdateActions();
      }

      if (Utils.biton((int)a, (int)PostUpdateActions.APPLY_GRAVITY)) {
        velocity.y += gravity * delta;
      }

      if (!Utils.biton((int)a, (int)PostUpdateActions.WORLD_COLLISIONS)) {
        controller.disableWorldCollisions = true;
      }

      controller.Move(velocity * delta);

      // this is meant to fix jump and falling hit something unexpected
      if (controller.collisions.above || controller.collisions.below) {
        velocity.y = 0;
      }

      if (lastAction != null && lastAction != action) {
        lastAction.LoseControl();
      }

      lastAction = action;
    }

    public bool IsOnState(States _state) {
      return (state & _state) == _state;
    }

    public bool IsOnArea(Areas _area) {
      return (area & _area) == _area;
    }

    public void EnterState(States a) {
      state |= a;
    }

    public void ExitState(States a) {
      state &= ~a;
    }

    public void EnterArea(Areas a) {
      area |= a;

      if (onEnterArea != null) {
        onEnterArea(); // TODO send params?
      }
    }

    public void ExitArea(Areas a) {
      area &= ~a;

      if (onExitArea != null) {
        onExitArea(); // TODO send params?
      }
    }

    virtual public void OnDeath() {
      Debug.Log("Player die! play some fancy animation!");
    }

    public virtual Vector3 GetFeetPosition() {
      return body.bounds.center - new Vector3(
        0,
        body.bounds.size.y * 0.5f,
        0);
    }
  }
}