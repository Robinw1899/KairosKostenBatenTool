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
        private int _analyseId;

        public AnalyseFilter(IAnalyseRepository repo)
        {
            _analyseRepository = repo;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            _analyseId = ReadAnalyseFromSession(context.HttpContext);
            Analyse analyse = new Analyse();

            if(_analyseId != 0)
            {// geen nieuwe analyse
                analyse = _analyseRepository.GetById(_analyseId);
            }
           
            context.ActionArguments["analyse"] = analyse;
            base.OnActionExecuting(context);
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            WriteAnalyseToSession(context.HttpContext, _analyseId);
            base.OnActionExecuted(context);
        }

        private int ReadAnalyseFromSession(HttpContext context)
        {
            int analyseid = context.Session.GetString("analyseid") == null
                ? 0
                : JsonConvert.DeserializeObject<int>(context.Session.GetString("analyseId"));

            return analyseid;
        }

        private void WriteAnalyseToSession(HttpContext context, int analyseId)//niet zeker of hier een int moet meegegeven worden/
        {
            context.Session.SetString("analyseId", JsonConvert.SerializeObject(analyseId));
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
