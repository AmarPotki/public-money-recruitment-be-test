﻿using System.Collections.Generic;
using System.Linq;

namespace Framework.Domain
{
    public abstract class ValueObject
    {
        #region Static Member(s)
        public static bool operator ==(ValueObject leftObject, ValueObject rightObject)
        {
            if (leftObject is null && rightObject is null)
            {
                return true;
            }

            if (leftObject is null && rightObject !=null)
            {
                return false;
            }

            if (leftObject != null && rightObject is null)
            {
                return false;
            }

            return leftObject.Equals(rightObject);
        }

        public static bool operator !=(ValueObject leftObject, ValueObject rightObject)
        {
            return !(leftObject == rightObject);
        }
        #endregion /Static Member(s)

        protected ValueObject() : base()
        {
        }

        protected abstract IEnumerable<object> GetEqualityComponents();

        public override bool Equals(object anotherObject)
        {
            if (anotherObject is null)
            {
                return false;
            }

            if (GetType() != anotherObject.GetType())
            {
                return false;
            }

            var stronglyTypedOtherObject =
                anotherObject as ValueObject;

            if (stronglyTypedOtherObject is null)
            {
                return false;
            }

            bool result =
                GetEqualityComponents()
                    .SequenceEqual(stronglyTypedOtherObject.GetEqualityComponents());

            return result;
        }

        public override int GetHashCode()
        {
            var result =
                GetEqualityComponents()
                    .Select(x => x != null ? x.GetHashCode() : 0)
                    .Aggregate((x, y) => x ^ y);

            return result;
        }
    }
}