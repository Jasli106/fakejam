using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


public class InventoryManager : MonoBehaviour, DisplayItemHolder
{
    public static InventoryManager instance = null;

    public static Item itemSelected = new Item();


    public static bool inventoryOpen = false;

    public GameObject expandedInventory;

    public GameObject machineUI;
    public InventoryUIController machineIn;
    public InventoryUIController machineOut;



    [SerializeField] ItemDisplayer pickedUpItemDisplayer;

    public Item itemPickedUp = new Item();

    public Canvas canvas;

    public List<InventorySlot> hotbarSlots;
    private int selectedSlotIndex = 0;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }
    private void Start()
    {
        SelectSlot(0);
    }

    // Render item selected image at mouse point
    private void Update()
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvas.transform as RectTransform,
                Input.mousePosition,
                canvas.worldCamera,
                out Vector2 localPoint);

        pickedUpItemDisplayer.gameObject.GetComponent<RectTransform>().localPosition = localPoint;


        for(int i=0; i<9; ++i)
        {
            if(Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                SelectSlot(i);
            }
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (inventoryOpen)
            {
                CloseInventory();
            }
            else
            {
                OpenInventory();
            }
        }
        HandleScrollInput();
    }
    void HandleScrollInput()
    {
        float scrollDelta = Input.mouseScrollDelta.y;
        if (scrollDelta == 0) return;
        int newIndex = selectedSlotIndex - (int)Mathf.Sign(scrollDelta);
        newIndex += hotbarSlots.Count();
        newIndex %= hotbarSlots.Count();
        SelectSlot(newIndex);
    }


    // Hotbar
    void SelectSlot(int idx)
    {
        DeselectSlot(selectedSlotIndex);
        itemSelected = hotbarSlots[idx].item;
        hotbarSlots[idx].selected = true;
        selectedSlotIndex = idx;

        // Highlight
        Image img = hotbarSlots[idx].GetComponent<Image>();
        img.color = img.color = img.color = new Color(0.8f, 0.8f, 0.8f);
    }

    void DeselectSlot(int idx)
    {
        hotbarSlots[idx].selected = false;
        Image img = hotbarSlots[idx].GetComponent<Image>();
        img.color = img.color = img.color = new Color(1, 1, 1);
    }

    public void OpenInventory() 
    {
        inventoryOpen = true;
        pickedUpItemDisplayer.gameObject.SetActive(true);
        expandedInventory.SetActive(true);
    }

    public void OpenMachineInventory(Inventory input, Inventory output)
    {
        OpenInventory();
        machineUI.SetActive(true);
        machineIn.RepresentInventory(input);
        machineOut.RepresentInventory(output);
    }

    public void CloseInventory()
    {
        inventoryOpen = false;
        pickedUpItemDisplayer.gameObject.SetActive(false);
        expandedInventory.SetActive(false);
        machineUI.SetActive(false);
    }

    public Item DisplayItem()
    {
        return itemPickedUp;
    }
}
