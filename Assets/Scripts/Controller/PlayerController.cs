using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : BaseController
{
    protected bool rideZone = false;
    protected bool appearanceChangeZone = false;
    protected bool punchGameZone = false;
    protected bool planeGameZone = false;
    protected override void Awake()
    {
        base.Awake();
        idleAnimation[0] = Resources.LoadAll<Sprite>("Animations/Dragoon/Idle");
        moveAnimation[0] = Resources.LoadAll<Sprite>("Animations/Dragoon/Move");
        idleAnimation[1] = Resources.LoadAll<Sprite>("Animations/Zealot/Idle");
        moveAnimation[1] = Resources.LoadAll<Sprite>("Animations/Zealot/Move");
        colors[0] = Color.white;
        colors[1] = Color.black;
        colors[2] = Color.red;
        colors[3] = Color.green;
        colors[4] = Color.blue;
        rideAnimation = Resources.LoadAll<Sprite>("Animations/Science");
        RefreshCollider();
        player.Play(sprite, idleAnimation[appearance], 0.1f, true);
    }

    void RefreshCollider()
    {
        PolygonCollider2D oldCollider = sprite.GetComponent<PolygonCollider2D>();
        if (oldCollider)
        {
            Destroy(oldCollider);
        }
        sprite.AddComponent<PolygonCollider2D>();
    }
    protected override void HandleAction()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        moveDirection = new Vector2(horizontal, vertical).normalized;
        rided = ride;
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (rideZone == true)
            {
                ride = !ride;
            }
        }
    }

    protected override void Update()
    {
        if (appearanceChangeZone == true)
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                if (appearance == 1)
                {
                    appearance = 0;
                    appearanceChanged = true;
                }
            }
            if (Input.GetKeyDown(KeyCode.X))
            {
                if (appearance == 0)
                {
                    appearance = 1;
                    appearanceChanged = true;
                }
            }
            if(Input.GetKeyDown(KeyCode.C))
            {
                if(color < 4)
                {
                    color++;
                }
                _spriteRenderer.color = colors[color];
            }
            if (Input.GetKeyDown(KeyCode.V))
            {
                if(color > 0)
                {
                    color--;
                }
                _spriteRenderer.color = colors[color];
            }
        }
        if(punchGameZone == true)
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                GameContext.getInstance().ClearBeforeLoadScene();
                SceneManager.LoadScene("PunchGameScene");
                return;
            }
        }
        if(planeGameZone == true)
        {
            if (Input.GetKeyDown(KeyCode.K))
            {
                GameContext.getInstance().ClearBeforeLoadScene();
                SceneManager.LoadScene("PlaneGameScene");
                return;
            }
        }
        base.Update();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        Vector3 cameraPosition = _camera.transform.position;
        Vector3 playerSpritePosition = sprite.transform.position;
        if (playerSpritePosition.x < 3 &&
            playerSpritePosition.x > -3)
        {
            _camera.transform.position = new Vector3(playerSpritePosition.x, playerSpritePosition.y, cameraPosition.z);
        }
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D (other);
        if (other.gameObject.CompareTag("SpeedUp") && moveSpeed <= maxSpeed)
        {
            moveSpeed *= 2;
            moveSpeed = Mathf.Min(moveSpeed, maxSpeed);
        }
        else if (other.gameObject.CompareTag("SpeedDown") && moveSpeed >= minSpeed)
        {
            moveSpeed /= 2;
            moveSpeed = Mathf.Max(moveSpeed, minSpeed);
        }
        if (other.gameObject.CompareTag("RideZone"))
        {
            rideZone = true;
        }
        if (other.gameObject.CompareTag("AppearanceChangeZone")){
            appearanceChangeZone = true;
        }
        if (other.gameObject.CompareTag("PunchGameZone"))
        {
            punchGameZone = true;
        }
        if (other.gameObject.CompareTag("PlaneGameZone"))
        {
            planeGameZone = true;
        }
    }

    protected override void OnTriggerExit2D(Collider2D other)
    {
        base.OnTriggerExit2D (other);
        if (other.gameObject.CompareTag("RideZone"))
        {
            rideZone = false;
        }
        if (other.gameObject.CompareTag("AppearanceChangeZone"))
        {
            appearanceChangeZone = false;
        }
        if (other.gameObject.CompareTag("PunchGameZone"))
        {
            punchGameZone = false;
        }
        if (other.gameObject.CompareTag("PlaneGameZone"))
        {
            planeGameZone = false;
        }
    }
}