﻿using System.Collections.Generic;

namespace ZemotoCommon;

internal sealed class ReferenceEqualityComparer : EqualityComparer<object>
{
   public override bool Equals( object x, object y )
   {
      return ReferenceEquals( x, y );
   }

   public override int GetHashCode( object obj )
   {
      if ( obj == null ) return 0;
      return obj.GetHashCode();
   }
}
