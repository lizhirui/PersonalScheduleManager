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

            /*set
            {
                if(value != null)
                {
                    _code = value;
                }
            }*/
        }

        public string _code;
        public static readonly IPrecompiledScript startCode;
        private IJsEngine engine;

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

                                                function newDate(year,month,day)
                                                {
                                                    return new Date(year,month - 1,day);
                                                }

                                                function getYear(date)
                                                {
                                                    return date.getFullYear();
                                                }

                                                function getMonth(date)
                                                {
                                                    return date.getMonth() + 1;
                                                }

                                                function getDay(date)
                                                {
                                                    return date.getDate();
                                                }

                                                function equalDate(date1,date2)
                                                {
                                                    return date1.getTime() == date2.getTime();
                                                }

                                                function nonEqualDate(date1,date2)
                                                {
                                                    return date1.getTime() != date2.getTime();
                                                }

                                                function lessDate(date1,date2)
                                                {
                                                    return date1.getTime() < date2.getTime();
                                                }

                                                function greaterDate(date1,date2)
                                                {
                                                    return date1.getTime() > date2.getTime();
                                                }

                                                function lessEqualDate(date1,date2)
                                                {
                                                    return date1.getTime() <= date2.getTime();
                                                }

                                                function greaterEqualDate(date1,date2)
                                                {
                                                    return date1.getTime() >= date2.getTime();
                                                }
                                               ");
            }
        }

        public FormulaManager(string code)
        {
            engine = JsEngineSwitcher.Current.CreateDefaultEngine();
            _code = code;

            try
            {
                engine.Execute(startCode);
                engine.Execute(code);
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

        ~FormulaManager()
        {
            engine.Dispose();
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
                return engine.CallFunction<T>(functionName,args);
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
