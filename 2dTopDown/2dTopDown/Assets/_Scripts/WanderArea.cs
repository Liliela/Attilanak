using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderArea : AIBehaviour
{
    public float wanderRadius;
    public float minwanderTimer;
    public float maxwanderTimer;

    private Vector2 targetPos;
    public Transform OriginTransform;

    public float mindistance;
    public float moveVelocityMultiplier = 20f;

    private float _currentSpeed;
    private Rigidbody2D _rigid;
    private Animator _anim;
    private Vector2 _direction;
    private GeneralStatistics _stat;
    private bool _onMove;

    public override void Awake()
    {
        base.Awake();
        _rigid = GetComponent<Rigidbody2D>();
        _stat = GetComponent<GeneralStatistics>();
        _anim = GetComponentInChildren<Animator>();
    }

    public override void EnterBehaviour()
    {
        base.EnterBehaviour();

        GetNewTarget();
    }

    public override void ExitBehaviour()
    {
        base.ExitBehaviour();
        CancelInvoke();
        _currentSpeed = 0;
        _rigid.velocity = Vector2.zero;
    }

    void Update()
    {
        if (PhotonNetwork.isMasterClient)
        {
            _direction = (targetPos - (Vector2)transform.position).normalized;
        }
    }

    private void GetNewTarget()
    {
        _onMove = true;

        if (OriginTransform == null)
        {
            targetPos = transform.position + UnityEngine.Random.insideUnitSphere * wanderRadius;
        }
        else
        {
            targetPos = OriginTransform.position + UnityEngine.Random.insideUnitSphere * wanderRadius;
        }
        Invoke("GetNewTarget", UnityEngine.Random.Range(minwanderTimer, maxwanderTimer));
    }

    private void FixedUpdate()
    {
        if (PhotonNetwork.isMasterClient)
        {
            UpdateMove();
            UpdateAnim();
        }
    }

    private float AngleBetweenVector2(Vector2 vec1, Vector2 vec2)
    {
        Vector2 diference = vec2 - vec1;
        float sign = (vec2.y < vec1.y) ? -1.0f : 1.0f;
        return Vector2.Angle(Vector2.right, diference) * sign;
    }

    private void UpdateAnim()
    {
        float _enemyAngle = 0;

        _enemyAngle = AngleBetweenVector2(transform.position, targetPos);

        _anim.SetFloat("MoveSpeed", _currentSpeed);
        //_anim.SetFloat("MoveSpeed", 10);

        if (_enemyAngle < 45 && _enemyAngle > -45)
        {
            _anim.SetFloat("inputX", 1);
            _anim.SetFloat("inputY", 0);
        }
        else if (_enemyAngle > 45 && _enemyAngle < 135)
        {
            _anim.SetFloat("inputX", 0);
            _anim.SetFloat("inputY", 1);
        }
        else if (_enemyAngle > 135 || _enemyAngle < -135)
        {
            _anim.SetFloat("inputX", -1);
            _anim.SetFloat("inputY", 0);
        }
        else if (_enemyAngle > -135 && _enemyAngle < -45)
        {
            _anim.SetFloat("inputX", 0);
            _anim.SetFloat("inputY", -1);
        }
    }

    private void UpdateMove()
    {
        if (Vector2.Distance(targetPos, transform.position) < 0.1f)
        {
            _onMove = false;
        }

        if (_onMove)
        {
            _rigid.velocity = (_direction * _stat.MoveSpeed * Time.deltaTime * moveVelocityMultiplier);
        }
        _currentSpeed = _direction.magnitude * _stat.MoveSpeed;
    }
}
