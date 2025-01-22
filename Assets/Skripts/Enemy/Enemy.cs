using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    [SerializeField] private EnemyView _view;
    [SerializeField] private RagdollHandler _ragdollHandler;
    [SerializeField] private Mover mover;

    public void Initializing()
    {
        _view.Initializing(transform);
        _ragdollHandler.Initializing();
    }
    public void StartRun()
    {
        _view.StartRunning();
        mover.Enable();
    }
    public void Kill()
    {
        EnableRagdollAnimator();
    }

    public void StanUp()
    {
        _ragdollHandler.Disable();
        _view.PlayStandingUp(_view.EnableAnimator,_view.PlayStandingIdle);
    }

    public void TakeDamage(Vector3 force, Vector3 hitpoint)
    {
        EnableRagdollAnimator();
        _ragdollHandler.Hit(force,hitpoint);
    }

    private void EnableRagdollAnimator()
    {
        _view.DisableAnimator();
        _ragdollHandler.Enable();
        mover.Disable();
    }
}
