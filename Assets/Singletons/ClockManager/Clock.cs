using UnityEngine;

public class Clock : MonoBehaviour
{

    private static Clock instance;

    public static Clock Instance => instance;

    private int tickSpeed = 1;
    [SerializeField] private float timer;
    

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        timer += Time.deltaTime;

        while (timer >= tickSpeed)
        {
            timer -= tickSpeed;
            OnTick();
        }
    }


    private void OnTick()
    {
        return;
    }

}
