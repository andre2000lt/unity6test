using UnityEditor;
using UnityEditor.SceneManagement;

public class ScenesMenu
{

	[MenuItem("Scenes/Pre Scene")]
	static void OpenPreScene()
	{
		OpenScene("Assets/_Game/Scenes/Pre.unity");
	}


	[MenuItem("Scenes/Tutorial")]
	static void OpenTutorial()
	{
		OpenScene("Assets/_Game/Scenes/Tutorial.unity");
	}


	[MenuItem("Scenes/Main")]
	static void OpenMain()
	{
		OpenScene("Assets/_Game/Scenes/Main.unity");
	}


	static void OpenScene(string scenePath)
	{
		if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
		{
			EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);
		}
	}
}


