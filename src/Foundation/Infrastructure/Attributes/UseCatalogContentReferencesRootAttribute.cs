using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;

namespace Foundation.Infrastructure.Attributes
{
    /// <summary>
    /// The <see cref="IList{T}"/> selector doesn't properly work with just a UI Hint.
    /// This just results in both the catalog content root AND the normal in one window.
    /// To hide the unused normal content root we use this attribute.
    /// <see href="https://world.optimizely.com/forum/developer-forum/CMS/Thread-Container/2015/11/contentreferencelist---browsing-for-content-issue/"/>
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class UseCatalogContentReferencesRootAttribute : UIHintAttribute, IDisplayMetadataProvider
    {
        private Injected<ReferenceConverter> _referenceConverter;

        public UseCatalogContentReferencesRootAttribute() : base(EPiServer.Commerce.UIHint.CatalogContent)
        {
        }

        public void CreateDisplayMetadata(DisplayMetadataProviderContext context)
        {
            if (!(context.DisplayMetadata.AdditionalValues[(object)"epi:extendedmetadata"] is ExtendedMetadata
                    additionalValue))
            {
                return;
            }

            additionalValue.EditorConfiguration["roots"] = new[]
            {
                _referenceConverter.Service.GetRootLink().ToReferenceWithoutVersion()
            };
        }
    }
}
