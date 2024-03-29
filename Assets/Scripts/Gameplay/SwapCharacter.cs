using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwapCharacter : MonoBehaviour, ISavable
{
    private Vector3 savePosition, loadPosition;
    public GameController gameController;

    public Image icon;
    public Sprite nuovaImmagine;
    public Sprite vecchiaImmagine;
    private DialogManager dialogManager;
    private MenuController menuController;
    private PlayerController playerController;
    private Character character;
    private CharacterAnimator characterAnimator;
    private int actualCharacter; // 0 = Mor, 1 = Istera

    [SerializeField] Text dialogText;
    [SerializeField] Font fontAssetNew;
    [SerializeField] Font fontAssetOld;

    // Start is called before the first frame update
    void Start()
    {
        actualCharacter = 0;
        dialogManager = gameController.GetComponent<DialogManager>();
        menuController = gameController.GetComponent<MenuController>();
        playerController = GetComponent<PlayerController>();
        character = playerController.GetComponent<Character>();
        characterAnimator = GetComponent<CharacterAnimator>();
        // Initializes the position of the other era
        loadPosition = new Vector3(68.50f, 57.50f);
    }

    // Update is called once per frame
    void Update()
    {
        if (!character.IsMoving && !dialogManager.IsShowing && !menuController.IsShowing && !playerController.IsTeleporting)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                // Swap sprites
                if (actualCharacter == 0) // Mor (0) -> Istera (1)
                {
                    actualCharacter = 1;
                    characterAnimator.setSprites(actualCharacter);
                    icon.sprite = nuovaImmagine;
                    dialogText.font = fontAssetNew;
                    dialogText.fontSize = 35;
                }
                else // Istera (1) -> Mor (0)
                {
                    actualCharacter = 0;
                    characterAnimator.setSprites(actualCharacter);
                    dialogText.font = fontAssetOld;
                    icon.sprite = vecchiaImmagine;
                    dialogText.fontSize = 40;
                }

                // Swap character
                savePosition = gameObject.transform.position; // Save the position of the actual era
                gameObject.transform.position = loadPosition; // Load the position of the other era
                loadPosition = savePosition; // The saved position becomes the new load position for the next swap
            }
        }
    }

    public int getActualCharacter()
    {
        return actualCharacter;
    }

    public object CaptureState()
    {
        float[] position = new float[] { loadPosition.x, loadPosition.y };
        return position;
    }

    public void RestoreState(object state)
    {
        var position = (float[])state;
        loadPosition = new Vector3(position[0], position[1]);
        actualCharacter = characterAnimator.getActualCharacter(); // Takes the actual character from CharacterAnimation beacuse it's updated there (and it saves the actualCharacter variable)
    }
}
