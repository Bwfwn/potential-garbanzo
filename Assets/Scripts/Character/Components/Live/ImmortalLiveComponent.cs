using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImmortalLiveComponent : ILiveComponent
{
    public float MaxHealth {  get => 1; set { } }
    public float Health { get => 1; set { } }

    public event Action<Character> onCharacterDeath;

    public void Initialize(Character selfCharacter)
    {
        //throw new NotImplementedException();
    }

    public void SetDamage(float damage)
    {
        Debug.Log("I am immortal");
    }
}
