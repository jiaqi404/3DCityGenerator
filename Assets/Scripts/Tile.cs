using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    // x axis
    public Tile[] rightNeighbours;
    public Tile[] leftNeighbours;
    // y axis
    public Tile[] upNeighbours;
    public Tile[] downNeighbours;
    // z axis
    public Tile[] frontNeighbours;
    public Tile[] backNeighbours;

    private void Awake()
    {
        transform.localScale = Vector3.zero;

        // pop out animation
        transform.DOScale(Vector3.one, 1f)
            .SetEase(Ease.OutElastic);
    }
}
