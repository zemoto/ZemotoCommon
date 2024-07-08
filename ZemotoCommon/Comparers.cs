using System.Collections.Generic;

namespace ZemotoCommon;

internal sealed class ReferenceEqualityComparer : EqualityComparer<object>
{
   public override bool Equals( object x, object y )
   {
      return ReferenceEquals( x, y );
   }

   public override int GetHashCode( object obj )
   {
      return obj == null ? 0 : obj.GetHashCode();
   }
}
