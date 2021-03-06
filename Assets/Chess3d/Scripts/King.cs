﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class King : Piece
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    public override bool IsFieldAvailable(Field field, Vector3 startingPosition)
    {
        Vector3 diff = field.transform.position - startingPosition;
        if (Mathf.Abs(diff.x) <= moveDistance && Mathf.Abs(diff.z) <= moveDistance)
        {
            return true;
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
            if (IsFieldAvailable(field, startingPosition) && !field.Destroyed && !field.Equals(occupiedField))
            {
                availableFields.Add(field);
            }
        }

        if(availableFields.Count > 8)
        {
            throw new System.Exception("King cannot have more than 8 fields available");
        }

        return availableFields;
    }
}
