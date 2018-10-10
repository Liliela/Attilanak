using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySimpleMelleeAttack : AIBehaviour
{
    public EnergyType EnergyType = EnergyType.Physical;
    public float Damage;
    public float Cooldown;
    public float OptimalRange;
    public float AttackRadius;
    public Transform AttackPos;
    public string AttackAnimName = "Attack";

    public LayerMask HitLayer;

    private bool _onCooldown;
    private Animator _anim;
    private FollowTarget _followTarget;

    public override void Awake()
    {
        base.Awake();
        _anim = GetComponentInChildren<Animator>();
        _followTarget = GetComponent<FollowTarget>();
    }

    private void Update()
    {
        if (!_onCooldown && Active && Vector2.Distance(transform.position, _followTarget.FollowTransform.position) < OptimalRange)
        {
            Collider2D[] playerToHit = Physics2D.OverlapCircleAll(AttackPos.position, AttackRadius, HitLayer);
            if (playerToHit.Length > 0)
            {
                foreach (var item in playerToHit)
                {
                    item.GetComponentInParent<PlayerStat>().photonView.RPC("RPC_ChangeHealth", PhotonTargets.AllBuffered, -Damage, EnergyType.ToString());
                }
                _onCooldown = true;
                _anim.SetTrigger(AttackAnimName);
                Invoke("CooldownReady", Cooldown);
            }
        }
    }

    public void CooldownReady()
    {
        _onCooldown = false;
    }

    public override void EnterBehaviour()
    {
        base.EnterBehaviour();
    }

    public override void ExitBehaviour()
    {
        base.ExitBehaviour();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(AttackPos.position, AttackRadius);
    }
}
