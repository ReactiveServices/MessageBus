using System;

namespace ReactiveServices.Extensions
{
    public static class ObjectExtensions
    {
        public static void ShouldNotBeNull(this object obj) 
        {
            if (obj == null) throw new ArgumentNullException();
        }
    }
}
