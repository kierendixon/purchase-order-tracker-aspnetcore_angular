﻿namespace PurchaseOrderTracker.Domain.Models
{
    // https://github.com/dotnet-architecture/eShopOnContainers
    public abstract class Entity
    {
        private int? _requestedHashCode;

        public virtual int Id { get; protected set; }

        public bool IsTransient()
        {
            return Id == default(int);
        }

        public override bool Equals(object obj)
        {
            if (obj is null or not Entity)
                return false;

            if (ReferenceEquals(this, obj))
                return true;

            if (GetType() != obj.GetType())
                return false;

            var item = (Entity)obj;

            if (item.IsTransient() || IsTransient())
                return false;
            return item.Id == Id;
        }

        public override int GetHashCode()
        {
            if (!IsTransient())
            {
                if (!_requestedHashCode.HasValue)
                    _requestedHashCode = Id.GetHashCode() ^ 31;

                return _requestedHashCode.Value;
            }

            return base.GetHashCode();
        }

        public static bool operator ==(Entity left, Entity right)
        {
            if (Equals(left, null))
                return Equals(right, null) ? true : false;
            return left.Equals(right);
        }

        public static bool operator !=(Entity left, Entity right)
        {
            return !(left == right);
        }
    }
}
