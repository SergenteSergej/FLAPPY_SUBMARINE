using UnityEngine;

public class FloorLooper : MonoBehaviour
{
    public Transform player; // Riferimento al sottomarino
    [SerializeField] float floorLength = 10f; // Lunghezza del singolo pezzo di pavimento
    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        if (player.position.x > transform.position.x + floorLength)
        {
            MoveFloorForward();
        }
    }

    void MoveFloorForward()
    {
        transform.position += Vector3.right * (floorLength * 2);
    }
}