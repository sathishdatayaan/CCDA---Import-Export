using CreateClinicalReport.Model;
using HL7SDK.Cda;
using System;

namespace CreateClinicalReport.Actions
{
    public class GenerateHeader 
    {
        CCDAHeader header;
        RealmCode realmCode_code;
        TypeId typeId;
        TemplateId templateId;
        Id id;
        Code code;
        EffectiveTime effectiveTime;
        ConfidentialityCode confidentialityCode;
        LanguageCode languageCode;
        SetId setId;
        VersionNumber versionNumber;
        public string BindHeader(string title, ClinicalDocument clinicalDoc, Factory hl7factory, ICS realmCode)
        {
            string docheader = String.Empty;
           
            header = new CCDAHeader();
            realmCode_code = new RealmCode();
            typeId = new TypeId();
            templateId = new TemplateId();
            id = new Id();
            code = new Code();
            effectiveTime = new EffectiveTime();
            confidentialityCode = new ConfidentialityCode();
            languageCode = new LanguageCode();
            setId = new SetId();
            versionNumber = new VersionNumber();
            header.Id = id;
            header.languageCode = languageCode;
            header.realmCode = realmCode_code;
            header.code = code;
            header.effectiveTime = effectiveTime;
            header.confidentialityCode = confidentialityCode;
            header.setId = setId;
            header.versionNumber = versionNumber;
            header.TemplateId = templateId;
            header.typeId = typeId;
            header.title = title;
            realmCode = hl7factory.CreateCS();
            realmCode = hl7factory.CreateCS();
            realmCode.Code = header.realmCode.code;
            clinicalDoc.RealmCode.Add(realmCode);
            // Manage Clinical Report Header
            //Manage TypeId
            clinicalDoc.TypeId.Root = header.typeId.root;
            clinicalDoc.TypeId.Extension = header.typeId.extension;
            //END
            //Manage template Id
            clinicalDoc.TemplateId.Append().Root = header.TemplateId.root;
            //END
            //Manage Header Id
            clinicalDoc.Id.Init(header.Id.root, header.Id.extension);
            //END
            //Manage Header Code
            clinicalDoc.Code.Code = header.code.code;
            clinicalDoc.Code.CodeSystemName = header.code.codeSystemName;
            clinicalDoc.Code.DisplayName = header.code.displayName;
            clinicalDoc.Code.CodeSystem = header.code.codeSystem;
            //END
            //Manage Header Title
            clinicalDoc.Title.Text = header.title;
            //END
            //Manage Header Effective Date
            clinicalDoc.EffectiveTime.AsDateTime = Convert.ToDateTime(header.effectiveTime.value);
            //END
            //Manage Header Confidencial Code
            clinicalDoc.ConfidentialityCode.Code = header.confidentialityCode.code;
            clinicalDoc.ConfidentialityCode.CodeSystem = header.confidentialityCode.codeSystem;
            //END
            //Manage Header Language Code
            clinicalDoc.LanguageCode.Code = header.languageCode.code;
            //END
            //Manage Header SetId 
            clinicalDoc.SetId.Root = header.setId.root;
            clinicalDoc.SetId.Extension = header.setId.extension;
            //END
            //Manage Header Version 
            clinicalDoc.VersionNumber.Value = header.versionNumber.value;
            //END
            //END
            //docheader = "<?xml version='1.0' encoding='UTF-8'?><?xml-stylesheet type='text/xsl' href='CDA.xsl'?>"+clinicalDoc.Xml;
            docheader =  clinicalDoc.Xml;

            return docheader.ToString();

        }
    }
}
