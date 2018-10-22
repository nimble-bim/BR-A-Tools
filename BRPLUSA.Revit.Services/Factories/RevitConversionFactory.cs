using RevitElement = Autodesk.Revit.DB.Element;

namespace BRPLUSA.Revit.Services.Factories
{
    public abstract class RevitConversionFactory<T, T1> : BaseConversionFactory<T, T1>  where T: Entity where T1 : RevitElement {  }
}