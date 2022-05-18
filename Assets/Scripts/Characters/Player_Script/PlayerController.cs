using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Vector2 input;

    private Character character;

    private void Awake()
    {
        character = GetComponent<Character>();
    }

    public void HandleUpdate()
    {
        if (!character.IsMoving)
        {
            input.x = Input.GetAxisRaw("Horizontal");
            input.y = Input.GetAxisRaw("Vertical");
            //remove diagonal movement
            if (input.x != 0) input.y = 0;


            if ( input != Vector2.zero)
            {
                StartCoroutine(character.Move(input, OnMoveOver));
            }
        }

        character.HandleUpdate();

        if (Input.GetKeyDown(KeyCode.Z))
        {
            Interact();
        }


    }

    void Interact()
    {

        var facingDir = new Vector3(character.Animator.MoveX, character.Animator.MoveY);
        var interactingPos = transform.position + facingDir;

        //Debug.DrawLine(transform.position, interactingPos, Color.red, 0.5f);

        var collider = Physics2D.OverlapCircle(interactingPos, 0.5f, GameLayers.i.InteractableLayer);

        if ( collider != null)
        {
            collider.GetComponent<Interactable>()?.Interact(transform);
        }
    }

    private void OnMoveOver()
    {
        var colliders = Physics2D.OverlapCircleAll(transform.position, 0.0f, GameLayers.i.TriggerableLayers);

        foreach (var collider in colliders)
        {
            var triggerable = collider.GetComponent<IPlayerTriggerable>();
            if ( triggerable != null)
            {
                triggerable.OnPlayerTrigger(this);
                break;
            }
        }
    }

    public Character Character => character;

}
