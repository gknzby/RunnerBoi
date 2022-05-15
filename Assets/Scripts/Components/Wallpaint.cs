using UnityEngine;
using System.Collections;
using RunnerBoi;

public class Wallpaint : MonoBehaviour
{
    [Tooltip("Texture Dimensions that will be on wall")]
    [SerializeField] private Vector2Int texDimensions;

    [Tooltip("Texture that to read render texture data. Low values: Performance, High values: Accuracy")]
    [SerializeField] private Vector2Int readTexDimensions;

    [SerializeField] private Material targetMaterial;

    private TextureFormat texFormat = TextureFormat.ARGB32;
    private FilterMode texFilterMode = FilterMode.Point;
    private RenderTextureFormat renTexFormat = RenderTextureFormat.ARGB32;
    private RenderTextureReadWrite renTexRW = RenderTextureReadWrite.Linear;
    private int texDepth = 0;

    private Texture2D readTex;
    private RenderTexture wallRenTex;
    private RenderTexture readRenTex;
    private Rect readTexRect;

    private Vector2 oldPos = new Vector2(0.5f, 0.5f);
    private Vector2 posVelocity = Vector2.zero;
    private int paintPercentage = 0;

    private void Start()
    {
        readTex = new Texture2D(readTexDimensions.x, readTexDimensions.y, texFormat, true);

        wallRenTex = new RenderTexture(texDimensions.x, texDimensions.y, 0, renTexFormat, renTexRW);
        wallRenTex.filterMode = texFilterMode;

        Graphics.Blit(Texture2D.whiteTexture, wallRenTex);
        targetMaterial.mainTexture = wallRenTex;

        readTexRect = new Rect(0, 0, texDimensions.x, texDimensions.y);

        readRenTex = new RenderTexture(readTexDimensions.x, readTexDimensions.y, texDepth, renTexFormat, renTexRW);
        RenderTexture.active = readRenTex;

        RunnerBoi.Actions.Instance.OnGameStateChange += HandleGameStateChange;
        StartCoroutine(UpdatePercantage());
    }

    private void FixedUpdate()
    {        
        if (posVelocity.magnitude < 0.01f) return;

        Vector2 newPos = oldPos + posVelocity;

        newPos.x = 1 < newPos.x ? 1 : (newPos.x < 0 ? 0 : newPos.x); //CLAMP(0, 1)
        newPos.y = 1 < newPos.y ? 1 : (newPos.y < 0 ? 0 : newPos.y); //CLAMP(0, 1)

        if ((newPos - oldPos).magnitude < 0.01f) return;
        oldPos = newPos;

        Paint(newPos);
    }

    private void HandlePlayerInput(Vector2 dragVel)
    {
        posVelocity = (dragVel*2)/Screen.width; //Sensitivy fix
    }

    private void Paint(Vector2 paintCoord)
    {
        Shader.SetGlobalVector("_PaintCoord", paintCoord);

        RenderTexture tempRenTex = RenderTexture.GetTemporary(texDimensions.x, texDimensions.y, texDepth, renTexFormat, renTexRW);
        tempRenTex.filterMode = texFilterMode;

        Graphics.Blit(wallRenTex, tempRenTex, targetMaterial);
        Graphics.Blit(tempRenTex, wallRenTex);

        RenderTexture.ReleaseTemporary(tempRenTex);
    }
    
    IEnumerator UpdatePercantage()
    {
        while(true)
        {
            yield return new WaitForSeconds(0.1f);

            int redCount = 0;

            Graphics.Blit(wallRenTex, readRenTex);
            readTex.ReadPixels(readTexRect, 0, 0);
            Color32[] readTexData = readTex.GetPixels32();

            for (int i = 0; i < readTexData.Length; i++)
            {
                if (readTexData[i].b < 100) //To be red, blue value must be low or zero
                    redCount++;
            }

            if(paintPercentage != (redCount * 100)/ readTexData.Length) //If percentage not changed, don't update
            {
                paintPercentage = (redCount * 100) / readTexData.Length; //Total red count / Total pixel count
                Actions.Instance.OnPaintPercantageChange?.Invoke(paintPercentage);
            }
        }
    }

    private void HandleGameStateChange(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.Wallpaint:
                Actions.Instance.OnPlayerInput += HandlePlayerInput;
                Graphics.Blit(Texture2D.whiteTexture, wallRenTex);
                StartCoroutine(UpdatePercantage());
                break;
            default:
                Actions.Instance.OnPlayerInput -= HandlePlayerInput;
                Graphics.Blit(Texture2D.whiteTexture, wallRenTex);
                StopAllCoroutines();
                break;
        }
    }
}
