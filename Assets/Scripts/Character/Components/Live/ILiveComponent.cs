using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILiveComponent : ICharacterComponent
{
    public event Action<Character> onCharacterDeath;

    float MaxHealth { get; }

    float Health { get; }

    public void SetDamage(float damage);
}
