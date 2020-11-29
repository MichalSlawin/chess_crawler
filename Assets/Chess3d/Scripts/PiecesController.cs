using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PiecesController : MonoBehaviour
{
    private Piece selectedPiece = null;
    private GameObject selectedObject;

    private Color32 selectColor = new Color32(107, 230, 46, 255);
    private Color32 originalColor = new Color32(255, 255, 255, 255);

    private bool computerMoveFinished = true;

    // Update is called once per frame
    void Update()
    {
        if(computerMoveFinished)
        {
            HandleSelection();
        }

        if(Input.GetKeyDown(KeyCode.P))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
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

        if (selectedEnemy.occupiedField.Available)
        {
            HandlePieceSelection();
            selectedPiece.Attack(selectedEnemy);

            StartCoroutine(DoComputerMove(selectedPiece.moveTime));
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
            if (selectedField.color == "golden")
            {
                StartCoroutine(LoadLevelAfterDelay(SceneManager.GetActiveScene().name, selectedPiece.moveTime + 0.5f));
            }
            else
            {
                StartCoroutine(DoComputerMove(selectedPiece.moveTime));
                selectedPiece = null;
            }
        }
    }

    //-----------------------------------------------------------------------------------------------------

    IEnumerator LoadLevelAfterDelay(string sceneName, float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(sceneName);
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

    private void ChangeFieldsAvailability(bool available)
    {
        List<Field> fields = selectedPiece.GetAvailableFields();

        foreach (Field field in fields)
        {
            field.Available = available;
            field.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = available;
        }
    }

    //-----------------------------------------------------------------------------------------------------

    private IEnumerator DoComputerMove(float waitTime)
    {
        computerMoveFinished = false;
        yield return new WaitForSeconds(waitTime);

        GameObject[] gos = GameObject.FindGameObjectsWithTag("ComputerControllable");
        Pawn[] pawns = FindObjectsOfType<Pawn>();

        foreach (Pawn pawn in pawns)
        {
            if(!pawn.IsDead)
            {
                DoComputerPieceMove(pawn);
            }
        }
        if (pawns.Length > 0) yield return new WaitForSeconds(pawns[0].moveTime);

        foreach (GameObject go in gos)
        {
            if(go != null)
            {
                Piece enemy = go.GetComponent<Piece>();
                if (!(enemy is Pawn) && !enemy.IsDead)
                {
                    DoComputerPieceMove(enemy);
                    yield return new WaitForSeconds(enemy.moveTime);
                }
                
            }
        }
        computerMoveFinished = true;
    }

    //-----------------------------------------------------------------------------------------------------

    private void DoComputerPieceMove(Piece enemy)
    {
        Piece pieceToAttack = enemy.GetComputerPieceToAttack();
        if (pieceToAttack != null)
        {
            enemy.Attack(pieceToAttack);
        }
        else
        {
            enemy.MoveTo(enemy.GetComputerFieldToMove());
        }
    }
}
