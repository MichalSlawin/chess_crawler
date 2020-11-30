using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bishop : Piece
{
    private bool IsFieldAvailable(Field field, Vector3 startingPosition)
    {
        Vector3 fieldPosition = field.transform.position;
        Vector3 diff = fieldPosition - startingPosition;
        if (Mathf.Abs(Mathf.RoundToInt(diff.x)) == Mathf.Abs(Mathf.RoundToInt(diff.z)) && Mathf.Abs(diff.x) <= moveDistance && Mathf.Abs(diff.z) <= moveDistance)
        {
            bool blocked = false;
            Piece[] pieces = Piece.FindObjectsOfType<Piece>();
            foreach (Piece piece in pieces)
            {
                if (piece.tag == "ComputerControllable" && !piece.Equals(this) && !piece.IsDead)
                {
                    Vector3 piecePosition = piece.occupiedField.transform.position;
                    Vector3 pieceDiff = piecePosition - startingPosition;
                    if (Mathf.Abs(Mathf.RoundToInt(pieceDiff.x)) == Mathf.Abs(Mathf.RoundToInt(pieceDiff.z)))
                    {
                        if ((piecePosition.x < startingPosition.x && piecePosition.x > fieldPosition.x) || (piecePosition.x > startingPosition.x && piecePosition.x < fieldPosition.x)
                            || (piecePosition.z < startingPosition.z && piecePosition.z > fieldPosition.z) || (piecePosition.z > startingPosition.z && piecePosition.z < fieldPosition.z))
                        {
                            blocked = true;
                            break;
                        }
                    }
                }
            }
            if (!blocked) return true;
            
        }
        return false;
    }

    public override List<Field> GetAvailableFields()
    {
        List<Field> availableFields = new List<Field>();
        Field[] fields = Field.FindObjectsOfType<Field>();

        Vector3 startingPosition = occupiedField.transform.position;
        foreach (Field field in fields)
        {
            if (IsFieldAvailable(field, startingPosition) && !field.Occupied && !field.Destroyed)
            {
                availableFields.Add(field);
            }
        }

        return availableFields;
    }

    public override Field GetComputerFieldToMove()
    {
        List<Field> fields = GetAvailableFields();

        int index = Random.Range(0, fields.Count);

        if (fields.Count == 0) return null;
        return fields[index];
    }

    public override Piece GetComputerPieceToAttack()
    {
        Piece[] pieces = Piece.FindObjectsOfType<Piece>();

        Vector3 startingPosition = occupiedField.transform.position;
        foreach (Piece piece in pieces)
        {
            if (piece.tag == "PlayerControllable" && IsFieldAvailable(piece.occupiedField, startingPosition))
            {
                return piece;
            }
        }
        return null;
    }
}
