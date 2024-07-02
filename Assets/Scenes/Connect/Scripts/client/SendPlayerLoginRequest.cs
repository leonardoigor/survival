using Assets.Scenes.Connect.Scripts;
using System.Collections;
using Unity.Entities;
using UnityEditor.MemoryProfiler;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SendPlayerLoginRequest : MonoBehaviour
{
    private AsyncOperation SceneLoadRef;
    public bool isLogger = false;
    public int SceneToLoadID = default;

    public static SendPlayerLoginRequest Singleton;
    public Slider LoadSlider;
    [SerializeField] private GameObject LoadGa;
    [SerializeField] private GameObject ConnectionGa;
    public Coroutine coroutineLoad { get; private set; }

    private void Awake()
    {
        Singleton = this;
    }
    // Start is called before the first frame update
    void Start()
    {

        ChangeStateGa(false);

    }

    // Update is called once per frame
    void Update()
    {
        if (!isLogger) return;
        var world = World.DefaultGameObjectInjectionWorld;
        if (world == null) return;
        var request = world.EntityManager.CreateEntity();
        world.EntityManager.AddComponentData(request, new ClientRequest
        {

        });
        SceneLoadRef = SceneManager.LoadSceneAsync(SceneToLoadID);
        SceneLoadRef.allowSceneActivation = true;

        isLogger = false;
        ChangeStateGa(true);
        coroutineLoad = StartCoroutine(LoadScene());
    }

    private void ChangeStateGa(bool val)
    {
        LoadGa.SetActive(val);
        ConnectionGa.SetActive(!LoadGa.active);
    }

    bool test = false;
    IEnumerator LoadScene()
    {
        while (true)
        {
            yield return new WaitForFixedUpdate();
            var progress = SceneLoadRef.progress;

            Debug.Log(progress);
            LoadSlider.value = progress;
        }

    }
}
