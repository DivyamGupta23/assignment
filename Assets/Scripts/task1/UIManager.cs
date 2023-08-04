using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class UIManager : MonoBehaviour
{
    // [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private float animationDuration;

    public void FadeIn()
    {
        // canvasGroup.alpha = 0f;
        rectTransform.localScale = Vector2.zero;
        rectTransform.localPosition = new Vector3(-1300f, 0f, 0f);
        rectTransform.DOAnchorPos(new Vector2(0f, 0f), animationDuration, false).SetEase(Ease.OutFlash);
        rectTransform.DOScale(0.6889259f, animationDuration).SetEase(Ease.OutSine);
        // canvasGroup.DOFade(1, fadeDuration);
    }


    public void FadeOut()
    {
        // canvasGroup.alpha = 1f;
        rectTransform.localScale = new Vector3(0.6889259f, 0.6889259f, 0.6889259f);
        rectTransform.localPosition = new Vector3(0f, 0f, 0f);
        // canvasGroup.DOFade(0, fadeDuration);
        rectTransform.DOAnchorPos(new Vector2(1300f, 0f), animationDuration, false).SetEase(Ease.OutFlash);
        rectTransform.DOScale(0f, animationDuration).SetEase(Ease.OutSine);

    }
}
