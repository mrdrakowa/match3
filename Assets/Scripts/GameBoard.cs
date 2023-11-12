using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameBoard : MonoBehaviour
{
    public GameObject[] cellPrefabs; // Массив префабов ячеек
    public RectTransform panelRectTransform; // Ссылка на RectTransform компонент панели

    public int rows = 8; // Количество строк
    public int columns = 8; // Количество столбцов
    private GameObject[,] cells; // Массив ячеек игрового поля
    private void Start()
    {
        GenerateGameBoard();
        checkBoard();
        FillEmptyBlocks();
    }
    private void Update()
    {
        checkBoard();
        FillEmptyBlocks();
    }

    public void GenerateGameBoard()
    {
        float panelWidth = panelRectTransform.rect.width;
        float panelHeight = panelRectTransform.rect.height;

        float cellWidth = panelWidth / columns;
        float cellHeight = panelHeight / rows;

        Vector3 startPosition = panelRectTransform.position - new Vector3(panelWidth / 2f, panelHeight / 2f);

        cells = new GameObject[columns, rows]; // Инициализация массива ячеек

        for (int row = 0; row < rows; row++)
        {
            for (int column = 0; column < columns; column++)
            {
                Vector3 position = startPosition + new Vector3(column * cellWidth + cellWidth / 2f, row * cellHeight + cellHeight / 2f);
                GameObject randomCellPrefab = cellPrefabs[UnityEngine.Random.Range(0, cellPrefabs.Length)]; // Случайный выбор префаба ячейки
                GameObject cell = Instantiate(randomCellPrefab, position, Quaternion.identity);
                cell.transform.SetParent(panelRectTransform, false); // Устанавливаем панель в качестве родительского объекта и сохраняем локальные координаты

                cells[column, row] = cell; // Сохраняем ссылку на созданную ячейку
            }
        }
    }
    public void checkBoard()
    {
        // Проверка вертикальных комбинаций
        for (int i = 0; i < columns; i++)
        {
            for (int j = 0; j < rows - 2; j++)
            {
                if (cells[i, j] != null && cells[i, j + 1] != null && cells[i, j + 2] != null &&
                    cells[i, j].CompareTag(cells[i, j + 1].tag) && cells[i, j + 1].CompareTag(cells[i, j + 2].tag))
                {
                    Destroy(cells[i, j]);
                    Destroy(cells[i, j + 1]);
                    Destroy(cells[i, j + 2]);

                    cells[i, j] = null;
                    cells[i, j + 1] = null;
                    cells[i, j + 2] = null;
                }
            }
        }

        // Проверка горизонтальных комбинаций
        for (int j = 0; j < rows; j++)
        {
            for (int i = 0; i < columns - 2; i++)
            {
                if (cells[i, j] != null && cells[i + 1, j] != null && cells[i + 2, j] != null &&
                    cells[i, j].CompareTag(cells[i + 1, j].tag) && cells[i + 1, j].CompareTag(cells[i + 2, j].tag))
                {
                    Destroy(cells[i, j]);
                    Destroy(cells[i + 1, j]);
                    Destroy(cells[i + 2, j]);

                    cells[i, j] = null;
                    cells[i + 1, j] = null;
                    cells[i + 2, j] = null;
                }
            }
        }
    }

    public void FillEmptyBlocks()
    {
        for (int i = 0; i < columns; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                if (cells[i, j] == null)
                {
                    GameObject randomCellPrefab = cellPrefabs[UnityEngine.Random.Range(0, cellPrefabs.Length)];
                    GameObject cell = Instantiate(randomCellPrefab, GetPositionFromIndices(i, j), Quaternion.identity);
                    cell.transform.SetParent(panelRectTransform, false);

                    cells[i, j] = cell;
                }
            }
        }
    }

    private Vector3 GetPositionFromIndices(int column, int row)
{
    float panelWidth = panelRectTransform.rect.width;
    float panelHeight = panelRectTransform.rect.height;

    float cellWidth = panelWidth / columns;
    float cellHeight = panelHeight / rows;

    Vector3 startPosition = panelRectTransform.position - new Vector3(panelWidth / 2f, panelHeight / 2f);
    Vector3 position = startPosition + new Vector3(column * cellWidth + cellWidth / 2f, row * cellHeight + cellHeight / 2f);

    return position;
}


}