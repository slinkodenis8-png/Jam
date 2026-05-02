using UnityEngine;

public class behind : MonoBehaviour
{
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.GetComponent<SpriteRenderer>().sortingLayerName = "Default";
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        collision.GetComponent<SpriteRenderer>().sortingLayerName = "Player";
    }
}
