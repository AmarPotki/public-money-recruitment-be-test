using System;
using System.Collections.Generic;
using System.Linq;

namespace Framework.Domain
{
    public abstract class Enumeration : object, IComparable
    {
        #region Static Member(s)
        public static IEnumerable<T> GetAll<T>() where T : Enumeration
        {
            var result1 = typeof(T);

            var result2 =
                result1.GetFields
                (System.Reflection.BindingFlags.Public |
                System.Reflection.BindingFlags.Static |
                System.Reflection.BindingFlags.DeclaredOnly);

            var result3 =
                result2
                .Where(current => current.IsLiteral == false)
                .ToList();

            var result4 =
                result3
                .Select(current => current.GetValue(null));

            var result5 =
                result4.Cast<T>();

            return result5;
        }

        private static bool TryParse<TEnumeration>
            (Func<TEnumeration, bool> predicate,
            out TEnumeration enumeration) where TEnumeration : Enumeration
        {
            enumeration =
                GetAll<TEnumeration>()
                .FirstOrDefault(predicate);

            return enumeration != null;
        }

        private static TEnumeration ParseByPredicate<TEnumeration>
            (Func<TEnumeration, bool> predicate) where TEnumeration : Enumeration
        {
            var matchingItem =
                GetAll<TEnumeration>()
                .FirstOrDefault(predicate);

            return matchingItem;
        }

        /// <summary>
        /// Gets the instance of <typeparamref name="TEnumeration"/> from value or name.
        /// </summary>
        /// <typeparam name="TEnumeration">The type that inherits <see cref="Enumeration"/>.</typeparam>
        /// <param name="valueOrName">The <typeparamref name="TEnumeration"/> value or name.</param>
        /// <param name="enumeration">The <typeparamref name="TEnumeration"/> instance.</param>
        /// <returns>
        /// <c>true</c> if the instance <see cref="Enumeration"/> contains <typeparamref name="TEnumeration"/> with the specified name;
        /// otherwise, <c>false</c>.
        /// </returns>
        public static bool TryGetFromValueOrName<TEnumeration>
            (string valueOrName, out TEnumeration enumeration) where TEnumeration : Enumeration
        {
            return TryParse
                (item => item.Name == valueOrName, out enumeration) ||
                int.TryParse(valueOrName, out var value) &&
                TryParse(item => item.Id == value, out enumeration);
        }

        /// <summary>
        /// Gets the instance of <typeparamref name="TEnumeration"/> from value.
        /// </summary>
        /// <typeparam name="TEnumeration">The type that inherits <see cref="Enumeration"/>.</typeparam>
        /// <returns>The <typeparamref name="TEnumeration"/> instance.</returns>
        /// <exception cref="InvalidOperationException">"<paramref name="value"/> is not valid"</exception> 
        public static TEnumeration
            FromValue<TEnumeration>(int value) where TEnumeration : Enumeration
        {
            //var matchingItem =
            //	Parse<TEnumeration, int>(value, "nameOrValue", item => item.Value == value);

            var matchingItem =
                ParseByPredicate<TEnumeration>(item => item.Id == value);

            return matchingItem;
        }
        #endregion /Static Member(s)

        protected Enumeration() : base()
        {
        }

        protected Enumeration(int id, string name) : base()
        {
            Id = id;
            Name = name;
        }

        public int Id { get; }

        public string Name { get; }

        public override string ToString()
        {
            return Name;
        }

        public override bool Equals(object anotherObject)
        {
            if (anotherObject is null)
            {
                return false;
            }

            if (!(anotherObject is Enumeration))
            {
                return false;
            }

            if (ReferenceEquals(this, anotherObject))
            {
                return true;
            }

            var anotherEnumeration = anotherObject as Enumeration;

            // For EF Core!
            if (GetRealType() != anotherEnumeration.GetRealType())
            {
                return false;
            }

            if (GetType() == anotherEnumeration.GetType())
            {
                return Id == anotherEnumeration.Id;
            }

            return false;
        }

        public int CompareTo(object otherObject)
        {
            int result =
                Id.CompareTo((otherObject as Enumeration).Id);

            return result;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public static bool operator ==(Enumeration left, Enumeration right)
        {
            if (left is null)
            {
                return right is null;
            }

            return left.Equals(right);
        }

        public static bool operator !=(Enumeration left, Enumeration right)
        {
            return !(left == right);
        }

        public static bool operator <(Enumeration left, Enumeration right)
        {
            return left is null ? right != null : left.CompareTo(right) < 0;
        }

        public static bool operator <=(Enumeration left, Enumeration right)
        {
            return left is null || left.CompareTo(right) <= 0;
        }

        public static bool operator >(Enumeration left, Enumeration right)
        {
            return left != null && left.CompareTo(right) > 0;
        }

        public static bool operator >=(Enumeration left, Enumeration right)
        {
            return left is null ? right is null : left.CompareTo(right) >= 0;
        }

        /// <summary>
        /// For EF Core!
        /// </summary>
        private Type GetRealType()
        {
            Type type = GetType();

            if (type.ToString().Contains("Castle.Proxies."))
            {
                return type.BaseType;
            }

            return type;
        }
    }
}