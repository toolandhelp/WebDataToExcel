using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebDataToExcel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebDataToExcel.Tests
{
    [TestClass()]
    public class Form1Tests
    {
        [TestMethod()]
        public void Form1Test()
        {
            int to = 1500, pa = 15;
            int qy = to % pa;
            int pc = to / pa;

            Assert.Fail();
        }
    }
}