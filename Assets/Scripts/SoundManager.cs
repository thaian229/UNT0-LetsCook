using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private const string PLAYER_PREFS_SOUND_EFFECT_VOLUME = "SoundEffectsVolume";

    public static SoundManager Instance { get; private set; }

    [SerializeField] private AudioClipRefSO audioClipRefSO;

    private float volume = .5f;

    private void Awake()
    {
        Instance = this;

        volume = PlayerPrefs.GetFloat(PLAYER_PREFS_SOUND_EFFECT_VOLUME, .5f);
    }

    private void Start()
    {
        TrashCounter.OnAnyObjectTrashed += TrashCounter_OnAnyObjectTrashed;
        BaseCounter.OnAnyObjectPlaced += BaseCounter_OnAnyObjectPlaced;
        Player.Instance.OnPickedSomething += Player_OnPickedSomething;
        CuttingCounter.OnAnyCut += CuttingCounter_OnAnyCut;
        DeliveryManager.Instance.OnRecipeFailed += DeliveryManager_OnRecipeFailed;
        DeliveryManager.Instance.OnRecipeSuccess += DeliveryManager_OnRecipeSuccess;
    }

    private void TrashCounter_OnAnyObjectTrashed(object sender, System.EventArgs e)
    {
        TrashCounter trashCounter = sender as TrashCounter;
        PlaySound(audioClipRefSO.trash, trashCounter.transform.position);
    }

    private void BaseCounter_OnAnyObjectPlaced(object sender, System.EventArgs e)
    {
        BaseCounter baseCounter = sender as BaseCounter;
        PlaySound(audioClipRefSO.objectDrop, baseCounter.transform.position);
    }

    private void Player_OnPickedSomething(object sender, System.EventArgs e)
    {
        Player player = sender as Player;
        PlaySound(audioClipRefSO.objectPickup, player.transform.position);
    }

    private void CuttingCounter_OnAnyCut(object sender, System.EventArgs e)
    {
        CuttingCounter cuttingCounter = sender as CuttingCounter;
        PlaySound(audioClipRefSO.chop, cuttingCounter.transform.position);
    }

    private void DeliveryManager_OnRecipeFailed(object sender, System.EventArgs e)
    {
        PlaySound(audioClipRefSO.deliveryFail, DeliveryManager.Instance.transform.position);
    }

    private void DeliveryManager_OnRecipeSuccess(object sender, System.EventArgs e)
    {
        PlaySound(audioClipRefSO.deliverySuccess, DeliveryManager.Instance.transform.position);
    }

    private void PlaySound(AudioClip audioClip, Vector3 position, float volumeMultiplier = 1f)
    {
        AudioSource.PlayClipAtPoint(audioClip, position, volume * volumeMultiplier);
    }

    private void PlaySound(AudioClip[] audioClipArray, Vector3 position, float volume = 1f)
    {
        PlaySound(audioClipArray[Random.Range(0, audioClipArray.Length)], position, volume);
    }

    public void PlayFootstepSound(Vector3 position, float volume = 1f)
    {
        PlaySound(audioClipRefSO.footstep, position, volume);
    }

    public void ChangeVolumeCycled()
    {
        volume += .1f;
        if (volume > 1f) volume = 0f;

        PlayerPrefs.SetFloat(PLAYER_PREFS_SOUND_EFFECT_VOLUME, volume);
        PlayerPrefs.Save();
    }

    public float GetVolume()
    {
        return volume;
    }
}
