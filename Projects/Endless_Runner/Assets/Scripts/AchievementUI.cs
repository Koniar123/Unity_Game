using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AchievementToast : MonoBehaviour
{
    [Header("Toast UI")]
    public Animator toastAnimator;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI descriptionText;
    public Image iconImage;

    private Queue<AchievementDefinition> _queue = new();
    private bool _isShowing = false;

    private void OnEnable()
    {
        if (AchievementManager.Instance != null)
            AchievementManager.Instance.OnAchievementUnlocked += Enqueue;
    }

    private void OnDisable()
    {
        if (AchievementManager.Instance != null)
            AchievementManager.Instance.OnAchievementUnlocked -= Enqueue;
    }

    private void Enqueue(AchievementDefinition def)
    {
        _queue.Enqueue(def);
        if (!_isShowing) StartCoroutine(ShowNext());
    }

    private IEnumerator ShowNext()
    {
        while (_queue.Count > 0)
        {
            _isShowing = true;
            var def = _queue.Dequeue();

            titleText.text = def.title;
            descriptionText.text = def.isSecret ? "Sekretny achievement!" : def.description;
            if (def.icon != null) iconImage.sprite = def.icon;

            toastAnimator.SetTrigger("Show");
            yield return new WaitForSeconds(3f);
            toastAnimator.SetTrigger("Hide");
            yield return new WaitForSeconds(0.5f);
        }
        _isShowing = false;
    }
}

public class AchievementScreen : MonoBehaviour
{
    [Header("Lista achievementów")]
    public Transform contentParent;
    public GameObject achievementEntryPrefab;

    private void OnEnable()
    {
        Refresh();
    }

    public void Refresh()
    {
        foreach (Transform child in contentParent)
            Destroy(child.gameObject);

        var manager = AchievementManager.Instance;
        if (manager == null) return;

        foreach (var def in manager.GetAllDefinitions())
        {
            bool unlocked = manager.IsUnlocked(def.id);

            var entry = Instantiate(achievementEntryPrefab, contentParent);
            var ui = entry.GetComponent<AchievementEntryUI>();
            if (ui != null) ui.Setup(def, unlocked);
        }
    }
}

public class AchievementEntryUI : MonoBehaviour
{
    public Image iconImage;
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI descriptionText;
    public GameObject lockedOverlay;   
    public Image checkmark;            

    public void Setup(AchievementDefinition def, bool unlocked)
    {
        titleText.text = unlocked || !def.isSecret ? def.title : "???";
        descriptionText.text = unlocked ? def.description
                             : def.isSecret ? "Sekretny achievement"
                             : def.description;

        if (def.icon != null) iconImage.sprite = def.icon;

        lockedOverlay.SetActive(!unlocked);
        checkmark.gameObject.SetActive(unlocked);

        iconImage.color = unlocked ? Color.white : new Color(0.3f, 0.3f, 0.3f);
    }
}
