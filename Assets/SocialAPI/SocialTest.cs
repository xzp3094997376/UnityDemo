using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

[ExecuteInEditMode]
public class SocialTest : MonoBehaviour
{
    void Start()
    {
        // 验证并注册 ProcessAuthentication 回调
        // 需要进行此调用才能继续进行 Social API 中的其他调用
        Social.localUser.Authenticate(ProcessAuthentication);
    }

    // 当验证完成时将调用此函数
    // 请注意，如果操作成功，Social.localUser 将包含来自服务器的数据。
    void ProcessAuthentication(bool success)
    {
        if (success)
        {
            Debug.Log("Authenticated, checking achievements");

            // 请求加载的成就，并注册回调来处理它们
            Social.LoadAchievements(ProcessLoadedAchievements);
        }
        else
            Debug.Log("Failed to authenticate");
    }

    // LoadAchievement 调用完成时将调用此函数
    void ProcessLoadedAchievements(IAchievement[] achievements)
    {
        if (achievements.Length == 0)
            Debug.Log("Error: no achievements found");
        else
            Debug.Log("Got " + achievements.Length + " achievements");

        // 也可以按照以下方式调用函数
        Social.ReportProgress("Achievement01", 100.0, result => {
            if (result)
                Debug.Log("Successfully reported achievement progress");
            else
                Debug.Log("Failed to report achievement");
        });
    }
}
