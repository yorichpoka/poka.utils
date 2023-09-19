namespace POKA.Utils
{
    public abstract class ChangeTracker : INotifyPropertyChanged, IRevertibleChangeTracking
    {
        [NotMapped]
        protected Dictionary<string, ITrackedChange> Tracked { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
        public bool IsTracking { get { return Tracked.Count > 0; } }
        private bool _isChanged = false;

        /// <summary>
        /// Gets the objects changed status
        /// </summary>
        /// <returns><see langword="true"/> if the objects content has changed since either <see cref="BeginChanges"/>
        /// was called, or since <see cref="AcceptChanges"/> was last called; <see langword="false"/> otherwise</returns>
        [NotMapped]
        public bool IsChanged
        {
            get
            {
                if (!_isChanged && IsTracking)
                {
                    var trackedChanges = GetTrackedChanges();
                    if (trackedChanges.Any(x => x.DetectChange(GetValue(x.Property))))
                    {
                        _isChanged = true;
                    }
                }
                return _isChanged;
            }
            protected set
            {
                _isChanged = value;
            }
        }

        protected ChangeTracker()
        {
            Tracked = new Dictionary<string, ITrackedChange>();
        }

        /// <summary>
        /// Fires the PropertyChanged event notifying that the specified property value changed
        /// </summary>
        /// <param name="currentValue">The current value</param>
        /// <param name="originalValue">The original value</param>
        /// <param name="propertyName">The corresponding property name</param>
        protected void NotifyPropertyChanged(object currentValue, object originalValue, [CallerMemberName] string propertyName = "")
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedTrackedEventArgs(propertyName, originalValue, currentValue));
        }

        /// <summary>
        /// Fires the PropertyChanged event notifying that the specified property value changed
        /// </summary>
        /// <param name="currentValue">The current value</param>
        /// <param name="propertyName">The corresponding property name</param>
        protected virtual void NotifyPropertyChanged(object currentValue, [CallerMemberName] string propertyName = "")
        {
            NotifyPropertyChanged(currentValue, GetTrackedChange(propertyName)?.OriginalValue, propertyName);
        }

        /// <summary>
        /// Starts the tracking operation on the object. Changes made to the
        /// object will start being tracked the moment this function is called
        /// </summary>
        /// <exception cref="InvalidOperationException">Changes have already begun</exception>
        public virtual void BeginChanges()
        {
            ThrowIfTrackingStarted();
            Track(GetTrackableProperties());
            IsChanged = false;
        }

        /// <summary>
        /// Ends the tracking operation on the object. Any pending tracked changes made to the
        /// object since either <see cref="BeginChanges"/> was called, or since <see cref="AcceptChanges"/>
        /// was last called will be lost the moment this function is called
        /// </summary>
        /// <exception cref="InvalidOperationException">Change tracking has not started</exception>
        public virtual void EndChanges()
        {
            ThrowIfTrackingNotStarted();
            Tracked.Clear();
            IsChanged = false;
        }

        /// <summary>
        /// Sets the current object state as its default state by accepting the modifications.
        /// Commits all the changes made to this object since either <see cref="BeginChanges"/>
        /// was called, or since <see cref="AcceptChanges"/> was last called
        /// </summary>
        /// <exception cref="InvalidOperationException">Change tracking has not started</exception>
        public virtual void AcceptChanges()
        {
            ThrowIfTrackingNotStarted();
            var trackedChanges = UpdateTracked();
            foreach (var change in trackedChanges)
            {
                change.AcceptChange();
            }
            IsChanged = false;
        }

        /// <summary>
        /// Resets the current objects state by rejecting the modifications. Rejects
        /// all changes made to the object since either <see cref="BeginChanges"/>
        /// was called, or since <see cref="AcceptChanges"/> was last called
        /// </summary>
        /// <exception cref="InvalidOperationException">Change tracking has not started</exception>
        public virtual void RejectChanges()
        {
            ThrowIfTrackingNotStarted();
            var trackedChanges = GetTrackedChanges();
            foreach (var change in trackedChanges)
            {
                SetValue(change.Property, change.OriginalValue);
                change.RejectChange();
            }
            IsChanged = false;
        }

        /// <summary>
        /// Returns a list containing information of all the properties with changes
        /// applied to it since either <see cref="BeginChanges"/> was called, or
        /// since <see cref="AcceptChanges"/> was last called
        /// </summary>
        /// <returns>A list containing the changes made to the object</returns>
        /// <exception cref="InvalidOperationException">Change tracking has not started</exception>
        public virtual List<ITrackedChange> GetChanges()
        {
            ThrowIfTrackingNotStarted();
            var result = new List<ITrackedChange>();
            var trackedChanges = UpdateTracked();
            foreach (var change in trackedChanges)
            {
                if (change.HasChanges)
                {
                    result.Add(change);
                }
            }
            return result;
        }

        /// <summary>
        /// Returns a list containing information of all the properties being tracked
        /// </summary>
        /// <returns>A list containing property tracking information</returns>
        /// <exception cref="InvalidOperationException">Change tracking has not started</exception>
        public List<ITrackedChange> GetTracked()
        {
            ThrowIfTrackingNotStarted();
            return Tracked.Values.ToList();
        }

        /// <summary>
        /// Updates the tracking status of all the properties being tracked
        /// </summary>
        /// <returns>A list containing properties being tracked</returns>
        protected IEnumerable<TrackedChange> UpdateTracked()
        {
            var trackedChanges = GetTrackedChanges();
            Track(trackedChanges.Select(x => x.Property));
            return trackedChanges;
        }

        /// <summary>
        /// Keeps track of the original and current values of the provided properties
        /// </summary>
        /// <param name="properties">The properties to track</param>
        protected void Track(IEnumerable<PropertyInfo> properties)
        {
            if (properties != null)
            {
                foreach (var prop in properties)
                {
                    Track(prop, GetValue(prop));
                }
            }
        }

        /// <summary>
        /// Keeps track of the original and current value of the provided property
        /// </summary>
        /// <param name="property">The property to track</param>
        /// <param name="currentValue">The current value of the property</param>
        protected void Track(PropertyInfo property, object currentValue)
        {
            TrackedChange currentChange = null;
            if (!IsTrackedChange(property.Name))
            {
                Func<object, object, bool> hasChangedFunc = (original, current) => {
                    return HasValuesChanged(original, current);
                };
                currentChange = new TrackedChange
                {
                    Property = property,
                    OriginalValue = currentValue,
                    HasChangedFunc = hasChangedFunc
                };
                Tracked.Add(property.Name, currentChange);
            }
            else
            {
                currentChange = GetTrackedChange(property.Name);
                currentChange.CurrentValue = currentValue;
                currentChange.Status = TrackedChange.TrackingStatus.Checked;
                currentChange.LastChecked = DateTime.Now;
            }
            if (currentChange.HasChanges)
            {
                IsChanged = true;
            }
        }

        /// <summary>
        /// Class which contains property tracking information
        /// </summary>
        protected class TrackedChange : ITrackedChange
        {
            public enum TrackingStatus
            {
                Unchecked,
                Checked
            }
            public PropertyInfo Property { get; set; }
            public string PropertyName { get { return Property?.Name; } }
            public object OriginalValue { get; set; }
            public object CurrentValue { get; set; }
            public TrackingStatus Status { get; set; } = TrackingStatus.Unchecked;
            public bool HasChanges
            {
                get
                {
                    return Status == TrackingStatus.Checked && DetectChange(CurrentValue);
                }
            }
            public DateTime? LastChecked { get; set; } = null;
            public Func<object, object, bool> HasChangedFunc { get; set; }
            public bool DetectChange(object currentValue)
            {
                return HasChangedFunc(OriginalValue, currentValue);
            }
            public void AcceptChange()
            {
                OriginalValue = CurrentValue;
                RejectChange();
            }
            public void RejectChange()
            {
                CurrentValue = null;
                Status = TrackingStatus.Unchecked;
                LastChecked = null;
            }
        }

        /// <summary>
        /// Provides support for object change tracking
        /// </summary>
        public interface ITrackedChange
        {
            string PropertyName { get; }
            object OriginalValue { get; }
            object CurrentValue { get; }
        }

        /// <summary>
        /// Provides data for the <see cref=" PropertyChangedEventArgs"/> event
        /// </summary>
        public class PropertyChangedTrackedEventArgs : PropertyChangedEventArgs
        {
            public object OriginalValue { get; private set; }
            public object CurrentValue { get; private set; }
            public PropertyChangedTrackedEventArgs(string propertyName, object originalValue, object currentValue)
                   : base(propertyName)
            {
                OriginalValue = originalValue;
                CurrentValue = currentValue;
            }
        }

        /// <summary>
        /// Attribute which allows a property to be ignored from being tracked
        /// </summary>
        public class ChangeTrackerIgnoreAttribute : Attribute { }

        #region "Helpers"
        protected bool IsTrackedChange(string propertyName)
        {
            return Tracked.ContainsKey(propertyName);
        }
        protected TrackedChange GetTrackedChange(string propertyName)
        {
            return IsTrackedChange(propertyName) ? (TrackedChange)Tracked[propertyName] : null;
        }
        protected IEnumerable<TrackedChange> GetTrackedChanges()
        {
            return Tracked.Select(pair => (TrackedChange)pair.Value);
        }
        protected virtual IEnumerable<PropertyInfo> GetTrackableProperties()
        {
            return this.GetType().GetProperties()
                    .Where(p =>
                        !p.DeclaringType.Equals(typeof(ChangeTracker))
                        && p.CanRead
                        //&& p.CanWrite
                        && !p.GetCustomAttributes<ChangeTrackerIgnoreAttribute>(false).Any()
                    );
        }
        protected virtual object GetValue(PropertyInfo property)
        {
            return property?.GetValue(this, property.GetIndexParameters()
                .Count() == 1 ? new object[] { null } : null);
        }
        protected virtual void SetValue(PropertyInfo property, object value)
        {
            if (property != null && property.CanWrite)
            {
                property.SetValue(this, value);
            }
        }
        protected virtual bool HasValuesChanged<T>(T originalValue, T currentValue)
        {
            return !EqualityComparer<T>.Default.Equals(originalValue, currentValue);
        }
        protected void ThrowIfTrackingNotStarted()
        {
            if (!IsTracking)
            {
                throw new InvalidOperationException("Change tracking has not started. Call 'BeginChanges' to start change tracking");
            }
        }
        protected void ThrowIfTrackingStarted()
        {
            if (IsTracking)
            {
                throw new InvalidOperationException("Change tracking has already started");
            }
        }
        #endregion
    }
}
