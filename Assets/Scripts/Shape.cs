using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shape : PersistableObject
{
    private int shapeId;

    public int ShapeId
    {
        get => shapeId;
        set
        {
            shapeId = value;
        }
    }
}
