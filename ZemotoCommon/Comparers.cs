namespace ZemotoCommon;

internal sealed class ReferenceEqualityComparer : EqualityComparer<object>
{
   public override bool Equals( object? x, object? y ) => ReferenceEquals( x, y );

   public override int GetHashCode( object obj ) => obj == null ? 0 : obj.GetHashCode();
}
