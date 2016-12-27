using Orchard.UI.Resources;

namespace Webionic.LinkList {
    public class ResourceManifest : IResourceManifestProvider {
        public void BuildManifests(ResourceManifestBuilder builder) {
            var manifest = builder.Add();
            
            manifest.DefineStyle("Webionic.LinkList").SetUrl("Webionic.LinkList.min.css", "Webionic.LinkList.css");
        }
    }
}
