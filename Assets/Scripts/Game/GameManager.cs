using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameData gameData;
    [SerializeField] private CharacterFactory characterFactory;
    [SerializeField] private CharacterSpawnController spawnController;

    private ScoreSystem scoreSystem;

    private float gameSessionTime;
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

        gameSessionTime = 0f;
        scoreSystem.StartGame();
        isGameActive = true;

        spawnController.Initialize(characterFactory);
    }

    private void Update()
    {
        if (!isGameActive)
            return;

        float deltaTime = Time.deltaTime;

        gameSessionTime += deltaTime;

        spawnController.Tick(deltaTime);

        if (gameSessionTime >= gameData.SessionTimeSeconds)
        {
            GameVictory();
        }
    }

    public void CharacterDeathHandler(Character deathCharacter)
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
