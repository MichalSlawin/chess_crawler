using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiecesController : MonoBehaviour
{
    private Piece selectedPiece = null;
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
                else if (selectedObject.CompareTag("Field") && selectedPiece != null)
                {
                    HandleFieldSelection();
                }
                else if (selectedObject.CompareTag("ComputerControllable") && selectedPiece != null)
                {
                    HandleEnemySelection();
                }
            }
        }
    }

    //-----------------------------------------------------------------------------------------------------

    private void HandleEnemySelection()
    {
        Piece selectedEnemy = selectedObject.GetComponent<Piece>();

        if(selectedEnemy.occupiedField.Available)
        {
            HandlePieceSelection();
            selectedPiece.Attack(selectedEnemy);
            selectedPiece = null;
        }
    }

    //-----------------------------------------------------------------------------------------------------

    private void HandleFieldSelection()
    {
        Field selectedField = selectedObject.GetComponent<Field>();

        if (selectedField.Available && !selectedField.Occupied && !selectedField.Equals(selectedPiece.occupiedField))
        {
            HandlePieceSelection();
            selectedPiece.MoveTo(selectedField);
            selectedPiece = null;
        }
    }

    //-----------------------------------------------------------------------------------------------------

    private void HandlePieceSelection()
    {
        if (selectedPiece.Selected) // deselect piece
        {
            selectedPiece.Selected = false;
            ChangeFieldsAvailability(false);
        }
        else // select piece
        {
            selectedPiece.Selected = true;
            ChangeFieldsAvailability(true);
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

    private void ChangeFieldsAvailability(bool available)
    {
        List<Field> fields = selectedPiece.GetAvailableFields();

        foreach(Field field in fields)
        {
            field.Available = available;
            field.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = available;
        }
    }
}
