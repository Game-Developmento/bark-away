using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMap : MonoBehaviour
{
    public Transform player;
    [SerializeField] private GameObject squareMiniMap;
    [SerializeField] private GameObject circleMiniMap;
        private Vector3 initialPosition;
    private Quaternion initialRotation;


    private void Start()
    {
        initialPosition = transform.position;
        initialRotation = transform.rotation;
    }

    private void LateUpdate()
    {
        if (circleMiniMap.activeSelf)
        {
            Vector3 newPosition = player.position;
            newPosition.y = transform.position.y;
            transform.position = newPosition;
            transform.rotation = Quaternion.Euler(90f, Camera.main.transform.eulerAngles.y, 0f);
        }
    }
    public void ToggleDifferentMiniMap()
    {
        squareMiniMap.SetActive(!squareMiniMap.activeSelf);
        circleMiniMap.SetActive(!circleMiniMap.activeSelf);
        if (squareMiniMap.activeSelf)
        {
            transform.rotation = initialRotation;
            transform.position = initialPosition;
        }
    }
}
