using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ModalMVC
{
    public class ModalForm
    {
        public string View { get; set; }
        public int? ID { get; set; }
        public Guid? Guid { get; set; }
        public string CodeName { get; set; }
        Dictionary<string, string> AdditionalParams { get; set; }

        public ModalForm()
        {
            AdditionalParams = new Dictionary<string, string>();
        }
    }
}