using UnityEngine;

public class GameManager : MonoBehaviour
{
    public struct GameInfo
    {
        public int mainStageMax { get; set; }
        public int subStageMax { get; set; }
        public int mainStageCurr { get; set; }
        public int subStageCurr { get; set; }
    }

    public static GameManager Instance;
    public GameInfo gameInfo;

    public Player player;
    public bool enterNext = true;

    private int monsterCount = 2;
    private int remainMonster;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("GameManager instance already exists, destroy this one");
            Destroy(gameObject);
        }
    }

    public GameManager()
    {
        if(true)
        {
            //���� �����Ͱ� ������
            gameInfo.mainStageMax = 1;
            gameInfo.subStageMax = 1;
            gameInfo.mainStageCurr = 1;
            gameInfo.subStageCurr = 1;
            remainMonster = monsterCount;
        }
        else
        {
            //���� �����Ͱ� ������ ������ �ε�
        }
    }

    public void MonsterDie(int Drop_ID)
    {
        //�÷��̾� ���� ����
        var data = DataTableMgr.GetTable<DropTable>().GetMonsterData(Drop_ID);
        player.GetItem(data);

        remainMonster -= 1; //���� �� �� ����
        if (remainMonster > 0) return; //��ƾ� �� ���� ���� ���������� ���� ���̰� return

        //�� ������ �ٽ� ä���
        remainMonster = monsterCount;
        Debug.Log("�������� Ŭ����");
        player.StageClear();  //�������� Ŭ�����ϸ� ĳ���� ü�� �ʱ�ȭ

        //���� ���������� �ִ� ���������� �Ȱ�����
        if (gameInfo.mainStageMax == gameInfo.mainStageCurr && gameInfo.subStageMax == gameInfo.subStageCurr)
        {
            gameInfo.subStageMax++; //���� �������� 1 �ø�
            if(gameInfo.subStageMax == 10) //���� ���������� 10�̸�
            {
                //���� �������� 0���� ���̰� ������ 1 ����
                gameInfo.subStageMax = 1;
                gameInfo.mainStageMax++;
            }
        }

        if (!enterNext) return; //�������� �ݺ� �ɼ� ���������� return

        //�ݺ� �ɼ� ���� ������ ���� �������� ����
        gameInfo.subStageCurr++; 
        if (gameInfo.subStageCurr == 10) 
        {
            gameInfo.subStageCurr = 1;
            gameInfo.mainStageCurr++;
        }
    }

    //�÷��̾ ������
    public void PlayerDie()
    {
        //�ٽ� �츲

        //�Ѵܰ� �Ʒ��� ����
        Debug.Log("�������� �϶�");
        gameInfo.subStageCurr--;
        if (gameInfo.subStageCurr == 0)
        {
            gameInfo.subStageCurr = 9;
            gameInfo.mainStageCurr--;
        }
    }
}