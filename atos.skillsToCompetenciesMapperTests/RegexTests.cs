using atos.skillsToCompetenciesMapper.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace atos.skillsToCompetenciesMapperTests
{
    [TestClass]
    public class RegexTests
    {
        [TestMethod]
        public void Test_DASID_LINE()
        {
            var testData = new[] { @"""Employee DAS ID"", ""A604444""" };

            foreach (var data in testData)
                Assert.IsTrue(Regex.IsMatch(data, Constants.DASIDREGEX_FIELD));
        }

        [TestMethod]
        public void Test_NAME_LINE()
        {
            var testData = new[] { @"""Employee Name"",""AMIEL Eric""" };

            foreach (var data in testData)
                Assert.IsTrue(Regex.IsMatch(data, Constants.EMPLOYEENAME_LINE));
        }

        [TestMethod]
        public void Test_GSM_Level()
        {
            var testData = new[] { @"""GCM Skill Level"",""8""", @"""GCM Skill Level"",""18""" };

            foreach (var data in testData)
                Assert.IsTrue(Regex.IsMatch(data, Constants.GSMLEVEL_LINE));

            var actual1 = Regex.Match(testData[0], Constants.Lastvalueindoublequotes).Value.Replace("\"", string.Empty);
            Assert.AreEqual("8", actual1);

            var actual2 = Regex.Match(testData[1], Constants.Lastvalueindoublequotes).Value.Replace("\"", string.Empty);
            Assert.AreEqual("18", actual2);
        }

        [TestMethod]
        public void Test_Supervisor()
        {
            var testdata = new[] { @"""Organizational Unit"",""CO CH_GAILLE Laurent""" };

            foreach (var data in testdata)
                Assert.IsTrue(Regex.IsMatch(data, Constants.SUPOERVISOR_LINE));

            var actual1 = Regex.Match(testdata[0], Constants.Lastvalueindoublequotes).Value;
            Assert.AreEqual("\"CO CH_GAILLE Laurent\"", actual1);
        }

        [TestMethod]
        public void Test_Skill_Group()
        {
            var testdata = new[,] {
                {"\"Activities\",\"Date of last update\",\"Proficiency\",\"Years of Experience\",\"Last Year Used\",\"Primary Skill\",\"Incl.In Profile\",\"Incl. in Ask me about\",\"Incl in. Work Interests\"","\"Activities\"" },
                {"\"Business Processes\",\"Date of last update\",\"Proficiency\",\"Years of Experience\",\"Last Year Used\",\"Primary Skill\",\"Incl. In Profile\",\"Incl. in Ask me about\",\"Incl in. Work Interests\"", "\"Business Processes\"" },
                {"\"Products\",\"Date of last update\",\"Proficiency\",\"Years of Experience\",\"Last Year Used\",\"Primary Skill\",\"Incl. In Profile\",\"Incl. in Ask me about\",\"Incl in. Work Interests\"","\"Products\"" },
                {"\"Methods & Standards\",\"Date of last update\",\"Proficiency\",\"Years of Experience\",\"Last Year Used\",\"Primary Skill\",\"Incl. In Profile\",\"Incl. in Ask me about\",\"Incl in. Work Interests\"","\"Methods & Standards\"" },
                {"\"Industries\",\"Date of last update\",\"Proficiency\",\"Years of Experience\",\"Last Year Used\",\"Primary Skill\",\"Incl. In Profile\",\"Incl. in Ask me about\",\"Incl in. Work Interests\"","\"Industries\"" },
                { "\"Offerings\",\"Date of last update\",\"Proficiency\",\"Years of Experience\",\"Last Year Used\",\"Primary Skill\",\"Incl. In Profile\",\"Incl. in Ask me about\",\"Incl in. Work Interests\"","\"Offerings\""}};

            for (int i = 0; i < testdata.Length/2; i++)
            {
                var data = testdata[i, 0]; 
                Assert.IsTrue(Regex.IsMatch(data, Constants.SKILLGROUP));

                var expected = testdata[i, 1];
                var actual = Regex.Match(testdata[i, 1], Constants.SKILLGROUP).Value;
                Assert.AreEqual(expected, actual);
            }
        }
    }
}
