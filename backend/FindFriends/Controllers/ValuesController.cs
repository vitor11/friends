using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FindFriends.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        [HttpGet]
        public double CalculateDistance()
        {
            double r = 6371.0;

            double p1LA = -23.173312;
            double p1LO = -45.890535;
            double p2LA = -23.177519;
            double p2LO = -45.879516;

            p1LA = (p1LA * System.Math.PI) / 180.0;
            p1LO = (p1LO * System.Math.PI) / 180.0;
            p2LA = (p2LA * System.Math.PI) / 180.0;
            p2LO = (p2LO * System.Math.PI) / 180.0;

            double dLat = (p2LA - p1LA);
            double dLong = (p2LO - p1LO);

            double a = (System.Math.Sin(dLat / 2) * System.Math.Sin(dLat / 2)) + (System.Math.Cos(p1LA) * System.Math.Cos(p2LA) * System.Math.Sin(dLong / 2) * System.Math.Sin(dLong / 2));
            double c = 2 * (System.Math.Atan2(System.Math.Sqrt(a), System.Math.Sqrt(1 - a)));

            double distanciaMT = (r * c * 1000);

            return distanciaMT;
        }
         

        // GET api/values
        [HttpGet("teste")]
        public IEnumerable<string> Teste()
        {
            return new string[] { "value1", "value2" };
        }

    }
}
