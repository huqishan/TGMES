using Newtonsoft.Json.Linq;
using Rep.Controls.Model;
using Shared.Infrastructure.PackMethod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Rep.Controls.Converts
{
    public static class MesDataConvert
    {
        static readonly string layoutFile = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase + "MESConfig";
        public static string? Convert(MesDataInfoTree sourceData, out string soapUrl)
        {
            soapUrl = null;

            TreeModel? dataLayout = JsonHelper.ReadJson<TreeModel>($"{layoutFile}\\{sourceData.MethodName}.json");
            switch (dataLayout?.DataType)
            {
                case "JSON":
                    return ItemsToJSONString(sourceData, dataLayout.Childs).ToString();
                case "XML":
                    break;
                case "SOAP":
                    soapUrl = $"{sourceData.Url}?op={dataLayout.MESCode}";
                    XNamespace xmlnx = sourceData.Xmlns;
                    XNamespace soap = sourceData.Soap;
                    XElement root = new XElement(xmlnx + dataLayout.MESCode);
                    ItemsToSOAPString(root, xmlnx, sourceData, dataLayout.Childs);
                    XDocument document = new XDocument();
                    document.Add(new XElement(soap + "Envelope",
                        new XAttribute(XNamespace.Xmlns + "xsi", sourceData.Xsi),
                        new XAttribute(XNamespace.Xmlns + "xsd", sourceData.Xsd),
                        new XAttribute(XNamespace.Xmlns + "soap", soap),
                        new XElement(soap + "Body", root)
                        ));
                    return document.ToString();
                default:
                    break;
            }
            return null;
        }
        private static XElement? ItemsToSOAPString(XElement root, XNamespace xmlnx, MesDataInfoTree sourceData, IEnumerable<TreeModel> mesDataInfoItems)
        {
            foreach (var item in mesDataInfoItems)
            {
                XElement element = new XElement(xmlnx + item.MESCode);
                var v = sourceData.MesDataInfoItems.FirstOrDefault(r => r.Code.Equals(item.ClientCode))?.Value;
                if (string.IsNullOrEmpty(v?.ToString())) v = item.DefectValue;
                if (item.Childs != null && item.Childs.Count > 0)
                {
                    if (item.DataType.ToUpper().Contains("STRINGARR"))
                    {
                        v = sourceData.MesDataInfoItems.FirstOrDefault(r => r.Code.Equals(item.Childs.FirstOrDefault()?.ClientCode))?.Value;
                        if (v is IEnumerable<object> arr)
                        {
                            for (int i = 0; i < arr.Count(); i++)
                            {
                                XElement elementArr = new XElement(xmlnx + item.MESCode);
                                foreach (var values in item.Childs)
                                {
                                    v = sourceData.MesDataInfoItems.FirstOrDefault(r => r.Code.Equals(values.ClientCode))?.Value;
                                    if (v is IEnumerable<object> valueArr)
                                    {
                                        var t = valueArr.ToArray()[i] as IEnumerable<object>;
                                        ValueTypeConvert(elementArr, values.DataType, valueArr.ToArray()[i].ToString(), values.MESCode, values.KeepDecimalLength);
                                    }
                                    else
                                    {
                                        if (string.IsNullOrEmpty(v?.ToString())) v = values.DefectValue;
                                        ValueTypeConvert(elementArr, values.DataType, v, values.MESCode, values.KeepDecimalLength);
                                    }
                                }
                                root.Add(elementArr);
                            }
                        }
                        else
                        {
                            ItemsToSOAPString(element, xmlnx, sourceData, item.Childs);
                            root.Add(element);
                        }

                    }
                    else
                    {
                        ItemsToSOAPString(element, xmlnx, sourceData, item.Childs);
                        root.Add(element);
                    }

                }
                else
                {
                    ValueTypeConvert(root, item.DataType, v, item.MESCode, item.KeepDecimalLength);
                }
            }
            void ValueTypeConvert(XElement root, string type, object value, string name, string keepDecimalLength)
            {
                XElement element = new XElement(xmlnx + name);
                if (type.ToUpper().Contains("STRING"))
                {
                    if (value is IEnumerable<object> arr)
                    {
                        root.Add(arr.Select(r => new XElement(xmlnx + name, r.ToString())));
                    }
                    else
                    {
                        element.Add((value ?? "null").ToString());
                    }
                }
                else if (type.ToUpper().Contains("INT"))
                {
                    if (value is IEnumerable<object> arr)
                    {
                        root.Add(arr.Select(r => new XElement(xmlnx + name, System.Convert.ToInt32(r))));
                    }
                    else
                    {
                        element.Add(System.Convert.ToInt32(value));
                    }
                }
                else if (type.ToUpper().Contains("DOUBLE"))
                {
                    if (value is IEnumerable<object> arr)
                    {
                        root.Add(arr.Select(r => new XElement(xmlnx + name, Math.Round(System.Convert.ToDouble(r), System.Convert.ToInt32(keepDecimalLength)))));
                    }
                    else
                    {
                        element.Add(Math.Round(System.Convert.ToDouble(value), System.Convert.ToInt32(keepDecimalLength)));
                    }
                }
                root.Add(element);
            }
            return root;
        }
        private static JObject ItemsToJSONString(MesDataInfoTree sourceData, IEnumerable<TreeModel> mesDataInfoItems)
        {
            JObject jsonObj = new JObject();
            foreach (var item in mesDataInfoItems)
            {
                var v = sourceData.MesDataInfoItems.FirstOrDefault(r => r.Code.Equals(item.ClientCode))?.Value;
                if (string.IsNullOrEmpty(v?.ToString())) v = item.DefectValue;
                if (item.Childs != null && item.Childs.Count > 0)
                {
                    if (item.DataType.ToUpper().Contains("STRINGARR"))
                    {
                        v = sourceData.MesDataInfoItems.FirstOrDefault(r => r.Code.Equals(item.Childs.FirstOrDefault()?.ClientCode))?.Value;
                        if (v is IEnumerable<object> arr)
                        {
                            JArray jArray = new JArray();
                            for (int i = 0; i < arr.Count(); i++)
                            {
                                JObject jsonObjArr = new JObject();
                                foreach (var values in item.Childs)
                                {
                                    v = sourceData.MesDataInfoItems.FirstOrDefault(r => r.Code.Equals(values.ClientCode))?.Value;
                                    if (v is IEnumerable<object> valueArr)
                                    {
                                        ValueTypeConvert(jsonObjArr, values.DataType, valueArr.ToArray()[i].ToString(), values.MESCode, values.KeepDecimalLength);
                                    }
                                    else
                                    {
                                        if (string.IsNullOrEmpty(v?.ToString())) v = values.DefectValue;
                                        ValueTypeConvert(jsonObjArr, values.DataType, v, values.MESCode, values.KeepDecimalLength);
                                    }
                                }
                                jArray.Add(jsonObjArr);
                            }
                            jsonObj.Add(item.MESCode, jArray);
                        }
                        else
                        {
                            jsonObj.Add(item.MESCode, ItemsToJSONString(sourceData, item.Childs));
                        }
                    }
                    else
                    {
                        jsonObj.Add(item.MESCode, ItemsToJSONString(sourceData, item.Childs));
                    }
                }
                else
                {
                    ValueTypeConvert(jsonObj, item.DataType, v, item.MESCode, item.KeepDecimalLength);
                }
            }
            void ValueTypeConvert(JObject root, string type, object value, string name, string keepDecimalLength)
            {
                if (type.ToUpper().Contains("STRING"))
                {
                    if (value is IEnumerable<object> arr)
                    {
                        JArray jArray = new JArray();
                        jArray.Add(arr.Select(r => r.ToString()));
                        root.Add(name, jArray);
                    }
                    else
                    {
                        root.Add(name, (value ?? "null").ToString());
                    }
                }
                else if (type.ToUpper().Contains("INT"))
                {
                    if (value is IEnumerable<object> arr)
                    {
                        JArray jArray = new JArray();
                        jArray.Add(arr.Select(r => System.Convert.ToInt32(r)));
                        root.Add(name, jArray);
                    }
                    else
                    {
                        root.Add(name, System.Convert.ToInt32(value));
                    }
                }
                else if (type.ToUpper().Contains("DOUBLE"))
                {
                    if (value is IEnumerable<object> arr)
                    {
                        JArray jArray = new JArray();
                        jArray.Add(arr.Select(r => Math.Round(System.Convert.ToDouble(r), System.Convert.ToInt32(keepDecimalLength))));
                        root.Add(name, jArray);
                    }
                    else
                    {
                        root.Add(name, Math.Round(System.Convert.ToDouble(value), System.Convert.ToInt32(keepDecimalLength)));
                    }
                }
            }
            return jsonObj;
        }
        public static MesResult ResultConvert(string result)
        {
            MesResult model = new MesResult();
            model.Message = result;
            if (result.StartsWith("{"))//Json数据
            {
                model.State = JsonHelper.GetJsonValue(result, "success").ToUpper() == "TRUE";
            }
            return model;
        }
    }
}
