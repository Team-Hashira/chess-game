using UnityEngine;

public class CrogenTestScript : MonoBehaviour
{
    [SerializeField] private TurnManager _turnManager;

    void Start()
    {
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            _turnManager.UseTurnCurTeam();
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            _turnManager.NextTurn();
            _turnManager.AddTurnCurTeam();
            Debug.Log(_turnManager.CurTeam);
        }
    }
}
