using Interfaces;
using UnityEngine;

// Generates all the possible states for a tile, enables easier object locating easier later on
public class TileStatesFactory
{
    public TileSelected tileSelected;
    public TileHighlighted tileHighlighted;
    public TileIdle tileIdle;
    public CursorOnHighlightedTile cursorOnHighlightedTile;
    public TileAwaitingMove awaitingMove;

    public TileStatesFactory(EmptyTile tile)
    {
        tileSelected = new TileSelected(tile, this);
        tileHighlighted = new TileHighlighted(tile, this);
        tileIdle = new TileIdle(tile, this);
        cursorOnHighlightedTile = new CursorOnHighlightedTile(tile, this);
        awaitingMove = new TileAwaitingMove(tile, this);
    }
}

public class TileSelected : State
{
    private readonly Tileable tile;
    private readonly TileStatesFactory states;

    public TileSelected(Tileable tile, TileStatesFactory stateGenerator)
    {
        this.tile = tile;
        states = stateGenerator;
    }

    public void TakeAction()
    {
    }
}

public class TileHighlighted : State
{
    private readonly Tileable tile;
    private readonly TileStatesFactory states;

    public TileHighlighted(Tileable tile, TileStatesFactory stateGenerator)
    {
        this.tile = tile;
        states = stateGenerator;
    }

    public void TakeAction()
    {
    }
}

public class TileIdle : State
{
    private readonly Tileable tile;
    private readonly TileStatesFactory states;

    public TileIdle(Tileable tile, TileStatesFactory stateGenerator)
    {
        this.tile = tile;
        states = stateGenerator;
    }

    public void TakeAction()
    {
        tile.RemoveHighLight();

    }
}

public class CursorOnHighlightedTile : State
{
    private readonly Tileable tile;
    private readonly TileStatesFactory states;

    public CursorOnHighlightedTile(Tileable tile, TileStatesFactory stateGenerator)
    {
        this.tile = tile;
        states = stateGenerator;
    }

    public void TakeAction()
    {

    }
}

public class TileAwaitingMove : State
{
    private readonly EmptyTile tile;
    private readonly TileStatesFactory states;

    public TileAwaitingMove(EmptyTile tile, TileStatesFactory stateGenerator)
    {
        this.tile = tile;
        states = stateGenerator;
    }

    public void TakeAction()
    {
        tile.NotifyObservers();
    }
}


