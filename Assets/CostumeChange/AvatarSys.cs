using UnityEngine;
using System.Collections.Generic;

public class AvatarSys : MonoBehaviour
{
    //来源模型资源的物件
    private Transform source;

    //目标物件
    private Transform target;

    //实例化的源文件和目标文件
    private GameObject sourceobj;

    private GameObject targetobj;

    //模型资源
    private Dictionary<string, Dictionary<string, Transform>> data =
        new Dictionary<string, Dictionary<string, Transform>>();

    //播放的动作
    private Animation mAnim;

    //目标物件的骨架
    private Transform[] hips;

    //目标物件各部位的SkinnedMeshRenderer（参照）
    private Dictionary<string, SkinnedMeshRenderer> targetSmr = new Dictionary<string, SkinnedMeshRenderer>();

    public static AvatarSys instance;

    //各部分换装的名字
    string[,] avatarstr = new string[,]
        {{"coat", "003"}, {"hair", "003"}, {"pant", "003"}, {"hand", "003"}, {"foot", "003"}, {"head", "003"}};

    string[,] avatarstr0 = new string[,]
        {{"coat", "001"}, {"hair", "001"}, {"pant", "001"}, {"hand", "003"}, {"foot", "003"}, {"head", "003"}};

    string[,] avatarstr1 = new string[,]
        {{"coat", "003"}, {"hair", "001"}, {"pant", "001"}, {"hand", "003"}, {"foot", "001"}, {"head", "001"}};


    private float pos;

    //用于初始化
    void Start()
    {
        instance = this;
        AvatarManager(0.0f);
        AvatarManager0(1.0f);
        AvatarManager1(2.0f);
    }

    //创建多个换装模型
    void AvatarManager(float pos)
    {
        InstantiateAvatar();
        InstantiateSkeleton(pos);

        LoadAvatarData(source);
        hips = target.GetComponentsInChildren<Transform>();
        Inivatar();
    }

    void AvatarManager0(float pos)
    {
        InstantiateAvatar();
        InstantiateSkeleton(pos);

        LoadAvatarData(source);
        hips = target.GetComponentsInChildren<Transform>();
        Inivatar0();
    }

    void AvatarManager1(float pos)
    {
        InstantiateAvatar();
        InstantiateSkeleton(pos);

        LoadAvatarData(source);
        hips = target.GetComponentsInChildren<Transform>();
        Inivatar1();
    }

    //实例化Avatar模型
    void InstantiateAvatar()
    {
        sourceobj = Instantiate(Resources.Load("FemaleAvatar")) as GameObject;
        source = sourceobj.transform;
        sourceobj.SetActive(false);
    }

    //实例化骨骼动画
    void InstantiateSkeleton(float pos)
    {
        targetobj = Instantiate(Resources.Load("targetmodel")) as GameObject;
        target = targetobj.transform;
        target.transform.position = new Vector3(pos, 0.0f, 0.0f);
    }

    //加载Avatar数据
    void LoadAvatarData(Transform source)
    {
        data.Clear();
        targetSmr.Clear();

        if (source == null)
            return;
        SkinnedMeshRenderer[] parts = source.GetComponentsInChildren<SkinnedMeshRenderer>(true);
        foreach (SkinnedMeshRenderer part in parts)
        {
            string[] partName = part.name.Split('-');
            if (!data.ContainsKey(partName[0]))
            {
                data.Add(partName[0], new Dictionary<string, Transform>());
                GameObject partobj = new GameObject();
                partobj.name = partName[0];
                partobj.transform.parent = target;

                targetSmr.Add(partName[0], partobj.AddComponent<SkinnedMeshRenderer>());
            }

            data[partName[0]].Add(partName[1], part.transform);
        }
    }

    //改变Avatar模型
    public void ChangeMesh(string part, string item)
    {
        SkinnedMeshRenderer smr = data[part][item].GetComponent<SkinnedMeshRenderer>();

        List<Transform> bones = new List<Transform>();
        foreach (Transform bone in smr.bones)
        {
            foreach (Transform hip in hips)
            {
                if (hip.name != bone.name)
                {
                    continue;
                }

                bones.Add(hip);
                break;

            }
        }

        targetSmr[part].sharedMesh = smr.sharedMesh;
        targetSmr[part].bones = bones.ToArray();
        targetSmr[part].materials = smr.materials;
    }

    void Inivatar()
    {
        int nLength = avatarstr.GetLength(0);
        for (int i = 0; i < nLength; i++)
        {
            ChangeMesh(avatarstr[i, 0], avatarstr[i, 1]);
        }
    }

    void Inivatar0()
    {
        int nLength = avatarstr0.GetLength(0);
        for (int i = 0; i < nLength; i++)
        {
            ChangeMesh(avatarstr0[i, 0], avatarstr0[i, 1]);
        }
    }

    void Inivatar1()
    {
        int nLength = avatarstr1.GetLength(0);
        for (int i = 0; i < nLength; i++)
        {
            ChangeMesh(avatarstr1[i, 0], avatarstr1[i, 1]);
        }
    }
}

