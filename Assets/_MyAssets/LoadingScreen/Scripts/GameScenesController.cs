
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class GameScenesController
{
    public static void LoadScene(GameScene scene)
    {
        SceneManager.LoadScene(scene.ToString());
    }


    public static async void LoadSceneAsync(GameScene scene)
    {
        LoadingScreen.Show();
        AsyncOperation loadAsync = SceneManager.LoadSceneAsync(scene.ToString());

        while (loadAsync.isDone != true)
        {
            LoadingScreen.SetProgressBarValue(loadAsync.progress);
            await Task.Yield();
        }

        LoadingScreen.Hide();
    }


    public static async void LoadSceneAsync(int sceneIndex)
    {
        LoadingScreen.Show();
        AsyncOperation loadAsync = SceneManager.LoadSceneAsync(sceneIndex);

        while (loadAsync.isDone != true)
        {
            LoadingScreen.SetProgressBarValue(loadAsync.progress);
            await Task.Yield();
        }

        LoadingScreen.Hide();
    }
}


public enum GameScene
{
    Pre,
    Main,
    Tutorial
}
