using System.Collections;
using UnityEngine;

public class NpcController : BaseController
{
    private Sprite[] deathAnimation;
    private bool isDeath = false;
    [SerializeField] private GameObject deathDialog;
    [SerializeField] private AudioClip deathAudio;
    private AudioSource audioSource;
    protected override void Awake()
    {
        base.Awake();
        idleAnimation[appearance] = Resources.LoadAll<Sprite>("Animations/Dragoon/Idle");
        moveAnimation[appearance] = Resources.LoadAll<Sprite>("Animations/Dragoon/Move");
        deathAnimation = Resources.LoadAll<Sprite>("Animations/Dragoon/Death");
        audioSource = gameObject.AddComponent<AudioSource>();
        player.Play(sprite, idleAnimation[appearance], 0.1f, true);
    }
    protected override void Update()
    {
        base.Update();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {

    }

    protected void OnCollisionEnter2D(UnityEngine.Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Player") && isDeath == false)
        {
            isDeath = true;
            player.Play(this._spriteRenderer.gameObject, deathAnimation, 0.05f, false);
            deathDialog.SetActive(true);
            StartCoroutine(HideDeathDialog());
        }
    }

    private IEnumerator HideDeathDialog()
    {
        yield return new WaitForSeconds(2f);
        deathDialog.SetActive(false);
    }
}