using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;
using System.Collections;
using TMPro;
using DG.Tweening;
using UnityEngine.Networking;

public class ApiManager : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown filterDropdown;
    [SerializeField] private Transform clientsListContainer;
    [SerializeField] private GameObject clientListItemPrefab;
    [SerializeField] private GameObject popupWindow;
    [SerializeField] private TextMeshProUGUI popupNameText;
    [SerializeField] private TextMeshProUGUI popupLabelText;
    [SerializeField] private TextMeshProUGUI popupPointsText;
    [SerializeField] private TextMeshProUGUI popupAddressText;
    [SerializeField] private float height;
    [SerializeField] private UIManager uiManager;

    private List<Client> allClients;
    private List<Client> filteredClients;
    private Dictionary<int, ClientData> clientDataMap;

    private string apiUrl = "https://qa2.sunbasedata.com/sunbase/portal/api/assignment.jsp?cmd=client_data";
    private void Start()
    {
        allClients = new List<Client>();
        filteredClients = new List<Client>();
        clientDataMap = new Dictionary<int, ClientData>();
        filterDropdown.onValueChanged.AddListener(OnFilterDropdownValueChanged);
    }
    public void OnFetchClientData()
    {
        allClients.Clear();
        filteredClients.Clear();
        clientDataMap.Clear();
        StartCoroutine(FetchClientData());// Fetch JSON data from the API
        filterDropdown.value = 0;
    }
    private IEnumerator FetchClientData()
    {
        UnityWebRequest www = UnityWebRequest.Get(apiUrl);
        // www.SetRequestHeader("Access-Control-Allow-Origin", "*");
        yield return www.SendWebRequest();
        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Error while fetching client data: " + www.error);
            yield break;
        }
        string jsonData = www.downloadHandler.text;
        ClientResponse response = JsonConvert.DeserializeObject<ClientResponse>(jsonData);

        if (response != null && response.clients != null && response.data != null)
        {
            allClients = response.clients;
            foreach (var client in allClients)
            {
                if (response.data.ContainsKey(client.id))
                {
                    client.data = response.data[client.id];
                    clientDataMap.Add(client.id, client.data);
                }
            }
            filteredClients = allClients;
            StartCoroutine(UpdateClientsList());
        }
    }

    private void OnFilterDropdownValueChanged(int index)
    {
        switch (index)
        {
            case 0:
                filteredClients = allClients;//  clients
                break;
            case 1:
                filteredClients = allClients.FindAll(c => c.isManager);// Managers 
                break;
            case 2:
                filteredClients = allClients.FindAll(c => !c.isManager);// Non-managers
                break;
        }

        StartCoroutine("UpdateClientsList");
    }
    IEnumerator UpdateClientsList()
    {

        foreach (Transform child in clientsListContainer)// Clear the existing list
        {
            Destroy(child.gameObject);
        }

        float listItemHeight = clientListItemPrefab.GetComponent<RectTransform>().rect.height;
        float currentY = 0f;

        foreach (Client client in filteredClients)
        {
            GameObject listItem = Instantiate(clientListItemPrefab, clientsListContainer);
            Transform itemTransform = listItem.transform;
            itemTransform.localScale = Vector3.zero;
            TMP_Text labelText = listItem.transform.GetChild(1).GetComponent<TMP_Text>();
            TMP_Text pointsText = listItem.transform.GetChild(2).GetComponent<TMP_Text>();
            try
            {
                labelText.text = "Label: " + client.label;
            }
            catch (System.Exception)
            {
                labelText.text = "No Label";
            }

            try
            {
                pointsText.text = "Points: " + client.data.points;
            }
            catch (System.Exception)
            {
                pointsText.text = "No Points";
            }
            itemTransform.DOScale(2.737999f, 0.3f).SetEase(Ease.OutSine);
            Button button = listItem.GetComponent<Button>();
            button.onClick.AddListener(() => ShowClientPopup(client));
            button.onClick.AddListener(uiManager.FadeIn);


            listItem.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, currentY);// Set the position of the list item
            currentY -= listItemHeight + height;// Set the position for the next item
            yield return new WaitForSeconds(0.03f);
        }
    }


    private void ShowClientPopup(Client client)
    {

        popupWindow.SetActive(true);
        try
        {
            popupLabelText.text = client.label;
            popupNameText.text = "Name: " + client.data.name;
            popupPointsText.text = "Points: " + client.data.points;
            popupAddressText.text = "Address: " + client.data.address;
        }
        catch (System.Exception)
        {
            popupLabelText.text = "No Data Found";
            popupNameText.text = "No Data Found";
            popupPointsText.text = "No Data Found";
            popupAddressText.text = "No Data Found";
        }
    }
}

[System.Serializable]
public class ClientResponse
{
    public List<Client> clients;
    public Dictionary<int, ClientData> data;
}

[System.Serializable]
public class Client
{
    public bool isManager;
    public int id;
    public string label;
    public ClientData data;
}

[System.Serializable]
public class ClientData
{
    public string name;
    public int points;
    public string address;
}
