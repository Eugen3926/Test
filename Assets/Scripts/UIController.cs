using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    public static bool isPaused;
    public static event onClickEvent onPauseEvent;
    public static event onClickEvent onRestartEvent;
    public delegate void onClickEvent();

    [SerializeField] private Image bgImage;
    [SerializeField] private Image fadeImage;
    [SerializeField] private Button pause;
    [SerializeField] private Button play;
    [SerializeField] private Button exit;
    [SerializeField] private Button shield;

    private void Start()
    {
        PlayerController.onGameOverEvent += GameOver;
        fadeImage.DOFade(0, 0.5f).OnComplete(FadeComplete);
    }

    private void FadeComplete()
    {
        fadeImage.gameObject.SetActive(false);
    }

    private void GameOver()
    {
        PlayerController.onGameOverEvent -= GameOver;
        fadeImage.gameObject.SetActive(true);
        fadeImage.DOFade(1, 0.5f).OnComplete(NewGame);        
    }

    private void NewGame() {
        StopAllCoroutines();
        SceneManager.LoadScene("Game");
    }

    public void PauseBtn() {
        isPaused = true;
        StopAllCoroutines();
        pause.interactable = false;
        shield.interactable = false;
        bgImage.DOFade(0.8f, 0.3f);
        play.gameObject.SetActive(true);
        exit.gameObject.SetActive(true);
        onPauseEvent?.Invoke();
    }

    public void PlayBtn()
    {
        isPaused = false;
        play.gameObject.SetActive(false);
        shield.interactable = true;
        exit.gameObject.SetActive(false);
        pause.interactable = true;
        bgImage.DOFade(0f, 0.3f);
        onRestartEvent?.Invoke();
    }

    public void ExitBtn()
    {
        Application.Quit();
    }
}
