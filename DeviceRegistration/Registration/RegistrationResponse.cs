using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeviceRegistration.Registration
{
    public class RegistrationResponse : IRegistrationResponse
    {
        public bool Succeeded { get; set; }
        public string? Message { get; set; }
        public List<IRegistrationItem>? Items { get; set; }
    }
}
