using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.CloudSave;
using Unity.Services.CloudSave.Models;

[Serializable]
public class AchievementDefinition
{
    public string id;           
    public string title;        
    public string description;  
    public Sprite icon;         
    public bool isSecret;       
}

[Serializable]
public class AchievementState
{
    public bool unlocked;
    public string unlockedAt;
}

public class AchievementManager : MonoBehaviour
{
    public static AchievementManager Instance { get; private set; }

    [Header("Definicje achievementów — uzupełnij w Inspectorze")]
    public List<AchievementDefinition> definitions = new();

    private Dictionary<string, AchievementState> _states = new();

    private const string CloudKey = "achievements";

    public event Action<AchievementDefinition> OnAchievementUnlocked;


    private void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private async void Start()
    {
        await InitializeUGS();
        await LoadFromCloud();
    }

    private async Task InitializeUGS()
    {
        try
        {
            await UnityServices.InitializeAsync();

            if (!AuthenticationService.Instance.IsSignedIn)
            {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();
                Debug.Log($"[UGS] Zalogowano jako: {AuthenticationService.Instance.PlayerId}");
            }
        }
        catch (Exception e)
        {
            Debug.LogWarning($"[UGS] Błąd logowania — tryb offline: {e.Message}");
        }
    }

    private async Task LoadFromCloud()
    {
        try
        {
            var keys = new HashSet<string> { CloudKey };
            var data = await CloudSaveService.Instance.Data.Player.LoadAsync(keys);

            if (data.TryGetValue(CloudKey, out var item))
            {
                var loaded = item.Value.GetAs<Dictionary<string, AchievementState>>();
                if (loaded != null) _states = loaded;
                Debug.Log($"[Achievements] Wczytano {_states.Count} achievementów z chmury.");
            }
        }
        catch (Exception e)
        {
            Debug.LogWarning($"[Achievements] Nie można wczytać z chmury (tryb offline): {e.Message}");
        }
    }


    private async Task SaveToCloud()
    {
        try
        {
            var data = new Dictionary<string, object> { { CloudKey, _states } };
            await CloudSaveService.Instance.Data.Player.SaveAsync(data);
        }
        catch (Exception e)
        {
            Debug.LogWarning($"[Achievements] Zapis do chmury nieudany: {e.Message}");
        }
    }

    public async void Unlock(string id)
    {
        if (IsUnlocked(id)) return;

        var definition = definitions.Find(d => d.id == id);
        if (definition == null)
        {
            Debug.LogWarning($"[Achievements] Nieznane ID: {id}");
            return;
        }

        _states[id] = new AchievementState
        {
            unlocked = true,
            unlockedAt = DateTime.UtcNow.ToString("o")
        };

        OnAchievementUnlocked?.Invoke(definition);
        Debug.Log($"[Achievements] Odblokowano: {definition.title}");

        await SaveToCloud();
    }

    public bool IsUnlocked(string id) =>
        _states.TryGetValue(id, out var s) && s.unlocked;

    public List<AchievementDefinition> GetAllDefinitions() => definitions;
    public Dictionary<string, AchievementState> GetAllStates() => _states;
}
