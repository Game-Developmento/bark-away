using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMap : MonoBehaviour
{
    private void LateUpdate()
    {
        transform.rotation = Quaternion.Euler(90f, Camera.main.transform.eulerAngles.y, 0f);
    }
}
