using atos.skillsToCompetenciesMapper.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace atos.skillsToCompetenciesMapperTests
{
    [TestClass]
    public class ExcelHelperTests
    {
        [TestMethod]
        public void ColumnIncrimentor()
        {
            var eh = new ExcelHelper();

            
            Assert.AreEqual("AA", eh.IncrimentColumn("Z"));
            Assert.AreEqual("L", eh.IncrimentColumn("K"));
            Assert.AreEqual("AC", eh.IncrimentColumn("AB"));
            Assert.AreEqual("BA", eh.IncrimentColumn("AZ"));
            Assert.AreEqual("BCA", eh.IncrimentColumn("BBZ"));
            Assert.AreEqual("FGADA", eh.IncrimentColumn("FGACZ"));
            Assert.AreEqual("BC", eh.IncrimentColumn("BB"));
            Assert.AreEqual("AR", eh.IncrimentColumn("AQ"));
        }

    }
   
}
