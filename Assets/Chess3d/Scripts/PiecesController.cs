using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PiecesController : MonoBehaviour
{
    private Color32 selectColor = new Color32(107, 230, 46, 255);
    private Color32 originalColor = new Color32(255, 255, 255, 255);

    // Update is called once per frame
    void Update()
    {
        HandleSelection();
    }

    private void HandleSelection()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100))
            {
                GameObject gameObject = hit.transform.gameObject;
                if (gameObject.CompareTag("PlayerControllable"))
                {
                    HandlePieceSelection(gameObject);
                }
            }
        }
    }

    private void HandlePieceSelection(GameObject gameObject)
    {
        GameObject baseObj = gameObject.transform.GetChild(0).gameObject;

        Color32 currentColor = baseObj.GetComponent<Renderer>().material.color;

        if (currentColor.Equals(selectColor))
        {
            baseObj.GetComponent<Renderer>().material.color = originalColor;
        }
        else if (currentColor.Equals(originalColor))
        {
            baseObj.GetComponent<Renderer>().material.color = selectColor;
        }
    }
}
