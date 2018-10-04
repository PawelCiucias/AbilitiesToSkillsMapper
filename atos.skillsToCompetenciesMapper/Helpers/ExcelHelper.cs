using atos.skillsToCompetenciesMapper.Models;
using atos.skillsToCompetenciesMapper.Models.Interfaces;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace atos.skillsToCompetenciesMapper.Helpers
{

    class ExcelHelper : IDisposable
    {
        static Color lightGreen = Color.FromArgb(226, 239, 218);
        static Color green = Color.FromArgb(198, 224, 180);
        static Color darkGreen = Color.FromArgb(169, 208, 142);

        //16247773
        static Color lightBlue = Color.FromArgb(221, 235, 247);
        static Color blue = Color.FromArgb(189, 215, 238);
        static Color darkBlue = Color.FromArgb(155, 194, 230);

        static Color lightYellow = Color.FromArgb(255, 242, 204);
        static Color yellow = Color.FromArgb(255, 230, 153);

        static Color silver = Color.FromArgb(237, 237, 237);
        static Color lightGray = Color.FromArgb(219, 219, 219);
        static Color gray = Color.FromArgb(201, 201, 201);
        static Color darkGray = Color.FromArgb(123, 123, 123);

        static Color lightPeach = Color.FromArgb(252, 228, 214);
        static Color peach = Color.FromArgb(248, 203, 173);

        static Color pineapple = Color.FromArgb(255, 242, 204);

        static Color lightOrange = Color.FromArgb(244, 176, 132);

        static Color red = Color.FromArgb(255, 0, 0);

        static Color[] Colors = new[] { lightGreen, green, darkGreen, lightBlue, blue, darkBlue, lightYellow, yellow,
                                        silver, lightGray, gray, darkGray, lightPeach, peach, pineapple, lightOrange};
        Application app;
        Workbook wb;
        Worksheet ws;

        IDictionary<string, string> roleToColumnMapping = new Dictionary<string, string>();


        string[] quatitative = new[] { "Activities", "Products", "Methods & Standards", "Business Processes", "Offerings" };

        string[] Activities = new[] { "Level 1 - Follow", "Level 2 - Assist", "Level 3 - Apply", "Level 4 - Enable", "Level 5 - Ensure", "Level 6 - Initiate", "Level 7 - Set Strategy", };
        string[] Products = new[] { "Novice", "Junior", "Proficient", "Senior", "Expert" };
        string[] BusinessProcess = new[] { "Novice", "Junior", "Proficient", "Senior", "Expert" };
        string[] MethodsStandards = new[] { "Novice", "Junior", "Proficient", "Senior", "Expert" };
        string[] Offerings = new[] { "Novice", "Junior", "Proficient", "Senior", "Expert" };

        string[] qualitative = new[] { "Languages", "Certificates", "Industries" };

        public ExcelHelper() { }

        public ExcelHelper(IEnumerable<IRole> roles, IList<IEmployee> employees, IDictionary<string, string> abilityToSkillMap)
        {
            app = new Application();
            app.Visible = true;
            app.WindowState = XlWindowState.xlMaximized;

            wb = app.Workbooks.Add(XlWBATemplate.xlWBATWorksheet);
            ws = wb.Worksheets[1];
            ws.Name = "Skill Matrix";
            
            CreateTitle();
            createInstructions();
            
            var index = 0;
            var column = CreateEmployeeHeaders("A");

            foreach (var role in roles)
                column = createRole(role, column, Colors[index++]);

            ws.Rows[5].RowHeight = 70;
            ws.Rows[6].RowHeight = 55;
   
            var row = 7;
            foreach (var employee in employees)
                CreateEmployee(employee, row++, abilityToSkillMap);

            FillInColumn(7, row-1);
        }

        private void FillInColumn(int startRow, int endRow) {
            double plainColor = ws.Range["A1"].Interior.Color;
            double fillColor = ws.Range["A6"].Interior.Color;
            var column = "A";
            while (fillColor != plainColor)
            {
                var range = $"{column}{startRow}:{column}{endRow}";
                ws.Range[range].Interior.Color = ToColor(fillColor);
                ws.Range[range].Cells.Borders.Weight = XlBorderWeight.xlThin;
                column = IncrimentColumn(column);
                fillColor = ws.Range[$"{column}6"].Interior.Color;
            } 
        }

        private Color ToColor(double value) {

            int colorNumber = System.Convert.ToInt32(value);
            return System.Drawing.ColorTranslator.FromOle(colorNumber);
        }

        private void CreateEmployee(IEmployee employee, int row, IDictionary<string, string> abilityToSkillMap)
        {
            var employeeProperties = typeof(IEmployee).GetProperties().Where(p => p.PropertyType != typeof(IList<IAbility>));
            var column = "A";
            foreach (PropertyInfo prop in employeeProperties)
            {
                ws.Range[$"{column}{row}"].Value = prop.GetValue(employee);
                column = IncrimentColumn(column);
            }

            foreach (var ability in employee.Abilities)
            {
                var abilityKey = $"{ability.Category}:{ability.Name}";
                if (abilityToSkillMap.ContainsKey(abilityKey))
                {
                    var roleKey = abilityToSkillMap[abilityKey];
                    if (roleToColumnMapping.ContainsKey(roleKey))
                    {
                        var subroleColumn = roleToColumnMapping[roleKey];
                        ws.Range[$"{subroleColumn}{row}"].Value = GetSkillLevel((Ability)ability);
                    }
                }
            }

        }

        private int GetSkillLevel(Ability ability)
        {
            var index = -1;

            if (ability.Category == "Activities")
            {
                index = Array.IndexOf(Activities, ability.Level);
                return index == -1 ? 0 : (int)Math.Ceiling((index / +1) / 2.0);
            }

            if (quatitative.Contains(ability.Category))
            {
                index = Array.IndexOf(Products, ability.Level) + 1;
                return index == 5 ? 4 : index;
            }

            return 0;
        }

        void CreateTitle()
        {
            ws.Range["A1:B2"].Merge();
            ws.Range["A1"].Value = "Skill Matrix Version 1.1";
        }

        void createInstructions()
        {
            ws.Range["A3"].Value = "Please rate the employee from 1= follow/assist, 2=apply/enable, 3=ensure/initiate, 4=set strategy. If additional, important skills exist, please describe it in detail.";
            ws.Range["A3"].Font.Color = red;
        }

        void createRole(string title, string range, Color color, bool bold = false, bool verticalAlign = false)
        {
            ws.Range[range].Merge();
            ws.Range[range].Value = title;
            ws.Range[range].Orientation = 90;
            ws.Range[range].HorizontalAlignment = XlHAlign.xlHAlignCenter;
            ws.Range[range].Interior.Color = color;
            ws.Range[range].Cells.WrapText = true;
            ws.Range[range].Font.Size = 11;

            if (bold)
                ws.Range[range].Font.Bold = true;
            if (verticalAlign)
                ws.Range[range].VerticalAlignment = XlVAlign.xlVAlignTop;

        }
        string createRole(IRole role, string column, Color color)
        {
            var startColumn = column;
            var lastColumn = column;
            var nextColumn = column;

            foreach (var sr in role.SubRoles)
            {
                lastColumn = nextColumn;
                createRole(sr.Category, $"{nextColumn}6", color);
                WrapBottomInBorder($"{nextColumn}6");
                roleToColumnMapping.Add($"{role.Category}:{sr.Category}", nextColumn);
                nextColumn = IncrimentColumn(lastColumn);
            }

            createRole(role.Category, $"{startColumn}5:{lastColumn}5", color, true, true);
            WrapTopInBorder($"{startColumn}5:{lastColumn}5");
            return nextColumn;
        }

        public string IncrimentColumn(string column)
        {
            var stack = new Stack<char>(column.ToUpper().ToCharArray());
            var last = stack.Pop();

            if (last == 'Z')
            {
                if (stack.Count == 0)
                {
                    stack.Push('A');
                }
                else
                {
                    char secondLast = stack.Pop();
                    stack.Push(++secondLast);
                }
                stack.Push('A');
            }
            else
            {
                stack.Push(++last);
            }
            return new string(stack.Reverse().ToArray());
        }

        string CreateEmployeeHeaders(string column)
        {
            var headers = typeof(IEmployee).GetProperties().Where(p => p.PropertyType != typeof(IList<IAbility>));

            foreach (var header in headers)
                column = createHeader(header.Name, column);
            return column;
        }

        private string createHeader(string title, string column)
        {
            ws.Range[$"{column}5"].Interior.Color = lightBlue;
            WrapTopInBorder($"{column}5");

            var range = $"{column}6";
            WrapBottomInBorder(range);
            
            ws.Range[range].VerticalAlignment = XlVAlign.xlVAlignTop;
            ws.Range[range].Interior.Color = lightBlue;
            ws.Range[range].Value = title;
            ws.Range[range].Font.Bold = true;

            return IncrimentColumn(column);
        }

        private void WrapTopInBorder(string range)
        {
            ws.Range[range].Borders[XlBordersIndex.xlEdgeTop].Weight = XlBorderWeight.xlThin;
            ws.Range[range].Borders[XlBordersIndex.xlEdgeLeft].Weight = XlBorderWeight.xlThin;
            ws.Range[range].Borders[XlBordersIndex.xlEdgeRight].Weight = XlBorderWeight.xlThin;
        }

        private void WrapBottomInBorder(string range)
        {
            ws.Range[range].Borders[XlBordersIndex.xlEdgeBottom].Weight = XlBorderWeight.xlThin;
            ws.Range[range].Borders[XlBordersIndex.xlEdgeLeft].Weight = XlBorderWeight.xlThin;
            ws.Range[range].Borders[XlBordersIndex.xlEdgeRight].Weight = XlBorderWeight.xlThin;
        }


        public void Dispose()
        {
            //wb.SaveAs("C:\\Temp\\SkillMatrix_V11.xlsx");
        }
    }
}
