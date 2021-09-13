using System;

namespace InterFace
{

    public interface IG_OV
    {
        public delegate void SomeCalculateWay(IReturnParameters RP);

        //将传入参数在系统底层进行某种处理，具体计算方法由开发者开发，函数仅提供执行计算方法后的返回值
        //public int PrintAndCalculate(int num1, int num2, SomeCalculateWay cal)
        //{
        //    Console.WriteLine("系统底层处理：" + num1);
        //    Console.WriteLine("系统底层处理：" + num2);
        //    return cal(num1, num2);//调用传入函数的一个引用   
        //}

        //返回
        public class IReturnParameters
        {
            /// <summary>
            /// 拓展
            /// </summary>
            public object more { get; set; }
            /// <summary>
            /// 方法名称
            /// </summary>
            public String declaringType { get; set; }
            /// <summary>
            /// 类名称
            /// </summary>
            public String currentMethod { get; set; }
            /// <summary>
            /// 内容
            /// </summary>
            public String content { get; set; }
            /// <summary>
            /// 成功还是失败
            /// </summary>
            public Boolean substate { get; set; }
        }
        void Init(SomeCalculateWay Sw);
        
    }


    public interface IExpand
    {
        void Init(object[] obj);
    }

}
