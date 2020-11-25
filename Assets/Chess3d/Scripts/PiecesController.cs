using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiecesController : MonoBehaviour
{
    private Piece selectedPiece;
    private GameObject selectedObject;

    private Color32 selectColor = new Color32(107, 230, 46, 255);
    private Color32 originalColor = new Color32(255, 255, 255, 255);

    // Update is called once per frame
    void Update()
    {
        HandleSelection();
    }

    //-----------------------------------------------------------------------------------------------------

    private void HandleSelection()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100))
            {
                selectedObject = hit.transform.gameObject;
                if (selectedObject.CompareTag("PlayerControllable"))
                {
                    selectedPiece = selectedObject.GetComponent<Piece>();
                    HandlePieceSelection();
                }
            }
        }
    }

    //-----------------------------------------------------------------------------------------------------

    private void HandlePieceSelection()
    {
        GameObject baseObj = selectedObject.transform.GetChild(0).gameObject;
        GameObject closestField = FindClosestObject(selectedObject, "Field");
        Color32 currentColor = baseObj.GetComponent<Renderer>().material.color;

        if (currentColor.Equals(selectColor)) // deselect piece
        {
            baseObj.GetComponent<Renderer>().material.color = originalColor;

            ChangeFieldsAvailability(closestField.GetComponent<Field>(), false);
        }
        else if (currentColor.Equals(originalColor)) // select piece
        {
            baseObj.GetComponent<Renderer>().material.color = selectColor;

            ChangeFieldsAvailability(closestField.GetComponent<Field>(), true);
        }
    }

    //-----------------------------------------------------------------------------------------------------

    private GameObject FindClosestObject(GameObject startObject, string tag)
    {
        GameObject[] gos;
        gos = GameObject.FindGameObjectsWithTag(tag);
        GameObject closest = null;
        float distance = Mathf.Infinity;
        Vector3 position = startObject.transform.position;
        foreach (GameObject go in gos)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = go;
                distance = curDistance;
            }
        }
        return closest;
    }

    //-----------------------------------------------------------------------------------------------------

    private void ChangeFieldsAvailability(Field closestField, bool available)
    {
        List<Field> fields = selectedPiece.GetAvailableFields(closestField);

        foreach(Field field in fields)
        {
            field.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = available;
        }
    }
}
