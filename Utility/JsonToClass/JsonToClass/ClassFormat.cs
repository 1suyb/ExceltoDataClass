using System;
using System.Collections.Generic;
using System.Text;

namespace JsonClassMaker
{
    class ClassFormat
    {
        // {0} : 데이터 타입 이름 / 번호 목록
        // {1} : class 목록
        public static string classFileFormat =
@"using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DataType
{{
{0}
}}


[System.Serializable]
public class Table
{{
    public int ID;
}}
{1}

";
        // {0} : type 이름
        // {1} : type 번호
        public static string enumFormat =
@"  {0} = {1},";
        #region Class
        // {0} : DataTable이름

        // {1} : 맴버 변수
        // {2} : 매개 변수
        // {3} : 변수 초기화 부분
        // {4} : 문자열 클래스 맴버 변수
        // {5} : 생성자 초기화 매개변수
        public static string classFormat =
@"[System.Serializable]
public class {0} : Table
{{
{1}
    public {0}({2})
    {{
{3}
    }}
}}
[System.Serializable]
public class String{0}
{{
{4}

    public static List<{0}> Convert(String{0}[] table)
    {{
        List<{0}> t = new List<{0}>();
        foreach (String{0} item in table)
        {{
            t.Add(new {0}({5}));
        }}
        return t;
    }}
}}
";
        // {0} : 변수
        public static string fullValueFormat =
@"  public {0};";
        // {0} : 변수 형식
        // {1} : 변수 이름
        public static string valueFormat =
@"{0} {1}";
        public static string setValueFormat =
@"      this.{0} = {1};";
        #endregion

        // {0} : case 목록
        // {1} : 역직렬화 목록
        public static string utilFileFormat =
@"using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class JsonHelper
{{
    public static T[] Deserialize<T>(string datatype)
    {{
        string path = Application.dataPath + ""/DataTables/"" + datatype + "".json"";
        string jsontext = System.IO.File.ReadAllText(path);
        jsontext = ""{{ \n \""Items\"" : "" + jsontext + ""}}"";

        Wrapper<T> w = JsonUtility.FromJson<Wrapper<T>>(jsontext);
        return w.Items;
    }}

    [System.Serializable]
    private struct Wrapper<T>
    {{
        public T[] Items;
    }}
}}

public static class JsonUtil
{{
    private static string ConvertType(DataType datatype)
    {{
        string filename;
        switch(datatype){{
{0}
            default :
                filename = """";
                break;
        }}
        return filename;
    }}
    {1}
}}
";
        #region JsonUtil
        // {0} : type 이름 (대문자)
        // {1} : type에 따른 파일이름 (DataTable 이름)
        public static string jsonUtilCaseFormat =
@"          case DataType.{0} : 
                filename = ""{1}"";
                break;";
        // {0} : DataTable 이름
        public static string jsonUtilDeserializeFormat =
@"
    public static List<{0}> {0}Deserialize(DataType datatype)
    {{
        String{0}[] t = JsonHelper.Deserialize<String{0}>(ConvertType(datatype));
        List<{0}> l = String{0}.Convert(t);
        return l;
    }}
";
        #endregion
    }

}
