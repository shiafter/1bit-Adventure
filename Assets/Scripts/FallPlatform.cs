using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallPlatform : MonoBehaviour
{
    public float visibleTime = 1.5f;
    public float hideTime = 2f;

    private bool hidden;

    private Collider2D col;
    private SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        col = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        gameObject.SetActive(true);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!hidden && collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(Fall());

        }
    }
    private IEnumerator Fall()
    {
        yield return new WaitForSeconds(visibleTime);

        //hide platform
        hidden = true;
        col.enabled = false;
        spriteRenderer.enabled = false;

        yield return new WaitForSeconds(hideTime);

        //visible
        hidden = false;
        col.enabled = true;
        spriteRenderer.enabled = true;
    }
}
