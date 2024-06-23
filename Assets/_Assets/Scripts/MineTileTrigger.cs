using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineTileTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("MineTileMain"))
        {
            collision.gameObject.GetComponent<MineTap>().canMine = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("MineTileMain"))
        {
            collision.gameObject.GetComponent<MineTap>().canMine = false;
        }
    }
}
