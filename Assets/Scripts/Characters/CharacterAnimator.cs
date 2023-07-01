using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimator : MonoBehaviour, ISavable
{
    public bool isNPC = true;
    private int actualCharacter;

    [Header("Mor Sprites")]
    [SerializeField] List<Sprite> walkDownSprites_Mor;
    [SerializeField] List<Sprite> walkUpSprites_Mor;
    [SerializeField] List<Sprite> walkRightSprites_Mor;
    [SerializeField] List<Sprite> walkLeftSprites_Mor;

    [Header("Istera Sprites")]
    [SerializeField] List<Sprite> walkDownSprites_Istera;
    [SerializeField] List<Sprite> walkUpSprites_Istera;
    [SerializeField] List<Sprite> walkRightSprites_Istera;
    [SerializeField] List<Sprite> walkLeftSprites_Istera;

    [Header("NPC Sprites")]
    [SerializeField] List<Sprite> walkDownSprites_NPC;
    [SerializeField] List<Sprite> walkUpSprites_NPC;
    [SerializeField] List<Sprite> walkRightSprites_NPC;
    [SerializeField] List<Sprite> walkLeftSprites_NPC;

    [HideInInspector]
    [SerializeField] List<Sprite> walkDownSprites_actual;
    [HideInInspector]
    [SerializeField] List<Sprite> walkUpSprites_actual;
    [HideInInspector]
    [SerializeField] List<Sprite> walkRightSprites_actual;
    [HideInInspector]
    [SerializeField] List<Sprite> walkLeftSprites_actual;

    // Parameters
    public float MoveX { get; set; }
    public float MoveY { get; set; }
    public bool IsMoving { get; set; }

    // States
    SpriteAnimator walkDownAnim;
    SpriteAnimator walkUpAnim;
    SpriteAnimator walkRightAnim;
    SpriteAnimator walkLeftAnim;

    SpriteAnimator currentAnim;
    bool wasPreviouslyMoving;

    // References
    SpriteRenderer spriteRenderer;
    SwapCharacter swapCharacter;
    private void Start()
    {
        int actual;
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (isNPC)
        {
            actual = 2;
        }
        else
        {
            swapCharacter = GetComponent<SwapCharacter>();
            actual = swapCharacter.getActualCharacter();
        }
        setSprites(actual);

        currentAnim = walkDownAnim;
    }

    private void Update()
    {
        var prevAnim = currentAnim;

        if (MoveX == 1)
            currentAnim = walkRightAnim;
        else if (MoveX == -1)
            currentAnim = walkLeftAnim;
        else if (MoveY == 1)
            currentAnim = walkUpAnim;
        else if (MoveY == -1)
            currentAnim = walkDownAnim;


        if (currentAnim != prevAnim || IsMoving != wasPreviouslyMoving)
            currentAnim.Start();


        if (IsMoving)
            currentAnim.HandleUpdate();
        else
            spriteRenderer.sprite = currentAnim.Frames[0];

        wasPreviouslyMoving = IsMoving;
    }

    public void setSprites(int actual)
    {
        if (actual == 2) // NPC (2)
        {
            walkDownSprites_actual = walkDownSprites_NPC;
            walkUpSprites_actual = walkUpSprites_NPC;
            walkRightSprites_actual = walkRightSprites_NPC;
            walkLeftSprites_actual = walkLeftSprites_NPC;
        }
        else if (actual == 0) // Mor (0)
        {
            walkDownSprites_actual = walkDownSprites_Mor;
            walkUpSprites_actual = walkUpSprites_Mor;
            walkRightSprites_actual = walkRightSprites_Mor;
            walkLeftSprites_actual = walkLeftSprites_Mor;
        }
        else // Istera (1)
        {
            walkDownSprites_actual = walkDownSprites_Istera;
            walkUpSprites_actual = walkUpSprites_Istera;
            walkRightSprites_actual = walkRightSprites_Istera;
            walkLeftSprites_actual = walkLeftSprites_Istera;
        }

        renderSprites();
    }

    private void renderSprites()
    {
        walkDownAnim = new SpriteAnimator(walkDownSprites_actual, spriteRenderer);
        walkUpAnim = new SpriteAnimator(walkUpSprites_actual, spriteRenderer);
        walkRightAnim = new SpriteAnimator(walkRightSprites_actual, spriteRenderer);
        walkLeftAnim = new SpriteAnimator(walkLeftSprites_actual, spriteRenderer);
    }

    public object CaptureState()
    {
        int actual;
        if (isNPC)
        {
            actual = 2;
        }
        else
        {
            actual = swapCharacter.getActualCharacter();
        }
        return actual;
    }

    public int getActualCharacter()
    {
        return actualCharacter;
    }

    public void RestoreState(object state)
    {
        int actual = (int)state;
        setSprites(actual);

        if (!isNPC)
        {
            currentAnim = walkDownAnim;
            if (actual == 0)
                print("IN ANIM - Mor");
            else
                print("IN ANIM - Istera");
            actualCharacter = actual;
        }
    }
}