using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour
{
    /** Set in editor to different screens of the main menu */
    [SerializeField] private GameObject[] menuScreens;
    [SerializeField] private Button initialButton;

    private void Awake()
    {
        initialButton.Select();
    }

    public void OnStartEndlessClick()
    {
        Debug.Log("Start Endless Button clicked");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void OnStartLevelClick()
    {
        Debug.Log("Start Level Button clicked");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
    }

    public void OnExitClick()
    {
        Debug.Log("Exit Button clicked");
        #if UNITY_EDITOR
        // Application.Quit() does not work in the editor so
        // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    public void LoadScreen(int screenIndex)
    {
        menuScreens[screenIndex].SetActive(true);
        for (int i = 0; i < menuScreens.Length; i++)
        {
            if (menuScreens[i].activeInHierarchy == true && i != screenIndex)
            {
                menuScreens[i].SetActive(false);
            }
        }
    }

    public void SelectButton(GameObject uiElement)
    {
        if (uiElement.GetComponent<Button>())
        {
            uiElement.GetComponent<Button>().Select();
        } else if (uiElement.GetComponent<Slider>())
        {
            uiElement.GetComponent<Slider>().Select();
        }
    }

}
