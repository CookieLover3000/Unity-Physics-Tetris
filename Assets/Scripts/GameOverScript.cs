using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

/*
 * Script that handles the collision of the 'floor' and resetting the game.
 */
public class GameOverScript : MonoBehaviour
{
    
    public bool GameOver { get; private set; }
    
    // text objects for displaying game over text
    [SerializeField] private TMP_Text[] texts = new TMP_Text[2];
    void Start()
    {
        // disable texts because we just started
        foreach(var text in texts)
            text.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // when game over game can be reset using the 'up arrow' key
        if (GameOver)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
                ResetGame();
        }
    }

    // collision for floor plane
    private void OnCollisionEnter2D()
    {
        // game over :(
        GameOver = true;
        // enable texts
        foreach (var text in texts)
            text.gameObject.SetActive(true);
        
    }

    private void ResetGame()
    {
        // reload the scene to reset the game
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }
}
