using UnityEngine;

// Generates all the possible states for a Unit, enables easier object locating easier later on
public class UnitStatesFactory
{
    public UnitSelected unitSelected;
    public UnitIdle unitIdle;
    
    public UnitStatesFactory(Unit unit) 
    {
        unitSelected = new UnitSelected(unit, this);
        unitIdle = new UnitIdle(unit, this);
    }
}

public class UnitSelected : State
{
    private readonly Unit unit;
    private readonly UnitStatesFactory states;

    public UnitSelected(Unit unit, UnitStatesFactory stateGenerator)
    {
        this.unit = unit;
        states = stateGenerator;
    }

    public void TakeAction()
    {
        unit.GetGameBoard().SetUnitToMove(unit);
        unit.GetGameBoard().ResetHighlightedTiles();
        unit.SetState(states.unitIdle);
    }
}

public class UnitIdle : State
{
    private readonly Unit unit;
    private readonly UnitStatesFactory states;

    public UnitIdle(Unit unit, UnitStatesFactory stateGenerator)
    {
        this.unit = unit;
        states = stateGenerator;
    }

    public void TakeAction()
    {
        unit.GetGameBoard().HighlightPotentialMoves(unit);
        unit.SetState(states.unitSelected);
    }
}
