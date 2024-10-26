using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSpawner : MonoBehaviour
{
    [SerializeField] private TargetHolder targetSpawn;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float targetSize = 1.0f;

    private List<List<TargetHolder>> targets = new List<List<TargetHolder>>();

    private void Start()
    {
        MinigameManager.Instance.OnTimerEnded += HandleOnTimerHit;
    }

    public void SpawnTargets(int rows, int columns)
    {
        Vector3 gridOffset = spawnPoint.position
                              + spawnPoint.right * ((columns - 1) * targetSize / 2); // Shift to the right for centering
                              

        // Loop through columns (x)
        for (int x = 0; x < columns; x++)
        {
            // Create a new list for each column
            List<TargetHolder> column = new List<TargetHolder>();

            // Loop through rows (y)
            for (int y = 0; y < rows; y++)
            {
                // Calculate the spawn position
                Vector3 spawnPosition = gridOffset
                                      - spawnPoint.right * (x * targetSize)
                                      + spawnPoint.up * (y * targetSize);

                // Instantiate the target and add it to the column list
                TargetHolder targetHolder = Instantiate(targetSpawn, spawnPosition, spawnPoint.rotation);
                column.Add(targetHolder);

                targetHolder.Setup(this, x, y);
            }

            // Add the column to the 2D list
            targets.Add(column);
        }

        SpawnRandomTarget(-1, -1);
    }

    public void SpawnRandomTarget(int x, int y)
    {
        // Check if the targets list is populated
        if (targets.Count == 0 || targets[0].Count == 0)
        {
            Debug.LogWarning("No targets available.");
            return;
        }

        int randomX, randomY;

        do
        {
            // Get random x and y indices
            randomX = Random.Range(0, targets.Count);          // Random column index (x)
            randomY = Random.Range(0, targets[randomX].Count); // Random row index (y)

        } while (randomX == x && randomY == y); // Continue until different from provided x and y

        TargetHolder selectedTarget = targets[randomX][randomY];

        selectedTarget.ActivateTarget();
    }

    public void OnTargetHit()
    {
        MinigameManager.Instance.IncreaseScoreBy(1);
    }

    private void HandleOnTimerHit()
    {
        // Loop through each column in the targets list
        foreach (var column in targets)
        {
            // Loop through each target in the column
            foreach (var target in column)
            {
                // Deactivate the target
                target.DeactivateTarget();

                // Destroy the target GameObject
                Destroy(target.gameObject);
            }
        }

        // Clear the 2D list after destroying the targets
        targets.Clear();

        MinigameManager.Instance.ShowMenu(false);
    }
}
