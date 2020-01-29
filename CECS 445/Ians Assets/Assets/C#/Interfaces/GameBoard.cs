using Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface GameBoard
{
    void AwaitUserSelection(Tileable player);
    void CheckForEndGame();
    void HighlightPotentialMoves(Tileable userUnit);
    void ResetHighlightedTiles();
}
