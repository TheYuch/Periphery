using UnityEngine;
using Image = UnityEngine.UI.Image;

public class PauseMenu : MonoBehaviour
{

    public static bool isPaused = false;

    public GameObject pauseMenuUI;
    private Image panel; //panel of pauseMenuUI;
    private GameObject pauseButton;
    private GameObject resumeButton;

    private void Awake()
    {
        pauseButton = pauseMenuUI.transform.Find("Pause Button").gameObject;
        resumeButton = pauseMenuUI.transform.Find("Resume Button").gameObject;
        panel = pauseMenuUI.GetComponent<Image>();
    }
    private void Start()
    {
        pauseButton.SetActive(true);
        resumeButton.SetActive(false);
        panel.enabled = false;
    }

    public void Resume()
    {
        panel.enabled = false;
        resumeButton.SetActive(false);
        pauseButton.SetActive(true);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void Pause()
    {
        panel.enabled = true;
        resumeButton.gameObject.SetActive(true);
        pauseButton.SetActive(false);

        Time.timeScale = 0f;
        isPaused = true;
    }
}
