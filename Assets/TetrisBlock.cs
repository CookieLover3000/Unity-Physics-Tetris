using System.Linq;
using UnityEngine;

public class TetrisBlock : MonoBehaviour
{

    [SerializeField] private Vector3 rotationPoint;
    
    private Rigidbody2D _rb;
    private SpawningScript _spawn;
    private GameOverScript _gameOver;

    private float sideLimit = 21f / 2f;


    private enum BlockState
    {
        Holding,
        Falling,
        Landed,
        Done
    }

    private BlockState State { get; set; } = BlockState.Holding;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.gravityScale = 0f;
        State = BlockState.Holding;
        _spawn = FindObjectOfType<SpawningScript>();
        _gameOver = FindObjectOfType<GameOverScript>();
        _rb.freezeRotation = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (_gameOver.GameOver)
            return;
        if (State == BlockState.Done)
        {
            TetrisBlock script = GetComponent<TetrisBlock>();
            Destroy(script);
            return;
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            transform.position += new Vector3(-1, 0, 0);
            
            if (Mathf.Abs(transform.position.x) > sideLimit)
                transform.position = new Vector3(-1f * sideLimit + 0.5f, transform.position.y , 0);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            transform.position += new Vector3(1, 0, 0);
            
            if (Mathf.Abs(transform.position.x) > sideLimit)
                transform.position = new Vector3(sideLimit - 0.5f, transform.position.y , 0);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            _rb.gravityScale = 1f;
            State = BlockState.Falling;
            _rb.freezeRotation = false;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), 90);
        }

        if (State == BlockState.Landed && State != BlockState.Done)
        {
            State = BlockState.Done;

            _spawn.NewTetromino();

        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        foreach (var contact in collision.contacts)
        {
            Debug.Log(contact.normal);
        }
        if(collision.contacts.All(contact => contact.normal == Vector2.right || contact.normal == Vector2.left))
            return;
        
        if(State != BlockState.Done)
            State = BlockState.Landed;
    }
}