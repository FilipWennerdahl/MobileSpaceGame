using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    public void LoadUnlimitedLevel() {
        SceneManager.LoadScene("UnlimitedLevel");
    }

    public void QuitGame() {
        Application.Quit();
    }

}
