using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickController : MonoBehaviour
{
    // Start is called before the first frame update
    private PunchGameManager punchGameManager;
    void Start()
    {
        punchGameManager = PunchGameManager.getInstance();
    }

    // Update is called once per frame
    void Update()
    {
        if(gameObject.transform.position.x < -15)
        {
            punchGameManager.GameOver();
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            punchGameManager.AddScore(10);
            Destroy(gameObject);
        }else if (collision.gameObject.CompareTag("Ground"))
        {
            punchGameManager.GameOver();
            Destroy(gameObject);
        }
    }
}
