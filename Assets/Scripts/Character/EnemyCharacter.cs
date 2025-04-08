using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class EnemyCharacter : Character
{
    [SerializeField] private AIState currentState;

    private float timeBetweenAttackCounter = 0;
    private float attackDistance = 1.0f;

    public override Character CharacterTarget 
        => GameManager.Instance.CharacterFactory.Player;



    public override void Initialize()
    {
        base.Initialize();  // Важно!

        // Дополнительные проверки
        if (MovableComponent == null)
        {
            Debug.LogError("MovableComponent is not initialized in EnemyCharacter.");
        }

        if (CharacterTarget == null)
        {
            Debug.LogError("CharacterTarget is not assigned in EnemyCharacter.");
        }

        // Инициализируем другие компоненты
        LiveComponent = new CharacterLiveComponent(this);
        DamageComponent = new CharacterDamageComponent(this);

        currentState = AIState.MoveToTarget;

        // Инициализируем всё заново
        if (LiveComponent == null)
            Debug.LogError("LiveComponent is not set for Enemy.");
        if (DamageComponent == null)
            Debug.LogError("DamageComponent is not set for Enemy.");
    }


    public override void Update()
    {
        if (CharacterTarget == null)
        {
            Debug.LogError("CharacterTarget is not assigned!");
            return;
        }

        float distanceToTarget = Vector3.Distance(CharacterTarget.transform.position, transform.position);
        Debug.Log($"Enemy distance to target: {distanceToTarget}");

        switch (currentState)
        {
            case AIState.None:
                break;

            case AIState.MoveToTarget:
                if (distanceToTarget <= attackDistance)
                {
                    Debug.Log("Switching to Attack state.");
                    currentState = AIState.Attack;
                }
                else
                {
                    MoveTowardsTarget();
                }
                break;

            case AIState.Attack:
                if (distanceToTarget <= attackDistance
                    /*&& timeBetweenAttackCounter <= 0*/)
                {
                    Debug.Log("Performing attack.");
                    PerformAttack();
                    timeBetweenAttackCounter = characterData.TimeBetweenAttacks;
                }
                else
                {
                    Debug.Log("Switching to MoveToTarget state.");
                    currentState = AIState.MoveToTarget;
                }
                break;
        }
        Debug.Log($"State: {currentState}");
    }

    private void MoveTowardsTarget()
    {
        if (CharacterTarget == null)
        {
            Debug.LogError("CharacterTarget is null in MoveTowardsTarget.");
            return;
        }
        if (MovableComponent == null)
        {
            Debug.LogError("MovableComponent is null in MoveTowardsTarget.");
            return;
        }

        Vector3 direction = CharacterTarget.transform.position - transform.position;
        direction.Normalize();

        MovableComponent.Move(direction);
        MovableComponent.Rotation(direction);
    }



    private void PerformAttack()
    {
        if (timeBetweenAttackCounter <= 0)
        {
            if (CharacterTarget != null && CharacterTarget.LiveComponent != null)
            {
                Debug.Log($"Enemy attacks {CharacterTarget.name} for {DamageComponent.Damage} damage.");
                DamageComponent.MakeDamage(CharacterTarget);
                timeBetweenAttackCounter = characterData.TimeBetweenAttacks;
            }
            else
            {
                Debug.LogError("CharacterTarget or LiveComponent is null.");
            }
        }
        else
        {
            timeBetweenAttackCounter -= Time.deltaTime;
        }
    }


}
