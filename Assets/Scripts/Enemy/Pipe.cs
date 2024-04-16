using System.Collections;
using UnityEngine;

public class Pipe : MonoBehaviour
{
    [SerializeField] Enemy _enemyPrefab;
    [SerializeField] float _spawnTimer = 3f;

    private void Start() 
    {
        StartCoroutine(SpawnRoutine());
    }
    
   IEnumerator SpawnRoutine() 
   {
        while (true)
        {
            Enemy enemy = Instantiate(_enemyPrefab, transform.position, transform.rotation);
            yield return new WaitForSeconds(_spawnTimer);
        }
    }
}
