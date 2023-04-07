using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameBoard : MonoBehaviour
{
    [SerializeField]
    private List<Vector3> _spawnPointsList;
    [SerializeField]
    private int _maxCountEnemyOnBoard=6;
    public int SpawnersCount => _spawnPointsList.Count;
    public int MaxCountEnemyOnBoard { get => _maxCountEnemyOnBoard; private set=>_maxCountEnemyOnBoard=value; }
    public Vector3 GetSpawnPoint(int index)
    { 
        return _spawnPointsList[index];
    }
    public void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        if (SpawnersCount == 0) return;
        foreach (var spawnPoint in _spawnPointsList)
        {
            Gizmos.DrawWireSphere(spawnPoint, 2f);
        }
    }

  

}
