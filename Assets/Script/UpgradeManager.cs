using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GoldEnhanceLevel
{
    public int hp_level = 0;
    public int atk_level = 0;
}

public class CharacterEnhanceLevel
{
    public int hp_level = 0;
    public int atk_level = 0;
}

public class UpgradeManager : MonoBehaviour
{
    public Player player;
    private GoldEnhanceTable goldEnhanceTable;
    private CharacterEnhanceTable enhanceTable;
    private GoldEnhanceLevel goldLevel;
    private CharacterEnhanceLevel charLevel;

    public TextMeshProUGUI textGoldHP;
    public TextMeshProUGUI textGoldAtk;

    void Start()
    {
        enhanceTable = DataTableMgr.GetTable<CharacterEnhanceTable>();
        goldEnhanceTable = DataTableMgr.GetTable<GoldEnhanceTable>();

        goldLevel = new GoldEnhanceLevel();
        charLevel = new CharacterEnhanceLevel();

        Debug.Log("12");
    }

    // Update is called once per frame
    void Update()
    {
    }


    #region
    public void Upgrade()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("1");
            var data = goldEnhanceTable.GetData(goldLevel.hp_level);

            if(data.HP_Gold <= player.status._gold)
            {
                player.status._gold -= data.HP_Gold;
                player.MaxHealth += data.GB_HP;
                goldLevel.hp_level++;
                Debug.Log($"ü�� {goldLevel.hp_level}���� {goldLevel.hp_level+1}�� ���׷��̵�");
            }
            else
            {
                Debug.Log("��尡 ���ڶ��ϴ�");
            }
        }

        if (Input.GetKeyDown(KeyCode.F2))
        {
            var data = goldEnhanceTable.GetData(goldLevel.atk_level);

            if (data.ATK_Gold <= player.status._gold)
            {
                player.status._gold -= data.ATK_Gold;
                player.Damage += data.ATK_Gold;
                goldLevel.atk_level++;
                Debug.Log($"���� {goldLevel.atk_level}���� {goldLevel.atk_level + 1}�� ���׷��̵�");
            }
            else
            {
                Debug.Log("��尡 ���ڶ��ϴ�");
            }
        }

        if (Input.GetKeyDown(KeyCode.F3))
        {
            var data = enhanceTable.GetData(charLevel.hp_level);

            if (player.status._levelPoint > 0)
            {
                player.status._levelPoint--;
                player.Damage += data.AB_HP;
                charLevel.hp_level++;
                Debug.Log($"ü�� {charLevel.hp_level}���� {charLevel.hp_level + 1}�� ���׷��̵�");
            }
            else
            {
                Debug.Log("����Ʈ�� ���ڶ��ϴ�");
            }
        }

        if (Input.GetKeyDown(KeyCode.F4))
        {
            var data = enhanceTable.GetData(charLevel.atk_level);

            if (player.status._levelPoint > 0)
            {
                player.status._levelPoint--;
                player.Damage += data.AB_ATK;
                charLevel.atk_level++;
                Debug.Log($"���� {charLevel.atk_level}���� {charLevel.atk_level + 1}�� ���׷��̵�");
            }
            else
            {
                Debug.Log("����Ʈ�� ���ڶ��ϴ�");
            }
        }    
    }
    #endregion 

    public void GoldUpgradeHealth()
    {
        var data = goldEnhanceTable.GetData(goldLevel.hp_level);

        if (data.HP_Gold <= player.status._gold)
        {
            player.status._gold -= data.HP_Gold;
            player.MaxHealth += data.GB_HP;
            goldLevel.hp_level++;
            Debug.Log($"ü�� {goldLevel.hp_level}���� {goldLevel.hp_level + 1}�� ���׷��̵�");

            data = goldEnhanceTable.GetData(goldLevel.hp_level);
            textGoldHP.SetText($"��� ��ȭ\nü��\n{data.HP_Gold}");
        }
        else
        {
            Debug.Log("��尡 ���ڶ��ϴ�");
        }
    }
    public void GoldUpgradeDamage()
    {
        var data = goldEnhanceTable.GetData(goldLevel.atk_level);

        if (data.ATK_Gold <= player.status._gold)
        {
            player.status._gold -= data.ATK_Gold;
            player.Damage += data.GB_ATK;
            goldLevel.atk_level++;
            Debug.Log($"���� {goldLevel.atk_level}���� {goldLevel.atk_level + 1}�� ���׷��̵�");

            data = goldEnhanceTable.GetData(goldLevel.atk_level);
            textGoldAtk.SetText($"��� ��ȭ\n������\n{data.ATK_Gold}");
        }
        else
        {
            Debug.Log("��尡 ���ڶ��ϴ�");
        }
    }
    public void PointUpgradeHealth()
    {
        var data = enhanceTable.GetData(charLevel.hp_level);

        if (player.status._levelPoint > 0)
        {
            player.status._levelPoint--;
            player.MaxHealth += data.AB_HP;
            charLevel.hp_level++;
            Debug.Log($"ü�� {charLevel.hp_level}���� {charLevel.hp_level + 1}�� ���׷��̵�");
        }
        else
        {
            Debug.Log("����Ʈ�� ���ڶ��ϴ�");
        }
    }
    public void PointUpgradeDamage()
    {
        var data = enhanceTable.GetData(charLevel.atk_level);

        if (player.status._levelPoint > 0)
        {
            player.status._levelPoint--;
            player.Damage += data.AB_ATK;
            charLevel.atk_level++;
            Debug.Log($"���� {charLevel.atk_level}���� {charLevel.atk_level + 1}�� ���׷��̵�");
        }
        else
        {
            Debug.Log("����Ʈ�� ���ڶ��ϴ�");
        }
    }
}
