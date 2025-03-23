using Io.AppMetrica;
using UnityEngine;

public class AppMetricaInit: MonoBehaviour
{

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Activate()
    {
        AppMetrica.Activate(new AppMetricaConfig("4292f6f4-f75b-4a5f-9f47-1413353ed9ee")
        {
            FirstActivationAsUpdate = !IsFirstLaunch(),
        });
    }

    private static bool IsFirstLaunch()
    {
        // Implement logic to detect whether the app is opening for the first time.
        // For example, you can check for files (settings, databases, and so on),
        // which the app creates on its first launch.
        return true;
    }
}

