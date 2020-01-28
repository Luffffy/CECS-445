using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Convert
{
    // Round X coordinate to nearet int
    public static int RoundXCoordToInt(float location)
    {
        return (int) Math.Round(location, 0);
    }

    public static int RoundYCoordToPosInt(float location)
    {
        return -1* (int)Math.Round(location, 0);
    }
}
