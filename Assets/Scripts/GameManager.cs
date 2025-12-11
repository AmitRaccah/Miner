using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public LevelDataBase LevelData;

    public int currentLevel;

    void Start()
    {
        StartGame();
    }


    public void StartGame()
    {
        LevelDataSO data = LevelData.GetLevel(currentLevel);
        SpawnManager.instance.CreateLevel(data);
        Instantiate(data.backgroundPrefab);

    }
}
