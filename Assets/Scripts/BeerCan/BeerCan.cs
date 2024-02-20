using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeerCan : BeerCanBase
{
    public string name;

    public Material normalSprite; // ����״̬�� Sprite
    public Material highlightSprite; // ����״̬�� Sprite
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.material = normalSprite; // ��ʼ״̬Ϊ����״̬
        // ����������ÿ��ơ�Ʒ��͹޵ĳ�ʼ״̬����������
        currentState = BeerCanState.Empty;
        name = "BeerCan1"; // ����ơ�Ʒ��͹޵�����
    }

    public override void StartBrewingProcess()
    {
        StartCoroutine(BrewingProcess());
    }

    public override void NextState()
    {
        switch (currentState)
        {
            case BeerCanState.Empty:
                currentState = BeerCanState.Saccharification;
                timerManager.StartTimer($"{name}_SaccharificationTimer", 30f); // ���� Saccharification �׶εļ�ʱ��
                break;
            case BeerCanState.Saccharification:
                currentState = BeerCanState.Boiled;
                timerManager.StartTimer($"{name}_BoiledTimer", 60f); // ���� Boiled �׶εļ�ʱ��
                break;
            case BeerCanState.Boiled:
                currentState = BeerCanState.Fermentation;
                // ���� Fermentation �׶εļ�ʱ������������
                break;
                // �������״̬�Ĵ���
        }
    }

    IEnumerator BrewingProcess()
    {
        while (true)
        {
            switch (currentState)
            {
                case BeerCanState.Saccharification:
                    if (timerManager.GetRemainingTime($"{name}_SaccharificationTimer") <= 0f)
                    {
                        NextState();
                    }
                    break;
                case BeerCanState.Boiled:
                    if (timerManager.GetRemainingTime($"{name}_BoiledTimer") <= 0f)
                    {
                        NextState();
                    }
                    break;
                    // �������״̬�Ĵ���
            }
            yield return null;
        }
    }

    void LoadState()
    {
        if (PlayerPrefs.HasKey($"{name}_CurrentState"))
        {
            currentState = (BeerCanState)PlayerPrefs.GetInt($"{name}_CurrentState");
            // �������������״̬��Ϣ�������ʱ��ʣ��ʱ��
        }
    }

    void OnMouseEnter()
    {
        spriteRenderer.material = highlightSprite; // ������ʱ�л�������״̬
    }

    void OnMouseExit()
    {
        spriteRenderer.material = normalSprite; // ����뿪ʱ�л�������״̬
    }
}

