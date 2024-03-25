using UnityEngine;
using Random = UnityEngine.Random;

/*
 * Script is used for creating new Tetromino
 */
public class SpawningScript : MonoBehaviour
{
    [SerializeField] private GameObject[] tetrominoes;
    
    void Start()
    {
        // Create a Tetromino at the start of the game
        NewTetromino();
    }

    // Create a random Tetromino
    public void NewTetromino()
    {
        Instantiate(tetrominoes[Random.Range(0, tetrominoes.Length)], transform.position, Quaternion.identity);
    }
}
