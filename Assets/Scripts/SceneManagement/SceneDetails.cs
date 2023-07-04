using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneDetails : MonoBehaviour
{
    [SerializeField] List<SceneDetails> connectedScenes;
    [SerializeField] AudioClip sceneMusic_Old;
    [SerializeField] AudioClip sceneMusic_New;

    public bool isLoaded { get; private set; }

    public GameObject player;
    SwapCharacter swapCharacter;
    int character;
    private void Start(){
        //per vedere che personaggio è selezionato e di conseguenza scegliere la musica
        swapCharacter = player.GetComponent<SwapCharacter>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(swapCharacter != null){
            if (collision.tag == "Player")
            {
                if(swapCharacter.getActualCharacter() == 0){
                    LoadScene();
                    GameController.Instance.SetCurrentScene(this);

                    if(sceneMusic_Old != null)
                        AudioManager.i.PlayMusic(sceneMusic_Old, fade: true);

                    foreach (var scene in connectedScenes)
                    {
                        scene.LoadScene();
                    }

                    if(GameController.Instance.PrevScene != null)
                    {
                        var previouslyLoadedScenes = GameController.Instance.PrevScene.connectedScenes;
                        foreach (var scene in previouslyLoadedScenes)
                        {
                            if (!connectedScenes.Contains(scene) && scene != this)
                            {
                                scene.UnloadScene();
                            }
                        }
                    }
                }else{
                    LoadScene();
                    GameController.Instance.SetCurrentScene(this);

                    if(sceneMusic_Old != null)
                        AudioManager.i.PlayMusic(sceneMusic_New, fade: true);

                    foreach (var scene in connectedScenes)
                    {
                        scene.LoadScene();
                    }

                    if(GameController.Instance.PrevScene != null)
                    {
                        var previouslyLoadedScenes = GameController.Instance.PrevScene.connectedScenes;
                        foreach (var scene in previouslyLoadedScenes)
                        {
                            if (!connectedScenes.Contains(scene) && scene != this)
                            {
                                scene.UnloadScene();
                            }
                        }
                    }
                }
                
            }
        }else{
            Debug.Log("NULL");
        }
        
    }

    public void LoadScene()
    {
        if (!isLoaded)
        {
            SceneManager.LoadSceneAsync(gameObject.name, LoadSceneMode.Additive);
            isLoaded = true;
        }
    }

    public void UnloadScene()
    {
        if (isLoaded)
        {
            SceneManager.UnloadSceneAsync(gameObject.name);
            isLoaded = false;
        }
    }

    public AudioClip SceneMusic(){
        if(swapCharacter.getActualCharacter() == 0){
            return sceneMusic_Old;
        }else{
            return sceneMusic_New;
        }
        
    }
}
