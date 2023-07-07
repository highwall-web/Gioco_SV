using System;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class Introduction : MonoBehaviour
{
    public GameObject fader2;
    [SerializeField] Fader fader;
    public float faderDuration;

    public GameObject thunderFader2;
    [SerializeField] Fader thunderFader;

    [SerializeField] GameObject dialogBox;
    public Sprite dialogSpriteOld;
    public Sprite dialogSpriteNew;
    [SerializeField] Text dialogText;
    [SerializeField] int lettersPerSecond;

    public event Action OnShowDialog;
    public event Action OnDialogFinished;

    public GameObject Mor;
    public GameObject Istera;
    private CharacterAnimator characterAnimMor;
    private CharacterAnimator characterAnimIstera;

    public Dialog dialogMor;
    public Dialog dialogIstera;
    public Dialog dialogBoth;
    private int actualCharacter;

    private bool firstTimeFade;
    private bool firstTimeDialogMor;
    private bool firstTimeDialogIstera;
    private int newScene;
    private int ended;
    private int fade;
    private int thunderFirst;
    private int thunderSecond;
    // Start is called before the first frame update
    void Start()
    {
        ended = 0;
        fade = 1;
        firstTimeFade = true;
        firstTimeDialogMor = true;
        firstTimeDialogIstera = true;
        newScene = 0;
        thunderFirst = 0;
        thunderSecond = 0;

        characterAnimMor = Mor.GetComponent<CharacterAnimator>();
        characterAnimIstera = Istera.GetComponent<CharacterAnimator>();
        characterAnimMor.currentAnim = characterAnimMor.walkUpAnim;
        characterAnimIstera.currentAnim = characterAnimIstera.walkUpAnim;                
    }

    private IEnumerator Fade(int fadeType) //Fade Out = 0, Fade In = 1
    {
        if (fadeType == 0)
        {
            yield return fader.FadeOut(faderDuration);
            fade = 0;
        }
        else
        {
            yield return fader.FadeIn(faderDuration);
            fade = 1;
        }
    }

    // First 2 thunders
    private IEnumerator ThunderFadeFirst()
    {
        AudioManager.i.PlaySfx(AudioID.FirstThunder, pauseMusic: false);
        yield return thunderFader.FadeIn(0.3f);
        yield return thunderFader.FadeOut(0.5f);

        yield return thunderFader.FadeIn(0.3f);
        yield return thunderFader.FadeOut(0.5f);
        thunderFirst = 1;
    }

    // Last thunder, the one that hits them
    private IEnumerator ThunderFadeSecond()
    {
        AudioManager.i.PlaySfx(AudioID.ThunderHit, pauseMusic: false);
        yield return thunderFader.FadeIn(0.3f);
        StartCoroutine(ShowDialog(dialogBoth));
        thunderSecond = 1;
    }

    // Update is called once per frame
    void Update()
    {
        // The fade in when the scene loads
        if (firstTimeFade)
        {
            firstTimeFade = false;
            StartCoroutine(Fade(0));
        }

        // Mor's talking
        if (fade == 0 && firstTimeDialogMor)
        {
            firstTimeDialogMor = false;
            actualCharacter = 0;
            StartCoroutine(ShowDialog(dialogMor));
        }

        // The first 2 thunders
        if (ended == 1 && firstTimeDialogIstera && thunderFirst == 0)
        {
            thunderFirst = 2;
            thunderFader2.SetActive(true);
            StartCoroutine(ThunderFadeFirst());
        }
        
        // Istera's talking
        if (ended == 1 && firstTimeDialogIstera && thunderFirst == 1)
        {
            firstTimeDialogIstera = false;
            actualCharacter = 1;
            StartCoroutine(ShowDialog(dialogIstera));
        }

        // The thunder that hits them and their scream
        if (ended == 2 && newScene == 0 && thunderSecond == 0)
        {
            thunderSecond = 2;
            StartCoroutine(ThunderFadeSecond());
        }

        // Fade out to change scene
        if (ended == 3 && newScene == 0 && thunderSecond == 1)
        {
            newScene = 1;
            StartCoroutine(Fade(1));
            StartCoroutine("SwitchScene");
        }
    }

    public bool IsShowing { get; private set; }
    public bool Ended { get; private set; }

    public IEnumerator ShowDialog(Dialog dialog)
    {
        yield return new WaitForEndOfFrame();

        if (actualCharacter == 0)
        {
            dialogBox.GetComponent<Image>().sprite = dialogSpriteOld;
        }
        else
        {
            dialogBox.GetComponent<Image>().sprite = dialogSpriteNew;
        }

        OnShowDialog?.Invoke();

        IsShowing = true;
        Ended = false;
        dialogBox.SetActive(true);

        foreach (var line in dialog.Lines)
        {
            AudioManager.i.PlaySfx(AudioID.UISelect);
            yield return TypeDialog(line);
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Z));
        }

        dialogBox.SetActive(false);
        IsShowing = false;
        Ended = true;
        ended++;
        OnDialogFinished?.Invoke();
    }

    public IEnumerator TypeDialog(string dialog)
    {
        dialogText.text = "";
        foreach (var letter in dialog.ToCharArray())
        {
            dialogText.text += letter;
            yield return new WaitForSeconds(1f / lettersPerSecond);
        }
    }

    IEnumerator SwitchScene()
    {
        fader2.SetActive(true);
        yield return fader.FadeIn(faderDuration);
        yield return SceneManager.LoadSceneAsync("Gameplay");
    }
}
