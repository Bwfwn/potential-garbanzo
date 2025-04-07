using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameData gameData;
    [SerializeField] private CharacterFactory characterFactory;

    private ScoreSystem scoreSystem;

    private float gameSessionTime;
    private float timeBetweenEnemySpawn;
    private bool isGameActive;
    public static GameManager Instance { get; private set; }

    public CharacterFactory CharacterFactory => characterFactory;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Initialize();
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Initialize()
    {
        scoreSystem = new ScoreSystem();
        isGameActive = false;
    }

    public void StartGame()
    {
        if (isGameActive)
            return;

        Character player = characterFactory.GetCharacter(CharacterType.Player);

        if (player == null)
        {
            Debug.LogError("Player character is not initialized properly in CharacterFactory.");
            return;
        }

        player.transform.position = Vector3.zero;
        player.gameObject.SetActive(true);
        player.Initialize();
        player.LiveComponent.onCharacterDeath += CharacterDeathHandler;

        gameSessionTime = 0;
        timeBetweenEnemySpawn = gameData.TimeBetweetEnemySpawn;

        scoreSystem.StartGame();

        isGameActive = true;
    }

    private void Update()
    {
        if (!isGameActive) // Проверяем, началась ли игра
            return;

        gameSessionTime += Time.deltaTime;
        timeBetweenEnemySpawn -= Time.deltaTime;

        if (timeBetweenEnemySpawn <= 0)
        {
            if (characterFactory.Player != null) // Проверка на наличие игрока
            {
                SpawnEnemy();
            }
            else
            {
                Debug.LogWarning("Player is not initialized yet. Skipping enemy spawn.");
            }
            timeBetweenEnemySpawn = gameData.TimeBetweetEnemySpawn;
        }

        if (gameSessionTime >= gameData.SessionTimeSeconds)
        {
            GameVictory();
        }
    }


    private void CharacterDeathHandler(Character deathCharacter)
    {
        switch (deathCharacter.CharacterType)
        {
            case CharacterType.Player:
                GameOver();
                break;

            case CharacterType.DefaultEnemy:
                scoreSystem.AddScore(deathCharacter.CharacterData.ScoreCost);
                break;
        }
        deathCharacter.gameObject.SetActive(false);
        characterFactory.ReturnCharacter(deathCharacter);

        deathCharacter.LiveComponent.onCharacterDeath -= CharacterDeathHandler;
    }

    private void SpawnEnemy()
    {
        Character enemy = characterFactory.GetCharacter(CharacterType.DefaultEnemy);
        if (characterFactory.Player == null)
        {
            Debug.LogError("Player not assigned in CharacterFactory! Enemy cannot target player.");
            return;
        }
        if (enemy == null)
        {
            Debug.LogError("Failed to spawn enemy.");
            return;
        }

        enemy.CharacterTarget = characterFactory.Player; // Назначаем цель.
        Debug.Log($"Enemy spawned with target: {enemy.CharacterTarget.name}");

        Vector3 playerPosition = characterFactory.Player.transform.position;
        enemy.transform.position = new Vector3(playerPosition.x + GetOffset(), 0, playerPosition.z + GetOffset());
        enemy.gameObject.SetActive(true);
        enemy.Initialize();
        enemy.LiveComponent.onCharacterDeath += CharacterDeathHandler;

        float GetOffset()
        {
            bool isPlus = Random.Range(0, 100) % 2 == 0;
            float offset = Random.Range(gameData.MinSpawnOffset, gameData.MaxSpawnOffset);
            return (isPlus) ? offset : (-1 * offset);
        }
    }


    private void GameVictory()
    {
        scoreSystem.EndGame();
        Debug.Log("Victory");
        isGameActive = false;
    }

    private void GameOver()
    {
        scoreSystem.EndGame();
        Debug.Log("Defeat");
        isGameActive = false;
    }
}
