using System.ComponentModel;
using System.Resources;

namespace ZemotoCommon;

/// <summary>
/// Allows use of localized strings in a DescriptionAttribute.
/// </summary>
/// <param name="resourceKey">Key of the resource in string form</param>
/// <param name="resourceType">The type of the class containing the resources</param>
internal sealed class LocalizedDescriptionAttribute( string resourceKey, Type resourceType ) : DescriptionAttribute
{
   private readonly ResourceManager _resourceManager = new( resourceType );

   public override string Description
   {
      get
      {
         string? description = _resourceManager.GetString( resourceKey );
         return string.IsNullOrEmpty( description ) ? resourceKey : description;
      }
   }
}