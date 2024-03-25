using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScript : MonoBehaviour
{
    // Start is called before the first frame update

    public bool GameOver { get; private set; }
    [SerializeField] private TMP_Text[] texts = new TMP_Text[2];
    void Start()
    {
        foreach(var text in texts)
            text.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameOver)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
                ResetGame();
        }
    }

    private void OnCollisionEnter2D()
    {
        GameOver = true;
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
