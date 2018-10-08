using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GeneralStatistics : MonoBehaviour
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
    [SerializeField]
    private float _healthActual;
    public Image HpBar;

    public bool Dead;

    private Animator _anim;

    private void Awake()
    {
        _anim = GetComponentInChildren<Animator>();
    }

    public virtual void Death()
    {
        _anim.SetBool("Dead", true);
        Dead = true;
    }

    public virtual void Revive(float percent)
    {
        _anim.SetBool("Dead",false);
        Dead = false;
        ChangeHealth(HealthMax * percent);
    }

    public virtual void ChangeHealth(float change)
    {
        Debug.Log(change);
        _anim.SetTrigger("Hurt");
        HealthActual += change;
    }
}
