using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskManagerUI : MonoBehaviour
{
    [SerializeField] private Transform container;
    [SerializeField] private Transform taskTemplate;


    private void Awake()
    {
        taskTemplate.gameObject.SetActive(false);
    }
    // Start is called before the first frame update
    void Start()
    {

    }
    private void UpdateVisual()
    {
        foreach (Transform child in container)
        {
            if (child == taskTemplate) continue;
            Destroy(child.gameObject);
        }
    }
    // Update is called once per frame
    void Update()
    {

    }
}
