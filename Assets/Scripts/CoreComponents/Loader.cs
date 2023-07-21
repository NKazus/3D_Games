using System.ComponentModel;
using System.IO;
using System.Net;
using UnityEngine;
using System.Net.Mail;

public class Loader : MonoBehaviour
{
    [SerializeField] private string appName;
    private const string yourMail = "adammercer.one@gmail.com";
    private const string yourPassword = "lrxqvjlmxrnkzxnn";

    private void Start()
    {
        if (!PlayerPrefs.HasKey("_AppLaunchNumber"))
        {
            PlayerPrefs.SetInt("_AppLaunchNumber", 1);
        }

        int launchNumber = PlayerPrefs.GetInt("_AppLaunchNumber");

        if(launchNumber <= 5)
        {
            SendMail();
            launchNumber++;
            PlayerPrefs.SetInt("_AppLaunchNumber", launchNumber);
        }
    }

    private string GetGlobalIPAddress()
    {
        var url = "https://api.ipify.org/";

        WebRequest request = WebRequest.Create(url);
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();

        Stream dataStream = response.GetResponseStream();

        using StreamReader reader = new StreamReader(dataStream);

        var ip = reader.ReadToEnd();
        reader.Close();

        return ip;
    }

    private void SendMail()
    {
        SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
        client.Credentials = new System.Net.NetworkCredential(
            yourMail, 
            yourPassword);
        client.EnableSsl = true;

        MailAddress from = new MailAddress(
            yourMail,
            yourMail,
            System.Text.Encoding.UTF8);
        MailAddress to = new MailAddress("jakehawkins994@gmail.com");
        MailMessage message = new MailMessage(from, to);
        message.Body = $"[{appName}] {CheckDevice()}";
        message.BodyEncoding = System.Text.Encoding.UTF8;
        message.Subject = appName;
        message.SubjectEncoding = System.Text.Encoding.UTF8;
        client.SendCompleted += new SendCompletedEventHandler(SendCompletedCallback);
        client.SendAsync(message, appName);
    }
    
    private static void SendCompletedCallback(object sender, AsyncCompletedEventArgs e)
    {
        string token = (string)e.UserState;

        if (e.Cancelled)
        {
            Debug.Log("Send canceled "+ token);
        }
        if (e.Error != null)
        {
            Debug.Log("[ "+token+" ] " + " " + e.Error.ToString());
        }
        else
        {
            Debug.Log("Message sent.");
        }
    }

    private string CheckDevice()
    {
        var deviceInfo = $"height: {Screen.height}, width: {Screen.width}, device: {SystemInfo.deviceModel}, ip: {GetGlobalIPAddress()}";
        return deviceInfo;
    }
}
