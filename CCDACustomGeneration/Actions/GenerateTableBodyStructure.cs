using HL7SDK.Cda;
using System;
using System.Collections;

namespace CreateClinicalReport.Actions
{
    public class GenerateTableBodyStructure
    {
        private IStrucDocTh th;
        private IStrucDocTd td;

        public void CreateTableHeader(ArrayList Data, Factory hl7factory, IStrucDocTable tble, IStrucDocThead thead, IStrucDocTr tr)
        {
            tble.Border = "1";
            tble.Width = "100%";
            for (int i = 0; i <= Data.Count - 1; i++)
            {
                th = hl7factory.CreateStrucDocTh();
                th.Items.Add(Data[i]);
                tr.Items.Add(th);
            }
            
        }
        public void CreateTableBody(ArrayList Data, Factory hl7factory, IStrucDocTable tble, IStrucDocTbody tbody, IStrucDocTr tr)
        {
            //tbody = tble.Tbody.Append();
            tr = hl7factory.CreateStrucDocTr();
            for (int n = 0; n <= Data.Count - 1; n++)
            {
                td = hl7factory.CreateStrucDocTd();
                td.Items.Add(Data[n]);
                tr.Items.Add(td);
            }
            tbody.Tr.Add(tr);
        }

        public void CreateTableBody1(string colSpan, Factory hl7factory, IStrucDocTable tble, IStrucDocTbody tbody, IStrucDocTr tr)
        {
            tr = hl7factory.CreateStrucDocTr();
            td = hl7factory.CreateStrucDocTd();
            td.Colspan = colSpan;
            td.Align = StrucDocTdAlign.center;
            td.Items.Add("N/A");
            tr.Items.Add(td);
            tbody.Tr.Add(tr);
        }
    }
}
