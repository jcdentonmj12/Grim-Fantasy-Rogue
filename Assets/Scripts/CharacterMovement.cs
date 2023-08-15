using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // Speed of the movement
    private bool isMoving;
    private Vector3 targetPosition;
    Animator anim;

    private TileMapGenerator tileMapGenerator;

    private void Start()
    {
        anim = GetComponent<Animator>();
        tileMapGenerator = FindObjectOfType<TileMapGenerator>();
    }

    private void Update()
    {
        if (isMoving)
        {
            float step = moveSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);
            //Debug.Log($"Moving from {transform.position} to {targetPosition}");

            if (Vector3.Distance(transform.position, targetPosition) < 0.001f)
            {
                isMoving = false;   
            }
        }

        else
        {
            Move();
        }
    }

    private void Move()
    {
        int xDir = 0;
        int yDir = 0;
        anim.SetBool("moving", false);
        if (Input.GetKey(KeyCode.UpArrow))
        {
            yDir = 1;
            //anim.SetInteger("Dir", 1);
            anim.SetBool("moving", true);

        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            yDir = -1;
            //anim.SetInteger("Dir", 0);
            anim.SetBool("moving", true);

        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            xDir = -1;
            FlipSprite(-1);  // Flip to left
            anim.SetBool("moving", true);

        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            xDir = 1;
            FlipSprite(1);  // Keep it or flip it back to right
            anim.SetBool("moving", true);

        }

        Debug.Log($"Input detected: xDir = {xDir}, yDir = {yDir}");

        if (xDir != 0 || yDir != 0)
        {
            int x = Mathf.RoundToInt(transform.position.x) + xDir;
            int y = Mathf.RoundToInt(transform.position.y) + yDir;

            /*if (x >= 0 && x < tileMapGenerator.mapWidth && y >= 0 && y < tileMapGenerator.mapHeight &&
                TileMapGenerator.tileDataMatrix[x, y].CanWalk)*/
            {
                targetPosition = new Vector3(x, y, transform.position.z);
                isMoving = true;
            }
        }
    }

    private void FlipSprite(int direction)
    {
        if (direction == 1)  // Right
            transform.localScale = new Vector3(1, 1, 1);
        else if (direction == -1)  // Left
            transform.localScale = new Vector3(-1, 1, 1);
    }


}
