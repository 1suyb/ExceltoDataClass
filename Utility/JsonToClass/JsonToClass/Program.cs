using System;
using System.IO;
using System.Xml;
using System.Windows.Forms;

namespace JsonClassMaker
{
    class Program
    {
        static string genClass;
        static ushort tableID;
        static string genUtil;

        static string dataTypeEnums;
        static string dataClass;

        static string caseFormat;
        static string deserializeFormat;
        static void Main(string[] args)
        {
            string DataSchemaPath = "DataSchema.xml";
            // 파일 dialog 출력

            XmlReaderSettings settings = new XmlReaderSettings()
            {
                IgnoreComments = true,
                IgnoreWhitespace = true
            };
            if (args.Length >= 1)
            {
                DataSchemaPath = args[0];
            }
            using (XmlReader r = XmlReader.Create(DataSchemaPath, settings))
            {
                r.MoveToContent();
                while (r.Read())
                {
                    if(r.Depth == 1&&r.NodeType == XmlNodeType.Element)
                    {
                        ParseSchema(r);
                    }
                }
                
            }
            // 폴더 선택창 출력
            genClass = string.Format(ClassFormat.classFileFormat, dataTypeEnums, dataClass);
            File.WriteAllText("GenClass.cs", genClass);
            genUtil = string.Format(ClassFormat.utilFileFormat, caseFormat, deserializeFormat);
            File.WriteAllText("GenUtil.cs", genUtil);




        }
        public static void ParseSchema(XmlReader r)
        {
            if (r.NodeType == XmlNodeType.EndElement)
                return;
            if(r.Name.ToLower() != "table")
            {
                Console.WriteLine("Invalid table node");
                return;
            }
            string tableName = r["name"];
            if (string.IsNullOrEmpty(tableName))
            {
                Console.WriteLine("Pack without name");
                return;
            }
            Tuple<string, string, string, string, string> tuple = ParseClass(r);
            dataTypeEnums += string.Format(ClassFormat.enumFormat, tableName, ++tableID);
            dataClass += string.Format(ClassFormat.classFormat, tableName, tuple.Item1, tuple.Item2, tuple.Item3, tuple.Item4, tuple.Item5);
            caseFormat += string.Format(ClassFormat.jsonUtilCaseFormat, tableName, tableName + "Table");
            deserializeFormat += string.Format(ClassFormat.jsonUtilDeserializeFormat, tableName);
        }

        // {1} : 맴버 변수
        // {2} : 매개 변수
        // {3} : 변수 초기화 부분
        // {4} : 문자열 클래스 맴버 변수
        // {5} : 생성자 초기화 매개변수
        public static Tuple<string,string,string,string,string> ParseClass(XmlReader r)
        {
            string memberValue = "";
            string newParameter = "";
            string setValue = "";
            string stringClassMemberValue = "";
            string parameter = "";

            int depth = r.Depth + 1;
            while (r.Read())
            {
                if (r.Depth != depth)
                    break;
                string membername = r["name"];
                if (string.IsNullOrEmpty(membername))
                {
                    Console.WriteLine("member without name");
                    return null;
                }
                if(string.IsNullOrEmpty(memberValue) == false)
                {
                    memberValue += Environment.NewLine;
                }
                if(string.IsNullOrEmpty(newParameter)== false)
                {
                    newParameter += ", ";
                }
                if(string.IsNullOrEmpty(setValue) == false)
                {
                    setValue += Environment.NewLine;
                }
                if(string.IsNullOrEmpty(stringClassMemberValue) == false)
                {
                    stringClassMemberValue += Environment.NewLine;
                }
                if(string.IsNullOrEmpty(parameter) == false)
                {
                    parameter += ", ";
                }
                string membertype = r.Name.ToLower();
                newParameter += string.Format(ClassFormat.valueFormat, membertype, membername);
                memberValue += string.Format(ClassFormat.fullValueFormat, membertype + " " + membername);
                setValue += string.Format(ClassFormat.setValueFormat, membername, membername);
                stringClassMemberValue += string.Format(ClassFormat.fullValueFormat, string.Format(ClassFormat.valueFormat, "string", membername));
                parameter += parameterstring(membername,membertype);
            }

            return new Tuple<string, string, string, string, string>(memberValue, newParameter, setValue, stringClassMemberValue, parameter);
        }
        public static string parameterstring(string membername,string membertype)
        {
            string parameter = "";
            if(membertype == "string")
            {
                parameter = "item.{0}";
            }
            else
            {
                parameter = membertype + ".Parse(item.{0})";
            }
            parameter = string.Format(parameter, membername);
            return parameter;
        }

    }
}
