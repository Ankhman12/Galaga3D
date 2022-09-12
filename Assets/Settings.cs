using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    public void SetSensitivity(System.Single sensitivity)
    {
        PlayerPrefs.SetFloat("Sensitivity", sensitivity);
    }

    private void Awake()
    {
        GameObject.Find("Sensitivity").GetComponent<Slider>().value = PlayerPrefs.GetFloat("Sensitivity", 1.5f);
        GameObject.Find("Volume").GetComponent<Slider>().value = PlayerPrefs.GetFloat("Volume", 1f);
    }

    public void SetVolume(System.Single volume)
    {
        PlayerPrefs.SetFloat("Volume", volume);
        AudioListener.volume = volume;
    }
}
