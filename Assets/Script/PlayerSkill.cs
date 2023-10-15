using System.Collections.Generic;
using System.Linq.Expressions;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public struct SkillInfo
{
    public int maxLevel;
    public int level;
    public int cost;
    public float increaseCost;
    public SkillData data;

    public const int Skill_Tier1 = 50;
    public const int Skill_Tier2 = 100;
    public const int Skill_Tier3 = 201;
}

public class PlayerSkill : MonoBehaviour
{
    private Player player;

    private SkillInfo magmaInfo;
    private SkillInfo explosionInfo;
    private SkillInfo exerciseInfo;
    private SkillInfo fenceInfo;
    private SkillInfo desireInfo;

    public List<Text> magmaText;
    public List<Text> explosionText;
    public List<Text> exerciseText;
    public List<Text> fenceText;
    public List<Text> desireText;


    private SkillTable skillTable;

    public Transform magmaPos;

    private float magmaCoolTime;
    private float explosionCoolTime;
    public TextMeshProUGUI magmaTimer;
    public TextMeshProUGUI explosionTimer;

    private void Start()
    {
        SkillCostTable skillCostTable = DataTableMgr.GetTable<SkillCostTable>();
        
        player = GetComponentInParent<Player>();
        skillTable = DataTableMgr.GetTable<SkillTable>();

        var magma = skillCostTable.GetCostData(0);
        magmaInfo.data = skillTable.GetSkillData(11000101);
        magmaInfo.maxLevel = magma.maxLevel;
        magmaInfo.cost = magma.cost;
        magmaInfo.increaseCost = magma.increaseCost;
        magmaInfo.level = 1;

        var explosion = skillCostTable.GetCostData(1);
        explosionInfo.data = skillTable.GetSkillData(11000201);
        explosionInfo.maxLevel = explosion.maxLevel;
        explosionInfo.cost = explosion.cost;
        explosionInfo.increaseCost = explosion.increaseCost;
        explosionInfo.level = 1;

        var exercise = skillCostTable.GetCostData(2);
        exerciseInfo.data = skillTable.GetSkillData(21000101);
        exerciseInfo.maxLevel = exercise.maxLevel;
        exerciseInfo.cost = exercise.cost;
        exerciseInfo.increaseCost = exercise.increaseCost;
        exerciseInfo.level = 1;

        var fence = skillCostTable.GetCostData(3);
        fenceInfo.data = skillTable.GetSkillData(21000102);
        fenceInfo.maxLevel = fence.maxLevel;
        fenceInfo.cost = fence.cost;
        fenceInfo.increaseCost = fence.increaseCost;
        fenceInfo.level = 1;

        var desire = skillCostTable.GetCostData(4);
        desireInfo.data = skillTable.GetSkillData(21000103);
        desireInfo.maxLevel = desire.maxLevel;
        desireInfo.cost = desire.cost;
        desireInfo.increaseCost = desire.increaseCost;
        desireInfo.level = 1;

        magmaCoolTime = 0;
        explosionCoolTime = 0;
        //������ ������ �ε�
        //�Ʒ��� �߰� �ؾ���
    }

    private void Update()
    {
        if(magmaCoolTime > 0)
        {
            magmaTimer.gameObject.SetActive(true);
            magmaCoolTime -= Time.deltaTime;
            magmaTimer.SetText($"{Mathf.RoundToInt(magmaCoolTime)}");
        }
        else
        {
            magmaTimer.gameObject.SetActive(false);
        }

        if(explosionCoolTime > 0)
        {
            explosionTimer.gameObject.SetActive(true);
            explosionCoolTime -= Time.deltaTime;
            explosionTimer.SetText($"{Mathf.RoundToInt(explosionCoolTime)}");
        }
        else
        {
            explosionTimer.gameObject.SetActive(false);
        }
    }

    public void ActiveMagma()
    {
        if(magmaCoolTime > 0)
        {
            Debug.Log("��Ÿ��");
            return;
        }

        var obj = ObjectPoolManager.instance.GetGo("SkillEffect");
        obj.GetComponent<SkillEffect>().SkillMagma(player, magmaInfo, magmaPos);

        magmaCoolTime = magmaInfo.data.Skill_Cool;
    }

