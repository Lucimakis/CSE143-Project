using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private BoardManager BM;
    // Start is called before the first frame update
    void Awake()
    {
        BM = GetComponent<BoardManager>();
        InitGame();
    }

    private void InitGame()
    {
        BM.setUpScene();
    }
}
