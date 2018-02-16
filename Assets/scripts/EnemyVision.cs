using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVision : MonoBehaviour
{
    public Vector2 range = new Vector2(1, 1);
    private EnemyAI AI;

    // Use this for initialization
    void Awake()
    {
        transform.localScale = new Vector3(transform.localScale.x * range.x, transform.localScale.y * range.y,
            transform.localScale.z);
        AI = transform.parent.GetComponent<EnemyAI>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            AI.setPlayerInVision(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            AI.setPlayerInVision(false);
        }
    }
}
