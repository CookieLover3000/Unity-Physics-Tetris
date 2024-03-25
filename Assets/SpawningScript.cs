using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawningScript : MonoBehaviour
{
    [SerializeField] private GameObject[] tetrominoes;
    [SerializeField] private bool a;
    
    // Start is called before the first frame update
    void Start()
    {
        NewTetromino();
    }

    private void Update()
    {
        if (a)
        {
            a = false;
            NewTetromino();
        }
    }

    public void NewTetromino()
    {
        Instantiate(tetrominoes[Random.Range(0, tetrominoes.Length)], transform.position, Quaternion.identity);
    }
}
