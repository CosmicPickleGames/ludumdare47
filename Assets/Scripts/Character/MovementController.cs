using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Controller2D))]
public class MovementController : MonoBehaviour
{
    public float movementSpeed = 10;

    [Header("Effects")]
    public AudioClip moveSFX;
    public float moveVolume = .3f;

    private Controller2D controller;
    private AudioSource _moveAudioSource;

    // Start is called before the first frame update
    void Awake()
    {
        controller = GetComponent<Controller2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(UIMenu.Instance.MenuOpen)
        {
            if (_moveAudioSource != null)
            {
                AudioManager.Instance.StopLoopingSound(_moveAudioSource);
                _moveAudioSource = null;
            }

            return;
        }

        float move = Input.GetAxisRaw("Horizontal");

        if(move != 0 && controller.Grounded)
        {
            if (_moveAudioSource == null)
            {
                _moveAudioSource = AudioManager.Instance.PlaySound(moveSFX, transform.position, transform, true, moveVolume);
            }
        }
        else if (_moveAudioSource != null)
        {
            AudioManager.Instance.StopLoopingSound(_moveAudioSource);
            _moveAudioSource = null;
        }

        controller.SetHorizontalMovement(Input.GetAxisRaw("Horizontal") * movementSpeed);
    }
}
