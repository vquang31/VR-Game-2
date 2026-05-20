using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MagicBook : NewMonoBehaviour
{
    [Header("Movement General")]
    [SerializeField] private GameObject positionMagicBook;
    [SerializeField] private GameObject mainCamera;
    [SerializeField] private float damping = 2.5f;
    private void Update()
    {
        // Target position
        Vector3 targetPos = positionMagicBook.transform.position;

        // Smooth position
        transform.position = Vector3.Lerp(
            transform.position,
            targetPos,
            Time.deltaTime * damping
        );

        // Target rotation (look at camera)
        Vector3 direction = mainCamera.transform.position - transform.position;
        Quaternion targetRot = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180f, 0);

        // Smooth rotation
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRot,
            Time.deltaTime * damping
        );
    }


    [Header("======================= UI Page =======================")]
    [SerializeField]
    private int indexCurrentPage = 0; // Trang hiện tại đang hiển thị = index của list magicDataList

    /// <summary>
    /// InfoCanvasGO have GO:
    /// - ImageMagic: hiển thị icon phép thuật
    /// - NameMagic_Text: hiển thị tên phép thuật
    /// - Duration_Text: hiển thị thời gian tồn tại của phép thuật
    /// - Cooldown_Text: hiển thị thời gian hồi chiêu của phép thuật
    /// 
    ///  descriptionCanvasGO have GO:
    /// - Description_Text: hiển thị mô tả của phép thuật ở trang trước
    /// 
    /// </summary>

    [SerializeField] private GameObject currentInfoCanvasGO;
    [SerializeField] private GameObject currentDescriptionCanvasGO;

    [SerializeField] private GameObject nextInfoCanvasGO;
    [SerializeField] private GameObject nextDescriptionCanvasGO;

    [SerializeField] private GameObject previousInfoCanvasGO;
    [SerializeField] private GameObject previousDescriptionCanvasGO;
    [SerializeField] private Animator animator;
    private string triggerNextPage = "NextPage";
    private string triggerPreviousPage = "PreviousPage";
    protected override void LoadComponents()
    {
        base.LoadComponents();
        mainCamera = Camera.main.gameObject;
        positionMagicBook = GameObject.Find("PositionBook");
        animator = GetComponent<Animator>();

    }


    protected override void Start()
    {
        base.Start();
        InitUIBook();

    }
    private void InitUIBook()
    {
        indexCurrentPage = 0;
        LoadDataCurrentPage();
    }

    public bool CanChangePage = true;

    [ContextMenu("NextPage")]
    public void NextPage()
    {
        StartCoroutine(NextPageRountine());
    }

    IEnumerator NextPageRountine()
    {
        yield return null;
        if (indexCurrentPage == MagicPool.Instance.magicDataList.Count - 1)
        {
            yield break;
        }
        if(!CanChangePage)
        {
            yield break;
        }
        CanChangePage = false;
        indexCurrentPage++;

        animator.SetTrigger(triggerNextPage);
    }




    [ContextMenu("PreviousPage")]
    public void PreviousPage()
    {
        StartCoroutine(PreviousPageRountine());
    }

    IEnumerator PreviousPageRountine()
    {
        yield return null;
        if (indexCurrentPage == 0)
        {
            yield break;
        }
        if(!CanChangePage)
        {
            yield break;
        }
        CanChangePage = false;
        indexCurrentPage--;
        animator.SetTrigger(triggerPreviousPage);
    }


    /// <summary>
    /// Run in animation event of animation NextPage and PreviousPage, at the end of animation, allow to change page again
    /// </summary>
    public void ChangePageValid()
    {
        CanChangePage = true;
    }




    /// <summary>
    ///  run in animation event of animation NextPage and PreviousPage  
    /// </summary>
    private void LoadDataCurrentPage()
    {
        if(MagicPool.Instance.magicDataList.Count == 0)
        {
            return;
        }
        EditInfoCanvas(currentInfoCanvasGO, MagicPool.Instance.magicDataList[indexCurrentPage]);
        EditDescriptionCanvas(currentDescriptionCanvasGO, MagicPool.Instance.magicDataList[indexCurrentPage]);
        LoadDataNextPage();
        LoadDataPreviousPage();
    }


    private void LoadDataPreviousPage()
    {
        if (indexCurrentPage == 0)
        {
            return;
        }
        
        EditInfoCanvas(previousInfoCanvasGO, MagicPool.Instance.magicDataList[indexCurrentPage - 1]);
        EditDescriptionCanvas(previousDescriptionCanvasGO, MagicPool.Instance.magicDataList[indexCurrentPage - 1]);
    }

    private void LoadDataNextPage()
    {
        if (indexCurrentPage == MagicPool.Instance.magicDataList.Count - 1)
        {
            return;
        }
        EditInfoCanvas(nextInfoCanvasGO, MagicPool.Instance.magicDataList[indexCurrentPage + 1]);
        EditDescriptionCanvas(nextDescriptionCanvasGO, MagicPool.Instance.magicDataList[indexCurrentPage + 1]);
    }


    /// InfoCanvasGO have GO:
    /// - ImageMagic: hiển thị icon phép thuật
    /// - NameMagic_Text: hiển thị tên phép thuật
    /// - Duration_Text: hiển thị thời gian tồn tại của phép thuật
    /// - Cooldown_Text: hiển thị thời gian hồi chiêu của phép thuật
    private void EditInfoCanvas(GameObject infoCanvasGO, MagicData magicData)
    {
        GameObject imageMagicGO = infoCanvasGO.transform.Find("ImageMagic").gameObject;
        GameObject nameMagicTextGO = infoCanvasGO.transform.Find("NameMagic_Text").gameObject;
        GameObject durationTextGO = infoCanvasGO.transform.Find("Duration_Text").gameObject;
        GameObject cooldownTextGO = infoCanvasGO.transform.Find("Cooldown_Text").gameObject;

        Image imageMagic = imageMagicGO.GetComponent<Image>();
        TextMeshProUGUI nameMagicText = nameMagicTextGO.GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI durationText = durationTextGO.GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI cooldownText = cooldownTextGO.GetComponent<TextMeshProUGUI>();


        imageMagic.sprite = magicData.magicIcon;
        nameMagicText.text = magicData.magicName;
        durationText.text = $"Duration: {magicData.duration}s";
        cooldownText.text = $"Cooldown: {magicData.cooldown}s";

    }
    ///  descriptionCanvasGO have GO:
    /// - Description_Text: hiển thị mô tả của phép thuật ở trang trước
    /// 
    private void EditDescriptionCanvas(GameObject descriptionCanvasGO, MagicData magicData)
    {
        GameObject descriptionTextGO = descriptionCanvasGO.transform.Find("Description_Text").gameObject;
        TextMeshProUGUI descriptionText = descriptionTextGO.GetComponent<TextMeshProUGUI>();
        descriptionText.text = magicData.description;
        GameObject imageSummonGO = descriptionCanvasGO.transform.Find("ImageSummon").gameObject;
        Image imageSummon = imageSummonGO.GetComponent<Image>();
        if (magicData is SummonMagicData summonMagicData)
        {
            imageSummon.sprite = summonMagicData.summonIcon;
            imageSummon.color = new Color(1, 1, 1, 1);
        }
        else
        {
            imageSummon.color = new Color(1, 1, 1, 0);
        }
    }

}