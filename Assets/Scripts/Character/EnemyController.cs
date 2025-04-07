using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private EnemyCharacter[] enemies; 

    private void Start()
    {
        enemies = GetComponentsInChildren<EnemyCharacter>();

        if (GameManager.Instance != null)
        {
            Character player = GameManager.Instance.CharacterFactory.Player;
            if (player != null)
            {
                foreach (var enemy in enemies)
                {
                    // ������ ���� ����� ������������ ������ ������ ��� ����
                    // �� ����� ������ �������� CharacterTarget ��������
                    enemy.CharacterTarget = player;
                    enemy.Initialize(); // ���� ���������, �������� ������ ��� �������������
                }
            }
        }
        else
        {
            Debug.LogError("GameManager instance not found.");
        }
    }

    private void Update()
    {
        foreach (var enemy in enemies)
        {
            enemy.Update();  // ���������� ������ ������� �����
        }
    }

}
