﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Queen : Piece
{
    public override bool IsFieldAvailable(Field field, Vector3 startingPosition)
    {
        Vector3 fieldPosition = field.transform.position;
        Vector3 diff = fieldPosition - startingPosition;
        if ((Mathf.Abs(diff.x) <= 0.1 && Mathf.Abs(diff.z) <= moveDistance) || (Mathf.Abs(diff.z) <= 0.1 && Mathf.Abs(diff.x) <= moveDistance)
            || Mathf.Abs(Mathf.RoundToInt(diff.x)) == Mathf.Abs(Mathf.RoundToInt(diff.z)) && Mathf.Abs(diff.x) <= moveDistance && Mathf.Abs(diff.z) <= moveDistance)
        {
            bool blocked = false;
            Piece[] pieces = Piece.FindObjectsOfType<Piece>();
            foreach (Piece piece in pieces)
            {
                if (piece.tag == "ComputerControllable" && !piece.Equals(this) && !piece.IsDead)
                {
                    Vector3 piecePosition = piece.occupiedField.transform.position;
                    Vector3 pieceDiff = piecePosition - startingPosition;
                    // rook part
                    if ((Mathf.Abs(diff.x) <= 0.1 && Mathf.Abs(pieceDiff.x) <= 0.1) || (Mathf.Abs(diff.z) <= 0.1 && Mathf.Abs(pieceDiff.z) <= 0.1))
                    {
                        if ((piecePosition.x < startingPosition.x && piecePosition.x > fieldPosition.x) || (piecePosition.x > startingPosition.x && piecePosition.x < fieldPosition.x)
                            || (piecePosition.z < startingPosition.z && piecePosition.z > fieldPosition.z) || (piecePosition.z > startingPosition.z && piecePosition.z < fieldPosition.z))
                        {
                            
                            blocked = true;
                            break;
                        }

                    }
                    // bishop part
                    if (Mathf.Abs(Mathf.RoundToInt(pieceDiff.x)) == Mathf.Abs(Mathf.RoundToInt(pieceDiff.z)))
                    {
                        // this if wall is a nightmare but I don't see other way
                        if ((piecePosition.x < startingPosition.x && piecePosition.x > fieldPosition.x && piecePosition.z < startingPosition.z && piecePosition.z > fieldPosition.z)
                            || (piecePosition.x > startingPosition.x && piecePosition.x < fieldPosition.x && piecePosition.z < startingPosition.z && piecePosition.z > fieldPosition.z)
                            || (piecePosition.x < startingPosition.x && piecePosition.x > fieldPosition.x && piecePosition.z > startingPosition.z && piecePosition.z < fieldPosition.z)
                            || (piecePosition.x > startingPosition.x && piecePosition.x < fieldPosition.x && piecePosition.z > startingPosition.z && piecePosition.z < fieldPosition.z))
                        {
                            blocked = true;
                            break;
                        }
                    }
                }
            }
            if (!blocked)
            {
                return true;
            }
        }
        return false;
    }
}