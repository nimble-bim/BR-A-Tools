using System;
using System.IO;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;

namespace BRPLUSA.Revit.Services.Parameters
{
    public class RevitParameterUtility
    {
        public static DefinitionFile CreateOrGetSharedParameterFile(Application app, string fileName)
        {
            FileStream file = null;

            if(!File.Exists(fileName))
                file = File.Create(fileName);

            file?.Close();

            return AddSharedParameterFileToModel(app, fileName);
        }

        public static DefinitionFile AddSharedParameterFileToModel(Application app, string sharedParamFile)
        {
            app.SharedParametersFilename = sharedParamFile;
            var file = app.OpenSharedParameterFile();

            return file;
        }

        public static DefinitionGroup CreateOrGetGroupInSharedParameterFile(DefinitionFile file, string groupName)
        {
            DefinitionGroup group = null;

            try
            {
                group = file.Groups.get_Item(groupName);
            }

            catch(Exception e) { }

            group = group ?? file.Groups.Create(groupName);

            return group;
        }

        public static void BindParameterToCategory(Document doc, Category category, Definition parameter)
        {
            try
            {
                var categories = doc.Application.Create.NewCategorySet();
                categories.Insert(category);
                var binding = doc.Application.Create.NewInstanceBinding(categories);
                using (var tr = new Transaction(doc))
                {
                    if (!tr.HasStarted())
                        tr.Start("Inserting parameter");

                    doc.ParameterBindings.Insert(parameter, binding);

                    tr.Commit();
                }
            }

            catch (Exception e)
            {
                throw new Exception("The requested binding could not be completed", e);
            }

        }


    }
}
