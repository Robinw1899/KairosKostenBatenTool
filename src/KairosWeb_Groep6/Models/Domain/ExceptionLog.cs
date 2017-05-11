using System;

namespace KairosWeb_Groep6.Models.Domain
{
    public class ExceptionLog
    {
        #region Properties
        public int ExceptionLogId { get; set; }

        public string Controller { get; set; }

        public string Action { get; set; }

        public string Message { get; set; }

        public string InnerExceptionMessage { get; set; }

        public DateTime DatumEnTijd { get; set; } = DateTime.Now;
        #endregion

        #region Constructors
        public ExceptionLog()
        {
            
        }

        public ExceptionLog(Exception e, string controller, string action)
        {
            Controller = controller;
            Action = action;
            Message = e.Message;
            InnerExceptionMessage = e.InnerException?.Message;
        }
        #endregion
    }
}
