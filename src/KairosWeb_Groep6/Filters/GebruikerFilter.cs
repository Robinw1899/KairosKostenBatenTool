using KairosWeb_Groep6.Data.Repositories;
using KairosWeb_Groep6.Models.Domain;
using Microsoft.AspNetCore.Mvc.Filters;

namespace KairosWeb_Groep6.Filters
{
    public class GebruikerFilter : ActionFilterAttribute
    {
        private readonly IGebruikerRepository _gebruikerRepository;

        public GebruikerFilter(IGebruikerRepository repo)
        {
            _gebruikerRepository = repo;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            context.ActionArguments["gebruiker"] = context.HttpContext.User.Identity.IsAuthenticated
                ? _gebruikerRepository.GetByEmail(context.HttpContext.User.Identity.Name)
                : null;

            base.OnActionExecuting(context);
        }
    }
}
