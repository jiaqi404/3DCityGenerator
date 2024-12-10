using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    // x axis
    public Tile[] rightNeighbours; // x + 1
    public Tile[] leftNeighbours; // x - 1
    // y axis
    public Tile[] upNeighbours; // y + 1
    public Tile[] downNeighbours; // y - 1
    // z axis
    public Tile[] frontNeighbours; // z + 1
    public Tile[] backNeighbours; // z - 1

    private void Awake()
    {
        transform.localScale = Vector3.zero;

        // pop out animation
        transform.DOScale(Vector3.one, 1f)
            .SetEase(Ease.OutElastic);
    }
}