    public void ActiveExplosion()
    {
        if (player.enemy == null)
        {
            Debug.Log("����� ����");
            return;
        }
        if (explosionCoolTime > 0)
        {
            Debug.Log("��Ÿ��");
            return;
        }

        var obj = ObjectPoolManager.instance.GetGo("SkillEffect");
        obj.GetComponent<SkillEffect>().SkillExplosion(player, explosionInfo);

        explosionCoolTime = explosionInfo.data.Skill_Cool;
    }

    public void UpgradeMagma()
    {
        if(magmaInfo.level >= magmaInfo.maxLevel)
        {
            Debug.Log("�ִ� ����");
            return;
        }

        var cost = magmaInfo.cost * Mathf.Pow(magmaInfo.increaseCost, magmaInfo.level - 1);

        if(player.status._gold < (int)cost)
        {
            Debug.Log("��� ���ڶ�");
            return;
        }

        player.status._gold -= (int)cost;


        magmaInfo.level++;
        Debug.Log(("���׷��̵�", "���� ����: " + magmaInfo.level));

        if (magmaInfo.level == SkillInfo.Skill_Tier1)
        {
            magmaInfo.data = skillTable.GetSkillData(11000102);
        }
        else if(magmaInfo.level == SkillInfo.Skill_Tier2)
        {
            magmaInfo.data = skillTable.GetSkillData(11000103);
        }

        float ratio;
        if(magmaInfo.level < SkillInfo.Skill_Tier1)
        {
            ratio = magmaInfo.data.Skill_Dmg + (magmaInfo.data.Skill_LevelUpDmgIncrease * (magmaInfo.level - 1));
        }
        else if(magmaInfo.level < SkillInfo.Skill_Tier2)
        {
            ratio = magmaInfo.data.Skill_Dmg + (magmaInfo.data.Skill_LevelUpDmgIncrease * (magmaInfo.level - SkillInfo.Skill_Tier1));
        }
        else
        {
            ratio = magmaInfo.data.Skill_Dmg + (magmaInfo.data.Skill_LevelUpDmgIncrease * (magmaInfo.level - SkillInfo.Skill_Tier2));
        }   

        magmaText[0].text = $"LV. {magmaInfo.level}";
        magmaText[1].text = $"���: {Mathf.RoundToInt(magmaInfo.cost * Mathf.Pow(magmaInfo.increaseCost, magmaInfo.level - 1))}G";
        magmaText[2].text = $"��ö���� ����� ���� ������ ���� �����Ѵ�.\r\n��ȭ ������ ���� ��ų�� ������ ����.\r\n\r\n" +
            $"���ݷ��� {Mathf.RoundToInt(ratio * 100)}% �� 1ȸ Ÿ���Ѵ�.";
    }

    public void UpgradeExplosion()
    {
        if (explosionInfo.level >= explosionInfo.maxLevel)
        {
            Debug.Log("�ִ� ����");
            return;
        }

        var cost = explosionInfo.cost * Mathf.Pow(explosionInfo.increaseCost, explosionInfo.level - 1);

        if (player.status._gold < (int)cost)
        {
            Debug.Log("��� ���ڶ�");
            return;
        }

        player.status._gold -= (int)cost;
        explosionInfo.level++;
        Debug.Log(("���׷��̵�", "���� ����: " + explosionInfo.level));

        if (explosionInfo.level == SkillInfo.Skill_Tier1)
        {
            explosionInfo.data = skillTable.GetSkillData(11000202);
        }
        else if (explosionInfo.level == SkillInfo.Skill_Tier2)
        {
            explosionInfo.data = skillTable.GetSkillData(11000203);
        }

        float ratio;
        if (explosionInfo.level < SkillInfo.Skill_Tier1)
        {
            ratio = explosionInfo.data.Skill_Dmg + (explosionInfo.data.Skill_LevelUpDmgIncrease * (explosionInfo.level - 1));
        }
        else if (explosionInfo.level < SkillInfo.Skill_Tier2)
        {
            ratio = explosionInfo.data.Skill_Dmg + (explosionInfo.data.Skill_LevelUpDmgIncrease * (explosionInfo.level - SkillInfo.Skill_Tier1));
        }
        else
        {
            ratio = explosionInfo.data.Skill_Dmg + (explosionInfo.data.Skill_LevelUpDmgIncrease * (explosionInfo.level - SkillInfo.Skill_Tier2));
        }

        explosionText[0].text = $"LV. {explosionInfo.level}";
        explosionText[1].text = $"���: {Mathf.RoundToInt(explosionInfo.cost * Mathf.Pow(explosionInfo.increaseCost, explosionInfo.level - 1))}G";
        explosionText[2].text = $"��ö���� ���� ���鿡 �ִ� ����� ���߽�Ų��.\r\n��ȭ ������ ���� ��ų�� ������ ����.\r\n\r\n" +
            $"���� ���ݷ��� {Mathf.RoundToInt(ratio * 100)}% ��ŭ ���� 1ȸ Ÿ���Ѵ�.";
    }

