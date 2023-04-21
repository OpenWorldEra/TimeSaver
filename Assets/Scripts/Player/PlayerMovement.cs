using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float speed = 5;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] float rotationForce = 20;
    float horizontal = 0;
    float vertical = 0;
    [SerializeField] AudioSource moveAudio;
    [SerializeField] float moveAudioInterval = 1;
    float currentMoveAudioIntervalTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = 0;
        vertical = 0;

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            horizontal = -1;
        } else if (Input.GetKey(KeyCode.RightArrow))
        {
            horizontal = 1;
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            vertical = 1;
        } else if (Input.GetKey(KeyCode.DownArrow))
        {
            vertical = -1;
        }
    }

    private void FixedUpdate() {
        MoveAudio();

        if (GameStateManager.Instance.CurrentGameState == GameState.Paused)
        {
            return;
        }
        
        Move();
        Rotate();
    }

    void MoveAudio()
    {
        currentMoveAudioIntervalTime += Time.deltaTime;

        if ((horizontal != 0 || vertical != 0) && GameStateManager.Instance.CurrentGameState == GameState.GamePlay)
        {
            if (moveAudio.isPlaying == false)
            {
                if (currentMoveAudioIntervalTime >= moveAudioInterval)
                {
                    moveAudio.Play();
                    currentMoveAudioIntervalTime = 0;
                }                
            }            
        } else
        {
            moveAudio.Stop();
        }
    }

    void Rotate()
    {
        float rotationDir = 0;
        if (Input.GetKey(KeyCode.A))
        {
            rotationDir = 1;
        } else if (Input.GetKey(KeyCode.D))
        {
            rotationDir = -1;
        }

        rb.rotation = rb.rotation + Time.deltaTime * rotationForce * rotationDir;
    }

    void Move() 
    {
        Vector2 mov = (Vector2)transform.position + (new Vector2(horizontal, vertical)).normalized * Time.deltaTime * speed;
        rb.MovePosition(mov);
    }
}
