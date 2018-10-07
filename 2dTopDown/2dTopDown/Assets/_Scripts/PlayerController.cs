using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Photon.MonoBehaviour
{
    private Vector2 _direction;
    private PlayerStat _ps;
    private Animator _anim;
    private float _currentSpeed;
    private Rigidbody2D _rigid;

    private void Awake()
    {
        _ps = GetComponent<PlayerStat>();
        _rigid = GetComponent<Rigidbody2D>();
        _anim = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if (photonView.isMine)
        {
            UpdateControll();
        }
    }

    private void FixedUpdate()
    {
        if (photonView.isMine)
        {
            UpdateMove();
            UpdateAnim();
        }
    }

    private void UpdateAnim()
    {
        _anim.SetFloat("MoveSpeed", _currentSpeed);
        _anim.SetFloat("X", _direction.x);
        _anim.SetFloat("Y", _direction.y);
    }

    private void UpdateMove()
    {
        _currentSpeed = _direction.magnitude * _ps.MoveSpeed;
        _rigid.MovePosition(_rigid.position + _direction * _ps.MoveSpeed * Time.deltaTime);
        //  transform.Translate(_direction * _ps.MoveSpeed * Time.deltaTime);
    }

    private void UpdateControll()
    {
        _direction = Vector2.zero;
        _currentSpeed = 0;
        if (Input.GetKey(KeyCode.A))
        {
            _direction += Vector2.left;
        }
        if (Input.GetKey(KeyCode.D))
        {
            _direction += Vector2.right;
        }
        if (Input.GetKey(KeyCode.S))
        {
            _direction += Vector2.down;
        }
        if (Input.GetKey(KeyCode.W))
        {
            _direction += Vector2.up;
        }
    }
}
