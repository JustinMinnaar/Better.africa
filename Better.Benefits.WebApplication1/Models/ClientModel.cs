using Better.Benefits.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class ClientModel
    {
        public Person Client { get; set; }

        public List<Person> Agents { get; set; }
    }
}