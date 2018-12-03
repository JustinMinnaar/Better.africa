using System;

namespace Benefits.Provider.Forms
{
    public class Form
    {
        public EFormType FormType { get; set; }
        public string AgentCode { get; set; }
        public DateTime FormSignDate { get; set; }
        public DateTime FormInceptionDate { get; set; }
    }
}