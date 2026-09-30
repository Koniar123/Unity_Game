using UnityEngine;

public static class AchievementService
{

    public static void UnlockDistanceMilestone(int distance)
    {
        if (distance >= 100)
            AchievementManager.Instance.Unlock("dist_100");

        if (distance >= 1000)
            AchievementManager.Instance.Unlock("dist_1000");

        if (distance >= 10000)
            AchievementManager.Instance.Unlock("dist_10000");

        if (distance >= 100000)
            AchievementManager.Instance.Unlock("dist_100000");
    }


    public static void UnlockFirstJump()
    {
        AchievementManager.Instance.Unlock("first_jump");
    }

    public static void OnObstacleDestroyed(int totalDestroyed)
    {
        if (totalDestroyed >= 10)
            AchievementManager.Instance.Unlock("obstacles_10");
        if (totalDestroyed >= 50)
            AchievementManager.Instance.Unlock("obstacles_50");
    }
}
