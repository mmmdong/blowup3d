using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyList : MonoBehaviour
{
    public static EnemyList instance;
    public int currentLev;
    public EnemyCollisionController[] enemies;

    [Space(20f)]
    [Header("UI")]
    public Transform _effectPos;
    public GameObject _valueUi;
    public int _capacity;

    [HideInInspector] public int _currenctValue;

    private Queue<Text> _txtQue = new Queue<Text>();
    private Coroutine _coMove;
    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    private void Start()
    {
        for (var i = 0; i < _capacity; i++)
        {
            AddText();
        }

        for (var i = 0; i < enemies.Length; i++)
        {
            enemies[i]._value = i + 1;
            enemies[i]._fullHp = (long)Mathf.Pow(10, enemies[i]._value) * 5;
            enemies[i]._currentHp = enemies[i]._fullHp;
        }
    }

    private void AddText()
    {
        var text = Instantiate(_valueUi, _effectPos.position, Quaternion.identity, _effectPos).GetComponent<Text>();
        text.gameObject.SetActive(false);
        _txtQue.Enqueue(text);
    }

    public Text Spawn(string value)
    {
        if (_txtQue.Count == 0)
        {
            _capacity++;
            AddText();
        }

        var text = _txtQue.Dequeue();
        text.text = value;
        text.gameObject.SetActive(true);

        StartCoroutine(CoObjMove(text));
        return text;
    }

    public void DestroyText(Text text)
    {
        text.rectTransform.localPosition = Vector2.zero;
        text.text = string.Empty;
        var textCanvasGroup = text.gameObject.GetComponent<CanvasGroup>();
        textCanvasGroup.alpha = 1f;
        text.gameObject.SetActive(false);
        _txtQue.Enqueue(text);
    }

    private IEnumerator CoObjMove(Text text)
    {
        while (true)
        {
            var pos = text.rectTransform.localPosition;
            pos.y += 10f;
            text.rectTransform.localPosition = Vector2.MoveTowards(text.rectTransform.localPosition, pos, 500f * Time.deltaTime);

            var textCanvasGroup = text.gameObject.GetComponent<CanvasGroup>();
            textCanvasGroup.alpha -= 2f * Time.deltaTime;

            if (textCanvasGroup.alpha <= 0f)
                break;

            yield return null;
        }

        DestroyText(text);
    }

    public void BlowEnemy(){
        StartCoroutine(CoBlow());
    }

    private IEnumerator CoBlow(){
        enemies[currentLev].col.isTrigger = true;
        enemies[currentLev].gameObject.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        enemies[currentLev].transform.localScale = Vector3.zero;
        enemies[currentLev].gameObject.SetActive(true);
        enemies[currentLev].FullHp();
    }
}
