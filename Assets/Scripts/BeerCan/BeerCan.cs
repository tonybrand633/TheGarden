using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeerCan : BeerCanBase
{
    public string name;

    public Material normalSprite; // 正常状态的 Sprite
    public Material highlightSprite; // 高亮状态的 Sprite
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.material = normalSprite; // 初始状态为正常状态
        // 在这里设置每个啤酒发酵罐的初始状态和其他属性
        currentState = BeerCanState.Empty;
        name = "BeerCan1"; // 设置啤酒发酵罐的名称
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
                timerManager.StartTimer($"{name}_SaccharificationTimer", 30f); // 设置 Saccharification 阶段的计时器
                break;
            case BeerCanState.Saccharification:
                currentState = BeerCanState.Boiled;
                timerManager.StartTimer($"{name}_BoiledTimer", 60f); // 设置 Boiled 阶段的计时器
                break;
            case BeerCanState.Boiled:
                currentState = BeerCanState.Fermentation;
                // 设置 Fermentation 阶段的计时器或其他属性
                break;
                // 添加其他状态的处理
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
                    // 添加其他状态的处理
            }
            yield return null;
        }
    }

    void LoadState()
    {
        if (PlayerPrefs.HasKey($"{name}_CurrentState"))
        {
            currentState = (BeerCanState)PlayerPrefs.GetInt($"{name}_CurrentState");
            // 在这里加载其他状态信息，比如计时器剩余时间
        }
    }

    void OnMouseEnter()
    {
        spriteRenderer.material = highlightSprite; // 鼠标进入时切换到高亮状态
    }

    void OnMouseExit()
    {
        spriteRenderer.material = normalSprite; // 鼠标离开时切换回正常状态
    }
}

