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
            Analyse analyse = new Analyse();

            if(_analyse.AnalyseId != 0)
            {// geen nieuwe analyse
                analyse = _analyseRepository.GetById(_analyse.AnalyseId);
            }
           
            context.ActionArguments["analyse"] = analyse;
            base.OnActionExecuting(context);
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            WriteAnalyseToSession(context.HttpContext, _analyse);
            base.OnActionExecuted(context);
        }

        private Analyse ReadAnalyseFromSession(HttpContext context)
        {
            Analyse analyse = context.Session.GetString("analyse") == null
                ? new Analyse()
                : JsonConvert.DeserializeObject<Analyse>(context.Session.GetString("analyse"));

            return analyse;
        }

        private void WriteAnalyseToSession(HttpContext context, Analyse analyse)//niet zeker of hier een int moet meegegeven worden/
        {
            context.Session.SetString("analyse", JsonConvert.SerializeObject(analyse, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Serialize
            }));
        }

        public static void ClearSession(HttpContext context)
        {
            context.Session.Remove("analyse");
        }

        public static void SetAnalyseInSession(HttpContext context, Analyse analyse)
        {
            context.Session.SetString("analyse", JsonConvert.SerializeObject(analyse, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Serialize
            }));
        }
    }
}
