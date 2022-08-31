using System;
using System.Collections.Generic;
using System.Text;

namespace ExceltoJson
{
    class StringFormat
    {
        // {0} : 데이터
        public static string JsonFormat =
@"
[
{0}
]
";
        // {0} : 데이터들
        public static string JsonDatasFormat = 
@"  {{
{0}
    }}";
        // {0} : 칼럼네임
        // {1} : 값
        public static string JsonDataFormat =
@"      ""{0}"" : ""{1}""";

        // {0} : table name
        // {1} : Datas
        public static string XmlFormat =
@"<?xml version=""1.0"" encoding=""utf-8"" ?>
<DataSchema>
    <table name = ""{0}"">
{1}
    </table>
</DataSchema>
";
        // {0} : 데이터 형식
        // {1} : 칼럼네임
        public static string XmlDataFormat =
@"      <{0} name = ""{1}""/>
";
    }
}
