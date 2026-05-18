using UnityEngine;

public class CameraTargetFollow : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Vector3 offset = new Vector3(0, 1.5f, 0);

    void LateUpdate()
    {
        transform.position = player.position + offset;
    }
}