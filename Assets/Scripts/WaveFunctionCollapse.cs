using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class WaveFunctionCollapse : MonoBehaviour
{
    public int dimensions;
    public Tile[] tileObjects;
    public List<Cell> gridComponents;
    public Cell cellObj;
    public Tile backupTile;
    public float iterationTime = 0.025f;

    private int iteration;

    private void Awake()
    {
        //gridComponents = new List<Cell>();
        //InitializeGrid();
    }

    public void StartWaveFunctionCollapse()
    {
        gridComponents = new List<Cell>();
        InitializeGrid();
    }

    // Initialize grid by instantiating cell objects in a d*d*d space
    void InitializeGrid()
    {
        for(int y= 0; y < dimensions; y++)
        {
            for (int z = 0; z < dimensions; z++)
            {
                for (int x = 0; x < dimensions; x++)
                {
                    Cell newCell = Instantiate(cellObj, new Vector3(x, y, z), Quaternion.identity);
                    newCell.CreateCell(false, tileObjects);
                    gridComponents.Add(newCell);
                }
            }
        }
        
        StartCoroutine(CheckEntropy());
    }

    // Sort out the cells with the lowest entropy (length of options)
    IEnumerator CheckEntropy()
    {
        List<Cell> tempGrid = new List<Cell>(gridComponents);
        tempGrid.RemoveAll(c => c.collapsed);
        tempGrid.Sort((a, b) => a.tileOptions.Length - b.tileOptions.Length);
        tempGrid.RemoveAll(a => a.tileOptions.Length != tempGrid[0].tileOptions.Length);

        yield return new WaitForSeconds(iterationTime);

        CollapseCell(tempGrid);
    }

    // Pick one ramdonly and collapse it by instantiating one random possible tile
    void CollapseCell(List<Cell> tempGrid)
    {
        int randIndex = UnityEngine.Random.Range(0, tempGrid.Count);

        Cell cellToCollapse = tempGrid[randIndex];

        cellToCollapse.collapsed = true;
        try
        {
            Tile selectedTile = cellToCollapse.tileOptions[UnityEngine.Random.Range(0, cellToCollapse.tileOptions.Length)];
            cellToCollapse.tileOptions = new Tile[] { selectedTile };
        }
        catch
        {
            Tile selectedTile = backupTile;
            cellToCollapse.tileOptions = new Tile[] { selectedTile };
        }

        Tile foundTile = cellToCollapse.tileOptions[0];
        Instantiate(foundTile, cellToCollapse.transform.position, foundTile.transform.rotation);

        UpdateGeneration();
    }

    // Update entropy by every cell
    void UpdateGeneration()
    {
        List<Cell> newGenerationCell = new List<Cell>(gridComponents);

        for(int y = 0; y < dimensions; y++)
        {
            for(int z = 0; z < dimensions; z++)
            {
                for (int x = 0; x < dimensions; x++)
                {
                    var index = x + z * dimensions + y * dimensions;

                    if (gridComponents[index].collapsed)
                    {
                        newGenerationCell[index] = gridComponents[index];
                    }
                    else
                    {
                        List<Tile> options = new List<Tile>();
                        foreach (Tile t in tileObjects)
                        {
                            options.Add(t);
                        }

                        // check validity in x axis
                        if (x < dimensions - 1)
                        {
                            Cell left = gridComponents[x + 1 + z * dimensions + y * dimensions];
                            List<Tile> validOptions = new List<Tile>();

                            foreach(Tile possibleOptions in left.tileOptions)
                            {
                                var validOption = Array.FindIndex(tileObjects, obj => obj == possibleOptions);
                                var valid = tileObjects[validOption].rightNeighbours;

                                validOptions = validOptions.Concat(valid).ToList();
                            }

                            CheckValidity(options, validOptions);
                        }
                        if (x > 0)
                        {
                            Cell right = gridComponents[x - 1 + z * dimensions + y * dimensions];
                            List<Tile> validOptions = new List<Tile>();

                            foreach (Tile possibleOptions in right.tileOptions)
                            {
                                var validOption = Array.FindIndex(tileObjects, obj => obj == possibleOptions);
                                var valid = tileObjects[validOption].leftNeighbours;

                                validOptions = validOptions.Concat(valid).ToList();
                            }

                            CheckValidity(options, validOptions);
                        }

                        // check validity in z axis
                        if (z < dimensions - 1)
                        {
                            Cell back = gridComponents[x + (z + 1) * dimensions + y * dimensions];
                            List<Tile> validOptions = new List<Tile>();

                            foreach (Tile possibleOptions in back.tileOptions)
                            {
                                var validOption = Array.FindIndex(tileObjects, obj => obj == possibleOptions);
                                var valid = tileObjects[validOption].frontNeighbours;

                                validOptions = validOptions.Concat(valid).ToList();
                            }

                            CheckValidity(options, validOptions);
                        }
                        if (z > 0)
                        {
                            Cell front = gridComponents[x + (z - 1) * dimensions + y * dimensions];
                            List<Tile> validOptions = new List<Tile>();

                            foreach (Tile possibleOptions in front.tileOptions)
                            {
                                var validOption = Array.FindIndex(tileObjects, obj => obj == possibleOptions);
                                var valid = tileObjects[validOption].backNeighbours;

                                validOptions = validOptions.Concat(valid).ToList();
                            }

                            CheckValidity(options, validOptions);
                        }

                        // check validity in y axis
                        if (y < dimensions - 1)
                        {
                            Cell down = gridComponents[x + z * dimensions + (y + 1) * dimensions];
                            List<Tile> validOptions = new List<Tile>();

                            foreach (Tile possibleOptions in down.tileOptions)
                            {
                                var validOption = Array.FindIndex(tileObjects, obj => obj == possibleOptions);
                                var valid = tileObjects[validOption].upNeighbours;

                                validOptions = validOptions.Concat(valid).ToList();
                            }

                            CheckValidity(options, validOptions);
                        }
                        if (y > 0)
                        {
                            Cell up = gridComponents[x + z * dimensions + (y - 1) * dimensions];
                            List<Tile> validOptions = new List<Tile>();

                            foreach (Tile possibleOptions in up.tileOptions)
                            {
                                var validOption = Array.FindIndex(tileObjects, obj => obj == possibleOptions);
                                var valid = tileObjects[validOption].downNeighbours;

                                validOptions = validOptions.Concat(valid).ToList();
                            }

                            CheckValidity(options, validOptions);
                        }

                        Tile[] newTileList = new Tile[options.Count];

                        for (int i = 0; i < options.Count; i++)
                        {
                            newTileList[i] = options[i];
                        }

                        newGenerationCell[index].RecreateCell(newTileList);
                    }
                }
            }
        }

        gridComponents = newGenerationCell;
        iteration++;

        if (iteration < dimensions * dimensions * dimensions)
        {
            StartCoroutine(CheckEntropy());
        }
    }

    // check the validity of tiles in optionList and remove those invalid
    void CheckValidity(List<Tile> optionList, List<Tile> validOption)
    {
        for(int x = optionList.Count - 1; x >=0; x--)
        {
            var element = optionList[x];
            if (!validOption.Contains(element))
            {
                optionList.RemoveAt(x);
            }
        }
    }
}
