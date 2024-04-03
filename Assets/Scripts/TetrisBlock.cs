using System.Linq;
using UnityEngine;

/*
 * This script handles the movement and logic of the Tetromino.
 */
public class TetrisBlock : MonoBehaviour
{
    // rotation point for different blocks. Most of the time the "origin" of the tetromino
    [SerializeField] private Vector3 rotationPoint;

    // references to components
    private Rigidbody2D _rb;
    private SpawningScript _spawn;
    private GameOverScript _gameOver;

    // limit of map. Needed because collision is whack.
    private readonly float _sideLimit = 21f / 2f;

    // get the spawner game object
    private GameObject _spawner;

    // enum for cheap state machine
    private enum BlockState
    {
        Holding,
        Falling,
        Landed,
        Done
    }

    private BlockState State { get; set; } = BlockState.Holding;


    // Observer because I wanted to figure out how this works in unity.
    // This class is the subject, ScoreSystem is the observer.
    public delegate void BlockHasLanded();

    public static BlockHasLanded OnBlockHasLanded;

    private void Start()
    {
        _spawner = GameObject.Find("Spawner");
        // initialize objects
        _rb = GetComponent<Rigidbody2D>();
        _rb.gravityScale = 0f;
        State = BlockState.Holding;
        _spawn = FindObjectOfType<SpawningScript>();
        _gameOver = FindObjectOfType<GameOverScript>();
        _rb.freezeRotation = true; // freeze rotation while holding. Otherwise wall collision can break
    }

    // Update is called once per frame
    private void Update()
    {
        // game is over cannot drop more blocks
        if (_gameOver.GameOver)
        {
            // remove the script from the block as it isn't necessary anymore.
            Destroy(this);
            return;
        }

        HandleInputs();
        StateMachine();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // if collision is with wall. don't switch state
        if (collision.contacts.All(contact => contact.normal == Vector2.right || contact.normal == Vector2.left))
            return;

        // switch state because block has landed
        State = BlockState.Landed;
    }

    private void HandleInputs()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            transform.position += new Vector3(-1, 0, 0);

            // control if block is moving off the map. works together with collision on the sides of the playable area
            // probably redundant
            if (Mathf.Abs(transform.position.x) > _sideLimit)
            {
                Transform blockTransform = transform;
                blockTransform.position = new Vector3(-1f * _sideLimit + 0.5f, blockTransform.position.y, 0);
            }
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            transform.position += new Vector3(1, 0, 0);

            // control if block is moving off the map. works together with collision on the sides of the playable area
            // probably redundant
            if (Mathf.Abs(transform.position.x) > _sideLimit)
            {
                Transform blockTransform = transform;
                blockTransform.position = new Vector3(_sideLimit - 0.5f, blockTransform.position.y, 0);
            }
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            // make the block fall.
            _rb.gravityScale = 1f;
            State = BlockState.Falling;
            _rb.freezeRotation = false;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            // rotate the block around a point. Used https://www.youtube.com/watch?v=T5P8ohdxDjo as reference
            transform.RotateAround(transform.TransformPoint(rotationPoint), new Vector3(0, 0, 1), 90);
        }
    }

    // very basic statemachine
    private void StateMachine()
    {
        // block has fallen and can't get up
        if (State == BlockState.Done)
        {
            // remove the script from the block as it isn't necessary anymore. Saves some if checks
            Destroy(this);
        }

        else if (State == BlockState.Holding)
        {
            // makes it so the block moves up with the spawner if the player hasn't dropped it yet.
            transform.position = new Vector3(transform.position.x, _spawner.transform.position.y, transform.position.z);
        }

        else if (State == BlockState.Landed)
        {
            // spawn a new Tetromino
            State = BlockState.Done;
            _spawn.NewTetromino();
            OnBlockHasLanded?.Invoke();
        }
    }
}