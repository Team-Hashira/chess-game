using System.Resources;
using System.Runtime.Serialization;
using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Managers : MonoBehaviour
{
    public static bool Initialized { get; set; } = false;

    private static Managers s_instance;
    private static Managers Instance { get { Init(); return s_instance; } }


    //여기에 Manager를 등록해주세요.
    #region Content

    #endregion

    #region Core
    private UIManager _ui = new UIManager();
    private CoinManager _coin = new CoinManager();


    public static UIManager UI { get { return Instance?._ui; } }
    public static CoinManager Coin { get { return Instance?._coin; } }
    #endregion

    public static void Init()
    {
        if (s_instance == null && Initialized == false)
        {
            Initialized = true;

            GameObject gameObject = GameObject.Find("@Managers");
            if (gameObject == null)
            {
                gameObject = new GameObject { name = "@Managers" };
                gameObject.AddComponent<Managers>();
            }
            DontDestroyOnLoad(gameObject);

            s_instance = gameObject.GetComponent<Managers>();
        }
    }
}
