using Shared.Abstractions;
using SqlSugar;
using System;

namespace Rep.Module.Communication.Services.CommunicationAggregate
{
    [SugarTable("Communication")]
    public class CommunicationEntity : Entity<string>, IAggregateRoot
    {
        ///// <summary>
        ///// ID 
        ///// </summary>
        [SugarColumn(IsPrimaryKey = true)]////如果是主键，此处必须指定，否则会引发InSingle(id)方法异常。
        public override string Id { get; protected set; } = DateTime.Now.ToString("yyyyMMddHHmmss");
        [SugarColumn(IsNullable = true)]
        public string ProductID { get; private set; }
        [SugarColumn(IsNullable = false)]
        public string ClientData { get;private set; }

        [SugarColumn(IsNullable = true)]
        public string MESResult { get; private set; }
        [SugarColumn(IsNullable = false)]
        public DateTime RecordTime { get; private set; }
        [SugarColumn(IsNullable = false)]
        public int State { get; private set; }
        [SugarColumn(IsNullable = false)]
        public int MothedType { get; private set; }
        [SugarColumn(IsNullable = true)]
        public string ClientObj { get; private set; }
        public CommunicationEntity()
        {

        }
        public CommunicationEntity(string id,string productID, string clientData,string mesResult, int state,int mothedType, string clientObj)
        {
            Id= id;
            ProductID = productID;
            ClientData= clientData;
            MESResult= mesResult;
            RecordTime= DateTime.Now;
            ClientObj= clientObj;
            State= state;
            MothedType= mothedType;
        }
        public void ChangeState(int state, string mesResult)
        {
            this.MESResult= mesResult;
            State= state;
        }
        
    }
}