    public void UpgradeExercise()
    {
        if (exerciseInfo.level >= exerciseInfo.maxLevel)
        {
            Debug.Log("�ִ� ����");
            return;
        }

        var cost = exerciseInfo.cost * Mathf.Pow(exerciseInfo.increaseCost, exerciseInfo.level - 1);

        if (player.status._gold < (int)cost)
        {
            Debug.Log("��� ���ڶ�");
            return;
        }

        player.Damage += exerciseInfo.data.Skill_LevelUpATKUpIncrease;

        player.status._gold -= (int)cost;
        exerciseInfo.level++;
        Debug.Log("���׷��̵�");

        exerciseText[0].text = $"LV. {exerciseInfo.level}";
        exerciseText[1].text = $"���: {Mathf.RoundToInt(exerciseInfo.cost * Mathf.Pow(exerciseInfo.increaseCost, exerciseInfo.level - 1))}G";
        exerciseText[2].text = $"��ö���� ƴƴ�� ��� �Ͽ� ���� ���ݷ���\n10 ��ŭ �����Ѵ�.";
    }

    public void UpgradeFence()
    {
        if (fenceInfo.level >= fenceInfo.maxLevel)
        {
            Debug.Log("�ִ� ����");
            return;
        }

        var cost = fenceInfo.cost * Mathf.Pow(fenceInfo.increaseCost, fenceInfo.level - 1);

        if (player.status._gold < (int)cost)
        {
            Debug.Log("��� ���ڶ�");
            return;
        }
        player.status.attackDelay -= fenceInfo.data.Skill_LevelUpATKSpeedIncrease;

        player.status._gold -= (int)cost;
        fenceInfo.level++;
        Debug.Log("���׷��̵�");

        fenceText[0].text = $"LV. {fenceInfo.level}";
        fenceText[1].text = $"���: {Mathf.RoundToInt(fenceInfo.cost * Mathf.Pow(fenceInfo.increaseCost, fenceInfo.level - 1))}G";
        fenceText[2].text = $"��ö���� Į���� �����Ͽ� ���ݼӵ��� \r\n0.5% ��ŭ �����Ѵ�.";
    }

    public void UpgradeDesire()
    {
        if (desireInfo.level >= desireInfo.maxLevel)
        {
            Debug.Log("�ִ� ����");
            return;
        }

        var cost = desireInfo.cost * Mathf.Pow(desireInfo.increaseCost, desireInfo.level - 1);

        if (player.status._gold < (int)cost)
        {
            Debug.Log("��� ���ڶ�");
            return;
        }

        player.status._MAP += desireInfo.data.Skill_LevelUpMAPIncrease;
        player.status._GOD += desireInfo.data.Skill_LevelUpGODIncrease;

        player.status._gold -= (int)cost;
        desireInfo.level++;
        Debug.Log("���׷��̵�");

        desireText[0].text = $"LV. {desireInfo.level}";
        desireText[1].text = $"���: {Mathf.RoundToInt(desireInfo.cost * Mathf.Pow(desireInfo.increaseCost, desireInfo.level - 1))}G";
        desireText[2].text = $"��ö���� ���ſ��� �⵵�Ͽ� ���°� �ŷ��� \r\n���� 5 �����Ѵ�.";
    }

    
}