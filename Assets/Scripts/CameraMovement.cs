using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float characterSpeed = 5;

    private void Update()
    {
        //detect input
        Vector2 movementInput = new Vector2(0, 0);
        if (Input.GetKey(KeyCode.W))
        {
            this.transform.position += new Vector3(0, characterSpeed * Time.deltaTime, 0);
            movementInput.y = 1;
        }
        if (Input.GetKey(KeyCode.S))
        {
            this.transform.position += new Vector3(0, -characterSpeed * Time.deltaTime, 0);
            movementInput.y = -1;
        }
        if (Input.GetKey(KeyCode.A))
        {
            this.transform.position += new Vector3(-characterSpeed * Time.deltaTime, 0, 0);
            movementInput.x = -1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            this.transform.position += new Vector3(characterSpeed * Time.deltaTime, 0, 0);
            movementInput.x = 1;
        }
    }
}

