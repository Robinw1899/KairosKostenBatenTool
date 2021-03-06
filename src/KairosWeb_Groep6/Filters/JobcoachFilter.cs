﻿using KairosWeb_Groep6.Models.Domain;
using Microsoft.AspNetCore.Mvc.Filters;

namespace KairosWeb_Groep6.Filters
{
    public class JobcoachFilter : ActionFilterAttribute
    {
        private readonly IJobcoachRepository _gebruikerRepository;

        public JobcoachFilter(IJobcoachRepository repo)
        {
            _gebruikerRepository = repo;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            context.ActionArguments["jobcoach"] = context.HttpContext.User.Identity.IsAuthenticated
                ? _gebruikerRepository.GetByEmail(context.HttpContext.User.Identity.Name)
                : null;

            base.OnActionExecuting(context);
        }
    }
}
