using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField]
    private GameBoard _board;

    [SerializeField, Range(0.1f, 5f)]
    private float _spawnSpeed=2f;
    private float _spawnProgress = 0;

    [SerializeField]
    private EnemyFactory _enemyFactory;

    private EnemyCollection _enemyCollection = new EnemyCollection();


    private void Update()
    {
        _spawnProgress += _spawnSpeed * Time.deltaTime;
        while (_spawnProgress >= 1)
        {
            _spawnProgress--;
            SpawnEnemy();
        }
        _enemyCollection.GameUpdate();
    }
    private void SpawnEnemy()
    {
        if (_board.MaxCountEnemyOnBoard <= _enemyCollection.EnemyCount)
        {
            return;
        }
        Enemy enemy = _enemyFactory.Get();
        enemy.transform.localPosition = _board.GetSpawnPoint(Random.Range(0, _board.SpawnersCount));
        enemy.OriginFactory = _enemyFactory;
        enemy.OnDeath.AddListener(EnemyKillsMonitor.AddKills);
        _enemyCollection.Add(enemy);
    }
}
