using Capstone.DAL;
using Capstone.Models;
using Capstone;
using System;
using System.Collections.Generic;
using System.Text;

namespace capstone
{
    class Program
    {
        static void Main(string[] args)
        {
            // If Main contains more than 2 lines of executable code,
            // you're doing it wrong.
            NationalParkRegistrationCLI cli = new NationalParkRegistrationCLI();
            cli.RunCLI();
        }
    }
}
