using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class King : Piece
{
    private float moveDistance = 2.1f;

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

    public override List<Field> GetAvailableFields()
    {
        List<Field> availableFields = new List<Field>();
        Field[] fields;
        fields = Field.FindObjectsOfType<Field>();
        
        Vector3 startingPosition = occupiedField.transform.position;
        foreach (Field field in fields)
        {
            Vector3 diff = field.transform.position - startingPosition;
            if(Mathf.Abs(diff.x) <= moveDistance && Mathf.Abs(diff.z) <= moveDistance)
            {
                availableFields.Add(field);
            }
        }

        if(availableFields.Count > 9)
        {
            throw new System.Exception("King cannot have more than 9 fields available");
        }

        return availableFields;
    }
}
