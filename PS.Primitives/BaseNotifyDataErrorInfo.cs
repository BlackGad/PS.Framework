using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace PS
{
    public abstract class BaseNotifyDataErrorInfo : BaseNotifyPropertyChanged,
                                                    INotifyDataErrorInfo
    {
        protected List<ValidationResult> CurrentValidationResults;

        protected BaseNotifyDataErrorInfo()
        {
            CurrentValidationResults = new List<ValidationResult>();
            Validate();
        }

        protected override void OnPropertyChanged(string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
            Validate();
        }

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public virtual bool HasErrors
        {
            get { return CurrentValidationResults.Any(); }
        }

        public IEnumerable GetErrors(string propertyName)
        {
            var errorMessages = CurrentValidationResults
                                .Where(x => x.MemberNames.Contains(propertyName))
                                .Select(x => x.ErrorMessage)
                                .ToList();

            return errorMessages;
        }

        protected virtual IEnumerable<ValidationResult> CustomValidation()
        {
            yield break;
        }

        protected virtual void OnErrorsChanged(DataErrorsChangedEventArgs e)
        {
            ErrorsChanged?.Invoke(this, e);
        }

        protected void Validate()
        {
            var validationResults = new List<ValidationResult>();
            Validator.TryValidateObject(this, new ValidationContext(this), validationResults, true);

            validationResults.AddRange(CustomValidation());

            var validationChangedPropertyNames = validationResults
                                                 .SelectMany(x => x.MemberNames)
                                                 .Union(CurrentValidationResults.SelectMany(x => x.MemberNames));

            CurrentValidationResults = validationResults;

            foreach (var validationChangedPropertyName in validationChangedPropertyNames)
            {
                OnErrorsChanged(new DataErrorsChangedEventArgs(validationChangedPropertyName));
            }
        }
    }
}
