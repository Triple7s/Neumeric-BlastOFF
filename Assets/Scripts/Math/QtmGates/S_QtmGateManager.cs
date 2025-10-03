using UnityEngine;
using UnityEngine.Rendering.Universal;
using Random = UnityEngine.Random;

public class S_QtmGateManager : MonoBehaviour
{
    public static S_QtmGateManager Instance;
    
    [SerializeField] private SO_Equations equations;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);

        

    }

    public Question GetQuestion()
    {
        int randomIndex = Random.Range(0, equations.questions.Count);
        return equations.questions[randomIndex];
    }
    
}
