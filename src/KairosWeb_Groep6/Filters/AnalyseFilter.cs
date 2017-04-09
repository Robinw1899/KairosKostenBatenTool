using System;
using KairosWeb_Groep6.Models.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;

namespace KairosWeb_Groep6.Filters
{
    public class AnalyseFilter : ActionFilterAttribute
    {
        private readonly IAnalyseRepository _analyseRepository;
        private Analyse _analyse;

        public AnalyseFilter(IAnalyseRepository repo)
        {
            _analyseRepository = repo;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            _analyse = ReadAnalyseFromSession(context.HttpContext);

            if(_analyse.AnalyseId != 0)
            {// geen nieuwe analyse
                _analyse = _analyseRepository.GetById(_analyse.AnalyseId);
            }
           
            context.ActionArguments["analyseId"] = _analyse.AnalyseId;
            base.OnActionExecuting(context);
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            WriteAnalyseToSession(context.HttpContext, _analyse.AnalyseId);
            base.OnActionExecuted(context);
        }

        private int ReadAnalyseFromSession(HttpContext context)
        {
            Analyse analyse = context.Session.GetString("analyse") == null
                ? new Analyse()
                : JsonConvert.DeserializeObject<Analyse>(context.Session.GetString("analyseId"));
            
            /*if(analyse == null)
                return -1;
            return analyse.AnalyseId;*/
            return (analyse == null ? -1 : analyse.AnalyseId);
        }

        private void WriteAnalyseToSession(HttpContext context, Analyse analyse)//niet zeker of hier een int moet meegegeven worden/
        {
            context.Session.SetString("analyseId", JsonConvert.SerializeObject(analyse.AnalyseId));
        }

        public static void ClearSession(HttpContext context)
        {
            context.Session.Remove("analyseId");
        }

        public static void SetAnalyseInSession(HttpContext context, Analyse analyse)
        {
            context.Session.SetString("analyseId", JsonConvert.SerializeObject(analyse.AnalyseId));
        }
    }
}
