using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterLiveComponent : ILiveComponent
{
    private Character selfCharacter;
    private float currentHealth;

    public event Action<Character> onCharacterDeath;

    public float MaxHealth => 50;

    public float Health
    {
        get => currentHealth;
        private set
        {
            currentHealth = value;
            if (currentHealth <= 0)
            {
                currentHealth = 0;
                SetDeath();
            }
        }
    }

    public CharacterLiveComponent(Character character)
    {
        selfCharacter = character;
        Health = MaxHealth;
    }

    public void SetDamage(float damage)
    {
        Health -= damage;
        Debug.Log("Got damage = " + damage);
    }

    public void SetDeath()
    {
        onCharacterDeath?.Invoke(selfCharacter);
        Debug.Log("Character is dead");
    }

    public void Initialize(Character selfCharacter)
    {
        this.selfCharacter = selfCharacter;
    }
}
