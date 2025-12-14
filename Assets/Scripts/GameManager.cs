using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public LevelDataBase LevelData;

    public int currentLevel;
    private int remainingMainLetters;

    private void OnEnable()
    {
        Letter.OnCollected += HandleLetterCollected;
    }

    private void OnDisable()
    {
        Letter.OnCollected -= HandleLetterCollected;
    }


    void Start()
    {
        StartGame();
    }


    public void StartGame()
    {
        LevelDataSO data = LevelData.GetLevel(currentLevel);

        remainingMainLetters = SpawnManager.instance.CreateLevel(data);
        Instantiate(data.backgroundPrefab);

        Debug.Log($"Main letters to collect: {remainingMainLetters}");
    }

    public void StartNextLevel()
    {
        currentLevel++;
        if (currentLevel >= LevelData.dataSO.Count)
        {
            return;
        }
        StartGame();
    }

    private void HandleLetterCollected(Letter letter)
    {
        if (letter == null)
        {
            return;
        }
        if (!letter.isMainLetter)
        {
            return;
        }

        remainingMainLetters--;

        if (remainingMainLetters <= 0)
        {
            StartNextLevel();
        }
    }
}


//TODO: *Test OnCollected* 1. Count how many main letters in level.  2.Clear level if 0(or if collectedMain = MainLettersInLevel)  3.Move to next stage