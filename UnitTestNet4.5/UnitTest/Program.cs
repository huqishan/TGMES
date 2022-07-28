using Communication;
using Communication.Model;
using System;
using System.Collections.Generic;

namespace UnitTest
{
    internal class Program
    {
        static void Main(string[] args)
        {
            DateTime now = DateTime.Now;
            RPCClient rpc = new RPCClient("192.168.1.234","admin","admin");
            List<MesDataInfoItem> mesData=new List<MesDataInfoItem>();
            string data1 = "{\"MesDataInfoItems\":[{\"Code\":\"PackCode\",\"Value\":\"0BOPEC201P2EAQABX0000002\"},{\"Code\":\"PackType\",\"Value\":\"2500010EDA\"},{\"Code\":\"TagNO\",\"Value\":\"ZBPT04RR\"},{\"Code\":\"OperateUser\",\"Value\":\"1\"},{\"Code\":\"StartTime\",\"Value\":\"2022/6/15 11:45:52\"},{\"Code\":\"EndTime\",\"Value\":\"2022/6/15 11:45:57\"},{\"Code\":\"Result\",\"Value\":\"OK\"},{\"Code\":\"FieldArr\",\"Value\":[\"放电DCR值(mΩ)\",\"放电(30A)电流\",\"充电(30A)电流\",\"放电(200A)电流\",\"放电(580A)电流\",\"充电(250A)电流\",\"IH_mOhm_BusBar01\",\"IH_mOhm_BusBar02\",\"IH_mOhm_BusBar03\",\"IH_mOhm_BusBar04\",\"IH_mOhm_BusBar05\",\"IH_mOhm_BusBar06\",\"IH_mOhm_BusBar07\",\"IH_mOhm_BusBar08\",\"IH_mOhm_BusBar09\",\"IH_mOhm_BusBar10\",\"IH_mOhm_BusBar11\",\"IH_mOhm_BusBar12\",\"IH_mOhm_BusBar13\",\"IH_mOhm_BusBar14\",\"IH_mOhm_BusBar15\",\"IH_mOhm_BusBar16\",\"IH_mOhm_BusBar17\",\"IH_mOhm_BusBar18\",\"IH_mOhm_BusBar19\",\"SOC范围（%）\",\"显示SOC值（%）\",\"BMS_BattSOH(%)\",\"继电器内侧高压（V）\",\"最高单体电压范围\",\"最低单体电压范围\",\"单体压差\",\"最高单体温度范围（℃）\",\"最低单体温度范围（℃）\",\"DTC数量\",\"故障代码\",\"压差(V)\"]},{\"Code\":\"ReferenceValueArr\",\"Value\":[\"放电DCR值(mΩ)<=75\",\"放电(30A)电流>=28 && 放电(30A)电流<=32\",\"充电(30A)电流>=-32 && 充电(30A)电流<=-28\",\"放电(200A)电流>=195 && 放电(200A)电流<=205\",\"放电(580A)电流>=560 && 放电(580A)电流<=600\",\"充电(250A)电流>=-255 && 充电(250A)电流<=-245\",\"信号值(mΩ)<=0.35 && 信号值(mΩ)>=0\",\"信号值(mΩ)<=0.35 && 信号值(mΩ)>=0\",\"信号值(mΩ)<=0.35 && 信号值(mΩ)>=0\",\"信号值(mΩ)<=0.35 && 信号值(mΩ)>=0\",\"信号值(mΩ)<=0.35 && 信号值(mΩ)>=0\",\"信号值(mΩ)<=0.35 && 信号值(mΩ)>=0\",\"信号值(mΩ)<=0.35 && 信号值(mΩ)>=0\",\"信号值(mΩ)<=0.35 && 信号值(mΩ)>=0\",\"信号值(mΩ)<=0.35 && 信号值(mΩ)>=0\",\"信号值(mΩ)<=0.35 && 信号值(mΩ)>=0\",\"信号值(mΩ)<=0.35 && 信号值(mΩ)>=0\",\"信号值(mΩ)<=0.35 && 信号值(mΩ)>=0\",\"信号值(mΩ)<=0.35 && 信号值(mΩ)>=0\",\"信号值(mΩ)<=0.35 && 信号值(mΩ)>=0\",\"信号值(mΩ)<=0.35 && 信号值(mΩ)>=0\",\"信号值(mΩ)<=0.35 && 信号值(mΩ)>=0\",\"信号值(mΩ)<=0.35 && 信号值(mΩ)>=0\",\"信号值(mΩ)<=0.35 && 信号值(mΩ)>=0\",\"信号值(mΩ)<=0.35 && 信号值(mΩ)>=0\",\"SOC范围（%）<=100 && SOC范围（%）>=0\",\"显示SOC值（%）<=100\",\"BMS_BattSOH(%)<=100 && BMS_BattSOH(%)>=80\",\"继电器内侧高压（V）<=437 && 继电器内侧高压（V）>=260\",\"最高单体电压范围<=4.17 && 最高单体电压范围>=3.12\",\"最低单体电压范围<=4.17 && 最低单体电压范围>=3.12\",\"单体压差<0.05\",\"最高单体温度范围（℃）>=-15 && 最高单体温度范围（℃）<=40\",\"最低单体温度范围（℃）<=40 && 最低单体温度范围（℃）>=-15\",\"DTC数量<11\",\"故障代码=DTC故障代码:\",\"压差(V)>=50\"]},{\"Code\":\"ValueArr\",\"Value\":[\"44.74\",\"30.5\",\"-30\",\"200.3\",\"579.2\",\"-250.5\",\"0.05\",\"0.05\",\"0.05\",\"0.1\",\"0.2\",\"0.15\",\"0.1\",\"0.05\",\"0\",\"0.05\",\"0.05\",\"0.05\",\"0.2\",\"0.15\",\"0.15\",\"0.05\",\"0.05\",\"0.05\",\"0.05\",\"65.8\",\"67\",\"100\",\"401\",\"3.875\",\"3.853\",\"0.0219999999999998\",\"30\",\"29\",\"9\",\"DTC故障代码:\",\"401\"]},{\"Code\":\"UnitArr\",\"Value\":[\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\",\"\"]},{\"Code\":\"DescriptionArr\",\"Value\":[\"放电DCR值(mΩ)\",\"放电(30A)电流\",\"充电(30A)电流\",\"放电(200A)电流\",\"放电(580A)电流\",\"充电(250A)电流\",\"IH_mOhm_BusBar01\",\"IH_mOhm_BusBar02\",\"IH_mOhm_BusBar03\",\"IH_mOhm_BusBar04\",\"IH_mOhm_BusBar05\",\"IH_mOhm_BusBar06\",\"IH_mOhm_BusBar07\",\"IH_mOhm_BusBar08\",\"IH_mOhm_BusBar09\",\"IH_mOhm_BusBar10\",\"IH_mOhm_BusBar11\",\"IH_mOhm_BusBar12\",\"IH_mOhm_BusBar13\",\"IH_mOhm_BusBar14\",\"IH_mOhm_BusBar15\",\"IH_mOhm_BusBar16\",\"IH_mOhm_BusBar17\",\"IH_mOhm_BusBar18\",\"IH_mOhm_BusBar19\",\"SOC范围（%）\",\"显示SOC值（%）\",\"BMS_BattSOH(%)\",\"继电器内侧高压（V）\",\"最高单体电压范围\",\"最低单体电压范围\",\"单体压差\",\"最高单体温度范围（℃）\",\"最低单体温度范围（℃）\",\"DTC数量\",\"故障代码\",\"压差(V)\"]},{\"Code\":\"ResultArr\",\"Value\":[\"OK\",\"OK\",\"OK\",\"OK\",\"OK\",\"OK\",\"OK\",\"OK\",\"OK\",\"OK\",\"OK\",\"OK\",\"OK\",\"OK\",\"OK\",\"OK\",\"OK\",\"OK\",\"OK\",\"OK\",\"OK\",\"OK\",\"OK\",\"OK\",\"OK\",\"OK\",\"OK\",\"OK\",\"OK\",\"OK\",\"OK\",\"OK\",\"OK\",\"OK\",\"OK\",\"OK\",\"OK\"]}],\"MethodName\":\"TestData\",\"MothedType\":5}";
            var model1= rpc.UpLoadTestData(mesData, "0BOPEC201P2EAQABX0000002");
            Console.WriteLine($"收到回复：{model1.Message} State:{model1.State} 耗时：{DateTime.Now.Subtract(now).TotalMilliseconds}");
            //List<Test> list = new List<Test>();
            //list.Add(new Test() { Name = "1", Description = "OK", Date = DateTime.Now.AddDays(1) });
            //list.Add(new Test() { Name = "1", Description = "NG", Date = DateTime.Now.AddDays(2) });
            //list.Add(new Test() { Name = "2", Description = "OK", Date = DateTime.Now.AddDays(3) });
            //list.Add(new Test() { Name = "3", Description = "OK", Date = DateTime.Now.AddDays(4) });
            //Test model = new Test() { Name = "2", Description = "OK", Date = DateTime.Now.AddDays(1) };
            //list.Insert(0, model);
            //ulong a = (ulong)0xF;
            //string b = "0xF";
            //var ttt = a == Convert.ToUInt64(b,16);
            //string num = "363";
            //int num1 = 363;
            //var hex=Convert.ToString(num1,16);
            //var ttttt = Convert.ToInt64(num,16);


            //CAN can = new CAN(new Shared.Infrastructure.CommunicationProtocol.Model.CommuniactionConfigModel(Shared.Infrastructure.CommunicationProtocol.Model.CANType.CANNET_2E_U,
            // "192.168.0.178",
            //  4001,
            //   "50000",
            //   0));
            //list.Add(new Test() {Name="1",Description="OK", Date = DateTime.Now.AddDays(1) });
            //list.Add(new Test() { Name = "1", Description = "NG",Date=DateTime.Now.AddDays(2) });
            //list.Add(new Test() { Name = "2", Description = "OK", Date = DateTime.Now.AddDays(3) });
            //list.Add(new Test() { Name = "3", Description = "OK", Date = DateTime.Now.AddDays(4) });
            //var t1=list.ToLookup(r=>r.Name).ToDictionary(r=>r.Key,r=>r.First()).Values.ToList();
            //foreach (var item in t1)
            //{
            //    Test tesst = list.Where(r => r.Name == item.Name).OrderByDescending(r => r.Date).First();
            //}
            //try
            //{
            //    var client = new RestClient("http://10.197.7.36:8204/equality.mes.ot/v1.0/packinterface/receivepackdcrtestdata");
            //    var request = new RestRequest("application/json", Method.POST);
            //    string data = "{\"PackCode\":\"0BOPEEA00P2EAQC3P0000007\",\"PackType\":\"2500010EDA\",\"TagNO\":\"ZBPT04RR\",\"OperateUser\":\"1\",\"StartTime\":\"2022/5/31 18:12:14\",\"EndTime\":\"2022/5/31 18:18:03\",\"Result\":\"OK\",\"TestDataItemParas\":[{\"Field\":\"放电DCR值(mΩ)\",\"ReferenceValue\":\"放电DCR值(mΩ)<=100\",\"Value\":\"24.75\",\"Unit\":\"\",\"Description\":\"放电DCR值(mΩ)\",\"Result\":\"OK\"},{\"Field\":\"放电(30A)电流\",\"ReferenceValue\":\"放电(30A)电流>=28 && 放电(30A)电流<=32\",\"Value\":\"30.2\",\"Unit\":\"\",\"Description\":\"放电(30A)电流\",\"Result\":\"OK\"},{\"Field\":\"充电(30A)电流\",\"ReferenceValue\":\"充电(30A)电流>=-32 && 充电(30A)电流<=-28\",\"Value\":\"-30.1\",\"Unit\":\"\",\"Description\":\"充电(30A)电流\",\"Result\":\"OK\"},{\"Field\":\"放电(200A)电流\",\"ReferenceValue\":\"放电(200A)电流>=195 && 放电(200A)电流<=205\",\"Value\":\"200.6\",\"Unit\":\"\",\"Description\":\"放电(200A)电流\",\"Result\":\"OK\"},{\"Field\":\"放电(580A)电流\",\"ReferenceValue\":\"放电(580A)电流>=560 && 放电(580A)电流<=600\",\"Value\":\"580.4\",\"Unit\":\"\",\"Description\":\"放电(580A)电流\",\"Result\":\"OK\"},{\"Field\":\"充电(250A)电流\",\"ReferenceValue\":\"充电(250A)电流>=-255 && 充电(250A)电流<=-245\",\"Value\":\"-250.6\",\"Unit\":\"\",\"Description\":\"充电(250A)电流\",\"Result\":\"OK\"},{\"Field\":\"IH_mOhm_BusBar01\",\"ReferenceValue\":\"信号值(mΩ)<=0.35\",\"Value\":\"0.05\",\"Unit\":\"\",\"Description\":\"IH_mOhm_BusBar01\",\"Result\":\"OK\"},{\"Field\":\"IH_mOhm_BusBar02\",\"ReferenceValue\":\"信号值(mΩ)<=0.35\",\"Value\":\"0.1\",\"Unit\":\"\",\"Description\":\"IH_mOhm_BusBar02\",\"Result\":\"OK\"},{\"Field\":\"IH_mOhm_BusBar03\",\"ReferenceValue\":\"信号值(mΩ)<=0.35\",\"Value\":\"0.05\",\"Unit\":\"\",\"Description\":\"IH_mOhm_BusBar03\",\"Result\":\"OK\"},{\"Field\":\"IH_mOhm_BusBar04\",\"ReferenceValue\":\"信号值(mΩ)<=0.35\",\"Value\":\"0.1\",\"Unit\":\"\",\"Description\":\"IH_mOhm_BusBar04\",\"Result\":\"OK\"},{\"Field\":\"IH_mOhm_BusBar05\",\"ReferenceValue\":\"信号值(mΩ)<=0.35\",\"Value\":\"0.1\",\"Unit\":\"\",\"Description\":\"IH_mOhm_BusBar05\",\"Result\":\"OK\"},{\"Field\":\"IH_mOhm_BusBar06\",\"ReferenceValue\":\"信号值(mΩ)<=0.35\",\"Value\":\"0.05\",\"Unit\":\"\",\"Description\":\"IH_mOhm_BusBar06\",\"Result\":\"OK\"},{\"Field\":\"IH_mOhm_BusBar07\",\"ReferenceValue\":\"信号值(mΩ)<=0.35\",\"Value\":\"0.2\",\"Unit\":\"\",\"Description\":\"IH_mOhm_BusBar07\",\"Result\":\"OK\"},{\"Field\":\"IH_mOhm_BusBar08\",\"ReferenceValue\":\"信号值(mΩ)<=0.35\",\"Value\":\"0.15\",\"Unit\":\"\",\"Description\":\"IH_mOhm_BusBar08\",\"Result\":\"OK\"},{\"Field\":\"IH_mOhm_BusBar09\",\"ReferenceValue\":\"信号值(mΩ)<=0.35\",\"Value\":\"0.05\",\"Unit\":\"\",\"Description\":\"IH_mOhm_BusBar09\",\"Result\":\"OK\"},{\"Field\":\"IH_mOhm_BusBar10\",\"ReferenceValue\":\"信号值(mΩ)<=0.35\",\"Value\":\"0.1\",\"Unit\":\"\",\"Description\":\"IH_mOhm_BusBar10\",\"Result\":\"OK\"},{\"Field\":\"IH_mOhm_BusBar11\",\"ReferenceValue\":\"信号值(mΩ)<=0.35\",\"Value\":\"0.05\",\"Unit\":\"\",\"Description\":\"IH_mOhm_BusBar11\",\"Result\":\"OK\"},{\"Field\":\"IH_mOhm_BusBar12\",\"ReferenceValue\":\"信号值(mΩ)<=0.35\",\"Value\":\"0.1\",\"Unit\":\"\",\"Description\":\"IH_mOhm_BusBar12\",\"Result\":\"OK\"},{\"Field\":\"IH_mOhm_BusBar13\",\"ReferenceValue\":\"信号值(mΩ)<=0.5\",\"Value\":\"0.35\",\"Unit\":\"\",\"Description\":\"IH_mOhm_BusBar13\",\"Result\":\"OK\"},{\"Field\":\"IH_mOhm_BusBar14\",\"ReferenceValue\":\"信号值(mΩ)<=0.35\",\"Value\":\"0.1\",\"Unit\":\"\",\"Description\":\"IH_mOhm_BusBar14\",\"Result\":\"OK\"},{\"Field\":\"IH_mOhm_BusBar15\",\"ReferenceValue\":\"信号值(mΩ)<=0.35\",\"Value\":\"0.05\",\"Unit\":\"\",\"Description\":\"IH_mOhm_BusBar15\",\"Result\":\"OK\"},{\"Field\":\"IH_mOhm_BusBar16\",\"ReferenceValue\":\"信号值(mΩ)<=0.35\",\"Value\":\"0.05\",\"Unit\":\"\",\"Description\":\"IH_mOhm_BusBar16\",\"Result\":\"OK\"},{\"Field\":\"IH_mOhm_BusBar17\",\"ReferenceValue\":\"信号值(mΩ)<=0.35\",\"Value\":\"0.1\",\"Unit\":\"\",\"Description\":\"IH_mOhm_BusBar17\",\"Result\":\"OK\"},{\"Field\":\"IH_mOhm_BusBar18\",\"ReferenceValue\":\"信号值(mΩ)<=0.35\",\"Value\":\"0.1\",\"Unit\":\"\",\"Description\":\"IH_mOhm_BusBar18\",\"Result\":\"OK\"},{\"Field\":\"IH_mOhm_BusBar19\",\"ReferenceValue\":\"信号值(mΩ)<=0.35\",\"Value\":\"0.05\",\"Unit\":\"\",\"Description\":\"IH_mOhm_BusBar19\",\"Result\":\"OK\"},{\"Field\":\"SOC范围（%）\",\"ReferenceValue\":\"SOC范围（%）<=100 && SOC范围（%）>=0\",\"Value\":\"28.1\",\"Unit\":\"\",\"Description\":\"SOC范围（%）\",\"Result\":\"OK\"},{\"Field\":\"显示SOC值（%）\",\"ReferenceValue\":\"显示SOC值（%）<=100\",\"Value\":\"27.3\",\"Unit\":\"\",\"Description\":\"显示SOC值（%）\",\"Result\":\"OK\"},{\"Field\":\"BMS_BattSOH(%)\",\"ReferenceValue\":\"BMS_BattSOH(%)<=100 && BMS_BattSOH(%)>=80\",\"Value\":\"100\",\"Unit\":\"\",\"Description\":\"BMS_BattSOH(%)\",\"Result\":\"OK\"},{\"Field\":\"继电器内侧高压（V）\",\"ReferenceValue\":\"继电器内侧高压（V）<=405 && 继电器内侧高压（V）>=295\",\"Value\":\"345\",\"Unit\":\"\",\"Description\":\"继电器内侧高压（V）\",\"Result\":\"OK\"},{\"Field\":\"最高单体电压范围\",\"ReferenceValue\":\"最高单体电压范围<=4.17 && 最高单体电压范围>=3.12\",\"Value\":\"3.603\",\"Unit\":\"\",\"Description\":\"最高单体电压范围\",\"Result\":\"OK\"},{\"Field\":\"最低单体电压范围\",\"ReferenceValue\":\"最低单体电压范围<=4.17 && 最低单体电压范围>=3.12\",\"Value\":\"3.594\",\"Unit\":\"\",\"Description\":\"最低单体电压范围\",\"Result\":\"OK\"},{\"Field\":\"单体压差\",\"ReferenceValue\":\"单体压差<0.05\",\"Value\":\"0.00900000000000034\",\"Unit\":\"\",\"Description\":\"单体压差\",\"Result\":\"OK\"},{\"Field\":\"最高单体温度范围（℃）\",\"ReferenceValue\":\"最高单体温度范围（℃）>=-15 && 最高单体温度范围（℃）<=40\",\"Value\":\"34\",\"Unit\":\"\",\"Description\":\"最高单体温度范围（℃）\",\"Result\":\"OK\"},{\"Field\":\"最低单体温度范围（℃）\",\"ReferenceValue\":\"最低单体温度范围（℃）<=40 && 最低单体温度范围（℃）>=-15\",\"Value\":\"33\",\"Unit\":\"\",\"Description\":\"最低单体温度范围（℃）\",\"Result\":\"OK\"},{\"Field\":\"DTC数量\",\"ReferenceValue\":\"DTC数量<14\",\"Value\":\"13\",\"Unit\":\"\",\"Description\":\"DTC数量\",\"Result\":\"OK\"},{\"Field\":\"故障代码\",\"ReferenceValue\":\"故障代码=DTC故障代码:\",\"Value\":\"DTC故障代码:\",\"Unit\":\"\",\"Description\":\"故障代码\",\"Result\":\"OK\"},{\"Field\":\"压差(V)\",\"ReferenceValue\":\"压差(V)>=50\",\"Value\":\"282\",\"Unit\":\"\",\"Description\":\"压差(V)\",\"Result\":\"OK\"}]}";
            //    request.AddBody(data);
            //    var result = client.Post(request);
            //    var t = result.Content;
            //    Console.WriteLine(t);
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.Message);
            //}

            Console.ReadKey();
        }
    }
    public class Test 
    {
        public string Name;
        public string Description;
        public DateTime Date;
    }
}
