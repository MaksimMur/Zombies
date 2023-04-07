using Opsive.UltimateCharacterController.Traits;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class Enemy : MonoBehaviour, IHealthSystem
{
    public EnemyFactory OriginFactory { get; set; }

    public float MaxHeatlh => GetComponent<AttributeManager>().Attributes[0].MaxValue;

    public float CurrentHealth => _health.HealthValue;

    private Health _health;

    private AudioSource _audioSource;

    private NavMeshAgent _agent;

    private Transform _player;

    private Animator _animator;

    private RagdollOnOff _ragdollOnOff;

    private EnemyAnimState _enemyAnimState = EnemyAnimState.idle;

    [SerializeField]
    private float _damage;

    [SerializeField]
    private LayerMask _whatIsGround, _whatIsPlayer;

    //patroling
    [SerializeField]
    private Vector3 _walkPoint;
    private bool _walkPointSet;
    [SerializeField]
    private float _walkPointRange;

    //attacking
    [SerializeField]
    private float _timeBetweenAttacks;
    private bool _alreadyAttacked;

    //States
    [SerializeField]
    private float _attackRange;
    [SerializeField]
    private float _sightRange;
    private bool _playerInSightRange, _playerInAttackRange;
    private bool _isDead = false;

    public UnityEvent OnDeath;

    public event System.Action onHpChanged;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _health = GetComponent<Health>();
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _ragdollOnOff = GetComponent<RagdollOnOff>();
        _player =PlayerMark.MARK!=null ? PlayerMark.MARK.GetComponent<Transform>(): null;
    }
    private void Start()
    {
        //_player = PlayerMark.MARK.GetComponent<Transform>();
        _ragdollOnOff.RagdollModeOnOff(true);
        print(CurrentHealth);
        print(MaxHeatlh);
    }
    private void Patroling()
    {
        if (_enemyAnimState != EnemyAnimState.movement)
        {
            _enemyAnimState = EnemyAnimState.movement;
            _animator.SetBool("Movement", true);
            _animator.SetBool("Attack", false);
        }
        if (!_walkPointSet)
        {
            SearchWalkPoint();
        }

        if (_walkPointSet)
        {
            _agent.SetDestination(_walkPoint);
        }

        Vector3 distanceToWalkPoint = transform.position - _walkPoint;

        if (distanceToWalkPoint.magnitude < 1f)
        {
            _walkPointSet = false;
        }
    }
    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-_walkPointRange, _walkPointRange);
        float randomX = Random.Range(-_walkPointRange, _walkPointRange);

        _walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
        if (Physics.Raycast(_walkPoint, -transform.up, _whatIsGround))
        {
            _walkPointSet = true;
        }
    }

    private void ChasePlayer()
    {
        if (_enemyAnimState != EnemyAnimState.movement)
        {
            _enemyAnimState = EnemyAnimState.movement;
            _animator.SetBool("Movement", true);
            _animator.SetBool("Attack", false);
        }

        _agent.SetDestination(_player.transform.position);
    }

    private void AttackPlayer()
    {
        if (_enemyAnimState != EnemyAnimState.attack)
        {
            _enemyAnimState = EnemyAnimState.attack;
            _animator.SetBool("Attack", true);
            _animator.SetBool("Movement", false);
        }
        transform.LookAt(_player);

        if (!_alreadyAttacked)
        {
            _alreadyAttacked = true;
            Invoke(nameof(ResetAttack), _timeBetweenAttacks);
        }
    }

    public void SetAttacked()
    {
        _alreadyAttacked = true;
    }

    private void ResetAttack()
    {
        _alreadyAttacked = false;
    } 

    public void SpawnOn(Vector3 pos)
    {
        transform.localPosition = pos;
    }
    
    public void GiveDamage()
    {
        if (_playerInAttackRange)
        {
            _player.GetComponent<Health>().Damage(_damage);
        }
    }

    public void GetDamage()
    {
        onHpChanged.Invoke();
        print(_health.HealthValue);
        GetComponent<Rigidbody>().isKinematic = true;
        if (_health.HealthValue <= 0 && !_isDead)
        {
            OnDeath.Invoke();
            _agent.enabled = false;
            _isDead = true;
            _ragdollOnOff.RagdollModeOnOff(false);
            _health.Invincible = true;
            Destroy(this.GetComponent<Collider>());
            Invoke("DeleteModele", 3f);
        }
    }

    private void DeleteModele()
    {
        OriginFactory.Reclaim(this);
        Destroy(this.gameObject);
    }

    public bool GameUpdate()
    {
        if (_player == null)
        {
            print("null");
                return false;
        }
        if (_isDead) return false;
        _playerInSightRange = Physics.CheckSphere(transform.position, _sightRange, _whatIsPlayer);
        _playerInAttackRange = Physics.CheckSphere(transform.position, _attackRange, _whatIsPlayer);

        if (!_playerInSightRange && !_playerInAttackRange && !_alreadyAttacked) Patroling();
        if (_playerInSightRange && !_playerInAttackRange && !_alreadyAttacked) ChasePlayer();
        if (_playerInAttackRange) AttackPlayer();

        return true;
    }
}

public enum EnemyAnimState
{ 
idle, 
movement,
attack
}
