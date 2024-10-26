using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultyObject : MonoBehaviour
{
    [SerializeField] private GameDifficulty difficulty;
    [SerializeField] private MinigameMenu menu;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Bullet"))
        {
            menu.SetDifficulty(difficulty);
        }
    }
}

public enum GameDifficulty
{
    Easy = 1,
    Medium = 2,
    Hard = 3,
}