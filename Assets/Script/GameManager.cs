using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    public TextMeshProUGUI stage;
    public TextMeshProUGUI stageLoop;

    public Player player;
    public bool enterNext = true;

    private int monsterCount = 1;
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
            gameInfo.mainStageMax = 4;
            gameInfo.subStageMax = 4;
            gameInfo.mainStageCurr = 4;
            gameInfo.subStageCurr = 4;
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
            if(gameInfo.subStageMax == 11) //���� ���������� 10�̸�
            {
                //���� �������� 0���� ���̰� ������ 1 ����
                gameInfo.subStageMax = 1;
                gameInfo.mainStageMax++;
            }
        }

        if (!enterNext) return; //�������� �ݺ� �ɼ� ���������� return

        //�ݺ� �ɼ� ���� ������ ���� �������� ����
        gameInfo.subStageCurr++; 
        if (gameInfo.subStageCurr == 11) 
        {
            gameInfo.subStageCurr = 1;
            gameInfo.mainStageCurr++;
        }

        SetStageText();
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
            gameInfo.subStageCurr = 10;
            gameInfo.mainStageCurr--;
        }
        SetStageText();

        if (!enterNext) return;
        StageLoopOnOff();
    }

    public void SetStageText()
    {
        if (stage == null) return;
        stage.SetText($"KF {gameInfo.mainStageCurr} - {gameInfo.subStageCurr}");
    }

    public void StageLoopOnOff()
    {
        if(stageLoop == null) return;   
        if(enterNext)
        {
            //���� �������� ���� ��Ȱ��
            enterNext = !enterNext;
            stageLoop.SetText("�������� �ݺ�\nON");
        }
        else
        {
            //���� �������� ���� Ȱ��
            enterNext = !enterNext;
            stageLoop.SetText("�������� �ݺ�\nOFF");
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene("UI Test");
    }
}