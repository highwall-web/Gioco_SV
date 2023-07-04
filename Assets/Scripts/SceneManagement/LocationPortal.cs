using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LocationPortal : MonoBehaviour, IPlayerTriggerable
{
    [SerializeField] Transform spawnPoint;
    [SerializeField] DestinationIdentifier destinationPortal;

    PlayerController player;

    public void OnPlayerTrigger(PlayerController player)
    {
        this.player = player;
        StartCoroutine(Teleport());
    }

    Fader fader;
    private void Start()
    {
        fader = FindObjectOfType<Fader>();
    }

    IEnumerator Teleport()
    {
        player.IsTeleporting = true;
        player.Character.Animator.IsMoving = false;
        GameController.Instance.PauseGame(true);
        yield return fader.FadeIn(0.5f);

        var destPortal = FindObjectsOfType<LocationPortal>().First(x => x != this && x.destinationPortal == this.destinationPortal);
        player.Character.SetPositionAndSnapToTile(destPortal.SpawnPoint.position);

        player.Character.Animator.IsMoving = false;
        yield return fader.FadeOut(0.5f);
        GameController.Instance.PauseGame(false);
        player.IsTeleporting = false;
    }

    public Transform SpawnPoint => spawnPoint;

    public bool TriggerRepeatedly => false;
}
