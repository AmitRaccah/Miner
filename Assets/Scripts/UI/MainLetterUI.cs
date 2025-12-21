using UnityEngine;
using UnityEngine.UI;

public class MainLetterUI : MonoBehaviour
{
    [SerializeField] private Image mainLetterImage;
    [SerializeField] private LettersDataBase lettersDataBase;

    private void OnEnable()
    {
        GameManager.OnLevelStarted += HandleLevelStarted;

        if (GameManager.instance != null && GameManager.instance.level != null)
        {
            HandleLevelStarted(GameManager.instance.level);
        }
    }

    private void OnDisable()
    {
        GameManager.OnLevelStarted -= HandleLevelStarted;
    }

    private void HandleLevelStarted(LevelDataSO level)
    {
        if (level == null || mainLetterImage == null || lettersDataBase == null)
        {
            return;
        }

        Sprite sprite = lettersDataBase.GetSpriteByName(level.mainLetter);
        mainLetterImage.sprite = sprite;
        mainLetterImage.enabled = (sprite != null);
    }
}
