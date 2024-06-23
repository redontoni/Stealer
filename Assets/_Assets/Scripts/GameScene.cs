using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : MonoBehaviour
{
    public GameObject elementPrefab; // The prefab of the element to spawn.
    private int rows = 10; // Number of rows in the matrix.
    private int columns = 5; // Number of columns in the matrix.
    public float spacingX = 6.3f; // Spacing between elements along the X-axis.
    public float spacingY = 6.3f; // Spacing between elements along the Y-axis.
    public Vector3 startPosition = new Vector3(-12.64f, 5f, 60f); // Starting position.

    private void Start()
    {
        SpawnMatrix();
    }

    private void SpawnMatrix()
    {
        Vector3 spawnPosition = startPosition;

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                // Skip the second element of the first row
                if (row == 0 && col == 1)
                {
                    spawnPosition.x += spacingX;
                    continue;
                }

                Instantiate(elementPrefab, spawnPosition, Quaternion.identity);
                spawnPosition.x += spacingX;
            }

            spawnPosition.x = startPosition.x; // Reset X position for the next row
            spawnPosition.y -= spacingY; // Move down along the Y-axis for the next row
        }
    }
}
