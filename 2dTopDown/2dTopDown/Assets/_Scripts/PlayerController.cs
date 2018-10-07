using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Photon.MonoBehaviour
{
    public Transform Crosshair;
    public float MouseSpeed = 2f;
    private Vector2 _direction;
    private PlayerStat _ps;
    private Animator _anim;
    private float _currentSpeed;
    private Rigidbody2D _rigid;
    public float moveVelocityMultiplier = 20f;
    private float _mouseAngle;
    private Camera _cam;

    private void Awake()
    {
        _ps = GetComponent<PlayerStat>();
        _cam = GetComponentInChildren<Camera>();
        _rigid = GetComponent<Rigidbody2D>();
        _anim = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if (photonView.isMine)
        {
            UpdateControll();
            UpdateMouse();
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

    private void UpdateMouse()
    {
        Vector3 crosshairPos = _cam.ScreenToWorldPoint(Input.mousePosition);

        Crosshair.transform.position = new Vector2(crosshairPos.x, crosshairPos.y);
        _mouseAngle = Vector2.Angle(transform.up, Crosshair.transform.position);
        if (crosshairPos.x < transform.position.x)
        {
            _mouseAngle = -_mouseAngle;
        }
    }

    private void UpdateAnim()
    {
        _anim.SetFloat("MoveSpeed", _currentSpeed);
        _anim.SetInteger("X", (int)_direction.x);
        _anim.SetInteger("Y", (int)_direction.y);
    }

    private void UpdateMove()
    {
        _currentSpeed = _direction.magnitude * _ps.MoveSpeed;
        _rigid.velocity = (_direction * _ps.MoveSpeed * Time.deltaTime * moveVelocityMultiplier);
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
