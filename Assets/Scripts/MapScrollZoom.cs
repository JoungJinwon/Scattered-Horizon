using System.Collections;
using UnityEngine;

public class ScrollZoom : MonoBehaviour
{
    public float scrollValue;
    public float elapsedTime = 0f;
    public float zoomSpeedTime = 0.5f;
    public float zoomMinScale = 1f;
    public float zoomMaxScale = 5f;
    public float currentMapScaleRate;
    public float targetMapScaleRate;

    public Vector3 mapLocalScale;
    public RectTransform content;

    private InputManager _input;

    private void Awake()
    {
        if (_input == null)
            _input = FindObjectOfType<InputManager>();
    }

    private void Start()
    {
        currentMapScaleRate = zoomMinScale;
        mapLocalScale = content.localScale;
    }

    private void FixedUpdate()
    {
        scrollValue = _input.scroll * 0.084f; // 120으로 반환되므로 10으로 줄여준다
        
        if (scrollValue != 0f)
            StartCoroutine(zoomCoroutine());
    }

    public void zoom()
    {
        if (scrollValue != 0f)
        {
            elapsedTime += Time.fixedDeltaTime;

            // 타겟 스케일 비율에 더해줄 Delta 값을 계산한다
            float zoomDelta = scrollValue * Time.fixedDeltaTime;
            // 현재 스케일 비율에 Delta값을 더해주지만, 그 값을 1과 5 사이로 제한한다
            targetMapScaleRate = Mathf.Clamp(currentMapScaleRate + zoomDelta, zoomMinScale, zoomMaxScale);

            mapLocalScale = Vector3.one * Mathf.Lerp(currentMapScaleRate, targetMapScaleRate, elapsedTime / zoomSpeedTime);
            content.localScale = mapLocalScale;
        }
        else
        {
            elapsedTime = 0f;
        }
    }

    public IEnumerator zoomCoroutine()
    {
        while (elapsedTime < zoomSpeedTime)
        {
            elapsedTime += Time.fixedDeltaTime;

            // 타겟 스케일 비율에 더해줄 Delta 값을 계산한다
            float zoomDelta = scrollValue * Time.fixedDeltaTime;
            // 현재 스케일 비율에 Delta값을 더해주지만, 그 값을 1과 5 사이로 제한한다
            targetMapScaleRate = Mathf.Clamp(currentMapScaleRate + zoomDelta, zoomMinScale, zoomMaxScale);

            mapLocalScale = Vector3.one * Mathf.Lerp(currentMapScaleRate, targetMapScaleRate, elapsedTime / zoomSpeedTime);
            content.localScale = mapLocalScale;

            yield return null;
        }
        

        elapsedTime = 0f;

    }
}
