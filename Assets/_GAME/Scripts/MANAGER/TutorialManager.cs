using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private GameObject tutorialPanel1;
    [SerializeField] private GameObject tutorialPanel2;
    [SerializeField] private GameObject tutorialPanel3;
    [SerializeField] private GameObject tutorialPanel4;

    // Start is called before the first frame update
    void Start()
    {
        if (!PlayerPrefs.HasKey("Tutorial"))
        {
            OpenPanel(tutorialPanel1);
        }
        else
        {
            tutorialPanel1.SetActive(false);
            tutorialPanel2.SetActive(false);
            tutorialPanel3.SetActive(false);
            tutorialPanel4.SetActive(false);
        }
    }

    public void TutorailPanel2()
    {
        ClosePanel(tutorialPanel1);
        OpenPanel(tutorialPanel2);
    }

    public void TutorailPanel3()
    {
        ClosePanel(tutorialPanel2);
        OpenPanel(tutorialPanel3);
    }

    public void TutorailPanel4()
    {
        ClosePanel(tutorialPanel3);
        OpenPanel(tutorialPanel4);
    }

    public void TutorailPanelOff()
    {
        ClosePanel(tutorialPanel4);
        PlayerPrefs.SetInt("Tutorial", 1);
    }

    private void OpenPanel(GameObject panel)
    {
        panel.SetActive(true);
        panel.transform.localScale = Vector3.zero;  // Ýlk baþta küçültülmüþ halde
        panel.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);  // Yumuþak bir þekilde büyüme efekti
    }

    private void ClosePanel(GameObject panel)
    {
        panel.transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InBack).OnComplete(() => panel.SetActive(false));
    }
}
