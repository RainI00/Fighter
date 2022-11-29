using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InGame;
public class GameManager : MonoBehaviour
{
    [SerializeField] Main _main;
    // Start is called before the first frame update
    void Start()
    {
        _main.Initialize();
        StartCoroutine(Co_StartGame());
    }

    private IEnumerator Co_StartGame()
    {
        yield return _main.Co_StartNewStage();
        yield return _main.Co_StartGame();
    }
}
