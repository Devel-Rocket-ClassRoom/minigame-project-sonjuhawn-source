using UnityEngine;

public class MonsterSpawnerDebug : MonoBehaviour
{
    [SerializeField] private GameObject monsterPrefab;
    [SerializeField] private KeyCode spawnKey = KeyCode.M;

    private void Update()
    {
        if (Input.GetKeyDown(spawnKey))
            Spawn();
    }

    private void Spawn()
    {
        if (monsterPrefab == null) return;
        Instantiate(monsterPrefab, new Vector3(0f,0f,3f), new Quaternion(0f, 180f ,0f ,1f));
    }
}