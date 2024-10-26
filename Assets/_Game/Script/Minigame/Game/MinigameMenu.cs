using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class MinigameMenu : MonoBehaviour
{
    [SerializeField] private TextMeshPro title;

    public void SetDifficulty(GameDifficulty gameDifficulty)
    {
        MinigameManager.Instance.StartGame(gameDifficulty);
        Destroy(gameObject);
    }

    public void SetTitle(bool newGame)
    {
        if(newGame)
        {
            title.text = "Choose Difficulty to Play";
        }
        else
        {
            title.text = "Game over! Choose Difficulty to Play Again";
        }
    }
}

