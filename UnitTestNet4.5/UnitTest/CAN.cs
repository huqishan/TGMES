using Shared.Infrastructure.CommunicationProtocol.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest
{
    public class CAN
    {
        #region Propertys

        IntPtr m_dev_handle;
        IntPtr m_ch_handle;
        IProperty _property;
        public uint devicetype = 17;
        #endregion
        public CAN(CommuniactionConfigModel config)
        {
            m_dev_handle = DBC_Method.ZCAN_OpenDevice((uint)config.CANType, 0, 0);
            if ((int)m_dev_handle == 0)
            {
            }
            IntPtr property = DBC_Method.GetIProperty(m_dev_handle);
            if ((int)property == 0)
            {
            }
            IProperty oneIProperty = (IProperty)Marshal.PtrToStructure((IntPtr)(uint)property, typeof(IProperty));
            oneIProperty.SetValue($"{config.Channel}/baud_rate", config.BaudRete);
            ZCAN_CHANNEL_INIT_CONFIG cfg = new ZCAN_CHANNEL_INIT_CONFIG();
            cfg.can_type = 0;
            cfg.can.mode = 0;
            IntPtr ptr = Marshal.AllocHGlobal(Marshal.SizeOf(cfg));
            Marshal.StructureToPtr(cfg, ptr, true);
            _property = (IProperty)Marshal.PtrToStructure((IntPtr)(uint)property, typeof(IProperty));
            _property.SetValue($"{config.Channel}/work_mode", "0");
            _property.SetValue($"{config.Channel}/ip", config.RemoteIPAddress);
            //property_.SetValue(channel_index + "/local_port", port + _can.E_id);  这个是电脑作为服务器模式
            _property.SetValue($"{config.Channel}/work_port", config.RemotePort.ToString());
            ZCAN_CHANNEL_INIT_CONFIG config_ = new ZCAN_CHANNEL_INIT_CONFIG();
            if (devicetype == 20 || devicetype == 21 || devicetype == 4)
            {
                config_.can_type = Define.TYPE_CAN;
                config_.can.filter = 0;
                config_.can.acc_code = 0;
                config_.can.acc_mask = 0xFFFFFFFF;
            }
            IntPtr pConfig = Marshal.AllocHGlobal(Marshal.SizeOf(config_));
            Marshal.StructureToPtr(config_, pConfig, true);
            m_ch_handle = DBC_Method.ZCAN_InitCAN(m_dev_handle, 0, pConfig);
            if ((int)m_ch_handle == 0)
            {
            }

            uint ret = DBC_Method.ReleaseIProperty(property);
            if (DBC_Method.ZCAN_StartCAN(m_ch_handle) != 1) //STATUS_OK 1
            {
            }
        }
        public bool Close()
        {
            throw new NotImplementedException();
        }


        public bool Start()
        {
            throw new NotImplementedException();
        }

    }
    #region dllManage

    public class DBC_Method
    {
        /* special address description flags for the MAKE_CAN_ID */
        public static UInt32 CAN_EFF_FLAG = 0x80000000U; /* EFF/SFF is set in the MSB */
        public static UInt32 CAN_RTR_FLAG = 0x40000000U; /* remote transmission request */
        public static UInt32 CAN_ERR_FLAG = 0x20000000U; /* error message frame */
        public static UInt32 CAN_ID_FLAG = 0x1FFFFFFFU; /* id */
        // make id
        public static UInt32 MAKE_CAN_ID(UInt32 id, UInt32 eff, UInt32 rtr, UInt32 err) { return (id | (eff << 31) | (rtr << 30) | (err << 29)); }
        public static UInt32 IS_EFF(UInt32 id) { return ((id & CAN_EFF_FLAG)); } //非0:extend frame 0:standard frame
        public static UInt32 IS_RTR(UInt32 id) { return ((id & CAN_RTR_FLAG)); } //非0:remote frame 0:data frame
        public static UInt32 IS_ERR(UInt32 id) { return ((id & CAN_ERR_FLAG)); } //非0:error frame 0:normal frame
        public static UInt32 GET_ID(UInt32 id) { return (id & CAN_ID_FLAG); }

        // LibDBCManager.dll
        public delegate bool OnSend(IntPtr ctx, IntPtr pObj);
        public delegate void OnMultiTransDone(IntPtr ctx, IntPtr pMsg, IntPtr data, UInt16 nLen, byte nDirection);
        public static OnSend onSend;
        public static OnMultiTransDone onMultiTransDone;

        [DllImport(@"\ZLG\LibDBCManager.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern uint DBC_Init();

        [DllImport(@"\ZLG\LibDBCManager.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern uint DBC_Release(uint hDBC);

        [DllImport(@"\ZLG\LibDBCManager.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern bool DBC_LoadFile(uint hDBC, IntPtr pMsg);

        [DllImport(@"\ZLG\LibDBCManager.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern uint DBC_GetMessageCount(uint hDBC);

        [DllImport(@"\ZLG\LibDBCManager.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern bool DBC_GetFirstMessage(uint hDBC, IntPtr pMsg);

        [DllImport(@"\ZLG\LibDBCManager.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern bool DBC_GetNextMessage(uint hDBC, IntPtr pMsg);

        [DllImport(@"\ZLG\LibDBCManager.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern void DBC_SetSender(uint hDBC, OnSend sender, IntPtr ctx);

        [DllImport(@"\ZLG\LibDBCManager.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern void DBC_SetOnMultiTransDoneFunc(uint hDBC, OnMultiTransDone func, IntPtr ctx);

        [DllImport(@"\ZLG\LibDBCManager.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern void DBC_OnReceive(uint hDBC, IntPtr pObj);

        [DllImport(@"\ZLG\LibDBCManager.dll", CallingConvention = CallingConvention.StdCall)]
        //public static extern bool DBC_Analyse(uint hDBC, IntPtr pObj, IntPtr pMsg);
        public static extern bool DBC_Analyse(uint hDBC, ref can_frame pObj, ref DBCMessage pMsg);

        [DllImport(@"\ZLG\LibDBCManager.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern byte DBC_Send(uint hDBC, IntPtr pMsg);

        // zlgcan.dll
        [DllImport(@"\ZLG\zlgcan.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr ZCAN_OpenDevice(uint device_type, uint device_index, uint reserved);

        [DllImport(@"\ZLG\zlgcan.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern uint ZCAN_CloseDevice(IntPtr device_handle);

        [DllImport(@"\ZLG\zlgcan.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr ZCAN_InitCAN(IntPtr device_handle, uint can_index, IntPtr pInitConfig);

        [DllImport(@"\ZLG\zlgcan.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern uint ZCAN_StartCAN(IntPtr channel_handle);

        [DllImport(@"\ZLG\zlgcan.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern uint ZCAN_ResetCAN(IntPtr channel_handle);

        [DllImport(@"\ZLG\zlgcan.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern uint ZCAN_Transmit(IntPtr channel_handle, IntPtr pTransmit, uint len);

        [DllImport("zlgcan.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern uint ZCAN_TransmitFD(IntPtr channel_handle, IntPtr pTransmit, uint len);

        [DllImport(@"\ZLG\zlgcan.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern uint ZCAN_GetReceiveNum(IntPtr channel_handle, byte type);

        [DllImport(@"\ZLG\zlgcan.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern uint ZCAN_Receive(IntPtr channel_handle, IntPtr data, uint len, int wait_time = -1);

        [DllImport(@"\ZLG\zlgcan.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern uint ZCAN_Receive(IntPtr channel_handle, [In, Out] ZCAN_Receive_Data[] data, uint len, int wait_time);

        [DllImport(@"\ZLG\zlgcan.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr GetIProperty(IntPtr device_handle);

        [DllImport(@"\ZLG\zlgcan.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern uint ReleaseIProperty(IntPtr pIProperty);

        [DllImport("zlgcan.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern uint ZCAN_ReceiveFD(IntPtr channel_handle, IntPtr data, uint len, int wait_time = -1);

        [DllImport("zdbc.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern bool ZDBC_Analyse(uint hDBC, IntPtr pObj, byte frame_type, IntPtr pMsg);

        [DllImport(@"\ZLG\zlgcan.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern uint ZCAN_ClearBuffer(IntPtr channel_handle);

    }

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate int SetValueFunc(string path, string value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate string GetValueFunc(string path, string value);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate IntPtr GetPropertysFunc(string path, string value);
    public class Define
    {
        public const int TYPE_CAN = 0;
        public const int _MAX_FILE_PATH_ = 260;//最长文件路径
        public const int _DBC_NAME_LENGTH_ = 127;//名称最长长度

        public const int _DBC_COMMENT_MAX_LENGTH_ = 200;//注释最长长度
        public const int _DBC_UNIT_MAX_LENGTH_ = 10;//单位最长长度
        public const int _DBC_SIGNAL_MAX_COUNT_ = 120;//一个消息含有的信号的最大数目
        public const int PROTOCOL_J1939 = 0;
        public const int PROTOCOL_OTHER = 1;
        public const uint INVALID_DBC_HANDLE = 0xffffffff; // 无效的DBC句柄
    }
    #region Struct
    [StructLayout(LayoutKind.Explicit)]
    public struct ZCAN_CHANNEL_INIT_CONFIG
    {
        [FieldOffset(0)]
        public uint can_type; //type:TYPE_CAN TYPE_CANFD

        [FieldOffset(4)]
        public ZCAN can;

        [FieldOffset(4)]
        public CANFD canfd;
    };
    [StructLayout(LayoutKind.Sequential)]
    public struct ZCAN
    {
        public uint acc_code;
        public uint acc_mask;
        public uint reserved;
        public byte filter;
        public byte timing0;
        public byte timing1;
        public byte mode;
    };
    [StructLayout(LayoutKind.Sequential)]
    public struct CANFD
    {
        public uint acc_code;
        public uint acc_mask;
        public uint abit_timing;
        public uint dbit_timing;
        public uint brp;
        public byte filter;
        public byte mode;
        public UInt16 pad;
        public uint reserved;
    };
    public struct IProperty
    {
        public SetValueFunc SetValue;
        public GetValueFunc GetValue;
        public GetPropertysFunc GetPropertys;
    };

    [StructLayout(LayoutKind.Sequential)]
    public struct ZCAN_ReceiveFD_Data
    {
        public canfd_frame frame;
        public UInt64 timestamp;//us
    };

    [StructLayout(LayoutKind.Sequential)]
    public struct ZCAN_Receive_Data
    {
        public can_frame frame;
        public UInt64 timestamp;//us
    };
    [StructLayout(LayoutKind.Sequential)]
    public struct can_frame
    {
        public uint can_id;  /* 32 bit MAKE_CAN_ID + EFF/RTR/ERR flags */
        public byte can_dlc; /* frame payload length in byte (0 .. CAN_MAX_DLEN) */
        public byte __pad;   /* padding */
        public byte __res0;  /* reserved / padding */
        public byte __res1;  /* reserved / padding */
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public byte[] data/* __attribute__((aligned(8)))*/;
    };
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct DBCMessage : ICloneable
    {
        public UInt32 nSignalCount; //信号数量
        public UInt32 nID;
        public UInt32 nSize;    //消息占的字节数目
        double nCycleTime;//发送周期
        public byte nExtend; //1:扩展帧, 0:标准帧
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Define._DBC_SIGNAL_MAX_COUNT_)]
        public DBCSignal[] vSignals; //信号集合
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Define._DBC_NAME_LENGTH_ + 1)]
        private char[] strName;  //名称
        public string StrName { get => new string(strName); set => strName = value.ToArray(); }
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Define._DBC_COMMENT_MAX_LENGTH_ + 1)]
        public char[] strComment;    //注释

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    };
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct DBCSignal
    {
        public UInt32 nStartBit; // 起始位
        public UInt32 nLen;	// 位长度
        public double nFactor; // 转换因子
        public double nOffset;	// 转换偏移 实际值=原始值*nFactor+nOffset
        public double nMin;    // 最小值
        public double nMax;	// 最大值
        /// <summary>
        /// 实际值
        /// </summary>
        public double nValue;
        public UInt64 nRawValue;//原始值
        public byte is_signed; //1:有符号数据, 0:无符号
        public byte is_motorola;//是否摩托罗拉格式
        public byte multiplexer_type;//see 'multiplexer type' above
        public byte val_type;//0:integer, 1:float, 2:double
        public UInt32 multiplexer_value;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Define._DBC_UNIT_MAX_LENGTH_ + 1)]
        public char[] unit;//单位
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Define._DBC_NAME_LENGTH_ + 1)]
        public char[] strName;  //名称
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Define._DBC_COMMENT_MAX_LENGTH_ + 1)]
        public char[] strComment;  //注释
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Define._DBC_NAME_LENGTH_ + 1)]
        public char[] strValDesc;  //值描述
        //[MarshalAs(UnmanagedType.ByValTStr, SizeConst = Define._DBC_UNIT_MAX_LENGTH_ + 1)]
        //public string unit;//单位
        //[MarshalAs(UnmanagedType.ByValTStr, SizeConst = Define._DBC_NAME_LENGTH_ + 1)]
        //public string strName;  //名称
        //[MarshalAs(UnmanagedType.ByValTStr, SizeConst = Define._DBC_COMMENT_MAX_LENGTH_ + 1)]
        //public string strComment;  //注释
        //[MarshalAs(UnmanagedType.ByValTStr, SizeConst = Define._DBC_NAME_LENGTH_ + 1)]
        //public string strValDesc;  //值描述

        public string Unit { get => new string(unit); set => unit = value.ToArray(); }
        public string StrName { get => new string(strName); set => strName = value.ToArray(); }
        public string StrComment { get => new string(strComment); set => strComment = value.ToArray(); }
        public double NValue { get => nValue; set => nValue = value; }
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct canfd_frame
    {
        public uint can_id;  /* 32 bit MAKE_CAN_ID + EFF/RTR/ERR flags */
        public byte len;     /* frame payload length in byte */
        public byte flags;   /* additional flags for CAN FD,i.e error code */
        public byte __res0;  /* reserved / padding */
        public byte __res1;  /* reserved / padding */
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
        public byte[] data/* __attribute__((aligned(8)))*/;
    };

    #endregion

    #endregion
}
