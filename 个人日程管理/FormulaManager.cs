using JavaScriptEngineSwitcher.Core;
using JavaScriptEngineSwitcher.V8;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 个人日程管理
{
    class FormulaManager
    {
        public string code
        {
            get
            {
                return _code;
            }

            set
            {
                if(value != null)
                {
                    _code = value;
                }
            }
        }

        public string _code;
        public static readonly IPrecompiledScript startCode;

        static FormulaManager()
        {
            var engineSwitcher = JsEngineSwitcher.Current;
            engineSwitcher.EngineFactories.Add(new V8JsEngineFactory());
            engineSwitcher.DefaultEngineName = V8JsEngine.EngineName;

            using(var engine = JsEngineSwitcher.Current.CreateDefaultEngine())
            {
                startCode = engine.Precompile(@"function returnDate(date)
                                                {
                                                    return date.getFullYear() + '-' + (date.getMonth() + 1) + '-' + date.getDate();
                                                }
                                     
                                                function getDate(arg)
                                                {
                                                    var args = arg.split('-');
                                                    return new Date(parseInt(args[0],10),parseInt(args[1],10),parseInt(args[2],10));
                                                }
                                               ");
            }
        }

        public FormulaManager()
        {
        
        }

        public FormulaManager(string code)
        {
            this.code = code;
        }

        private DateTime ConvertJSObjectToDate(string date)
        {
            var args = date.Split('-');

            if(args.Length != 3)
            {
                throw new ArgumentException("数组长度必须为3！");
            }

            return new DateTime(Convert.ToInt32(args[0]),Convert.ToInt32(args[1]),Convert.ToInt32(args[2]),0,0,0);
        }

        private string ConvertDateToJSObject(DateTime date)
        {
            return date.Year + "-" + (date.Month - 1) + "-" + date.Day;
        }
        
        private T CallJSFunction<T>(string functionName,params object[] args)
        {
            try
            {
                using(var engine = JsEngineSwitcher.Current.CreateDefaultEngine())
                {
                    
                    engine.Execute(startCode);
                    engine.Execute(code);
                    return engine.CallFunction<T>(functionName,args);
                }
            }
            catch(JsCompilationException e)
            {
                Global.Error("行" + e.LineNumber + "列" + e.ColumnNumber + "编译错误：" + e.Description);
                throw e;
            }
            catch(JsRuntimeException e)
            {
                Global.Error("行" + e.LineNumber + "列" + e.ColumnNumber + "运行时错误：" + e.Description);
                throw e;
            }
            catch(Exception e)
            {
                Global.Error(e.Message);
                throw e;
            }
        }

        public bool CheckDate(DateTime date)
        {
            return CallJSFunction<bool>("checkDate",ConvertDateToJSObject(date));
        }

        public DateTime GetStartDate()
        {
            return ConvertJSObjectToDate(CallJSFunction<string>("getStartDate"));
        }

        public DateTime GetEndDate()
        {
            var date = ConvertJSObjectToDate(CallJSFunction<string>("getEndDate"));
            return new DateTime(date.Year,date.Month,date.Day,23,59,59);
        }
    }
}
