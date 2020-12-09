using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PiecesController : MonoBehaviour
{
    private Piece selectedPiece = null;
    private GameObject selectedObject;

    private bool computerMoveFinished = true;
    private int turnNumber = 1;
    private float fieldSize = 2;

    public float currentFieldsXToDestroy = -1;
    public float currentFieldsZToDestroy = -1;
    public int turnsNumberToDestroy = 3;
    public int dummiesToPlace = 1;
    public int enemiesToEnrage = 1;
    public Pawn pawnPrefab;
    public string nextLevelName = "";

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

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
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
                    
                    if(!selectedPiece.waitMode)
                    {
                        HandlePieceSelection();
                    }
                }
                else if (selectedObject.CompareTag("Field") && selectedPiece != null && selectedPiece.Selected)
                {
                    HandleFieldSelection();
                }
                else if (selectedObject.CompareTag("Field") && (selectedPiece == null || !selectedPiece.Selected))
                {
                    if(dummiesToPlace > 0)
                    {
                        PlacePawnDummy();
                    }
                }
                else if (selectedObject.CompareTag("ComputerControllable") && selectedPiece != null && selectedPiece.Selected)
                {
                    HandleEnemySelection();
                }
                else if (selectedObject.CompareTag("ComputerControllable") && (selectedPiece == null || !selectedPiece.Selected))
                {
                    if(enemiesToEnrage > 0)
                    {
                        EnrageEnemy();
                    }
                    
                }
            }
        }
    }

    //-----------------------------------------------------------------------------------------------------

    private void EnrageEnemy()
    {
        Piece selectedEnemy = selectedObject.GetComponent<Piece>();
        selectedEnemy.attackAllMode = true;
        selectedObject.transform.GetChild(0).GetComponent<MeshRenderer>().material.color = Color.red;
        enemiesToEnrage--;
    }

    //-----------------------------------------------------------------------------------------------------

    private void PlacePawnDummy()
    {
        Field selectedField = selectedObject.GetComponent<Field>();
        Vector3 position = selectedField.transform.position;

        if (!selectedField.Occupied)
        {
            pawnPrefab.occupiedField = selectedField;
            Instantiate(pawnPrefab, new Vector3(position.x, position.y+1, position.z), Quaternion.identity);
            dummiesToPlace--;
        }
    }

    //-----------------------------------------------------------------------------------------------------

    private void HandleEnemySelection()
    {
        Piece selectedEnemy = selectedObject.GetComponent<Piece>();

        if (selectedEnemy.occupiedField.Available)
        {
            HandlePieceSelection();
            selectedPiece.Attack(selectedEnemy, 10);

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
                string nextScene = nextLevelName;
                if (nextScene == "") nextScene = SceneManager.GetActiveScene().name;
                StartCoroutine(LoadLevelAfterDelay(nextScene, selectedPiece.moveTime + 0.5f));
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
        if (selectedPiece.Selected && !selectedPiece.IsDead) // deselect piece
        {
            selectedPiece.Selected = false;
            ChangeFieldsAvailability(false);
        }
        else if (!selectedPiece.IsDead) // select piece
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
        DestroyFields();
        yield return new WaitForSeconds(waitTime);

        GameObject[] gos = GameObject.FindGameObjectsWithTag("ComputerControllable");
        Pawn[] pawns = FindObjectsOfType<Pawn>();

        bool allPawnsWaiting = true;
        foreach (Pawn pawn in pawns)
        {
            if (!pawn.IsDead && pawn.CompareTag("ComputerControllable"))
            {
                DoComputerPieceMove(pawn);
                if (!pawn.waitMode) allPawnsWaiting = false;
            }
        }
        if (pawns.Length > 0 && !allPawnsWaiting) yield return new WaitForSeconds(pawns[0].moveTime);

        foreach (GameObject go in gos)
        {
            if(go != null)
            {
                Piece enemy = go.GetComponent<Piece>();
                if (!(enemy is Pawn) && !enemy.IsDead)
                {
                    float afterMoveDelay = DoComputerPieceMove(enemy);
                    yield return new WaitForSeconds(afterMoveDelay);
                }
                
            }
        }
        turnNumber++;
        computerMoveFinished = true;
    }

    //-----------------------------------------------------------------------------------------------------

    // Returns how much time should game wait after move
    private float DoComputerPieceMove(Piece enemy)
    {
        Piece pieceToAttack = enemy.GetComputerPieceToAttack();
        if (pieceToAttack != null && !pieceToAttack.IsDead)
        {
            enemy.Attack(pieceToAttack, 2);
            return enemy.moveTime + 0.5f;
        }
        else if(!enemy.waitMode)
        {
            enemy.MoveTo(enemy.GetComputerFieldToMove());
            return enemy.moveTime;
        }
        return 0;
    }

    //-----------------------------------------------------------------------------------------------------

    private void DestroyFields()
    {
        if(turnNumber % turnsNumberToDestroy == 0)
        {
            Field[] fields = Field.FindObjectsOfType<Field>();

            foreach(Field field in fields)
            {
                if(field.color != "golden")
                {
                    if (currentFieldsXToDestroy != -1 && field.transform.position.x == currentFieldsXToDestroy)
                    {
                        field.DestroyField();
                    }
                    if (currentFieldsZToDestroy != -1 && field.transform.position.z == currentFieldsZToDestroy)
                    {
                        field.DestroyField();
                    }
                }
                
            }
            if (currentFieldsXToDestroy != -1) currentFieldsXToDestroy += fieldSize;
            if (currentFieldsZToDestroy != -1) currentFieldsZToDestroy += fieldSize;
        }
    }
}
