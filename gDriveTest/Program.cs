using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Google.GData.Documents;
using Google.GData.Client;
using System.Diagnostics;
using System.Configuration;

namespace gDriveTest
{
    class Program
    {
        static void Main(string[] args)
        {
            gDriveTest test = new gDriveTest();
            test.handleAuthAndPrintDocuments();
        }
            
    }
}
