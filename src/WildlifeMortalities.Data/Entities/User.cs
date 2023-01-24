using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WildlifeMortalities.Data.Entities;

public class User
{
    public int Id { get; set; }
    public string EmailAddress { get; set; } = string.Empty;
}
