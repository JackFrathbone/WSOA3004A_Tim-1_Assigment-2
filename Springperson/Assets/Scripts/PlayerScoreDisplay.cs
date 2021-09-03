using TMPro;
using UnityEngine;

public class PlayerScoreDisplay : MonoBehaviour
{
    [SerializeField] GameObject textParent;
    [SerializeField] GameObject textPrefab;

    public void ShowScore(int i)
    {
        TextMeshProUGUI _scoreText = Instantiate(textPrefab, textParent.transform.position, Quaternion.identity, textParent.transform).GetComponent<TextMeshProUGUI>();
        _scoreText.text = "+" + i.ToString();
        Destroy(_scoreText.gameObject, 5f);
    }
}
