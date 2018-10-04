using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace atos.skillsToCompetenciesMapper.Core
{
    static class Constants
    {
        //^"\\"(?i)employee das id\\",\\"A\d*\\""$
        public const string NUMBERREGEX = @"\d+";
        public const string DASIDREGEX_LINE = @"^""(?i)employee das id"",""A\d*""$";
        public const string DASIDREGEX_FIELD = @"A\d+";

        public const string EMPLOYEENAME_LINE = @"^""(E|e)mployee Name"",""([\u00C0-\u017Fa-zA-Z'-]+\s[\u00C0-\u017Fa-zA-Z'-]+)+""$";
        public const string GSMLEVEL_LINE = @"^""(?i)gcm skill level"",""\d+""$";
        public const string SUPOERVISOR_LINE = @"^""(?i)organizational unit"",""(.*)""$";
        
        public const string Lastvalueindoublequotes = @"""[^""]*""$";
        public const string UPPERCASEWORDS = @"\b[\u00C0-\u00DEA-Z]+-?'?\b";
        public const string CAPITALIZEDWORDS = @"\b[\u00C0-\u00DEA-Z][\u00E0-\u00FFa-z]+-?'?\b";

        public const string SKILLGROUP = "^\"(?i)(Offerings|Certificates|Languages|Industries|Methods & Standards|Products|Business Processes|Activities)\"";
        public const string SKILL = @"^""[^\""]*"",""\d{2}\/\d{2}\/\d{4}"",""[^\""]*""";


        public const string CATEGORY_KEY = "category";
        public const string NAME_KEY = "name";
        public const string SKILLS_KEY = "skills";


        public const string SKILLMATRIX_PATH = @"Data\RoleMatrix.json";

        public const string MAP_FILTER = "map files(*.map)|*.map";
    }
}
