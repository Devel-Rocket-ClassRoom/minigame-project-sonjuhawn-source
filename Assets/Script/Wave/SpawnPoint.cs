using UnityEngine;

public class SpawnPoint : MonoBehaviour
{

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, 2f);
    }
#endif
}