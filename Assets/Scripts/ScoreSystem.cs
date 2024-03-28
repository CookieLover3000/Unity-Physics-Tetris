using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreSystem : MonoBehaviour
{
    // observer. increeases score when a block lands on the platform or on another block.
    private int _score;
    [SerializeField] private TMP_Text _scoreDisplay;
    
    // Start is called before the first frame update
    void Start()
    {
        TetrisBlock.OnBlockHasLanded += IncreaseScore;
    }

    private void IncreaseScore()
    {
        _score += 1;

        _scoreDisplay.text = _score.ToString();
    }

}
