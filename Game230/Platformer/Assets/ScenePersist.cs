using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenePersist : MonoBehaviour
{

    
    // Start is called before the first frame update
    int startingSceneIndex;

    private void Awake()
    {
       int numScenePersists = FindObjectsOfType<ScenePersist>().Length;

       if(numScenePersists > 1)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
       else
        {
            DontDestroyOnLoad(gameObject);
        }
    }


    void Start()
    {
        startingSceneIndex = SceneManager.GetActiveScene().buildIndex;
    }

    // Update is called once per frame
    private void Update()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        if (currentSceneIndex != startingSceneIndex)
        {
            Destroy(gameObject);
        }
    }
}
