using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace SalesAndInventorySystem.Models
{

    public class EntityModel : PropertyChangedBase
    {
        public void Notify([CallerMemberName] string source = "")
        {
            NotifyOfPropertyChange(source);
        }

        public void NotifyAll()
        {
            NotifyOfPropertyChange("");
        }

        public Guid Id { get; set; }

        public bool Deleted { get; set; }

        public DateTime DateCreated { get; set; } = DateTime.Now;

        [NotMapped]
        public override bool IsNotifying { get => base.IsNotifying; set => base.IsNotifying = value; }

    }

    public class TransactionEntityModel : EntityModel
    {
        public void Post()
        {
            IsPosted = true;
            DatePosted = DateTime.Now;
            NotifyAll();
        }

        public void Unpost()
        {
            IsPosted = false;
            DatePosted = null;
            NotifyAll();
        }

        [MaxLength(100)]
        public virtual string TransactionId { get; set; }

        public virtual DateTime DateTime { get; set; }

        public bool IsPosted { get; set; }

        public DateTime? DatePosted { get; set; }
    }

    public class ValidatingEntityModel : EntityModel, IDataErrorInfo
    {
        [NotMapped]
        string IDataErrorInfo.Error
        {
            get
            {
                throw new NotSupportedException("IDataErrorInfo.Error is not supported, use IDataErrorInfo.this[propertyName] instead.");
            }
        }
        string IDataErrorInfo.this[string propertyName]
        {
            get
            {
                if (string.IsNullOrEmpty(propertyName))
                {
                    throw new ArgumentException("Invalid property name", propertyName);
                }
                string error = string.Empty;
                var value = GetValue(propertyName);
                var results = new List<System.ComponentModel.DataAnnotations.ValidationResult>(1);
                var result = Validator.TryValidateProperty(
                    value,
                    new ValidationContext(this, null, null)
                    {
                        MemberName = propertyName
                    },
                    results);
                if (!result)
                {
                    var validationResult = results.First();
                    error = validationResult.ErrorMessage;
                }
                return error;
            }
        }
        private object GetValue(string propertyName)
        {
            PropertyInfo propInfo = GetType().GetProperty(propertyName);
            return propInfo.GetValue(this);
        }
    }
}
