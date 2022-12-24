using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movement : MonoBehaviour
{
    public float moveSpeed;
    public Camera cam;
    private Vector2 playerMovement;
    private Vector2 mousePos;
    private CharacterController controller;
    // Start is called before the first frame update
    void Start()
    {
        controller = gameObject.AddComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        playerMovement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        
    }
    void FixedUpdate() {
        controller.Move(playerMovement * Time.fixedDeltaTime * moveSpeed);
    }
}
