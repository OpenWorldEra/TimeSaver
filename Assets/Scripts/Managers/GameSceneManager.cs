using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{
    [SerializeField] Transform leftBoundary;
    [SerializeField] Transform rightBoundary;
    [SerializeField] GameObject pauseBtn;
    [SerializeField] GameObject resumeBtn;
    [SerializeField] GameObject pausePanel;
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] AudioSource clickAudio;
    [SerializeField] GameObject backgroundAudio;
    

    private void Awake() {
        GameStateManager.Instance.SetState(GameState.GamePlay);
    }

    // Start is called before the first frame update
    void Start()
    {
        SetBoundaries();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SetBoundaries()
    {
        Vector3 left = new Vector3(0, Screen.height / 2, 0);
        Vector3 right = new Vector3(Screen.width, Screen.height / 2, 0);
        left = Camera.main.ScreenToWorldPoint(left);
        right = Camera.main.ScreenToWorldPoint(right);
        left.z = right.z = 0;

        leftBoundary.transform.position = new Vector3(left.x, 0, 0);
        rightBoundary.transform.position = new Vector3(right.x, 0, 0);
    }

    public void PauseGame()
    {
        if (GameStateManager.Instance.CurrentGameState == GameState.Paused)
        {
            return;
        }

        clickAudio.Play();
        pauseBtn.SetActive(false);
        resumeBtn.SetActive(true);
        pausePanel.SetActive(true);
        GameStateManager.Instance.SetState(GameState.Paused);
    }

    public void ResumeGame()
    {
        if (GameStateManager.Instance.CurrentGameState == GameState.GamePlay)
        {
            return;
        }

        clickAudio.Play();
        resumeBtn.SetActive(false);
        pauseBtn.SetActive(true);
        pausePanel.SetActive(false);
        GameStateManager.Instance.SetState(GameState.GamePlay);
    }

    public void ActivateGameOver()
    {
        backgroundAudio.SetActive(false);
        gameOverPanel.SetActive(true);
    }

    public void RestartBtn()
    {
        clickAudio.Play();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void BackBtn()
    {
        clickAudio.Play();
        SceneManager.LoadScene(0);
    }
}
