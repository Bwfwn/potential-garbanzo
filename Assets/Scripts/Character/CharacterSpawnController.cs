using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSpawnController : MonoBehaviour
{
    [SerializeField] private GameData gameData;
    private CharacterFactory characterFactory;

    private float spawnTimer;
    private float gameTime;

    private int maxEnemyCount = 3;
    private float nextDifficultyIncreaseTime = 60f; // через каждую минуту

    public void Initialize(CharacterFactory factory)
    {
        characterFactory = factory;
        spawnTimer = gameData.TimeBetweetEnemySpawn;
        gameTime = 0f;
    }

    public void Tick(float deltaTime)
    {
        if (characterFactory == null || characterFactory.Player == null)
            return;

        gameTime += deltaTime;
        spawnTimer -= deltaTime;

        // Увеличиваем сложность каждую минуту
        if (gameTime >= nextDifficultyIncreaseTime)
        {
            maxEnemyCount += 1;
            nextDifficultyIncreaseTime += 60f;
            Debug.Log("Increased max enemies to: " + maxEnemyCount);
        }

        // Спавним врагов если нужно
        if (spawnTimer <= 0)
        {
            if (characterFactory.ActiveCharacters.FindAll(c => c.CharacterType == CharacterType.DefaultEnemy).Count < maxEnemyCount)
            {
                SpawnEnemy();
            }

            spawnTimer = gameData.TimeBetweetEnemySpawn;
        }
    }

    private void SpawnEnemy()
    {
        Character enemy = characterFactory.GetCharacter(CharacterType.DefaultEnemy);

        if (enemy == null)
        {
            Debug.LogError("Failed to spawn enemy.");
            return;
        }

        enemy.CharacterTarget = characterFactory.Player;
        Vector3 playerPos = characterFactory.Player.transform.position;
        enemy.transform.position = new Vector3(playerPos.x + GetOffset(), 0, playerPos.z + GetOffset());
        enemy.gameObject.SetActive(true);
        enemy.Initialize();
        enemy.LiveComponent.onCharacterDeath += GameManager.Instance.CharacterDeathHandler;

        float GetOffset()
        {
            bool isPlus = Random.Range(0, 100) % 2 == 0;
            float offset = Random.Range(gameData.MinSpawnOffset, gameData.MaxSpawnOffset);
            return (isPlus) ? offset : (-1 * offset);
        }
    }
}
