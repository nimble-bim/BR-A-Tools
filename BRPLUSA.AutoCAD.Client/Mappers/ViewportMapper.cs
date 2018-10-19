using System.Linq;
using Autodesk.AutoCAD.DatabaseServices;
using BRPLUSA.Client.AutoCAD.Wrappers;

namespace BRPLUSA.Client.AutoCAD.Mappers
{
    public class ViewportMapper : IMapper<Viewport, ACADViewport>
    {
        public ACADViewport Map(Viewport item)
        {
            var layers = item.GetFrozenLayers().Cast<ObjectId>().Select(o => o.ToString());

            return new ACADViewport
            {
                StandardScale = (int) item.StandardScale,
                CustomScale = item.CustomScale,
                CenterPoint = new ACADCoordinate(item.CenterPoint.X, item.CenterPoint.Y, item.CenterPoint.Z),
                ViewCenter = new ACADCoordinate(item.ViewCenter.X, item.ViewCenter.Y),
                ViewTarget = new ACADCoordinate(item.ViewTarget.X, item.ViewTarget.Y, item.ViewTarget.Z),
                Width = item.Width,
                Height = item.Height,
                ViewHeight = item.ViewHeight,
                FrozenLayers = layers // might need some work
            };
        }
    }
}