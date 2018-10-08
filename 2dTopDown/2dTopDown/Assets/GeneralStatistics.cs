using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GeneralStatistics : Photon.MonoBehaviour
{
    public float MoveSpeed
    {
        get { return _moveSpeed; }
        set { _moveSpeed = value; }
    }

    [SerializeField]
    private float _moveSpeed;

    public float HealthMax
    {
        get { return _healthMax; }
        set { _healthMax = value; }
    }

    [SerializeField]
    private float _healthMax;
    public float HealthActual
    {
        get { return _healthActual; }
        set
        {
            if (value > HealthMax)
            {
                _healthActual = HealthMax;
            }
            else if (value <= 0)
            {
                Death();
                _healthActual = 0;
            }
            else
            {
                _healthActual = value;
            }

            if (HpBar)
                HpBar.fillAmount = _healthActual / HealthMax;
        }
    }

    private List<EnemySensor> _monsterSenses = new List<EnemySensor>();

    [SerializeField]
    private float _healthActual;
    public Image HpBar;

    public bool Dead;

    private Animator _anim;

    private void Awake()
    {
        _anim = GetComponentInChildren<Animator>();
    }

    public void AddMonsterSense(EnemySensor enemySensor)
    {

        if (!_monsterSenses.Contains(enemySensor))
        {
            _monsterSenses.Add(enemySensor);
        }
    }

    public void RemoveMonsterSenser(EnemySensor enemySensor)
    {
        if (_monsterSenses.Contains(enemySensor))
        {
            _monsterSenses.Remove(enemySensor);
        }
    }

    public virtual void Death()
    {
        _anim.SetBool("Dead", true);
        Dead = true;
        foreach (var sense in _monsterSenses)
        {
            sense.RemovePlayer(GetComponent<PhotonPlayerController>());
        }
    }

    public virtual void Revive(float percent)
    {
        _anim.SetBool("Dead", false);
        Dead = false;
        ChangeHealth(HealthMax * percent);
    }

    [PunRPC]
    public void RPC_ChangeHealt(float change)
    {
        ChangeHealth(change);
    }

    public virtual void ChangeHealth(float change)
    {
        Debug.Log(change);
        _anim.SetTrigger("Hurt");
        HealthActual += change;
    }

    public event EventHandler DeathEvent;

    protected virtual void OnDeathEvent(EventArgs e)
    {
        EventHandler handler = DeathEvent;
        if (handler != null)
        {
            handler(this, e);
        }
    }
}
