using Interfaces;
using System.Collections.Generic;
using UnityEngine;
using static GridConverter;

// This class outlines all user controlled units.
public class UserControlledUnit : Unit
{
    // Handles Mouse Clicks
    public override void OnMouseDown()
    {
        currentState.TakeAction();
    }

    // Highlights the units tile with a preset color
    public override void Highlight()
    {
        throw new System.NotImplementedException();
    }

    // Removes any highlights applied to the units tile
    public override void RemoveHighLight()
    {
        graphicRenderer.material.color = unhighlightedColor;
        currentState = states.unitIdle;
    }
}

