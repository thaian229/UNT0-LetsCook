using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayingClockUI : MonoBehaviour
{
    [SerializeField] public Image timerImage;

    private void Update()
    {
        if (timerImage == null) return;

        timerImage.fillAmount = GameManager.Instance.GetGamePlayingTimerNormalized();
    }
}
