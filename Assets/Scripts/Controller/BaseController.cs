using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseController : MonoBehaviour
{
    protected int appearance = 0;
    protected const int AppearanceCount = 2;
    protected Sprite[][] idleAnimation = new Sprite[AppearanceCount][];
    protected Sprite[][] moveAnimation = new Sprite[AppearanceCount][];

    protected int color = 0;
    protected const int ColorCount = 5;
    protected Color[] colors = new Color[ColorCount];
    protected bool appearanceChanged = false;
    protected Sprite[] rideAnimation;

    // Start is called before the first frame update
    public GameObject sprite;
    protected AnimationPlayer player = new();
    protected Rigidbody2D _rigidbody;
    protected float moveSpeed = 4.0f;
    protected float rideOnMaxSpeed = 16.0f;
    protected float rideOffMaxSpeed = 8.0f;
    protected float rideOnMinSpeed = 4.0f;
    protected float rideOffMinSpeed = 1f;
    protected float maxSpeed = 6.0f;
    protected float minSpeed = 1.5f;
    protected SpriteRenderer _spriteRenderer;
    private Vector2 lastMoveDirection = Vector2.zero;


    protected Vector2 velocity = Vector2.zero;
    protected Vector2 moveDirection = Vector2.zero;
    protected Camera _camera;

    protected bool moved = false;
    protected bool moving = false;
    protected bool ride = false;
    protected bool rided = false;

    protected virtual void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _camera = GameObject.Find("Main Camera").GetComponent<Camera>();
        _spriteRenderer = sprite.GetComponent<SpriteRenderer>();
    }

    protected virtual void Start()
    {
        
    }
    protected virtual void HandleAction()
    {

    }
    protected virtual void Movement(Vector2 moveDirection)
    {
        moveDirection *= moveSpeed;
        _rigidbody.velocity = moveDirection;
    }

    protected virtual void FixedUpdate()
    {
        Movement(moveDirection);
    }

    protected virtual void Rotate(Vector2 lastMoveDirection)
    {
        if(lastMoveDirection.x != 0)
        {
            _spriteRenderer.flipX = lastMoveDirection.x < 0;
        }
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        moved = moving;
        if(appearanceChanged)
        {
            if (ride == false)
            {
                player.Play(sprite, idleAnimation[appearance], 0.1f, true);
            }
            appearanceChanged = false;
        }
        if (rided == false && ride == true)
        {
            player.Play(sprite, rideAnimation, 0.1f, true);
            moveSpeed *= 3;
            maxSpeed = rideOnMaxSpeed;
            minSpeed = rideOnMinSpeed;
            moveSpeed = Mathf.Min(moveSpeed, maxSpeed);
        }
        else if (rided == true && ride == true)
        {

        }
        else if (rided == true && ride == false)
        {
            player.Play(sprite, idleAnimation[appearance], 0.1f, true);
            moveSpeed /= 3;
            maxSpeed = rideOffMaxSpeed;
            minSpeed = rideOffMinSpeed;
            moveSpeed = Mathf.Max(moveSpeed, minSpeed);
        }
        else if (_rigidbody.velocity.magnitude > 0.5f)
        {
            moving = true;
            if(_rigidbody.velocity.x != 0)
            {
                lastMoveDirection = _rigidbody.velocity;
            }
            if (moved == false)
            {
                player.Play(sprite, moveAnimation[appearance], 0.1f, true);
            }
        }
        else
        {
            moving = false;
            if (moved == true)
            {
                player.Play(sprite, idleAnimation[appearance], 0.1f, true);
            }
        }
        HandleAction();
        player.Update(Time.deltaTime);
        Rotate(lastMoveDirection);
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
    }

    protected virtual void OnTriggerExit2D(Collider2D other)
    {
    }
}
