using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Horse : Piece
{
    public override List<Field> GetAvailableFields()
    {
        List<Field> availableFields = new List<Field>();
        Field[] fields = Field.FindObjectsOfType<Field>();

        Vector3 startingPosition = occupiedField.transform.position;
        foreach (Field field in fields)
        {
            if (IsFieldAvailable(field, startingPosition) && !field.Destroyed)
            {
                availableFields.Add(field);
            }
        }

        if (availableFields.Count > 8)
        {
            throw new System.Exception("Horse cannot have more than 8 fields available");
        }

        return availableFields;
    }

    private bool IsFieldAvailable(Field field, Vector3 startingPosition)
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
