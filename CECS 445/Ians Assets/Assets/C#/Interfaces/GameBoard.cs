using Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface GameBoard
{
    void AwaitUserSelection(Tileable player);
    void MovePlayerToTile(Tileable destination);
    void CheckForEndGame();
    void RecordTileMovement(Tileable tile, int newColumn, int newRow);
    void HighlightPotentialMoves(Tileable userUnit);
    void ResetHighlightedTiles();
}
