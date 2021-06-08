using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Excel;
using System.Data;
using System.Text;

public class TestExcel : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Excel2Txt();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Excel2Txt()
    {
        string excelPath = "C:/Users/zhanpeng.xie/Desktop/1.xlsx";//EditorUtility.OpenFilePanel("打开 excel需求表 ", Application.dataPath, "*");
        if (string.IsNullOrEmpty(excelPath))
        {
            Debug.LogError("excel 路径不存在");
            return;
        }
        Debug.Log(excelPath);

        FileStream stream = File.Open(excelPath, FileMode.Open, FileAccess.Read);
        Debug.Log(stream == null);

        IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
        Debug.Log(excelReader.IsValid + " + " + excelReader.ExceptionMessage);
        DataSet result = excelReader.AsDataSet();
        Debug.Log(result == null);
        int columns = result.Tables[0].Columns.Count;
        int rows = result.Tables[0].Rows.Count;
        Debug.LogError(columns + "  " + rows);
        string txt_path = "D:/UnityProject/AR_Book_Project/txt"; // EditorUtility.OpenFilePanel(" txt保存目录 ", AppDomain.CurrentDomain.BaseDirectory, "*");
        if (string.IsNullOrEmpty(txt_path))
        {
            Debug.LogError("txt 路径不存在");
            return;
        }
        for (int i = 1; i < rows; i++)
        {
            //ModelInfo theme = new ModelInfo();
            //theme.name = result.Tables[0].Rows[i][0].ToString();
            //theme.path = result.Tables[0].Rows[i][1].ToString();
            //theme.type = result.Tables[0].Rows[i][2].ToString();
            //theme.thumbnail = result.Tables[0].Rows[i][3].ToString();
            //theme.load = result.Tables[0].Rows[i][4].ToString();
            //m_allModelsInfo.Add(theme);
            string txtPath = txt_path + "/" + (result.Tables[0].Rows[i][1]).ToString() + ".txt";
            Debug.Log(txtPath + "   txt文件目录");
            using (FileStream _stream = File.Open(txt_path, FileMode.OpenOrCreate))
            {
                string _name = "name:" + result.Tables[0].Rows[i][0].ToString() + "\n";
                string _content = "content:" + result.Tables[0].Rows[i][3].ToString();
                string str = _name + _content;
                byte[] bytes = Encoding.UTF8.GetBytes(str);
                _stream.Write(bytes, 0, bytes.Length);
                _stream.Flush();
                _stream.Close();
            }
        }
        stream.Dispose();
    }
}
