using UnityEngine;

public class BubbleDestroyer : MonoBehaviour
{
    [SerializeField] private float maxHeight = 10f;

    void Update()
    {
        if (transform.position.y >= maxHeight)
        {
            Destroy(gameObject);
        }
    }
}
