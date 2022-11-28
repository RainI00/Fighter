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
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
