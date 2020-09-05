using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using DG.Tweening;

public class FailedCanvasManager : BaseCanvasManager
{
    [SerializeField] Button restartButton;

    public override void OnStart()
    {
        base.SetScreenAction(thisScreen: ScreenState.Failed);
        restartButton.onClick.AddListener(OnClickRestartButton);
        gameObject.SetActive(false);
    }

    public override void OnInitialize()
    {
    }

    public override void OnUpdate()
    {
        if (!base.IsThisScreen) { return; }

    }

    protected override void OnOpen()
    {
        DOVirtual.DelayedCall(0f, () =>
        {
            gameObject.SetActive(true);
            transform.localScale = Vector3.zero;
            transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);

            Sequence retryButtonSequence = DOTween.Sequence()
            .Append(restartButton.transform.DOScale(Vector3.one * 1.1f, 0.5f))
            .Append(restartButton.transform.DOScale(Vector3.one, 0.5f));
            retryButtonSequence.SetLoops(-1);
        });
    }

    protected override void OnClose()
    {
        gameObject.SetActive(false);
    }

    void OnClickRestartButton()
    {
        base.ReLoadScene();
        SoundManager.i.PlayOneShot(0);
    }

    void OnClickHomeButton()
    {
        Variables.screenState = ScreenState.Home;
        SoundManager.i.PlayOneShot(0);
    }
}
