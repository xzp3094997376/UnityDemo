using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using Excel;
using System.Data;
using System;
using System.Text;
using ICSharpCode.SharpZipLib;//需要这个dll
using System.Text.RegularExpressions;

public class ExcelTool 
{
    [MenuItem("ExcelTool/Excel")]
    static void Excel2Txt()
    {

        string excelPath = EditorUtility.OpenFilePanel("打开 excel需求表 ", Application.dataPath, "*");
        if (string.IsNullOrEmpty(excelPath))
        {
            Debug.LogError("excel 路径不存在");
            return;
        }
        Debug.Log(excelPath);

        using (FileStream stream = File.Open(excelPath, FileMode.Open, FileAccess.ReadWrite))
        {
            Debug.Log(stream == null);

            IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);//exel需要放在Assets目录下
            Debug.Log(excelReader.IsValid + "   " + excelReader.ExceptionMessage);
            DataSet result = excelReader.AsDataSet();
            Debug.Log(result == null);
            int columns = result.Tables[0].Columns.Count;
            int rows = result.Tables[0].Rows.Count;
            Debug.LogError(columns + "  " + rows);
            string txt_path = EditorUtility.SaveFolderPanel(" txt保存目录 ", Application.dataPath, " 保存目录");
            Debug.Log(txt_path);
            if (string.IsNullOrEmpty(txt_path))
            {
                Debug.LogError("txt 路径不存在");
                return;
            }
            for (int i = 0; i < rows; i++)
            {
                string str = string.Empty;
                for (int j = 0; j < columns; j++)
                {
                    str += result.Tables[0].Rows[i][j] + $"-{j}   ";
                }
                Debug.Log(i + " +   " + str);
            }
            string txtPath = txt_path + "/" + "花艺.txt";//6，4
            txtPath = txtPath.Replace("/", "\\").Replace("\n", "");//路径替换
            using (FileStream _stream = new FileStream(txtPath, FileMode.OpenOrCreate, FileAccess.Write))
            {
                //return;
                for (int i = 1; i < rows; i++)
                {
                //ModelInfo theme = new ModelInfo();
                //theme.name = result.Tables[0].Rows[i][0].ToString();
                //theme.path = result.Tables[0].Rows[i][1].ToString();
                //theme.type = result.Tables[0].Rows[i][2].ToString();
                //theme.thumbnail = result.Tables[0].Rows[i][3].ToString();
                //theme.load = result.Tables[0].Rows[i][4].ToString();
                //m_allModelsInfo.Add(theme);




                //string txtPath = txt_path + "/" + (result.Tables[0].Rows[i][6]).ToString() + ".txt";//6，4
                //txtPath = txtPath.Replace("/", "\\").Replace("\n", "");//路径替换

                //Debug.Log(txtPath + "   txt文件目录");
               
                
                    //string _name = "name:" + result.Tables[0].Rows[i][7].ToString() + "\n";//7
                    //string _content = "content:" + result.Tables[0].Rows[i][8].ToString();//8,6
                    //_content = Regex.Replace(_content, @"\s", "");
                    //string str = (_name + _content).Trim();

                    string str = result.Tables[0].Rows[i][1].ToString() + "\n";
                    //str = str.Trim();
                    //Debug.Log(" StreamWriter " + str);
                    StreamWriter sw = new StreamWriter(_stream,Encoding.UTF8);
                    sw.Write(str);
                    sw.Flush();
                   
                }
                _stream.Flush();
            }
            //stream.Flush();
            // stream.Close(); 
        }
    }

    static Dictionary<string,string> imgNamesDic = new Dictionary<string, string>();
    [MenuItem("ExcelTool/缩略图修改")]
    static void RenameImg()
    {
        imgNamesDic.Clear();

        string excelPath = EditorUtility.OpenFilePanel("打开 excel需求表 ", Application.dataPath, "*");
        if (string.IsNullOrEmpty(excelPath))
        {
            Debug.LogError("excel 路径不存在");
            return;
        }
        Debug.Log(excelPath);
        using (FileStream stream = File.Open(excelPath, FileMode.Open, FileAccess.ReadWrite))
        {
            Debug.Log(stream == null);

            IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);//exel需要放在Assets目录下
            Debug.Log(excelReader.IsValid + "   " + excelReader.ExceptionMessage);
            DataSet result = excelReader.AsDataSet();
            Debug.Log(result == null);
            int columns = result.Tables[0].Columns.Count;
            int rows = result.Tables[0].Rows.Count;
            Debug.LogError(columns + "  " + rows);

            for (int i = 1; i < rows; i++)
            {
                string zh_cn= result.Tables[0].Rows[i][5].ToString();
                string en = result.Tables[0].Rows[i][6].ToString();
                string[] zhStr = zh_cn.Split('-');
                zh_cn = zhStr.Length > 1 ? zhStr[1] : zhStr[0];
                zh_cn = Regex_ZhCN(zh_cn);
                zh_cn = zh_cn.Contains("邮轮") ? zh_cn.Replace("邮轮", "") : zh_cn;
                imgNamesDic.Add(zh_cn, en);
                Debug.Log($"zh:={zh_cn},   en={en}");
            }
              
        }      
        if (imgNamesDic.Count == 0)
        {
            Debug.LogError("先从excel表中获取名字");
            return; ;
        }



        string dirPath = EditorUtility.OpenFolderPanel("打开缩略图目录 ", Application.dataPath, "缩略图目录");
        if (string.IsNullOrEmpty(dirPath))
        {
            Debug.LogError("excel 路径不存在");
            return;
        }
        Debug.Log(dirPath);
        string[] imgPaths= Directory.GetFiles(dirPath, "*.png");
        for (int i = 0; i < imgPaths.Length; i++)
        {
            string item = imgPaths[i];
            string oldfileName = Path.GetFileNameWithoutExtension(item);
            Debug.Log("oldfileName  " + oldfileName);
            if (!imgNamesDic.ContainsKey(oldfileName))
            {
                Debug.Log("没有找到文件");
                continue;
            }
            string newFileName =imgNamesDic[oldfileName];
            string newFileFullName = Path.Combine(dirPath, newFileName);            
            item= item.Replace('\\', '/');
            newFileFullName= newFileFullName.Replace('\\', '/')+".png";
            Debug.Log($"旧文件名字：{item},   新文件名字：  {newFileFullName}");
            File.Move(item, newFileFullName);
        }
    }
    //[MenuItem("ExcelTool/正则表达式替换中文")]
    static string Regex_ZhCN(string _inputStr)
    {
        string[] RegexNumber = {
                                    @"[\u4e00-\u9fa5]+"
                                   };

        //string _inputStr = "邮轮公司logo-星梦邮轮DREAMCRUISES";
        //_inputStr = _inputStr.Split('-')[1];

        for (int j = 0; j < RegexNumber.Length; j++)
        {
            if (Regex.IsMatch(_inputStr, RegexNumber[j], RegexOptions.IgnoreCase))
            {
                Match match = Regex.Match(_inputStr, RegexNumber[j], RegexOptions.IgnoreCase);
                //string Name = name.Replace(match.Value, "");
                _inputStr = match.Value;
                Debug.Log(_inputStr);
            }
        }
        return _inputStr;
    }

    static Dictionary<string, string> mp3NamesDic = new Dictionary<string, string>();
    [MenuItem("ExcelTool/语音重命名")]
    static void RenameMP3()
    {
        mp3NamesDic.Clear();

        string excelPath = EditorUtility.OpenFilePanel("打开 excel需求表 ", Application.dataPath, "*");
        if (string.IsNullOrEmpty(excelPath))
        {
            Debug.LogError("excel 路径不存在");
            return;
        }
        Debug.Log(excelPath);
        using (FileStream stream = File.Open(excelPath, FileMode.Open, FileAccess.ReadWrite))
        {
            Debug.Log(stream == null);

            IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);//exel需要放在Assets目录下
            Debug.Log(excelReader.IsValid + "   " + excelReader.ExceptionMessage);
            DataSet result = excelReader.AsDataSet();
            Debug.Log(result == null);
            int columns = result.Tables[0].Columns.Count;
            int rows = result.Tables[0].Rows.Count;
            Debug.LogError(columns + "  " + rows);

            for (int i = 1; i < rows; i++)
            {
                string excelValue = result.Tables[0].Rows[i][6].ToString();
                string excelKey = result.Tables[0].Rows[i][6].ToString();                
                if (excelKey.Split('_').Length<3)
                {
                    continue;
                }
                string _key = excelKey.Split('_')[3];
                mp3NamesDic.Add(_key, excelValue);
                Debug.Log($"key:={_key},   value={excelValue}");
            }

        }
        
        if (mp3NamesDic.Count == 0)
        {
            Debug.LogError("先从excel表中获取名字");
            return; ;
        }



        string dirPath = EditorUtility.OpenFolderPanel("打开语音目录 ", Application.dataPath, "语音目录");
        if (string.IsNullOrEmpty(dirPath))
        {
            Debug.LogError("语音 路径不存在");
            return;
        }
        Debug.Log(dirPath);
        string[] mp3Path = Directory.GetFiles(dirPath, "*.mp3");
        for (int i = 0; i < mp3Path.Length; i++)
        {
            string item = mp3Path[i];
            string oldfileName = Path.GetFileNameWithoutExtension(item);
            Debug.Log("oldfileName  " + oldfileName);
            string _oldFileName = oldfileName.Split('_')[3];
            if (!mp3NamesDic.ContainsKey(_oldFileName))
            {
                Debug.Log("没有找到文件");
                continue;
            }
            string newFileName = mp3NamesDic[_oldFileName];
            string newFileFullName = Path.Combine(dirPath, newFileName);
            item = item.Replace('\\', '/');
            newFileFullName = newFileFullName.Replace('\\', '/') + ".mp3";
            Debug.Log($"旧文件名字：{item},   新文件名字：  {newFileFullName}");
            File.Move(item, newFileFullName);
        }
    }

    [MenuItem("ExcelTool/将文件从一个目录Copy到其他目录")]
    static void CopyFiles()
    {
        string dirPath = EditorUtility.OpenFolderPanel("打开缩略图目录 ", Application.dataPath, "缩略图目录");
        if (string.IsNullOrEmpty(dirPath))
        {
            Debug.LogError("缩略图 路径不存在");
            return;
        }

        string savePath = EditorUtility.OpenFolderPanel("另存为 ", Application.dataPath, "缩略图目录");
        if (string.IsNullOrEmpty(savePath))
        {
            Debug.LogError("缩略图 路径不存在");
            return;
        }
        CopyFilesToDestFolder(dirPath, "*.jpg",savePath);
    }

    static void CopyFilesToDestFolder(string path,string pattern,string savePath)
    {
        string[] jpg_paths= Directory.GetFiles(path,"*.jpg",SearchOption.AllDirectories);
        Debug.Log($"jpg数量={jpg_paths.Length}");
        if (jpg_paths.Length==0)
        {
            Debug.Log("该目录下没有jpg文件");
            return;
        }
        for (int i = 0; i < jpg_paths.Length; i++)
        {
            string FileName= Path.GetFileName(jpg_paths[i]);
            if (!FileName.Contains("_"))
            {
                continue;
            }
            string newFileFullName = Path.Combine(savePath, FileName);
            string item = jpg_paths[i];
            item = item.Replace('\\', '/');
            newFileFullName = newFileFullName.Replace('\\', '/');
            Debug.Log($"旧文件名字：{item},   新文件名字：  {newFileFullName}");
            File.Copy(item, newFileFullName,true);
        }

    }
    [MenuItem("ExcelTool/修改书中模型名字")]
    static void RenameBook()
    {
        string dirPath = EditorUtility.OpenFolderPanel("打开书目录 ", Application.dataPath, "Book目录");
        if (string.IsNullOrEmpty(dirPath))
        {
            Debug.LogError("目录 路径不存在");
            return;
        }
        string[] book_paths= Directory.GetDirectories(dirPath);
        for (int i = 0; i < book_paths.Length; i++)
        {
            //Debug.Log(book_paths[i]);
            string modelName = Path.GetFileName(book_paths[i]);
            Debug.Log(modelName);
            modelName = modelName.Split('_')[2];
            string newDirName= Path.Combine(dirPath,modelName);
            newDirName= newDirName.Replace("\\", "/");
            Debug.Log($"旧文件名字：{book_paths[i]},   新文件名字：  {newDirName}");
            Directory.Move(book_paths[i], newDirName);
        }
    }
    [MenuItem("ExcelTool/修改扫描图后缀")]
    static void RenameImgs()
    {
        string dirPath = EditorUtility.OpenFolderPanel("打开扫描图目录 ", Application.dataPath, "图片目录");
        if (string.IsNullOrEmpty(dirPath))
        {
            Debug.LogError("目录 路径不存在");
            return;
        }
        string[] book_paths = Directory.GetDirectories(dirPath);
        for (int i = 0; i < book_paths.Length; i++)
        {
            //Debug.Log(book_paths[i]);
            string oldName = Path.GetFileNameWithoutExtension(book_paths[i]);
            Debug.Log(oldName);           
            string newDirName = Path.Combine(dirPath, oldName);
            newDirName = newDirName.Replace("\\", "/")+".jpg";
            Debug.Log($"旧文件名字：{book_paths[i]},   新文件名字：  {newDirName}");
            File.Move(book_paths[i], newDirName);
        }
    }

    [MenuItem("ExcelTool/将图片从一个目录Copy到其他目录")]
    static void CopyImgs()
    {
        string dirPath = EditorUtility.OpenFolderPanel("打开缩略图目录 ", Application.dataPath, "缩略图目录");
        if (string.IsNullOrEmpty(dirPath))
        {
            Debug.LogError("缩略图 路径不存在");
            return;
        }

        string savePath = EditorUtility.OpenFolderPanel("另存为 ", Application.dataPath, "缩略图目录");
        if (string.IsNullOrEmpty(savePath))
        {
            Debug.LogError("缩略图 路径不存在");
            return;
        }
        CopySpecialtFilesToDestFolder(dirPath, "*.png", savePath);
    }

    static void CopySpecialtFilesToDestFolder(string path, string pattern, string savePath)
    {
        string txtPath = EditorUtility.OpenFilePanel("打开txt文件 ", Application.dataPath, "txt文件");
        Debug.Log(txtPath);
        txtPath = txtPath.Replace("/", "\\").Replace("\n", "");//路径替换
        string[] linesTxts= File.ReadAllLines(txtPath,Encoding.UTF8);

        List<string> strList = new List<string>();
        strList.AddRange(linesTxts);
        foreach (var item in strList)
        {
            Debug.Log(item);
        }

        string[] jpg_paths = Directory.GetFiles(path, "*.png", SearchOption.AllDirectories);
        Debug.Log($"png数量={jpg_paths.Length}");
        if (jpg_paths.Length == 0)
        {
            Debug.Log("该目录下没有png文件");
            return;
        }
        for (int i = 0; i < jpg_paths.Length; i++)
        {
            string FileName = Path.GetFileName(jpg_paths[i]);
            //Debug.Log(FileName);
            if (!strList.Contains(FileName.Split('.')[0]))
            {
                continue;
            }
            string newFileFullName = Path.Combine(savePath, FileName);
            string item = jpg_paths[i];
            item = item.Replace('\\', '/');
            newFileFullName = newFileFullName.Replace('\\', '/');
            Debug.Log($"旧文件名字：{item},   新文件名字：  {newFileFullName}");
            File.Copy(item, newFileFullName, true);
        }

    }
}
