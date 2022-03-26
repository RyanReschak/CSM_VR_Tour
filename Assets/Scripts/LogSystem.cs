using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Text;
using System.Linq;
using TMPro;

//Handles Changes and Logs every Action that might happen in the Program
public class LogSystem : MonoBehaviour
{
    public string file_name;
    public Dropdown prev_users;

    private FileReadWrite frw;

    public class FileReadWrite
    {
        public string file_name;

        public void setFileLocation(string file_name)
        {
            this.file_name = Path.Combine(Application.persistentDataPath, file_name);
        }

        public void WriteLines(List<string> lines)
        {
            //This appends to the file 
            StreamWriter sw = new StreamWriter(file_name, true, Encoding.ASCII);

            foreach (string line in lines)
            {
                sw.WriteLine(line);
            }

            sw.Close();
        }

        public List<string> ReadFile(string startString, string stopString, int instance)
        {
            string line;
            List<string> returnLines = new List<string>();

            if(!File.Exists(file_name))
            {
                return returnLines;
            }

            StreamReader sr = new StreamReader(file_name);

            line = sr.ReadLine();

            int num_instances = 0;
            bool start_saving = false;

            while (line != null)
            {
                if (string.Equals(startString, line))
                    if (instance == num_instances)
                        start_saving = true;
                    else
                        num_instances++;
                else if (start_saving && !string.Equals(line, stopString))
                    returnLines.Add(line);
                else if (start_saving && string.Equals(line, stopString))
                    break;

                line = sr.ReadLine();
            }

            sr.Close();

            return returnLines;
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        frw = new FileReadWrite();
        frw.setFileLocation(file_name);

        List<string> dropOptions = new List<string>();

        prev_users.ClearOptions();
        dropOptions.Add(" ");
        int instance = 0;
        List<string> user = frw.ReadFile("<user>", "</user>", 0);
        while (user.Any()) //If empty break;
        {
            dropOptions.Add(user.ElementAt(0)); //username
            instance++;
            user = frw.ReadFile("<user>", "</user>", instance);
        }

        prev_users.AddOptions(dropOptions);

    }

    public void UpdateUserInfo(GameObject LoginMenu)
    {
        List<string> lines = new List<string>();
        lines.Add("<user>");

        TMP_InputField[] tmps = LoginMenu.GetComponentsInChildren<TMP_InputField>();

        foreach (TMP_InputField child in tmps)
        {
            lines.Add(child.text);
        }
        lines.Add("</user>");

        frw.WriteLines(lines);
    }

    public void UpdateUserInterests(GameObject panel)
    {
        List<string> lines = new List<string>();

        Toggle[] toggles = panel.GetComponentsInChildren<Toggle>();

        lines.Add("<inter>");

        foreach (Toggle child in toggles)
        {
            if (child.isOn)
            {
                foreach (Text t in child.GetComponentsInChildren<Text>())
                {
                    lines.Add(t.text);
                }

            }
        }

        lines.Add("</inter>");

        frw.WriteLines(lines);
    }

    public void UpdateUserID(string userType)
    {
        List<string> lines = new List<string>() { "<type>" + userType + "</type>" };
        frw.WriteLines(lines);
    }

    public void UpdateStudentID(GameObject studentPanel)
    {
        List<string> lines = new List<string>();
        Toggle[] toggles = studentPanel.GetComponentsInChildren<Toggle>();

        lines.Add("<stype>");

        foreach (Toggle child in toggles)
        {
            if (child.isOn)
            {
                foreach (Text t in child.GetComponentsInChildren<Text>())
                {
                    lines.Add(t.text);
                }

            }
        }

        Dropdown[] gradYear = studentPanel.GetComponentsInChildren<Dropdown>();

        foreach (Dropdown child in gradYear)
        {
            
            lines.Add(child.captionText.text);
                
        }

        lines.Add("</stype>");
        frw.WriteLines(lines);
    }

    public void UpdateLog(string subject)
    {
        List<string> lines = new List<string>() { "<log>" + subject + "</log>" };
        frw.WriteLines(lines);
    }
    //(SFTP) -> Mines [Maura Koch] -> Slait
    //Recommmend that it goes straight to Slait
    //Grad Student or Undergrad
    //Expected Grad Year
    //DOB

    //Interaction Data with certain places
    //town interactions like coupons
}