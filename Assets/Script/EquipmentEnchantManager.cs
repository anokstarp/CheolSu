using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentEnchantManager : MonoBehaviour
{
    [System.Serializable]
    private class EnchantInfo
    {
        public Text NameEnchant;
        public Text ItemInfo;
        public Text Cost;
        public Button equipButton;
        public Button enchantButton;
    }

    [SerializeField]
    private EnchantInfo[] enchantInfos;

    public Player player;
    public Dictionary<GachaData, Item> list;
    public GachaTable table;

    public Button[] equipButton;
    public Button[] enchantButton;


    private void Awake()
    {
        list = player.itemList;
        table = DataTableMgr.GetTable<GachaTable>();
    }

    private void Start()
    {
        for (int i = 0; i < enchantButton.Length; i++)
        {
            var temp = i;
            enchantInfos[temp].equipButton.onClick.AddListener(() => EquipItem(temp));
            enchantInfos[temp].enchantButton.onClick.AddListener(() => EnchantEquipment(temp));
        }

    }

    private void Update()
    {
        
    }

    private void EnchantEquipment(int num)
    {
        GachaData data = null;

        if (num < 20)
        {
            data = table.GetWeaponData(num); //������ ��ȣ �޾Ƽ� ã��
        }
        else if (num >= 20)
        {
            data = table.GetArmorData(num - 20);
        }

        var item = list[data]; //�װɷ� ��ųʸ����� �̾ƿ�

        if (!item.unlock) return; //��� �ȵ����� ����

        if (player.status._gold < item.data.Item_Gold + (item.data.Item_LevelUp_Gold * item.enhance)) return;

        player.status._gold -= item.data.Item_Gold + (item.data.Item_LevelUp_Gold * item.enhance);
        item.enhance++;

        enchantInfos[num].NameEnchant.text = $"{item.data.Item_Name} +{item.enhance}";
        enchantInfos[num].Cost.text = $"���: {item.data.Item_Gold + (item.data.Item_LevelUp_Gold * item.enhance)}G";


        switch (item.data.Item_Kind)
        {
            case 1:
                enchantInfos[num].ItemInfo.text = $"\n\n���� ȿ��\n\n���ݷ� {item.data.Item_ATK + (item.data.Item_LevelUp_ATKUP * item.enhance)}����";

                if(player.status.e_weapon == item.data)
                {
                    player.Damage += item.data.Item_LevelUp_ATKUP;
                }
                break;
            case 2:
                enchantInfos[num].ItemInfo.text = $"\n\n���� ȿ��\n\nü�� {item.data.Item_HP + (item.data.Item_LevelUp_HPUP * item.enhance)}����";

                if (player.status.e_topArmor == item.data)
                {
                    player.MaxHealth += item.data.Item_LevelUp_HPUP;
                }
                break;
            case 3:
                enchantInfos[num].ItemInfo.text = $"\n\n���� ȿ��\n\n" +
                    $"���� {item.data.Item_MAP + (item.data.Item_LevelUP_MAPUP * item.enhance)}����\n" +
                    $"�ŷ� {item.data.Item_GOD + (item.data.Item_LevelUP_GODUP * item.enhance)}����";

                if (player.status.e_bottomArmor == item.data)
                {
                    player.status._MAP += item.data.Item_MAP;
                    player.status._GOD += item.data.Item_GOD;
                }
                break;
        }
    }

    private void EquipItem(int num)
    {
        GachaData data = null;
        if (num < 20)
        {
            data = table.GetWeaponData(num); //������ ��ȣ �޾Ƽ� ã��
        }
        else if (num >= 20)
        {
            data = table.GetArmorData(num - 20);
        }

        var item = list[data]; //�װɷ� ��ųʸ����� �̾ƿ�
        if (!item.unlock) 
        {
            Debug.Log("��� �ȵ�");
            return; //��� �ȵ����� ����
        }

        switch (item.data.Item_Kind) //������ ����
        {
            case 1:
                var e_weapon = player.status.e_weapon;
                if (e_weapon != null)
                {
                    player.Damage -= e_weapon.Item_ATK + (e_weapon.Item_LevelUp_ATKUP * list[e_weapon].enhance);
                }
                player.Damage += item.data.Item_ATK + (item.data.Item_LevelUp_ATKUP * item.enhance);
                player.status.e_weapon = item.data;

                break;
            case 2:
                var e_top = player.status.e_topArmor;
                if(e_top != null)
                {
                    player.MaxHealth -= e_top.Item_HP + (e_top.Item_LevelUp_HPUP * list[e_top].enhance);
                }
                player.MaxHealth += item.data.Item_HP + (item.data.Item_LevelUp_HPUP * item.enhance);
                player.status.e_topArmor = item.data;

                break;
            case 3:
                var e_bottom = player.status.e_bottomArmor;
                if(e_bottom != null)
                {
                    player.status._MAP -= e_bottom.Item_MAP + (e_bottom.Item_LevelUP_MAPUP * list[e_bottom].enhance);
                    player.status._GOD -= e_bottom.Item_GOD + (e_bottom.Item_LevelUP_GODUP * list[e_bottom].enhance);
                }
                player.status._MAP += item.data.Item_MAP + (item.data.Item_LevelUP_MAPUP * item.enhance);
                player.status._GOD += item.data.Item_GOD + (item.data.Item_LevelUP_GODUP * item.enhance);
                player.status.e_bottomArmor = item.data;

                break;
        }
    }
}