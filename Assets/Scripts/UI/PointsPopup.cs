using System.Collections;
using UnityEngine;
using TMPro;

public class PointsPopup : MonoBehaviour
{
    [SerializeField] private float duration = 3f;
    [SerializeField] private float speed = 10f;
    private TextMeshProUGUI textMesh;

    private void Awake()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
    }

    public void ShowScorePopup(int score)
    {
        gameObject.SetActive(true);
        StartCoroutine(AnimateScorePopup(score));
    }

    private IEnumerator AnimateScorePopup(int score)
    {
        textMesh.text = "+ " + score.ToString();
        float elapsedTime = 0f;
        float initialYPos = transform.position.y;

        while (elapsedTime < duration)
        {
            float newYPos = initialYPos + (speed * elapsedTime);
            transform.position = new Vector3(transform.position.x, newYPos, transform.position.z);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        textMesh.text = string.Empty;
        transform.position = new Vector3(transform.position.x, initialYPos, transform.position.z);
        gameObject.SetActive(false);
    }

}
