using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinishLevel : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public Sprite lockedDoor;
    public Sprite openedDoor;
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (GameController.instance.levelCompleted)
        {
            spriteRenderer.sprite = openedDoor;
        }
        spriteRenderer.sprite = lockedDoor;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && GameController.instance.levelCompleted)
        {
            SoundEffectManager.Play("LevelUp");
            GameController.instance.GoToNextLevel();
        }
    }
}
