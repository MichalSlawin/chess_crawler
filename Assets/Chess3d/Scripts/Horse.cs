using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Horse : Piece
{
    public override bool IsFieldAvailable(Field field, Vector3 startingPosition)
    {
        Vector3 diff = field.transform.position - startingPosition;
        if (((Mathf.Abs(diff.x) <= moveDistance && Mathf.Abs(diff.x) > moveDistance - 1 && Mathf.Abs(diff.z) <= moveDistance / 2 && Mathf.Abs(diff.z) > (moveDistance / 2) - 1)
            || (Mathf.Abs(diff.z) <= moveDistance && Mathf.Abs(diff.z) > moveDistance - 1 && Mathf.Abs(diff.x) <= moveDistance / 2 && Mathf.Abs(diff.x) > (moveDistance / 2) - 1))
            && !field.Equals(occupiedField))
        {
            return true;
        }
        return false;
    }
}
