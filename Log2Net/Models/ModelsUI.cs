using Log2Net.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Log2Net.Models
{

    /// <summary>
    /// 业务系统中的操作轨迹实体
    /// </summary>
    [Serializable]
    public class Log_OperateTraceBllEdm
    {
        public string UserID { get { return _UserID; } set { _UserID = value; } }//用户工号
        public string UserName { get { return _UserName; } set { _UserName = value; } }//用户姓名
        public LogType LogType { get { return _LogType; } set { _LogType = value; } } //日志类型
        public string TabOrModu { get { return _TabOrModu; } set { _TabOrModu = value; } }//表名或模块名称
        public string Detail { get; set; } //日志内容
        public string Remark { get; set; } //其他信息

        string _TabOrModu = "启动";
        string _UserID = "系统";
        string _UserName = "系统";
        LogType _LogType = LogType.业务记录;
    }

    /// <summary>
    /// 各业务系统中使用的监控信息实体
    /// </summary>
    public class LogMonitorEdm
    {
        public string Remark { get; set; }
        public List<PageVist> PagesView { get; set; }//重要页面访问量
    }

    /// <summary>
    /// 页面访问量实体定义
    /// </summary>
    public class PageVist
    {
        public string PageUrl { get; set; }
        public int VisitNum { get; set; }
    }

    /// <summary>
    /// 日志的级别
    /// </summary>
    public enum LogLevel
    {
        Off = 1,
        Error = 2,
        Warn = 3,
        Info = 4,
        Debug = 5
    }



    #region 系统类别枚举:SysCategory
    /// <summary>
    /// 业务系统枚举 
    /// </summary>
    public enum SysCategory
    {
        [StringEnum.EnumDescription("所有系统")]
        ALL = -1,
        [StringEnum.EnumDescription("未定义")]
        NoDefined = 0,

        [Description("A系列系统的第1个子网站")]
        SysA_01 = 0101,
        [Description("A系列系统的第2个子网站")]
        SysA_02 = 0102,
        [Description("A系列系统的第3个子网站")]
        SysA_03 = 0103,
        [Description("A系列系统的第4个子网站")]
        SysA_04 = 0104,
        [Description("A系列系统的第5个子网站")]
        SysA_05 = 0105,
        [Description("A系列系统的第6个子网站")]
        SysA_06 = 0106,
        [Description("A系列系统的第7个子网站")]
        SysA_07 = 0107,
        [Description("A系列系统的第8个子网站")]
        SysA_08 = 0108,
        [Description("A系列系统的第9个子网站")]
        SysA_09 = 0109,
        [Description("A系列系统的第10个子网站")]
        SysA_10 = 0110,
        [Description("A系列系统的第11个子网站")]
        SysA_11 = 0111,
        [Description("A系列系统的第12个子网站")]
        SysA_12 = 0112,
        [Description("A系列系统的第13个子网站")]
        SysA_13 = 0113,
        [Description("A系列系统的第14个子网站")]
        SysA_14 = 0114,
        [Description("A系列系统的第15个子网站")]
        SysA_15 = 0115,
        [Description("A系列系统的第16个子网站")]
        SysA_16 = 0116,
        [Description("A系列系统的第17个子网站")]
        SysA_17 = 0117,
        [Description("A系列系统的第18个子网站")]
        SysA_18 = 0118,
        [Description("A系列系统的第19个子网站")]
        SysA_19 = 0119,
        [Description("A系列系统的第20个子网站")]
        SysA_20 = 0120,
        [Description("A系列系统的第21个子网站")]
        SysA_21 = 0121,
        [Description("A系列系统的第22个子网站")]
        SysA_22 = 0122,
        [Description("A系列系统的第23个子网站")]
        SysA_23 = 0123,
        [Description("A系列系统的第24个子网站")]
        SysA_24 = 0124,
        [Description("A系列系统的第25个子网站")]
        SysA_25 = 0125,
        [Description("A系列系统的第26个子网站")]
        SysA_26 = 0126,
        [Description("A系列系统的第27个子网站")]
        SysA_27 = 0127,
        [Description("A系列系统的第28个子网站")]
        SysA_28 = 0128,
        [Description("A系列系统的第29个子网站")]
        SysA_29 = 0129,
        [Description("A系列系统的第30个子网站")]
        SysA_30 = 0130,
        [Description("A系列系统的第31个子网站")]
        SysA_31 = 0131,
        [Description("A系列系统的第32个子网站")]
        SysA_32 = 0132,
        [Description("A系列系统的第33个子网站")]
        SysA_33 = 0133,
        [Description("A系列系统的第34个子网站")]
        SysA_34 = 0134,
        [Description("A系列系统的第35个子网站")]
        SysA_35 = 0135,
        [Description("A系列系统的第36个子网站")]
        SysA_36 = 0136,
        [Description("A系列系统的第37个子网站")]
        SysA_37 = 0137,
        [Description("A系列系统的第38个子网站")]
        SysA_38 = 0138,
        [Description("A系列系统的第39个子网站")]
        SysA_39 = 0139,
        [Description("A系列系统的第40个子网站")]
        SysA_40 = 0140,
        [Description("A系列系统的第41个子网站")]
        SysA_41 = 0141,
        [Description("A系列系统的第42个子网站")]
        SysA_42 = 0142,
        [Description("A系列系统的第43个子网站")]
        SysA_43 = 0143,
        [Description("A系列系统的第44个子网站")]
        SysA_44 = 0144,
        [Description("A系列系统的第45个子网站")]
        SysA_45 = 0145,
        [Description("A系列系统的第46个子网站")]
        SysA_46 = 0146,
        [Description("A系列系统的第47个子网站")]
        SysA_47 = 0147,
        [Description("A系列系统的第48个子网站")]
        SysA_48 = 0148,
        [Description("A系列系统的第49个子网站")]
        SysA_49 = 0149,
        [Description("A系列系统的第50个子网站")]
        SysA_50 = 0150,
        [Description("A系列系统的第51个子网站")]
        SysA_51 = 0151,
        [Description("A系列系统的第52个子网站")]
        SysA_52 = 0152,
        [Description("A系列系统的第53个子网站")]
        SysA_53 = 0153,
        [Description("A系列系统的第54个子网站")]
        SysA_54 = 0154,
        [Description("A系列系统的第55个子网站")]
        SysA_55 = 0155,
        [Description("A系列系统的第56个子网站")]
        SysA_56 = 0156,
        [Description("A系列系统的第57个子网站")]
        SysA_57 = 0157,
        [Description("A系列系统的第58个子网站")]
        SysA_58 = 0158,
        [Description("A系列系统的第59个子网站")]
        SysA_59 = 0159,
        [Description("A系列系统的第60个子网站")]
        SysA_60 = 0160,
        [Description("A系列系统的第61个子网站")]
        SysA_61 = 0161,
        [Description("A系列系统的第62个子网站")]
        SysA_62 = 0162,
        [Description("A系列系统的第63个子网站")]
        SysA_63 = 0163,
        [Description("A系列系统的第64个子网站")]
        SysA_64 = 0164,
        [Description("A系列系统的第65个子网站")]
        SysA_65 = 0165,
        [Description("A系列系统的第66个子网站")]
        SysA_66 = 0166,
        [Description("A系列系统的第67个子网站")]
        SysA_67 = 0167,
        [Description("A系列系统的第68个子网站")]
        SysA_68 = 0168,
        [Description("A系列系统的第69个子网站")]
        SysA_69 = 0169,
        [Description("A系列系统的第70个子网站")]
        SysA_70 = 0170,
        [Description("A系列系统的第71个子网站")]
        SysA_71 = 0171,
        [Description("A系列系统的第72个子网站")]
        SysA_72 = 0172,
        [Description("A系列系统的第73个子网站")]
        SysA_73 = 0173,
        [Description("A系列系统的第74个子网站")]
        SysA_74 = 0174,
        [Description("A系列系统的第75个子网站")]
        SysA_75 = 0175,
        [Description("A系列系统的第76个子网站")]
        SysA_76 = 0176,
        [Description("A系列系统的第77个子网站")]
        SysA_77 = 0177,
        [Description("A系列系统的第78个子网站")]
        SysA_78 = 0178,
        [Description("A系列系统的第79个子网站")]
        SysA_79 = 0179,
        [Description("A系列系统的第80个子网站")]
        SysA_80 = 0180,
        [Description("A系列系统的第81个子网站")]
        SysA_81 = 0181,
        [Description("A系列系统的第82个子网站")]
        SysA_82 = 0182,
        [Description("A系列系统的第83个子网站")]
        SysA_83 = 0183,
        [Description("A系列系统的第84个子网站")]
        SysA_84 = 0184,
        [Description("A系列系统的第85个子网站")]
        SysA_85 = 0185,
        [Description("A系列系统的第86个子网站")]
        SysA_86 = 0186,
        [Description("A系列系统的第87个子网站")]
        SysA_87 = 0187,
        [Description("A系列系统的第88个子网站")]
        SysA_88 = 0188,
        [Description("A系列系统的第89个子网站")]
        SysA_89 = 0189,
        [Description("A系列系统的第90个子网站")]
        SysA_90 = 0190,
        [Description("A系列系统的第91个子网站")]
        SysA_91 = 0191,
        [Description("A系列系统的第92个子网站")]
        SysA_92 = 0192,
        [Description("A系列系统的第93个子网站")]
        SysA_93 = 0193,
        [Description("A系列系统的第94个子网站")]
        SysA_94 = 0194,
        [Description("A系列系统的第95个子网站")]
        SysA_95 = 0195,
        [Description("A系列系统的第96个子网站")]
        SysA_96 = 0196,
        [Description("A系列系统的第97个子网站")]
        SysA_97 = 0197,
        [Description("A系列系统的第98个子网站")]
        SysA_98 = 0198,
        [Description("A系列系统的第99个子网站")]
        SysA_99 = 0199,
        [Description("B系列系统的第1个子网站")]
        SysB_01 = 0201,
        [Description("B系列系统的第2个子网站")]
        SysB_02 = 0202,
        [Description("B系列系统的第3个子网站")]
        SysB_03 = 0203,
        [Description("B系列系统的第4个子网站")]
        SysB_04 = 0204,
        [Description("B系列系统的第5个子网站")]
        SysB_05 = 0205,
        [Description("B系列系统的第6个子网站")]
        SysB_06 = 0206,
        [Description("B系列系统的第7个子网站")]
        SysB_07 = 0207,
        [Description("B系列系统的第8个子网站")]
        SysB_08 = 0208,
        [Description("B系列系统的第9个子网站")]
        SysB_09 = 0209,
        [Description("B系列系统的第10个子网站")]
        SysB_10 = 0210,
        [Description("B系列系统的第11个子网站")]
        SysB_11 = 0211,
        [Description("B系列系统的第12个子网站")]
        SysB_12 = 0212,
        [Description("B系列系统的第13个子网站")]
        SysB_13 = 0213,
        [Description("B系列系统的第14个子网站")]
        SysB_14 = 0214,
        [Description("B系列系统的第15个子网站")]
        SysB_15 = 0215,
        [Description("B系列系统的第16个子网站")]
        SysB_16 = 0216,
        [Description("B系列系统的第17个子网站")]
        SysB_17 = 0217,
        [Description("B系列系统的第18个子网站")]
        SysB_18 = 0218,
        [Description("B系列系统的第19个子网站")]
        SysB_19 = 0219,
        [Description("B系列系统的第20个子网站")]
        SysB_20 = 0220,
        [Description("B系列系统的第21个子网站")]
        SysB_21 = 0221,
        [Description("B系列系统的第22个子网站")]
        SysB_22 = 0222,
        [Description("B系列系统的第23个子网站")]
        SysB_23 = 0223,
        [Description("B系列系统的第24个子网站")]
        SysB_24 = 0224,
        [Description("B系列系统的第25个子网站")]
        SysB_25 = 0225,
        [Description("B系列系统的第26个子网站")]
        SysB_26 = 0226,
        [Description("B系列系统的第27个子网站")]
        SysB_27 = 0227,
        [Description("B系列系统的第28个子网站")]
        SysB_28 = 0228,
        [Description("B系列系统的第29个子网站")]
        SysB_29 = 0229,
        [Description("B系列系统的第30个子网站")]
        SysB_30 = 0230,
        [Description("B系列系统的第31个子网站")]
        SysB_31 = 0231,
        [Description("B系列系统的第32个子网站")]
        SysB_32 = 0232,
        [Description("B系列系统的第33个子网站")]
        SysB_33 = 0233,
        [Description("B系列系统的第34个子网站")]
        SysB_34 = 0234,
        [Description("B系列系统的第35个子网站")]
        SysB_35 = 0235,
        [Description("B系列系统的第36个子网站")]
        SysB_36 = 0236,
        [Description("B系列系统的第37个子网站")]
        SysB_37 = 0237,
        [Description("B系列系统的第38个子网站")]
        SysB_38 = 0238,
        [Description("B系列系统的第39个子网站")]
        SysB_39 = 0239,
        [Description("B系列系统的第40个子网站")]
        SysB_40 = 0240,
        [Description("B系列系统的第41个子网站")]
        SysB_41 = 0241,
        [Description("B系列系统的第42个子网站")]
        SysB_42 = 0242,
        [Description("B系列系统的第43个子网站")]
        SysB_43 = 0243,
        [Description("B系列系统的第44个子网站")]
        SysB_44 = 0244,
        [Description("B系列系统的第45个子网站")]
        SysB_45 = 0245,
        [Description("B系列系统的第46个子网站")]
        SysB_46 = 0246,
        [Description("B系列系统的第47个子网站")]
        SysB_47 = 0247,
        [Description("B系列系统的第48个子网站")]
        SysB_48 = 0248,
        [Description("B系列系统的第49个子网站")]
        SysB_49 = 0249,
        [Description("B系列系统的第50个子网站")]
        SysB_50 = 0250,
        [Description("B系列系统的第51个子网站")]
        SysB_51 = 0251,
        [Description("B系列系统的第52个子网站")]
        SysB_52 = 0252,
        [Description("B系列系统的第53个子网站")]
        SysB_53 = 0253,
        [Description("B系列系统的第54个子网站")]
        SysB_54 = 0254,
        [Description("B系列系统的第55个子网站")]
        SysB_55 = 0255,
        [Description("B系列系统的第56个子网站")]
        SysB_56 = 0256,
        [Description("B系列系统的第57个子网站")]
        SysB_57 = 0257,
        [Description("B系列系统的第58个子网站")]
        SysB_58 = 0258,
        [Description("B系列系统的第59个子网站")]
        SysB_59 = 0259,
        [Description("B系列系统的第60个子网站")]
        SysB_60 = 0260,
        [Description("B系列系统的第61个子网站")]
        SysB_61 = 0261,
        [Description("B系列系统的第62个子网站")]
        SysB_62 = 0262,
        [Description("B系列系统的第63个子网站")]
        SysB_63 = 0263,
        [Description("B系列系统的第64个子网站")]
        SysB_64 = 0264,
        [Description("B系列系统的第65个子网站")]
        SysB_65 = 0265,
        [Description("B系列系统的第66个子网站")]
        SysB_66 = 0266,
        [Description("B系列系统的第67个子网站")]
        SysB_67 = 0267,
        [Description("B系列系统的第68个子网站")]
        SysB_68 = 0268,
        [Description("B系列系统的第69个子网站")]
        SysB_69 = 0269,
        [Description("B系列系统的第70个子网站")]
        SysB_70 = 0270,
        [Description("B系列系统的第71个子网站")]
        SysB_71 = 0271,
        [Description("B系列系统的第72个子网站")]
        SysB_72 = 0272,
        [Description("B系列系统的第73个子网站")]
        SysB_73 = 0273,
        [Description("B系列系统的第74个子网站")]
        SysB_74 = 0274,
        [Description("B系列系统的第75个子网站")]
        SysB_75 = 0275,
        [Description("B系列系统的第76个子网站")]
        SysB_76 = 0276,
        [Description("B系列系统的第77个子网站")]
        SysB_77 = 0277,
        [Description("B系列系统的第78个子网站")]
        SysB_78 = 0278,
        [Description("B系列系统的第79个子网站")]
        SysB_79 = 0279,
        [Description("B系列系统的第80个子网站")]
        SysB_80 = 0280,
        [Description("B系列系统的第81个子网站")]
        SysB_81 = 0281,
        [Description("B系列系统的第82个子网站")]
        SysB_82 = 0282,
        [Description("B系列系统的第83个子网站")]
        SysB_83 = 0283,
        [Description("B系列系统的第84个子网站")]
        SysB_84 = 0284,
        [Description("B系列系统的第85个子网站")]
        SysB_85 = 0285,
        [Description("B系列系统的第86个子网站")]
        SysB_86 = 0286,
        [Description("B系列系统的第87个子网站")]
        SysB_87 = 0287,
        [Description("B系列系统的第88个子网站")]
        SysB_88 = 0288,
        [Description("B系列系统的第89个子网站")]
        SysB_89 = 0289,
        [Description("B系列系统的第90个子网站")]
        SysB_90 = 0290,
        [Description("B系列系统的第91个子网站")]
        SysB_91 = 0291,
        [Description("B系列系统的第92个子网站")]
        SysB_92 = 0292,
        [Description("B系列系统的第93个子网站")]
        SysB_93 = 0293,
        [Description("B系列系统的第94个子网站")]
        SysB_94 = 0294,
        [Description("B系列系统的第95个子网站")]
        SysB_95 = 0295,
        [Description("B系列系统的第96个子网站")]
        SysB_96 = 0296,
        [Description("B系列系统的第97个子网站")]
        SysB_97 = 0297,
        [Description("B系列系统的第98个子网站")]
        SysB_98 = 0298,
        [Description("B系列系统的第99个子网站")]
        SysB_99 = 0299,
        [Description("C系列系统的第1个子网站")]
        SysC_01 = 0301,
        [Description("C系列系统的第2个子网站")]
        SysC_02 = 0302,
        [Description("C系列系统的第3个子网站")]
        SysC_03 = 0303,
        [Description("C系列系统的第4个子网站")]
        SysC_04 = 0304,
        [Description("C系列系统的第5个子网站")]
        SysC_05 = 0305,
        [Description("C系列系统的第6个子网站")]
        SysC_06 = 0306,
        [Description("C系列系统的第7个子网站")]
        SysC_07 = 0307,
        [Description("C系列系统的第8个子网站")]
        SysC_08 = 0308,
        [Description("C系列系统的第9个子网站")]
        SysC_09 = 0309,
        [Description("C系列系统的第10个子网站")]
        SysC_10 = 0310,
        [Description("C系列系统的第11个子网站")]
        SysC_11 = 0311,
        [Description("C系列系统的第12个子网站")]
        SysC_12 = 0312,
        [Description("C系列系统的第13个子网站")]
        SysC_13 = 0313,
        [Description("C系列系统的第14个子网站")]
        SysC_14 = 0314,
        [Description("C系列系统的第15个子网站")]
        SysC_15 = 0315,
        [Description("C系列系统的第16个子网站")]
        SysC_16 = 0316,
        [Description("C系列系统的第17个子网站")]
        SysC_17 = 0317,
        [Description("C系列系统的第18个子网站")]
        SysC_18 = 0318,
        [Description("C系列系统的第19个子网站")]
        SysC_19 = 0319,
        [Description("C系列系统的第20个子网站")]
        SysC_20 = 0320,
        [Description("C系列系统的第21个子网站")]
        SysC_21 = 0321,
        [Description("C系列系统的第22个子网站")]
        SysC_22 = 0322,
        [Description("C系列系统的第23个子网站")]
        SysC_23 = 0323,
        [Description("C系列系统的第24个子网站")]
        SysC_24 = 0324,
        [Description("C系列系统的第25个子网站")]
        SysC_25 = 0325,
        [Description("C系列系统的第26个子网站")]
        SysC_26 = 0326,
        [Description("C系列系统的第27个子网站")]
        SysC_27 = 0327,
        [Description("C系列系统的第28个子网站")]
        SysC_28 = 0328,
        [Description("C系列系统的第29个子网站")]
        SysC_29 = 0329,
        [Description("C系列系统的第30个子网站")]
        SysC_30 = 0330,
        [Description("C系列系统的第31个子网站")]
        SysC_31 = 0331,
        [Description("C系列系统的第32个子网站")]
        SysC_32 = 0332,
        [Description("C系列系统的第33个子网站")]
        SysC_33 = 0333,
        [Description("C系列系统的第34个子网站")]
        SysC_34 = 0334,
        [Description("C系列系统的第35个子网站")]
        SysC_35 = 0335,
        [Description("C系列系统的第36个子网站")]
        SysC_36 = 0336,
        [Description("C系列系统的第37个子网站")]
        SysC_37 = 0337,
        [Description("C系列系统的第38个子网站")]
        SysC_38 = 0338,
        [Description("C系列系统的第39个子网站")]
        SysC_39 = 0339,
        [Description("C系列系统的第40个子网站")]
        SysC_40 = 0340,
        [Description("C系列系统的第41个子网站")]
        SysC_41 = 0341,
        [Description("C系列系统的第42个子网站")]
        SysC_42 = 0342,
        [Description("C系列系统的第43个子网站")]
        SysC_43 = 0343,
        [Description("C系列系统的第44个子网站")]
        SysC_44 = 0344,
        [Description("C系列系统的第45个子网站")]
        SysC_45 = 0345,
        [Description("C系列系统的第46个子网站")]
        SysC_46 = 0346,
        [Description("C系列系统的第47个子网站")]
        SysC_47 = 0347,
        [Description("C系列系统的第48个子网站")]
        SysC_48 = 0348,
        [Description("C系列系统的第49个子网站")]
        SysC_49 = 0349,
        [Description("C系列系统的第50个子网站")]
        SysC_50 = 0350,
        [Description("C系列系统的第51个子网站")]
        SysC_51 = 0351,
        [Description("C系列系统的第52个子网站")]
        SysC_52 = 0352,
        [Description("C系列系统的第53个子网站")]
        SysC_53 = 0353,
        [Description("C系列系统的第54个子网站")]
        SysC_54 = 0354,
        [Description("C系列系统的第55个子网站")]
        SysC_55 = 0355,
        [Description("C系列系统的第56个子网站")]
        SysC_56 = 0356,
        [Description("C系列系统的第57个子网站")]
        SysC_57 = 0357,
        [Description("C系列系统的第58个子网站")]
        SysC_58 = 0358,
        [Description("C系列系统的第59个子网站")]
        SysC_59 = 0359,
        [Description("C系列系统的第60个子网站")]
        SysC_60 = 0360,
        [Description("C系列系统的第61个子网站")]
        SysC_61 = 0361,
        [Description("C系列系统的第62个子网站")]
        SysC_62 = 0362,
        [Description("C系列系统的第63个子网站")]
        SysC_63 = 0363,
        [Description("C系列系统的第64个子网站")]
        SysC_64 = 0364,
        [Description("C系列系统的第65个子网站")]
        SysC_65 = 0365,
        [Description("C系列系统的第66个子网站")]
        SysC_66 = 0366,
        [Description("C系列系统的第67个子网站")]
        SysC_67 = 0367,
        [Description("C系列系统的第68个子网站")]
        SysC_68 = 0368,
        [Description("C系列系统的第69个子网站")]
        SysC_69 = 0369,
        [Description("C系列系统的第70个子网站")]
        SysC_70 = 0370,
        [Description("C系列系统的第71个子网站")]
        SysC_71 = 0371,
        [Description("C系列系统的第72个子网站")]
        SysC_72 = 0372,
        [Description("C系列系统的第73个子网站")]
        SysC_73 = 0373,
        [Description("C系列系统的第74个子网站")]
        SysC_74 = 0374,
        [Description("C系列系统的第75个子网站")]
        SysC_75 = 0375,
        [Description("C系列系统的第76个子网站")]
        SysC_76 = 0376,
        [Description("C系列系统的第77个子网站")]
        SysC_77 = 0377,
        [Description("C系列系统的第78个子网站")]
        SysC_78 = 0378,
        [Description("C系列系统的第79个子网站")]
        SysC_79 = 0379,
        [Description("C系列系统的第80个子网站")]
        SysC_80 = 0380,
        [Description("C系列系统的第81个子网站")]
        SysC_81 = 0381,
        [Description("C系列系统的第82个子网站")]
        SysC_82 = 0382,
        [Description("C系列系统的第83个子网站")]
        SysC_83 = 0383,
        [Description("C系列系统的第84个子网站")]
        SysC_84 = 0384,
        [Description("C系列系统的第85个子网站")]
        SysC_85 = 0385,
        [Description("C系列系统的第86个子网站")]
        SysC_86 = 0386,
        [Description("C系列系统的第87个子网站")]
        SysC_87 = 0387,
        [Description("C系列系统的第88个子网站")]
        SysC_88 = 0388,
        [Description("C系列系统的第89个子网站")]
        SysC_89 = 0389,
        [Description("C系列系统的第90个子网站")]
        SysC_90 = 0390,
        [Description("C系列系统的第91个子网站")]
        SysC_91 = 0391,
        [Description("C系列系统的第92个子网站")]
        SysC_92 = 0392,
        [Description("C系列系统的第93个子网站")]
        SysC_93 = 0393,
        [Description("C系列系统的第94个子网站")]
        SysC_94 = 0394,
        [Description("C系列系统的第95个子网站")]
        SysC_95 = 0395,
        [Description("C系列系统的第96个子网站")]
        SysC_96 = 0396,
        [Description("C系列系统的第97个子网站")]
        SysC_97 = 0397,
        [Description("C系列系统的第98个子网站")]
        SysC_98 = 0398,
        [Description("C系列系统的第99个子网站")]
        SysC_99 = 0399,
        [Description("D系列系统的第1个子网站")]
        SysD_01 = 0401,
        [Description("D系列系统的第2个子网站")]
        SysD_02 = 0402,
        [Description("D系列系统的第3个子网站")]
        SysD_03 = 0403,
        [Description("D系列系统的第4个子网站")]
        SysD_04 = 0404,
        [Description("D系列系统的第5个子网站")]
        SysD_05 = 0405,
        [Description("D系列系统的第6个子网站")]
        SysD_06 = 0406,
        [Description("D系列系统的第7个子网站")]
        SysD_07 = 0407,
        [Description("D系列系统的第8个子网站")]
        SysD_08 = 0408,
        [Description("D系列系统的第9个子网站")]
        SysD_09 = 0409,
        [Description("D系列系统的第10个子网站")]
        SysD_10 = 0410,
        [Description("D系列系统的第11个子网站")]
        SysD_11 = 0411,
        [Description("D系列系统的第12个子网站")]
        SysD_12 = 0412,
        [Description("D系列系统的第13个子网站")]
        SysD_13 = 0413,
        [Description("D系列系统的第14个子网站")]
        SysD_14 = 0414,
        [Description("D系列系统的第15个子网站")]
        SysD_15 = 0415,
        [Description("D系列系统的第16个子网站")]
        SysD_16 = 0416,
        [Description("D系列系统的第17个子网站")]
        SysD_17 = 0417,
        [Description("D系列系统的第18个子网站")]
        SysD_18 = 0418,
        [Description("D系列系统的第19个子网站")]
        SysD_19 = 0419,
        [Description("D系列系统的第20个子网站")]
        SysD_20 = 0420,
        [Description("D系列系统的第21个子网站")]
        SysD_21 = 0421,
        [Description("D系列系统的第22个子网站")]
        SysD_22 = 0422,
        [Description("D系列系统的第23个子网站")]
        SysD_23 = 0423,
        [Description("D系列系统的第24个子网站")]
        SysD_24 = 0424,
        [Description("D系列系统的第25个子网站")]
        SysD_25 = 0425,
        [Description("D系列系统的第26个子网站")]
        SysD_26 = 0426,
        [Description("D系列系统的第27个子网站")]
        SysD_27 = 0427,
        [Description("D系列系统的第28个子网站")]
        SysD_28 = 0428,
        [Description("D系列系统的第29个子网站")]
        SysD_29 = 0429,
        [Description("D系列系统的第30个子网站")]
        SysD_30 = 0430,
        [Description("D系列系统的第31个子网站")]
        SysD_31 = 0431,
        [Description("D系列系统的第32个子网站")]
        SysD_32 = 0432,
        [Description("D系列系统的第33个子网站")]
        SysD_33 = 0433,
        [Description("D系列系统的第34个子网站")]
        SysD_34 = 0434,
        [Description("D系列系统的第35个子网站")]
        SysD_35 = 0435,
        [Description("D系列系统的第36个子网站")]
        SysD_36 = 0436,
        [Description("D系列系统的第37个子网站")]
        SysD_37 = 0437,
        [Description("D系列系统的第38个子网站")]
        SysD_38 = 0438,
        [Description("D系列系统的第39个子网站")]
        SysD_39 = 0439,
        [Description("D系列系统的第40个子网站")]
        SysD_40 = 0440,
        [Description("D系列系统的第41个子网站")]
        SysD_41 = 0441,
        [Description("D系列系统的第42个子网站")]
        SysD_42 = 0442,
        [Description("D系列系统的第43个子网站")]
        SysD_43 = 0443,
        [Description("D系列系统的第44个子网站")]
        SysD_44 = 0444,
        [Description("D系列系统的第45个子网站")]
        SysD_45 = 0445,
        [Description("D系列系统的第46个子网站")]
        SysD_46 = 0446,
        [Description("D系列系统的第47个子网站")]
        SysD_47 = 0447,
        [Description("D系列系统的第48个子网站")]
        SysD_48 = 0448,
        [Description("D系列系统的第49个子网站")]
        SysD_49 = 0449,
        [Description("D系列系统的第50个子网站")]
        SysD_50 = 0450,
        [Description("D系列系统的第51个子网站")]
        SysD_51 = 0451,
        [Description("D系列系统的第52个子网站")]
        SysD_52 = 0452,
        [Description("D系列系统的第53个子网站")]
        SysD_53 = 0453,
        [Description("D系列系统的第54个子网站")]
        SysD_54 = 0454,
        [Description("D系列系统的第55个子网站")]
        SysD_55 = 0455,
        [Description("D系列系统的第56个子网站")]
        SysD_56 = 0456,
        [Description("D系列系统的第57个子网站")]
        SysD_57 = 0457,
        [Description("D系列系统的第58个子网站")]
        SysD_58 = 0458,
        [Description("D系列系统的第59个子网站")]
        SysD_59 = 0459,
        [Description("D系列系统的第60个子网站")]
        SysD_60 = 0460,
        [Description("D系列系统的第61个子网站")]
        SysD_61 = 0461,
        [Description("D系列系统的第62个子网站")]
        SysD_62 = 0462,
        [Description("D系列系统的第63个子网站")]
        SysD_63 = 0463,
        [Description("D系列系统的第64个子网站")]
        SysD_64 = 0464,
        [Description("D系列系统的第65个子网站")]
        SysD_65 = 0465,
        [Description("D系列系统的第66个子网站")]
        SysD_66 = 0466,
        [Description("D系列系统的第67个子网站")]
        SysD_67 = 0467,
        [Description("D系列系统的第68个子网站")]
        SysD_68 = 0468,
        [Description("D系列系统的第69个子网站")]
        SysD_69 = 0469,
        [Description("D系列系统的第70个子网站")]
        SysD_70 = 0470,
        [Description("D系列系统的第71个子网站")]
        SysD_71 = 0471,
        [Description("D系列系统的第72个子网站")]
        SysD_72 = 0472,
        [Description("D系列系统的第73个子网站")]
        SysD_73 = 0473,
        [Description("D系列系统的第74个子网站")]
        SysD_74 = 0474,
        [Description("D系列系统的第75个子网站")]
        SysD_75 = 0475,
        [Description("D系列系统的第76个子网站")]
        SysD_76 = 0476,
        [Description("D系列系统的第77个子网站")]
        SysD_77 = 0477,
        [Description("D系列系统的第78个子网站")]
        SysD_78 = 0478,
        [Description("D系列系统的第79个子网站")]
        SysD_79 = 0479,
        [Description("D系列系统的第80个子网站")]
        SysD_80 = 0480,
        [Description("D系列系统的第81个子网站")]
        SysD_81 = 0481,
        [Description("D系列系统的第82个子网站")]
        SysD_82 = 0482,
        [Description("D系列系统的第83个子网站")]
        SysD_83 = 0483,
        [Description("D系列系统的第84个子网站")]
        SysD_84 = 0484,
        [Description("D系列系统的第85个子网站")]
        SysD_85 = 0485,
        [Description("D系列系统的第86个子网站")]
        SysD_86 = 0486,
        [Description("D系列系统的第87个子网站")]
        SysD_87 = 0487,
        [Description("D系列系统的第88个子网站")]
        SysD_88 = 0488,
        [Description("D系列系统的第89个子网站")]
        SysD_89 = 0489,
        [Description("D系列系统的第90个子网站")]
        SysD_90 = 0490,
        [Description("D系列系统的第91个子网站")]
        SysD_91 = 0491,
        [Description("D系列系统的第92个子网站")]
        SysD_92 = 0492,
        [Description("D系列系统的第93个子网站")]
        SysD_93 = 0493,
        [Description("D系列系统的第94个子网站")]
        SysD_94 = 0494,
        [Description("D系列系统的第95个子网站")]
        SysD_95 = 0495,
        [Description("D系列系统的第96个子网站")]
        SysD_96 = 0496,
        [Description("D系列系统的第97个子网站")]
        SysD_97 = 0497,
        [Description("D系列系统的第98个子网站")]
        SysD_98 = 0498,
        [Description("D系列系统的第99个子网站")]
        SysD_99 = 0499,
        [Description("E系列系统的第1个子网站")]
        SysE_01 = 0501,
        [Description("E系列系统的第2个子网站")]
        SysE_02 = 0502,
        [Description("E系列系统的第3个子网站")]
        SysE_03 = 0503,
        [Description("E系列系统的第4个子网站")]
        SysE_04 = 0504,
        [Description("E系列系统的第5个子网站")]
        SysE_05 = 0505,
        [Description("E系列系统的第6个子网站")]
        SysE_06 = 0506,
        [Description("E系列系统的第7个子网站")]
        SysE_07 = 0507,
        [Description("E系列系统的第8个子网站")]
        SysE_08 = 0508,
        [Description("E系列系统的第9个子网站")]
        SysE_09 = 0509,
        [Description("E系列系统的第10个子网站")]
        SysE_10 = 0510,
        [Description("E系列系统的第11个子网站")]
        SysE_11 = 0511,
        [Description("E系列系统的第12个子网站")]
        SysE_12 = 0512,
        [Description("E系列系统的第13个子网站")]
        SysE_13 = 0513,
        [Description("E系列系统的第14个子网站")]
        SysE_14 = 0514,
        [Description("E系列系统的第15个子网站")]
        SysE_15 = 0515,
        [Description("E系列系统的第16个子网站")]
        SysE_16 = 0516,
        [Description("E系列系统的第17个子网站")]
        SysE_17 = 0517,
        [Description("E系列系统的第18个子网站")]
        SysE_18 = 0518,
        [Description("E系列系统的第19个子网站")]
        SysE_19 = 0519,
        [Description("E系列系统的第20个子网站")]
        SysE_20 = 0520,
        [Description("E系列系统的第21个子网站")]
        SysE_21 = 0521,
        [Description("E系列系统的第22个子网站")]
        SysE_22 = 0522,
        [Description("E系列系统的第23个子网站")]
        SysE_23 = 0523,
        [Description("E系列系统的第24个子网站")]
        SysE_24 = 0524,
        [Description("E系列系统的第25个子网站")]
        SysE_25 = 0525,
        [Description("E系列系统的第26个子网站")]
        SysE_26 = 0526,
        [Description("E系列系统的第27个子网站")]
        SysE_27 = 0527,
        [Description("E系列系统的第28个子网站")]
        SysE_28 = 0528,
        [Description("E系列系统的第29个子网站")]
        SysE_29 = 0529,
        [Description("E系列系统的第30个子网站")]
        SysE_30 = 0530,
        [Description("E系列系统的第31个子网站")]
        SysE_31 = 0531,
        [Description("E系列系统的第32个子网站")]
        SysE_32 = 0532,
        [Description("E系列系统的第33个子网站")]
        SysE_33 = 0533,
        [Description("E系列系统的第34个子网站")]
        SysE_34 = 0534,
        [Description("E系列系统的第35个子网站")]
        SysE_35 = 0535,
        [Description("E系列系统的第36个子网站")]
        SysE_36 = 0536,
        [Description("E系列系统的第37个子网站")]
        SysE_37 = 0537,
        [Description("E系列系统的第38个子网站")]
        SysE_38 = 0538,
        [Description("E系列系统的第39个子网站")]
        SysE_39 = 0539,
        [Description("E系列系统的第40个子网站")]
        SysE_40 = 0540,
        [Description("E系列系统的第41个子网站")]
        SysE_41 = 0541,
        [Description("E系列系统的第42个子网站")]
        SysE_42 = 0542,
        [Description("E系列系统的第43个子网站")]
        SysE_43 = 0543,
        [Description("E系列系统的第44个子网站")]
        SysE_44 = 0544,
        [Description("E系列系统的第45个子网站")]
        SysE_45 = 0545,
        [Description("E系列系统的第46个子网站")]
        SysE_46 = 0546,
        [Description("E系列系统的第47个子网站")]
        SysE_47 = 0547,
        [Description("E系列系统的第48个子网站")]
        SysE_48 = 0548,
        [Description("E系列系统的第49个子网站")]
        SysE_49 = 0549,
        [Description("E系列系统的第50个子网站")]
        SysE_50 = 0550,
        [Description("E系列系统的第51个子网站")]
        SysE_51 = 0551,
        [Description("E系列系统的第52个子网站")]
        SysE_52 = 0552,
        [Description("E系列系统的第53个子网站")]
        SysE_53 = 0553,
        [Description("E系列系统的第54个子网站")]
        SysE_54 = 0554,
        [Description("E系列系统的第55个子网站")]
        SysE_55 = 0555,
        [Description("E系列系统的第56个子网站")]
        SysE_56 = 0556,
        [Description("E系列系统的第57个子网站")]
        SysE_57 = 0557,
        [Description("E系列系统的第58个子网站")]
        SysE_58 = 0558,
        [Description("E系列系统的第59个子网站")]
        SysE_59 = 0559,
        [Description("E系列系统的第60个子网站")]
        SysE_60 = 0560,
        [Description("E系列系统的第61个子网站")]
        SysE_61 = 0561,
        [Description("E系列系统的第62个子网站")]
        SysE_62 = 0562,
        [Description("E系列系统的第63个子网站")]
        SysE_63 = 0563,
        [Description("E系列系统的第64个子网站")]
        SysE_64 = 0564,
        [Description("E系列系统的第65个子网站")]
        SysE_65 = 0565,
        [Description("E系列系统的第66个子网站")]
        SysE_66 = 0566,
        [Description("E系列系统的第67个子网站")]
        SysE_67 = 0567,
        [Description("E系列系统的第68个子网站")]
        SysE_68 = 0568,
        [Description("E系列系统的第69个子网站")]
        SysE_69 = 0569,
        [Description("E系列系统的第70个子网站")]
        SysE_70 = 0570,
        [Description("E系列系统的第71个子网站")]
        SysE_71 = 0571,
        [Description("E系列系统的第72个子网站")]
        SysE_72 = 0572,
        [Description("E系列系统的第73个子网站")]
        SysE_73 = 0573,
        [Description("E系列系统的第74个子网站")]
        SysE_74 = 0574,
        [Description("E系列系统的第75个子网站")]
        SysE_75 = 0575,
        [Description("E系列系统的第76个子网站")]
        SysE_76 = 0576,
        [Description("E系列系统的第77个子网站")]
        SysE_77 = 0577,
        [Description("E系列系统的第78个子网站")]
        SysE_78 = 0578,
        [Description("E系列系统的第79个子网站")]
        SysE_79 = 0579,
        [Description("E系列系统的第80个子网站")]
        SysE_80 = 0580,
        [Description("E系列系统的第81个子网站")]
        SysE_81 = 0581,
        [Description("E系列系统的第82个子网站")]
        SysE_82 = 0582,
        [Description("E系列系统的第83个子网站")]
        SysE_83 = 0583,
        [Description("E系列系统的第84个子网站")]
        SysE_84 = 0584,
        [Description("E系列系统的第85个子网站")]
        SysE_85 = 0585,
        [Description("E系列系统的第86个子网站")]
        SysE_86 = 0586,
        [Description("E系列系统的第87个子网站")]
        SysE_87 = 0587,
        [Description("E系列系统的第88个子网站")]
        SysE_88 = 0588,
        [Description("E系列系统的第89个子网站")]
        SysE_89 = 0589,
        [Description("E系列系统的第90个子网站")]
        SysE_90 = 0590,
        [Description("E系列系统的第91个子网站")]
        SysE_91 = 0591,
        [Description("E系列系统的第92个子网站")]
        SysE_92 = 0592,
        [Description("E系列系统的第93个子网站")]
        SysE_93 = 0593,
        [Description("E系列系统的第94个子网站")]
        SysE_94 = 0594,
        [Description("E系列系统的第95个子网站")]
        SysE_95 = 0595,
        [Description("E系列系统的第96个子网站")]
        SysE_96 = 0596,
        [Description("E系列系统的第97个子网站")]
        SysE_97 = 0597,
        [Description("E系列系统的第98个子网站")]
        SysE_98 = 0598,
        [Description("E系列系统的第99个子网站")]
        SysE_99 = 0599,
        [Description("F系列系统的第1个子网站")]
        SysF_01 = 0601,
        [Description("F系列系统的第2个子网站")]
        SysF_02 = 0602,
        [Description("F系列系统的第3个子网站")]
        SysF_03 = 0603,
        [Description("F系列系统的第4个子网站")]
        SysF_04 = 0604,
        [Description("F系列系统的第5个子网站")]
        SysF_05 = 0605,
        [Description("F系列系统的第6个子网站")]
        SysF_06 = 0606,
        [Description("F系列系统的第7个子网站")]
        SysF_07 = 0607,
        [Description("F系列系统的第8个子网站")]
        SysF_08 = 0608,
        [Description("F系列系统的第9个子网站")]
        SysF_09 = 0609,
        [Description("F系列系统的第10个子网站")]
        SysF_10 = 0610,
        [Description("F系列系统的第11个子网站")]
        SysF_11 = 0611,
        [Description("F系列系统的第12个子网站")]
        SysF_12 = 0612,
        [Description("F系列系统的第13个子网站")]
        SysF_13 = 0613,
        [Description("F系列系统的第14个子网站")]
        SysF_14 = 0614,
        [Description("F系列系统的第15个子网站")]
        SysF_15 = 0615,
        [Description("F系列系统的第16个子网站")]
        SysF_16 = 0616,
        [Description("F系列系统的第17个子网站")]
        SysF_17 = 0617,
        [Description("F系列系统的第18个子网站")]
        SysF_18 = 0618,
        [Description("F系列系统的第19个子网站")]
        SysF_19 = 0619,
        [Description("F系列系统的第20个子网站")]
        SysF_20 = 0620,
        [Description("F系列系统的第21个子网站")]
        SysF_21 = 0621,
        [Description("F系列系统的第22个子网站")]
        SysF_22 = 0622,
        [Description("F系列系统的第23个子网站")]
        SysF_23 = 0623,
        [Description("F系列系统的第24个子网站")]
        SysF_24 = 0624,
        [Description("F系列系统的第25个子网站")]
        SysF_25 = 0625,
        [Description("F系列系统的第26个子网站")]
        SysF_26 = 0626,
        [Description("F系列系统的第27个子网站")]
        SysF_27 = 0627,
        [Description("F系列系统的第28个子网站")]
        SysF_28 = 0628,
        [Description("F系列系统的第29个子网站")]
        SysF_29 = 0629,
        [Description("F系列系统的第30个子网站")]
        SysF_30 = 0630,
        [Description("F系列系统的第31个子网站")]
        SysF_31 = 0631,
        [Description("F系列系统的第32个子网站")]
        SysF_32 = 0632,
        [Description("F系列系统的第33个子网站")]
        SysF_33 = 0633,
        [Description("F系列系统的第34个子网站")]
        SysF_34 = 0634,
        [Description("F系列系统的第35个子网站")]
        SysF_35 = 0635,
        [Description("F系列系统的第36个子网站")]
        SysF_36 = 0636,
        [Description("F系列系统的第37个子网站")]
        SysF_37 = 0637,
        [Description("F系列系统的第38个子网站")]
        SysF_38 = 0638,
        [Description("F系列系统的第39个子网站")]
        SysF_39 = 0639,
        [Description("F系列系统的第40个子网站")]
        SysF_40 = 0640,
        [Description("F系列系统的第41个子网站")]
        SysF_41 = 0641,
        [Description("F系列系统的第42个子网站")]
        SysF_42 = 0642,
        [Description("F系列系统的第43个子网站")]
        SysF_43 = 0643,
        [Description("F系列系统的第44个子网站")]
        SysF_44 = 0644,
        [Description("F系列系统的第45个子网站")]
        SysF_45 = 0645,
        [Description("F系列系统的第46个子网站")]
        SysF_46 = 0646,
        [Description("F系列系统的第47个子网站")]
        SysF_47 = 0647,
        [Description("F系列系统的第48个子网站")]
        SysF_48 = 0648,
        [Description("F系列系统的第49个子网站")]
        SysF_49 = 0649,
        [Description("F系列系统的第50个子网站")]
        SysF_50 = 0650,
        [Description("F系列系统的第51个子网站")]
        SysF_51 = 0651,
        [Description("F系列系统的第52个子网站")]
        SysF_52 = 0652,
        [Description("F系列系统的第53个子网站")]
        SysF_53 = 0653,
        [Description("F系列系统的第54个子网站")]
        SysF_54 = 0654,
        [Description("F系列系统的第55个子网站")]
        SysF_55 = 0655,
        [Description("F系列系统的第56个子网站")]
        SysF_56 = 0656,
        [Description("F系列系统的第57个子网站")]
        SysF_57 = 0657,
        [Description("F系列系统的第58个子网站")]
        SysF_58 = 0658,
        [Description("F系列系统的第59个子网站")]
        SysF_59 = 0659,
        [Description("F系列系统的第60个子网站")]
        SysF_60 = 0660,
        [Description("F系列系统的第61个子网站")]
        SysF_61 = 0661,
        [Description("F系列系统的第62个子网站")]
        SysF_62 = 0662,
        [Description("F系列系统的第63个子网站")]
        SysF_63 = 0663,
        [Description("F系列系统的第64个子网站")]
        SysF_64 = 0664,
        [Description("F系列系统的第65个子网站")]
        SysF_65 = 0665,
        [Description("F系列系统的第66个子网站")]
        SysF_66 = 0666,
        [Description("F系列系统的第67个子网站")]
        SysF_67 = 0667,
        [Description("F系列系统的第68个子网站")]
        SysF_68 = 0668,
        [Description("F系列系统的第69个子网站")]
        SysF_69 = 0669,
        [Description("F系列系统的第70个子网站")]
        SysF_70 = 0670,
        [Description("F系列系统的第71个子网站")]
        SysF_71 = 0671,
        [Description("F系列系统的第72个子网站")]
        SysF_72 = 0672,
        [Description("F系列系统的第73个子网站")]
        SysF_73 = 0673,
        [Description("F系列系统的第74个子网站")]
        SysF_74 = 0674,
        [Description("F系列系统的第75个子网站")]
        SysF_75 = 0675,
        [Description("F系列系统的第76个子网站")]
        SysF_76 = 0676,
        [Description("F系列系统的第77个子网站")]
        SysF_77 = 0677,
        [Description("F系列系统的第78个子网站")]
        SysF_78 = 0678,
        [Description("F系列系统的第79个子网站")]
        SysF_79 = 0679,
        [Description("F系列系统的第80个子网站")]
        SysF_80 = 0680,
        [Description("F系列系统的第81个子网站")]
        SysF_81 = 0681,
        [Description("F系列系统的第82个子网站")]
        SysF_82 = 0682,
        [Description("F系列系统的第83个子网站")]
        SysF_83 = 0683,
        [Description("F系列系统的第84个子网站")]
        SysF_84 = 0684,
        [Description("F系列系统的第85个子网站")]
        SysF_85 = 0685,
        [Description("F系列系统的第86个子网站")]
        SysF_86 = 0686,
        [Description("F系列系统的第87个子网站")]
        SysF_87 = 0687,
        [Description("F系列系统的第88个子网站")]
        SysF_88 = 0688,
        [Description("F系列系统的第89个子网站")]
        SysF_89 = 0689,
        [Description("F系列系统的第90个子网站")]
        SysF_90 = 0690,
        [Description("F系列系统的第91个子网站")]
        SysF_91 = 0691,
        [Description("F系列系统的第92个子网站")]
        SysF_92 = 0692,
        [Description("F系列系统的第93个子网站")]
        SysF_93 = 0693,
        [Description("F系列系统的第94个子网站")]
        SysF_94 = 0694,
        [Description("F系列系统的第95个子网站")]
        SysF_95 = 0695,
        [Description("F系列系统的第96个子网站")]
        SysF_96 = 0696,
        [Description("F系列系统的第97个子网站")]
        SysF_97 = 0697,
        [Description("F系列系统的第98个子网站")]
        SysF_98 = 0698,
        [Description("F系列系统的第99个子网站")]
        SysF_99 = 0699,
        [Description("G系列系统的第1个子网站")]
        SysG_01 = 0701,
        [Description("G系列系统的第2个子网站")]
        SysG_02 = 0702,
        [Description("G系列系统的第3个子网站")]
        SysG_03 = 0703,
        [Description("G系列系统的第4个子网站")]
        SysG_04 = 0704,
        [Description("G系列系统的第5个子网站")]
        SysG_05 = 0705,
        [Description("G系列系统的第6个子网站")]
        SysG_06 = 0706,
        [Description("G系列系统的第7个子网站")]
        SysG_07 = 0707,
        [Description("G系列系统的第8个子网站")]
        SysG_08 = 0708,
        [Description("G系列系统的第9个子网站")]
        SysG_09 = 0709,
        [Description("G系列系统的第10个子网站")]
        SysG_10 = 0710,
        [Description("G系列系统的第11个子网站")]
        SysG_11 = 0711,
        [Description("G系列系统的第12个子网站")]
        SysG_12 = 0712,
        [Description("G系列系统的第13个子网站")]
        SysG_13 = 0713,
        [Description("G系列系统的第14个子网站")]
        SysG_14 = 0714,
        [Description("G系列系统的第15个子网站")]
        SysG_15 = 0715,
        [Description("G系列系统的第16个子网站")]
        SysG_16 = 0716,
        [Description("G系列系统的第17个子网站")]
        SysG_17 = 0717,
        [Description("G系列系统的第18个子网站")]
        SysG_18 = 0718,
        [Description("G系列系统的第19个子网站")]
        SysG_19 = 0719,
        [Description("G系列系统的第20个子网站")]
        SysG_20 = 0720,
        [Description("G系列系统的第21个子网站")]
        SysG_21 = 0721,
        [Description("G系列系统的第22个子网站")]
        SysG_22 = 0722,
        [Description("G系列系统的第23个子网站")]
        SysG_23 = 0723,
        [Description("G系列系统的第24个子网站")]
        SysG_24 = 0724,
        [Description("G系列系统的第25个子网站")]
        SysG_25 = 0725,
        [Description("G系列系统的第26个子网站")]
        SysG_26 = 0726,
        [Description("G系列系统的第27个子网站")]
        SysG_27 = 0727,
        [Description("G系列系统的第28个子网站")]
        SysG_28 = 0728,
        [Description("G系列系统的第29个子网站")]
        SysG_29 = 0729,
        [Description("G系列系统的第30个子网站")]
        SysG_30 = 0730,
        [Description("G系列系统的第31个子网站")]
        SysG_31 = 0731,
        [Description("G系列系统的第32个子网站")]
        SysG_32 = 0732,
        [Description("G系列系统的第33个子网站")]
        SysG_33 = 0733,
        [Description("G系列系统的第34个子网站")]
        SysG_34 = 0734,
        [Description("G系列系统的第35个子网站")]
        SysG_35 = 0735,
        [Description("G系列系统的第36个子网站")]
        SysG_36 = 0736,
        [Description("G系列系统的第37个子网站")]
        SysG_37 = 0737,
        [Description("G系列系统的第38个子网站")]
        SysG_38 = 0738,
        [Description("G系列系统的第39个子网站")]
        SysG_39 = 0739,
        [Description("G系列系统的第40个子网站")]
        SysG_40 = 0740,
        [Description("G系列系统的第41个子网站")]
        SysG_41 = 0741,
        [Description("G系列系统的第42个子网站")]
        SysG_42 = 0742,
        [Description("G系列系统的第43个子网站")]
        SysG_43 = 0743,
        [Description("G系列系统的第44个子网站")]
        SysG_44 = 0744,
        [Description("G系列系统的第45个子网站")]
        SysG_45 = 0745,
        [Description("G系列系统的第46个子网站")]
        SysG_46 = 0746,
        [Description("G系列系统的第47个子网站")]
        SysG_47 = 0747,
        [Description("G系列系统的第48个子网站")]
        SysG_48 = 0748,
        [Description("G系列系统的第49个子网站")]
        SysG_49 = 0749,
        [Description("G系列系统的第50个子网站")]
        SysG_50 = 0750,
        [Description("G系列系统的第51个子网站")]
        SysG_51 = 0751,
        [Description("G系列系统的第52个子网站")]
        SysG_52 = 0752,
        [Description("G系列系统的第53个子网站")]
        SysG_53 = 0753,
        [Description("G系列系统的第54个子网站")]
        SysG_54 = 0754,
        [Description("G系列系统的第55个子网站")]
        SysG_55 = 0755,
        [Description("G系列系统的第56个子网站")]
        SysG_56 = 0756,
        [Description("G系列系统的第57个子网站")]
        SysG_57 = 0757,
        [Description("G系列系统的第58个子网站")]
        SysG_58 = 0758,
        [Description("G系列系统的第59个子网站")]
        SysG_59 = 0759,
        [Description("G系列系统的第60个子网站")]
        SysG_60 = 0760,
        [Description("G系列系统的第61个子网站")]
        SysG_61 = 0761,
        [Description("G系列系统的第62个子网站")]
        SysG_62 = 0762,
        [Description("G系列系统的第63个子网站")]
        SysG_63 = 0763,
        [Description("G系列系统的第64个子网站")]
        SysG_64 = 0764,
        [Description("G系列系统的第65个子网站")]
        SysG_65 = 0765,
        [Description("G系列系统的第66个子网站")]
        SysG_66 = 0766,
        [Description("G系列系统的第67个子网站")]
        SysG_67 = 0767,
        [Description("G系列系统的第68个子网站")]
        SysG_68 = 0768,
        [Description("G系列系统的第69个子网站")]
        SysG_69 = 0769,
        [Description("G系列系统的第70个子网站")]
        SysG_70 = 0770,
        [Description("G系列系统的第71个子网站")]
        SysG_71 = 0771,
        [Description("G系列系统的第72个子网站")]
        SysG_72 = 0772,
        [Description("G系列系统的第73个子网站")]
        SysG_73 = 0773,
        [Description("G系列系统的第74个子网站")]
        SysG_74 = 0774,
        [Description("G系列系统的第75个子网站")]
        SysG_75 = 0775,
        [Description("G系列系统的第76个子网站")]
        SysG_76 = 0776,
        [Description("G系列系统的第77个子网站")]
        SysG_77 = 0777,
        [Description("G系列系统的第78个子网站")]
        SysG_78 = 0778,
        [Description("G系列系统的第79个子网站")]
        SysG_79 = 0779,
        [Description("G系列系统的第80个子网站")]
        SysG_80 = 0780,
        [Description("G系列系统的第81个子网站")]
        SysG_81 = 0781,
        [Description("G系列系统的第82个子网站")]
        SysG_82 = 0782,
        [Description("G系列系统的第83个子网站")]
        SysG_83 = 0783,
        [Description("G系列系统的第84个子网站")]
        SysG_84 = 0784,
        [Description("G系列系统的第85个子网站")]
        SysG_85 = 0785,
        [Description("G系列系统的第86个子网站")]
        SysG_86 = 0786,
        [Description("G系列系统的第87个子网站")]
        SysG_87 = 0787,
        [Description("G系列系统的第88个子网站")]
        SysG_88 = 0788,
        [Description("G系列系统的第89个子网站")]
        SysG_89 = 0789,
        [Description("G系列系统的第90个子网站")]
        SysG_90 = 0790,
        [Description("G系列系统的第91个子网站")]
        SysG_91 = 0791,
        [Description("G系列系统的第92个子网站")]
        SysG_92 = 0792,
        [Description("G系列系统的第93个子网站")]
        SysG_93 = 0793,
        [Description("G系列系统的第94个子网站")]
        SysG_94 = 0794,
        [Description("G系列系统的第95个子网站")]
        SysG_95 = 0795,
        [Description("G系列系统的第96个子网站")]
        SysG_96 = 0796,
        [Description("G系列系统的第97个子网站")]
        SysG_97 = 0797,
        [Description("G系列系统的第98个子网站")]
        SysG_98 = 0798,
        [Description("G系列系统的第99个子网站")]
        SysG_99 = 0799,
        [Description("H系列系统的第1个子网站")]
        SysH_01 = 0801,
        [Description("H系列系统的第2个子网站")]
        SysH_02 = 0802,
        [Description("H系列系统的第3个子网站")]
        SysH_03 = 0803,
        [Description("H系列系统的第4个子网站")]
        SysH_04 = 0804,
        [Description("H系列系统的第5个子网站")]
        SysH_05 = 0805,
        [Description("H系列系统的第6个子网站")]
        SysH_06 = 0806,
        [Description("H系列系统的第7个子网站")]
        SysH_07 = 0807,
        [Description("H系列系统的第8个子网站")]
        SysH_08 = 0808,
        [Description("H系列系统的第9个子网站")]
        SysH_09 = 0809,
        [Description("H系列系统的第10个子网站")]
        SysH_10 = 0810,
        [Description("H系列系统的第11个子网站")]
        SysH_11 = 0811,
        [Description("H系列系统的第12个子网站")]
        SysH_12 = 0812,
        [Description("H系列系统的第13个子网站")]
        SysH_13 = 0813,
        [Description("H系列系统的第14个子网站")]
        SysH_14 = 0814,
        [Description("H系列系统的第15个子网站")]
        SysH_15 = 0815,
        [Description("H系列系统的第16个子网站")]
        SysH_16 = 0816,
        [Description("H系列系统的第17个子网站")]
        SysH_17 = 0817,
        [Description("H系列系统的第18个子网站")]
        SysH_18 = 0818,
        [Description("H系列系统的第19个子网站")]
        SysH_19 = 0819,
        [Description("H系列系统的第20个子网站")]
        SysH_20 = 0820,
        [Description("H系列系统的第21个子网站")]
        SysH_21 = 0821,
        [Description("H系列系统的第22个子网站")]
        SysH_22 = 0822,
        [Description("H系列系统的第23个子网站")]
        SysH_23 = 0823,
        [Description("H系列系统的第24个子网站")]
        SysH_24 = 0824,
        [Description("H系列系统的第25个子网站")]
        SysH_25 = 0825,
        [Description("H系列系统的第26个子网站")]
        SysH_26 = 0826,
        [Description("H系列系统的第27个子网站")]
        SysH_27 = 0827,
        [Description("H系列系统的第28个子网站")]
        SysH_28 = 0828,
        [Description("H系列系统的第29个子网站")]
        SysH_29 = 0829,
        [Description("H系列系统的第30个子网站")]
        SysH_30 = 0830,
        [Description("H系列系统的第31个子网站")]
        SysH_31 = 0831,
        [Description("H系列系统的第32个子网站")]
        SysH_32 = 0832,
        [Description("H系列系统的第33个子网站")]
        SysH_33 = 0833,
        [Description("H系列系统的第34个子网站")]
        SysH_34 = 0834,
        [Description("H系列系统的第35个子网站")]
        SysH_35 = 0835,
        [Description("H系列系统的第36个子网站")]
        SysH_36 = 0836,
        [Description("H系列系统的第37个子网站")]
        SysH_37 = 0837,
        [Description("H系列系统的第38个子网站")]
        SysH_38 = 0838,
        [Description("H系列系统的第39个子网站")]
        SysH_39 = 0839,
        [Description("H系列系统的第40个子网站")]
        SysH_40 = 0840,
        [Description("H系列系统的第41个子网站")]
        SysH_41 = 0841,
        [Description("H系列系统的第42个子网站")]
        SysH_42 = 0842,
        [Description("H系列系统的第43个子网站")]
        SysH_43 = 0843,
        [Description("H系列系统的第44个子网站")]
        SysH_44 = 0844,
        [Description("H系列系统的第45个子网站")]
        SysH_45 = 0845,
        [Description("H系列系统的第46个子网站")]
        SysH_46 = 0846,
        [Description("H系列系统的第47个子网站")]
        SysH_47 = 0847,
        [Description("H系列系统的第48个子网站")]
        SysH_48 = 0848,
        [Description("H系列系统的第49个子网站")]
        SysH_49 = 0849,
        [Description("H系列系统的第50个子网站")]
        SysH_50 = 0850,
        [Description("H系列系统的第51个子网站")]
        SysH_51 = 0851,
        [Description("H系列系统的第52个子网站")]
        SysH_52 = 0852,
        [Description("H系列系统的第53个子网站")]
        SysH_53 = 0853,
        [Description("H系列系统的第54个子网站")]
        SysH_54 = 0854,
        [Description("H系列系统的第55个子网站")]
        SysH_55 = 0855,
        [Description("H系列系统的第56个子网站")]
        SysH_56 = 0856,
        [Description("H系列系统的第57个子网站")]
        SysH_57 = 0857,
        [Description("H系列系统的第58个子网站")]
        SysH_58 = 0858,
        [Description("H系列系统的第59个子网站")]
        SysH_59 = 0859,
        [Description("H系列系统的第60个子网站")]
        SysH_60 = 0860,
        [Description("H系列系统的第61个子网站")]
        SysH_61 = 0861,
        [Description("H系列系统的第62个子网站")]
        SysH_62 = 0862,
        [Description("H系列系统的第63个子网站")]
        SysH_63 = 0863,
        [Description("H系列系统的第64个子网站")]
        SysH_64 = 0864,
        [Description("H系列系统的第65个子网站")]
        SysH_65 = 0865,
        [Description("H系列系统的第66个子网站")]
        SysH_66 = 0866,
        [Description("H系列系统的第67个子网站")]
        SysH_67 = 0867,
        [Description("H系列系统的第68个子网站")]
        SysH_68 = 0868,
        [Description("H系列系统的第69个子网站")]
        SysH_69 = 0869,
        [Description("H系列系统的第70个子网站")]
        SysH_70 = 0870,
        [Description("H系列系统的第71个子网站")]
        SysH_71 = 0871,
        [Description("H系列系统的第72个子网站")]
        SysH_72 = 0872,
        [Description("H系列系统的第73个子网站")]
        SysH_73 = 0873,
        [Description("H系列系统的第74个子网站")]
        SysH_74 = 0874,
        [Description("H系列系统的第75个子网站")]
        SysH_75 = 0875,
        [Description("H系列系统的第76个子网站")]
        SysH_76 = 0876,
        [Description("H系列系统的第77个子网站")]
        SysH_77 = 0877,
        [Description("H系列系统的第78个子网站")]
        SysH_78 = 0878,
        [Description("H系列系统的第79个子网站")]
        SysH_79 = 0879,
        [Description("H系列系统的第80个子网站")]
        SysH_80 = 0880,
        [Description("H系列系统的第81个子网站")]
        SysH_81 = 0881,
        [Description("H系列系统的第82个子网站")]
        SysH_82 = 0882,
        [Description("H系列系统的第83个子网站")]
        SysH_83 = 0883,
        [Description("H系列系统的第84个子网站")]
        SysH_84 = 0884,
        [Description("H系列系统的第85个子网站")]
        SysH_85 = 0885,
        [Description("H系列系统的第86个子网站")]
        SysH_86 = 0886,
        [Description("H系列系统的第87个子网站")]
        SysH_87 = 0887,
        [Description("H系列系统的第88个子网站")]
        SysH_88 = 0888,
        [Description("H系列系统的第89个子网站")]
        SysH_89 = 0889,
        [Description("H系列系统的第90个子网站")]
        SysH_90 = 0890,
        [Description("H系列系统的第91个子网站")]
        SysH_91 = 0891,
        [Description("H系列系统的第92个子网站")]
        SysH_92 = 0892,
        [Description("H系列系统的第93个子网站")]
        SysH_93 = 0893,
        [Description("H系列系统的第94个子网站")]
        SysH_94 = 0894,
        [Description("H系列系统的第95个子网站")]
        SysH_95 = 0895,
        [Description("H系列系统的第96个子网站")]
        SysH_96 = 0896,
        [Description("H系列系统的第97个子网站")]
        SysH_97 = 0897,
        [Description("H系列系统的第98个子网站")]
        SysH_98 = 0898,
        [Description("H系列系统的第99个子网站")]
        SysH_99 = 0899,
        [Description("I系列系统的第1个子网站")]
        SysI_01 = 0901,
        [Description("I系列系统的第2个子网站")]
        SysI_02 = 0902,
        [Description("I系列系统的第3个子网站")]
        SysI_03 = 0903,
        [Description("I系列系统的第4个子网站")]
        SysI_04 = 0904,
        [Description("I系列系统的第5个子网站")]
        SysI_05 = 0905,
        [Description("I系列系统的第6个子网站")]
        SysI_06 = 0906,
        [Description("I系列系统的第7个子网站")]
        SysI_07 = 0907,
        [Description("I系列系统的第8个子网站")]
        SysI_08 = 0908,
        [Description("I系列系统的第9个子网站")]
        SysI_09 = 0909,
        [Description("I系列系统的第10个子网站")]
        SysI_10 = 0910,
        [Description("I系列系统的第11个子网站")]
        SysI_11 = 0911,
        [Description("I系列系统的第12个子网站")]
        SysI_12 = 0912,
        [Description("I系列系统的第13个子网站")]
        SysI_13 = 0913,
        [Description("I系列系统的第14个子网站")]
        SysI_14 = 0914,
        [Description("I系列系统的第15个子网站")]
        SysI_15 = 0915,
        [Description("I系列系统的第16个子网站")]
        SysI_16 = 0916,
        [Description("I系列系统的第17个子网站")]
        SysI_17 = 0917,
        [Description("I系列系统的第18个子网站")]
        SysI_18 = 0918,
        [Description("I系列系统的第19个子网站")]
        SysI_19 = 0919,
        [Description("I系列系统的第20个子网站")]
        SysI_20 = 0920,
        [Description("I系列系统的第21个子网站")]
        SysI_21 = 0921,
        [Description("I系列系统的第22个子网站")]
        SysI_22 = 0922,
        [Description("I系列系统的第23个子网站")]
        SysI_23 = 0923,
        [Description("I系列系统的第24个子网站")]
        SysI_24 = 0924,
        [Description("I系列系统的第25个子网站")]
        SysI_25 = 0925,
        [Description("I系列系统的第26个子网站")]
        SysI_26 = 0926,
        [Description("I系列系统的第27个子网站")]
        SysI_27 = 0927,
        [Description("I系列系统的第28个子网站")]
        SysI_28 = 0928,
        [Description("I系列系统的第29个子网站")]
        SysI_29 = 0929,
        [Description("I系列系统的第30个子网站")]
        SysI_30 = 0930,
        [Description("I系列系统的第31个子网站")]
        SysI_31 = 0931,
        [Description("I系列系统的第32个子网站")]
        SysI_32 = 0932,
        [Description("I系列系统的第33个子网站")]
        SysI_33 = 0933,
        [Description("I系列系统的第34个子网站")]
        SysI_34 = 0934,
        [Description("I系列系统的第35个子网站")]
        SysI_35 = 0935,
        [Description("I系列系统的第36个子网站")]
        SysI_36 = 0936,
        [Description("I系列系统的第37个子网站")]
        SysI_37 = 0937,
        [Description("I系列系统的第38个子网站")]
        SysI_38 = 0938,
        [Description("I系列系统的第39个子网站")]
        SysI_39 = 0939,
        [Description("I系列系统的第40个子网站")]
        SysI_40 = 0940,
        [Description("I系列系统的第41个子网站")]
        SysI_41 = 0941,
        [Description("I系列系统的第42个子网站")]
        SysI_42 = 0942,
        [Description("I系列系统的第43个子网站")]
        SysI_43 = 0943,
        [Description("I系列系统的第44个子网站")]
        SysI_44 = 0944,
        [Description("I系列系统的第45个子网站")]
        SysI_45 = 0945,
        [Description("I系列系统的第46个子网站")]
        SysI_46 = 0946,
        [Description("I系列系统的第47个子网站")]
        SysI_47 = 0947,
        [Description("I系列系统的第48个子网站")]
        SysI_48 = 0948,
        [Description("I系列系统的第49个子网站")]
        SysI_49 = 0949,
        [Description("I系列系统的第50个子网站")]
        SysI_50 = 0950,
        [Description("I系列系统的第51个子网站")]
        SysI_51 = 0951,
        [Description("I系列系统的第52个子网站")]
        SysI_52 = 0952,
        [Description("I系列系统的第53个子网站")]
        SysI_53 = 0953,
        [Description("I系列系统的第54个子网站")]
        SysI_54 = 0954,
        [Description("I系列系统的第55个子网站")]
        SysI_55 = 0955,
        [Description("I系列系统的第56个子网站")]
        SysI_56 = 0956,
        [Description("I系列系统的第57个子网站")]
        SysI_57 = 0957,
        [Description("I系列系统的第58个子网站")]
        SysI_58 = 0958,
        [Description("I系列系统的第59个子网站")]
        SysI_59 = 0959,
        [Description("I系列系统的第60个子网站")]
        SysI_60 = 0960,
        [Description("I系列系统的第61个子网站")]
        SysI_61 = 0961,
        [Description("I系列系统的第62个子网站")]
        SysI_62 = 0962,
        [Description("I系列系统的第63个子网站")]
        SysI_63 = 0963,
        [Description("I系列系统的第64个子网站")]
        SysI_64 = 0964,
        [Description("I系列系统的第65个子网站")]
        SysI_65 = 0965,
        [Description("I系列系统的第66个子网站")]
        SysI_66 = 0966,
        [Description("I系列系统的第67个子网站")]
        SysI_67 = 0967,
        [Description("I系列系统的第68个子网站")]
        SysI_68 = 0968,
        [Description("I系列系统的第69个子网站")]
        SysI_69 = 0969,
        [Description("I系列系统的第70个子网站")]
        SysI_70 = 0970,
        [Description("I系列系统的第71个子网站")]
        SysI_71 = 0971,
        [Description("I系列系统的第72个子网站")]
        SysI_72 = 0972,
        [Description("I系列系统的第73个子网站")]
        SysI_73 = 0973,
        [Description("I系列系统的第74个子网站")]
        SysI_74 = 0974,
        [Description("I系列系统的第75个子网站")]
        SysI_75 = 0975,
        [Description("I系列系统的第76个子网站")]
        SysI_76 = 0976,
        [Description("I系列系统的第77个子网站")]
        SysI_77 = 0977,
        [Description("I系列系统的第78个子网站")]
        SysI_78 = 0978,
        [Description("I系列系统的第79个子网站")]
        SysI_79 = 0979,
        [Description("I系列系统的第80个子网站")]
        SysI_80 = 0980,
        [Description("I系列系统的第81个子网站")]
        SysI_81 = 0981,
        [Description("I系列系统的第82个子网站")]
        SysI_82 = 0982,
        [Description("I系列系统的第83个子网站")]
        SysI_83 = 0983,
        [Description("I系列系统的第84个子网站")]
        SysI_84 = 0984,
        [Description("I系列系统的第85个子网站")]
        SysI_85 = 0985,
        [Description("I系列系统的第86个子网站")]
        SysI_86 = 0986,
        [Description("I系列系统的第87个子网站")]
        SysI_87 = 0987,
        [Description("I系列系统的第88个子网站")]
        SysI_88 = 0988,
        [Description("I系列系统的第89个子网站")]
        SysI_89 = 0989,
        [Description("I系列系统的第90个子网站")]
        SysI_90 = 0990,
        [Description("I系列系统的第91个子网站")]
        SysI_91 = 0991,
        [Description("I系列系统的第92个子网站")]
        SysI_92 = 0992,
        [Description("I系列系统的第93个子网站")]
        SysI_93 = 0993,
        [Description("I系列系统的第94个子网站")]
        SysI_94 = 0994,
        [Description("I系列系统的第95个子网站")]
        SysI_95 = 0995,
        [Description("I系列系统的第96个子网站")]
        SysI_96 = 0996,
        [Description("I系列系统的第97个子网站")]
        SysI_97 = 0997,
        [Description("I系列系统的第98个子网站")]
        SysI_98 = 0998,
        [Description("I系列系统的第99个子网站")]
        SysI_99 = 0999,
        [Description("J系列系统的第1个子网站")]
        SysJ_01 = 1001,
        [Description("J系列系统的第2个子网站")]
        SysJ_02 = 1002,
        [Description("J系列系统的第3个子网站")]
        SysJ_03 = 1003,
        [Description("J系列系统的第4个子网站")]
        SysJ_04 = 1004,
        [Description("J系列系统的第5个子网站")]
        SysJ_05 = 1005,
        [Description("J系列系统的第6个子网站")]
        SysJ_06 = 1006,
        [Description("J系列系统的第7个子网站")]
        SysJ_07 = 1007,
        [Description("J系列系统的第8个子网站")]
        SysJ_08 = 1008,
        [Description("J系列系统的第9个子网站")]
        SysJ_09 = 1009,
        [Description("J系列系统的第10个子网站")]
        SysJ_10 = 1010,
        [Description("J系列系统的第11个子网站")]
        SysJ_11 = 1011,
        [Description("J系列系统的第12个子网站")]
        SysJ_12 = 1012,
        [Description("J系列系统的第13个子网站")]
        SysJ_13 = 1013,
        [Description("J系列系统的第14个子网站")]
        SysJ_14 = 1014,
        [Description("J系列系统的第15个子网站")]
        SysJ_15 = 1015,
        [Description("J系列系统的第16个子网站")]
        SysJ_16 = 1016,
        [Description("J系列系统的第17个子网站")]
        SysJ_17 = 1017,
        [Description("J系列系统的第18个子网站")]
        SysJ_18 = 1018,
        [Description("J系列系统的第19个子网站")]
        SysJ_19 = 1019,
        [Description("J系列系统的第20个子网站")]
        SysJ_20 = 1020,
        [Description("J系列系统的第21个子网站")]
        SysJ_21 = 1021,
        [Description("J系列系统的第22个子网站")]
        SysJ_22 = 1022,
        [Description("J系列系统的第23个子网站")]
        SysJ_23 = 1023,
        [Description("J系列系统的第24个子网站")]
        SysJ_24 = 1024,
        [Description("J系列系统的第25个子网站")]
        SysJ_25 = 1025,
        [Description("J系列系统的第26个子网站")]
        SysJ_26 = 1026,
        [Description("J系列系统的第27个子网站")]
        SysJ_27 = 1027,
        [Description("J系列系统的第28个子网站")]
        SysJ_28 = 1028,
        [Description("J系列系统的第29个子网站")]
        SysJ_29 = 1029,
        [Description("J系列系统的第30个子网站")]
        SysJ_30 = 1030,
        [Description("J系列系统的第31个子网站")]
        SysJ_31 = 1031,
        [Description("J系列系统的第32个子网站")]
        SysJ_32 = 1032,
        [Description("J系列系统的第33个子网站")]
        SysJ_33 = 1033,
        [Description("J系列系统的第34个子网站")]
        SysJ_34 = 1034,
        [Description("J系列系统的第35个子网站")]
        SysJ_35 = 1035,
        [Description("J系列系统的第36个子网站")]
        SysJ_36 = 1036,
        [Description("J系列系统的第37个子网站")]
        SysJ_37 = 1037,
        [Description("J系列系统的第38个子网站")]
        SysJ_38 = 1038,
        [Description("J系列系统的第39个子网站")]
        SysJ_39 = 1039,
        [Description("J系列系统的第40个子网站")]
        SysJ_40 = 1040,
        [Description("J系列系统的第41个子网站")]
        SysJ_41 = 1041,
        [Description("J系列系统的第42个子网站")]
        SysJ_42 = 1042,
        [Description("J系列系统的第43个子网站")]
        SysJ_43 = 1043,
        [Description("J系列系统的第44个子网站")]
        SysJ_44 = 1044,
        [Description("J系列系统的第45个子网站")]
        SysJ_45 = 1045,
        [Description("J系列系统的第46个子网站")]
        SysJ_46 = 1046,
        [Description("J系列系统的第47个子网站")]
        SysJ_47 = 1047,
        [Description("J系列系统的第48个子网站")]
        SysJ_48 = 1048,
        [Description("J系列系统的第49个子网站")]
        SysJ_49 = 1049,
        [Description("J系列系统的第50个子网站")]
        SysJ_50 = 1050,
        [Description("J系列系统的第51个子网站")]
        SysJ_51 = 1051,
        [Description("J系列系统的第52个子网站")]
        SysJ_52 = 1052,
        [Description("J系列系统的第53个子网站")]
        SysJ_53 = 1053,
        [Description("J系列系统的第54个子网站")]
        SysJ_54 = 1054,
        [Description("J系列系统的第55个子网站")]
        SysJ_55 = 1055,
        [Description("J系列系统的第56个子网站")]
        SysJ_56 = 1056,
        [Description("J系列系统的第57个子网站")]
        SysJ_57 = 1057,
        [Description("J系列系统的第58个子网站")]
        SysJ_58 = 1058,
        [Description("J系列系统的第59个子网站")]
        SysJ_59 = 1059,
        [Description("J系列系统的第60个子网站")]
        SysJ_60 = 1060,
        [Description("J系列系统的第61个子网站")]
        SysJ_61 = 1061,
        [Description("J系列系统的第62个子网站")]
        SysJ_62 = 1062,
        [Description("J系列系统的第63个子网站")]
        SysJ_63 = 1063,
        [Description("J系列系统的第64个子网站")]
        SysJ_64 = 1064,
        [Description("J系列系统的第65个子网站")]
        SysJ_65 = 1065,
        [Description("J系列系统的第66个子网站")]
        SysJ_66 = 1066,
        [Description("J系列系统的第67个子网站")]
        SysJ_67 = 1067,
        [Description("J系列系统的第68个子网站")]
        SysJ_68 = 1068,
        [Description("J系列系统的第69个子网站")]
        SysJ_69 = 1069,
        [Description("J系列系统的第70个子网站")]
        SysJ_70 = 1070,
        [Description("J系列系统的第71个子网站")]
        SysJ_71 = 1071,
        [Description("J系列系统的第72个子网站")]
        SysJ_72 = 1072,
        [Description("J系列系统的第73个子网站")]
        SysJ_73 = 1073,
        [Description("J系列系统的第74个子网站")]
        SysJ_74 = 1074,
        [Description("J系列系统的第75个子网站")]
        SysJ_75 = 1075,
        [Description("J系列系统的第76个子网站")]
        SysJ_76 = 1076,
        [Description("J系列系统的第77个子网站")]
        SysJ_77 = 1077,
        [Description("J系列系统的第78个子网站")]
        SysJ_78 = 1078,
        [Description("J系列系统的第79个子网站")]
        SysJ_79 = 1079,
        [Description("J系列系统的第80个子网站")]
        SysJ_80 = 1080,
        [Description("J系列系统的第81个子网站")]
        SysJ_81 = 1081,
        [Description("J系列系统的第82个子网站")]
        SysJ_82 = 1082,
        [Description("J系列系统的第83个子网站")]
        SysJ_83 = 1083,
        [Description("J系列系统的第84个子网站")]
        SysJ_84 = 1084,
        [Description("J系列系统的第85个子网站")]
        SysJ_85 = 1085,
        [Description("J系列系统的第86个子网站")]
        SysJ_86 = 1086,
        [Description("J系列系统的第87个子网站")]
        SysJ_87 = 1087,
        [Description("J系列系统的第88个子网站")]
        SysJ_88 = 1088,
        [Description("J系列系统的第89个子网站")]
        SysJ_89 = 1089,
        [Description("J系列系统的第90个子网站")]
        SysJ_90 = 1090,
        [Description("J系列系统的第91个子网站")]
        SysJ_91 = 1091,
        [Description("J系列系统的第92个子网站")]
        SysJ_92 = 1092,
        [Description("J系列系统的第93个子网站")]
        SysJ_93 = 1093,
        [Description("J系列系统的第94个子网站")]
        SysJ_94 = 1094,
        [Description("J系列系统的第95个子网站")]
        SysJ_95 = 1095,
        [Description("J系列系统的第96个子网站")]
        SysJ_96 = 1096,
        [Description("J系列系统的第97个子网站")]
        SysJ_97 = 1097,
        [Description("J系列系统的第98个子网站")]
        SysJ_98 = 1098,
        [Description("J系列系统的第99个子网站")]
        SysJ_99 = 1099,
        [Description("K系列系统的第1个子网站")]
        SysK_01 = 1101,
        [Description("K系列系统的第2个子网站")]
        SysK_02 = 1102,
        [Description("K系列系统的第3个子网站")]
        SysK_03 = 1103,
        [Description("K系列系统的第4个子网站")]
        SysK_04 = 1104,
        [Description("K系列系统的第5个子网站")]
        SysK_05 = 1105,
        [Description("K系列系统的第6个子网站")]
        SysK_06 = 1106,
        [Description("K系列系统的第7个子网站")]
        SysK_07 = 1107,
        [Description("K系列系统的第8个子网站")]
        SysK_08 = 1108,
        [Description("K系列系统的第9个子网站")]
        SysK_09 = 1109,
        [Description("K系列系统的第10个子网站")]
        SysK_10 = 1110,
        [Description("K系列系统的第11个子网站")]
        SysK_11 = 1111,
        [Description("K系列系统的第12个子网站")]
        SysK_12 = 1112,
        [Description("K系列系统的第13个子网站")]
        SysK_13 = 1113,
        [Description("K系列系统的第14个子网站")]
        SysK_14 = 1114,
        [Description("K系列系统的第15个子网站")]
        SysK_15 = 1115,
        [Description("K系列系统的第16个子网站")]
        SysK_16 = 1116,
        [Description("K系列系统的第17个子网站")]
        SysK_17 = 1117,
        [Description("K系列系统的第18个子网站")]
        SysK_18 = 1118,
        [Description("K系列系统的第19个子网站")]
        SysK_19 = 1119,
        [Description("K系列系统的第20个子网站")]
        SysK_20 = 1120,
        [Description("K系列系统的第21个子网站")]
        SysK_21 = 1121,
        [Description("K系列系统的第22个子网站")]
        SysK_22 = 1122,
        [Description("K系列系统的第23个子网站")]
        SysK_23 = 1123,
        [Description("K系列系统的第24个子网站")]
        SysK_24 = 1124,
        [Description("K系列系统的第25个子网站")]
        SysK_25 = 1125,
        [Description("K系列系统的第26个子网站")]
        SysK_26 = 1126,
        [Description("K系列系统的第27个子网站")]
        SysK_27 = 1127,
        [Description("K系列系统的第28个子网站")]
        SysK_28 = 1128,
        [Description("K系列系统的第29个子网站")]
        SysK_29 = 1129,
        [Description("K系列系统的第30个子网站")]
        SysK_30 = 1130,
        [Description("K系列系统的第31个子网站")]
        SysK_31 = 1131,
        [Description("K系列系统的第32个子网站")]
        SysK_32 = 1132,
        [Description("K系列系统的第33个子网站")]
        SysK_33 = 1133,
        [Description("K系列系统的第34个子网站")]
        SysK_34 = 1134,
        [Description("K系列系统的第35个子网站")]
        SysK_35 = 1135,
        [Description("K系列系统的第36个子网站")]
        SysK_36 = 1136,
        [Description("K系列系统的第37个子网站")]
        SysK_37 = 1137,
        [Description("K系列系统的第38个子网站")]
        SysK_38 = 1138,
        [Description("K系列系统的第39个子网站")]
        SysK_39 = 1139,
        [Description("K系列系统的第40个子网站")]
        SysK_40 = 1140,
        [Description("K系列系统的第41个子网站")]
        SysK_41 = 1141,
        [Description("K系列系统的第42个子网站")]
        SysK_42 = 1142,
        [Description("K系列系统的第43个子网站")]
        SysK_43 = 1143,
        [Description("K系列系统的第44个子网站")]
        SysK_44 = 1144,
        [Description("K系列系统的第45个子网站")]
        SysK_45 = 1145,
        [Description("K系列系统的第46个子网站")]
        SysK_46 = 1146,
        [Description("K系列系统的第47个子网站")]
        SysK_47 = 1147,
        [Description("K系列系统的第48个子网站")]
        SysK_48 = 1148,
        [Description("K系列系统的第49个子网站")]
        SysK_49 = 1149,
        [Description("K系列系统的第50个子网站")]
        SysK_50 = 1150,
        [Description("K系列系统的第51个子网站")]
        SysK_51 = 1151,
        [Description("K系列系统的第52个子网站")]
        SysK_52 = 1152,
        [Description("K系列系统的第53个子网站")]
        SysK_53 = 1153,
        [Description("K系列系统的第54个子网站")]
        SysK_54 = 1154,
        [Description("K系列系统的第55个子网站")]
        SysK_55 = 1155,
        [Description("K系列系统的第56个子网站")]
        SysK_56 = 1156,
        [Description("K系列系统的第57个子网站")]
        SysK_57 = 1157,
        [Description("K系列系统的第58个子网站")]
        SysK_58 = 1158,
        [Description("K系列系统的第59个子网站")]
        SysK_59 = 1159,
        [Description("K系列系统的第60个子网站")]
        SysK_60 = 1160,
        [Description("K系列系统的第61个子网站")]
        SysK_61 = 1161,
        [Description("K系列系统的第62个子网站")]
        SysK_62 = 1162,
        [Description("K系列系统的第63个子网站")]
        SysK_63 = 1163,
        [Description("K系列系统的第64个子网站")]
        SysK_64 = 1164,
        [Description("K系列系统的第65个子网站")]
        SysK_65 = 1165,
        [Description("K系列系统的第66个子网站")]
        SysK_66 = 1166,
        [Description("K系列系统的第67个子网站")]
        SysK_67 = 1167,
        [Description("K系列系统的第68个子网站")]
        SysK_68 = 1168,
        [Description("K系列系统的第69个子网站")]
        SysK_69 = 1169,
        [Description("K系列系统的第70个子网站")]
        SysK_70 = 1170,
        [Description("K系列系统的第71个子网站")]
        SysK_71 = 1171,
        [Description("K系列系统的第72个子网站")]
        SysK_72 = 1172,
        [Description("K系列系统的第73个子网站")]
        SysK_73 = 1173,
        [Description("K系列系统的第74个子网站")]
        SysK_74 = 1174,
        [Description("K系列系统的第75个子网站")]
        SysK_75 = 1175,
        [Description("K系列系统的第76个子网站")]
        SysK_76 = 1176,
        [Description("K系列系统的第77个子网站")]
        SysK_77 = 1177,
        [Description("K系列系统的第78个子网站")]
        SysK_78 = 1178,
        [Description("K系列系统的第79个子网站")]
        SysK_79 = 1179,
        [Description("K系列系统的第80个子网站")]
        SysK_80 = 1180,
        [Description("K系列系统的第81个子网站")]
        SysK_81 = 1181,
        [Description("K系列系统的第82个子网站")]
        SysK_82 = 1182,
        [Description("K系列系统的第83个子网站")]
        SysK_83 = 1183,
        [Description("K系列系统的第84个子网站")]
        SysK_84 = 1184,
        [Description("K系列系统的第85个子网站")]
        SysK_85 = 1185,
        [Description("K系列系统的第86个子网站")]
        SysK_86 = 1186,
        [Description("K系列系统的第87个子网站")]
        SysK_87 = 1187,
        [Description("K系列系统的第88个子网站")]
        SysK_88 = 1188,
        [Description("K系列系统的第89个子网站")]
        SysK_89 = 1189,
        [Description("K系列系统的第90个子网站")]
        SysK_90 = 1190,
        [Description("K系列系统的第91个子网站")]
        SysK_91 = 1191,
        [Description("K系列系统的第92个子网站")]
        SysK_92 = 1192,
        [Description("K系列系统的第93个子网站")]
        SysK_93 = 1193,
        [Description("K系列系统的第94个子网站")]
        SysK_94 = 1194,
        [Description("K系列系统的第95个子网站")]
        SysK_95 = 1195,
        [Description("K系列系统的第96个子网站")]
        SysK_96 = 1196,
        [Description("K系列系统的第97个子网站")]
        SysK_97 = 1197,
        [Description("K系列系统的第98个子网站")]
        SysK_98 = 1198,
        [Description("K系列系统的第99个子网站")]
        SysK_99 = 1199,
        [Description("L系列系统的第1个子网站")]
        SysL_01 = 1201,
        [Description("L系列系统的第2个子网站")]
        SysL_02 = 1202,
        [Description("L系列系统的第3个子网站")]
        SysL_03 = 1203,
        [Description("L系列系统的第4个子网站")]
        SysL_04 = 1204,
        [Description("L系列系统的第5个子网站")]
        SysL_05 = 1205,
        [Description("L系列系统的第6个子网站")]
        SysL_06 = 1206,
        [Description("L系列系统的第7个子网站")]
        SysL_07 = 1207,
        [Description("L系列系统的第8个子网站")]
        SysL_08 = 1208,
        [Description("L系列系统的第9个子网站")]
        SysL_09 = 1209,
        [Description("L系列系统的第10个子网站")]
        SysL_10 = 1210,
        [Description("L系列系统的第11个子网站")]
        SysL_11 = 1211,
        [Description("L系列系统的第12个子网站")]
        SysL_12 = 1212,
        [Description("L系列系统的第13个子网站")]
        SysL_13 = 1213,
        [Description("L系列系统的第14个子网站")]
        SysL_14 = 1214,
        [Description("L系列系统的第15个子网站")]
        SysL_15 = 1215,
        [Description("L系列系统的第16个子网站")]
        SysL_16 = 1216,
        [Description("L系列系统的第17个子网站")]
        SysL_17 = 1217,
        [Description("L系列系统的第18个子网站")]
        SysL_18 = 1218,
        [Description("L系列系统的第19个子网站")]
        SysL_19 = 1219,
        [Description("L系列系统的第20个子网站")]
        SysL_20 = 1220,
        [Description("L系列系统的第21个子网站")]
        SysL_21 = 1221,
        [Description("L系列系统的第22个子网站")]
        SysL_22 = 1222,
        [Description("L系列系统的第23个子网站")]
        SysL_23 = 1223,
        [Description("L系列系统的第24个子网站")]
        SysL_24 = 1224,
        [Description("L系列系统的第25个子网站")]
        SysL_25 = 1225,
        [Description("L系列系统的第26个子网站")]
        SysL_26 = 1226,
        [Description("L系列系统的第27个子网站")]
        SysL_27 = 1227,
        [Description("L系列系统的第28个子网站")]
        SysL_28 = 1228,
        [Description("L系列系统的第29个子网站")]
        SysL_29 = 1229,
        [Description("L系列系统的第30个子网站")]
        SysL_30 = 1230,
        [Description("L系列系统的第31个子网站")]
        SysL_31 = 1231,
        [Description("L系列系统的第32个子网站")]
        SysL_32 = 1232,
        [Description("L系列系统的第33个子网站")]
        SysL_33 = 1233,
        [Description("L系列系统的第34个子网站")]
        SysL_34 = 1234,
        [Description("L系列系统的第35个子网站")]
        SysL_35 = 1235,
        [Description("L系列系统的第36个子网站")]
        SysL_36 = 1236,
        [Description("L系列系统的第37个子网站")]
        SysL_37 = 1237,
        [Description("L系列系统的第38个子网站")]
        SysL_38 = 1238,
        [Description("L系列系统的第39个子网站")]
        SysL_39 = 1239,
        [Description("L系列系统的第40个子网站")]
        SysL_40 = 1240,
        [Description("L系列系统的第41个子网站")]
        SysL_41 = 1241,
        [Description("L系列系统的第42个子网站")]
        SysL_42 = 1242,
        [Description("L系列系统的第43个子网站")]
        SysL_43 = 1243,
        [Description("L系列系统的第44个子网站")]
        SysL_44 = 1244,
        [Description("L系列系统的第45个子网站")]
        SysL_45 = 1245,
        [Description("L系列系统的第46个子网站")]
        SysL_46 = 1246,
        [Description("L系列系统的第47个子网站")]
        SysL_47 = 1247,
        [Description("L系列系统的第48个子网站")]
        SysL_48 = 1248,
        [Description("L系列系统的第49个子网站")]
        SysL_49 = 1249,
        [Description("L系列系统的第50个子网站")]
        SysL_50 = 1250,
        [Description("L系列系统的第51个子网站")]
        SysL_51 = 1251,
        [Description("L系列系统的第52个子网站")]
        SysL_52 = 1252,
        [Description("L系列系统的第53个子网站")]
        SysL_53 = 1253,
        [Description("L系列系统的第54个子网站")]
        SysL_54 = 1254,
        [Description("L系列系统的第55个子网站")]
        SysL_55 = 1255,
        [Description("L系列系统的第56个子网站")]
        SysL_56 = 1256,
        [Description("L系列系统的第57个子网站")]
        SysL_57 = 1257,
        [Description("L系列系统的第58个子网站")]
        SysL_58 = 1258,
        [Description("L系列系统的第59个子网站")]
        SysL_59 = 1259,
        [Description("L系列系统的第60个子网站")]
        SysL_60 = 1260,
        [Description("L系列系统的第61个子网站")]
        SysL_61 = 1261,
        [Description("L系列系统的第62个子网站")]
        SysL_62 = 1262,
        [Description("L系列系统的第63个子网站")]
        SysL_63 = 1263,
        [Description("L系列系统的第64个子网站")]
        SysL_64 = 1264,
        [Description("L系列系统的第65个子网站")]
        SysL_65 = 1265,
        [Description("L系列系统的第66个子网站")]
        SysL_66 = 1266,
        [Description("L系列系统的第67个子网站")]
        SysL_67 = 1267,
        [Description("L系列系统的第68个子网站")]
        SysL_68 = 1268,
        [Description("L系列系统的第69个子网站")]
        SysL_69 = 1269,
        [Description("L系列系统的第70个子网站")]
        SysL_70 = 1270,
        [Description("L系列系统的第71个子网站")]
        SysL_71 = 1271,
        [Description("L系列系统的第72个子网站")]
        SysL_72 = 1272,
        [Description("L系列系统的第73个子网站")]
        SysL_73 = 1273,
        [Description("L系列系统的第74个子网站")]
        SysL_74 = 1274,
        [Description("L系列系统的第75个子网站")]
        SysL_75 = 1275,
        [Description("L系列系统的第76个子网站")]
        SysL_76 = 1276,
        [Description("L系列系统的第77个子网站")]
        SysL_77 = 1277,
        [Description("L系列系统的第78个子网站")]
        SysL_78 = 1278,
        [Description("L系列系统的第79个子网站")]
        SysL_79 = 1279,
        [Description("L系列系统的第80个子网站")]
        SysL_80 = 1280,
        [Description("L系列系统的第81个子网站")]
        SysL_81 = 1281,
        [Description("L系列系统的第82个子网站")]
        SysL_82 = 1282,
        [Description("L系列系统的第83个子网站")]
        SysL_83 = 1283,
        [Description("L系列系统的第84个子网站")]
        SysL_84 = 1284,
        [Description("L系列系统的第85个子网站")]
        SysL_85 = 1285,
        [Description("L系列系统的第86个子网站")]
        SysL_86 = 1286,
        [Description("L系列系统的第87个子网站")]
        SysL_87 = 1287,
        [Description("L系列系统的第88个子网站")]
        SysL_88 = 1288,
        [Description("L系列系统的第89个子网站")]
        SysL_89 = 1289,
        [Description("L系列系统的第90个子网站")]
        SysL_90 = 1290,
        [Description("L系列系统的第91个子网站")]
        SysL_91 = 1291,
        [Description("L系列系统的第92个子网站")]
        SysL_92 = 1292,
        [Description("L系列系统的第93个子网站")]
        SysL_93 = 1293,
        [Description("L系列系统的第94个子网站")]
        SysL_94 = 1294,
        [Description("L系列系统的第95个子网站")]
        SysL_95 = 1295,
        [Description("L系列系统的第96个子网站")]
        SysL_96 = 1296,
        [Description("L系列系统的第97个子网站")]
        SysL_97 = 1297,
        [Description("L系列系统的第98个子网站")]
        SysL_98 = 1298,
        [Description("L系列系统的第99个子网站")]
        SysL_99 = 1299,
        [Description("M系列系统的第1个子网站")]
        SysM_01 = 1301,
        [Description("M系列系统的第2个子网站")]
        SysM_02 = 1302,
        [Description("M系列系统的第3个子网站")]
        SysM_03 = 1303,
        [Description("M系列系统的第4个子网站")]
        SysM_04 = 1304,
        [Description("M系列系统的第5个子网站")]
        SysM_05 = 1305,
        [Description("M系列系统的第6个子网站")]
        SysM_06 = 1306,
        [Description("M系列系统的第7个子网站")]
        SysM_07 = 1307,
        [Description("M系列系统的第8个子网站")]
        SysM_08 = 1308,
        [Description("M系列系统的第9个子网站")]
        SysM_09 = 1309,
        [Description("M系列系统的第10个子网站")]
        SysM_10 = 1310,
        [Description("M系列系统的第11个子网站")]
        SysM_11 = 1311,
        [Description("M系列系统的第12个子网站")]
        SysM_12 = 1312,
        [Description("M系列系统的第13个子网站")]
        SysM_13 = 1313,
        [Description("M系列系统的第14个子网站")]
        SysM_14 = 1314,
        [Description("M系列系统的第15个子网站")]
        SysM_15 = 1315,
        [Description("M系列系统的第16个子网站")]
        SysM_16 = 1316,
        [Description("M系列系统的第17个子网站")]
        SysM_17 = 1317,
        [Description("M系列系统的第18个子网站")]
        SysM_18 = 1318,
        [Description("M系列系统的第19个子网站")]
        SysM_19 = 1319,
        [Description("M系列系统的第20个子网站")]
        SysM_20 = 1320,
        [Description("M系列系统的第21个子网站")]
        SysM_21 = 1321,
        [Description("M系列系统的第22个子网站")]
        SysM_22 = 1322,
        [Description("M系列系统的第23个子网站")]
        SysM_23 = 1323,
        [Description("M系列系统的第24个子网站")]
        SysM_24 = 1324,
        [Description("M系列系统的第25个子网站")]
        SysM_25 = 1325,
        [Description("M系列系统的第26个子网站")]
        SysM_26 = 1326,
        [Description("M系列系统的第27个子网站")]
        SysM_27 = 1327,
        [Description("M系列系统的第28个子网站")]
        SysM_28 = 1328,
        [Description("M系列系统的第29个子网站")]
        SysM_29 = 1329,
        [Description("M系列系统的第30个子网站")]
        SysM_30 = 1330,
        [Description("M系列系统的第31个子网站")]
        SysM_31 = 1331,
        [Description("M系列系统的第32个子网站")]
        SysM_32 = 1332,
        [Description("M系列系统的第33个子网站")]
        SysM_33 = 1333,
        [Description("M系列系统的第34个子网站")]
        SysM_34 = 1334,
        [Description("M系列系统的第35个子网站")]
        SysM_35 = 1335,
        [Description("M系列系统的第36个子网站")]
        SysM_36 = 1336,
        [Description("M系列系统的第37个子网站")]
        SysM_37 = 1337,
        [Description("M系列系统的第38个子网站")]
        SysM_38 = 1338,
        [Description("M系列系统的第39个子网站")]
        SysM_39 = 1339,
        [Description("M系列系统的第40个子网站")]
        SysM_40 = 1340,
        [Description("M系列系统的第41个子网站")]
        SysM_41 = 1341,
        [Description("M系列系统的第42个子网站")]
        SysM_42 = 1342,
        [Description("M系列系统的第43个子网站")]
        SysM_43 = 1343,
        [Description("M系列系统的第44个子网站")]
        SysM_44 = 1344,
        [Description("M系列系统的第45个子网站")]
        SysM_45 = 1345,
        [Description("M系列系统的第46个子网站")]
        SysM_46 = 1346,
        [Description("M系列系统的第47个子网站")]
        SysM_47 = 1347,
        [Description("M系列系统的第48个子网站")]
        SysM_48 = 1348,
        [Description("M系列系统的第49个子网站")]
        SysM_49 = 1349,
        [Description("M系列系统的第50个子网站")]
        SysM_50 = 1350,
        [Description("M系列系统的第51个子网站")]
        SysM_51 = 1351,
        [Description("M系列系统的第52个子网站")]
        SysM_52 = 1352,
        [Description("M系列系统的第53个子网站")]
        SysM_53 = 1353,
        [Description("M系列系统的第54个子网站")]
        SysM_54 = 1354,
        [Description("M系列系统的第55个子网站")]
        SysM_55 = 1355,
        [Description("M系列系统的第56个子网站")]
        SysM_56 = 1356,
        [Description("M系列系统的第57个子网站")]
        SysM_57 = 1357,
        [Description("M系列系统的第58个子网站")]
        SysM_58 = 1358,
        [Description("M系列系统的第59个子网站")]
        SysM_59 = 1359,
        [Description("M系列系统的第60个子网站")]
        SysM_60 = 1360,
        [Description("M系列系统的第61个子网站")]
        SysM_61 = 1361,
        [Description("M系列系统的第62个子网站")]
        SysM_62 = 1362,
        [Description("M系列系统的第63个子网站")]
        SysM_63 = 1363,
        [Description("M系列系统的第64个子网站")]
        SysM_64 = 1364,
        [Description("M系列系统的第65个子网站")]
        SysM_65 = 1365,
        [Description("M系列系统的第66个子网站")]
        SysM_66 = 1366,
        [Description("M系列系统的第67个子网站")]
        SysM_67 = 1367,
        [Description("M系列系统的第68个子网站")]
        SysM_68 = 1368,
        [Description("M系列系统的第69个子网站")]
        SysM_69 = 1369,
        [Description("M系列系统的第70个子网站")]
        SysM_70 = 1370,
        [Description("M系列系统的第71个子网站")]
        SysM_71 = 1371,
        [Description("M系列系统的第72个子网站")]
        SysM_72 = 1372,
        [Description("M系列系统的第73个子网站")]
        SysM_73 = 1373,
        [Description("M系列系统的第74个子网站")]
        SysM_74 = 1374,
        [Description("M系列系统的第75个子网站")]
        SysM_75 = 1375,
        [Description("M系列系统的第76个子网站")]
        SysM_76 = 1376,
        [Description("M系列系统的第77个子网站")]
        SysM_77 = 1377,
        [Description("M系列系统的第78个子网站")]
        SysM_78 = 1378,
        [Description("M系列系统的第79个子网站")]
        SysM_79 = 1379,
        [Description("M系列系统的第80个子网站")]
        SysM_80 = 1380,
        [Description("M系列系统的第81个子网站")]
        SysM_81 = 1381,
        [Description("M系列系统的第82个子网站")]
        SysM_82 = 1382,
        [Description("M系列系统的第83个子网站")]
        SysM_83 = 1383,
        [Description("M系列系统的第84个子网站")]
        SysM_84 = 1384,
        [Description("M系列系统的第85个子网站")]
        SysM_85 = 1385,
        [Description("M系列系统的第86个子网站")]
        SysM_86 = 1386,
        [Description("M系列系统的第87个子网站")]
        SysM_87 = 1387,
        [Description("M系列系统的第88个子网站")]
        SysM_88 = 1388,
        [Description("M系列系统的第89个子网站")]
        SysM_89 = 1389,
        [Description("M系列系统的第90个子网站")]
        SysM_90 = 1390,
        [Description("M系列系统的第91个子网站")]
        SysM_91 = 1391,
        [Description("M系列系统的第92个子网站")]
        SysM_92 = 1392,
        [Description("M系列系统的第93个子网站")]
        SysM_93 = 1393,
        [Description("M系列系统的第94个子网站")]
        SysM_94 = 1394,
        [Description("M系列系统的第95个子网站")]
        SysM_95 = 1395,
        [Description("M系列系统的第96个子网站")]
        SysM_96 = 1396,
        [Description("M系列系统的第97个子网站")]
        SysM_97 = 1397,
        [Description("M系列系统的第98个子网站")]
        SysM_98 = 1398,
        [Description("M系列系统的第99个子网站")]
        SysM_99 = 1399,
        [Description("N系列系统的第1个子网站")]
        SysN_01 = 1401,
        [Description("N系列系统的第2个子网站")]
        SysN_02 = 1402,
        [Description("N系列系统的第3个子网站")]
        SysN_03 = 1403,
        [Description("N系列系统的第4个子网站")]
        SysN_04 = 1404,
        [Description("N系列系统的第5个子网站")]
        SysN_05 = 1405,
        [Description("N系列系统的第6个子网站")]
        SysN_06 = 1406,
        [Description("N系列系统的第7个子网站")]
        SysN_07 = 1407,
        [Description("N系列系统的第8个子网站")]
        SysN_08 = 1408,
        [Description("N系列系统的第9个子网站")]
        SysN_09 = 1409,
        [Description("N系列系统的第10个子网站")]
        SysN_10 = 1410,
        [Description("N系列系统的第11个子网站")]
        SysN_11 = 1411,
        [Description("N系列系统的第12个子网站")]
        SysN_12 = 1412,
        [Description("N系列系统的第13个子网站")]
        SysN_13 = 1413,
        [Description("N系列系统的第14个子网站")]
        SysN_14 = 1414,
        [Description("N系列系统的第15个子网站")]
        SysN_15 = 1415,
        [Description("N系列系统的第16个子网站")]
        SysN_16 = 1416,
        [Description("N系列系统的第17个子网站")]
        SysN_17 = 1417,
        [Description("N系列系统的第18个子网站")]
        SysN_18 = 1418,
        [Description("N系列系统的第19个子网站")]
        SysN_19 = 1419,
        [Description("N系列系统的第20个子网站")]
        SysN_20 = 1420,
        [Description("N系列系统的第21个子网站")]
        SysN_21 = 1421,
        [Description("N系列系统的第22个子网站")]
        SysN_22 = 1422,
        [Description("N系列系统的第23个子网站")]
        SysN_23 = 1423,
        [Description("N系列系统的第24个子网站")]
        SysN_24 = 1424,
        [Description("N系列系统的第25个子网站")]
        SysN_25 = 1425,
        [Description("N系列系统的第26个子网站")]
        SysN_26 = 1426,
        [Description("N系列系统的第27个子网站")]
        SysN_27 = 1427,
        [Description("N系列系统的第28个子网站")]
        SysN_28 = 1428,
        [Description("N系列系统的第29个子网站")]
        SysN_29 = 1429,
        [Description("N系列系统的第30个子网站")]
        SysN_30 = 1430,
        [Description("N系列系统的第31个子网站")]
        SysN_31 = 1431,
        [Description("N系列系统的第32个子网站")]
        SysN_32 = 1432,
        [Description("N系列系统的第33个子网站")]
        SysN_33 = 1433,
        [Description("N系列系统的第34个子网站")]
        SysN_34 = 1434,
        [Description("N系列系统的第35个子网站")]
        SysN_35 = 1435,
        [Description("N系列系统的第36个子网站")]
        SysN_36 = 1436,
        [Description("N系列系统的第37个子网站")]
        SysN_37 = 1437,
        [Description("N系列系统的第38个子网站")]
        SysN_38 = 1438,
        [Description("N系列系统的第39个子网站")]
        SysN_39 = 1439,
        [Description("N系列系统的第40个子网站")]
        SysN_40 = 1440,
        [Description("N系列系统的第41个子网站")]
        SysN_41 = 1441,
        [Description("N系列系统的第42个子网站")]
        SysN_42 = 1442,
        [Description("N系列系统的第43个子网站")]
        SysN_43 = 1443,
        [Description("N系列系统的第44个子网站")]
        SysN_44 = 1444,
        [Description("N系列系统的第45个子网站")]
        SysN_45 = 1445,
        [Description("N系列系统的第46个子网站")]
        SysN_46 = 1446,
        [Description("N系列系统的第47个子网站")]
        SysN_47 = 1447,
        [Description("N系列系统的第48个子网站")]
        SysN_48 = 1448,
        [Description("N系列系统的第49个子网站")]
        SysN_49 = 1449,
        [Description("N系列系统的第50个子网站")]
        SysN_50 = 1450,
        [Description("N系列系统的第51个子网站")]
        SysN_51 = 1451,
        [Description("N系列系统的第52个子网站")]
        SysN_52 = 1452,
        [Description("N系列系统的第53个子网站")]
        SysN_53 = 1453,
        [Description("N系列系统的第54个子网站")]
        SysN_54 = 1454,
        [Description("N系列系统的第55个子网站")]
        SysN_55 = 1455,
        [Description("N系列系统的第56个子网站")]
        SysN_56 = 1456,
        [Description("N系列系统的第57个子网站")]
        SysN_57 = 1457,
        [Description("N系列系统的第58个子网站")]
        SysN_58 = 1458,
        [Description("N系列系统的第59个子网站")]
        SysN_59 = 1459,
        [Description("N系列系统的第60个子网站")]
        SysN_60 = 1460,
        [Description("N系列系统的第61个子网站")]
        SysN_61 = 1461,
        [Description("N系列系统的第62个子网站")]
        SysN_62 = 1462,
        [Description("N系列系统的第63个子网站")]
        SysN_63 = 1463,
        [Description("N系列系统的第64个子网站")]
        SysN_64 = 1464,
        [Description("N系列系统的第65个子网站")]
        SysN_65 = 1465,
        [Description("N系列系统的第66个子网站")]
        SysN_66 = 1466,
        [Description("N系列系统的第67个子网站")]
        SysN_67 = 1467,
        [Description("N系列系统的第68个子网站")]
        SysN_68 = 1468,
        [Description("N系列系统的第69个子网站")]
        SysN_69 = 1469,
        [Description("N系列系统的第70个子网站")]
        SysN_70 = 1470,
        [Description("N系列系统的第71个子网站")]
        SysN_71 = 1471,
        [Description("N系列系统的第72个子网站")]
        SysN_72 = 1472,
        [Description("N系列系统的第73个子网站")]
        SysN_73 = 1473,
        [Description("N系列系统的第74个子网站")]
        SysN_74 = 1474,
        [Description("N系列系统的第75个子网站")]
        SysN_75 = 1475,
        [Description("N系列系统的第76个子网站")]
        SysN_76 = 1476,
        [Description("N系列系统的第77个子网站")]
        SysN_77 = 1477,
        [Description("N系列系统的第78个子网站")]
        SysN_78 = 1478,
        [Description("N系列系统的第79个子网站")]
        SysN_79 = 1479,
        [Description("N系列系统的第80个子网站")]
        SysN_80 = 1480,
        [Description("N系列系统的第81个子网站")]
        SysN_81 = 1481,
        [Description("N系列系统的第82个子网站")]
        SysN_82 = 1482,
        [Description("N系列系统的第83个子网站")]
        SysN_83 = 1483,
        [Description("N系列系统的第84个子网站")]
        SysN_84 = 1484,
        [Description("N系列系统的第85个子网站")]
        SysN_85 = 1485,
        [Description("N系列系统的第86个子网站")]
        SysN_86 = 1486,
        [Description("N系列系统的第87个子网站")]
        SysN_87 = 1487,
        [Description("N系列系统的第88个子网站")]
        SysN_88 = 1488,
        [Description("N系列系统的第89个子网站")]
        SysN_89 = 1489,
        [Description("N系列系统的第90个子网站")]
        SysN_90 = 1490,
        [Description("N系列系统的第91个子网站")]
        SysN_91 = 1491,
        [Description("N系列系统的第92个子网站")]
        SysN_92 = 1492,
        [Description("N系列系统的第93个子网站")]
        SysN_93 = 1493,
        [Description("N系列系统的第94个子网站")]
        SysN_94 = 1494,
        [Description("N系列系统的第95个子网站")]
        SysN_95 = 1495,
        [Description("N系列系统的第96个子网站")]
        SysN_96 = 1496,
        [Description("N系列系统的第97个子网站")]
        SysN_97 = 1497,
        [Description("N系列系统的第98个子网站")]
        SysN_98 = 1498,
        [Description("N系列系统的第99个子网站")]
        SysN_99 = 1499,
        [Description("O系列系统的第1个子网站")]
        SysO_01 = 1501,
        [Description("O系列系统的第2个子网站")]
        SysO_02 = 1502,
        [Description("O系列系统的第3个子网站")]
        SysO_03 = 1503,
        [Description("O系列系统的第4个子网站")]
        SysO_04 = 1504,
        [Description("O系列系统的第5个子网站")]
        SysO_05 = 1505,
        [Description("O系列系统的第6个子网站")]
        SysO_06 = 1506,
        [Description("O系列系统的第7个子网站")]
        SysO_07 = 1507,
        [Description("O系列系统的第8个子网站")]
        SysO_08 = 1508,
        [Description("O系列系统的第9个子网站")]
        SysO_09 = 1509,
        [Description("O系列系统的第10个子网站")]
        SysO_10 = 1510,
        [Description("O系列系统的第11个子网站")]
        SysO_11 = 1511,
        [Description("O系列系统的第12个子网站")]
        SysO_12 = 1512,
        [Description("O系列系统的第13个子网站")]
        SysO_13 = 1513,
        [Description("O系列系统的第14个子网站")]
        SysO_14 = 1514,
        [Description("O系列系统的第15个子网站")]
        SysO_15 = 1515,
        [Description("O系列系统的第16个子网站")]
        SysO_16 = 1516,
        [Description("O系列系统的第17个子网站")]
        SysO_17 = 1517,
        [Description("O系列系统的第18个子网站")]
        SysO_18 = 1518,
        [Description("O系列系统的第19个子网站")]
        SysO_19 = 1519,
        [Description("O系列系统的第20个子网站")]
        SysO_20 = 1520,
        [Description("O系列系统的第21个子网站")]
        SysO_21 = 1521,
        [Description("O系列系统的第22个子网站")]
        SysO_22 = 1522,
        [Description("O系列系统的第23个子网站")]
        SysO_23 = 1523,
        [Description("O系列系统的第24个子网站")]
        SysO_24 = 1524,
        [Description("O系列系统的第25个子网站")]
        SysO_25 = 1525,
        [Description("O系列系统的第26个子网站")]
        SysO_26 = 1526,
        [Description("O系列系统的第27个子网站")]
        SysO_27 = 1527,
        [Description("O系列系统的第28个子网站")]
        SysO_28 = 1528,
        [Description("O系列系统的第29个子网站")]
        SysO_29 = 1529,
        [Description("O系列系统的第30个子网站")]
        SysO_30 = 1530,
        [Description("O系列系统的第31个子网站")]
        SysO_31 = 1531,
        [Description("O系列系统的第32个子网站")]
        SysO_32 = 1532,
        [Description("O系列系统的第33个子网站")]
        SysO_33 = 1533,
        [Description("O系列系统的第34个子网站")]
        SysO_34 = 1534,
        [Description("O系列系统的第35个子网站")]
        SysO_35 = 1535,
        [Description("O系列系统的第36个子网站")]
        SysO_36 = 1536,
        [Description("O系列系统的第37个子网站")]
        SysO_37 = 1537,
        [Description("O系列系统的第38个子网站")]
        SysO_38 = 1538,
        [Description("O系列系统的第39个子网站")]
        SysO_39 = 1539,
        [Description("O系列系统的第40个子网站")]
        SysO_40 = 1540,
        [Description("O系列系统的第41个子网站")]
        SysO_41 = 1541,
        [Description("O系列系统的第42个子网站")]
        SysO_42 = 1542,
        [Description("O系列系统的第43个子网站")]
        SysO_43 = 1543,
        [Description("O系列系统的第44个子网站")]
        SysO_44 = 1544,
        [Description("O系列系统的第45个子网站")]
        SysO_45 = 1545,
        [Description("O系列系统的第46个子网站")]
        SysO_46 = 1546,
        [Description("O系列系统的第47个子网站")]
        SysO_47 = 1547,
        [Description("O系列系统的第48个子网站")]
        SysO_48 = 1548,
        [Description("O系列系统的第49个子网站")]
        SysO_49 = 1549,
        [Description("O系列系统的第50个子网站")]
        SysO_50 = 1550,
        [Description("O系列系统的第51个子网站")]
        SysO_51 = 1551,
        [Description("O系列系统的第52个子网站")]
        SysO_52 = 1552,
        [Description("O系列系统的第53个子网站")]
        SysO_53 = 1553,
        [Description("O系列系统的第54个子网站")]
        SysO_54 = 1554,
        [Description("O系列系统的第55个子网站")]
        SysO_55 = 1555,
        [Description("O系列系统的第56个子网站")]
        SysO_56 = 1556,
        [Description("O系列系统的第57个子网站")]
        SysO_57 = 1557,
        [Description("O系列系统的第58个子网站")]
        SysO_58 = 1558,
        [Description("O系列系统的第59个子网站")]
        SysO_59 = 1559,
        [Description("O系列系统的第60个子网站")]
        SysO_60 = 1560,
        [Description("O系列系统的第61个子网站")]
        SysO_61 = 1561,
        [Description("O系列系统的第62个子网站")]
        SysO_62 = 1562,
        [Description("O系列系统的第63个子网站")]
        SysO_63 = 1563,
        [Description("O系列系统的第64个子网站")]
        SysO_64 = 1564,
        [Description("O系列系统的第65个子网站")]
        SysO_65 = 1565,
        [Description("O系列系统的第66个子网站")]
        SysO_66 = 1566,
        [Description("O系列系统的第67个子网站")]
        SysO_67 = 1567,
        [Description("O系列系统的第68个子网站")]
        SysO_68 = 1568,
        [Description("O系列系统的第69个子网站")]
        SysO_69 = 1569,
        [Description("O系列系统的第70个子网站")]
        SysO_70 = 1570,
        [Description("O系列系统的第71个子网站")]
        SysO_71 = 1571,
        [Description("O系列系统的第72个子网站")]
        SysO_72 = 1572,
        [Description("O系列系统的第73个子网站")]
        SysO_73 = 1573,
        [Description("O系列系统的第74个子网站")]
        SysO_74 = 1574,
        [Description("O系列系统的第75个子网站")]
        SysO_75 = 1575,
        [Description("O系列系统的第76个子网站")]
        SysO_76 = 1576,
        [Description("O系列系统的第77个子网站")]
        SysO_77 = 1577,
        [Description("O系列系统的第78个子网站")]
        SysO_78 = 1578,
        [Description("O系列系统的第79个子网站")]
        SysO_79 = 1579,
        [Description("O系列系统的第80个子网站")]
        SysO_80 = 1580,
        [Description("O系列系统的第81个子网站")]
        SysO_81 = 1581,
        [Description("O系列系统的第82个子网站")]
        SysO_82 = 1582,
        [Description("O系列系统的第83个子网站")]
        SysO_83 = 1583,
        [Description("O系列系统的第84个子网站")]
        SysO_84 = 1584,
        [Description("O系列系统的第85个子网站")]
        SysO_85 = 1585,
        [Description("O系列系统的第86个子网站")]
        SysO_86 = 1586,
        [Description("O系列系统的第87个子网站")]
        SysO_87 = 1587,
        [Description("O系列系统的第88个子网站")]
        SysO_88 = 1588,
        [Description("O系列系统的第89个子网站")]
        SysO_89 = 1589,
        [Description("O系列系统的第90个子网站")]
        SysO_90 = 1590,
        [Description("O系列系统的第91个子网站")]
        SysO_91 = 1591,
        [Description("O系列系统的第92个子网站")]
        SysO_92 = 1592,
        [Description("O系列系统的第93个子网站")]
        SysO_93 = 1593,
        [Description("O系列系统的第94个子网站")]
        SysO_94 = 1594,
        [Description("O系列系统的第95个子网站")]
        SysO_95 = 1595,
        [Description("O系列系统的第96个子网站")]
        SysO_96 = 1596,
        [Description("O系列系统的第97个子网站")]
        SysO_97 = 1597,
        [Description("O系列系统的第98个子网站")]
        SysO_98 = 1598,
        [Description("O系列系统的第99个子网站")]
        SysO_99 = 1599,
        [Description("P系列系统的第1个子网站")]
        SysP_01 = 1601,
        [Description("P系列系统的第2个子网站")]
        SysP_02 = 1602,
        [Description("P系列系统的第3个子网站")]
        SysP_03 = 1603,
        [Description("P系列系统的第4个子网站")]
        SysP_04 = 1604,
        [Description("P系列系统的第5个子网站")]
        SysP_05 = 1605,
        [Description("P系列系统的第6个子网站")]
        SysP_06 = 1606,
        [Description("P系列系统的第7个子网站")]
        SysP_07 = 1607,
        [Description("P系列系统的第8个子网站")]
        SysP_08 = 1608,
        [Description("P系列系统的第9个子网站")]
        SysP_09 = 1609,
        [Description("P系列系统的第10个子网站")]
        SysP_10 = 1610,
        [Description("P系列系统的第11个子网站")]
        SysP_11 = 1611,
        [Description("P系列系统的第12个子网站")]
        SysP_12 = 1612,
        [Description("P系列系统的第13个子网站")]
        SysP_13 = 1613,
        [Description("P系列系统的第14个子网站")]
        SysP_14 = 1614,
        [Description("P系列系统的第15个子网站")]
        SysP_15 = 1615,
        [Description("P系列系统的第16个子网站")]
        SysP_16 = 1616,
        [Description("P系列系统的第17个子网站")]
        SysP_17 = 1617,
        [Description("P系列系统的第18个子网站")]
        SysP_18 = 1618,
        [Description("P系列系统的第19个子网站")]
        SysP_19 = 1619,
        [Description("P系列系统的第20个子网站")]
        SysP_20 = 1620,
        [Description("P系列系统的第21个子网站")]
        SysP_21 = 1621,
        [Description("P系列系统的第22个子网站")]
        SysP_22 = 1622,
        [Description("P系列系统的第23个子网站")]
        SysP_23 = 1623,
        [Description("P系列系统的第24个子网站")]
        SysP_24 = 1624,
        [Description("P系列系统的第25个子网站")]
        SysP_25 = 1625,
        [Description("P系列系统的第26个子网站")]
        SysP_26 = 1626,
        [Description("P系列系统的第27个子网站")]
        SysP_27 = 1627,
        [Description("P系列系统的第28个子网站")]
        SysP_28 = 1628,
        [Description("P系列系统的第29个子网站")]
        SysP_29 = 1629,
        [Description("P系列系统的第30个子网站")]
        SysP_30 = 1630,
        [Description("P系列系统的第31个子网站")]
        SysP_31 = 1631,
        [Description("P系列系统的第32个子网站")]
        SysP_32 = 1632,
        [Description("P系列系统的第33个子网站")]
        SysP_33 = 1633,
        [Description("P系列系统的第34个子网站")]
        SysP_34 = 1634,
        [Description("P系列系统的第35个子网站")]
        SysP_35 = 1635,
        [Description("P系列系统的第36个子网站")]
        SysP_36 = 1636,
        [Description("P系列系统的第37个子网站")]
        SysP_37 = 1637,
        [Description("P系列系统的第38个子网站")]
        SysP_38 = 1638,
        [Description("P系列系统的第39个子网站")]
        SysP_39 = 1639,
        [Description("P系列系统的第40个子网站")]
        SysP_40 = 1640,
        [Description("P系列系统的第41个子网站")]
        SysP_41 = 1641,
        [Description("P系列系统的第42个子网站")]
        SysP_42 = 1642,
        [Description("P系列系统的第43个子网站")]
        SysP_43 = 1643,
        [Description("P系列系统的第44个子网站")]
        SysP_44 = 1644,
        [Description("P系列系统的第45个子网站")]
        SysP_45 = 1645,
        [Description("P系列系统的第46个子网站")]
        SysP_46 = 1646,
        [Description("P系列系统的第47个子网站")]
        SysP_47 = 1647,
        [Description("P系列系统的第48个子网站")]
        SysP_48 = 1648,
        [Description("P系列系统的第49个子网站")]
        SysP_49 = 1649,
        [Description("P系列系统的第50个子网站")]
        SysP_50 = 1650,
        [Description("P系列系统的第51个子网站")]
        SysP_51 = 1651,
        [Description("P系列系统的第52个子网站")]
        SysP_52 = 1652,
        [Description("P系列系统的第53个子网站")]
        SysP_53 = 1653,
        [Description("P系列系统的第54个子网站")]
        SysP_54 = 1654,
        [Description("P系列系统的第55个子网站")]
        SysP_55 = 1655,
        [Description("P系列系统的第56个子网站")]
        SysP_56 = 1656,
        [Description("P系列系统的第57个子网站")]
        SysP_57 = 1657,
        [Description("P系列系统的第58个子网站")]
        SysP_58 = 1658,
        [Description("P系列系统的第59个子网站")]
        SysP_59 = 1659,
        [Description("P系列系统的第60个子网站")]
        SysP_60 = 1660,
        [Description("P系列系统的第61个子网站")]
        SysP_61 = 1661,
        [Description("P系列系统的第62个子网站")]
        SysP_62 = 1662,
        [Description("P系列系统的第63个子网站")]
        SysP_63 = 1663,
        [Description("P系列系统的第64个子网站")]
        SysP_64 = 1664,
        [Description("P系列系统的第65个子网站")]
        SysP_65 = 1665,
        [Description("P系列系统的第66个子网站")]
        SysP_66 = 1666,
        [Description("P系列系统的第67个子网站")]
        SysP_67 = 1667,
        [Description("P系列系统的第68个子网站")]
        SysP_68 = 1668,
        [Description("P系列系统的第69个子网站")]
        SysP_69 = 1669,
        [Description("P系列系统的第70个子网站")]
        SysP_70 = 1670,
        [Description("P系列系统的第71个子网站")]
        SysP_71 = 1671,
        [Description("P系列系统的第72个子网站")]
        SysP_72 = 1672,
        [Description("P系列系统的第73个子网站")]
        SysP_73 = 1673,
        [Description("P系列系统的第74个子网站")]
        SysP_74 = 1674,
        [Description("P系列系统的第75个子网站")]
        SysP_75 = 1675,
        [Description("P系列系统的第76个子网站")]
        SysP_76 = 1676,
        [Description("P系列系统的第77个子网站")]
        SysP_77 = 1677,
        [Description("P系列系统的第78个子网站")]
        SysP_78 = 1678,
        [Description("P系列系统的第79个子网站")]
        SysP_79 = 1679,
        [Description("P系列系统的第80个子网站")]
        SysP_80 = 1680,
        [Description("P系列系统的第81个子网站")]
        SysP_81 = 1681,
        [Description("P系列系统的第82个子网站")]
        SysP_82 = 1682,
        [Description("P系列系统的第83个子网站")]
        SysP_83 = 1683,
        [Description("P系列系统的第84个子网站")]
        SysP_84 = 1684,
        [Description("P系列系统的第85个子网站")]
        SysP_85 = 1685,
        [Description("P系列系统的第86个子网站")]
        SysP_86 = 1686,
        [Description("P系列系统的第87个子网站")]
        SysP_87 = 1687,
        [Description("P系列系统的第88个子网站")]
        SysP_88 = 1688,
        [Description("P系列系统的第89个子网站")]
        SysP_89 = 1689,
        [Description("P系列系统的第90个子网站")]
        SysP_90 = 1690,
        [Description("P系列系统的第91个子网站")]
        SysP_91 = 1691,
        [Description("P系列系统的第92个子网站")]
        SysP_92 = 1692,
        [Description("P系列系统的第93个子网站")]
        SysP_93 = 1693,
        [Description("P系列系统的第94个子网站")]
        SysP_94 = 1694,
        [Description("P系列系统的第95个子网站")]
        SysP_95 = 1695,
        [Description("P系列系统的第96个子网站")]
        SysP_96 = 1696,
        [Description("P系列系统的第97个子网站")]
        SysP_97 = 1697,
        [Description("P系列系统的第98个子网站")]
        SysP_98 = 1698,
        [Description("P系列系统的第99个子网站")]
        SysP_99 = 1699,
        [Description("Q系列系统的第1个子网站")]
        SysQ_01 = 1701,
        [Description("Q系列系统的第2个子网站")]
        SysQ_02 = 1702,
        [Description("Q系列系统的第3个子网站")]
        SysQ_03 = 1703,
        [Description("Q系列系统的第4个子网站")]
        SysQ_04 = 1704,
        [Description("Q系列系统的第5个子网站")]
        SysQ_05 = 1705,
        [Description("Q系列系统的第6个子网站")]
        SysQ_06 = 1706,
        [Description("Q系列系统的第7个子网站")]
        SysQ_07 = 1707,
        [Description("Q系列系统的第8个子网站")]
        SysQ_08 = 1708,
        [Description("Q系列系统的第9个子网站")]
        SysQ_09 = 1709,
        [Description("Q系列系统的第10个子网站")]
        SysQ_10 = 1710,
        [Description("Q系列系统的第11个子网站")]
        SysQ_11 = 1711,
        [Description("Q系列系统的第12个子网站")]
        SysQ_12 = 1712,
        [Description("Q系列系统的第13个子网站")]
        SysQ_13 = 1713,
        [Description("Q系列系统的第14个子网站")]
        SysQ_14 = 1714,
        [Description("Q系列系统的第15个子网站")]
        SysQ_15 = 1715,
        [Description("Q系列系统的第16个子网站")]
        SysQ_16 = 1716,
        [Description("Q系列系统的第17个子网站")]
        SysQ_17 = 1717,
        [Description("Q系列系统的第18个子网站")]
        SysQ_18 = 1718,
        [Description("Q系列系统的第19个子网站")]
        SysQ_19 = 1719,
        [Description("Q系列系统的第20个子网站")]
        SysQ_20 = 1720,
        [Description("Q系列系统的第21个子网站")]
        SysQ_21 = 1721,
        [Description("Q系列系统的第22个子网站")]
        SysQ_22 = 1722,
        [Description("Q系列系统的第23个子网站")]
        SysQ_23 = 1723,
        [Description("Q系列系统的第24个子网站")]
        SysQ_24 = 1724,
        [Description("Q系列系统的第25个子网站")]
        SysQ_25 = 1725,
        [Description("Q系列系统的第26个子网站")]
        SysQ_26 = 1726,
        [Description("Q系列系统的第27个子网站")]
        SysQ_27 = 1727,
        [Description("Q系列系统的第28个子网站")]
        SysQ_28 = 1728,
        [Description("Q系列系统的第29个子网站")]
        SysQ_29 = 1729,
        [Description("Q系列系统的第30个子网站")]
        SysQ_30 = 1730,
        [Description("Q系列系统的第31个子网站")]
        SysQ_31 = 1731,
        [Description("Q系列系统的第32个子网站")]
        SysQ_32 = 1732,
        [Description("Q系列系统的第33个子网站")]
        SysQ_33 = 1733,
        [Description("Q系列系统的第34个子网站")]
        SysQ_34 = 1734,
        [Description("Q系列系统的第35个子网站")]
        SysQ_35 = 1735,
        [Description("Q系列系统的第36个子网站")]
        SysQ_36 = 1736,
        [Description("Q系列系统的第37个子网站")]
        SysQ_37 = 1737,
        [Description("Q系列系统的第38个子网站")]
        SysQ_38 = 1738,
        [Description("Q系列系统的第39个子网站")]
        SysQ_39 = 1739,
        [Description("Q系列系统的第40个子网站")]
        SysQ_40 = 1740,
        [Description("Q系列系统的第41个子网站")]
        SysQ_41 = 1741,
        [Description("Q系列系统的第42个子网站")]
        SysQ_42 = 1742,
        [Description("Q系列系统的第43个子网站")]
        SysQ_43 = 1743,
        [Description("Q系列系统的第44个子网站")]
        SysQ_44 = 1744,
        [Description("Q系列系统的第45个子网站")]
        SysQ_45 = 1745,
        [Description("Q系列系统的第46个子网站")]
        SysQ_46 = 1746,
        [Description("Q系列系统的第47个子网站")]
        SysQ_47 = 1747,
        [Description("Q系列系统的第48个子网站")]
        SysQ_48 = 1748,
        [Description("Q系列系统的第49个子网站")]
        SysQ_49 = 1749,
        [Description("Q系列系统的第50个子网站")]
        SysQ_50 = 1750,
        [Description("Q系列系统的第51个子网站")]
        SysQ_51 = 1751,
        [Description("Q系列系统的第52个子网站")]
        SysQ_52 = 1752,
        [Description("Q系列系统的第53个子网站")]
        SysQ_53 = 1753,
        [Description("Q系列系统的第54个子网站")]
        SysQ_54 = 1754,
        [Description("Q系列系统的第55个子网站")]
        SysQ_55 = 1755,
        [Description("Q系列系统的第56个子网站")]
        SysQ_56 = 1756,
        [Description("Q系列系统的第57个子网站")]
        SysQ_57 = 1757,
        [Description("Q系列系统的第58个子网站")]
        SysQ_58 = 1758,
        [Description("Q系列系统的第59个子网站")]
        SysQ_59 = 1759,
        [Description("Q系列系统的第60个子网站")]
        SysQ_60 = 1760,
        [Description("Q系列系统的第61个子网站")]
        SysQ_61 = 1761,
        [Description("Q系列系统的第62个子网站")]
        SysQ_62 = 1762,
        [Description("Q系列系统的第63个子网站")]
        SysQ_63 = 1763,
        [Description("Q系列系统的第64个子网站")]
        SysQ_64 = 1764,
        [Description("Q系列系统的第65个子网站")]
        SysQ_65 = 1765,
        [Description("Q系列系统的第66个子网站")]
        SysQ_66 = 1766,
        [Description("Q系列系统的第67个子网站")]
        SysQ_67 = 1767,
        [Description("Q系列系统的第68个子网站")]
        SysQ_68 = 1768,
        [Description("Q系列系统的第69个子网站")]
        SysQ_69 = 1769,
        [Description("Q系列系统的第70个子网站")]
        SysQ_70 = 1770,
        [Description("Q系列系统的第71个子网站")]
        SysQ_71 = 1771,
        [Description("Q系列系统的第72个子网站")]
        SysQ_72 = 1772,
        [Description("Q系列系统的第73个子网站")]
        SysQ_73 = 1773,
        [Description("Q系列系统的第74个子网站")]
        SysQ_74 = 1774,
        [Description("Q系列系统的第75个子网站")]
        SysQ_75 = 1775,
        [Description("Q系列系统的第76个子网站")]
        SysQ_76 = 1776,
        [Description("Q系列系统的第77个子网站")]
        SysQ_77 = 1777,
        [Description("Q系列系统的第78个子网站")]
        SysQ_78 = 1778,
        [Description("Q系列系统的第79个子网站")]
        SysQ_79 = 1779,
        [Description("Q系列系统的第80个子网站")]
        SysQ_80 = 1780,
        [Description("Q系列系统的第81个子网站")]
        SysQ_81 = 1781,
        [Description("Q系列系统的第82个子网站")]
        SysQ_82 = 1782,
        [Description("Q系列系统的第83个子网站")]
        SysQ_83 = 1783,
        [Description("Q系列系统的第84个子网站")]
        SysQ_84 = 1784,
        [Description("Q系列系统的第85个子网站")]
        SysQ_85 = 1785,
        [Description("Q系列系统的第86个子网站")]
        SysQ_86 = 1786,
        [Description("Q系列系统的第87个子网站")]
        SysQ_87 = 1787,
        [Description("Q系列系统的第88个子网站")]
        SysQ_88 = 1788,
        [Description("Q系列系统的第89个子网站")]
        SysQ_89 = 1789,
        [Description("Q系列系统的第90个子网站")]
        SysQ_90 = 1790,
        [Description("Q系列系统的第91个子网站")]
        SysQ_91 = 1791,
        [Description("Q系列系统的第92个子网站")]
        SysQ_92 = 1792,
        [Description("Q系列系统的第93个子网站")]
        SysQ_93 = 1793,
        [Description("Q系列系统的第94个子网站")]
        SysQ_94 = 1794,
        [Description("Q系列系统的第95个子网站")]
        SysQ_95 = 1795,
        [Description("Q系列系统的第96个子网站")]
        SysQ_96 = 1796,
        [Description("Q系列系统的第97个子网站")]
        SysQ_97 = 1797,
        [Description("Q系列系统的第98个子网站")]
        SysQ_98 = 1798,
        [Description("Q系列系统的第99个子网站")]
        SysQ_99 = 1799,
        [Description("R系列系统的第1个子网站")]
        SysR_01 = 1801,
        [Description("R系列系统的第2个子网站")]
        SysR_02 = 1802,
        [Description("R系列系统的第3个子网站")]
        SysR_03 = 1803,
        [Description("R系列系统的第4个子网站")]
        SysR_04 = 1804,
        [Description("R系列系统的第5个子网站")]
        SysR_05 = 1805,
        [Description("R系列系统的第6个子网站")]
        SysR_06 = 1806,
        [Description("R系列系统的第7个子网站")]
        SysR_07 = 1807,
        [Description("R系列系统的第8个子网站")]
        SysR_08 = 1808,
        [Description("R系列系统的第9个子网站")]
        SysR_09 = 1809,
        [Description("R系列系统的第10个子网站")]
        SysR_10 = 1810,
        [Description("R系列系统的第11个子网站")]
        SysR_11 = 1811,
        [Description("R系列系统的第12个子网站")]
        SysR_12 = 1812,
        [Description("R系列系统的第13个子网站")]
        SysR_13 = 1813,
        [Description("R系列系统的第14个子网站")]
        SysR_14 = 1814,
        [Description("R系列系统的第15个子网站")]
        SysR_15 = 1815,
        [Description("R系列系统的第16个子网站")]
        SysR_16 = 1816,
        [Description("R系列系统的第17个子网站")]
        SysR_17 = 1817,
        [Description("R系列系统的第18个子网站")]
        SysR_18 = 1818,
        [Description("R系列系统的第19个子网站")]
        SysR_19 = 1819,
        [Description("R系列系统的第20个子网站")]
        SysR_20 = 1820,
        [Description("R系列系统的第21个子网站")]
        SysR_21 = 1821,
        [Description("R系列系统的第22个子网站")]
        SysR_22 = 1822,
        [Description("R系列系统的第23个子网站")]
        SysR_23 = 1823,
        [Description("R系列系统的第24个子网站")]
        SysR_24 = 1824,
        [Description("R系列系统的第25个子网站")]
        SysR_25 = 1825,
        [Description("R系列系统的第26个子网站")]
        SysR_26 = 1826,
        [Description("R系列系统的第27个子网站")]
        SysR_27 = 1827,
        [Description("R系列系统的第28个子网站")]
        SysR_28 = 1828,
        [Description("R系列系统的第29个子网站")]
        SysR_29 = 1829,
        [Description("R系列系统的第30个子网站")]
        SysR_30 = 1830,
        [Description("R系列系统的第31个子网站")]
        SysR_31 = 1831,
        [Description("R系列系统的第32个子网站")]
        SysR_32 = 1832,
        [Description("R系列系统的第33个子网站")]
        SysR_33 = 1833,
        [Description("R系列系统的第34个子网站")]
        SysR_34 = 1834,
        [Description("R系列系统的第35个子网站")]
        SysR_35 = 1835,
        [Description("R系列系统的第36个子网站")]
        SysR_36 = 1836,
        [Description("R系列系统的第37个子网站")]
        SysR_37 = 1837,
        [Description("R系列系统的第38个子网站")]
        SysR_38 = 1838,
        [Description("R系列系统的第39个子网站")]
        SysR_39 = 1839,
        [Description("R系列系统的第40个子网站")]
        SysR_40 = 1840,
        [Description("R系列系统的第41个子网站")]
        SysR_41 = 1841,
        [Description("R系列系统的第42个子网站")]
        SysR_42 = 1842,
        [Description("R系列系统的第43个子网站")]
        SysR_43 = 1843,
        [Description("R系列系统的第44个子网站")]
        SysR_44 = 1844,
        [Description("R系列系统的第45个子网站")]
        SysR_45 = 1845,
        [Description("R系列系统的第46个子网站")]
        SysR_46 = 1846,
        [Description("R系列系统的第47个子网站")]
        SysR_47 = 1847,
        [Description("R系列系统的第48个子网站")]
        SysR_48 = 1848,
        [Description("R系列系统的第49个子网站")]
        SysR_49 = 1849,
        [Description("R系列系统的第50个子网站")]
        SysR_50 = 1850,
        [Description("R系列系统的第51个子网站")]
        SysR_51 = 1851,
        [Description("R系列系统的第52个子网站")]
        SysR_52 = 1852,
        [Description("R系列系统的第53个子网站")]
        SysR_53 = 1853,
        [Description("R系列系统的第54个子网站")]
        SysR_54 = 1854,
        [Description("R系列系统的第55个子网站")]
        SysR_55 = 1855,
        [Description("R系列系统的第56个子网站")]
        SysR_56 = 1856,
        [Description("R系列系统的第57个子网站")]
        SysR_57 = 1857,
        [Description("R系列系统的第58个子网站")]
        SysR_58 = 1858,
        [Description("R系列系统的第59个子网站")]
        SysR_59 = 1859,
        [Description("R系列系统的第60个子网站")]
        SysR_60 = 1860,
        [Description("R系列系统的第61个子网站")]
        SysR_61 = 1861,
        [Description("R系列系统的第62个子网站")]
        SysR_62 = 1862,
        [Description("R系列系统的第63个子网站")]
        SysR_63 = 1863,
        [Description("R系列系统的第64个子网站")]
        SysR_64 = 1864,
        [Description("R系列系统的第65个子网站")]
        SysR_65 = 1865,
        [Description("R系列系统的第66个子网站")]
        SysR_66 = 1866,
        [Description("R系列系统的第67个子网站")]
        SysR_67 = 1867,
        [Description("R系列系统的第68个子网站")]
        SysR_68 = 1868,
        [Description("R系列系统的第69个子网站")]
        SysR_69 = 1869,
        [Description("R系列系统的第70个子网站")]
        SysR_70 = 1870,
        [Description("R系列系统的第71个子网站")]
        SysR_71 = 1871,
        [Description("R系列系统的第72个子网站")]
        SysR_72 = 1872,
        [Description("R系列系统的第73个子网站")]
        SysR_73 = 1873,
        [Description("R系列系统的第74个子网站")]
        SysR_74 = 1874,
        [Description("R系列系统的第75个子网站")]
        SysR_75 = 1875,
        [Description("R系列系统的第76个子网站")]
        SysR_76 = 1876,
        [Description("R系列系统的第77个子网站")]
        SysR_77 = 1877,
        [Description("R系列系统的第78个子网站")]
        SysR_78 = 1878,
        [Description("R系列系统的第79个子网站")]
        SysR_79 = 1879,
        [Description("R系列系统的第80个子网站")]
        SysR_80 = 1880,
        [Description("R系列系统的第81个子网站")]
        SysR_81 = 1881,
        [Description("R系列系统的第82个子网站")]
        SysR_82 = 1882,
        [Description("R系列系统的第83个子网站")]
        SysR_83 = 1883,
        [Description("R系列系统的第84个子网站")]
        SysR_84 = 1884,
        [Description("R系列系统的第85个子网站")]
        SysR_85 = 1885,
        [Description("R系列系统的第86个子网站")]
        SysR_86 = 1886,
        [Description("R系列系统的第87个子网站")]
        SysR_87 = 1887,
        [Description("R系列系统的第88个子网站")]
        SysR_88 = 1888,
        [Description("R系列系统的第89个子网站")]
        SysR_89 = 1889,
        [Description("R系列系统的第90个子网站")]
        SysR_90 = 1890,
        [Description("R系列系统的第91个子网站")]
        SysR_91 = 1891,
        [Description("R系列系统的第92个子网站")]
        SysR_92 = 1892,
        [Description("R系列系统的第93个子网站")]
        SysR_93 = 1893,
        [Description("R系列系统的第94个子网站")]
        SysR_94 = 1894,
        [Description("R系列系统的第95个子网站")]
        SysR_95 = 1895,
        [Description("R系列系统的第96个子网站")]
        SysR_96 = 1896,
        [Description("R系列系统的第97个子网站")]
        SysR_97 = 1897,
        [Description("R系列系统的第98个子网站")]
        SysR_98 = 1898,
        [Description("R系列系统的第99个子网站")]
        SysR_99 = 1899,
        [Description("S系列系统的第1个子网站")]
        SysS_01 = 1901,
        [Description("S系列系统的第2个子网站")]
        SysS_02 = 1902,
        [Description("S系列系统的第3个子网站")]
        SysS_03 = 1903,
        [Description("S系列系统的第4个子网站")]
        SysS_04 = 1904,
        [Description("S系列系统的第5个子网站")]
        SysS_05 = 1905,
        [Description("S系列系统的第6个子网站")]
        SysS_06 = 1906,
        [Description("S系列系统的第7个子网站")]
        SysS_07 = 1907,
        [Description("S系列系统的第8个子网站")]
        SysS_08 = 1908,
        [Description("S系列系统的第9个子网站")]
        SysS_09 = 1909,
        [Description("S系列系统的第10个子网站")]
        SysS_10 = 1910,
        [Description("S系列系统的第11个子网站")]
        SysS_11 = 1911,
        [Description("S系列系统的第12个子网站")]
        SysS_12 = 1912,
        [Description("S系列系统的第13个子网站")]
        SysS_13 = 1913,
        [Description("S系列系统的第14个子网站")]
        SysS_14 = 1914,
        [Description("S系列系统的第15个子网站")]
        SysS_15 = 1915,
        [Description("S系列系统的第16个子网站")]
        SysS_16 = 1916,
        [Description("S系列系统的第17个子网站")]
        SysS_17 = 1917,
        [Description("S系列系统的第18个子网站")]
        SysS_18 = 1918,
        [Description("S系列系统的第19个子网站")]
        SysS_19 = 1919,
        [Description("S系列系统的第20个子网站")]
        SysS_20 = 1920,
        [Description("S系列系统的第21个子网站")]
        SysS_21 = 1921,
        [Description("S系列系统的第22个子网站")]
        SysS_22 = 1922,
        [Description("S系列系统的第23个子网站")]
        SysS_23 = 1923,
        [Description("S系列系统的第24个子网站")]
        SysS_24 = 1924,
        [Description("S系列系统的第25个子网站")]
        SysS_25 = 1925,
        [Description("S系列系统的第26个子网站")]
        SysS_26 = 1926,
        [Description("S系列系统的第27个子网站")]
        SysS_27 = 1927,
        [Description("S系列系统的第28个子网站")]
        SysS_28 = 1928,
        [Description("S系列系统的第29个子网站")]
        SysS_29 = 1929,
        [Description("S系列系统的第30个子网站")]
        SysS_30 = 1930,
        [Description("S系列系统的第31个子网站")]
        SysS_31 = 1931,
        [Description("S系列系统的第32个子网站")]
        SysS_32 = 1932,
        [Description("S系列系统的第33个子网站")]
        SysS_33 = 1933,
        [Description("S系列系统的第34个子网站")]
        SysS_34 = 1934,
        [Description("S系列系统的第35个子网站")]
        SysS_35 = 1935,
        [Description("S系列系统的第36个子网站")]
        SysS_36 = 1936,
        [Description("S系列系统的第37个子网站")]
        SysS_37 = 1937,
        [Description("S系列系统的第38个子网站")]
        SysS_38 = 1938,
        [Description("S系列系统的第39个子网站")]
        SysS_39 = 1939,
        [Description("S系列系统的第40个子网站")]
        SysS_40 = 1940,
        [Description("S系列系统的第41个子网站")]
        SysS_41 = 1941,
        [Description("S系列系统的第42个子网站")]
        SysS_42 = 1942,
        [Description("S系列系统的第43个子网站")]
        SysS_43 = 1943,
        [Description("S系列系统的第44个子网站")]
        SysS_44 = 1944,
        [Description("S系列系统的第45个子网站")]
        SysS_45 = 1945,
        [Description("S系列系统的第46个子网站")]
        SysS_46 = 1946,
        [Description("S系列系统的第47个子网站")]
        SysS_47 = 1947,
        [Description("S系列系统的第48个子网站")]
        SysS_48 = 1948,
        [Description("S系列系统的第49个子网站")]
        SysS_49 = 1949,
        [Description("S系列系统的第50个子网站")]
        SysS_50 = 1950,
        [Description("S系列系统的第51个子网站")]
        SysS_51 = 1951,
        [Description("S系列系统的第52个子网站")]
        SysS_52 = 1952,
        [Description("S系列系统的第53个子网站")]
        SysS_53 = 1953,
        [Description("S系列系统的第54个子网站")]
        SysS_54 = 1954,
        [Description("S系列系统的第55个子网站")]
        SysS_55 = 1955,
        [Description("S系列系统的第56个子网站")]
        SysS_56 = 1956,
        [Description("S系列系统的第57个子网站")]
        SysS_57 = 1957,
        [Description("S系列系统的第58个子网站")]
        SysS_58 = 1958,
        [Description("S系列系统的第59个子网站")]
        SysS_59 = 1959,
        [Description("S系列系统的第60个子网站")]
        SysS_60 = 1960,
        [Description("S系列系统的第61个子网站")]
        SysS_61 = 1961,
        [Description("S系列系统的第62个子网站")]
        SysS_62 = 1962,
        [Description("S系列系统的第63个子网站")]
        SysS_63 = 1963,
        [Description("S系列系统的第64个子网站")]
        SysS_64 = 1964,
        [Description("S系列系统的第65个子网站")]
        SysS_65 = 1965,
        [Description("S系列系统的第66个子网站")]
        SysS_66 = 1966,
        [Description("S系列系统的第67个子网站")]
        SysS_67 = 1967,
        [Description("S系列系统的第68个子网站")]
        SysS_68 = 1968,
        [Description("S系列系统的第69个子网站")]
        SysS_69 = 1969,
        [Description("S系列系统的第70个子网站")]
        SysS_70 = 1970,
        [Description("S系列系统的第71个子网站")]
        SysS_71 = 1971,
        [Description("S系列系统的第72个子网站")]
        SysS_72 = 1972,
        [Description("S系列系统的第73个子网站")]
        SysS_73 = 1973,
        [Description("S系列系统的第74个子网站")]
        SysS_74 = 1974,
        [Description("S系列系统的第75个子网站")]
        SysS_75 = 1975,
        [Description("S系列系统的第76个子网站")]
        SysS_76 = 1976,
        [Description("S系列系统的第77个子网站")]
        SysS_77 = 1977,
        [Description("S系列系统的第78个子网站")]
        SysS_78 = 1978,
        [Description("S系列系统的第79个子网站")]
        SysS_79 = 1979,
        [Description("S系列系统的第80个子网站")]
        SysS_80 = 1980,
        [Description("S系列系统的第81个子网站")]
        SysS_81 = 1981,
        [Description("S系列系统的第82个子网站")]
        SysS_82 = 1982,
        [Description("S系列系统的第83个子网站")]
        SysS_83 = 1983,
        [Description("S系列系统的第84个子网站")]
        SysS_84 = 1984,
        [Description("S系列系统的第85个子网站")]
        SysS_85 = 1985,
        [Description("S系列系统的第86个子网站")]
        SysS_86 = 1986,
        [Description("S系列系统的第87个子网站")]
        SysS_87 = 1987,
        [Description("S系列系统的第88个子网站")]
        SysS_88 = 1988,
        [Description("S系列系统的第89个子网站")]
        SysS_89 = 1989,
        [Description("S系列系统的第90个子网站")]
        SysS_90 = 1990,
        [Description("S系列系统的第91个子网站")]
        SysS_91 = 1991,
        [Description("S系列系统的第92个子网站")]
        SysS_92 = 1992,
        [Description("S系列系统的第93个子网站")]
        SysS_93 = 1993,
        [Description("S系列系统的第94个子网站")]
        SysS_94 = 1994,
        [Description("S系列系统的第95个子网站")]
        SysS_95 = 1995,
        [Description("S系列系统的第96个子网站")]
        SysS_96 = 1996,
        [Description("S系列系统的第97个子网站")]
        SysS_97 = 1997,
        [Description("S系列系统的第98个子网站")]
        SysS_98 = 1998,
        [Description("S系列系统的第99个子网站")]
        SysS_99 = 1999,
        [Description("T系列系统的第1个子网站")]
        SysT_01 = 2001,
        [Description("T系列系统的第2个子网站")]
        SysT_02 = 2002,
        [Description("T系列系统的第3个子网站")]
        SysT_03 = 2003,
        [Description("T系列系统的第4个子网站")]
        SysT_04 = 2004,
        [Description("T系列系统的第5个子网站")]
        SysT_05 = 2005,
        [Description("T系列系统的第6个子网站")]
        SysT_06 = 2006,
        [Description("T系列系统的第7个子网站")]
        SysT_07 = 2007,
        [Description("T系列系统的第8个子网站")]
        SysT_08 = 2008,
        [Description("T系列系统的第9个子网站")]
        SysT_09 = 2009,
        [Description("T系列系统的第10个子网站")]
        SysT_10 = 2010,
        [Description("T系列系统的第11个子网站")]
        SysT_11 = 2011,
        [Description("T系列系统的第12个子网站")]
        SysT_12 = 2012,
        [Description("T系列系统的第13个子网站")]
        SysT_13 = 2013,
        [Description("T系列系统的第14个子网站")]
        SysT_14 = 2014,
        [Description("T系列系统的第15个子网站")]
        SysT_15 = 2015,
        [Description("T系列系统的第16个子网站")]
        SysT_16 = 2016,
        [Description("T系列系统的第17个子网站")]
        SysT_17 = 2017,
        [Description("T系列系统的第18个子网站")]
        SysT_18 = 2018,
        [Description("T系列系统的第19个子网站")]
        SysT_19 = 2019,
        [Description("T系列系统的第20个子网站")]
        SysT_20 = 2020,
        [Description("T系列系统的第21个子网站")]
        SysT_21 = 2021,
        [Description("T系列系统的第22个子网站")]
        SysT_22 = 2022,
        [Description("T系列系统的第23个子网站")]
        SysT_23 = 2023,
        [Description("T系列系统的第24个子网站")]
        SysT_24 = 2024,
        [Description("T系列系统的第25个子网站")]
        SysT_25 = 2025,
        [Description("T系列系统的第26个子网站")]
        SysT_26 = 2026,
        [Description("T系列系统的第27个子网站")]
        SysT_27 = 2027,
        [Description("T系列系统的第28个子网站")]
        SysT_28 = 2028,
        [Description("T系列系统的第29个子网站")]
        SysT_29 = 2029,
        [Description("T系列系统的第30个子网站")]
        SysT_30 = 2030,
        [Description("T系列系统的第31个子网站")]
        SysT_31 = 2031,
        [Description("T系列系统的第32个子网站")]
        SysT_32 = 2032,
        [Description("T系列系统的第33个子网站")]
        SysT_33 = 2033,
        [Description("T系列系统的第34个子网站")]
        SysT_34 = 2034,
        [Description("T系列系统的第35个子网站")]
        SysT_35 = 2035,
        [Description("T系列系统的第36个子网站")]
        SysT_36 = 2036,
        [Description("T系列系统的第37个子网站")]
        SysT_37 = 2037,
        [Description("T系列系统的第38个子网站")]
        SysT_38 = 2038,
        [Description("T系列系统的第39个子网站")]
        SysT_39 = 2039,
        [Description("T系列系统的第40个子网站")]
        SysT_40 = 2040,
        [Description("T系列系统的第41个子网站")]
        SysT_41 = 2041,
        [Description("T系列系统的第42个子网站")]
        SysT_42 = 2042,
        [Description("T系列系统的第43个子网站")]
        SysT_43 = 2043,
        [Description("T系列系统的第44个子网站")]
        SysT_44 = 2044,
        [Description("T系列系统的第45个子网站")]
        SysT_45 = 2045,
        [Description("T系列系统的第46个子网站")]
        SysT_46 = 2046,
        [Description("T系列系统的第47个子网站")]
        SysT_47 = 2047,
        [Description("T系列系统的第48个子网站")]
        SysT_48 = 2048,
        [Description("T系列系统的第49个子网站")]
        SysT_49 = 2049,
        [Description("T系列系统的第50个子网站")]
        SysT_50 = 2050,
        [Description("T系列系统的第51个子网站")]
        SysT_51 = 2051,
        [Description("T系列系统的第52个子网站")]
        SysT_52 = 2052,
        [Description("T系列系统的第53个子网站")]
        SysT_53 = 2053,
        [Description("T系列系统的第54个子网站")]
        SysT_54 = 2054,
        [Description("T系列系统的第55个子网站")]
        SysT_55 = 2055,
        [Description("T系列系统的第56个子网站")]
        SysT_56 = 2056,
        [Description("T系列系统的第57个子网站")]
        SysT_57 = 2057,
        [Description("T系列系统的第58个子网站")]
        SysT_58 = 2058,
        [Description("T系列系统的第59个子网站")]
        SysT_59 = 2059,
        [Description("T系列系统的第60个子网站")]
        SysT_60 = 2060,
        [Description("T系列系统的第61个子网站")]
        SysT_61 = 2061,
        [Description("T系列系统的第62个子网站")]
        SysT_62 = 2062,
        [Description("T系列系统的第63个子网站")]
        SysT_63 = 2063,
        [Description("T系列系统的第64个子网站")]
        SysT_64 = 2064,
        [Description("T系列系统的第65个子网站")]
        SysT_65 = 2065,
        [Description("T系列系统的第66个子网站")]
        SysT_66 = 2066,
        [Description("T系列系统的第67个子网站")]
        SysT_67 = 2067,
        [Description("T系列系统的第68个子网站")]
        SysT_68 = 2068,
        [Description("T系列系统的第69个子网站")]
        SysT_69 = 2069,
        [Description("T系列系统的第70个子网站")]
        SysT_70 = 2070,
        [Description("T系列系统的第71个子网站")]
        SysT_71 = 2071,
        [Description("T系列系统的第72个子网站")]
        SysT_72 = 2072,
        [Description("T系列系统的第73个子网站")]
        SysT_73 = 2073,
        [Description("T系列系统的第74个子网站")]
        SysT_74 = 2074,
        [Description("T系列系统的第75个子网站")]
        SysT_75 = 2075,
        [Description("T系列系统的第76个子网站")]
        SysT_76 = 2076,
        [Description("T系列系统的第77个子网站")]
        SysT_77 = 2077,
        [Description("T系列系统的第78个子网站")]
        SysT_78 = 2078,
        [Description("T系列系统的第79个子网站")]
        SysT_79 = 2079,
        [Description("T系列系统的第80个子网站")]
        SysT_80 = 2080,
        [Description("T系列系统的第81个子网站")]
        SysT_81 = 2081,
        [Description("T系列系统的第82个子网站")]
        SysT_82 = 2082,
        [Description("T系列系统的第83个子网站")]
        SysT_83 = 2083,
        [Description("T系列系统的第84个子网站")]
        SysT_84 = 2084,
        [Description("T系列系统的第85个子网站")]
        SysT_85 = 2085,
        [Description("T系列系统的第86个子网站")]
        SysT_86 = 2086,
        [Description("T系列系统的第87个子网站")]
        SysT_87 = 2087,
        [Description("T系列系统的第88个子网站")]
        SysT_88 = 2088,
        [Description("T系列系统的第89个子网站")]
        SysT_89 = 2089,
        [Description("T系列系统的第90个子网站")]
        SysT_90 = 2090,
        [Description("T系列系统的第91个子网站")]
        SysT_91 = 2091,
        [Description("T系列系统的第92个子网站")]
        SysT_92 = 2092,
        [Description("T系列系统的第93个子网站")]
        SysT_93 = 2093,
        [Description("T系列系统的第94个子网站")]
        SysT_94 = 2094,
        [Description("T系列系统的第95个子网站")]
        SysT_95 = 2095,
        [Description("T系列系统的第96个子网站")]
        SysT_96 = 2096,
        [Description("T系列系统的第97个子网站")]
        SysT_97 = 2097,
        [Description("T系列系统的第98个子网站")]
        SysT_98 = 2098,
        [Description("T系列系统的第99个子网站")]
        SysT_99 = 2099,
        [Description("U系列系统的第1个子网站")]
        SysU_01 = 2101,
        [Description("U系列系统的第2个子网站")]
        SysU_02 = 2102,
        [Description("U系列系统的第3个子网站")]
        SysU_03 = 2103,
        [Description("U系列系统的第4个子网站")]
        SysU_04 = 2104,
        [Description("U系列系统的第5个子网站")]
        SysU_05 = 2105,
        [Description("U系列系统的第6个子网站")]
        SysU_06 = 2106,
        [Description("U系列系统的第7个子网站")]
        SysU_07 = 2107,
        [Description("U系列系统的第8个子网站")]
        SysU_08 = 2108,
        [Description("U系列系统的第9个子网站")]
        SysU_09 = 2109,
        [Description("U系列系统的第10个子网站")]
        SysU_10 = 2110,
        [Description("U系列系统的第11个子网站")]
        SysU_11 = 2111,
        [Description("U系列系统的第12个子网站")]
        SysU_12 = 2112,
        [Description("U系列系统的第13个子网站")]
        SysU_13 = 2113,
        [Description("U系列系统的第14个子网站")]
        SysU_14 = 2114,
        [Description("U系列系统的第15个子网站")]
        SysU_15 = 2115,
        [Description("U系列系统的第16个子网站")]
        SysU_16 = 2116,
        [Description("U系列系统的第17个子网站")]
        SysU_17 = 2117,
        [Description("U系列系统的第18个子网站")]
        SysU_18 = 2118,
        [Description("U系列系统的第19个子网站")]
        SysU_19 = 2119,
        [Description("U系列系统的第20个子网站")]
        SysU_20 = 2120,
        [Description("U系列系统的第21个子网站")]
        SysU_21 = 2121,
        [Description("U系列系统的第22个子网站")]
        SysU_22 = 2122,
        [Description("U系列系统的第23个子网站")]
        SysU_23 = 2123,
        [Description("U系列系统的第24个子网站")]
        SysU_24 = 2124,
        [Description("U系列系统的第25个子网站")]
        SysU_25 = 2125,
        [Description("U系列系统的第26个子网站")]
        SysU_26 = 2126,
        [Description("U系列系统的第27个子网站")]
        SysU_27 = 2127,
        [Description("U系列系统的第28个子网站")]
        SysU_28 = 2128,
        [Description("U系列系统的第29个子网站")]
        SysU_29 = 2129,
        [Description("U系列系统的第30个子网站")]
        SysU_30 = 2130,
        [Description("U系列系统的第31个子网站")]
        SysU_31 = 2131,
        [Description("U系列系统的第32个子网站")]
        SysU_32 = 2132,
        [Description("U系列系统的第33个子网站")]
        SysU_33 = 2133,
        [Description("U系列系统的第34个子网站")]
        SysU_34 = 2134,
        [Description("U系列系统的第35个子网站")]
        SysU_35 = 2135,
        [Description("U系列系统的第36个子网站")]
        SysU_36 = 2136,
        [Description("U系列系统的第37个子网站")]
        SysU_37 = 2137,
        [Description("U系列系统的第38个子网站")]
        SysU_38 = 2138,
        [Description("U系列系统的第39个子网站")]
        SysU_39 = 2139,
        [Description("U系列系统的第40个子网站")]
        SysU_40 = 2140,
        [Description("U系列系统的第41个子网站")]
        SysU_41 = 2141,
        [Description("U系列系统的第42个子网站")]
        SysU_42 = 2142,
        [Description("U系列系统的第43个子网站")]
        SysU_43 = 2143,
        [Description("U系列系统的第44个子网站")]
        SysU_44 = 2144,
        [Description("U系列系统的第45个子网站")]
        SysU_45 = 2145,
        [Description("U系列系统的第46个子网站")]
        SysU_46 = 2146,
        [Description("U系列系统的第47个子网站")]
        SysU_47 = 2147,
        [Description("U系列系统的第48个子网站")]
        SysU_48 = 2148,
        [Description("U系列系统的第49个子网站")]
        SysU_49 = 2149,
        [Description("U系列系统的第50个子网站")]
        SysU_50 = 2150,
        [Description("U系列系统的第51个子网站")]
        SysU_51 = 2151,
        [Description("U系列系统的第52个子网站")]
        SysU_52 = 2152,
        [Description("U系列系统的第53个子网站")]
        SysU_53 = 2153,
        [Description("U系列系统的第54个子网站")]
        SysU_54 = 2154,
        [Description("U系列系统的第55个子网站")]
        SysU_55 = 2155,
        [Description("U系列系统的第56个子网站")]
        SysU_56 = 2156,
        [Description("U系列系统的第57个子网站")]
        SysU_57 = 2157,
        [Description("U系列系统的第58个子网站")]
        SysU_58 = 2158,
        [Description("U系列系统的第59个子网站")]
        SysU_59 = 2159,
        [Description("U系列系统的第60个子网站")]
        SysU_60 = 2160,
        [Description("U系列系统的第61个子网站")]
        SysU_61 = 2161,
        [Description("U系列系统的第62个子网站")]
        SysU_62 = 2162,
        [Description("U系列系统的第63个子网站")]
        SysU_63 = 2163,
        [Description("U系列系统的第64个子网站")]
        SysU_64 = 2164,
        [Description("U系列系统的第65个子网站")]
        SysU_65 = 2165,
        [Description("U系列系统的第66个子网站")]
        SysU_66 = 2166,
        [Description("U系列系统的第67个子网站")]
        SysU_67 = 2167,
        [Description("U系列系统的第68个子网站")]
        SysU_68 = 2168,
        [Description("U系列系统的第69个子网站")]
        SysU_69 = 2169,
        [Description("U系列系统的第70个子网站")]
        SysU_70 = 2170,
        [Description("U系列系统的第71个子网站")]
        SysU_71 = 2171,
        [Description("U系列系统的第72个子网站")]
        SysU_72 = 2172,
        [Description("U系列系统的第73个子网站")]
        SysU_73 = 2173,
        [Description("U系列系统的第74个子网站")]
        SysU_74 = 2174,
        [Description("U系列系统的第75个子网站")]
        SysU_75 = 2175,
        [Description("U系列系统的第76个子网站")]
        SysU_76 = 2176,
        [Description("U系列系统的第77个子网站")]
        SysU_77 = 2177,
        [Description("U系列系统的第78个子网站")]
        SysU_78 = 2178,
        [Description("U系列系统的第79个子网站")]
        SysU_79 = 2179,
        [Description("U系列系统的第80个子网站")]
        SysU_80 = 2180,
        [Description("U系列系统的第81个子网站")]
        SysU_81 = 2181,
        [Description("U系列系统的第82个子网站")]
        SysU_82 = 2182,
        [Description("U系列系统的第83个子网站")]
        SysU_83 = 2183,
        [Description("U系列系统的第84个子网站")]
        SysU_84 = 2184,
        [Description("U系列系统的第85个子网站")]
        SysU_85 = 2185,
        [Description("U系列系统的第86个子网站")]
        SysU_86 = 2186,
        [Description("U系列系统的第87个子网站")]
        SysU_87 = 2187,
        [Description("U系列系统的第88个子网站")]
        SysU_88 = 2188,
        [Description("U系列系统的第89个子网站")]
        SysU_89 = 2189,
        [Description("U系列系统的第90个子网站")]
        SysU_90 = 2190,
        [Description("U系列系统的第91个子网站")]
        SysU_91 = 2191,
        [Description("U系列系统的第92个子网站")]
        SysU_92 = 2192,
        [Description("U系列系统的第93个子网站")]
        SysU_93 = 2193,
        [Description("U系列系统的第94个子网站")]
        SysU_94 = 2194,
        [Description("U系列系统的第95个子网站")]
        SysU_95 = 2195,
        [Description("U系列系统的第96个子网站")]
        SysU_96 = 2196,
        [Description("U系列系统的第97个子网站")]
        SysU_97 = 2197,
        [Description("U系列系统的第98个子网站")]
        SysU_98 = 2198,
        [Description("U系列系统的第99个子网站")]
        SysU_99 = 2199,
        [Description("V系列系统的第1个子网站")]
        SysV_01 = 2201,
        [Description("V系列系统的第2个子网站")]
        SysV_02 = 2202,
        [Description("V系列系统的第3个子网站")]
        SysV_03 = 2203,
        [Description("V系列系统的第4个子网站")]
        SysV_04 = 2204,
        [Description("V系列系统的第5个子网站")]
        SysV_05 = 2205,
        [Description("V系列系统的第6个子网站")]
        SysV_06 = 2206,
        [Description("V系列系统的第7个子网站")]
        SysV_07 = 2207,
        [Description("V系列系统的第8个子网站")]
        SysV_08 = 2208,
        [Description("V系列系统的第9个子网站")]
        SysV_09 = 2209,
        [Description("V系列系统的第10个子网站")]
        SysV_10 = 2210,
        [Description("V系列系统的第11个子网站")]
        SysV_11 = 2211,
        [Description("V系列系统的第12个子网站")]
        SysV_12 = 2212,
        [Description("V系列系统的第13个子网站")]
        SysV_13 = 2213,
        [Description("V系列系统的第14个子网站")]
        SysV_14 = 2214,
        [Description("V系列系统的第15个子网站")]
        SysV_15 = 2215,
        [Description("V系列系统的第16个子网站")]
        SysV_16 = 2216,
        [Description("V系列系统的第17个子网站")]
        SysV_17 = 2217,
        [Description("V系列系统的第18个子网站")]
        SysV_18 = 2218,
        [Description("V系列系统的第19个子网站")]
        SysV_19 = 2219,
        [Description("V系列系统的第20个子网站")]
        SysV_20 = 2220,
        [Description("V系列系统的第21个子网站")]
        SysV_21 = 2221,
        [Description("V系列系统的第22个子网站")]
        SysV_22 = 2222,
        [Description("V系列系统的第23个子网站")]
        SysV_23 = 2223,
        [Description("V系列系统的第24个子网站")]
        SysV_24 = 2224,
        [Description("V系列系统的第25个子网站")]
        SysV_25 = 2225,
        [Description("V系列系统的第26个子网站")]
        SysV_26 = 2226,
        [Description("V系列系统的第27个子网站")]
        SysV_27 = 2227,
        [Description("V系列系统的第28个子网站")]
        SysV_28 = 2228,
        [Description("V系列系统的第29个子网站")]
        SysV_29 = 2229,
        [Description("V系列系统的第30个子网站")]
        SysV_30 = 2230,
        [Description("V系列系统的第31个子网站")]
        SysV_31 = 2231,
        [Description("V系列系统的第32个子网站")]
        SysV_32 = 2232,
        [Description("V系列系统的第33个子网站")]
        SysV_33 = 2233,
        [Description("V系列系统的第34个子网站")]
        SysV_34 = 2234,
        [Description("V系列系统的第35个子网站")]
        SysV_35 = 2235,
        [Description("V系列系统的第36个子网站")]
        SysV_36 = 2236,
        [Description("V系列系统的第37个子网站")]
        SysV_37 = 2237,
        [Description("V系列系统的第38个子网站")]
        SysV_38 = 2238,
        [Description("V系列系统的第39个子网站")]
        SysV_39 = 2239,
        [Description("V系列系统的第40个子网站")]
        SysV_40 = 2240,
        [Description("V系列系统的第41个子网站")]
        SysV_41 = 2241,
        [Description("V系列系统的第42个子网站")]
        SysV_42 = 2242,
        [Description("V系列系统的第43个子网站")]
        SysV_43 = 2243,
        [Description("V系列系统的第44个子网站")]
        SysV_44 = 2244,
        [Description("V系列系统的第45个子网站")]
        SysV_45 = 2245,
        [Description("V系列系统的第46个子网站")]
        SysV_46 = 2246,
        [Description("V系列系统的第47个子网站")]
        SysV_47 = 2247,
        [Description("V系列系统的第48个子网站")]
        SysV_48 = 2248,
        [Description("V系列系统的第49个子网站")]
        SysV_49 = 2249,
        [Description("V系列系统的第50个子网站")]
        SysV_50 = 2250,
        [Description("V系列系统的第51个子网站")]
        SysV_51 = 2251,
        [Description("V系列系统的第52个子网站")]
        SysV_52 = 2252,
        [Description("V系列系统的第53个子网站")]
        SysV_53 = 2253,
        [Description("V系列系统的第54个子网站")]
        SysV_54 = 2254,
        [Description("V系列系统的第55个子网站")]
        SysV_55 = 2255,
        [Description("V系列系统的第56个子网站")]
        SysV_56 = 2256,
        [Description("V系列系统的第57个子网站")]
        SysV_57 = 2257,
        [Description("V系列系统的第58个子网站")]
        SysV_58 = 2258,
        [Description("V系列系统的第59个子网站")]
        SysV_59 = 2259,
        [Description("V系列系统的第60个子网站")]
        SysV_60 = 2260,
        [Description("V系列系统的第61个子网站")]
        SysV_61 = 2261,
        [Description("V系列系统的第62个子网站")]
        SysV_62 = 2262,
        [Description("V系列系统的第63个子网站")]
        SysV_63 = 2263,
        [Description("V系列系统的第64个子网站")]
        SysV_64 = 2264,
        [Description("V系列系统的第65个子网站")]
        SysV_65 = 2265,
        [Description("V系列系统的第66个子网站")]
        SysV_66 = 2266,
        [Description("V系列系统的第67个子网站")]
        SysV_67 = 2267,
        [Description("V系列系统的第68个子网站")]
        SysV_68 = 2268,
        [Description("V系列系统的第69个子网站")]
        SysV_69 = 2269,
        [Description("V系列系统的第70个子网站")]
        SysV_70 = 2270,
        [Description("V系列系统的第71个子网站")]
        SysV_71 = 2271,
        [Description("V系列系统的第72个子网站")]
        SysV_72 = 2272,
        [Description("V系列系统的第73个子网站")]
        SysV_73 = 2273,
        [Description("V系列系统的第74个子网站")]
        SysV_74 = 2274,
        [Description("V系列系统的第75个子网站")]
        SysV_75 = 2275,
        [Description("V系列系统的第76个子网站")]
        SysV_76 = 2276,
        [Description("V系列系统的第77个子网站")]
        SysV_77 = 2277,
        [Description("V系列系统的第78个子网站")]
        SysV_78 = 2278,
        [Description("V系列系统的第79个子网站")]
        SysV_79 = 2279,
        [Description("V系列系统的第80个子网站")]
        SysV_80 = 2280,
        [Description("V系列系统的第81个子网站")]
        SysV_81 = 2281,
        [Description("V系列系统的第82个子网站")]
        SysV_82 = 2282,
        [Description("V系列系统的第83个子网站")]
        SysV_83 = 2283,
        [Description("V系列系统的第84个子网站")]
        SysV_84 = 2284,
        [Description("V系列系统的第85个子网站")]
        SysV_85 = 2285,
        [Description("V系列系统的第86个子网站")]
        SysV_86 = 2286,
        [Description("V系列系统的第87个子网站")]
        SysV_87 = 2287,
        [Description("V系列系统的第88个子网站")]
        SysV_88 = 2288,
        [Description("V系列系统的第89个子网站")]
        SysV_89 = 2289,
        [Description("V系列系统的第90个子网站")]
        SysV_90 = 2290,
        [Description("V系列系统的第91个子网站")]
        SysV_91 = 2291,
        [Description("V系列系统的第92个子网站")]
        SysV_92 = 2292,
        [Description("V系列系统的第93个子网站")]
        SysV_93 = 2293,
        [Description("V系列系统的第94个子网站")]
        SysV_94 = 2294,
        [Description("V系列系统的第95个子网站")]
        SysV_95 = 2295,
        [Description("V系列系统的第96个子网站")]
        SysV_96 = 2296,
        [Description("V系列系统的第97个子网站")]
        SysV_97 = 2297,
        [Description("V系列系统的第98个子网站")]
        SysV_98 = 2298,
        [Description("V系列系统的第99个子网站")]
        SysV_99 = 2299,
        [Description("W系列系统的第1个子网站")]
        SysW_01 = 2301,
        [Description("W系列系统的第2个子网站")]
        SysW_02 = 2302,
        [Description("W系列系统的第3个子网站")]
        SysW_03 = 2303,
        [Description("W系列系统的第4个子网站")]
        SysW_04 = 2304,
        [Description("W系列系统的第5个子网站")]
        SysW_05 = 2305,
        [Description("W系列系统的第6个子网站")]
        SysW_06 = 2306,
        [Description("W系列系统的第7个子网站")]
        SysW_07 = 2307,
        [Description("W系列系统的第8个子网站")]
        SysW_08 = 2308,
        [Description("W系列系统的第9个子网站")]
        SysW_09 = 2309,
        [Description("W系列系统的第10个子网站")]
        SysW_10 = 2310,
        [Description("W系列系统的第11个子网站")]
        SysW_11 = 2311,
        [Description("W系列系统的第12个子网站")]
        SysW_12 = 2312,
        [Description("W系列系统的第13个子网站")]
        SysW_13 = 2313,
        [Description("W系列系统的第14个子网站")]
        SysW_14 = 2314,
        [Description("W系列系统的第15个子网站")]
        SysW_15 = 2315,
        [Description("W系列系统的第16个子网站")]
        SysW_16 = 2316,
        [Description("W系列系统的第17个子网站")]
        SysW_17 = 2317,
        [Description("W系列系统的第18个子网站")]
        SysW_18 = 2318,
        [Description("W系列系统的第19个子网站")]
        SysW_19 = 2319,
        [Description("W系列系统的第20个子网站")]
        SysW_20 = 2320,
        [Description("W系列系统的第21个子网站")]
        SysW_21 = 2321,
        [Description("W系列系统的第22个子网站")]
        SysW_22 = 2322,
        [Description("W系列系统的第23个子网站")]
        SysW_23 = 2323,
        [Description("W系列系统的第24个子网站")]
        SysW_24 = 2324,
        [Description("W系列系统的第25个子网站")]
        SysW_25 = 2325,
        [Description("W系列系统的第26个子网站")]
        SysW_26 = 2326,
        [Description("W系列系统的第27个子网站")]
        SysW_27 = 2327,
        [Description("W系列系统的第28个子网站")]
        SysW_28 = 2328,
        [Description("W系列系统的第29个子网站")]
        SysW_29 = 2329,
        [Description("W系列系统的第30个子网站")]
        SysW_30 = 2330,
        [Description("W系列系统的第31个子网站")]
        SysW_31 = 2331,
        [Description("W系列系统的第32个子网站")]
        SysW_32 = 2332,
        [Description("W系列系统的第33个子网站")]
        SysW_33 = 2333,
        [Description("W系列系统的第34个子网站")]
        SysW_34 = 2334,
        [Description("W系列系统的第35个子网站")]
        SysW_35 = 2335,
        [Description("W系列系统的第36个子网站")]
        SysW_36 = 2336,
        [Description("W系列系统的第37个子网站")]
        SysW_37 = 2337,
        [Description("W系列系统的第38个子网站")]
        SysW_38 = 2338,
        [Description("W系列系统的第39个子网站")]
        SysW_39 = 2339,
        [Description("W系列系统的第40个子网站")]
        SysW_40 = 2340,
        [Description("W系列系统的第41个子网站")]
        SysW_41 = 2341,
        [Description("W系列系统的第42个子网站")]
        SysW_42 = 2342,
        [Description("W系列系统的第43个子网站")]
        SysW_43 = 2343,
        [Description("W系列系统的第44个子网站")]
        SysW_44 = 2344,
        [Description("W系列系统的第45个子网站")]
        SysW_45 = 2345,
        [Description("W系列系统的第46个子网站")]
        SysW_46 = 2346,
        [Description("W系列系统的第47个子网站")]
        SysW_47 = 2347,
        [Description("W系列系统的第48个子网站")]
        SysW_48 = 2348,
        [Description("W系列系统的第49个子网站")]
        SysW_49 = 2349,
        [Description("W系列系统的第50个子网站")]
        SysW_50 = 2350,
        [Description("W系列系统的第51个子网站")]
        SysW_51 = 2351,
        [Description("W系列系统的第52个子网站")]
        SysW_52 = 2352,
        [Description("W系列系统的第53个子网站")]
        SysW_53 = 2353,
        [Description("W系列系统的第54个子网站")]
        SysW_54 = 2354,
        [Description("W系列系统的第55个子网站")]
        SysW_55 = 2355,
        [Description("W系列系统的第56个子网站")]
        SysW_56 = 2356,
        [Description("W系列系统的第57个子网站")]
        SysW_57 = 2357,
        [Description("W系列系统的第58个子网站")]
        SysW_58 = 2358,
        [Description("W系列系统的第59个子网站")]
        SysW_59 = 2359,
        [Description("W系列系统的第60个子网站")]
        SysW_60 = 2360,
        [Description("W系列系统的第61个子网站")]
        SysW_61 = 2361,
        [Description("W系列系统的第62个子网站")]
        SysW_62 = 2362,
        [Description("W系列系统的第63个子网站")]
        SysW_63 = 2363,
        [Description("W系列系统的第64个子网站")]
        SysW_64 = 2364,
        [Description("W系列系统的第65个子网站")]
        SysW_65 = 2365,
        [Description("W系列系统的第66个子网站")]
        SysW_66 = 2366,
        [Description("W系列系统的第67个子网站")]
        SysW_67 = 2367,
        [Description("W系列系统的第68个子网站")]
        SysW_68 = 2368,
        [Description("W系列系统的第69个子网站")]
        SysW_69 = 2369,
        [Description("W系列系统的第70个子网站")]
        SysW_70 = 2370,
        [Description("W系列系统的第71个子网站")]
        SysW_71 = 2371,
        [Description("W系列系统的第72个子网站")]
        SysW_72 = 2372,
        [Description("W系列系统的第73个子网站")]
        SysW_73 = 2373,
        [Description("W系列系统的第74个子网站")]
        SysW_74 = 2374,
        [Description("W系列系统的第75个子网站")]
        SysW_75 = 2375,
        [Description("W系列系统的第76个子网站")]
        SysW_76 = 2376,
        [Description("W系列系统的第77个子网站")]
        SysW_77 = 2377,
        [Description("W系列系统的第78个子网站")]
        SysW_78 = 2378,
        [Description("W系列系统的第79个子网站")]
        SysW_79 = 2379,
        [Description("W系列系统的第80个子网站")]
        SysW_80 = 2380,
        [Description("W系列系统的第81个子网站")]
        SysW_81 = 2381,
        [Description("W系列系统的第82个子网站")]
        SysW_82 = 2382,
        [Description("W系列系统的第83个子网站")]
        SysW_83 = 2383,
        [Description("W系列系统的第84个子网站")]
        SysW_84 = 2384,
        [Description("W系列系统的第85个子网站")]
        SysW_85 = 2385,
        [Description("W系列系统的第86个子网站")]
        SysW_86 = 2386,
        [Description("W系列系统的第87个子网站")]
        SysW_87 = 2387,
        [Description("W系列系统的第88个子网站")]
        SysW_88 = 2388,
        [Description("W系列系统的第89个子网站")]
        SysW_89 = 2389,
        [Description("W系列系统的第90个子网站")]
        SysW_90 = 2390,
        [Description("W系列系统的第91个子网站")]
        SysW_91 = 2391,
        [Description("W系列系统的第92个子网站")]
        SysW_92 = 2392,
        [Description("W系列系统的第93个子网站")]
        SysW_93 = 2393,
        [Description("W系列系统的第94个子网站")]
        SysW_94 = 2394,
        [Description("W系列系统的第95个子网站")]
        SysW_95 = 2395,
        [Description("W系列系统的第96个子网站")]
        SysW_96 = 2396,
        [Description("W系列系统的第97个子网站")]
        SysW_97 = 2397,
        [Description("W系列系统的第98个子网站")]
        SysW_98 = 2398,
        [Description("W系列系统的第99个子网站")]
        SysW_99 = 2399,
        [Description("X系列系统的第1个子网站")]
        SysX_01 = 2401,
        [Description("X系列系统的第2个子网站")]
        SysX_02 = 2402,
        [Description("X系列系统的第3个子网站")]
        SysX_03 = 2403,
        [Description("X系列系统的第4个子网站")]
        SysX_04 = 2404,
        [Description("X系列系统的第5个子网站")]
        SysX_05 = 2405,
        [Description("X系列系统的第6个子网站")]
        SysX_06 = 2406,
        [Description("X系列系统的第7个子网站")]
        SysX_07 = 2407,
        [Description("X系列系统的第8个子网站")]
        SysX_08 = 2408,
        [Description("X系列系统的第9个子网站")]
        SysX_09 = 2409,
        [Description("X系列系统的第10个子网站")]
        SysX_10 = 2410,
        [Description("X系列系统的第11个子网站")]
        SysX_11 = 2411,
        [Description("X系列系统的第12个子网站")]
        SysX_12 = 2412,
        [Description("X系列系统的第13个子网站")]
        SysX_13 = 2413,
        [Description("X系列系统的第14个子网站")]
        SysX_14 = 2414,
        [Description("X系列系统的第15个子网站")]
        SysX_15 = 2415,
        [Description("X系列系统的第16个子网站")]
        SysX_16 = 2416,
        [Description("X系列系统的第17个子网站")]
        SysX_17 = 2417,
        [Description("X系列系统的第18个子网站")]
        SysX_18 = 2418,
        [Description("X系列系统的第19个子网站")]
        SysX_19 = 2419,
        [Description("X系列系统的第20个子网站")]
        SysX_20 = 2420,
        [Description("X系列系统的第21个子网站")]
        SysX_21 = 2421,
        [Description("X系列系统的第22个子网站")]
        SysX_22 = 2422,
        [Description("X系列系统的第23个子网站")]
        SysX_23 = 2423,
        [Description("X系列系统的第24个子网站")]
        SysX_24 = 2424,
        [Description("X系列系统的第25个子网站")]
        SysX_25 = 2425,
        [Description("X系列系统的第26个子网站")]
        SysX_26 = 2426,
        [Description("X系列系统的第27个子网站")]
        SysX_27 = 2427,
        [Description("X系列系统的第28个子网站")]
        SysX_28 = 2428,
        [Description("X系列系统的第29个子网站")]
        SysX_29 = 2429,
        [Description("X系列系统的第30个子网站")]
        SysX_30 = 2430,
        [Description("X系列系统的第31个子网站")]
        SysX_31 = 2431,
        [Description("X系列系统的第32个子网站")]
        SysX_32 = 2432,
        [Description("X系列系统的第33个子网站")]
        SysX_33 = 2433,
        [Description("X系列系统的第34个子网站")]
        SysX_34 = 2434,
        [Description("X系列系统的第35个子网站")]
        SysX_35 = 2435,
        [Description("X系列系统的第36个子网站")]
        SysX_36 = 2436,
        [Description("X系列系统的第37个子网站")]
        SysX_37 = 2437,
        [Description("X系列系统的第38个子网站")]
        SysX_38 = 2438,
        [Description("X系列系统的第39个子网站")]
        SysX_39 = 2439,
        [Description("X系列系统的第40个子网站")]
        SysX_40 = 2440,
        [Description("X系列系统的第41个子网站")]
        SysX_41 = 2441,
        [Description("X系列系统的第42个子网站")]
        SysX_42 = 2442,
        [Description("X系列系统的第43个子网站")]
        SysX_43 = 2443,
        [Description("X系列系统的第44个子网站")]
        SysX_44 = 2444,
        [Description("X系列系统的第45个子网站")]
        SysX_45 = 2445,
        [Description("X系列系统的第46个子网站")]
        SysX_46 = 2446,
        [Description("X系列系统的第47个子网站")]
        SysX_47 = 2447,
        [Description("X系列系统的第48个子网站")]
        SysX_48 = 2448,
        [Description("X系列系统的第49个子网站")]
        SysX_49 = 2449,
        [Description("X系列系统的第50个子网站")]
        SysX_50 = 2450,
        [Description("X系列系统的第51个子网站")]
        SysX_51 = 2451,
        [Description("X系列系统的第52个子网站")]
        SysX_52 = 2452,
        [Description("X系列系统的第53个子网站")]
        SysX_53 = 2453,
        [Description("X系列系统的第54个子网站")]
        SysX_54 = 2454,
        [Description("X系列系统的第55个子网站")]
        SysX_55 = 2455,
        [Description("X系列系统的第56个子网站")]
        SysX_56 = 2456,
        [Description("X系列系统的第57个子网站")]
        SysX_57 = 2457,
        [Description("X系列系统的第58个子网站")]
        SysX_58 = 2458,
        [Description("X系列系统的第59个子网站")]
        SysX_59 = 2459,
        [Description("X系列系统的第60个子网站")]
        SysX_60 = 2460,
        [Description("X系列系统的第61个子网站")]
        SysX_61 = 2461,
        [Description("X系列系统的第62个子网站")]
        SysX_62 = 2462,
        [Description("X系列系统的第63个子网站")]
        SysX_63 = 2463,
        [Description("X系列系统的第64个子网站")]
        SysX_64 = 2464,
        [Description("X系列系统的第65个子网站")]
        SysX_65 = 2465,
        [Description("X系列系统的第66个子网站")]
        SysX_66 = 2466,
        [Description("X系列系统的第67个子网站")]
        SysX_67 = 2467,
        [Description("X系列系统的第68个子网站")]
        SysX_68 = 2468,
        [Description("X系列系统的第69个子网站")]
        SysX_69 = 2469,
        [Description("X系列系统的第70个子网站")]
        SysX_70 = 2470,
        [Description("X系列系统的第71个子网站")]
        SysX_71 = 2471,
        [Description("X系列系统的第72个子网站")]
        SysX_72 = 2472,
        [Description("X系列系统的第73个子网站")]
        SysX_73 = 2473,
        [Description("X系列系统的第74个子网站")]
        SysX_74 = 2474,
        [Description("X系列系统的第75个子网站")]
        SysX_75 = 2475,
        [Description("X系列系统的第76个子网站")]
        SysX_76 = 2476,
        [Description("X系列系统的第77个子网站")]
        SysX_77 = 2477,
        [Description("X系列系统的第78个子网站")]
        SysX_78 = 2478,
        [Description("X系列系统的第79个子网站")]
        SysX_79 = 2479,
        [Description("X系列系统的第80个子网站")]
        SysX_80 = 2480,
        [Description("X系列系统的第81个子网站")]
        SysX_81 = 2481,
        [Description("X系列系统的第82个子网站")]
        SysX_82 = 2482,
        [Description("X系列系统的第83个子网站")]
        SysX_83 = 2483,
        [Description("X系列系统的第84个子网站")]
        SysX_84 = 2484,
        [Description("X系列系统的第85个子网站")]
        SysX_85 = 2485,
        [Description("X系列系统的第86个子网站")]
        SysX_86 = 2486,
        [Description("X系列系统的第87个子网站")]
        SysX_87 = 2487,
        [Description("X系列系统的第88个子网站")]
        SysX_88 = 2488,
        [Description("X系列系统的第89个子网站")]
        SysX_89 = 2489,
        [Description("X系列系统的第90个子网站")]
        SysX_90 = 2490,
        [Description("X系列系统的第91个子网站")]
        SysX_91 = 2491,
        [Description("X系列系统的第92个子网站")]
        SysX_92 = 2492,
        [Description("X系列系统的第93个子网站")]
        SysX_93 = 2493,
        [Description("X系列系统的第94个子网站")]
        SysX_94 = 2494,
        [Description("X系列系统的第95个子网站")]
        SysX_95 = 2495,
        [Description("X系列系统的第96个子网站")]
        SysX_96 = 2496,
        [Description("X系列系统的第97个子网站")]
        SysX_97 = 2497,
        [Description("X系列系统的第98个子网站")]
        SysX_98 = 2498,
        [Description("X系列系统的第99个子网站")]
        SysX_99 = 2499,
        [Description("Y系列系统的第1个子网站")]
        SysY_01 = 2501,
        [Description("Y系列系统的第2个子网站")]
        SysY_02 = 2502,
        [Description("Y系列系统的第3个子网站")]
        SysY_03 = 2503,
        [Description("Y系列系统的第4个子网站")]
        SysY_04 = 2504,
        [Description("Y系列系统的第5个子网站")]
        SysY_05 = 2505,
        [Description("Y系列系统的第6个子网站")]
        SysY_06 = 2506,
        [Description("Y系列系统的第7个子网站")]
        SysY_07 = 2507,
        [Description("Y系列系统的第8个子网站")]
        SysY_08 = 2508,
        [Description("Y系列系统的第9个子网站")]
        SysY_09 = 2509,
        [Description("Y系列系统的第10个子网站")]
        SysY_10 = 2510,
        [Description("Y系列系统的第11个子网站")]
        SysY_11 = 2511,
        [Description("Y系列系统的第12个子网站")]
        SysY_12 = 2512,
        [Description("Y系列系统的第13个子网站")]
        SysY_13 = 2513,
        [Description("Y系列系统的第14个子网站")]
        SysY_14 = 2514,
        [Description("Y系列系统的第15个子网站")]
        SysY_15 = 2515,
        [Description("Y系列系统的第16个子网站")]
        SysY_16 = 2516,
        [Description("Y系列系统的第17个子网站")]
        SysY_17 = 2517,
        [Description("Y系列系统的第18个子网站")]
        SysY_18 = 2518,
        [Description("Y系列系统的第19个子网站")]
        SysY_19 = 2519,
        [Description("Y系列系统的第20个子网站")]
        SysY_20 = 2520,
        [Description("Y系列系统的第21个子网站")]
        SysY_21 = 2521,
        [Description("Y系列系统的第22个子网站")]
        SysY_22 = 2522,
        [Description("Y系列系统的第23个子网站")]
        SysY_23 = 2523,
        [Description("Y系列系统的第24个子网站")]
        SysY_24 = 2524,
        [Description("Y系列系统的第25个子网站")]
        SysY_25 = 2525,
        [Description("Y系列系统的第26个子网站")]
        SysY_26 = 2526,
        [Description("Y系列系统的第27个子网站")]
        SysY_27 = 2527,
        [Description("Y系列系统的第28个子网站")]
        SysY_28 = 2528,
        [Description("Y系列系统的第29个子网站")]
        SysY_29 = 2529,
        [Description("Y系列系统的第30个子网站")]
        SysY_30 = 2530,
        [Description("Y系列系统的第31个子网站")]
        SysY_31 = 2531,
        [Description("Y系列系统的第32个子网站")]
        SysY_32 = 2532,
        [Description("Y系列系统的第33个子网站")]
        SysY_33 = 2533,
        [Description("Y系列系统的第34个子网站")]
        SysY_34 = 2534,
        [Description("Y系列系统的第35个子网站")]
        SysY_35 = 2535,
        [Description("Y系列系统的第36个子网站")]
        SysY_36 = 2536,
        [Description("Y系列系统的第37个子网站")]
        SysY_37 = 2537,
        [Description("Y系列系统的第38个子网站")]
        SysY_38 = 2538,
        [Description("Y系列系统的第39个子网站")]
        SysY_39 = 2539,
        [Description("Y系列系统的第40个子网站")]
        SysY_40 = 2540,
        [Description("Y系列系统的第41个子网站")]
        SysY_41 = 2541,
        [Description("Y系列系统的第42个子网站")]
        SysY_42 = 2542,
        [Description("Y系列系统的第43个子网站")]
        SysY_43 = 2543,
        [Description("Y系列系统的第44个子网站")]
        SysY_44 = 2544,
        [Description("Y系列系统的第45个子网站")]
        SysY_45 = 2545,
        [Description("Y系列系统的第46个子网站")]
        SysY_46 = 2546,
        [Description("Y系列系统的第47个子网站")]
        SysY_47 = 2547,
        [Description("Y系列系统的第48个子网站")]
        SysY_48 = 2548,
        [Description("Y系列系统的第49个子网站")]
        SysY_49 = 2549,
        [Description("Y系列系统的第50个子网站")]
        SysY_50 = 2550,
        [Description("Y系列系统的第51个子网站")]
        SysY_51 = 2551,
        [Description("Y系列系统的第52个子网站")]
        SysY_52 = 2552,
        [Description("Y系列系统的第53个子网站")]
        SysY_53 = 2553,
        [Description("Y系列系统的第54个子网站")]
        SysY_54 = 2554,
        [Description("Y系列系统的第55个子网站")]
        SysY_55 = 2555,
        [Description("Y系列系统的第56个子网站")]
        SysY_56 = 2556,
        [Description("Y系列系统的第57个子网站")]
        SysY_57 = 2557,
        [Description("Y系列系统的第58个子网站")]
        SysY_58 = 2558,
        [Description("Y系列系统的第59个子网站")]
        SysY_59 = 2559,
        [Description("Y系列系统的第60个子网站")]
        SysY_60 = 2560,
        [Description("Y系列系统的第61个子网站")]
        SysY_61 = 2561,
        [Description("Y系列系统的第62个子网站")]
        SysY_62 = 2562,
        [Description("Y系列系统的第63个子网站")]
        SysY_63 = 2563,
        [Description("Y系列系统的第64个子网站")]
        SysY_64 = 2564,
        [Description("Y系列系统的第65个子网站")]
        SysY_65 = 2565,
        [Description("Y系列系统的第66个子网站")]
        SysY_66 = 2566,
        [Description("Y系列系统的第67个子网站")]
        SysY_67 = 2567,
        [Description("Y系列系统的第68个子网站")]
        SysY_68 = 2568,
        [Description("Y系列系统的第69个子网站")]
        SysY_69 = 2569,
        [Description("Y系列系统的第70个子网站")]
        SysY_70 = 2570,
        [Description("Y系列系统的第71个子网站")]
        SysY_71 = 2571,
        [Description("Y系列系统的第72个子网站")]
        SysY_72 = 2572,
        [Description("Y系列系统的第73个子网站")]
        SysY_73 = 2573,
        [Description("Y系列系统的第74个子网站")]
        SysY_74 = 2574,
        [Description("Y系列系统的第75个子网站")]
        SysY_75 = 2575,
        [Description("Y系列系统的第76个子网站")]
        SysY_76 = 2576,
        [Description("Y系列系统的第77个子网站")]
        SysY_77 = 2577,
        [Description("Y系列系统的第78个子网站")]
        SysY_78 = 2578,
        [Description("Y系列系统的第79个子网站")]
        SysY_79 = 2579,
        [Description("Y系列系统的第80个子网站")]
        SysY_80 = 2580,
        [Description("Y系列系统的第81个子网站")]
        SysY_81 = 2581,
        [Description("Y系列系统的第82个子网站")]
        SysY_82 = 2582,
        [Description("Y系列系统的第83个子网站")]
        SysY_83 = 2583,
        [Description("Y系列系统的第84个子网站")]
        SysY_84 = 2584,
        [Description("Y系列系统的第85个子网站")]
        SysY_85 = 2585,
        [Description("Y系列系统的第86个子网站")]
        SysY_86 = 2586,
        [Description("Y系列系统的第87个子网站")]
        SysY_87 = 2587,
        [Description("Y系列系统的第88个子网站")]
        SysY_88 = 2588,
        [Description("Y系列系统的第89个子网站")]
        SysY_89 = 2589,
        [Description("Y系列系统的第90个子网站")]
        SysY_90 = 2590,
        [Description("Y系列系统的第91个子网站")]
        SysY_91 = 2591,
        [Description("Y系列系统的第92个子网站")]
        SysY_92 = 2592,
        [Description("Y系列系统的第93个子网站")]
        SysY_93 = 2593,
        [Description("Y系列系统的第94个子网站")]
        SysY_94 = 2594,
        [Description("Y系列系统的第95个子网站")]
        SysY_95 = 2595,
        [Description("Y系列系统的第96个子网站")]
        SysY_96 = 2596,
        [Description("Y系列系统的第97个子网站")]
        SysY_97 = 2597,
        [Description("Y系列系统的第98个子网站")]
        SysY_98 = 2598,
        [Description("Y系列系统的第99个子网站")]
        SysY_99 = 2599,
        [Description("Z系列系统的第1个子网站")]
        SysZ_01 = 2601,
        [Description("Z系列系统的第2个子网站")]
        SysZ_02 = 2602,
        [Description("Z系列系统的第3个子网站")]
        SysZ_03 = 2603,
        [Description("Z系列系统的第4个子网站")]
        SysZ_04 = 2604,
        [Description("Z系列系统的第5个子网站")]
        SysZ_05 = 2605,
        [Description("Z系列系统的第6个子网站")]
        SysZ_06 = 2606,
        [Description("Z系列系统的第7个子网站")]
        SysZ_07 = 2607,
        [Description("Z系列系统的第8个子网站")]
        SysZ_08 = 2608,
        [Description("Z系列系统的第9个子网站")]
        SysZ_09 = 2609,
        [Description("Z系列系统的第10个子网站")]
        SysZ_10 = 2610,
        [Description("Z系列系统的第11个子网站")]
        SysZ_11 = 2611,
        [Description("Z系列系统的第12个子网站")]
        SysZ_12 = 2612,
        [Description("Z系列系统的第13个子网站")]
        SysZ_13 = 2613,
        [Description("Z系列系统的第14个子网站")]
        SysZ_14 = 2614,
        [Description("Z系列系统的第15个子网站")]
        SysZ_15 = 2615,
        [Description("Z系列系统的第16个子网站")]
        SysZ_16 = 2616,
        [Description("Z系列系统的第17个子网站")]
        SysZ_17 = 2617,
        [Description("Z系列系统的第18个子网站")]
        SysZ_18 = 2618,
        [Description("Z系列系统的第19个子网站")]
        SysZ_19 = 2619,
        [Description("Z系列系统的第20个子网站")]
        SysZ_20 = 2620,
        [Description("Z系列系统的第21个子网站")]
        SysZ_21 = 2621,
        [Description("Z系列系统的第22个子网站")]
        SysZ_22 = 2622,
        [Description("Z系列系统的第23个子网站")]
        SysZ_23 = 2623,
        [Description("Z系列系统的第24个子网站")]
        SysZ_24 = 2624,
        [Description("Z系列系统的第25个子网站")]
        SysZ_25 = 2625,
        [Description("Z系列系统的第26个子网站")]
        SysZ_26 = 2626,
        [Description("Z系列系统的第27个子网站")]
        SysZ_27 = 2627,
        [Description("Z系列系统的第28个子网站")]
        SysZ_28 = 2628,
        [Description("Z系列系统的第29个子网站")]
        SysZ_29 = 2629,
        [Description("Z系列系统的第30个子网站")]
        SysZ_30 = 2630,
        [Description("Z系列系统的第31个子网站")]
        SysZ_31 = 2631,
        [Description("Z系列系统的第32个子网站")]
        SysZ_32 = 2632,
        [Description("Z系列系统的第33个子网站")]
        SysZ_33 = 2633,
        [Description("Z系列系统的第34个子网站")]
        SysZ_34 = 2634,
        [Description("Z系列系统的第35个子网站")]
        SysZ_35 = 2635,
        [Description("Z系列系统的第36个子网站")]
        SysZ_36 = 2636,
        [Description("Z系列系统的第37个子网站")]
        SysZ_37 = 2637,
        [Description("Z系列系统的第38个子网站")]
        SysZ_38 = 2638,
        [Description("Z系列系统的第39个子网站")]
        SysZ_39 = 2639,
        [Description("Z系列系统的第40个子网站")]
        SysZ_40 = 2640,
        [Description("Z系列系统的第41个子网站")]
        SysZ_41 = 2641,
        [Description("Z系列系统的第42个子网站")]
        SysZ_42 = 2642,
        [Description("Z系列系统的第43个子网站")]
        SysZ_43 = 2643,
        [Description("Z系列系统的第44个子网站")]
        SysZ_44 = 2644,
        [Description("Z系列系统的第45个子网站")]
        SysZ_45 = 2645,
        [Description("Z系列系统的第46个子网站")]
        SysZ_46 = 2646,
        [Description("Z系列系统的第47个子网站")]
        SysZ_47 = 2647,
        [Description("Z系列系统的第48个子网站")]
        SysZ_48 = 2648,
        [Description("Z系列系统的第49个子网站")]
        SysZ_49 = 2649,
        [Description("Z系列系统的第50个子网站")]
        SysZ_50 = 2650,
        [Description("Z系列系统的第51个子网站")]
        SysZ_51 = 2651,
        [Description("Z系列系统的第52个子网站")]
        SysZ_52 = 2652,
        [Description("Z系列系统的第53个子网站")]
        SysZ_53 = 2653,
        [Description("Z系列系统的第54个子网站")]
        SysZ_54 = 2654,
        [Description("Z系列系统的第55个子网站")]
        SysZ_55 = 2655,
        [Description("Z系列系统的第56个子网站")]
        SysZ_56 = 2656,
        [Description("Z系列系统的第57个子网站")]
        SysZ_57 = 2657,
        [Description("Z系列系统的第58个子网站")]
        SysZ_58 = 2658,
        [Description("Z系列系统的第59个子网站")]
        SysZ_59 = 2659,
        [Description("Z系列系统的第60个子网站")]
        SysZ_60 = 2660,
        [Description("Z系列系统的第61个子网站")]
        SysZ_61 = 2661,
        [Description("Z系列系统的第62个子网站")]
        SysZ_62 = 2662,
        [Description("Z系列系统的第63个子网站")]
        SysZ_63 = 2663,
        [Description("Z系列系统的第64个子网站")]
        SysZ_64 = 2664,
        [Description("Z系列系统的第65个子网站")]
        SysZ_65 = 2665,
        [Description("Z系列系统的第66个子网站")]
        SysZ_66 = 2666,
        [Description("Z系列系统的第67个子网站")]
        SysZ_67 = 2667,
        [Description("Z系列系统的第68个子网站")]
        SysZ_68 = 2668,
        [Description("Z系列系统的第69个子网站")]
        SysZ_69 = 2669,
        [Description("Z系列系统的第70个子网站")]
        SysZ_70 = 2670,
        [Description("Z系列系统的第71个子网站")]
        SysZ_71 = 2671,
        [Description("Z系列系统的第72个子网站")]
        SysZ_72 = 2672,
        [Description("Z系列系统的第73个子网站")]
        SysZ_73 = 2673,
        [Description("Z系列系统的第74个子网站")]
        SysZ_74 = 2674,
        [Description("Z系列系统的第75个子网站")]
        SysZ_75 = 2675,
        [Description("Z系列系统的第76个子网站")]
        SysZ_76 = 2676,
        [Description("Z系列系统的第77个子网站")]
        SysZ_77 = 2677,
        [Description("Z系列系统的第78个子网站")]
        SysZ_78 = 2678,
        [Description("Z系列系统的第79个子网站")]
        SysZ_79 = 2679,
        [Description("Z系列系统的第80个子网站")]
        SysZ_80 = 2680,
        [Description("Z系列系统的第81个子网站")]
        SysZ_81 = 2681,
        [Description("Z系列系统的第82个子网站")]
        SysZ_82 = 2682,
        [Description("Z系列系统的第83个子网站")]
        SysZ_83 = 2683,
        [Description("Z系列系统的第84个子网站")]
        SysZ_84 = 2684,
        [Description("Z系列系统的第85个子网站")]
        SysZ_85 = 2685,
        [Description("Z系列系统的第86个子网站")]
        SysZ_86 = 2686,
        [Description("Z系列系统的第87个子网站")]
        SysZ_87 = 2687,
        [Description("Z系列系统的第88个子网站")]
        SysZ_88 = 2688,
        [Description("Z系列系统的第89个子网站")]
        SysZ_89 = 2689,
        [Description("Z系列系统的第90个子网站")]
        SysZ_90 = 2690,
        [Description("Z系列系统的第91个子网站")]
        SysZ_91 = 2691,
        [Description("Z系列系统的第92个子网站")]
        SysZ_92 = 2692,
        [Description("Z系列系统的第93个子网站")]
        SysZ_93 = 2693,
        [Description("Z系列系统的第94个子网站")]
        SysZ_94 = 2694,
        [Description("Z系列系统的第95个子网站")]
        SysZ_95 = 2695,
        [Description("Z系列系统的第96个子网站")]
        SysZ_96 = 2696,
        [Description("Z系列系统的第97个子网站")]
        SysZ_97 = 2697,
        [Description("Z系列系统的第98个子网站")]
        SysZ_98 = 2698,
        [Description("Z系列系统的第99个子网站")]
        SysZ_99 = 2699,
    }
    #endregion 系统类别枚举:SysCategory



    /// <summary>
    /// 日志类型类型，枚举
    /// </summary>
    public enum LogType
    {
        所有 = -1,
        //用户行为/数据库操作
        登录 = 0,
        添加 = 1,
        硬删除 = 2,
        软删除 = 3,
        清空 = 4,
        修改 = 5,
        事务 = 6,
        查询 = 9,

        //业务操作
        业务变更 = 11,
        业务记录 = 12,
        审核 = 13,
        反审核 = 14,
        导入 = 15,
        批量插入 = 16,
        提交 = 17,
        审批 = 18,
        驳回 = 19,

        //调试和异常
        Debug = 21,
        异常 = 22,

        启动 = 31,
        停止 = 32,
        初次访问 = 33,




    }


}
