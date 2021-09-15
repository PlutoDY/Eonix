using System;

namespace Eonix.Util
{

    public interface IPoolableObject
    {
        bool CanRecyle { get; set; }

        Action OnRecyleStart { get; set; }
    }

}
