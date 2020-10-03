using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Singleton<Player>
{
    public Controller2D Controller
    {
        get
        {
            if(_controller == null)
            {
                _controller = GetComponent<Controller2D>();
            }

            return _controller;
        }
    }
    Controller2D _controller;

    public MovementController Movement
    {
        get
        {
            if (_movement == null)
            {
                _movement = GetComponent<MovementController>();
            }

            return _movement;
        }
    }
    MovementController _movement;

    public JumpController Jump
    {
        get
        {
            if (_jump == null)
            {
                _jump = GetComponent<JumpController>();
            }

            return _jump;
        }
    }
    JumpController _jump;

    public DashController Dash
    {
        get
        {
            if (_dash == null)
            {
                _dash = GetComponent<DashController>();
            }

            return _dash;
        }
    }
    DashController _dash;

    public CharacterHealth Health
    {
        get
        {
            if (_health == null)
            {
                _health = GetComponent<CharacterHealth>();
            }

            return _health;
        }
    }
    CharacterHealth _health;
}
